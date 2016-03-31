/*
   Author: RBW
*/
using Commons.Utils;
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_VariExtendSlot : APacketProcessor
    {
        public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write("010000".ToBytes());

				return stream.ToArray();
			}
		}
    }
}
