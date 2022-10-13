using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PokerShark.Core.Poker.Deck;
using static System.Net.Mime.MediaTypeNames;

namespace PokerShark.Core.Poker
{
    internal class Oracle
    {
        public static string GetStringRepresentation(List<Card> cards)
        {
            if (cards == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (Card card in cards)
                sb.Append(card.ToString() + " ");
            return sb.ToString();
        }
    }
}
