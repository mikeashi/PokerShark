using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Deck = PokerShark.Core.Poker.Deck;
using HoldemHand;
using MathNet.Numerics.Integration;
using PokerShark.Core.HTN;
using PokerShark.Core.PyPoker;
using Serilog;

namespace PokerShark.Core.Helpers
{
    public class EvaluatorHelper
    {
        public static (double ppot, double npot) HandPotential(List<Deck.Card> Pocket, List<Deck.Card> Board)
        {
            double ppot = 0;
            double npot = 0;
            Hand.HandPotential(GetMask(Pocket), GetMask(Board), out ppot, out npot);
            return (ppot, npot);
        }
        
        public static double HandStrength(List<Deck.Card> Pocket, List<Deck.Card> Board)
        {
            double ahead = 0;
            double tied = 0;
            double beheind = 0;
            uint pocketBest, oppBest;

            var pocketMask = GetMask(Pocket);
            var boardMask = GetMask(Board);

            foreach (ulong oppcards in Hand.Hands(0UL, pocketMask | boardMask, 2))
            {
                foreach (ulong handmask in Hand.Hands(boardMask, pocketMask | oppcards, 5))
                {
                    pocketBest = Hand.Evaluate(pocketMask | handmask, 7);
                    oppBest = Hand.Evaluate(oppcards | handmask, 7);

                    if (pocketBest > oppBest) ahead++;
                    else if (pocketBest == oppBest) tied++;
                    else beheind++;
                }
            }
            return (ahead + (tied / 2) ) / (ahead + tied + beheind);
        }

        public static double RawHandStrength(List<Deck.Card> Pocket, List<Deck.Card> Board)
        {
            double ahead= 0;
            double tied = 0;
            double beheind = 0;
            uint pocketBest, oppBest;
            
            var pocketMask = GetMask(Pocket);
            var boardMask = GetMask(Board);

            foreach (ulong oppcards in Hand.Hands(0UL, pocketMask | boardMask, 2))
            {
                    pocketBest = Hand.Evaluate(pocketMask | boardMask, 5);
                    oppBest = Hand.Evaluate(oppcards | boardMask, 5);

                    if (pocketBest > oppBest) ahead++;
                    else if (pocketBest == oppBest) tied++;
                    else beheind++;
            }

            return (ahead + (tied / 2) ) / (ahead + tied + beheind);
        }
        
        public static int Outs(List<Deck.Card> pocket, List<Deck.Card> board)
        {
            return Hand.Outs(GetMask(pocket), GetMask(board));
        }
        
        public static List<Deck.Card> OutsCards(List<Deck.Card> Pocket, List<Deck.Card> Board)
        {
            return FromMask(Hand.OutsMask(GetMask(Pocket), GetMask(Board)));
        }
        
        public static IEnumerable<ulong> GetOpponentPockets(List<Deck.Card> pocket, List<Deck.Card> board)
        {
            return Hand.Hands(0UL, GetMask(board) | GetMask(pocket), 2);
        }

        public static List<Deck.Card> FromMask(ulong mask, List<Deck.Card> board)
        {
            return FromMask(mask, GetMask(board));
        }
        
        public static List<Deck.Card> FromMask(ulong mask, ulong board = 0UL)
        {
            if (board != 0UL) mask |= board;
            List<Deck.Card> cards = new List<Deck.Card>();
            foreach(var card in Hand.MaskToPockerShark(mask))
            {
                cards.Add(new Deck.Card(card));
            }
            return cards;
        } 
        
        public static ulong GetMask(List<Deck.Card> cards)
        {
            return Hand.ParseHand(GetAsString(cards));
        }
        
        public static string GetAsString(List<Deck.Card> cards)
        {
            if (cards == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (Deck.Card card in cards)
                sb.Append(card.ToString() + " ");
            return sb.ToString();
        }

        public static String CardsToString(List<Deck.Card> cards)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            stringBuilder.Append(String.Join(", ", cards));
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

    }
}
