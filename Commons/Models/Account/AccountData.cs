using System;

namespace Commons.Models.Account
{
    public class AccountData
    {
        public virtual int Id { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual string FamilyName { get; set; }

        public virtual int AccessLevel { get; set; }

        public virtual string PinCode { get; set; }

        public virtual int Money { get; set; }

        public virtual string Token { get; set; }

        public virtual string GameHash { get; set; }

        public virtual int MaxSlotCount { get; set; }

        public virtual int OpenedSlotCount { get; set; }

        public virtual DateTime ExpireTime { get; set; }
    }
}
