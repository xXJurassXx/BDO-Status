using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    internal class SpCreateCharacterError : APacketProcessor
    {
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write("2284adb1");//name already used. TODO - find other

                return stream.ToArray();
            }
        }
    }
}
