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

			Core.Act(s => s.LobbyProcessor.InitialEnterWorld(client, characterId));
			Log.Info("INITIAL DUMMY ENTER WORLD STARTED");

			//Core.Act(s => s.LobbyProcessor.PrepareForEnterOnWorld(client, characterId));
			//Log.Info("FULL ENTER WORLD STARTED!");
		}
    }
}
