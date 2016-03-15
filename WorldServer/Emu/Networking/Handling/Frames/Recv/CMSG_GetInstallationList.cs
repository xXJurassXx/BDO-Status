using System.IO;
using System.Text;
using Commons.Utils;
/*
   Author: RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_GetInstallationList : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
				/* Q,h,Q,c */
				var objectId = reader.ReadInt64(); // 0
				var houseId = reader.ReadInt16(); // 0
				var accountId = reader.ReadInt64(); // -1
				var type = reader.ReadByte(); // 10
            }
        }
    }
}
