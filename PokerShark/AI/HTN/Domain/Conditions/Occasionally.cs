using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class Occasionally : ICondition<Object>
    {
        public string Name { get; } = "Randomly true less than 30% of the time";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                if (new Random().Next(1, 11) < 4)
                {
                    return true;
                }
                return false;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
