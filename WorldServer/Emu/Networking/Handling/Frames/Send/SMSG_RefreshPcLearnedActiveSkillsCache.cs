using System.IO;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    class SMSG_RefreshPcLearnedActiveSkillsCache : APacketProcessor
    {
        private readonly Player _character;
        public SMSG_RefreshPcLearnedActiveSkillsCache(Player character)
        {
            _character = character;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((int)_character.GameSessionId);
                writer.Write((long)_character.Uid);
                writer.Write("E86C0000".ToBytes()); // unk
				writer.Write((short)2); // loop count?
				{
					// 1
					writer.Write("0100".ToBytes()); // unk
					writer.Write("3203".ToBytes()); // unk
					// 2
					writer.Write("0100".ToBytes()); // unk
					writer.Write("3B03".ToBytes()); // unk
				}
				
				return stream.ToArray();
            }
        }
    }
}