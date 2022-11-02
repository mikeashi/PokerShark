using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class FoldRecommendation : ICondition<Object>
    {
        public string Name { get; } = "if fold recommendation is made";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                var decision = c.GetRecomanndedDecision();
                if (decision.Fold >= decision.Call && decision.Fold >= decision.Raise)
                    return true;
                return false;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
