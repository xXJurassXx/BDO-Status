/**
 * Author: InCube
*/

using System;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    public class RpRequestDisconnect : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            var sendToTray = BitConverter.ToChar(data, 0);

            // Disconnect client immediately.
            client.CloseConnection();
        }
    }
}
