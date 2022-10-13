using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.Poker.Deck
{
    public class PocketHand
    {
        private Rank FirstCardRank;
        private Rank SecondCardRank;
        private bool Suited;

        public PocketHand(Rank firstCardRank, Rank secondCardRank, bool suited=false)
        {
            FirstCardRank = firstCardRank;
            SecondCardRank = secondCardRank;
            Suited = suited;
        }

        public bool Equals(List<Card> cards)
        {
            if (Suited && cards[0].Suit != cards[1].Suit) return false;
            if (cards[0].Rank == FirstCardRank && cards[1].Rank == SecondCardRank) return true;
            if (cards[0].Rank == SecondCardRank && cards[1].Rank == FirstCardRank) return true;
            return false;
        }
    }
}
