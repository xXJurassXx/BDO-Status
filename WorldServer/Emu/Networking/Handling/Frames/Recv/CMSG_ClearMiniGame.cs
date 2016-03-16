/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_ClearMiniGame : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* h,d */
				var unk1 = reader.ReadInt16();
                var unk2 = reader.ReadInt32();
			}
			Log.Info("Client Clear MiniGame Resquested From Game Session!");
		}
    }
}
