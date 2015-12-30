using SharpDX;
using WorldServer.Emu.Models.Creature.Player;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Networking.Handling.Frames.Send;
using WorldServer.Emu.Networking.Handling.Frames.Send.OPSBlob;
using WorldServer.Emu.Structures.Geo.Basics;
/*
   Author:Sagara
*/
namespace WorldServer.Scripts.AdminCommands
{
    public class ScrTest : ICommandScript
    {
        public void Process(ClientConnection connection, string[] message)
        {
            var player = new Player(connection, connection.ActivePlayer.DatabaseCharacterData)
            {
                GameSessionId = 1135620,
                Position =
                    new Position(new Vector3(connection.ActivePlayer.Position.Point.X,
                        connection.ActivePlayer.Position.Point.Y, connection.ActivePlayer.Position.Point.Z))
            };

            new SBpPlayerSpawn.SpSpawnPlayer(player).Send(connection);
            new SBpPlayerSpawn.SpSetPlayerName(player).Send(connection);
            new SBpPlayerSpawn.SpSetPlayerFamilyName(player, connection.Account).Send(connection);
            new SBpPlayerSpawn.SpSetPlayerEquipment(player).Send(connection);
            new SpCharacterCustimozationData(player).Send(connection);
            new SpCharacterCustomizationResponse(player).Send(connection);            
        }
    }
}
