using PokerShark.Core.HTN.Context;
using PokerShark.Poker.Deck;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Poker
{
    /// <summary>
    ///     Represents a game of poker.
    /// </summary>
    public class Game
    {
        #region Properties
        public string Id { get; private set; }
        public int PlayersCount { get; private set; }
        public int InitialStack { get; private set; }
        public int MaxRound { get; private set; }
        public int SmallBlind { get; private set; }
        public int BigBlind { get; private set; }
        public int Ante { get; private set; }
        public List<Player> Players { get; private set; }
        public List<Round> Rounds { get; private set; }
        public Round? CurrentRound { get; private set; }
        #endregion

        #region Constructors
        public Game(int playersCount, int initialStack, int maxRound, int smallBlind, int bigBlind, int ante, List<Player> players)
        {
            Id = Guid.NewGuid().ToString();
            PlayersCount = playersCount;
            InitialStack = initialStack;
            MaxRound = maxRound;
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            Ante = ante;
            Players = Helper.ClonePlayerList(players);
            Rounds = new List<Round>();
            // log game information 
            LogStart();
        }
        #endregion

        #region Methods
        internal void StartRound(int roundNumber, List<Card> pocket, List<Player> players)
        {
            // can not start a round if there is already a round in progress
            if (CurrentRound != null)
                throw new InvalidOperationException("Round already started");

            // check pocket cards
            if (pocket.Count != 2)
                throw new ArgumentException("Pocket cards must be 2");

            // check players
            if (players.Count != PlayersCount)
                throw new ArgumentException("Players count must be " + PlayersCount);

            // check players ids
            foreach (var player in players)
            {
                if (!Players.Any(p => p.Id == player.Id))
                    throw new ArgumentException("Player id not found");
            }
            
            // start new round
            CurrentRound = new Round(roundNumber, pocket, players);
        }

        internal void StartStreet(int dealerPosition, int smallBlindPosition, int BigBlindPosition, RoundState roundState, List<Card> board, Pot pot)
        {
            // can not start a street if there is no round in progress
            if (CurrentRound == null)
                throw new InvalidOperationException("Round not started");

            // start street
            CurrentRound.StartStreet(dealerPosition, smallBlindPosition, BigBlindPosition, roundState, board, pot);
        }
        
        internal void ReceiveAction(Action action, double updatedPlayerStack, PlayerState updatedPlayerState, Pot updatedPot)
        {
            // can not receive action if there is no round in progress
            if (CurrentRound == null)
                throw new InvalidOperationException("Round not started");

            // can not receive action if street not in progress
            if (CurrentRound.RoundState == RoundState.NotStarted)
                throw new InvalidOperationException("Street not started");

            // store action
            CurrentRound.StoreAction(action, updatedPlayerStack, updatedPlayerState, updatedPot);
        }

        public void EndRound(List<Player> winners)
        {
            // can not receive action if there is no round in progress
            if (CurrentRound == null)
                throw new InvalidOperationException("Round not started");

            // can not receive action if street not in progress
            if (CurrentRound.RoundState == RoundState.NotStarted)
                throw new InvalidOperationException("Street not started");

            CurrentRound.EndRound(winners);
            
            // store round in round history
            Rounds.Add(new Round(CurrentRound));
            
            // reset current round
            CurrentRound = null;
        }
        #endregion
        
        #region log
        private void LogStart()
        {
            Log.Information("Starting game {GameId}", Id);
            Log.Information("MaxRound: {MaxRound}", MaxRound);
            Log.Information("Small blind amount: {sb}", SmallBlind);
            Log.Information("Big blind amount: {bb}", BigBlind);
            Log.Information("Ante: {ante}", Ante);
            Log.Information("Players: [{players}]", String.Join(" ,", Players.Select(p => p.Name)));
        }
        #endregion

        #region Serialization
        public bool ShouldSerializeRounds()
        {
            return Rounds.Any();
        }
        public bool ShouldSerializeCurrentRound()
        {
            return false;
        }
        #endregion
    }
}
