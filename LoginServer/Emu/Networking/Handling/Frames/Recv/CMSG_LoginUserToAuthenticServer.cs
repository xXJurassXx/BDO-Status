using LoginServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara, InCube
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    // ReSharper disable once InconsistentNaming
    class CMSG_LoginUserToAuthenticServer : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            new SMSG_LoginUserToAuthenticServer().Send(client);
            new SMSG_GetContentServiceInfo().Send(client, false);
        }
    }
}
