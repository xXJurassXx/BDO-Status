using System.Collections.Generic;
using NLog;
using WorldServer.Configs;
using WorldServer.Emu.Interfaces;
using WorldServer.Emu.Models;
using WorldServer.Emu.Models.Creature.Player;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Structures.Geo;
using WorldServer.Emu.Structures.Geo.Basics;
/*
   Author:Sagara
   TODO - Analyse bdo map structure, channeling
*/
namespace WorldServer.Emu.Processors
{
    public class WorldProcessor : IProcessor
    {
        private static readonly Logger Log = LogManager.GetLogger(typeof(WorldProcessor).Name);
        private readonly Dictionary<int, List<Area>> _world = new Dictionary<int, List<Area>>();
        private readonly object _worldLock = new object();

        public void OnLoad(object previousInstanceContext)
        {
            for (int i = 0; i < CfgCore.Default.WorldChannelsCount; i++)
            {
                var channelId = i + 1;

                _world.Add(channelId, new List<Area>());
                _world[channelId].Add(new Area());
            }

            Log.Info($"[{GetType().Name}] Created {_world.Count} channels");
        }

        public void EndLoad(ClientConnection connection)
        {
            var player = connection.ActivePlayer;
            lock (_worldLock)
            {
                var area = _world[1][0];

                area.PlaceToArea(player);
                player.Area = area;

                player.PlayerActions += (action, parameters) =>
                {
                    switch (action)
                    {
                       case Player.PlayerAction.Logout:
                            player.Area.RemoveFromArea(player);
                            player.Area = null;
                            break;
                    }
                };
            }
        }

        public void ObjectMoved(ABdoObject obj, MovementAction movement)
        {
            switch (obj.Family)
            {
                case ABdoObject.ObjectFamily.Player:

                    var player = (Player)obj;

                    player.Position.Point.X = movement.StartPosition.Point.X;
                    player.Position.Point.Y = movement.StartPosition.Point.Y;
                    player.Position.Point.Z = movement.StartPosition.Point.Z;
                    player.Position.Heading = movement.Heading;

                    player.VisibleAi.OwnerMoved(movement);

                    break;

                default:
                    Log.Debug($"Cannot process move action for {obj.Family} object");
                    break;
            }
        }
        public object OnUnload()
        {
            return null;
        }
    }
}
