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
        private readonly string _text;
        private readonly string _accountName;
        private readonly ChatType _chatType;

        public SpChat(string text, string accountName, ChatType chatType)
        {
            _text = text;
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
                    var data = new byte[]
                    {
                        0x0D, 0x05, 0xDC, 0xFB
                    };
                    writer.Write(data);
                    writer.Write((byte)0x00); // Spacer for char name

                    // Character name
                    writer.Write(BinaryExt.WriteFixedString(_accountName, Encoding.Unicode, 62));

                    // Unknown data
                    var unk = new byte[] { 0x00, 0x01, 0x00, 0x18, 0x00 };
                    writer.Write(unk);

                    // Write string
                    writer.WriteS(_text);

                    // Say that we have ended sending this packet
                    writer.Write((byte)0x00);
                    writer.Write((byte)0x00);
                }
                return stream.ToArray();
            }
        }
    }
}
