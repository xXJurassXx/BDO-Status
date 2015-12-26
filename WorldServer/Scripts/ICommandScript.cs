using WorldServer.Emu.Networking;
/*
   Author:Sagara
*/
namespace WorldServer.Scripts
{
    public interface ICommandScript
    {
        void Process(ClientConnection connection, string[] message);
    }
}
