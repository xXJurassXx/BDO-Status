/*
   Author: RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_CancelDelayedLogout : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			Log.Info("Client Logout Cancel Resquested From Game Session!");
		}
    }
}
