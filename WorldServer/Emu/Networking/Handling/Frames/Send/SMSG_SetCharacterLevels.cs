using System.IO;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SMSG_SetCharacterLevels : APacketProcessor
    {
        private readonly Player _player;
        public SMSG_SetCharacterLevels(Player player)
        {
            _player = player;
        }
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write("ABE7FFFFFFFFFFFF".ToBytes());
				writer.Write("ABE7FFFFFFFFFFFF".ToBytes());
				writer.Write((int)_player.GameSessionId);
                writer.Write((int)_player.DatabaseCharacterData.Level);              
                writer.Write((long)0); // exp have
                writer.Write((long)1); // exp need
				writer.Write((int)0);
				writer.Write((int)0);
				writer.Write((byte)0);
				return stream.ToArray();
            }
        }
    }
}