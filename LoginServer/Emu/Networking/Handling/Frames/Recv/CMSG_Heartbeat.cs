using LoginServer.Emu.Networking.Handling.Frames.Send;
/*
   Author: RBW
*/
using System.IO;
using System.Text;

namespace LoginServer.Emu.Networking.Handling.Frames.Recv
{
    // ReSharper disable once InconsistentNaming
    class CMSG_Heartbeat : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			Log.Info("Client Heartbeat Resquested From Login Session!");
        }
    }
}
