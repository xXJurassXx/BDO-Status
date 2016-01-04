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
                _writer.Write("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000".ToBytes());
                _writer.Write("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000".ToBytes());
                _writer.Write("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000".ToBytes());
                _writer.Write("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000".ToBytes());
                _writer.Write("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000".ToBytes());
                _writer.Write("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000".ToBytes());
                _writer.Write("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000".ToBytes());
                _writer.Write("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000".ToBytes());

                return stream.ToArray();
            }
        }
        //0000000012000000f10bb8740000000059e7994b0f00000070808b0204000000000000ffffffffffffffff000000000000e1f505000000002a8000000000000000000000000000000000000000000000000000000000000000000000000000
        protected void WriteEquipedItem(EquipmentStorage storage, EquipSlot slot)
        {
            if (storage.IsEquip(slot))
            {
                var item = storage.GetItemByEquipSlot(slot);
                _writer.WriteH(item.ItemId);
                _writer.WriteH(0); //Enchant
                _writer.WriteQ(1); //Cannot trade
                _writer.WriteB("FFFFFFFFFFFFFFFF0000000000000000000100" +
                               "3200" + //current health
                               "3200" + //max health
                               "A72CE56FF2862300FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000000000000000000000000000000000000000000000000000");
            }
            else //GAG
                _writer.WriteB("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000");
            
        }
    }
}
