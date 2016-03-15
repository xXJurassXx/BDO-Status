/*
   Author: RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_ExitFieldServerToServerSelection : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			Log.Info("Client Exit to Server Selection Resquested From Lobby Session!");

			Core.Act(s => s.LobbyProcessor.BackToServerSelection(client, 0));
		}
    }
}
