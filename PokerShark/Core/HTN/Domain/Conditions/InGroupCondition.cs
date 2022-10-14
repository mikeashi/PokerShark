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
    public class InGroupCondition : ICondition<Object>
    {
        public string Name { get; } = "InGroupCondition";
        public List<int> GroupsQuery { get; set; }
        

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                return GroupsQuery.Contains((new Groups()).GetStrength(c.GetPocket()));
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
