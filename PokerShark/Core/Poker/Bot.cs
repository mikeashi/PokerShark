using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokerShark.Core.Helpers;
using PokerShark.Core.HTN;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;
using Serilog;

namespace PokerShark.Core.Poker
{
    public class Bot : PyPokerBot
    {
        public PokerContext Context { get; internal set; }
        private bool hasStarted = false;

        public Bot()
        {
            Context = new PokerContext();
        }

        public override void GameStarted(GameInfo gameInfo)
        {
            hasStarted = true;
            Log.Information("New Game started.");
            Context.Initialize(gameInfo);
            gameInfo.Verbose();
        }

        public override void RoundStarted(int roundCount, List<Card> pocketCards, List<Seat> seats)
        {
            // store last round History
            Context.StoreRoundHistory();

            Log.Information("Round started.");

            Context.ResetBoardCards();
            Context.SetRoundCount(roundCount);
            Context.SetPocketCards(pocketCards);
            Context.SetSeats(seats);

            PyPokerHelper.VerboseRoundStarted(roundCount, pocketCards, seats);
        }

        public override void StreetStarted(RoundState state)
        {
            Log.Information("Street ( " + state.StreetState + " ).");

            Context.SetCurrentRound(state);

            state.Verbose();
        }

        public override PyAction DeclareAction(List<PyAction> validActions, RoundState state, List<Card> pocketCards)
        {
            Log.Information("Action requestd, available actions : " + PyPokerHelper.GetValidActions(validActions));

            Context.SetCurrentRound(state);
            Context.SetPocketCards(pocketCards);
            Context.SetValidActions(validActions);

            // return action.
            var planner = new PokerPlanner();

            return planner.GetAction(Context);
        }

        public override void GameUpdated(RoundState state, PyAction action)
        {
            Context.SetCurrentRound(state);
            
            var name = PyPokerHelper.GetPlayerNameFromId(state.Seats, action.PlayerId);
            
            Context.UpdatePlayerModel(name, action);
            
            if (name != "PokerShark")
                Log.Information(string.Format("New Action From {0}, Type: {1}, Amount: {2}", name, action.Name, action.Amount));
        }

        public override void RoundResult(RoundState state, List<Card> deadCards, List<Seat> winners)
        {
            Context.SetCurrentRound(state);
            Context.AddDeadCards(deadCards);
            Context.SetLastRoundWinners(winners);
        }

        public override bool InGame()
        {
            return hasStarted;
        }
    }
}
