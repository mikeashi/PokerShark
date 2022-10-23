using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions.Position
{
    internal class InSmallBlind : ICondition<Object>
    {
        public string Name { get; } = "If in small blind position";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetPosition() == Poker.Position.SmallBlind;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
