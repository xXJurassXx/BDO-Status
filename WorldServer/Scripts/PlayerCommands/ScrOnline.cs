using Commons.Enums;
using WorldServer.Emu;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara
*/
namespace WorldServer.Scripts.PlayerCommands
{
    public class ScrOnline : ICommandScript
    {
        public void Process(ClientConnection connection, string[] message)
        {
            Core.Act(s =>
            {
                var onlineCount = s.CharacterProcessor.OnlineList.Count;


                new SMSG_Chat($"Players online:{onlineCount}", connection.ActivePlayer.GameSessionId, connection.ActivePlayer.DatabaseCharacterData.CharacterName, ChatType.Public).Send(connection);
            });
        }
    }
}
