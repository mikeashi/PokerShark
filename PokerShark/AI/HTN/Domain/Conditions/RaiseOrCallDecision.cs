using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
