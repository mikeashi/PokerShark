using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.Poker.Deck;

namespace PokerShark.AI.HTN.Domain.Conditions.Pockt
{
    internal class PocketEquityLessThan : ICondition<Object>
    {
        public string Name { get; } = "If pocket equity is less than";
        public double Equity { get; set; }

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                var game = c.GetGame();
                var NotFoldedPlayers = game.Players.Where(p => p.State != Poker.PlayerState.Folded).ToList().Count;
                var pocketCards = c.GetPocket();
                Pocket pocket = new Pocket(pocketCards[0].Rank, pocketCards[1].Rank, pocketCards[0].Suit == pocketCards[1].Suit);
                return HandEvaluator.GetPocketEquity(pocket, NotFoldedPlayers) < Equity;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
