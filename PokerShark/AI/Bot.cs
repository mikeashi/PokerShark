using PokerShark.AI.HTN;
using PokerShark.Poker;
using PokerShark.Poker.Deck;
using Action = PokerShark.Poker.Action;

namespace PokerShark.AI
{
    public class Bot
    {
        public Game? CurrentGame;
        public static string Name = "PokerShark";
        private Context? context;

        /// <summary>
        ///     Start a new game
        /// </summary>
        /// <param name="game">game state object</param>
        public void StartGame(Game game)
        {
            if (CurrentGame != null)
                CurrentGame.Store();
            CurrentGame = game;
            context = new Context();
            context.Initialize(game);
        }

        /// <summary>
        ///     Start a new round
        /// </summary>
        /// <param name="roundNumber">round number</param>
        /// <param name="pocket">pocket cards</param>
        /// <param name="players">players list</param>
        public void StartRound(int roundNumber, List<Card> pocket, List<Player> players)
        {
            // can not start a round if there is no game
            if (CurrentGame == null)
                throw new InvalidOperationException("Game not started");

            // start round
            CurrentGame.StartRound(roundNumber, pocket, players);
        }

        /// <summary>
        ///    Start a new hand
        /// </summary>
        /// <param name="dealerPosition">dealer position</param>
        /// <param name="smallBlindPosition">small blind position</param>
        /// <param name="BigBlindPosition">big blind position</param>
        /// <param name="roundState">stage</param>
        /// <param name="board">board cards</param>
        /// <param name="pot">pot</param>
        public void StartStreet(int dealerPosition, int smallBlindPosition, int BigBlindPosition, RoundState roundState, List<Card> board, Pot pot)
        {
            // can not start a round if there is no game
            if (CurrentGame == null)
                throw new InvalidOperationException("Game not started");

            // start street
            CurrentGame.StartStreet(dealerPosition, smallBlindPosition, BigBlindPosition, roundState, board, pot);
        }

        /// <summary>
        ///     Declear action
        /// </summary>
        /// <param name="validActions">list of valid actions</param>
        /// <returns>bot action</returns>
        public Action DeclareAction(List<Action> validActions)
        {
            // can not declare an action if there is no game
            if (CurrentGame == null)
                throw new InvalidOperationException("Game not started");

            // can not declare an action if there is no round
            if (CurrentGame.CurrentRound == null)
                throw new InvalidOperationException("Round not started");

            // declare action to call
            var action = validActions[1];

            // check context
            if (context == null)
                throw new InvalidOperationException("Context not initialized");

            // initialize planner
            var planner = new Planner();

            // set valid actions
            context.SetValidActions(validActions);

            // get planner action
            action = planner.GetAction(context);

            // update stats window
            var attitude = context.GetAttitude();
            Windows.WindowsManager.UpdateBotStates(attitude, CurrentGame.GetBotModel());

            return action;
        }

        /// <summary>
        ///     Receive action
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="updatedPlayerStack">new player stack</param>
        /// <param name="updatedPlayerState">new player state</param>
        /// <param name="updatedPot">new pot</param>
        public void ReceiveAction(Action action, double updatedPlayerStack, PlayerState updatedPlayerState, Pot updatedPot)
        {
            // can not start a round if there is no game
            if (CurrentGame == null)
                throw new InvalidOperationException("Game not started");

            // store action
            CurrentGame.ReceiveAction(action, updatedPlayerStack, updatedPlayerState, updatedPot);
        }

        /// <summary>
        ///     End round
        /// </summary>
        /// <param name="winners"> list of winners</param>
        public void EndRound(List<Player> winners, List<Player> players)
        {
            // can not start a round if there is no game
            if (CurrentGame == null)
                throw new InvalidOperationException("Game not started");

            CurrentGame.EndRound(winners, players);
        }
    }
}
