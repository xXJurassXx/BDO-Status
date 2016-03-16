/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_SaveCheckedQuest : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* TODO */
			}
			Log.Info("Client Save Checked Quest Resquested From Game Session!");
		}
    }
}
