using LoginServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    class RpUnk : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            new SpUnk().Send(client);
            new SpUnk2().Send(client, false);
        }
    }
}
