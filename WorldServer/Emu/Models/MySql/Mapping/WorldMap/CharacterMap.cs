using Commons.Models.Character;
using FluentNHibernate.Mapping;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.MySql.Mapping.WorldMap
{
    public class CharacterMap : ClassMap<CharacterData>
    {
        public CharacterMap()
        {
           
            Table("bd_characters");

            LazyLoad();

            Id(x => x.CreatedId, "c_created_id").Not.Nullable();
            Map(x => x.AccountId, "c_account_id").Not.Nullable();
            Map(x => x.CharacterId, "c_character_id").Not.Nullable();
            Map(x => x.Level, "c_character_level").Not.Nullable();
            Map(x => x.AppearancePresets, "c_character_appearance_presets").Not.Nullable();
            Map(x => x.AppearanceOptions, "c_character_appearance_options").Not.Nullable();
            Map(x => x.CreationDate, "c_character_creation_date").Not.Nullable();
            Map(x => x.DeletionDate, "c_character_deletion_date").Not.Nullable();
            Map(x => x.PunishmentEnd, "c_character_punishment_end").Not.Nullable();
            Map(x => x.LastOnline, "c_character_last_online").Not.Nullable();
            Map(x => x.CharacterName, "c_character_name").Not.Nullable();
            Map(x => x.Surname, "c_character_surname").Not.Nullable();
            Map(x => x.ClassType, "c_character_class").Not.Nullable();
            Map(x => x.Zodiac, "c_character_zodiac").Not.Nullable();
            Map(x => x.PositionX, "c_character_position_x").Not.Nullable();
            Map(x => x.PositionY, "c_character_position_y").Not.Nullable();
            Map(x => x.PositionZ, "c_character_position_z").Not.Nullable();
        }
    }
}
