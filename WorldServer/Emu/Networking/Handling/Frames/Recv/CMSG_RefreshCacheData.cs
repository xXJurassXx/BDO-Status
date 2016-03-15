/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_RefreshCacheData : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* h,d,d,d */
				var type = reader.ReadInt16();
				var clientSessionId = reader.ReadInt32();
				var unk1 = reader.ReadInt32();
				var unk2 = reader.ReadInt32();
			}
			Log.Info("Client Refresh Resquested From Game Session!");
		}
    }
}
