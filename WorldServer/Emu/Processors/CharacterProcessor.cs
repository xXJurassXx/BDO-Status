using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Enums;
using NLog;
using WorldServer.Configs;
using WorldServer.Emu.Interfaces;
using WorldServer.Emu.Models.Creature.Player;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Networking.Handling.Frames.Send;
using WorldServer.Scripts;
using WorldServer.Scripts.AdminCommands;
using WorldServer.Scripts.PlayerCommands;

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
            ChatProcessor = new ChatSubProcessor(_onlineCharacters);
        }

        /// <summary>
        /// Get player list from online collection
        /// </summary>
        public List<Player> OnlineList
        {
            get
            {
                lock (_lOnline)
                    return _onlineCharacters.Values.ToList();
            }
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
                                ChatProcessor.HandleChatMessage(connection, parameters);
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

        /*
            Author: InCube, Sagara
        */
        public class ChatSubProcessor
        {
            /// <summary>
            /// [accessLevel][CommandName][CommandCompile]
            /// </summary>
            public static readonly Dictionary<int, Dictionary<string, Type>> Commands = new Dictionary<int, Dictionary<string, Type>>
            {
                {
                    5, new Dictionary<string, Type>
                    {
                        {"Kick", typeof(ScrKick) },
                        {"test", typeof(ScrTest) } 
                    }                    
                },
                {0, new Dictionary<string, Type> { {"online", typeof(ScrOnline) } } }
            };

            private readonly Dictionary<int, Player> _onlinePlayers;

            public ChatSubProcessor(Dictionary<int, Player> onlineContainer)
            {
                _onlinePlayers = onlineContainer;
            }
          
            public void HandleChatMessage(ClientConnection client, object[] parameters)
            {
                if (ProcessCommand(client, (string)parameters[1], client.Account.AccessLevel))
                    return; // Since we are parsing the command, there is no need to reply to client

                switch ((ChatType)parameters[0])
                {
                    case ChatType.Public:
                        client.ActivePlayer.VisibleAi.NotifyObjectsThatSeeMe<Player>(s => new SpChat((string)parameters[1], client.ActivePlayer.GameSessionId, client.ActivePlayer.DatabaseCharacterData.CharacterName, (ChatType)parameters[0]).Send(s.Connection));
                        new SpChat((string)parameters[1], client.ActivePlayer.GameSessionId, client.ActivePlayer.DatabaseCharacterData.CharacterName, ChatType.Public).Send(client);
                        break;

                    case ChatType.World:
                        foreach (var receiver in _onlinePlayers.Values)
                            new SpChat((string)parameters[1], client.ActivePlayer.GameSessionId, client.ActivePlayer.DatabaseCharacterData.CharacterName, (ChatType)parameters[0]).Send(receiver.Connection);
                        break;
                }

                Log.Debug("Received message: {0}", (string)parameters[1]);
            }

            private bool ProcessCommand(ClientConnection client, string text, int accessLevel)
            {
                if (text.StartsWith("`") && accessLevel < CfgCore.Default.AccesForAdminCommand)
                    return false; //dont have access for admin commands

                if (!text.StartsWith("`") && !text.StartsWith("!"))
                    return false; //not command

                var commandName = text.Split(' ')[0].Replace("!", "").Replace("`", "");

                var command = Commands.Values.FirstOrDefault(s => s.ContainsKey(commandName));
                if (command != null)
                {
                    var selected = Commands.FirstOrDefault(s => s.Value == command);
                    if (accessLevel < selected.Key) //if user dont have access for that command
                        return false;                
                    try
                    {
                        ((ICommandScript)Activator.CreateInstance(command[commandName])).Process(client, text.Split(' ').Skip(1).ToArray());
                        return true; //command processed
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Something wrong happened, while try to process command {command.GetType().Name}, \nEx:{e}");
                    }                   
                }
                else
                    return false; //command not found
                  
                return false;
            }
        }
    }
}