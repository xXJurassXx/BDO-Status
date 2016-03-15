/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_PaymentPasswordRegister : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* c,c,Q*20 */
				var unk1 = reader.ReadByte();
				var unk2 = reader.ReadByte();
				var blocks = reader.ReadBytes(160); // 20*8
			}
			Log.Info("Client Payment Password Register Resquested From Game Session!");
		}
    }
}
