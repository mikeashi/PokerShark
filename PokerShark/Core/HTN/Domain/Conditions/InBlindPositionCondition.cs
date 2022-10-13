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
    public class InBlindPositionCondition : ICondition<Object>
    {
        public string Name { get; } = "If In Blind Position";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                return (c.GetPosition() == PyPoker.Position.SmallBlind) || (c.GetPosition() == PyPoker.Position.BigBlind);
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
