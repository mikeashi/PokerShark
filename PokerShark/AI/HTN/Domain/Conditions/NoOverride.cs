using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
