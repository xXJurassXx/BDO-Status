using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Commons.Models.Account;
using Commons.Models.Character;
using Commons.Networking.Cryptography;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking
{
    public class ClientConnection
    {
        public Socket Socket { get; set; }

        public byte[] WaitPacketLen { get; set; }

        public BdoTransformer Session { get; private set; }

        public int SequenceId = 37612;

        public AccountData Account { get; set; }
        
        public List<CharacterData> Characters { get; set; }  

        public ClientConnection()
        {
            WaitPacketLen = new byte[2];
            Session = new BdoTransformer();
        }

        public void SendRaw(byte[] data)
        {
            Socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendEnd, null);
        }

        private void SendEnd(IAsyncResult ar)
        {
            Socket.EndSend(ar);
        }

        public void CloseConnection()
        {
            Socket.Close();
        }
    }
}
