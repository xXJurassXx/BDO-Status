using System.IO;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SpUpdateLevel : APacketProcessor
    {
        private readonly Player _player;
        public SpUpdateLevel(Player player)
        {
            _player = player;
        }
        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write("ABE7FFFFFFFFFFFFABE7FFFFFFFFFFFF".ToBytes());//static field
                writer.WriteD(_player.GameSessionId);
                writer.WriteD(_player.DatabaseCharacterData.Level);              
                writer.WriteQ(120); 
                writer.WriteD(0); //2626 percent todo
                writer.Skip(13);
                return stream.ToArray();
            }
        }
    }
}
