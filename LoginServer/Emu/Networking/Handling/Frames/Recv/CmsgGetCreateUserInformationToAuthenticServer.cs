/*
   Author:Sagara, InCube
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
                string token = Encoding.Unicode.GetString(reader.ReadBytes(2048)).Replace("\0", "");
                if (!string.IsNullOrEmpty(token))
                    Core.Act(s => s.AuthProcessor.AuthProcess(client, token));
                else
                    Log.Error("Cannot process authorize, unreadable token");                
            }         
        }
    }
}
