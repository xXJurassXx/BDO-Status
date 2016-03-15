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
    class CMSG_ListEnchantFailCountOfMyCharacter : APacketProcessor
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

			// SMSG_ListEnchantFailCountOfMyCharacter
			new SpRaw("0100" + uid + "0000000000000000", 0x0CFD).SendRaw(client.ActivePlayer.Connection);
			*/
			Log.Info("Client List Enchant Fail Count Of My Character Resquested From Game Session!");
		}
    }
}
