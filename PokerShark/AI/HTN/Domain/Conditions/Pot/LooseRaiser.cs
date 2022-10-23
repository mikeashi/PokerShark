using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions.Pot
{
    internal class LooseReiser : ICondition<Object>
    {
        public string Name { get; } = "If one raise has been made from loose player";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                var raiseAction = c.GetHistory().LastOrDefault(action => action.Type == Poker.ActionType.Raise);
                if (raiseAction == null)
                    return false;
                var models = c.GetPlayersModels();
                var playerModel = models.FirstOrDefault(m => m.Player.Id == raiseAction.PlayerId);
                if (playerModel == null)
                    return false;
                return playerModel.PlayingStyle == PlayingStyle.LooseAggressive || playerModel.PlayingStyle == PlayingStyle.LoosePassive;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
