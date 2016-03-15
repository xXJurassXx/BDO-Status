/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_ListSiegeGuild : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* h */
				var siegeId = reader.ReadInt16();
			}
			Log.Info("Client List Siege Guild Resquested From Game Session!");
		}
    }
}
