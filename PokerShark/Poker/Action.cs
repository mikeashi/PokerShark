using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace PokerShark.Poker
{
    /// <summary>
    /// Represents an action type
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType {
        Fold,
        Call,
        Raise,
    }

    /// <summary>
    /// Represents an action in a poker game
    /// </summary>
    public class Action
    {
        #region Properties
        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        public double Amount { get; private set; }
        public double MinAmount { get; private set; }
        public double MaxAmount { get; private set; }
        public RoundState Stage { get; private set; }
        public ActionType Type { get; private set; }
        #endregion

        #region Constructors
        public Action(string playerId, string playerName, double amount, double min, double max, RoundState stage, ActionType type)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            Amount = amount;
            MinAmount = min;
            MaxAmount = max;
            Stage = stage;
            Type = type;
        }
        public Action(string playerId, string playerName, double amount, RoundState stage)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            Amount = amount;
            Stage = stage;
            Type = ActionType.Call;
        }
        public Action(string playerId, string playerName, double amount, double minAmount, double maxAmount, RoundState stage)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            Amount = amount;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Stage = stage;
            Type = ActionType.Raise;
        }
        public Action(string playerId, string playerName, RoundState stage)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            Stage = stage;
            Type = ActionType.Fold;
        }
        public Action(Action action)
        {
            PlayerId = action.PlayerId;
            PlayerName = action.PlayerName;
            Amount = action.Amount;
            MinAmount = action.MinAmount;
            MaxAmount = action.MaxAmount;
            Stage = action.Stage;
            Type = action.Type;
        }
        
        #endregion

        #region Methods
        public override string ToString()
        {
            return "{"+ String.Format("\"action\" : \"{0}\" , \"amount\" : \"{1}\"", GetName(), Amount) + "}";
        }

        private string GetName()
        {
            switch (Type)
            {
                case ActionType.Fold:
                    return "fold";
                case ActionType.Call:
                    return "call";
                case ActionType.Raise:
                    return "raise";
                default:
                    return "unknown";
            }
        }
        #endregion

        #region Serialization
        public bool ShouldSerializePlayerId()
        {
            return false;
        }

        public bool ShouldSerializeAmount()
        {
            return Type != ActionType.Fold;
        }

        public bool ShouldSerializeMinAmount()
        {
            return Type == ActionType.Raise;
        }
        public bool ShouldSerializeMaxAmount()
        {
            return Type == ActionType.Raise;
        }

        #endregion

        # region Static getters
        public static Action GetFoldAction()
        {
          return new Action("fold", "fold", RoundState.NotStarted);
        }

        public static Action GetCallAction(double amount)
        {
            return new Action("call", "call", amount, RoundState.NotStarted);
        }

        public static Action GetRaiseAction(double min, double max)
        {
            return new Action("raise", "raise", min, min, max, RoundState.NotStarted);
        }
        #endregion
    }

}
