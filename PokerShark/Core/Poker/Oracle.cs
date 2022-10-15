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

        public static (double ppot, double npot) HandPotential(List<Card> Pocket, List<Card> Board)
        {
            return EvaluatorHelper.HandPotential(Pocket, Board);
        }
        

        /// <summary>
        ///     Returns RHS also known as HR based on the HandStrength function described
        ///     by Aaron Davidson's paper: "Opponent modeling in poker: Learning and acting in a hostile and uncertain environment"
        /// </summary>
        /// <param name="Pocket">pocket</param>
        /// <param name="Board">hand</param>
        /// <returns></returns>
        public static double RawHandStrength(List<Card> Pocket, List<Card> Board)
        {
            return EvaluatorHelper.RawHandStrength(Pocket, Board);
        }
        
        /// <summary>
        ///     Returns the number of cards that can improve the hand.
        /// </summary>
        /// <param name="Pocket">pocket</param>
        /// <param name="Board">hand</param>
        /// <returns></returns>
        public static int CountOfCardsThatImprooveHand(List<Card> Pocket, List<Card> Board)
        {
            return EvaluatorHelper.Outs(Pocket, Board);
        }

        /// <summary>
        ///     Returns a list of cards that can improve the hand
        /// </summary>
        /// <param name="Pocket">pocket</param>
        /// <param name="Board">hand</param>
        /// <returns></returns>
        public static List<Card> CardsThatImprooveHand(List<Card> Pocket, List<Card> Board)
        {
            return EvaluatorHelper.OutsCards(Pocket, Board);
        }
    }
}
