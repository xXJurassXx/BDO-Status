using Commons.Models.Character;
using FluentNHibernate.Mapping;

namespace WorldServer.Emu.Models.MySql.Mapping.WorldMap
{
    public class StorageMap : ClassMap<CharacterItem>
    {
        public StorageMap()
        {
            Table("bd_items");

            LazyLoad();

            Id(s => s.ItemUid, "i_item_uid");
            Map(x => x.CharacterId, "i_character_id").Not.Nullable();
            Map(x => x.ItemId, "i_item_id").Not.Nullable();
            Map(x => x.Count, "i_item_count").Not.Nullable();
            Map(x => x.Slot, "i_item_slot").Not.Nullable();
            Map(x => x.StorageType, "i_storage_type").Not.Nullable();
        }
    }
}
