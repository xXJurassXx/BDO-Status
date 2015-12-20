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
    }
}
