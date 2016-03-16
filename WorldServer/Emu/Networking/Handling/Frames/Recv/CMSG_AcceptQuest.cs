/*
   Author: RBW
*/
using System.IO;
using System.Text;
using WorldServer.Emu.Networking.Handling.Frames.Send;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_AcceptQuest : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* h,h,unk */
				var unk1 = reader.ReadInt16();
				var unk2 = reader.ReadInt16();
				/* TODO: more bytes below to find out */

				new SMSG_AcceptQuest(unk1, unk2).Send(client);
			}

			Log.Info("Client Accept Quest Resquested From Game Session!");
		}
    }
}
