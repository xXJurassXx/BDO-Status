using System;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Recv
{
    public class CMSG_EnterPlayerCharacterToField : APacketProcessor
    {
        public override void Process(ClientConnection client, byte[] data)
        {
			/* Q */
            long characterId = BitConverter.ToInt64(data, 0);
			Log.Info("ENTER WORLD TEMP DISABLED UNTIL REWORK IS FINISHED");
			//Core.Act(s => s.LobbyProcessor.PrepareForEnterOnWorld(client, characterId));
		}
    }
}
