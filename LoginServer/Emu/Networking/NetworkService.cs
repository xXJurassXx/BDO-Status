/*
   Author:Sagara
*/
using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Commons.Models.Realm;
using Commons.Networking.Remoted;
using LoginServer.Configs;
using NLog;

namespace LoginServer.Emu.Networking
{
    internal class NetworkService
    {
        private static readonly Logger Log = LogManager.GetLogger(typeof(NetworkService).Name);

        private static readonly TcpServer ClientsListener =
            new TcpServer(CfgNetwork.Default.Host, CfgNetwork.Default.Port, CfgNetwork.Default.MaxConnections);

        public static ConcurrentDictionary<int, WsRealmInfo> WorldServers = new ConcurrentDictionary<int, WsRealmInfo>();

        public static void Initialize()
        {
            ClientsListener.Initialize();

            Log.Info($"Listening for new client connections at '{CfgNetwork.Default.Host}:{CfgNetwork.Default.Port}'");

            var remoted = new TcpChannel(5545);
            ChannelServices.RegisterChannel(remoted, true);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(LoginRemoteService), "login_service", WellKnownObjectMode.Singleton);
        }

        public class LoginRemoteService : MarshalByRefObject, IRMILoginService
        {
            public void Register(WsRealmInfo info)
            {
                if (info.RealmPassword != CfgNetwork.Default.RealmPassword)
                {
                    Log.Info($"Realm id:{info.Id} not passed!");
                    return;
                }
                if (WorldServers.ContainsKey(info.Id))
                {
                    Log.Info("Server already registered!");
                    return;
                }

                WorldServers.TryAdd(info.Id, info);

                Log.Info($"Realm server: {info.RealmName} registered!");
            }
        }
    }
}
