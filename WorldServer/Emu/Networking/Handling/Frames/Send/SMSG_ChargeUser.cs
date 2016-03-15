/*
   Author: InCube, RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_ChargeUser : APacketProcessor
    {
        public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				/* Q,Q,Q,Q,Q,Q */
				writer.Write((long)0); // premium buff timer
				writer.Write((long)0); // premium buff timer
				writer.Write((long)0); // premium buff timer
				writer.Write((long)0); // premium buff timer
				writer.Write((long)0); // premium buff timer
				writer.Write((long)0); // premium buff timer

				return stream.ToArray();
			}
		}
    }
}
