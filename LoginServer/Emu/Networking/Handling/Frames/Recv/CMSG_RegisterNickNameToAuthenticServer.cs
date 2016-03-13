using LoginServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara, InCube
*/
namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    // ReSharper disable once InconsistentNaming
    class CMSG_RegisterNickNameToAuthenticServer : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            new SMSG_RegisterNickNameToAuthenticServer().Send(client, false);
            new SMSG_FixedCharge().Send(client, false);
            new SMSG_GetWorldInformations(client.AccountInfo).Send(client);
        }
    }
}
