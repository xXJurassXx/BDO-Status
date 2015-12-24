using System.IO;
using Commons.Enums;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
using WorldServer.Emu.Models.Storages;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SpCharacterEquipment : APacketProcessor
    {
        private readonly Player _player;
        private BinaryWriter _writer;
        public SpCharacterEquipment(Player player)
        {
            _player = player;
        }

        public override byte[] WritedData()
        {
            using (var stream = new MemoryStream())
            using (_writer = new BinaryWriter(stream))
            {
                var storage = _player.Equipment;

                _writer.WriteC(1);

                WriteEquipedItem(storage, EquipSlot.LeftHand);
                WriteEquipedItem(storage, EquipSlot.RightHand);
                WriteEquipedItem(storage, EquipSlot.SubTool);
                WriteEquipedItem(storage, EquipSlot.Chest);
                WriteEquipedItem(storage, EquipSlot.Glove);
                WriteEquipedItem(storage, EquipSlot.Boots);
                WriteEquipedItem(storage, EquipSlot.Helm);
                WriteEquipedItem(storage, EquipSlot.Necklace);
                WriteEquipedItem(storage, EquipSlot.Ring1);
                WriteEquipedItem(storage, EquipSlot.Ring2);
                WriteEquipedItem(storage, EquipSlot.Earing1);
                WriteEquipedItem(storage, EquipSlot.Earing2);
                WriteEquipedItem(storage, EquipSlot.Belt);
                WriteEquipedItem(storage, EquipSlot.Lantern);
                WriteEquipedItem(storage, EquipSlot.AvatarChest);
                WriteEquipedItem(storage, EquipSlot.AvatarGlove);
                WriteEquipedItem(storage, EquipSlot.AvatarBoots);
                WriteEquipedItem(storage, EquipSlot.AvatarHelm);
                WriteEquipedItem(storage, EquipSlot.AvatarWeapon);
                WriteEquipedItem(storage, EquipSlot.AvatarSubWeapon);
                WriteEquipedItem(storage, EquipSlot.AvatarUnderWear);
                WriteEquipedItem(storage, EquipSlot.FaceDecoration3);
                WriteEquipedItem(storage, EquipSlot.FaceDecoration1);
                WriteEquipedItem(storage, EquipSlot.FaceDecoration2);

                /*TODO find other*/
                _writer.Write("00000000F80000F80000000047549A1AFD7F0000000000000000000020000000000000FFFFFFFFFFFFFFFF000054C92D19000000FF0300000000000000006902000000001067000000000000E774E51DFD7F000098009596020000001F2001".ToBytes());
                _writer.Write("0000000000000000001700000018000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF000000000000000000000000000000000000D0009596020000000100000002000000000000000000000000000000000000009800".ToBytes());
                _writer.Write("00000000000010EEF90600000000980095960200000000D46F54190000000085145BF6FFFFFFFFFFFFFFFF00000000D46F541900000075C0AD840300000000006902000000000000E21DFD7F000010000000000000000054C92D190000006F".ToBytes());
                _writer.Write("000000007F000000006902000000001054C92D19000000000000000000000002000000FFFFFFFFFFFFFFFF0000000098009596020000000000000000000000462C5D5BF67F000010E4322017000000030000000200000000000000F10B0000".ToBytes());
                _writer.Write("000000000000000075C0AD84030000000000000000000000000000000000000020BBC9FFFFFFFFFFFFFFFF020100000059E532201700000018E5322017000000948D236D000000000000000000000000980095960200000000000000000000".ToBytes());
                _writer.Write("00000000000000000018E53220170000006A96075BF67F000020BBC92D00000000F0E4FFFFFFFFFFFFFFFF00000000000010E0350201000000FEFFFFFFFFFFFFFF20BBC92D1900000020BBC92D190000006900165BF67F000018E532201700".ToBytes());
                _writer.Write("00000000C92D19000000F10B35020100000059E532201700000070C0AD000000000000FFFFFFFFFFFFFFFF0000000000000000E1F505F10B000000000000000000000000000000000000000000000000000000000000000000000000000000".ToBytes());
                _writer.Write("00000000C2000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF10E0350201000000FEFFFFFFFFFFFFFF0000000000000000F721AF590000000010E0350201000000000000000000000000000000".ToBytes());

                return stream.ToArray();
            }
        }

        protected void WriteEquipedItem(EquipmentStorage storage, EquipSlot slot)
        {
            if (storage.IsEquip(slot))
            {
                var item = storage.GetItemByEquipSlot(slot);
                _writer.WriteH(item.ItemId);
                _writer.WriteH(0); //Enchant
                _writer.WriteQ(1); //Cannot trade
                _writer.WriteB("FFFFFFFFFFFFFFFF000000000000000000010031003200A72CE56FF2862300FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000000000000000000000000000");
            }
            else //GAG
                _writer.WriteB("0000000012000000f10bb8740000000059e7994b0f00000070808b0204000000000000ffffffffffffffff000000000000e1f505000000002a8000000000000000000000000000000000000000000000000000000000000000000000000000");
            
        }
    }
}
