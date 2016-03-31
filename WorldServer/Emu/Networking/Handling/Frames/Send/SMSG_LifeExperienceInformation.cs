/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_LifeExperienceInformation : APacketProcessor
    {
		private readonly byte _type;

		public SMSG_LifeExperienceInformation(byte type)
		{
			_type = type;
		}

		public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((byte)_type);
				writer.Write((int)1);
				writer.Write((long)0);

				return stream.ToArray();
			}
		}
    }
}
