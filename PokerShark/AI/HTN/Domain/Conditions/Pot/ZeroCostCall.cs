using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Pot
{
    internal class ZeroCostCall : ICondition<Object>
    {
        public string Name { get; } = "If call amount == 0";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetCallAmount() == 0;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
