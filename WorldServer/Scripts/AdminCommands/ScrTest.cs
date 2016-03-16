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
            new SBpPlayerSpawn.SMSG_RefreshPcBasicCache(player).Send(connection);
            new SBpPlayerSpawn.SMSG_RefreshUserBasicCache(player).Send(connection);
            new SBpPlayerSpawn.SMSG_RefreshPcEquipSlotCache(player).Send(connection);
            new SMSG_RefreshPcCustomizationCache(player).Send(connection);
            new SMSG_RefreshPcLearnedActiveSkillsCache(player).Send(connection);            
        }
    }
}
