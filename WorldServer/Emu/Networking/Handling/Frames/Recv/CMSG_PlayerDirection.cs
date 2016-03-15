/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_PlayerDirection : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* f,d,f,f,f,f,d */
				float cosinus = reader.ReadSingle();
				reader.ReadInt32();
				float sinus = reader.ReadSingle();
				float start_x = reader.ReadSingle();
				float start_y = reader.ReadSingle();
				float start_z = reader.ReadSingle();
				var sessionId = reader.ReadInt32();
			}
			//Log.Info("Client Player Direction Resquested From Game Session!");
		}
    }
}
