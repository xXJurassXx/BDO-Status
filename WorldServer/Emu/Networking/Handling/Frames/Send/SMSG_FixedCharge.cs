/*
   Author: InCube, RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_FixedCharge : APacketProcessor
    {
		public override byte[] WritedData()
		{
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				/* Q,c */
				writer.Write((long)-2); // premium timer - default -2
				writer.Write((byte)0); // unk boolean [always 0 if not premium user]

				return stream.ToArray();
			}
		}
	}
}
