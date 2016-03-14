using LoginServer.Emu.Networking.Handling.Frames.Send;
/*
   Author: Sagara, InCube, RBW
*/
using System.IO;
using System.Text;

namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    // ReSharper disable once InconsistentNaming
    class CMSG_RegisterNickNameToAuthenticServer : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* S(62),c,c,Q*20,d */
				string familyName = Encoding.Unicode.GetString(reader.ReadBytes(62)).Replace("\0", "");
				var screenW = reader.ReadInt32();
				var screenH = reader.ReadInt32();
				Log.Info("family name: " + familyName + " screen width: " + screenW + " screen height: " + screenH);
			}

			new SMSG_RegisterNickNameToAuthenticServer().Send(client, false);
            new SMSG_FixedCharge().Send(client, false);
            new SMSG_GetWorldInformations(client.AccountInfo).Send(client);
        }
    }
}
