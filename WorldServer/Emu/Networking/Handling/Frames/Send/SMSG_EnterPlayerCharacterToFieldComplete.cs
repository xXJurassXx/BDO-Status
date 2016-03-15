/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_EnterPlayerCharacterToFieldComplete : APacketProcessor
    {
        public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((byte)1);
				writer.Write((byte)1);
				writer.Write((byte)1);
				writer.Write((int)1);
				writer.Write((byte)1);

				return stream.ToArray();
			}
		}
    }
}
