using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions.Pot
{
    internal class FirstToPot : ICondition<Object>
    {
        public string Name { get; } = "If no calls or raises yet";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return !c.GetHistory().Any(action => action.Type == Poker.ActionType.Call || action.Type == Poker.ActionType.Raise);
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
