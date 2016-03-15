/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_ExitFieldServerToServerSelection : APacketProcessor
    {
		public override byte[] WritedData()
		{
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				/* d */
				writer.Write((int)1678849575); // cookie id

				return stream.ToArray();
			}
		}
	}
}
