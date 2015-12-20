using System.IO;
using System.Text;
using Commons.Utils;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class RpGetToken : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                string token = reader.ReadString(38, Encoding.ASCII).Replace("\0", "");

                Core.Act(s => s.AuthProcessor.AuthProcess(client, token));
            }
        }
    }
}
