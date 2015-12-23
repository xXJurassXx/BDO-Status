using System;
using System.Collections.Generic;
using Commons.Enums;
using NLog;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.Storages.Abstracts
{
    public abstract class AStorage
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected AStorageItem[] StorageItems;

        public abstract StorageType StorageType { get; }

        public abstract short[] GetSlots(AStorageItem item);

        public abstract bool AddItem(AStorageItem item, short targetSlot);

        public abstract bool RemoveItem(short fromSlot);

        public virtual short Size
        {
            get { return (short)StorageItems.Length; }
            set { Array.Resize(ref StorageItems, value); }
        }

        /// <param name="itemDatas">Items collection</param>
        /// <param name="storageSize">Items collection size</param>
        protected AStorage(Dictionary<short, AStorageItem> itemDatas, short storageSize)
        {
            StorageItems = new AStorageItem[storageSize];

            if (itemDatas == null)
                return;

            foreach (var item in itemDatas)
                StorageItems[item.Key - 1] = item.Value;
        }

        /// <summary>
        /// Return items collection, slot (game slot, NOT internal index) and item
        /// </summary>
        public Dictionary<short, AStorageItem> Items
        {
            get
            {
                var itms = new Dictionary<short, AStorageItem>();
                for (short i = 0; i < StorageItems.Length; i++)
                    if (StorageItems[i] != null)
                        itms.Add((short)(i + 1), StorageItems[i]);

                return itms;
            }
        }

        /// <summary>
        /// Check slot for valid
        /// </summary>
        /// <param name="slot">Slot id</param>
        /// <returns></returns>
        public bool IsSlotValid(short slot)
        {
            return StorageItems.Length > slot && slot >= 0;
        }

        /// <summary>
        /// Get slot id by item
        /// </summary>
        /// <param name="item">Storage item</param>
        /// <returns></returns>
        public short GetSlotOfItem(AStorageItem item)
        {
            return (short)Array.IndexOf(StorageItems, item);
        }
    }
}
