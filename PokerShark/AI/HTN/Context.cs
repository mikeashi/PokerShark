using FluidHTN.Contexts;
using FluidHTN.Debug;
using FluidHTN.Factory;
using PokerShark.AI.HTN.Utility;
using PokerShark.Helpers;
using PokerShark.Poker;
using PokerShark.Poker.Deck;
using Action = PokerShark.Poker.Action;
using RoundState = PokerShark.Poker.RoundState;

namespace PokerShark.AI.HTN
{
    internal enum State
    {
        Game,
        Decision,
        RaiseAmount,
        CheckRaise,
        ValidActions,
    }

    public class Context : BaseContext<object>
    {
        #region Properties
        private object[] _worldState = new object[Enum.GetValues(typeof(State)).Length];
        public override IFactory Factory { get; set; } = new DefaultFactory();
        public override List<string> MTRDebug { get; set; } = new List<String>();
        public override List<string> LastMTRDebug { get; set; } = new List<String>();
        public override bool DebugMTR { get; } = true;
        public override Queue<IBaseDecompositionLogEntry> DecompositionLog { get; set; } = new Queue<IBaseDecompositionLogEntry>();
        public override bool LogDecomposition { get; } = true;
        public override object[] WorldState => _worldState;
        #endregion

        #region Initialize
        public void Initialize(Game game)
        {
            base.Init();
            DirectSet(State.Game, game);
            DirectSet(State.Decision, new Decision() { Fold = 0, Call = 0, Raise = 0 });
            DirectSet(State.RaiseAmount, 0);
            DirectSet(State.CheckRaise, false);
            DirectSet(State.ValidActions, new List<Action>());
        }

        internal void ResetDecision()
        {
            double zero = 0;
            DirectSet(State.Decision, new Decision() { Fold = zero, Call = zero, Raise = zero });
            DirectSet(State.RaiseAmount, zero);
        }
        #endregion

        #region Methods
        // public methods
        public StaticUtilityFunction GetAttitude()
        {
            // TODO dynamic attitude
            // based on stack amount
            // c > 2 * i => seeking
            // c < i/2 => averse
            // neutral
            // if loose game & c > i/2 => seeking
            return new RiskNeutral();
        }

        // private methods
        private void DirectSet(State state, object value)
        {
            _worldState[(int)state] = value;
        }
        #endregion

        #region State Setters
        public void SetDecision(Decision decision)
        {
            SetState((int)State.Decision, decision);
        }
        public void SetRaiseAmount(double amount)
        {
            SetState((int)State.RaiseAmount, amount);
        }
        public void SetRaiseAmount(params (int Factor, float Weight)[] amounts)
        {
            Dictionary<int, float> WeightedFactors = new Dictionary<int, float>();
            foreach (var amount in amounts)
            {
                WeightedFactors.Add(amount.Factor, amount.Weight);
            }

            var factor = WeightedFactors.RandomElementByWeight(e => e.Value).Key;
            SetState((int)State.RaiseAmount, factor * GetGame().BigBlind);
        }
        public void SetCheckRaise()
        {
            SetState((int)State.CheckRaise, true);
        }
        public void UnsetCheckRaise()
        {
            SetState((int)State.CheckRaise, false);
        }
        public void SetValidActions(List<Action> actions)
        {
            SetState((int)State.ValidActions, actions);
        }
        #endregion

        #region State Getters
        public Game GetGame()
        {
            return (Game)GetState((int)State.Game);
        }
        public RoundState GetRoundState()
        {
            var game = GetGame();
            if (game.CurrentRound == null)
                throw new Exception("Current Round is null!");
            return game.CurrentRound.RoundState;
        }
        public Position GetPosition()
        {
            var game = GetGame();
            if (game.CurrentRound == null)
                throw new Exception("Current Round is null!");
            return game.CurrentRound.Players.First(p => p.Name == Bot.Name).Position;
        }
        public List<Action> GetHistory()
        {
            var game = GetGame();
            if (game.CurrentRound == null)
                throw new Exception("Current Round is null!");
            return game.CurrentRound.History;
        }
        public List<Card> GetPocket()
        {
            var game = GetGame();
            if (game.CurrentRound == null)
                throw new Exception("Current Round is null!");
            return game.CurrentRound.Pocket;
        }
        public List<PlayerModel> GetPlayersModels()
        {
            var game = GetGame();
            if (game.CurrentRound == null)
                throw new Exception("Current Round is null!");
            return game.GetOpponentModels();
        }
        public Decision GetDecision()
        {
            return (Decision)GetState((int)State.Decision);
        }
        public double GetRaiseAmount()
        {
            return (double)GetState((int)State.RaiseAmount);
        }
        public bool GetCheckRaise()
        {
            return (bool)GetState((int)State.CheckRaise);
        }
        public List<Action> GetValidActions()
        {
            return (List<Action>)GetState((int)State.ValidActions);
        }
        public double GetMaxPossibleRaiseAmount()
        {
            var actions = GetValidActions();
            double maxAmount = 0;
            foreach (var a in actions)
            {
                if (a.Type == ActionType.Raise)
                    maxAmount = a.MaxAmount;
            }
            return maxAmount;
        }
        public double GetMinPossibleRaiseAmount()
        {
            var actions = GetValidActions();
            double minAmount = 0;
            foreach (var a in actions)
            {
                if (a.Type == ActionType.Raise)
                    minAmount = a.MinAmount;
            }
            return minAmount;
        }
        public double GetCallAmount()
        {
            var actions = GetValidActions();
            double amount = 0;
            foreach (var a in actions)
            {
                if (a.Type == ActionType.Call)
                    amount = a.MinAmount;
            }
            return amount;
        }
        public double GetPotAmount()
        {
            double amount = 0;
            var game = GetGame();
            if (game == null)
                return amount;
            var round = game.CurrentRound;
            if (round == null)
                return amount;
            return round.Pot.getTotalForPlayer(game.Players.First(p => p.Name == Bot.Name).Id);
        }

        #endregion
    }

}
