using WorldServer.Emu.Networking;

namespace WorldServer.Scripts
{
    public interface ICommandScript
    {
        void Process(ClientConnection connection, string[] message);
    }
}
