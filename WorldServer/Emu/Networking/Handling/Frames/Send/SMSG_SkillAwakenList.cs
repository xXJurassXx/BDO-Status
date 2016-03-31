/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_SkillAwakenList : APacketProcessor
    {
        public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((short)0); // loop count
				{
					// TODO: find loop data
				}

				return stream.ToArray();
			}
		}
    }
}
