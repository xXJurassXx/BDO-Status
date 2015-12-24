using Commons.Enums;

namespace WorldServer.Emu.Extensions
{
    public static class EnumExt
    {
        public static int GetOrdinal(this ClassType classType)
        {
            return ((int)classType / 4) + 1;
        }

        public static int Ordinal(this ClassType type)
        {
            switch (type)
            {
                case ClassType.Warrior:
                    return 1;
                case ClassType.Ranger:
                    return 2;
                case ClassType.Sorcerer:
                    return 3;
                case ClassType.Giant:
                    return 4;
                case ClassType.Tamer:
                    return 5;
                case ClassType.BladeMaster:
                    return 21;
                case ClassType.BladeMasterWomen:
                    return 22;
                case ClassType.Valkyrie:
                    return 25;
                case ClassType.Wizard:
                    return 29;
                case ClassType.Kunoichi:
                    return 26;
                case ClassType.WizardWomen:
                    return 32;
            }
            return 0;
        }

        public static int SlotByEquipType(EquipType slot)
        {
            return GetSlotByEquipType(slot).GetHashCode();
        }

        public static EquipSlot GetSlotByEquipType(EquipType type)
        {
            //Todo:others
            switch (type)
            {
                case EquipType.Sword:
                case EquipType.TwinBlades:
                case EquipType.Katana:
                case EquipType.Axe:
                case EquipType.Bow:
                case EquipType.Talisman:
                    return EquipSlot.LeftHand; //weapon slot
                case EquipType.SubWeapon:
                case EquipType.Dagger:
                    return EquipSlot.RightHand; //subweapon slot
                case EquipType.WeaponStyle:
                    return EquipSlot.AvatarWeapon;
                case EquipType.SubWeaponStyle:
                    return EquipSlot.AvatarSubWeapon;
                case EquipType.Upperbody:
                    return EquipSlot.Chest;
                case EquipType.UpperbodyStyle:
                    return EquipSlot.AvatarChest;
                case EquipType.Underwear:
                    return EquipSlot.AvatarUnderWear;
                case EquipType.Ring:
                    return EquipSlot.Ring1;//todo ring2 too
                case EquipType.Necklace:
                    return EquipSlot.Necklace;
                case EquipType.Head:
                    return EquipSlot.Helm;
                case EquipType.HeadStyle:
                    return EquipSlot.AvatarHelm;
                case EquipType.Hand:
                    return EquipSlot.Glove;
                case EquipType.HandStyle:
                    return EquipSlot.AvatarGlove;
                case EquipType.Foot:
                    return EquipSlot.Boots;
                case EquipType.FootStyle:
                    return EquipSlot.AvatarBoots;
                case EquipType.Accessory:
                    return EquipSlot.FaceDecoration1;//todo decorations
            }
            return EquipSlot.None;
        }
    }
}
