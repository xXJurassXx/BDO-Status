using System.Collections.Generic;
using WorldServer.Emu.Interfaces;
using WorldServer.Emu.Models.Creature.Player;
using WorldServer.Emu.Networking;

namespace WorldServer.Emu.Processors
{
    public class CharacterProcessor : IProcessor
    {
        public CRequests Requests { get; private set; }

        private readonly Dictionary<int, Player> _onlineCharacters = new Dictionary<int, Player>();
        private readonly object _lOnline = new object();

        public void OnLoad(object previousInstanceContext)
        {
            Requests = new CRequests();
        }

        public void EndLoad(ClientConnection connection)
        {
            lock (_lOnline)
            {
                if (connection.ActivePlayer != null)
                {
                    var player = connection.ActivePlayer;
                    if(!_onlineCharacters.ContainsKey(player.GameSessionId))
                        _onlineCharacters.Add(player.GameSessionId, player);

                    connection.ActivePlayer.PlayerActions += action =>
                    {
                        switch (action)
                        {
                           case Player.PlayerAction.Logout:
                                if (_onlineCharacters.ContainsKey(player.GameSessionId))
                                    _onlineCharacters.Remove(player.GameSessionId);

                                break;
                        }
                    };
                }
            }
        }

        public object OnUnload()
        {
            return null;
        }

        public class CRequests
        {
            
        }
    }
}
