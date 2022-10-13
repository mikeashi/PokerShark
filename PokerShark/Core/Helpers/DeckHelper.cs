using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerShark.Core.HTN;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;
using Serilog;

namespace PokerShark.Core.Helpers
{
    public class DeckHelper
    {
        public static void VerboseRoundStarted(int roundCount, List<Card> pocketCards, List<Seat> seats)
        {
            Log.Verbose("Round Info:");
            Log.Verbose(StringHelper.Indent(3) + "Round Count: " + roundCount);
            Log.Verbose(StringHelper.Indent(3) + "PocketCards: " + DeckHelper.DebugPocketCards(pocketCards));
            Log.Verbose(StringHelper.Indent(3) + "Seats: ");
            foreach (var seat in seats)
            {
                seat.Verbose(6);
                Log.Verbose(StringHelper.Indent(6) + "--------------------------------");
            }
        }

        public static String DebugPocketCards(List<Card> pocketCards)
        {
            return String.Format("[{0}, {1}]", pocketCards[0].ToString(), pocketCards[1].ToString());
        }

        public static String DebugBoardCards(List<Card> boardCards)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            stringBuilder.Append(String.Join(" ," ,boardCards));
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        public static String GetPlayerNameFromId(List<Seat> seats, String id)
        {
            var name = "";
            foreach(var seat in seats)
            {
                if (seat.Id == id) return seat.Name;
            }
            return name;
        }

        public static String GetValidActions(List<PyAction> actions)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            for(int i = 0; i < actions.Count; i++)
            {
                stringBuilder.Append(actions[i].Information() + " ");
                if (i < actions.Count - 1)
                    stringBuilder.Append(", ");
            }
            
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }
    }
}
