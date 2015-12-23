/*
   Author:Sagara
*/
namespace Commons.Models.Character
{
    public class CharacterItem
    {
        public virtual int ItemUid { get; set; }
    
        public virtual int CharacterId { get; set; }

        public virtual int ItemId { get; set; }

        public virtual int Count { get; set; }

        public virtual int Slot { get; set; }

        public virtual int StorageType { get; set; }
    }
}
