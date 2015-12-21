using System;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    public class RpEnterOnWorldRequest : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            long id = BitConverter.ToInt64(data, 0);

            Core.Act(s => s.LobbyProcessor.PrepareForEnterOnWorld(client, id));
        }
    }
}
