using PokerShark.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Poker
{
    public class Helper
    {
        public static  List<Player> ClonePlayerList(List<Player> players)
        {
            var clonedPlayers = new List<Player>();
            foreach (var p in players)
            {
                clonedPlayers.Add(new Player(p));
            }
            return clonedPlayers;
        }

        public static List<Action> CloneHistoryList(List<Action> actions)
        {
            var clonedActions = new List<Action>();
            foreach (var a in actions)
            {
                clonedActions.Add(new Action(a));
            }
            return clonedActions;
        }

        public static List<Card> CloneCardList(List<Card> cards)
        {
            var clonedCards = new List<Card>();
            foreach (var c in cards)
            {
                clonedCards.Add(new Card(c));
            }
            return clonedCards;
        }


    }
}
