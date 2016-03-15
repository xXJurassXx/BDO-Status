using System.IO;
using Commons.Enums;
using Commons.Utils;
using WorldServer.Emu.Models.Creature.Player;
using WorldServer.Emu.Models.Storages;
/*
   Author: Sagara, RBW
*/
namespace WorldServer.Emu.Networking.Handling.Frames.Send
{
    public class SMSG_GetAllEquipSlot : APacketProcessor
    {
        private readonly Player _player;
        private BinaryWriter _writer;
        public SMSG_GetAllEquipSlot(Player player)
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
        
        protected void WriteEquipedItem(EquipmentStorage storage, EquipSlot slot)
        {
            if (storage.IsEquip(slot))
            {
                var item = storage.GetItemByEquipSlot(slot);

                _writer.Write((short)item.ItemId); // id
                _writer.Write((short)0); // enchant
                _writer.Write((long)1); // count
				_writer.Write((long)-1); // expiration
				_writer.Write((byte)0); // unk
				_writer.Write((long)0); // price maybe
				_writer.Write((short)1); // unk
				_writer.Write((short)50); // cur endurance
				_writer.Write((short)50); // max endurance
				_writer.Write((long)1); // obj id
				_writer.WriteB("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"); // dye data - 12 * 2
				_writer.Write((int)0); // unk
				_writer.Write((byte)0); // unk
				_writer.WriteB("000000000000000000000000000000000000000000000000"); // jewel/crystal data - 6 * 4
            }
            else //GAG
                _writer.WriteB("0000000000C0C200000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF00001010441501000000FEFFFFFFFFFFFFFF00000000000000005F978756000000001010441501000000A96BD0ED85BE3F01000000");
            
        }
    }
}
