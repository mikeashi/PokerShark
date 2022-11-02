using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.AI.HTN.Utility;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class RiskNeutralAttitude : ICondition<Object>
    {
        public string Name { get; } = "if bot attitude is risk neutral";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetAttitude() is RiskNeutral;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
