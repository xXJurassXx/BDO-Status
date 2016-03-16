/*
   Author: RBW
*/
using System.IO;
using System.Text;
using WorldServer.Emu.Networking.Handling.Frames.Send;

namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_ExitFieldToCharacterSelection : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			using (var stream = new MemoryStream(data))
			using (var reader = new BinaryReader(stream))
			{
				/* empty */
				/* TODO: create method */
				//new SMSG_ExitFieldToCharacterSelection().Send(client);
			}
			Log.Info("Client Exit Field To Character Selection Resquested From Game Session!");
		}
    }
}
