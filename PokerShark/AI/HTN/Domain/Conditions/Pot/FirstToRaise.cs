using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Pot
{
    internal class FirstToRaise : ICondition<Object>
    {
        public string Name { get; } = "If no raises yet";

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
