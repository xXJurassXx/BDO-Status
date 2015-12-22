using System;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class RpEnterOnWorldProcess : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            int gameSession = BitConverter.ToInt32(data, 0);

            Core.Act(s => s.LobbyProcessor.EnterOnWorldProcess(client, gameSession));
        }
    }
}
