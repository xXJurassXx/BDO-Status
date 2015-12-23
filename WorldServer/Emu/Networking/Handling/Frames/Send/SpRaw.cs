using Commons.Utils;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SpRaw : APacketProcessor
    {
        private readonly byte[] _packetData;
        private readonly short _opCode;
        private readonly bool _isCrypted;

        public SpRaw(string hex, short opCode, bool isCrypted = true)
        {
            _packetData = hex.ToBytes();
            _opCode = opCode;
            _isCrypted = isCrypted;
        }

        public override byte[] WritedData()
        {
            return _packetData;
        }

        public void SendRaw(ClientConnection connection)
        {
            Send(connection, _isCrypted, _opCode);
        }
    }
}
