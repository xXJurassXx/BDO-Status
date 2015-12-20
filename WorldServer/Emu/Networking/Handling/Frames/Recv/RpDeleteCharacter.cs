using System;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    class RpDeleteCharacter : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
            long characterId = BitConverter.ToInt64(data, 0);

            Core.Act(s => s.LobbyProcessor.DeleteCharacterProcess(client, characterId));
        }
    }
}
