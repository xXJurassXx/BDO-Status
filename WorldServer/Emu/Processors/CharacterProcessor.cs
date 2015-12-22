using System.Collections.Generic;
using Commons.Enums;
using NLog;
using WorldServer.Configs;
using WorldServer.Emu.Interfaces;
using WorldServer.Emu.Models.Creature.Player;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Networking.Handling.Frames.Send;

namespace WorldServer.Emu.Processors
{
    public class CharacterProcessor : IProcessor
    {
        /// <summary>
        /// Logger for this class
        /// </summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Class for requests
        /// </summary>
        public CRequests Requests { get; private set; }

        /// <summary>
        /// Online characters collection
        /// </summary>
        private readonly Dictionary<int, Player> _onlineCharacters = new Dictionary<int, Player>();
        /// <summary>
        /// Object for lock collection
        /// </summary>
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

                    connection.ActivePlayer.PlayerActions += (action, parameters) =>
                    {
                        switch (action)
                        {
                           case Player.PlayerAction.Logout:
                                if (_onlineCharacters.ContainsKey(player.GameSessionId))
                                {
                                    _onlineCharacters.Remove(player.GameSessionId);
                                    Log.Debug($"Player:{player.DatabaseCharacterData.CharacterName} has left the world");
                                }
                                break;

                            case Player.PlayerAction.Chat:
                                var chatType = (ChatType) parameters[0];
                                var message = (string) parameters[1];
                                switch (chatType)
                                {
                                    case ChatType.World:
                                        foreach (var receiver in _onlineCharacters.Values)
                                            new SpChat(message, player.GameSessionId, player.DatabaseCharacterData.CharacterName, ChatType.World).Send(receiver.Connection);
                                        break; 
                                                        
                                    //TODO - Other types send on visible ai
                                }
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
            public void CloseClientRequest(ClientConnection connection)
            {
                Core.Act(s =>
                {
                    connection.CloseConnection();
                }, CfgCore.Default.LogoutSeconds * 1000, connection.ActivePlayer.CancelTokenSource);
            }

            public void CancelCloseClientRequest(ClientConnection connection)
            {
                var player = connection.ActivePlayer;
                if(!player.CancelTokenSource.IsCancellationRequested)
                    player.CancelTokenSource.Cancel();
            }
        }
    }
}
