using PokerShark.AI.HTN.Utility;
using PokerShark.Poker.Deck;

namespace PokerShark.AI
{
    public class Oracle
    {
        #region HandStrength
        internal static (double, double, double, double, double) EffectiveHandStrength(List<Card>? Pocket, List<Card>? Board, List<PlayerModel> models)
        {
            if (Pocket == null || Board == null)
                return (0, 0, 0, 0, 0);
            var rawHandStrength = HandStrength(Pocket, Board, models);
            var monteCarloHandStrength = MonteCarloHandStrength(Pocket, Board, models);
            (double ppot, double npot) = HandPotential(Pocket, Board, models);
            var HS = (rawHandStrength + monteCarloHandStrength + monteCarloHandStrength) / 3;
            return (HS + (1 - HS) * ppot - (HS * npot), rawHandStrength, monteCarloHandStrength, ppot, npot);
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
        private static double[][] RaiseFactors = new double[][]{
            new double []{-4,-5,-6,-7,-8,-9,-10,-11,-12,-13},
            new double []{-3,-4,-5,-6,-7,-8,-9,-10,-11,-12},
            new double []{-2,-3,-4,-5,-6,-7,-8,-9,-10,-11},
            new double []{-1,-2,-3,-4,-5,-6,-7,-8,-9,-10},
            new double []{1,-1,-2,-3,-4,-5,-6,-7,-8,-9},
            new double []{2,3,3,4,5,-1,-2,-3,-4,-5},
            new double []{3,4,5,5,5,6,6,4,4,4},
            new double []{4,5,6,6,6,7,7,7,6,6},
            new double []{1,2,2,3,4,5,8,8,9,9},
            new double []{1,1,1,2,2,8,8,9,9,9},

        };

        internal static List<VariableCost> RaiseOdds(double effectiveHandStrength, double raiseAmount, double callAmount, double min, double max, double paid, double potAmount)
        {
            var callingAmount = potAmount + callAmount;

            var normailsedRaiseAmount = NormailseRaise(min, max, raiseAmount);
            var normailsedEHS = Math.Round(effectiveHandStrength, 1);

            var winningAmount = RaiseFactors[GetIndex(normailsedEHS)][GetIndex(normailsedRaiseAmount)] * callingAmount;

            var lossingAmount = paid + raiseAmount;

            return GetOdds(
                        // winning odds
                        (winningAmount, effectiveHandStrength),
                        // lossing odds
                        (-1 * lossingAmount, 1 - effectiveHandStrength)
                    );
        }
        internal static List<VariableCost> CallOdds(double effectiveHandStrength, double callAmount, double paid, double potAmount, List<PlayerModel> opponents, double bigBlind)
        {
            // When facing only one TightPassive player
            // increase cost of call.
            //if(opponents.Count == 1 && opponents[0].PlayingStyle == PlayingStyle.TightPassive) { 
            //    if(callAmount == 0)
            //    {
            //        callAmount = bigBlind;
            //    }
            //    callAmount *= 2;
            //}

            var winningAmount = potAmount + callAmount;

            var lossingAmount = (callAmount + paid);


            if (effectiveHandStrength < 0.5 && callAmount > 0)
            {
                winningAmount *= -1;
            }

            return GetOdds(
                        // winning odds
                        (winningAmount, effectiveHandStrength),
                        // lossing odds
                        (-1 * lossingAmount, 1 - effectiveHandStrength)
                    );
        }
        internal static List<VariableCost> FoldOdds(List<PlayerModel> opponents, double paid)
        {
            double cost = paid;
            double factor = 1;
            foreach (var opponent in opponents)
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
                if (opponent.PFF > 60)
                {
                    // player has PFF, this means player usually folds weak hands
                    factor += 2 * cost;
                }
                // player folds weak cards x 2 => fold is cheaper
                if (opponent.PFF > 70)
                {
                    // player has PFF, this means player usually folds weak hands
                    factor += 1 * cost;
                }
                // player wins alot when seeing the flop => fold is cheaper
                if (opponent.WTSD < 40 && opponent.WSD > 30)
                {
                    // player wins alot and does not enter the showdown alot
                    factor += 2 * cost;
                }
                if (opponent.WSD > 40)
                {
                    // player wins alot and does not enter the showdown alot x 2
                    factor += 2 * cost;
                }
            }
            // take avarage over all opponents
            factor = (double)factor / opponents.Count;

            return GetOdds(
                       // when folding there is only one outcome
                       //( factor * cost, 1)
                       (-1 * cost, 1)
                   );
        }
        private static List<VariableCost> GetOdds(params (double cost, double probability)[] tuples)
        {
            var odds = new List<VariableCost>();
            foreach (var t in tuples)
                odds.Add(new VariableCost(t.cost, t.probability));
            return odds;
        }


        private static double NormailseRaise(double min, double max, double amount)
        {
            var normalisedRaise = (amount - min) / (max - min);

            if (normalisedRaise < 0.1)
                normalisedRaise = 0.1;

            return Math.Round(normalisedRaise, 1);
        }

        private static int GetIndex(double factor)
        {
            var index = 0;
            if (factor < 1)
                index = (int)((factor * 10) - 1);
            else
                index = 1;

            if (index > 9)
                index = 9;

            return 1;
        }
        #endregion
    }
}
