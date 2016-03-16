/*
   Author: RBW
*/
using System.IO;
using System.Text;
using WorldServer.Emu.Networking.Handling.Frames.Send;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_ContactNpc : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* TODO */
				var unk1 = reader.ReadInt64();
				var unk2 = reader.ReadInt32();

				new SMSG_ContactNpc(unk1, unk2).Send(client);
			}
			Log.Info("Client Contact Npc Resquested From Game Session!");
		}
    }
}
