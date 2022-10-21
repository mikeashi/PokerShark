using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HoldemHand;
using PokerShark.Core.Helpers;
using PokerShark.Core.Poker.Deck;


namespace PokerShark.Core.Poker
{
    internal class Oracle
    {
        /// <summary>
        ///     Effective Hand Strength
        /// </summary>
        /// <param name="Pocket"></param>
        /// <param name="Board"></param>
        /// <param name="weights"></param>
        /// <returns></returns>
        public static double EHS(List<Card> Pocket, List<Card> Board, List<double[]> weights)
        {
            var hs = HandStrength(Pocket, Board, weights);
            var mhs = MonteCarloHandStrength(Pocket, Board, weights);
            (double ppot, double npot) = HandPotential(Pocket, Board, weights);
            hs = (hs + mhs) / 2;
            //return mhs + (1- mhs) *ppot;
            return hs + (1-hs) *ppot;
        }

        public static (double ppot, double npot) HandPotential(List<Card> Pocket, List<Card> Board, List<double[]> weights)
        {
            return EvaluatorHelper.WeightedHandPotential(Pocket, Board, weights);
        }


        public static double HandStrength(List<Card> Pocket, List<Card> Board, List<double[]> weights)
        {
            return EvaluatorHelper.WeightedHandStrength(Pocket, Board, weights);
        }

        public static double MonteCarloHandStrength(List<Card> Pocket, List<Card> Board, List<double[]> weights)
        {
            return EvaluatorHelper.MonteCarloHandStrength(Pocket, Board, weights);
        }
    }
}
