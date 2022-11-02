using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Round
{
    internal class InPreflop : ICondition<Object>
    {
        public string Name { get; } = "If Round stage is preflop";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetRoundState() == Poker.RoundState.Preflop;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
