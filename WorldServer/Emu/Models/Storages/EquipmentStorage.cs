using System;
using System.Collections.Generic;
using System.IO;
using Commons.Enums;
using Commons.Utils;
using WorldServer.Emu.Extensions;
using WorldServer.Emu.Models.Storages.Abstracts;

namespace WorldServer.Emu.Models.Storages
{
    public class EquipmentStorage : AStorage, IDisposable
    {
        public EquipmentStorage(Dictionary<short, AStorageItem> itemDatas, short storageSize) : base(itemDatas, storageSize)
        {
        }

        public override StorageType StorageType => StorageType.Equipment;

        public override short[] GetSlots(AStorageItem item)
        {
            var type = (EquipType)Enum.Parse(typeof(EquipType), item.Template.EquipType);

            return new[] { (short)EnumExt.GetSlotByEquipType(type) };
        }

        public override bool AddItem(AStorageItem item, short targetSlot)
        {
            short[] slot = GetSlots(item);

            if (!IsSlotValid(targetSlot))
                return false;

            if (slot.Length == 1)
            {
                if (!IsSlotValid(targetSlot) || StorageItems[targetSlot] != null)
                    return false;

                StorageItems[targetSlot] = item;
            }
            return true;
        }

        public override bool RemoveItem(short fromSlot)
        {
            fromSlot--;
            if (!IsSlotValid(fromSlot) || StorageItems[fromSlot] == null)
                return false;

            AStorageItem item = StorageItems[fromSlot];
            //TODO send update inventory packet
            StorageItems[fromSlot] = null;

            return true;
        }

        public AStorageItem GetItemByEquipType(EquipType type)
        {
            var slot = EnumExt.SlotByEquipType(type);

            return StorageItems[slot]; 
        }

        public AStorageItem GetItemByEquipSlot(EquipSlot slot)
        {
            return StorageItems[slot.GetHashCode()];
        }

        public int GetItemIdByEquipType(EquipType type)
        {
            var target = GetItemByEquipType(type);
            if (target != null)
                return target.ItemId;

            return 0;
        }

        public int GetItemIdBySlot(EquipSlot slot)
        {
            var target = StorageItems[slot.GetHashCode()];
            if (target != null)
                return target.ItemId;

            return 0;
        }

        public byte[] GetEquipmentData()
        {
            byte[] data;
            byte[] gag = "FEFFFFFFFFFFFFFF6400FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF".ToBytes();
            byte[] avatarGag = "ffffffffffffffffff7f0155ffff0125017cffffffffffffffffffffffffffffffff".ToBytes();

            #region stream builder

            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(GetItemIdBySlot(EquipSlot.RightHand));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.LeftHand));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.SubTool));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Chest));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Glove));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Boots));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Helm));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Necklace));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Ring1));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Ring2));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Earing1));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Earing2));
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.Belt));
                writer.Write(gag);
                writer.Write(0);
                writer.Write(gag);
                writer.Write(GetItemIdBySlot(EquipSlot.AvatarChest));
                writer.Write(avatarGag);
                writer.Write(GetItemIdBySlot(EquipSlot.AvatarGlove));
                writer.Write(avatarGag);
                writer.Write(GetItemIdBySlot(EquipSlot.AvatarBoots));
                writer.Write(avatarGag);
                writer.Write(GetItemIdBySlot(EquipSlot.AvatarHelm));
                writer.Write(avatarGag);
                writer.Write(GetItemIdBySlot(EquipSlot.AvatarWeapon));
                writer.Write(avatarGag);
                writer.Write(GetItemIdBySlot(EquipSlot.AvatarSubWeapon));
                writer.Write(avatarGag);
                writer.Write(GetItemIdBySlot(EquipSlot.AvatarUnderWear));
                writer.Write(avatarGag);
                writer.Write(0);
                writer.Write(gag);
                writer.Write(0);
                writer.Write(gag);
                writer.Write(0);
                writer.Write(gag);
                writer.Write(0);
                writer.Write(gag);
                writer.Write(0);
                writer.Write(gag);
                writer.Write(0);
                writer.Write(gag);
                writer.Write(0);
                writer.Write(gag);
                writer.Write(0);
                writer.Write(gag);

                data = ms.ToArray();
            }

            #endregion

            return data;
        }

        public bool IsEquip(EquipSlot type)
        {
            var targetItem = StorageItems[type.GetHashCode()];
            if (targetItem != null)
                return true;

            return false;
        }

        public void Dispose()
        {
            Items.Clear();
            Log.Debug($"Temporary instance {GetType().Name} destroyed.");
        }
    }
}
