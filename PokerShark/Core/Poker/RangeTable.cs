using PokerShark.Core.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.Poker
{
    public class RangeTable
    {
        public string[,] GetRefrenceTable()
        {
            string[,] RefrenceTable = new string[13, 13];
            // get ranks ace to two.
            var AceToTwo = Enum.GetValues(typeof(Rank)).Cast<Rank>().ToList();
            AceToTwo.Reverse();
            // set suited flag
            var suited = false;
            for (int r1 = 0; r1 < AceToTwo.Count; r1++)
            {
                suited = false;
                for (int r2 = 0; r2 < AceToTwo.Count; r2++)
                {
                    RefrenceTable[r1, r2] = GetRefrenceCardName(AceToTwo[r1], AceToTwo[r2], suited);
                    if (AceToTwo[r1] == AceToTwo[r2]) suited = true;
                }
            }
            return RefrenceTable;
        }

        public static string GetName(List<Card> Pocket)
        {
            if (Pocket.Count != 2) throw new Exception("Pocket has to have 2 cards");
            return GetRefrenceCardName(Pocket[0].Rank, Pocket[1].Rank, Pocket[0].Suit == Pocket[1].Suit);
        }
        
        public static (int,int) GetPosition(List<Card> Pocket)
        {
            if (Pocket.Count != 2) throw new Exception("Pocket has to have 2 cards");
            // order cards
            Pocket.Sort((x, y) => (x.Rank).CompareTo(y.Rank));
            if (Pocket[1].Suit == Pocket[0].Suit)
            {
                return (RankMap(Pocket[1]), RankMap(Pocket[0]));
            }
            else
            {
                return (RankMap(Pocket[0]), RankMap(Pocket[1]));
            }
        }
        
        public void GetWeight(Object[,] table, List<Card> Pocket)
        {
            if (Pocket.Count != 2) throw new Exception("Pocket has to have 2 cards");
            // order cards
            Pocket.Sort((x,y) => (x.Rank).CompareTo(y.Rank));
            if (Pocket[1].Suit == Pocket[0].Suit)
            {
                Console.WriteLine(table[RankMap(Pocket[1]), RankMap(Pocket[0])]);
            }
            else
            {
                Console.WriteLine(table[RankMap(Pocket[0]), RankMap(Pocket[1])]);
            }
        }
        
        private static int RankMap(Card card)
        {
            switch (card.Rank)
            {
                case Rank.Two:
                    return 12;
                case Rank.Three:
                    return 11;
                case Rank.Four:
                    return 10;
                case Rank.Five:
                    return 9;
                case Rank.Six:
                    return 8;
                case Rank.Seven:
                    return 7;
                case Rank.Eight:
                    return 6;
                case Rank.Nine:
                    return 5;
                case Rank.Ten:
                    return 4;
                case Rank.Jack:
                    return 3;
                case Rank.Queen:
                    return 2;
                case Rank.King:
                    return 1;
                case Rank.Ace:
                    return 0;
            }
            return -1;
        }
        
        public void PrintTable(Object[,] table)
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    Console.Write(table[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        
        private static string GetRefrenceCardName(Rank rank1, Rank rank2, bool suited)
        {
            if (rank1 < rank2)
                return GetRankRefrenceName(rank2) +  GetRankRefrenceName(rank1) + (suited ? "s" : "") ;
            return GetRankRefrenceName(rank1)  + GetRankRefrenceName(rank2) + (suited ? "s" : "");
        }
        
        private static string GetRankRefrenceName(Rank rank)
        {
            string name = "" + (int)rank;
            
            if ((int)rank == 10)
                name = "T";
            
            if ((int)rank == 11)
                name = "J";

            if ((int)rank == 12)
                name = "Q";

            if ((int)rank == 13)
                name = "K";

            if ((int)rank == 14)
                name = "A";
            return name;
        }
    }
}
