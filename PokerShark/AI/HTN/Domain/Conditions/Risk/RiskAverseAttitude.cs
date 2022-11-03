using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.AI.HTN.Utility;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class RiskAverseAttitude : ICondition<Object>
    {
        public string Name { get; } = "if bot attitude is risk averse";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetAttitude() is RiskAverse;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
