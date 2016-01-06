using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Commons.Models.Realm;
using Commons.Networking.Remoted;
using NLog;
using WorldServer.Configs;
using WorldServer.Emu.Processors;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking
{
    internal class NetworkService
    {
        private static readonly Logger Log = LogManager.GetLogger(typeof(NetworkService).Name);

        private static readonly TcpServer ClientsListener =
            new TcpServer(CfgNetwork.Default.Host, CfgNetwork.Default.Port, CfgNetwork.Default.MaxConnections);

        public static void Initialize()
        {
            ClientsListener.Initialize();

            Log.Info($"Listening for new client connections at '{CfgNetwork.Default.Host}:{CfgNetwork.Default.Port}'");

            try
            {
                var chan = new TcpChannel(5546);

                ChannelServices.RegisterChannel(chan, true);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(RealmRemoteService), "realm_service", WellKnownObjectMode.Singleton);

                var obj = (IRMILoginService)Activator.GetObject(typeof(IRMILoginService), $"tcp://{CfgNetwork.Default.RemotedHost}:5545/login_service");               
                obj.Register(new WsRealmInfo
                {
                    Id = 1, ChannelName = CfgCore.Default.ChannelName, RealmName = CfgCore.Default.RealmName, RealmPassword = CfgNetwork.Default.RealmPassword, ChannelId = 1,
                    RealmIp = "127.0.0.1", RealmPort = 8889
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Exception occured:\n{ex}");
            }
        }

        public class RealmRemoteService : MarshalByRefObject, IRMIRealmService
        {
            public int CharacterCount(int accountId)
            {
                return LobbyProcessor.GetCharactersCount(accountId);
            }
        }
    }
}
