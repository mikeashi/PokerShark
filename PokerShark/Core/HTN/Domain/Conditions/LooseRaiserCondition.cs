using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.PyPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.HTN.Domain.Conditions
{
    public class LooseRaiserCondition : ICondition<Object>
    {
        public string Name { get; } = "If Only one Raise";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is PokerContext c)
            {
                var raiseAction = c.GetCurrentRound().ActionHistory.LastOrDefault(a => a is RaiseAction);
                if (raiseAction == null)
                    return false;
                var models = c.GetPlayersModels();
                var playerModel = models.FirstOrDefault(m => m.Id == raiseAction.PlayerId);
                if (playerModel == null)
                    return false;
                return playerModel.PlayingStyle == Poker.PlayingStyle.LooseAggressive || playerModel.PlayingStyle == Poker.PlayingStyle.LoosePassive;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
