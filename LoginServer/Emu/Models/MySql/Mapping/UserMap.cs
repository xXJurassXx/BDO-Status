using Commons.Models.Account;
using FluentNHibernate.Mapping;

namespace LoginServer.Emu.Models.MySql.Mapping
{
    public class UserMap : ClassMap<AccountData>
    {
        public UserMap()
        {
            Table("bd_accounts");

            LazyLoad();

            Id(x => x.Id, "a_id").GeneratedBy.Identity();

            Map(x => x.Login, "a_user_name").Not.Nullable().Length(32).Unique();
            Map(x => x.FamilyName, "a_user_family_name").Not.Nullable().Length(32).Unique();
            Map(x => x.Password, "a_password").Not.Nullable();
            Map(x => x.Token, "a_token").Not.Nullable();
            Map(x => x.AccessLevel, "a_permission").Not.Nullable();
            Map(x => x.PinCode, "a_pin").Not.Nullable();
            Map(x => x.GameHash, "a_game_hash").Not.Nullable();
            Map(x => x.MaxSlotCount, "a_max_slot_count").Not.Nullable();
            Map(x => x.OpenedSlotCount, "a_opened_slot_count").Not.Nullable();
            Map(x => x.ExpireTime, "a_token_expire_time").Not.Nullable();
        }
    }
}
