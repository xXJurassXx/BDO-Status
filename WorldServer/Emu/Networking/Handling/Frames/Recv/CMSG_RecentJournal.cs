/*
   Author: RBW
*/
using System.IO;
using System.Text;
using WorldServer.Emu.Networking.Handling.Frames.Send;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_RecentJournal : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* h */
				var unk = reader.ReadInt16();
			}
			/*
			// SMSG_RecentJournal
			new SpRaw("0000", 0x10D9).SendRaw(client.ActivePlayer.Connection);
			*/
			Log.Info("Client Recent Journal Resquested From Game Session!");
		}
    }
}
