/*
   Author: RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_EndDelayedLogout : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			Log.Info("Client End Logout Resquested From Game Session!");
		}
    }
}
