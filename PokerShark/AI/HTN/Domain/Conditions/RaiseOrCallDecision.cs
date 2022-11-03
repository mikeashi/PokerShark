using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class RaiseOrCallDecision : ICondition<Object>
    {
        public string Name { get; } = "if call decision is made";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {

                var decision = c.GetDecision();
                if (decision.Call > decision.Fold || decision.Raise > decision.Fold)
                    return true;
                return false;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
