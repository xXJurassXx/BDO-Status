/**
 * Author: InCube, Sagara, RBW
*/
using System.IO;
using System.Text;

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
			}

            Core.Act(s => s.CharacterProcessor.Requests.CloseClientRequest(client));
        }
    }
}
