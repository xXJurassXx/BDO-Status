/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_CompleteQuest : APacketProcessor
    {
        public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				/* TODO */

				return stream.ToArray();
			}
		}
    }
}
