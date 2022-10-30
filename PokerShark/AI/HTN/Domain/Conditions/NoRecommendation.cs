using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class NoRecommendation : ICondition<Object>
    {
        public string Name { get; } = "if no recommendation is made yet";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                var decision = c.GetRecomanndedDecision();
                if (decision.Fold == 0 && decision.Call == 0 && decision.Raise == 0)
                    return true;
                return false;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
