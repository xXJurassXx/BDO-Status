using System;
/*
   Author: RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_RemoveCancelCharacterFromField : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			/* Q */
            long characterId = BitConverter.ToInt64(data, 0);

            // TODO: create method here
        }
    }
}
