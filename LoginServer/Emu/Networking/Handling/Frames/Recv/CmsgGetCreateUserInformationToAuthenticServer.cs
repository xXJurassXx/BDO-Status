/*
   Author: Sagara, InCube, RBW
*/
using System.IO;
using System.Text;

namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    // ReSharper disable once InconsistentNaming
    public class CMSG_GetCreateUserInformationToAuthenticServer : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
				/* S(2048),d,d,d */
                string token = Encoding.Unicode.GetString(reader.ReadBytes(2050)).Replace("\0", ""); // auth token
				var cookie = reader.ReadInt32(); // login session cookie id
				var clientVersion = reader.ReadInt32(); // static 51, not used anymore
				var unk = reader.ReadInt32(); // always 0
				Log.Info("cookie id: " + cookie + " client version: " + clientVersion + " unk zero: " + unk);

				if (!string.IsNullOrEmpty(token))
                    Core.Act(s => s.AuthProcessor.AuthProcess(client, token));
                else
                    Log.Error("Cannot process authorize, unreadable token");
            }
        }
    }
}
