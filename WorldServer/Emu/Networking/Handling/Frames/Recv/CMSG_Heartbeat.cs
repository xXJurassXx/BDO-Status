/*
   Author: InCube, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_Heartbeat : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			Log.Info("Client Heartbeat Resquested From Game Session!");
		}
    }
}
