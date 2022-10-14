using FluidHTN.Contexts;
using FluidHTN.Debug;
using FluidHTN.Factory;
using PokerShark.Core.Poker;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.HTN.Context
{
    public class PokerBaseContext : BaseContext<object>
    {
        #region properties
        private object[] _worldState = new object[Enum.GetValues(typeof(State)).Length];
        public override IFactory Factory { get; set; } = new DefaultFactory();
        public override List<string> MTRDebug { get; set; } = null;
        public override List<string> LastMTRDebug { get; set; } = null;
        public override bool DebugMTR { get; } = true;
        public override Queue<IBaseDecompositionLogEntry> DecompositionLog { get; set; } = null;
        public override bool LogDecomposition { get; } = true;
        public override object[] WorldState => _worldState;
        public bool Done { get; set; } = false;
        public bool CheckRaise { get; set; } = false;

        #endregion

        #region Initialize
        public void Initialize(GameInfo info)
        {
            // init base
            base.Init();

            // init Game information
            DirectSet(State.GameInfo, info);

            // init Board Cards
            DirectSet(State.BoardCards, new List<Card>());

            // init Round Count
            DirectSet(State.RoundCount, 0);

            // init Pocket Cards
            DirectSet(State.PocketCards, new List<Card>());

            // init Seats
            DirectSet(State.Seats, info.Seats);

            // init Current Round
            DirectSet(State.CurrentRound, RoundState.GetEmpty());

            // init Valid Actions
            DirectSet(State.ValidActions, new List<PyAction>());

            // init Dead Cards
            DirectSet(State.DeadCards, new List<Card>());

            // init Last Round Winners
            DirectSet(State.LastRoundWinners, new List<Seat>());

            // init Decision
            (float Fold, float Call, float Raise) decision = (Fold: 0, Call:0, Raise:0);
            DirectSet(State.Decision, decision);

            // init action History
            DirectSet(State.ActionHistory, new List<PyAction>());

            // init Players Models
            DirectSet(State.PlayersModels, new List<PlayerModel>());
        }

        #endregion

        private void DirectSet(State state, object value)
        {
            WorldState[(int)state] = value;
        }
    }
}
