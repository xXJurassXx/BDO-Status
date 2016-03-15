/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_ReadJournal : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* Q,h */
				var time = reader.ReadInt64();
				var type = reader.ReadInt16();
			}
			Log.Info("Client Read Journal Resquested From Game Session!");
		}
    }
}
