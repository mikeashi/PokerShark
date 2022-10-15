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
    public class OccasionallyCondition : ICondition<Object>
    {
        public string Name { get; } = "Randomly true less than 30% of the time";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                if (new Random().Next(1, 11) > 4)
                {
                    return true;
                }
                return false;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
