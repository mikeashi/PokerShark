using Newtonsoft.Json;
using PokerShark.Core.PyPoker;
using PokerShark.Poker;
using PokerShark.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = PokerShark.Poker.Action;
using PlayerState = PokerShark.Poker.PlayerState;

namespace PokerShark.Interfaces.PyPoker
{
    internal class Helper
    {
        public static Game GetGame(string payload)
        {
            var gameInfo = JsonConvert.DeserializeObject<GameInfoMessage>(payload);

            if (gameInfo == null)
                throw new Exception("Could not parse game info");

            int playersCount = gameInfo.PlayerNum;
            double initialStack = gameInfo.Rule.InitialStack;
            int maxRound = gameInfo.Rule.MaxRound;
            double smallBlind = gameInfo.Rule.SmallBlindAmount;
            double bigBlind = gameInfo.Rule.SmallBlindAmount * 2;
            double ante = gameInfo.Rule.Ante;
            List<Player> players = new List<Player>();

            foreach (var seat in gameInfo.Seats)
            {
                players.Add(new Player(seat.Name, seat.Uuid));
            }

            return new Game(playersCount, initialStack, maxRound, smallBlind, bigBlind, ante, players);
        }

        public static RoundStartMessage GetRoundStartMessage(string payload)
        {
            var message =  JsonConvert.DeserializeObject<RoundStartMessage>(payload);
            
            if (message == null)
                throw new Exception("Could not parse round start message");

            return message;
        }

        public static List<Card> GetCards(string[] cards)
        {
            List<Card> cardList = new List<Card>();
            
            foreach (var card in cards)
            {
                cardList.Add(new Card(card));
            }
            
            return cardList;
        }

       
        
        public static List<Player> GetPlayers(Seat[] seats)
        {
            List<Player> players = new List<Player>();

            foreach (var seat in seats)
            {
                var player = new Player(seat.Name, seat.Uuid);
                player.UpdateStack(seat.Stack);
                player.UpdateState(GetPlayerState(seat));
                players.Add(player);
            }
            
            return players;
        }

        public static PlayerState GetPlayerState(Seat seat)
        {
            var state = PlayerState.NotSet;
            switch (seat.State)
            {
                case "participating":
                    state = PlayerState.Participating;
                    break;
                case "allin":
                    state = PlayerState.AllIn;
                    break;
                case "folded":
                    state = PlayerState.Folded;
                    break;
            }
            return state;
        }


        public static RoundStateMessage GetRoundStateMessage(string payload)
        {
            var message = JsonConvert.DeserializeObject<RoundStateMessage>(payload);

            if (message == null)
                throw new Exception("Could not parse round state message");

            return message;
        }

        public static Poker.RoundState GetRoundState(string state)
        {
            Poker.RoundState street = Poker.RoundState.Preflop;
            switch (state)
            {
                case "preflop":
                    street = Poker.RoundState.Preflop;
                    break;
                case "flop":
                    street = Poker.RoundState.Flop;
                    break;
                case "turn":
                    street = Poker.RoundState.Turn;
                    break;
                case "river":
                    street = Poker.RoundState.River;
                    break;
                case "showdown":
                    street = Poker.RoundState.Showdown;
                    break;
            }
            return street;
        }

        public static Poker.Pot GetPot(Pot pot)
        {
            Poker.Pot p = new Poker.Pot(pot.Main.Amount);

            foreach (var sidePot in pot.Side)
            {
                p.SidePots.Add(new SidePot(sidePot.Amount, sidePot.Eligibles.ToList()));
            }
            
            return p;
        }


        public static NewActionMessage GetNewActionMessage(string payload)
        {
            var message = JsonConvert.DeserializeObject<NewActionMessage>(payload);

            if (message == null)
                throw new Exception("Could not parse new action message");

            return message;
        }



        public static List<Player> GetWinners(string payload)
        {
            var message = JsonConvert.DeserializeObject<RoundResultMessage>(payload);

            if (message == null)
                throw new Exception("Could not parse winner message");
            
            return GetPlayers(message.Winners);
        }

        public static List<Player> GetResultPlayers(string payload)
        {
            var message = JsonConvert.DeserializeObject<RoundResultMessage>(payload);

            if (message == null)
                throw new Exception("Could not parse winner message");

            return GetPlayers(message.RoundState.Seats);
        }

        



        public static Action GetAction(NewActionMessage message)
        {
            Action action = null;

            var seat = message.RoundState.Seats.Where(s => s.Uuid == message.NewAction.PlayerUuid).FirstOrDefault();
            
            if(seat == null)
                throw new Exception("Could not parse new action message");
            
            switch (message.NewAction.Action)
            {
                case "fold":
                     action = new Action(seat.Uuid, seat.Name, GetRoundState(message.RoundState.Street));
                    break;
                case "call":
                     action = new Action(seat.Uuid, seat.Name, message.NewAction.Amount, GetRoundState(message.RoundState.Street));
                    break;
                case "raise":
                     action = new Action(seat.Uuid, seat.Name, message.NewAction.Amount, message.NewAction.Amount, message.NewAction.Amount, GetRoundState(message.RoundState.Street));
                    break;
            }
            
            if (action == null)
                throw new Exception("Could not parse new action message");
            
            return action;
        }
        public static double GetPlayerStack(NewActionMessage message, string id)
        {
            var seat = message.RoundState.Seats.Where(s => s.Uuid == id).FirstOrDefault();

            if (seat == null)
                throw new Exception("Could not parse new action message");

            return seat.Stack;
        }
        public static PlayerState GetPlayerState(NewActionMessage message, string id)
        {
            var seat = message.RoundState.Seats.Where(s => s.Uuid == id).FirstOrDefault();

            if (seat == null)
                throw new Exception("Could not parse new action message");

            return GetPlayerState(seat);
        }
        public static List<Action> GetValidActions(string payload)
        {
            var message = JsonConvert.DeserializeObject<ValidActionsMessage>(payload);

            if (message == null)
                throw new Exception("Could not parse valid action message");

            List<Action> actions = new List<Action>();
            
            foreach(var action in message.ValidActions)
            {
                switch (action.Action)
                {
                    case "fold":
                        actions.Add(Action.GetFoldAction());
                        break;
                    case "call":
                        if(action.Amount.Integer == null)
                            throw new Exception("Could not parse valid action message");
                        actions.Add(Action.GetCallAction((double) action.Amount.Integer));
                        break;
                    case "raise":
                        if (action.Amount.AmountClass == null)
                            throw new Exception("Could not parse valid action message");
                        actions.Add(Action.GetRaiseAction(action.Amount.AmountClass.Min, action.Amount.AmountClass.Max));
                        break;
                }
            }
            
            return actions;
        }
        
    }
}
