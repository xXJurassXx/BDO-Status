/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_SetGameTime : APacketProcessor
    {
        public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((long)1457978263);
				writer.Write((int)0);
				writer.Write((long)290759406);
				writer.Write((long)1454284800);
				writer.Write((int)0);
				writer.Write((int)0);
				writer.Write((byte)0);

				return stream.ToArray();
			}
		}
    }
}
