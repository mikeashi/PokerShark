using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PokerShark.Poker
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Position
    {
        NotSet,
        Early,
        Middle,
        Late,
        SmallBlind,
        BigBlind,
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlayerState
    {
        NotSet,
        Participating,
        AllIn,
        Folded,
    }

    /// <summary>
    ///     Represents a player in a poker game.
    /// </summary>
    public class Player
    {
        #region Properties
        public string Name { get; private set; }
        public string Id { get; private set; }
        public double Stack { get; private set; }
        public Position Position { get; internal set; }
        public PlayerState State { get; private set; }
        public bool IsSmallBlind { get; internal set; }
        public bool IsBigBlind { get; internal set; }
        #endregion

        #region Constructors
        public Player(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public Player(Player player)
        {
            Name = player.Name;
            Id = player.Id;
            Stack = player.Stack;
            Position = player.Position;
            State = player.State;
            IsSmallBlind = player.IsSmallBlind;
            IsBigBlind = player.IsBigBlind;
        }
        #endregion

        #region Methods
        public void UpdateStack(double amount)
        {
            Stack = amount;
        }
        
        public void UpdateState(PlayerState state)
        {
            State = state;
        }

        public override string? ToString()
        {
            return String.Format("{0} : {1}", Name, Stack);
        }


        #endregion

        #region Serialization
        public bool ShouldSerializeIsSmallBlind()
        {
            return IsSmallBlind;
        }

        public bool ShouldSerializeIsBigBlind()
        {
            return IsBigBlind;
        }

        public bool ShouldSerializeState()
        {
            return State != PlayerState.NotSet;
        }
        
        public bool ShouldSerializeStack()
        {
            return State != PlayerState.NotSet;
        }

        public bool ShouldSerializePosition()
        {
            return Position != Position.NotSet;
        }
        #endregion
    }
}
