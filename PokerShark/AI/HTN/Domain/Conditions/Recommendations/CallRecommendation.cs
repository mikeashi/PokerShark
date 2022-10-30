using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class CallRecommendation : ICondition<Object>
    {
        public string Name { get; } = "if call recommendation is made";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                var decision = c.GetRecomanndedDecision();
                if (decision.Call >= decision.Fold && decision.Call >= decision.Raise)
                    return true;
                return false;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
