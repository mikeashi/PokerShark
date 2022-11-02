using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.AI.HTN.Utility;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class TooRiskyCall : ICondition<Object>
    {
        public string Name { get; } = "if call is too risky";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                var stack = c.GetCurrentStack();
                var initialStack = c.GetGame().InitialStack;
                var call = c.GetCallAmount();
                var attitude = c.GetAttitude();

                if (call <= c.GetGame().BigBlind * 2)
                    return false;

                if (attitude is RiskNeutral)
                {
                    return call > stack / 6;
                }
                else if (attitude is RiskAverse)
                {
                    return call > stack / 8;
                }
                else if (attitude is RiskSeeking)
                {
                    if (initialStack < stack)
                        return call > stack / 4;
                    return call > stack / 5;
                }
                else
                {
                    throw new Exception("Invalid attitude");
                }
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
