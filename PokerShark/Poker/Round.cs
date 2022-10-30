using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using PokerShark.Poker.Deck;

namespace PokerShark.Poker
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoundState
    {
        NotStarted,
        Preflop,
        Flop,
        Turn,
        River,
        Showdown,
    }

    /// <summary>
    ///     Represents a round of poker.
    /// </summary>
    public class Round
    {
        #region Properties
        public int DealerPosition { get; private set; }
        public int SmallBlindPosition { get; private set; }
        public int BigBlindPosition { get; private set; }
        public RoundState RoundState { get; private set; }
        public List<Player> Players { get; private set; }
        public int RoundCount { get; private set; }
        public List<Card> Board { get; private set; }
        public List<Card> Pocket { get; private set; }
        public Pot Pot { get; private set; }
        public List<Player> Winner { get; private set; }
        public List<Action> History { get; private set; }
        public List<String> OddsLog { get; private set; }

        #endregion

        #region constructor
        public Round(int roundCount, List<Card> pocket, List<Player> players)
        : this(0, 0, 0, RoundState.NotStarted, players, roundCount, pocket, new List<Card>(),  new Pot(0))
        {
        }
        
        public Round(int dealerPosition, int smallBlindPosition, int bigBlindPosition, RoundState roundState, List<Player> players, int roundCount, List<Card> pocket, List<Card> board, Pot pot)
        {
            DealerPosition = dealerPosition;
            SmallBlindPosition = smallBlindPosition;
            BigBlindPosition = bigBlindPosition;
            RoundState = roundState;
            Players = Helper.ClonePlayerList(players);
            RoundCount = roundCount;
            Pocket = Helper.CloneCardList(pocket);
            Board = Helper.CloneCardList(board);
            Pot = new Pot(pot);
            Winner = new List<Player>();
            History = new List<Action>();
            //if (Players.Any()) CalculatePositions();
            OddsLog = new List<String>();
            LogStartRound();
        }

        public Round(Round round)
        {
            DealerPosition = round.DealerPosition;
            SmallBlindPosition = round.SmallBlindPosition;
            BigBlindPosition = round.BigBlindPosition;
            RoundState = round.RoundState;
            Players = Helper.ClonePlayerList(round.Players);
            RoundCount = round.RoundCount;
            Pocket = Helper.CloneCardList(round.Pocket);
            Board = Helper.CloneCardList(round.Board);
            Pot = new Pot(round.Pot);
            Winner = Helper.ClonePlayerList(round.Winner);
            History = Helper.CloneHistoryList(round.History);
            OddsLog = round.OddsLog;
        }

        #endregion

        #region Methods
        internal void StartStreet(int dealerPosition, int smallBlindPosition, int bigBlindPosition, RoundState roundState, List<Card> board, Pot pot)
        {
            // check legal positions
            if (dealerPosition < 0 || dealerPosition >= Players.Count)
                throw new ArgumentException("Dealer position must be between 0 and " + (Players.Count - 1));
            if (smallBlindPosition < 0 || smallBlindPosition >= Players.Count)
                throw new ArgumentException("Small blind position must be between 0 and " + (Players.Count - 1));
            if (BigBlindPosition < 0 || BigBlindPosition >= Players.Count)
                throw new ArgumentException("Big blind position must be between 0 and " + (Players.Count - 1));

            
            // check board cards
            if (board.Count != 0 && board.Count != 3 && board.Count != 4 && board.Count != 5)
                throw new ArgumentException("Board cards must be 0, 3, 4 or 5");

            // check pot
            if (pot == null)
                throw new ArgumentNullException("Pot");

            // check pot amount
            if (pot.Amount < 0)
                throw new ArgumentException("Pot amount must be positive");
            
            DealerPosition = dealerPosition;
            SmallBlindPosition = smallBlindPosition;
            BigBlindPosition = bigBlindPosition;
            RoundState = roundState;
            Board = Helper.CloneCardList(board);
            Pot = new Pot(pot);
            CalculatePositions();
            LogStartStreet();
        }
        
        internal void StoreAction(Action action, double updatedPlayerStack, PlayerState updatedPlayerState, Pot updatedPot)
        {
            // store action in round histore
            History.Add(new Action(action));

            // find player
            var player = Players.Find(p => p.Id == action.PlayerId);

            if (player == null)
                throw new ArgumentException("Player not found");
            
            // update player stack
            player.UpdateStack(updatedPlayerStack);

            // update player state
            player.UpdateState(updatedPlayerState);

            // update pot
            Pot = new Pot(updatedPot);

            // log action
            LogAction(action);
        }

        internal void EndRound(List<Player> winners, List<Player> players)
        {
            RoundState = RoundState.Showdown;
            Winner = Helper.ClonePlayerList(winners);
            Players = Helper.ClonePlayerList(players);
            LogEndRound();
        }

        private void CalculatePositions()
        {
            Players[SmallBlindPosition].IsSmallBlind = true;
            Players[BigBlindPosition].IsBigBlind = true;

            Players[SmallBlindPosition].Position = Position.SmallBlind;
            Players[BigBlindPosition].Position = Position.BigBlind;

            switch (Players.Count)
            {
                case 3:
                    Players[(BigBlindPosition + 1) % 3].Position = Position.Early;
                    break;
                case 4:
                    Players[(BigBlindPosition + 1) % 4].Position = Position.Early;
                    Players[(BigBlindPosition + 2) % 4].Position = Position.Middle;
                    break;
                case 5:
                    Players[(BigBlindPosition + 1) % 5].Position = Position.Early;
                    Players[(BigBlindPosition + 2) % 5].Position = Position.Middle;
                    Players[(BigBlindPosition + 3) % 5].Position = Position.Late;
                    break;
                case 6:
                    Players[(BigBlindPosition + 1) % 6].Position = Position.Early;
                    Players[(BigBlindPosition + 2) % 6].Position = Position.Early;
                    Players[(BigBlindPosition + 3) % 6].Position = Position.Middle;
                    Players[(BigBlindPosition + 4) % 6].Position = Position.Late;
                    break;
                case 7:
                    Players[(BigBlindPosition + 1) % 7].Position = Position.Early;
                    Players[(BigBlindPosition + 2) % 7].Position = Position.Early;
                    Players[(BigBlindPosition + 3) % 7].Position = Position.Middle;
                    Players[(BigBlindPosition + 4) % 7].Position = Position.Late;
                    Players[(BigBlindPosition + 5) % 7].Position = Position.Late;
                    break;
                case 8:
                    Players[(BigBlindPosition + 1) % 8].Position = Position.Early;
                    Players[(BigBlindPosition + 2) % 8].Position = Position.Early;
                    Players[(BigBlindPosition + 3) % 8].Position = Position.Middle;
                    Players[(BigBlindPosition + 4) % 8].Position = Position.Middle;
                    Players[(BigBlindPosition + 5) % 8].Position = Position.Late;
                    Players[(BigBlindPosition + 6) % 8].Position = Position.Late;
                    break;
                case 9:
                    Players[(BigBlindPosition + 1) % 9].Position = Position.Early;
                    Players[(BigBlindPosition + 2) % 9].Position = Position.Early;
                    Players[(BigBlindPosition + 3) % 9].Position = Position.Early;
                    Players[(BigBlindPosition + 4) % 9].Position = Position.Middle;
                    Players[(BigBlindPosition + 5) % 9].Position = Position.Middle;
                    Players[(BigBlindPosition + 6) % 9].Position = Position.Late;
                    Players[(BigBlindPosition + 7) % 9].Position = Position.Late;
                    break;
                case 10:
                    Players[(BigBlindPosition + 1) % 10].Position = Position.Early;
                    Players[(BigBlindPosition + 2) % 10].Position = Position.Early;
                    Players[(BigBlindPosition + 3) % 10].Position = Position.Early;
                    Players[(BigBlindPosition + 4) % 10].Position = Position.Middle;
                    Players[(BigBlindPosition + 5) % 10].Position = Position.Middle;
                    Players[(BigBlindPosition + 6) % 10].Position = Position.Middle;
                    Players[(BigBlindPosition + 7) % 10].Position = Position.Late;
                    Players[(BigBlindPosition + 8) % 10].Position = Position.Late;
                    break;
            }
        }
        #endregion
        
        #region log
        private void LogStartRound()
        {
            Log.Information("Starting Round {RoundCount}", RoundCount);
            Log.Information("Players [{players}]", String.Join(" ,", Players));

            if (Pocket.Any())
            {
                Log.Information("Pocket: [{Pocket}]", String.Join(" ,", Pocket.Select(c => c.ToJson()).ToList()));
            }
        }
        private void LogStartStreet()
        {
            Log.Information("Starting Street {RoundState}", RoundState);
            Log.Information("Dealer {Dealer}", Players[DealerPosition].Name);
            Log.Information("Small Blind {SB}", Players[SmallBlindPosition].Name);
            Log.Information("Big Blind {BB}", Players[BigBlindPosition].Name);

            if (Board.Any())
            {
                Log.Information("Board: [{Board}]", String.Join(" ,", Board.Select(c => c.ToJson()).ToList()));
            }
        }
        private void LogAction(Action action)
        {
            if (action.Type == ActionType.Fold)
                Log.Information("Player {Name} folded", action.PlayerName);

            if (action.Type == ActionType.Call)
                Log.Information("Player {Name} called {Amount}", action.PlayerName, action.Amount);

            if (action.Type == ActionType.Raise)
                Log.Information("Player {Name} raised {Amount}", action.PlayerName, action.Amount);

            // Log.Information("Pot: {Pot}", Pot.Amount);
        }
        private void LogEndRound()
        {
           Log.Information("Round {RoundCount} ended", RoundCount);
           Log.Information("Winners: [{Winners}]", String.Join(" ,", Winner.Select(w => w.Name)));
        }
        #endregion

        #region Serialization
        public bool ShouldSerializeDealerPosition()
        {
            return false;
        }

        public bool ShouldSerializeSmallBlindPosition()
        {
            return true;
        }
        
        public bool ShouldSerializeBigBlindPosition()
        {
            return true;
        }
        public bool ShouldSerializeBoard()
        {
            return Board.Any();
        }
        public bool ShouldSerializeHistory()
        {
            return History.Any();
        }
        public bool ShouldSerializeWinner()
        {
            return Winner.Any();
        }
        public bool ShouldSerializeRoundState()
        {
            return false;
        }
        #endregion
    }
}
