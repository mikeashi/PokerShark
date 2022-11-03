using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class NoOverride : ICondition<Object>
    {
        public string Name { get; } = "if cut override flag is not set";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                return !c.OverrideCut;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
