using Commons.Enums;
using WorldServer.Emu.Models.Storages.Abstracts;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.Storages
{
    public class InventoryItem : AStorageItem
    {
        public StorageType StorageType;

        public InventoryItem(int itemId, int count = 1) : base(ObjectFamily.InventoryItem, itemId, count)
        {
        }
    }
}
