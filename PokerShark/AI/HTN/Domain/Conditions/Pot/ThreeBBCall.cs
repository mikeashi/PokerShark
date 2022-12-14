using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Pot
{
    internal class ThreeBBCall : ICondition<Object>
    {
        public string Name { get; } = "If call amount >= 3 BB";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetCallAmount() >= c.GetGame().BigBlind * 3;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
