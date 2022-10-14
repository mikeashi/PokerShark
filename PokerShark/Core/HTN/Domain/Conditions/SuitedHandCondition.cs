using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.HTN.Domain.Conditions
{
    public class SuitedHandCondition : ICondition<Object>
    {
        public string Name { get; } = "If Hand is suited";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                var pocket = c.GetPocket();
                return pocket[0].Suit == pocket[1].Suit;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
