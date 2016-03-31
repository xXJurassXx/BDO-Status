/*
   Author: RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_SetReadyToPlay : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			Log.Info("Client Set Ready To Play Resquested From Game Session!");
			Log.Info("Client Set Ready Checking And Preparing To Unlock Character!");

			Core.Act(s => s.LobbyProcessor.SetReadyToPlay(client));
		}
    }
}
