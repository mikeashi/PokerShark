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
        RecomanndedDecision,
        RecomanndedRaiseAmount,
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
        public bool OverrideCut = false;
        public bool PrefoldDecision = false;
        public bool Done = false;
        #endregion

        #region Initialize
        public void Initialize(Game game)
        {
            double zero = 0;
            base.Init();
            DirectSet(State.Game, game);
            DirectSet(State.Decision, new Decision() { Fold = zero, Call = zero, Raise = zero });
            DirectSet(State.RecomanndedDecision, new Decision() { Fold = zero, Call = zero, Raise = zero });
            DirectSet(State.RaiseAmount, zero);
            DirectSet(State.RecomanndedRaiseAmount, zero);
            DirectSet(State.CheckRaise, false);
            DirectSet(State.ValidActions, new List<Action>());
            Done = false;
        }

        internal void ResetDecision()
        {
            double zero = 0;
            DirectSet(State.Decision, new Decision() { Fold = zero, Call = zero, Raise = zero });
            DirectSet(State.RecomanndedDecision, new Decision() { Fold = zero, Call = zero, Raise = zero });
            DirectSet(State.RecomanndedRaiseAmount, zero);
            DirectSet(State.RaiseAmount, zero);
            OverrideCut = false;
            PrefoldDecision = false;
            Done = false;
        }
        #endregion

        #region Methods
        public StaticUtilityFunction GetAttitude()
        {
            var stack = GetCurrentStack();
            var initial = GetGame().InitialStack;


            if (stack > initial * 1.5)
                return new RiskSeeking();
            else if (stack < initial * 0.5)
                return new RiskAverse();
            else
                return new RiskNeutral();
        }
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
        public void SetRecomanndedDecision(Decision decision)
        {
            SetState((int)State.RecomanndedDecision, decision);
        }
        public void SetRecomanndedRaiseAmount(double amount)
        {
            SetState((int)State.RecomanndedRaiseAmount, amount);
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
        public void SetRecomanndedRaiseAmount(params (int Factor, float Weight)[] amounts)
        {
            Dictionary<int, float> WeightedFactors = new Dictionary<int, float>();
            foreach (var amount in amounts)
            {
                WeightedFactors.Add(amount.Factor, amount.Weight);
            }

            var factor = WeightedFactors.RandomElementByWeight(e => e.Value).Key;
            SetState((int)State.RecomanndedRaiseAmount, factor * GetGame().BigBlind);
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
        public Decision GetRecomanndedDecision()
        {
            return (Decision)GetState((int)State.RecomanndedDecision);
        }
        public double GetRaiseAmount()
        {
            return (double)GetState((int)State.RaiseAmount);
        }
        public double GetRecomanndedRaiseAmount()
        {
            return (double)GetState((int)State.RecomanndedRaiseAmount);
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
                    amount = a.Amount;
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
        public double GetCurrentStack()
        {
            var game = GetGame();
            var round = game.CurrentRound;
            if (round == null)
                return 0;
            return round.Players.First(p => p.Name == Bot.Name).Stack;
        }
        public double GetPaid()
        {
            var game = GetGame();
            var round = game.CurrentRound;
            var botHistory = round?.History.Where(a => a.PlayerName == Bot.Name).ToList();
            double blindAmount = 0;

            if (round.Players.First(p => p.Name == Bot.Name).IsBigBlind)
                blindAmount += game.BigBlind;

            if (round.Players.First(p => p.Name == Bot.Name).IsSmallBlind)
                blindAmount += game.SmallBlind;

            if (botHistory == null)
                return blindAmount;

            return botHistory.Sum(a => a.Amount) - blindAmount;
        }

        #endregion
    }

}
