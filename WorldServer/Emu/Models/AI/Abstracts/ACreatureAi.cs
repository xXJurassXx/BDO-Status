using WorldServer.Emu.Structures.Geo.Basics;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.AI.Abstracts
{
    public abstract class ACreatureAi : AVisibleObjectAi
    {
        protected ACreatureAi(ABdoObject owner, float visibleRange) : base(owner, visibleRange)
        {
        }

        protected override sealed bool CanSee(ABdoObject target)
        {
            return true;
        }

        protected virtual void Rotate(Position target, int time)
        {

        }
    }
}
