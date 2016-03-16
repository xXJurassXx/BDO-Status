/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_ContactNpc : APacketProcessor
    {
		private readonly long _unk1;
		private readonly int _unk2;

		public SMSG_ContactNpc(long unk1, int unk2)
		{
			_unk1 = unk1;
			_unk2 = unk2;
		}
		public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((long)_unk1);
				writer.Write((int)_unk2);
				writer.Write((int)1);
				writer.Write((int)-1);

				return stream.ToArray();
			}
		}
    }
}
