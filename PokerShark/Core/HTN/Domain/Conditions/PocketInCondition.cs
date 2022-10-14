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
    public class PocketInCondition : ICondition<Object>
    {
        public string Name { get; } = "If Pocket in Hands";
        public List<PocketHand> Hands { get; set; }

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                var pocket = c.GetPocket();
                foreach (var hand in Hands)
                {
                    if (hand.Equals(pocket)) return true;
                }
                return false;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
