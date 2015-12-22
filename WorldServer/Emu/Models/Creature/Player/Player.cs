using Commons.Models.Character;
using WorldServer.Emu.Networking;

namespace WorldServer.Emu.Models.Creature.Player
{
    public class Player : ABdoObject
    {
        public event PlayerActionCallback PlayerActions;

        public int GameSessionId;

        public ClientConnection Connection;
               
        public CharacterData DatabaseCharacterData;

        public Player(ClientConnection connection, CharacterData characterData) : base(ObjectFamily.Player)
        {
            Connection = connection;
            DatabaseCharacterData = characterData;
        }


        public void Dispose()
        {
            Release(); //release factories
            Action(PlayerAction.Logout); //handle logout action for processors
        }

        public void Action(PlayerAction action)
        {
            PlayerActions?.Invoke(action);
            if(PlayerActions == null)
                Log.Error($"Cannot invoke player action:{action}");
        }

        public enum PlayerAction
        {
            Chat,
            Logout
        }

        public delegate void PlayerActionCallback(PlayerAction action);
    }
}
