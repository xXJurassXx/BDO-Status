/*
   Author:Sagara
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Commons.Generics;
using Commons.Utils;
using LoginServer.Emu.Networking.Handling;

namespace LoginServer.Emu.Networking
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

            Debug.Print($"Network server started at {_host}:{_port}");
        }

        private readonly byte[] _key =
        {
            0x81, 0x00, 0x00, 0x00, 0x00, 0xeb, 0x03, 0xc0, 0x00, 0x1d, 0xa0, 0x02, 0xf2, 0x40, 0x32, 0xb4, 0x4e, 0x20, 0xe0, 0xf0, 0x28, 0x5b, 0x92, 0xd8, 0x4d, 0x9b, 0x51, 0x00, 0x06, 0x57, 0x85, 0xcf, 0x33, 0xed, 0x68, 0x9e, 0xe9, 0x76, 0xac, 0xc2, 0x6f, 0xe6, 0xb9, 0x1e, 0x81, 0x4b, 0x50, 0xfe, 0xe3, 0x3d, 0xeb, 0x02, 0x1d, 0x44, 0xaa, 0xb7, 0x7a, 0x72, 0x28, 0x51, 0x26, 0x51, 0xf4, 0x19, 0x0e, 0xfc, 0xec, 0x99, 0x25, 0x43, 0x00, 0x37, 0x62, 0x65, 0x72, 0x66, 0xbb, 0x4e, 0x0b, 0x28, 0x2c, 0x1c, 0x71, 0x8f, 0x30, 0x6b, 0xb2, 0x01, 0x4f, 0x30, 0x77, 0xc8, 0x40, 0x24, 0xbb, 0x33, 0x3b, 0xc8, 0xee, 0x18, 0xb7, 0x2b, 0x6c, 0x4c, 0xb8, 0x48, 0x02, 0x03, 0xb8, 0x28, 0x8c, 0xa8, 0xfc, 0xf5, 0x90, 0x1d, 0xad, 0x79, 0xb6, 0x21, 0x09, 0x97, 0x6b, 0xe5, 0xbc, 0xb8, 0xc4, 0xb6, 0x89
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
