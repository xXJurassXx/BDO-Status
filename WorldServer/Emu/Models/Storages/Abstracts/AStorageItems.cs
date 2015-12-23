using WorldServer.Emu.Data;
using WorldServer.Emu.Data.Templates;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.Storages.Abstracts
{
    public abstract class AStorageItem : ABdoObject
    {
        public int ItemId;

        public int Count;

        public ItemTemplate Template;

        protected AStorageItem(ObjectFamily family, int itemId, int count = 1) : base(family)
        {
            ItemId = itemId;
            Count = count;
            Template = DataLoader.Items.TemplatesById[itemId];
        }
    }
}
