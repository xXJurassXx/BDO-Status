using System.IO;
using System.Text;
using Commons.Models.Character;
using Commons.Utils;
using WorldServer.Emu.Extensions;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_CreateCharacterToField : APacketProcessor
    {
        private readonly CharacterData _info;

        public SMSG_CreateCharacterToField(CharacterData info)
        {
            _info = info;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((short)_info.ClassType.Ordinal());

                writer.Write((long)_info.CharacterId);
                writer.Write((byte)0); // slot
                writer.Write(BinaryExt.WriteFixedString(_info.CharacterName, Encoding.Unicode, 62));
                writer.Write((int)_info.Level);
                writer.Write((int)0);

				for (int i = 0; i < 31; i++) // inventoryEquipSlotsCount = 31
				{
					writer.Write((short)0); // item id
					writer.Write((short)0); // item enchant
					writer.Write((long)-2); // item expiration date
					writer.Write((short)0); // item current endurance
					for (int x = 0; x < 12; x++) // dye color palette infos = 12
					{
						writer.Write((byte)0xFF); // -1
						writer.Write((byte)0xFF); // -1
					}
					writer.Write((byte)0); // unk
				}

                writer.Write(_info.AppearancePresets); // 10 bytes
                writer.WriteC((byte)_info.Zodiac);
                writer.Write(_info.AppearanceOptions); // 800 bytes

                writer.Write(BinaryExt.WriteFixedString(_info.Surname, Encoding.Unicode, 62)); // family name

				writer.Write((long)-1); // character delete date
				writer.Write((byte)0); // character state
				writer.Write(new byte[28]); // zeroes
				writer.Write((long)-1); // character last login date

				writer.Write("0000000000309F055BF67F0000701ED5DA150000000000000000000000000000FFFFFFFFFFE86FC2A10A".ToBytes());

                return stream.ToArray();
            }
        }      
    }
}