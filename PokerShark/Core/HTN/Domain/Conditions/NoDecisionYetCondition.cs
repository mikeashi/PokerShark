using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.Core.HTN.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.HTN.Domain.Conditions
{
    public class NoDecisionYetCondition : ICondition<Object>
    {
        public string Name { get; } = "If There Is No Decision Yet";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                var decision = c.GetDecision();
                if (decision.Fold == 0 && decision.Call == 0 && decision.Raise == 0)
                    return true;
                return false;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
