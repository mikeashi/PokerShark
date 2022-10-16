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

        public static (double ppot, double npot) WeightedHandPotential(List<Card> Pocket, List<Card> Board, List<double[]> weights)
        {
            return EvaluatorHelper.WeightedHandPotential(Pocket, Board, weights);
        }
        
        public static (double ppot, double npot) WeightedHandPotential(List<Card> Pocket, List<Card> Board, double[] weights)
        {
            return EvaluatorHelper.WeightedHandPotential(Pocket, Board, weights);
        }
        
        public static (double ppot, double npot) HandPotential(List<Card> Pocket, List<Card> Board)
        {
            return EvaluatorHelper.HandPotential(Pocket, Board);
        }


        public static double WeightedHandStrength(List<Card> Pocket, List<Card> Board, List<double[]> weights)
        {
            return EvaluatorHelper.WeightedHandStrength(Pocket, Board, weights);
        }
        
        public static double RawHandStrength(List<Card> Pocket, List<Card> Board, int PlayersCount)
        {
            return EvaluatorHelper.RawHandStrength(Pocket, Board, PlayersCount-1);
        }
    }
}
