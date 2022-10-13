using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.Poker.Deck
{
    public enum Suit : int
    {
        Spades, Hearts, Diamonds, Clubs
    }

    public enum Rank : int
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    }

    public class Card
    {
        public Rank Rank { get; private set; }
        public Suit Suit { get; private set; }
        public StateCard StateCard { get; private set; }


        public Card(string card)
        {
            StateCard = (StateCard)Enum.Parse(typeof(StateCard), card);
            Rank = (Rank)Enum.Parse(typeof(Rank), StateCard.ToString().Split("Of".ToCharArray())[0]);
            Suit = (Suit)Enum.Parse(typeof(Suit), StateCard.ToString().Split("Of".ToCharArray())[2]);
        }

        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
            StateCard = GetStateCard();
        }

        public Card(StateCard stateCard)
        {
            StateCard = stateCard;
            if (stateCard != StateCard.None)
            {
                Rank = (Rank)Enum.Parse(typeof(Rank), StateCard.ToString().Split("Of".ToCharArray())[0]);
                Suit = (Suit)Enum.Parse(typeof(Suit), StateCard.ToString().Split("Of".ToCharArray())[2]);
            }
        }

        public override string ToString()
        {
            // handle none card
            if (StateCard == StateCard.None)
                return "";

            // convert rank to number
            string rank = "" + (int)Rank;
            string suit = Suit.ToString().Substring(0, 1);

            // handle Jack, Queen, King, Ace
            if ((int)Rank == 11)
                rank = "J";

            if ((int)Rank == 12)
                rank = "Q";

            if ((int)Rank == 13)
                rank = "K";

            if ((int)Rank == 14)
                rank = "A";

            return rank + suit;
        }

        private StateCard GetStateCard()
        {
            return (StateCard)Enum.Parse(typeof(StateCard), Rank + "Of" + Suit);
        }
    }
}
