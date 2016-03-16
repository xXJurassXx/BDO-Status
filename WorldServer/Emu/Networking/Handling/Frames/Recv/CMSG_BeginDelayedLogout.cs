/**
 * Author: InCube, Sagara, RBW
*/
using System.IO;
using System.Text;
using WorldServer.Emu.Networking.Handling.Frames.Send;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    public class CMSG_BeginDelayedLogout : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				var result = reader.ReadByte();

				new SMSG_BeginDelayedLogout(10000).Send(client);
			}

            Core.Act(s => s.CharacterProcessor.Requests.CloseClientRequest(client));

			Log.Info("Client Begin Delayed Logout Resquested From Game Session!");
		}
    }
}
