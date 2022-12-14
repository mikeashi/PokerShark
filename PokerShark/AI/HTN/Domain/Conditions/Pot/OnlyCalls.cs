using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Pot
{
    internal class OnlyCalls : ICondition<Object>
    {
        public string Name { get; } = "If only calls";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return !c.GetHistory().Any(action => action.Type == Poker.ActionType.Raise);
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
