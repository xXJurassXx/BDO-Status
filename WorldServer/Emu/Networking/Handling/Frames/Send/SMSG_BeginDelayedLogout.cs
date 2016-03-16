/*
   Author: RBW
*/
using System.IO;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_BeginDelayedLogout : APacketProcessor
    {
		private readonly long _time;

		public SMSG_BeginDelayedLogout(long time)
		{
			_time = time;
		}

		public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				/* Q */
				writer.Write((long)_time); // timer in milli

				return stream.ToArray();
			}
		}
    }
}
