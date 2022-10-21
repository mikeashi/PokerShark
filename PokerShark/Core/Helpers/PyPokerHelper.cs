using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace PokerShark.Core.Helpers
{
    public class PyPokerHelper
    {
        public static void VerboseRoundStarted(int roundCount, List<Card> pocketCards, List<Seat> seats)
        {
            Log.Verbose("Round Info:");
            Log.Verbose(StringHelper.Indent(3) + "Round Count: " + roundCount);
            Log.Verbose(StringHelper.Indent(3) + "PocketCards: " + PyPokerHelper.DebugPocketCards(pocketCards));
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
        
        public static String GetPlayerNameFromId(List<Seat> seats, String id)
        {
            var name = "";
            foreach (var seat in seats)
            {
                if (seat.Id == id) return seat.Name;
            }
            return name;
        }
        
        
        
        public static String GetValidActions(List<PyAction> actions)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            for (int i = 0; i < actions.Count; i++)
            {
                stringBuilder.Append(actions[i].Information() + " ");
                if (i < actions.Count - 1)
                    stringBuilder.Append(", ");
            }

            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }
        
        public static GameInfo GetGameInfo(JToken payload)
        {
            var playersNumber = (double) payload["player_num"];
            var initialStack = (double) payload["rule"]["initial_stack"];
            var maxRound = (double) payload["rule"]["max_round"];
            var smallBlind = (double) payload["rule"]["small_blind_amount"];
            var ante = (double) payload["rule"]["ante"];
            var bigBlind = 2 * smallBlind;
            var seats = getSeats(payload["seats"]);
            return new GameInfo(playersNumber, initialStack, maxRound, smallBlind, bigBlind, ante, seats);
        }

        public static PyAction getAction(JToken payload, StreetState streetState)
        {
            PyAction action = null;
            switch ((String)payload["action"])
            {
                case "fold":
                    action = new FoldAction();
                    action.PlayerId = (String)payload["player_uuid"];
                    action.Stage = streetState;
                    break;
                case "call":
                    action = new CallAction((Double)payload["amount"]);
                    action.PlayerId = (String)payload["player_uuid"];
                    action.Stage = streetState;
                    break;
                case "raise":
                    action = new RaiseAction((Double)payload["amount"], (Double)payload["amount"]);
                    action.PlayerId = (String)payload["player_uuid"];
                    action.Stage = streetState;
                    break;
            }
            return action;
        }

        public static PyAction getHistoryAction(JToken payload, StreetState stage)
        {
            PyAction action = null;
            switch ((String)payload["action"])
            {
                case "FOLD":
                    action = new FoldAction();
                    action.PlayerId = (String)payload["uuid"];
                    //action.Paid = (Double)payload["paid"];
                    action.Stage = stage;
                    break;
                case "CALL":
                    action = new CallAction((Double)payload["amount"]);
                    action.PlayerId = (String)payload["uuid"];
                    action.Paid = (Double)payload["paid"];
                    action.Stage = stage;
                    break;
                case "RAISE":
                    action = new RaiseAction((Double)payload["amount"], (Double)payload["amount"]);
                    action.PlayerId = (String)payload["uuid"];
                    action.Paid = (Double)payload["paid"];
                    action.Stage = stage;
                    break;
            }
            return action;
        }

        public static List<PyAction> getValidActions(JToken payload)
        {
            var actions = new List<PyAction>();
            foreach (var action in payload)
            {
                switch ((String)action["action"])
                {
                    case "fold":
                        actions.Add(new FoldAction());
                        break;
                    case "call":
                        actions.Add(new CallAction((Double)action["amount"]));
                        break;
                    case "raise":
                        actions.Add(new RaiseAction((Double)action["amount"]["min"], (Double)action["amount"]["max"]));
                        break;
                }
            }
            return actions;
        }

        public static RoundState getRoundState(JToken payload)
        {
            int dealerPosition = (int)payload["dealer_btn"];
            int smallBlindPosition = (int)payload["small_blind_pos"];
            int bigBlindPosition = (int)payload["big_blind_pos"];
            StreetState streetState = getStreetStateFromString((String)payload["street"]);
            List<Seat> seats = getSeats(payload["seats"]);
            int nextPlayer = -1;

            // Sometimes not_found is returned for some resone.
            if ((string)payload["next_player"] != "not_found")
                nextPlayer = (int)payload["next_player"];

            int roundCount = (int)payload["round_count"];
            List<Card> board = getBoardCards(payload["formated_community_card"]);
            Pot pot = JsonConvert.DeserializeObject<Pot>(payload["pot"].ToString());

            List<PyAction> actionHistory = new List<PyAction>();
            // parse action History
            if (payload["action_histories"] != null)
            {
                var history = payload["action_histories"];
                // parse preflop actions
                if (payload["action_histories"]?["preflop"] != null)
                {
                    var preflop_history = payload["action_histories"]?["preflop"];

                    foreach (var preflopAction in preflop_history)
                    {
                        if (preflopAction?["action"].ToString() == "SMALLBLIND") continue;
                        if (preflopAction?["action"].ToString() == "BIGBLIND") continue;
                        actionHistory.Add(getHistoryAction(preflopAction,StreetState.Preflop));
                    }
                }
                if (payload["action_histories"]?["flop"] != null)
                {
                    var flop_history = payload["action_histories"]?["flop"];

                    foreach (var flopAction in flop_history)
                    {
                        actionHistory.Add(getHistoryAction(flopAction, StreetState.Flop));
                    }
                }
                if (payload["action_histories"]?["turn"] != null)
                {
                    var turn_history = payload["action_histories"]?["turn"];

                    foreach (var turnAction in turn_history)
                    {
                        actionHistory.Add(getHistoryAction(turnAction, StreetState.Turn));
                    }
                }
            }
            return new RoundState(dealerPosition, smallBlindPosition, bigBlindPosition, streetState, seats, nextPlayer, roundCount, board, pot, actionHistory);
        }

        public static List<Card> getBoardCards(JToken payload)
        {
            var board = new List<Card>();
            foreach(var card in payload)
            {
                board.Add(new Card((string)card));
            }
            return board;
        }

        public static List<Card> getPocketCards(JToken payload)
        {
            var pocket = new List<Card>();
            pocket.Add(new Card((string)payload[0]));
            pocket.Add(new Card((string)payload[1]));
            return pocket;
        }

        public static List<Seat> getSeats(JToken payload)
        {
            var seats = new List<Seat>();
            foreach (var seat in payload)
            {
                seats.Add(getSeat(seat));
            }
            return seats;
        }

        public static Seat getSeat(JToken payload)
        {
            var stack = (double) payload["stack"];
            var name = (string) payload["name"];
            var id = (string) payload["uuid"];
            PlayerState state = PlayerState.Participating;

            switch ((string) payload["state"])
            {
                case "participating":
                    state = PlayerState.Participating;
                    break;
                case "allin":
                    state = PlayerState.Allin;
                    break;
                case "folded":
                    state = PlayerState.Folded;
                    break;
            }
            return new Seat(stack, state, name, id);
        }

        public static StreetState getStreetStateFromString(String streetState)
        {
            StreetState street = StreetState.Preflop;
            switch (streetState)
            {
                case "preflop":
                    street = StreetState.Preflop;
                    break;
                case "flop":
                    street = StreetState.Flop;
                    break;
                case "turn":
                    street = StreetState.Turn;
                    break;
                case "river":
                    street = StreetState.River;
                    break;
                case "showdown":
                    street = StreetState.Showdown;
                    break;
            }
            return street;
        }

        internal static PyAction GetUserAction(PyAction action, List<PyAction> validActions)
        {
            // ask user for action
            Console.WriteLine("Please choose an action:");
            Console.WriteLine(0 + ": " + action);
            for (int i = 0; i < validActions.Count; i++)
            {
                Console.WriteLine(i+1 + ": " + validActions[i]);
            }
            
            int userAction = -1;
            
            while (userAction < 0 || userAction >= validActions.Count + 1 )
            {
                Console.Write("Action: ");
                userAction = Convert.ToInt32(Console.ReadLine());
            }

            if (userAction == 0)
            {
                return action;
            }
            else
            {
                return validActions[userAction - 1];
            }
        }
    }
}
