using System.Collections.Generic;
using System.Linq;
using Commons.Enums;
using WorldServer.Configs;
using WorldServer.Emu.Models.Storages.Abstracts;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.Storages
{
    public class InventoryStorage : AStorage
    {
        public InventoryStorage(Dictionary<short, AStorageItem> itemDatas, short storageSize) : base(itemDatas, storageSize)
        {
        }

        public override StorageType StorageType => StorageType.Inventory;

        /// <summary>
        /// Get available slots for item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override short[] GetSlots(AStorageItem item)
        {
            var slots = new List<short>();
            var counter = item.Count;

            for (short i = 0; i < StorageItems.Length; i++)
            {
                if (StorageItems[i] == null)
                {
                    slots.Add(i);
                    break;
                }

                if (item.Template.MaxStack == 0 || item.ItemId != StorageItems[i].ItemId ||
                    StorageItems[i].Count >= item.Template.MaxStack) continue;
                counter -= (short)(item.Template.MaxStack - StorageItems[i].Count);
                slots.Add(i);

                if (counter <= 0)
                    break;
            }

            if (item.Template.MaxStack != 0 && counter > 0)
                return null;

            return slots.ToArray();
        }

        /// <summary>
        /// Add item in storage
        /// </summary>
        /// <param name="item"></param>
        /// <param name="targetSlot"></param>
        /// <returns></returns>
        public override bool AddItem(AStorageItem item, short targetSlot)
        {
            short[] targetSlots = null;
            if (targetSlot == -1)
            {
                var sl = GetSlots(item);
                if (sl != null && sl.Length > 0)
                    targetSlots = sl;
            }
            else
                targetSlots = new[] { targetSlot };

            if (targetSlots == null || !targetSlots.All(IsSlotValid))
                return false;

            int initialCount = item.Count;

            foreach (short slot in targetSlots)
            {
                if (StorageItems[slot] != null)
                {
                    short avail = (short)(StorageItems[slot].Template.MaxStack - StorageItems[slot].Count);
                    if (avail >= initialCount)
                    {
                        StorageItems[slot].Count += initialCount;
                        break;
                    }

                    StorageItems[slot].Count = (short)StorageItems[slot].Template.MaxStack;
                    initialCount -= avail;
                }
                else
                {
                    StorageItems[slot] = item;
                }
            }

            return true;
        }

        /// <summary>
        /// Remove item from storage by slot id
        /// </summary>
        /// <param name="fromSlot"></param>
        /// <returns></returns>
        public override bool RemoveItem(short fromSlot)
        {
            fromSlot--;
            if (!IsSlotValid(fromSlot) || StorageItems[fromSlot] == null)
                return false;

            var item = StorageItems[fromSlot]; //removed item   
            StorageItems[fromSlot] = null;
            //TODO send update inventory packet
            return true;
        }

        public static InventoryStorage GetDefault(ClassType profession)
        {
            var items = new Dictionary<short, AStorageItem>();

            items.Add(1, new InventoryItem(1, CfgCore.Default.StartedMoney));
            switch (profession)
            {
                    
            } 

            return new InventoryStorage(items, 48);
        }
    }
}
