/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_InventorySlotCount : APacketProcessor
    {
        public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((byte)18); // inventory open slots - default 18

				return stream.ToArray();
			}
		}
    }
}
