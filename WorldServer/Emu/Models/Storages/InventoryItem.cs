using WorldServer.Emu.Models.Storages.Abstracts;

namespace WorldServer.Emu.Models.Storages
{
    public class InventoryItem : AStorageItem
    {
        public InventoryItem(int itemId, int count = 1) : base(ObjectFamily.InventoryItem, itemId, count)
        {
        }
    }
}
