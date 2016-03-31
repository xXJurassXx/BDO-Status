/*
   Author: RBW
*/
using System.IO;
using System.Text;
using Commons.Utils;
using WorldServer.Emu.Extensions;
using WorldServer.Emu.Models.Creature.Player;

namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_AddPlayers : APacketProcessor
    {
		private readonly Player _character;
		private readonly byte _type;

		public SMSG_AddPlayers(Player character, byte type)
		{
			_character = character;
			_type = type;
		}

		public override byte[] WritedData()
        {
			using (var stream = new MemoryStream())
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((byte)1);
				writer.Write((byte)0);
				writer.Write((byte)0);
				writer.Write((byte)_type);

				writer.Write((short)1); // loop count
				{
					writer.Write((int)_character.GameSessionId);
					writer.Write((float)_character.Position.Point.X); // x
					writer.Write((float)_character.Position.Point.Y); // y
					writer.Write((float)_character.Position.Point.Z); // z
					writer.Write((float)_character.Position.Cosinus); // cosinus
					writer.Write((int)0);
					writer.Write((float)_character.Position.Sinus); // sinus
					writer.Write((short)_character.DatabaseCharacterData.ClassType.Ordinal());
					writer.Write((short)0);
					writer.Write((float)150); // hp
					writer.Write((float)150); // hp
					writer.Write((long)-6012);
					writer.Write(new byte[8]);
					writer.Write((long)-6012);
					writer.Write((byte)1);
					writer.Write("946DE495".ToBytes());
					writer.Write(new byte [16]);
					writer.Write((byte)3);
					writer.Write(new byte[12]);
					writer.Write((int)-1);
					writer.Write((short)0);
					writer.Write((short)0);
					writer.Write(new byte[800]); // nulled appearance data?
					writer.Write((long)0);
					writer.Write((long)_character.DatabaseCharacterData.AccountId);
					writer.Write((int)1); // incremental
					writer.Write((long)_character.Uid);
					writer.Write((int)1); // incremental
					writer.Write((int)1); // incremental
					writer.Write((int)0); // incremental
					writer.Write((byte)17);
					writer.Write((byte)0);
					writer.Write((byte)0);
					writer.Write((int)0);
					writer.Write((int)0);
					writer.Write((short)0);
					writer.Write((byte)0); // pvp?
					writer.Write((byte)0); // pvp?
					writer.Write((short)0); // item - weapon?
					writer.Write((short)0);
					writer.Write((int)-1021);
					writer.Write(new byte[6]);
					writer.Write((byte)37);
					writer.Write(new byte[12]);
					writer.Write((byte)5);
					writer.Write((int)0); // karma?
					writer.Write((int)0);
					writer.Write(new byte[64]);
					writer.Write((int)-1024);
					writer.Write((byte)1); // 0 when created or 1 if not new character
					writer.Write((int)_character.DatabaseCharacterData.Level);
					writer.Write((int)0);
					writer.Write((byte)3);
					writer.Write(new byte[43]);
					writer.Write((int)-2048);
					writer.Write(new byte[12]);
					writer.Write((int)-2048);
					writer.Write(new byte[12]);
					writer.Write((int)2);
					writer.Write(new byte[10]);
				}

				return stream.ToArray();
			}
		}
    }
}
