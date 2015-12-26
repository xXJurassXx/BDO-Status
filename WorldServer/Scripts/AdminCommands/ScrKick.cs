using System.Linq;
using Commons.Enums;
using WorldServer.Emu;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Networking.Handling.Frames.Send;
/*
   Author:Sagara, InCube
*/
namespace WorldServer.Scripts.AdminCommands
{
    public class ScrKick : ICommandScript
    {
        public void Process(ClientConnection connection, string[] message)
        {
            Core.Act(s =>
            {
                var playersList = s.CharacterProcessor.OnlineList;

                if (message.Length > 1)
                {
                    foreach (var selectedName in message)
                    {
                        var result = playersList.FirstOrDefault(
                                p => p.DatabaseCharacterData.CharacterName == selectedName);

                        if (result == null)
                            new SpChat($"[Admin processor] cannot found {selectedName} player", connection.ActivePlayer.GameSessionId,
                                connection.ActivePlayer.DatabaseCharacterData.CharacterName, ChatType.Notice)
                                .Send(connection);
                        else
                            result.Connection.CloseConnection();
                    }
                }
                if (message.Length == 1)
                {
                    var select =
                        playersList.FirstOrDefault(p => p.DatabaseCharacterData.CharacterName == message[0]);
                    if (select != null)
                        select.Connection.CloseConnection();
                    else
                        new SpChat($"[Admin processor] cannot found {message[0]} player",
                            connection.ActivePlayer.GameSessionId, connection.ActivePlayer.DatabaseCharacterData.CharacterName, ChatType.Notice).Send(connection);
                }
            });
        }
    }
}