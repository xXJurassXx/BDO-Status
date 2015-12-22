using Commons.Models.Character;

namespace WorldServer.Emu.Models.Creature.Player
{
    public class Player : ABdoObject
    {
        public int GameSessionId;
               
        public CharacterData DatabaseCharacterData;

        public Player(CharacterData characterData) : base(ObjectFamily.Player)
        {
            DatabaseCharacterData = characterData;
        }
    }
}
