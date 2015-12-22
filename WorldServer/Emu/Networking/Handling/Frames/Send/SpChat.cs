using System.IO;
using System.Text;
using Commons.Enums;
using Commons.Utils;
/**
* Author: InCube, Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpChat : APacketProcessor
    {
        private readonly int _sessionId;
        private readonly string _message;
        private readonly string _accountName;
        private readonly ChatType _chatType;

        public SpChat(string message, int sessionId, string accountName, ChatType chatType)
        {
            _message = message;
            _sessionId = sessionId;
            _accountName = accountName;
            _chatType = chatType;
        }
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.WriteH(_chatType.GetHashCode());
                    writer.WriteD(_sessionId);
                    writer.Write(BinaryExt.WriteFixedString(_accountName, Encoding.Unicode, 62));
                    writer.WriteH(1);
                    writer.WriteH(0);
                    writer.Write(Encoding.Unicode.GetBytes(_message));
                    writer.WriteH(0);
                }
                return stream.ToArray();
            }
        }
    }
}
