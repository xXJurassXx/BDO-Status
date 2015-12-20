using System;
using System.Net.Sockets;
using Commons.Utils;
using NLog;

/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling
{
    public abstract class APacketProcessor
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void Send(ClientConnection client, bool isCrypted = true)
        {
            var body = WritedData(); //get body data from sended packet

            var packet = new byte[7 + body.Length]; //create buffer for packet
            var len = BitConverter.GetBytes((ushort)(body.Length + 7)); //get size for fully packet

            Buffer.BlockCopy(len, 0, packet, 0, 2); //copy packet size in buffer

            if (isCrypted) //if need crypt, set val
                packet[2] = 1;

            var opCode = PacketHandler.GetOpCode(GetType()); //get opcode from handler

            Buffer.BlockCopy(BitConverter.GetBytes(opCode), 0, packet, 5, 2); //copy opcode in buffer
            Buffer.BlockCopy(body, 0, packet, 7, body.Length); //copy packet body in buffer

            if (isCrypted) //packet ecnryption action
            {
                client.SequenceId++; //if we crypt packet, iterate sequence id

                var ch = BitConverter.GetBytes((short)client.SequenceId); //get sequence id bytes

                Buffer.BlockCopy(ch, 0, packet, 3, 2); //copy sequence id bytes in packet

                int templen = packet.Length - 2; //get len for crypted body (fully packet size without length)
                byte[] temp = new byte[templen]; //create temp for crypted body

                Buffer.BlockCopy(packet, 2, temp, 0, templen); //copy body datas in temp

                client.Session.Transform(ref temp); //encrypt datas in temp

                Buffer.BlockCopy(temp, 0, packet, 2, templen); //copy encrypted datas in packet
            }

            Log.Debug($"RAW\n{packet.FormatHex()}");
            client.Socket.BeginSend(packet, 0, packet.Length, SocketFlags.None, null, null); //send packet datas to client
        }

        public virtual void Process(ClientConnection client, byte[] data)
        {
            throw new Exception("Cannot handle empty packet!");
        }

        public virtual byte[] WritedData()
        {
            throw new Exception("Cannot write empty packet!");
        }
    }
}
