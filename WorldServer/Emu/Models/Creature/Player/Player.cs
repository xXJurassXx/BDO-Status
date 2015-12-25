using System.Threading;
using Commons.Models.Character;
using SharpDX;
using WorldServer.Configs;
using WorldServer.Emu.Models.AI;
using WorldServer.Emu.Models.Storages;
using WorldServer.Emu.Networking;
using WorldServer.Emu.Structures.Geo.Basics;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.Creature.Player
{
    public class Player : ABdoObject
    {
        public event PlayerActionCallback PlayerActions;

        public CancellationTokenSource CancelTokenSource;
      
        public ClientConnection Connection;
               
        public CharacterData DatabaseCharacterData;

        public InventoryStorage Inventory;

        public EquipmentStorage Equipment;

        public int GameSessionId;

        public Player(ClientConnection connection, CharacterData characterData) : base(ObjectFamily.Player)
        {
            Connection = connection;
            DatabaseCharacterData = characterData;
            Position = new Position(new Vector3(characterData.PositionX, characterData.PositionY, characterData.PositionZ));
            Ai = new PlayerAI(this, CfgCore.Default.VisibleRangeDistance);
        }

        public void Dispose()
        {
            Release(); //release factories
            Action(PlayerAction.Logout); //handle logout action for processors
            CancelTokenSource = null;
            Connection = null;
            DatabaseCharacterData = null;
        }

        public void Action(PlayerAction action, params object[] parameters)
        {
            PlayerActions?.Invoke(action, parameters);
            if(PlayerActions == null)
                Log.Error($"Cannot invoke player action:{action}");
        }

        public enum PlayerAction
        {
            Chat,
            Logout
        }

        public delegate void PlayerActionCallback(PlayerAction action, params object[] parameters);
    }
}
