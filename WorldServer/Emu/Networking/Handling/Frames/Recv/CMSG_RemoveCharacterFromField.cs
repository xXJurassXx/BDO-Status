using System;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class CMSG_RemoveCharacterFromField : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			/* Q */
            long characterId = BitConverter.ToInt64(data, 0);

            Core.Act(s => s.LobbyProcessor.DeleteCharacterProcess(client, characterId));
        }
    }
}
