using PokerShark.AI;
using PokerShark.Poker.Deck;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public double InitialStack { get; private set; }
        public int MaxRound { get; private set; }
        public double SmallBlind { get; private set; }
        public double BigBlind { get; private set; }
        public double Ante { get; private set; }
        public List<Player> Players { get; private set; }
        public List<Round> Rounds { get; private set; }
        public Round? CurrentRound { get; private set; }
        public List<Player> Winners { get; private set; }
        public List<PlayerModel> PlayerModels { get;  set; }
        public List<Result> Results { get; private set; }

        #endregion

        #region Constructors
        public Game(int playersCount, double initialStack, int maxRound, double smallBlind, double bigBlind, double ante, List<Player> players)
        {
            Id = Guid.NewGuid().ToString();
            PlayersCount = playersCount;
            InitialStack = initialStack;
            MaxRound = maxRound;
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            Ante = ante;
            Players = Helper.ClonePlayerList(players);
            Winners = new List<Player>();
            Rounds = new List<Round>();
            PlayerModels = new List<PlayerModel>();
            Results = new List<Result>();
            foreach (var player in Players)
            {
                Results.Add(new Result(player, initialStack));
                PlayerModels.Add(new PlayerModel(player));
            }
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

            // update model
            var model = PlayerModels.FirstOrDefault(m => m.Player.Id == action.PlayerId);
            model?.ReceiveAction(action);
            
            // update profiles window
            Windows.WindowsManager.UpdateProfiles(GetOpponentModels());
        }
        public void EndRound(List<Player> winners, List<Player> players)
        {
            // can not receive action if there is no round in progress
            if (CurrentRound == null)
                throw new InvalidOperationException("Round not started");

            // can not receive action if street not in progress
            if (CurrentRound.RoundState == RoundState.NotStarted)
                throw new InvalidOperationException("Street not started");


            var stage = CurrentRound.RoundState;
            var old_player_state = Helper.ClonePlayerList(CurrentRound.Players);
            CurrentRound.EndRound(winners, players);

            // update results & player models
            foreach(var result in Results)
            {
                var model = PlayerModels.FirstOrDefault(m => m.Player.Id == result.Player.Id);
                result.UpdateStack(CurrentRound.Players.First(p => p.Id == result.Player.Id).Stack);
                if (winners.Any(w => w.Id == result.Player.Id))
                {
                    // postflop win
                    if (stage != RoundState.Preflop)
                    {
                        model?.AddPostFlopWin();
                        if (old_player_state.Where(p => p.Id != result.Player.Id).Any(p => p.State != PlayerState.Folded))
                            model?.AddWinAgainstNotFoldedPlayers();                            
                    }
                    
                    if (winners.Count > 1)
                    {
                        result.Drew();
                    }
                    else
                    {
                        result.Won();
                    }
                }
                else
                {
                    result.Lost();
                    
                    // postflop lost
                    if (stage != RoundState.Preflop)
                    {
                        model?.AddPostFlopLost();
                        if (old_player_state.Where(p => p.Id != result.Player.Id).Any(p => p.State != PlayerState.Folded))
                            model?.AddLostAgainstNotFoldedPlayers();
                    }
                }
            }

            // update results window
            Windows.WindowsManager.UpdateResults(Results.Where(r => r.Player.Name == Bot.Name).ToList());
            
            // store round in round history
            Rounds.Add(new Round(CurrentRound));
            
            // reset current round
            CurrentRound = null;
        }
        public List<PlayerModel> GetOpponentModels()
        {
            var models = new List<PlayerModel>();
            foreach(var model in PlayerModels)
            {
                if (model.Player.Name != Bot.Name)
                    models.Add(model);
            }
            return models;
        }
        public List<PlayerModel> GetNotFoldedOpponentModels()
        {
            var models = new List<PlayerModel>();
            foreach (var model in PlayerModels)
            {
                if (model.Player.Name == Bot.Name)
                    continue;
                if (CurrentRound?.Players.Where(p => p.Id == model.Player.Id).First().State != PlayerState.Folded)
                    models.Add(model);
            }
            return models;
        }
        public PlayerModel? GetBotModel()
        {
            return PlayerModels.FirstOrDefault(m => m.Player.Name == Bot.Name);
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

        #region Store
        public void Store()
        {
            //Log.Information("Game {GameId} ended", Id);

            // create logs folder
            var path = "logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // create today's folder
            path = "logs/" + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // write game to file
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(Path.Combine(path, Id + ".json"), json);
        }
        #endregion
    }
}
