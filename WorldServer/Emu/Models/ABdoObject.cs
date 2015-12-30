using System;
using System.Collections.Generic;
using Commons.UID;
using NLog;
using WorldServer.Emu.Models.AI.Abstracts;
using WorldServer.Emu.Structures.Geo;
using WorldServer.Emu.Structures.Geo.Basics;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models
{
    public abstract class ABdoObject
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Object family
        /// </summary>
        public ObjectFamily Family { get; }

        /// <summary>
        /// Object id on factory
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Object unique id on factory
        /// </summary>
        public long Uid { get; private set; }

        /// <summary>
        /// Current object Area
        /// </summary>
        public Area Area { get; set; }

        /// <summary>
        /// Object position in world
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Object AI
        /// </summary>
        public ACreatureAi Ai { get; protected set; }

        /// <summary>
        /// Cast object AI as Visible AI
        /// </summary>
        public AVisibleObjectAi VisibleAi => Ai;

        /// <summary>
        /// Object factories
        /// </summary>
        private static readonly Dictionary<ObjectFamily, UInt32UidFactory> Factories = new Dictionary<ObjectFamily, UInt32UidFactory>();

        static ABdoObject()
        {
            var enumEntries = Enum.GetValues(typeof(ObjectFamily));
            foreach (ObjectFamily enumEntry in enumEntries)
                Factories.Add(enumEntry, new UInt32UidFactory());
        }

        protected ABdoObject(ObjectFamily family)
        {
            Family = family;

            Id = Factories[Family].Next();
            Uid = Id | (long)family.GetHashCode() << 32;
        }

        public virtual void Release()
        {
            Factories[Family].ReleaseUniqueInt(Id);
        }

        public T Cast<T>() where T : ABdoObject
        {
            return (T)this;
        }

        public enum ObjectFamily
        {
            Player = 0x00,
            Monster = 0x11,
            Npc = 0x22,
            Vehicle = 0x33,
            Gate = 0x44,
            Alterego = 0x05,
            Collect = 0x66,
            Household = 0x77,
            Installation = 0x88,
            Deadbody = 0x99,
            Skill = 0x100,
            Request = 0x111,
            Item = 0x222,
            InventoryItem = 0x333,
            Projectile = 0x444,
        }
    }
}
