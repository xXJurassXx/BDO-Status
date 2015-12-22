/**
 * Author: InCube
*/

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    public class RpRequestDisconnect : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            //TODO Delayed exit on lobby, or just close client
        }
    }
}
