using System.IO;
using System.Text;
using Commons.Enums;
using Commons.Utils;

/**
* Author: InCube
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpChat : APacketProcessor
    {
        private readonly int _sessionId;
        private readonly string _text;
        private readonly string _accountName;
        private readonly ChatType _chatType;

        public SpChat(string text, int sessionId, string accountName, ChatType chatType)
        {
            _text = text;
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
                    writer.Write((byte)_chatType.GetHashCode());
                    writer.Write("0D05DCFB".ToBytes()); // TODO
                    writer.Write((byte)0x00);
                    writer.Write(BinaryExt.WriteFixedString(_accountName, Encoding.Unicode, 62));

                    // Unknown data
                    writer.Write("0001001800".ToBytes());

                    // Chat text
                    writer.WriteS(_text);
                    writer.Write("0000".ToBytes());
                }
                return stream.ToArray();
            }
        }
    }
}
