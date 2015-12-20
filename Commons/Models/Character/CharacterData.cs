using System;
using Commons.Enums;

namespace Commons.Models.Character
{
    public class CharacterData
    {
        public virtual int AccountId { get; set; }
        public virtual uint CharacterId { get; set; }
        public virtual int Level { get; set; }
        public virtual byte[] AppearancePresets { get; set; }
        public virtual byte[] AppearanceOptions { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime DeletionDate { get; set; }
        public virtual DateTime PunishmentEnd { get; set; }
        public virtual DateTime LastOnline { get; set; }
        public virtual string CharacterName { get; set; }
        public virtual string Surname { get; set; }
        public virtual ClassType ClassType { get; set; }
        public virtual Zodiac Zodiac { get; set; }
        public virtual float PositionX { get; set; }
        public virtual float PositionY { get; set; }
        public virtual float PositionZ { get; set; }
        public virtual byte Face { get { return AppearancePresets[0]; } }
        public virtual byte Hair { get { return AppearancePresets[1]; } }
        public virtual byte Goatee { get { return AppearancePresets[2]; } }
        public virtual byte Mustache { get { return AppearancePresets[3]; } }
        public virtual byte Sideburns { get { return AppearancePresets[4]; } }
    }
}
