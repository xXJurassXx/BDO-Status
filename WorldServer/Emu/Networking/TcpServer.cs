using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Commons.Generics;
using Commons.Utils;
using WorldServer.Emu.Networking.Handling;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking
{
    public class TcpServer
    {
        private readonly HashSet<Thread> _listeningThreads = new HashSet<Thread>();

        private readonly Socket _listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private ObjectsPool<ClientConnection> _connectionsPool;

        private readonly string _host;

        private readonly int _port;

        private readonly int _maxConnectionsCount;

        /// <summary>
        /// Tcp server constructor
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="maxConnectionsCount"></param>
        public TcpServer(string host, int port, int maxConnectionsCount)
        {
            _host = host;
            _port = port;
            _maxConnectionsCount = maxConnectionsCount;
        }

        /// <summary>
        /// Initilize service, creating threads for socket and start listening
        /// </summary>
        public void Initialize()
        {
            _connectionsPool = new ObjectsPool<ClientConnection>(_maxConnectionsCount);

            EndPoint endPoint = new IPEndPoint(IPAddress.Parse(_host), _port);

            _listeningSocket.Bind(endPoint);
            _listeningSocket.Listen(40);

            for (int i = 0; i < 10; i++)
            {
                var th = new Thread(() => _listeningSocket.BeginAccept(AcceptCallback, null));
                th.Start();
                _listeningThreads.Add(th);
            }

            Debug.Print($"Tcp server started at {_host}:{_port}");
        }

        private readonly byte[] _key =
        {
            0x81, 0x00, 0x00, 0x00, 0x00, 0xeb, 0x03, 0x41,
            0x88, 0x92, 0x80, 0x0e, 0xf0, 0x50, 0xe1, 0x27,
            0x5b, 0x40, 0x90, 0x37, 0x30, 0x38, 0xae, 0x5e,
            0x20, 0x4b, 0xef, 0x72, 0x4a, 0x83, 0x1c, 0xc7,
            0x7c, 0x2e, 0x89, 0x50, 0xad, 0x34, 0x33, 0x03,
            0xce, 0x62, 0xdb, 0x2a, 0x87, 0x96, 0x7b, 0xc8,
            0x16, 0x23, 0x42, 0x77, 0xd7, 0x5e, 0x95, 0x5c,
            0xf0, 0xc1, 0x5f, 0xa4, 0xdf, 0x91, 0x6a, 0x4d,
            0x69, 0x05, 0x60, 0xfe, 0x10, 0x5e, 0x9a, 0xed,
            0x44, 0x5a, 0x77, 0x00, 0x91, 0xc0, 0x94, 0x00,
            0x1b, 0x8b, 0x21, 0x04, 0x61, 0x46, 0x23, 0xb0,
            0xed, 0x1b, 0x9d, 0xa4, 0xcf, 0x04, 0xec, 0x73,
            0xa4, 0x9f, 0x6f, 0xc1, 0x1b, 0xcd, 0x7e, 0xe6,
            0xfc, 0xa0, 0x73, 0x81, 0x19, 0x84, 0x43, 0x77,
            0x7d, 0xb0, 0xbd, 0x6b, 0xdf, 0x34, 0x4d, 0x46,
            0x22, 0xa1, 0x6d, 0x59, 0xec, 0x23, 0x82, 0x22,
            0xdd
        };

        /// <summary>
        /// Accept callback, start receive message body
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket sock = null;
            try
            {
                sock = _listeningSocket.EndAccept(ar);

                var connection = _connectionsPool.Get();
                if (connection == null)
                {
                    Debug.Print("Connections limit reached! Can't accept new connection");
                }
                else
                {
                    connection.Socket = sock;
                    connection.Socket.BeginSend(_key, 0, _key.Length, SocketFlags.None, null, null);
                    connection.Socket.BeginReceive(connection.WaitPacketLen, 0,
                        connection.WaitPacketLen.Length, SocketFlags.None, ReceivePacketCallback, connection);
                }
            }
            catch (Exception)
            {
                sock?.Close();
            }
            _listeningSocket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Receive message body
        /// </summary>
        /// <param name="ar"></param>
        private void ReceivePacketCallback(IAsyncResult ar)
        {
            var connection = (ClientConnection)ar.AsyncState;

            try
            {
                SocketError err;
                var readed = connection.Socket.EndReceive(ar, out err);

                if (err != SocketError.Success || readed <= 0)
                {
                    Debug.Print("Client disconnected.");

                    connection.Socket.Disconnect(false);
                    connection.Socket.Close();
                    _connectionsPool.Release(connection);

                    return;
                }

                var header = connection.WaitPacketLen;

                var len = (BitConverter.ToUInt16(header, 0)) - connection.WaitPacketLen.Length; //packet body size without len

                if (len <= 0) //if message is empty, continue
                {
                    connection.Socket.BeginReceive(connection.WaitPacketLen, 0,
                        connection.WaitPacketLen.Length, SocketFlags.None, ReceivePacketCallback, connection);
                }
                else
                {
                    var datas = new byte[len];

                    connection.Socket.AsyncReceiveFixed(datas, (state, buffer) => //receive fully datas by len
                    {
                        PacketHandler.Process(connection, datas);
                    }, null);

                    connection.Socket.BeginReceive(connection.WaitPacketLen, 0,
                        connection.WaitPacketLen.Length, SocketFlags.None, ReceivePacketCallback, connection); //Make receive sequence
                }
            }
            catch (ObjectDisposedException) { }
            catch (SocketException) { }
            catch (Exception e)
            {
                Debug.Print("Exception on packet receive. {0}", e);

                if (connection != null)
                    Disconnect(connection);
            }
        }

        public void Disconnect(ClientConnection connection)
        {
            try
            {
                connection.Socket.BeginDisconnect(false, EndDisconect, connection);
            }
            catch (Exception e)
            {
                Debug.Print("Exception occured on begin disconnect, {0}", e);
            }
        }

        private void EndDisconect(IAsyncResult ar)
        {
            var connection = (ClientConnection)ar.AsyncState;
            try
            {
                connection.Socket.EndDisconnect(ar);
            }
            catch (Exception e)
            {
                Debug.Print("Exception occured on end disconnect, {0}", e);
            }
            finally
            {
                connection.Socket.Close();
                _connectionsPool.Release(connection);

                Debug.Print("Client disconnected.");
            }
        }
    }
}
