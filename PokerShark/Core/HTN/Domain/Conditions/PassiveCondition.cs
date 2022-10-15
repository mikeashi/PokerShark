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
    public class PassiveCondition : ICondition<Object>
    {
        public string Name { get; } = "If Passive game";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                var models = c.GetPlayersModels();
                var aggressive = 0;
                var passive = 0;
                foreach(var model in models)
                {
                    if (model.PlayingStyle == Poker.PlayingStyle.LooseAggressive || model.PlayingStyle == Poker.PlayingStyle.TightAggressive)
                        aggressive++;
                    else
                        passive++;
                }
                return passive >= aggressive;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
