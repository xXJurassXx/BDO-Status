using System.IO;
using System.Text;
using Commons.Enums;
using Commons.Utils;
/**
* Author: InCube, Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SMSG_Chat : APacketProcessor
    {
        private readonly int _sessionId;
        private readonly string _message;
        private readonly string _characterName;
        private readonly ChatType _chatType;

        public SMSG_Chat(string message, int sessionId, string characterName, ChatType chatType)
        {
            _message = message;
            _sessionId = sessionId;
            _characterName = characterName;
            _chatType = chatType;
        }
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)_chatType.GetHashCode());
					writer.Write((byte)1);
					writer.Write((int)_sessionId);
                    writer.Write(BinaryExt.WriteFixedString(_characterName, Encoding.Unicode, 62));
                    writer.Write((byte)1);
                    writer.Write((byte)0);
					writer.Write((byte)1);
					writer.Write(Encoding.Unicode.GetBytes(_message));
                }
                return stream.ToArray();
            }
        }
    }
}
