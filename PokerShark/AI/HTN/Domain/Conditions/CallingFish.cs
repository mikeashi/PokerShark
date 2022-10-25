using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.AI.HTN.Domain.Conditions.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class CallingFish : ICondition<Object>
    {
        public string Name { get; } = "if opponents are are fish and like to call";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {

                // if players profile too shallow
                if (c.GetGame().CurrentRound?.RoundCount < 5)
                    return false;

                var opponents = c.GetPlayersModels();
                var players = c.GetGame().CurrentRound?.Players;

                foreach (var opponent in opponents)
                {
                    if (opponent.VPIP > 70 && players.First(p => p.Id == opponent.Player.Id).State != Poker.PlayerState.Folded && opponent.PFR < 40)
                        return true;
                }
                return false;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
