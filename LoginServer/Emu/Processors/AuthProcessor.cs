/*
   Author:Sagara
*/
using LoginServer.Emu.Interfaces;
using LoginServer.Emu.Networking;
using NLog;

namespace LoginServer.Emu.Processors
{
    public class AuthProcessor : IProcessor
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void OnLoad(object previousInstanceContext)
        {
            
        }

        public void AuthProcess(ClientConnection client, string token)
        {
            //TODO - now need sleep =_=

            Log.Debug($"Token readed: {token}");
        }

        public object OnUnload()
        {
            return null;
        }
    }
}
