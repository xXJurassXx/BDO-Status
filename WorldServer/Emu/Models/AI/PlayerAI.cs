using WorldServer.Emu.Models.AI.Abstracts;
using WorldServer.Emu.Structures.Geo.Basics;
/*
   Author:Sagara
*/
namespace WorldServer.Emu.Models.AI
{
    public sealed class PlayerAI : ACreatureAi
    {
        public PlayerAI(ABdoObject owner, float visibleRange) : base(owner, visibleRange)
        {
        }

        protected override void IamSeeSomeone(ABdoObject someone)
        {            
        }

        protected override void IamUnseeSomeone(ABdoObject someone)
        {            
        }

        protected override void SomeoneSeeMe(ABdoObject someone)
        {          
        }

        protected override void SomeoneUnseeMee(ABdoObject someone)
        {          
        }

        protected override void SomeoneThatIamSeeWasMoved(ABdoObject someone, MovementAction action)
        {          
        }

        protected override void SomeoneThatSeeMeWasMoved(ABdoObject someone, MovementAction action)
        {
        }
    }
}
