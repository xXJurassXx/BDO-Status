using LoginServer.Emu.Networking.Handling.Frames.Send;
/*
   Author: Sagara, InCube, RBW
*/
using System.IO;
using System.Text;

namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    // ReSharper disable once InconsistentNaming
    class CMSG_LoginUserToAuthenticServer : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* S(62),c,c,Q*20,d */
				string token = Encoding.Unicode.GetString(reader.ReadBytes(62)).Replace("\0", ""); // auth token
				var pinOrderBlockIndex = reader.ReadByte(); // index of correct pin panel number order block to be used/show to user
				var usedPinCode = reader.ReadByte(); // usedPinCode
				var pinIndexBlocks = reader.ReadBytes(160); // 20 * 8 bytes pin panel number order
				var sessionId = reader.ReadInt32(); // always 0 if login, if coming from GS back to LS, its ls session id
				Log.Info("token: " + token + " pin order index: " + pinOrderBlockIndex + " used pin: " + usedPinCode);
			}

			new SMSG_LoginUserToAuthenticServer().Send(client);
            new SMSG_GetContentServiceInfo().Send(client, false);
        }
    }
}
