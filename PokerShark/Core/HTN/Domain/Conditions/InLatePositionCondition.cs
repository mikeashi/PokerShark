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
    public class InLatePositionCondition : ICondition<Object>
    {
        public string Name { get; } = "If In Late Position";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                return c.GetPosition() == PyPoker.Position.Late;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
