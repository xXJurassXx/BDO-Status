/*
   Author: RBW
*/
using System.IO;
using System.Text;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_SkillList : APacketProcessor
    {
		private readonly Player _character;

		public SMSG_SkillList(Player character)
		{
			_character = character;
		}

		public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((byte)1);
				writer.Write((byte)_character.DatabaseCharacterData.ClassType);
				writer.Write((short)0); // TODO: get skill count by class to the loop
				{
					/* 
					writer.Write((short)1);
					writer.Write((short)88);
					writer.Write((byte)0);
					*/
				}

				return stream.ToArray();
			}
		}
    }
}
