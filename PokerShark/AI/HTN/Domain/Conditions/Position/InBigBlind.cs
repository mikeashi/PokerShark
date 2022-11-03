using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Position
{
    internal class InBigBlind : ICondition<Object>
    {
        public string Name { get; } = "If in big blind position";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetPosition() == Poker.Position.BigBlind;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
