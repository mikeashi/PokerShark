using Newtonsoft.Json.Converters;
using PokerShark.Core.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PokerShark.Poker.Deck
{
    public class CardToJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Card));
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
                throw new Exception("can not convert card to json");

            if (value is Card card)
                writer.WriteValue(card.ToJson());
            else
                throw new Exception("can not convert a none card object to json");
        }
    }


    public enum StateCard
    {
        None,
        TwoOfSpades,
        ThreeOfSpades,
        FourOfSpades,
        FiveOfSpades,
        SixOfSpades,
        SevenOfSpades,
        EightOfSpades,
        NineOfSpades,
        TenOfSpades,
        JackOfSpades,
        QueenOfSpades,
        KingOfSpades,
        AceOfSpades,
        TwoOfHearts,
        ThreeOfHearts,
        FourOfHearts,
        FiveOfHearts,
        SixOfHearts,
        SevenOfHearts,
        EightOfHearts,
        NineOfHearts,
        TenOfHearts,
        JackOfHearts,
        QueenOfHearts,
        KingOfHearts,
        AceOfHearts,
        TwoOfDiamonds,
        ThreeOfDiamonds,
        FourOfDiamonds,
        FiveOfDiamonds,
        SixOfDiamonds,
        SevenOfDiamonds,
        EightOfDiamonds,
        NineOfDiamonds,
        TenOfDiamonds,
        JackOfDiamonds,
        QueenOfDiamonds,
        KingOfDiamonds,
        AceOfDiamonds,
        TwoOfClubs,
        ThreeOfClubs,
        FourOfClubs,
        FiveOfClubs,
        SixOfClubs,
        SevenOfClubs,
        EightOfClubs,
        NineOfClubs,
        TenOfClubs,
        JackOfClubs,
        QueenOfClubs,
        KingOfClubs,
        AceOfClubs,
    }
    public enum Suit : int
    {
        Spades, Hearts, Diamonds, Clubs
    }
    public enum Rank : int
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    }

    [JsonConverter(typeof(CardToJsonConverter))]
    public class Card
    {
        #region Properties
        public Rank Rank { get; private set; }
        public Suit Suit { get; private set; }
        public StateCard StateCard { get; private set; }
        #endregion

        #region Constructors
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

        public Card(Card card)
        {
            StateCard = card.StateCard;
            Rank = card.Rank;
            Suit = card.Suit;
        }

        #endregion

        #region Methods
        public string ToJson()
        {
            // handle none card
            if (StateCard == StateCard.None)
                return "";

            // convert rank to number
            string rank = "" + (int)Rank;
            string suit = Suit.ToString().Substring(0, 1);

            // handle 10, Jack, Queen, King, Ace
            if ((int)Rank == 10)
                rank = "T";

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

        public string ToHoldemCard()
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

        #endregion

        #region Serialization
        public bool ShouldSerializeRank()
        {
            return false;
        }

        public bool ShouldSerializeSuit()
        {
            return false;
        }
        #endregion
    }
}
