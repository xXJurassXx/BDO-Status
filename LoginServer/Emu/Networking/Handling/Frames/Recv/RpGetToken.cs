/*
   Author:Sagara
*/
using System.IO;
using System.Text;

namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    public class RpGetToken : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                string token = Encoding.Unicode.GetString(reader.ReadBytes(2048)).Replace("\0", "");

                Core.Act(s => s.AuthProcessor.AuthProcess(client, token));
            }         
        }
    }
}
