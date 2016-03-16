/*
   Author: RBW
*/
using Commons.Utils;
using System;
using System.IO;
using System.Text;
using WorldServer.Emu.Networking.Handling.Frames.Send;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_ListWaitingCountOfMyCharacter : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* empty */
			}
			/*
			var uid = BitConverter.GetBytes(client.ActivePlayer.Uid).ToHex();

			// SMSG_WaitToEnterToField [Q,Q,Q,c,Q,Q]
			new SpRaw("FFFFFFFFFFFFFFFF0000000000000000000000000000000001" + uid + "FFFFFFFFFFFFFFFF", 0x0CF1).SendRaw(client);
			*/
			Log.Info("Client List Waiting Count Of My Character Resquested From Game Session!");
		}
    }
}
