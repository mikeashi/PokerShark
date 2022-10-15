using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.PyPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.HTN.Domain.Conditions
{
    public class TwoOrMoreRaisesCondition : ICondition<Object>
    {
        public string Name { get; } = "If Two Or More Raises Condition";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                return c.GetCurrentRound().ActionHistory.Count(a => a is RaiseAction) > 1;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
