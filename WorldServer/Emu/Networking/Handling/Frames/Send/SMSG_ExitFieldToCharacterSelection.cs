/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_ExitFieldToCharacterSelection : APacketProcessor
    {
        public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				/* d */
				writer.Write((int)1975713236);

				return stream.ToArray();
			}
		}
    }
}
