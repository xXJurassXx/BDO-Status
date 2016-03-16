/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_AcceptQuest : APacketProcessor
    {
		private readonly short _unk1;
		private readonly short _unk2;

		public SMSG_AcceptQuest(short unk1, short unk2)
		{
			_unk1 = unk1;
			_unk2 = unk2;
		}

		public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				/* h,h */
				writer.Write((short)_unk1);
				writer.Write((short)_unk2);

				return stream.ToArray();
			}
		}
    }
}
