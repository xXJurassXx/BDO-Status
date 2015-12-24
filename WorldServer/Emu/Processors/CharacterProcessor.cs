using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Enums;
using NHibernate.Util;
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
        /// Class for processing chat
        /// </summary>
        public ChatSubProcessor ChatProcessor { get; private set; }

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
            ChatProcessor = new ChatSubProcessor();
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
                                ChatProcessor.HandleChatMessage(connection, parameters, _onlineCharacters);
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

        /**
           * Author: InCube
        */
        public class ChatSubProcessor
        {
            private readonly object _cLock = new object();

            private enum CommandList
            {
                GM_SPAWN_MOB,
                GM_KICK_PLAYER,
                NONE
            };

            private readonly Dictionary<string, Dictionary<short, CommandList>> _commands = new Dictionary
                <string, Dictionary<short, CommandList>>
            {
                // Command  <Permission Level,  Command Enum> { List of available command levels }
                {"spawn", new Dictionary<short, CommandList> {{0, CommandList.GM_SPAWN_MOB}}},
                {"kick", new Dictionary<short, CommandList> {{0, CommandList.GM_KICK_PLAYER}}},
            };

            private Dictionary<int, Player> _onlinePlayers; 

            public void HandleChatMessage(ClientConnection client, object[] parameters, Dictionary<int, Player> onlineCharacters)
            {
                _onlinePlayers = onlineCharacters;
                if (ParseCommands((string)parameters[1], client.Account.AccessLevel))
                    return; // Since we are parsing the command, there is no need to reply to client

                switch ((ChatType)parameters[0])
                {
                    case ChatType.Alliance:         break; // TODO
                    case ChatType.Friend:           break; // TODO
                    case ChatType.Guild:            break; // TODO
                    case ChatType.Party:            break; // TODO
                    case ChatType.Private:          break; // TODO
                    case ChatType.Public:
                        client.ActivePlayer.VisibleAi.NotifyObjectsThatSeeMe<Player>(s
                            => new SpChat((string)parameters[1], client.ActivePlayer.GameSessionId, client.ActivePlayer.DatabaseCharacterData.CharacterName, (ChatType)parameters[0]).Send(s.Connection));
                        new SpChat((string)parameters[1], client.ActivePlayer.GameSessionId, client.ActivePlayer.DatabaseCharacterData.CharacterName, ChatType.Public).Send(client);
                        break;
                    case ChatType.World:
                    case ChatType.WorldWithItem: // TODO
                        foreach (var receiver in onlineCharacters.Values)
                            new SpChat((string)parameters[1], client.ActivePlayer.GameSessionId, client.ActivePlayer.DatabaseCharacterData.CharacterName, (ChatType)parameters[0]).Send(receiver.Connection);
                        break;
                }
                Log.Debug("Received message: {0}", (string)parameters[1]);
            }

            private bool ParseCommands(string text, int accessLevel)
            {
                if (text[0] != '!' && text[0] != '`' || text[0] < 3)
                    return false;
                var str = text.Split(' ');
                var command = _commands.FirstOrDefault(x => x.Key == str[0].Substring(1, str[0].Length - 1));
                var hasAccess = false;
                var commandListCommand = CommandList.NONE;
                
                
                foreach (var cmd in command.Value.Where(cmd => cmd.Key <= accessLevel))
                {
                    hasAccess = true;
                    commandListCommand = cmd.Value;
                }


                return hasAccess && ExecuteCommand(commandListCommand, str);
            }

            private bool ExecuteCommand(CommandList command, IEnumerable<string> parameters)
            {
                lock (_cLock)
                {
                    switch (command)
                    {
                        case CommandList.GM_SPAWN_MOB:
                            // TODO
                            Log.Debug("Should spawn a mob!");
                            break;
                        case CommandList.GM_KICK_PLAYER:
                            foreach (var player2 in parameters.Select(player
                                =>
                                _onlinePlayers.Where(
                                    x =>
                                        x.Value.DatabaseCharacterData.CharacterName.ToLower().Contains(player.ToLower())))
                                .SelectMany(thePlayers => thePlayers))
                            {
                                Log.Info("[GM] Kicked player {0}", player2.Value.DatabaseCharacterData.CharacterName);
                                player2.Value.Connection.CloseConnection();
                            }
                            break;
                    }
                }

                return true;
            }
        }
    }
}
