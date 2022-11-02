using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class TooFishy : ICondition<Object>
    {
        public string Name { get; } = "if opponents are too fishy";

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
                    if (opponent.VPIP < 70 && players?.First(p => p.Id == opponent.Player.Id).State != Poker.PlayerState.Folded)
                        return false;
                }
                return true;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
