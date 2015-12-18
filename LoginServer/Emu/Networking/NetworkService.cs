/*
   Author:Sagara
*/
using LoginServer.Configs;
using NLog;

namespace LoginServer.Emu.Networking
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
        }
    }
}
