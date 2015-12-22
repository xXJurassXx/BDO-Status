/**
 * Author: InCube, Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    public class RpRequestDisconnect : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            Core.Act(s => s.CharacterProcessor.Requests.CloseClientRequest(client));
        }
    }
}
