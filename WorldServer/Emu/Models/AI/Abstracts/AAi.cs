using NLog;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.AI.Abstracts
{
    public abstract class AAi
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ABdoObject Owner { get; private set; }

        protected AAi(ABdoObject owner)
        {
            Owner = owner;
        }

        public abstract void Activate();

        public abstract void Deactivate();

        protected abstract void Action();
    }
}
