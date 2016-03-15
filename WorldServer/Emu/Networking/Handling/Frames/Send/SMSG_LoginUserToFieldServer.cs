using System.Collections.Generic;
using System.IO;
using System.Text;
using Commons.Models.Account;
using Commons.Models.Character;
using Commons.Utils;
using NHibernate.Util;
using WorldServer.Emu.Extensions;
using WorldServer.Emu.Models.Storages;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_LoginUserToFieldServer : APacketProcessor
    {
        private readonly AccountData _account;
        private readonly List<CharacterData> _characters;

        public SMSG_LoginUserToFieldServer(AccountData account, List<CharacterData> characters)
        {
            _account = account;
            _characters = characters;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
				writer.Write((int)0); // packet number
				writer.Write((int)4521); // server version
				writer.Write((int)14998534); // client session id
				writer.Write((int)1678849575); // session cookie id
				writer.Write((long)_account.Id); // account id
				writer.Write(BinaryExt.WriteFixedString(_account.FamilyName, Encoding.Unicode, 62)); // family name
				writer.Write((short)-1);
				writer.Write((short)0);
				writer.Write((byte)_characters.Count); // equal to characters count
				writer.Write((byte)0);
				writer.Write((long)-1);
				writer.Write((long)0);
				writer.Write((byte)_characters.Count); // characters count

				for (int index = 0; index < _characters.Count; index++)
                {
                    var characterInfo = _characters[index];
                    var equipment = (EquipmentStorage)characterInfo.EquipmentData;

                    writer.Write((short)characterInfo.ClassType.Ordinal()); // class
                    writer.Write((long)characterInfo.CharacterId); // id
                    writer.Write((byte)0); // slot
                    writer.Write(BinaryExt.WriteFixedString(characterInfo.CharacterName, Encoding.Unicode, 62)); // name
                    writer.Write((int)characterInfo.Level); // level
                    writer.WriteD(0); // item cache related

					using (equipment)
						if (equipment.Items.Count != 0)
						{
							byte[] equipmentData = equipment.GetEquipmentData();
							writer.Write(equipmentData);
							writer.Skip(107);
						}
						else
						{
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
						}

                    writer.Write(characterInfo.AppearancePresets); // 10 bytes
                    writer.Write((byte)characterInfo.Zodiac);
                    writer.Write(characterInfo.AppearanceOptions); // 800 bytes
                    writer.Skip(62); // family name
					writer.Write((long)-1); // character delete date
					writer.Write((byte)0); // character state
					writer.Write((long)-1); // character last login date
					writer.Write((long)-1); // character creation date
					writer.Write((float)0); // character location x
					writer.Write((float)0); // character location y
					writer.Write((float)0); // character location z
					writer.Write((long)-1); // character suspended date
					writer.Write((short)0); // character zone id
					writer.Write((long)-1); // character unk date
					writer.Write(new byte [32]);
                }

                return stream.ToArray();
            }
        }
    }  
}
