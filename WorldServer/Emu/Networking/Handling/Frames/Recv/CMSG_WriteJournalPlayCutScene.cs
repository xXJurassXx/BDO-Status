/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_WriteJournalPlayCutScene : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* d */
				var unk = reader.ReadInt32();
			}
			Log.Info("Client Write Journal Play Cut Scene Resquested From Game Session!");
		}
    }
}
