using System.IO;
using System.Text;
using Commons.Utils;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_LoginUserToFieldServer : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
				/* S(62),d,d,d,s(18),b(15) */
                string token = reader.ReadString(62, Encoding.Unicode).Replace("\0", "");
				var cookie = reader.ReadInt32(); // cookie id
				var unk1 = reader.ReadInt32(); // 12
				var unk2 = reader.ReadInt32(); // 2
				string macAddress = reader.ReadString(18, Encoding.ASCII).Replace("\0", "");
				var unkBytes = reader.ReadBytes(15); // unk padding bytes
				Log.Info("token id: " + token + " cookie: " + cookie + " unk1: " + unk1 + " unk2: " + unk2);

				Core.Act(s => s.AuthProcessor.AuthProcess(client, token));
            }
        }
    }
}
