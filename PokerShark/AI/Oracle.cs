using PokerShark.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI
{
    public class Oracle
    {
        public static double EffectiveHandStrength(List<Card> Pocket, List<Card> Board, List<PlayerModel> models)
        {
            var rawHandStrength = HandStrength(Pocket, Board, models);
            var monteCarloHandStrength = MonteCarloHandStrength(Pocket, Board, models);
            (double ppot, double npot) = HandPotential(Pocket, Board, models);
            var HS = (rawHandStrength + monteCarloHandStrength) / 2;
            return HS + (1 - HS) * ppot;
        }

        private static double HandStrength(List<Card> Pocket, List<Card> Board, List<PlayerModel> models)
        {
            return HandEvaluator.HandStrength(Pocket, Board, models);
        }

        private static double MonteCarloHandStrength(List<Card> Pocket, List<Card> Board, List<PlayerModel> models)
        {
            return HandEvaluator.MonteCarloHandStrength(Pocket, Board, models);
        }

        private static (double ppot, double npot) HandPotential(List<Card> Pocket, List<Card> Board, List<PlayerModel> models)
        {
            return HandEvaluator.HandPotential(Pocket, Board, models);
        }
    }
}
