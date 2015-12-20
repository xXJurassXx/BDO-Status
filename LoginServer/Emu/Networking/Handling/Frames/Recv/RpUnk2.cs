using LoginServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    class RpUnk2 : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            new SpUnk3().Send(client, false);
            new SpUnk4().Send(client, false);
            new SpServerlist().Send(client);
        }
    }
}
