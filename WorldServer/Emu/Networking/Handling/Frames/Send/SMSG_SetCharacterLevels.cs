using System.IO;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author:Sagara
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
                writer.Write("ABE7FFFFFFFFFFFFABE7FFFFFFFFFFFF".ToBytes());//static field
                writer.WriteD(_player.GameSessionId);
                writer.WriteD(_player.DatabaseCharacterData.Level);              
                writer.WriteQ(0); 
                writer.WriteQ(1); //2626 percent todo
                writer.Skip(9);
                return stream.ToArray();
            }
        }
    }
}