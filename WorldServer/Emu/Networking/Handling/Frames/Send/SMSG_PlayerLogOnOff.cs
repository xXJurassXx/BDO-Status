/*
   Author: RBW
*/
using Commons.Utils;
using System.IO;
using System.Text;
using WorldServer.Emu.Extensions;
using WorldServer.Emu.Models.Creature.Player;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_PlayerLogOnOff : APacketProcessor
    {
		private readonly Player _character;

		public SMSG_PlayerLogOnOff(Player character)
		{
			_character = character;
		}

		public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((int)_character.GameSessionId);
				writer.Write((long)_character.Connection.Account.Id);
				writer.Write((long)_character.Id);
				writer.Write((long)0); // utc
				writer.Write((long)0); // utc
				writer.Write((short)0); // wp
				writer.Write((short)0); // cp
				writer.Write(BinaryExt.WriteFixedString(_character.DatabaseCharacterData.CharacterName, Encoding.Unicode, 62));
				writer.Write((int)_character.DatabaseCharacterData.Level);
				writer.Write((long)-6012); // guild?
				writer.Write(BinaryExt.WriteFixedString(_character.DatabaseCharacterData.Surname, Encoding.Unicode, 62));
				writer.Write((short)0);
				writer.Write((short)_character.DatabaseCharacterData.ClassType.Ordinal());

				return stream.ToArray();
			}
		}
    }
}
