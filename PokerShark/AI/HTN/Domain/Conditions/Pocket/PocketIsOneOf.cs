using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions.Pockt
{
    internal class PocketIsOneOf : ICondition<Object>
    {
        public string Name { get; } = "If pocket is one of the given cards";
        public List<Pocket> Pockets { get; set; } 

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                var pocket = c.GetPocket();
                foreach (var p in Pockets)
                {
                    if (p.Equals(pocket)) return true;
                }
                return false;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
