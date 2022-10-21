using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokerShark.Core.Helpers;
using PokerShark.Core.HTN;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;
using Serilog;
using System.Xml.Linq;

namespace PokerShark.Core.Poker
{
    public class Bot : PyPokerBot
    {
        public PokerContext Context { get; internal set; }
        private bool hasStarted = false;
        private string gameId = null;

        public Bot()
        {
            Context = new PokerContext();
        }

        public override void GameStarted(GameInfo gameInfo)
        {
            hasStarted = true;
            // generate unique id for game
            gameId = Guid.NewGuid().ToString();
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

            // get action from planner.
            var action = planner.GetAction(Context);

            // for debuging.
            if (false)
            {
                var round = state;
                var id = round.Seats.First(s => s.Name == "PokerShark").Id;
                
                Console.WriteLine("Pot: "+ state.Pot.Amount(id));

                // print hand
                Console.WriteLine("Hand: " +PyPokerHelper.DebugPocketCards(pocketCards));

                Console.WriteLine("Cost of folding: " + Context.GetPaid());

                // print odds 
                if (state.StreetState != StreetState.Preflop)
                {
                    var raiseOdds = Context.RaiseOdds(Context.GetMinRaiseAmount());
                    var callOdds = Context.CallOdds();
                    var foldOdds = Context.FoldOdds();

                    Console.WriteLine("RaiseOdds: " + String.Join(" ,", raiseOdds) + " E =" + raiseOdds.Sum(vc => (Context).GetAttitude().CalculateUtility(vc.Cost) * vc.Probability));
                    Console.WriteLine("CallOdds: " + String.Join(" ,", callOdds) + " E =" + callOdds.Sum(vc => (Context).GetAttitude().CalculateUtility(vc.Cost) * vc.Probability));
                    Console.WriteLine("FoldOdds: " + String.Join(" ,", foldOdds) + " E =" + foldOdds.Sum(vc => (Context).GetAttitude().CalculateUtility(vc.Cost) * vc.Probability));
                }

                // ask user to confirm action or change action
                action = PyPokerHelper.GetUserAction(action, validActions);

            }
           

            return action;
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
