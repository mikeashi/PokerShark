using PokerShark.AI.HTN;
using PokerShark.AI.HTN.Utility;
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
        #region HandStrength
        internal static double EffectiveHandStrength(List<Card>? Pocket, List<Card>? Board, List<PlayerModel> models)
        {
            if (Pocket == null || Board == null)
                return 0;
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
        #endregion

        #region Odds
        internal static List<VariableCost> RaiseOdds(double effectiveHandStrength, double raiseAmount, double potAmount)
        {
            return GetOdds(
                        // winning odds
                        (potAmount, effectiveHandStrength),
                        // lossing odds
                        (-1* raiseAmount, 1-effectiveHandStrength)
                    );
        }
        internal static List<VariableCost> CallOdds(double effectiveHandStrength, double callAmount, double potAmount)
        {
            return GetOdds(
                        // winning odds
                        (potAmount, effectiveHandStrength),
                        // lossing odds
                        (-1 * callAmount, 1 - effectiveHandStrength)
                    );
        }
        internal static List<VariableCost> FoldOdds(List<PlayerModel> opponents)
        {
            double cost = 1;
            double factor = 1;
            foreach(var opponent in opponents)
            {
                // player is too loose => fold is more expensive
                if (opponent.VPIP > 75 && opponent.PFR > 50)
                {
                    // player has high VPIP, this means player usually holds 
                    // weak cards. Player has also high PFR which means
                    // he usually raises with weak cards.
                    factor -= 2 * cost;
                }
                // player does not fold often && loses alot => fold is more expensive
                if (opponent.WTSD > 40 && opponent.WSD < 20)
                {
                    // player has high WTSD, this means player rarly folds
                    // player also have low wining rate which means
                    // he refuses to fold weak cards 
                    factor -= 4 * cost;
                }
                // player folds weak cards => fold is cheaper
                if(opponent.PFF > 60)
                {
                    // player has PFF, this means player usually folds weak hands
                    factor += 1 * cost;
                }
                // player folds weak cards x 2 => fold is cheaper
                if (opponent.PFF > 70)
                {
                    // player has PFF, this means player usually folds weak hands
                    factor += 1 * cost;
                }
                // player wins alot when seeing the flop => fold is cheaper
                if (opponent.WTSD < 40 && opponent.WSD > 60)
                {
                    // player wins alot and does not enter the showdown alot
                    factor += 2 * cost;
                }
                if (opponent.WSD > 80)
                {
                    // player wins alot and does not enter the showdown alot x 2
                    factor += 2 * cost;
                }
            }
            // take avarage over all opponents
            factor = (double) factor / opponents.Count;
            
            return GetOdds(
                       // when folding there is only one outcome
                       ( factor * cost, 1)
                   );
        }
        private static List<VariableCost> GetOdds(params (double cost, double probability)[] tuples)
        {
            var odds = new List<VariableCost>();
            foreach (var t in tuples)
                odds.Add(new VariableCost(t.cost, t.probability));
            return odds;
        }

       
        #endregion



    }
}
