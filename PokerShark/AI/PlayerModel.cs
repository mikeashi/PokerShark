using Newtonsoft.Json.Converters;
using PokerShark.Poker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = PokerShark.Poker.Action;
using Newtonsoft.Json;

namespace PokerShark.AI
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlayingStyle
    {
        LooseAggressive,
        LoosePassive,
        TightAggressive,
        TightPassive,
        Balanced,
    }

    public class PlayerModel
    {
        #region Properties
        public Player Player { get; private set; }
        public List<Action> History { get; private set; }

        // Voluntarily Put Into Pot (VPIP):
        //      how often a player has voluntarily put money
        //      into the pot preflop, either by raising or
        //      calling.
        public double VPIP { get; private set; }

        // Preflop Raise (PFR):
        //      how often a player has entered the pot
        //      preflop by raising.
        public double PFR { get; private set; }

        // Preflop Fold (PFF):
        //    how often a player has folded preflop.
        public double PFF { get; private set; }

        // Went To Showdown (WTSD):
        //   how often a player reaches showdown
        //   after seeing the flop.
        public double WTSD { get; private set; }

        // Pre-Showdown Fold (PSDF):
        //   how often a player has folded
        //   after seeing the flop.
        public double PSDF { get; private set; }

        // Won Money at Showdown (WSD):
        //   how often a player has won money
        //   at showdown.
        public double WSD { get; private set; }

        // Won When Saw Flop (WWSF):
        //  how often a player wins money at showdown
        //  after seening the flop.
        public double WWSF { get; private set; }

        // number of rounds won/drewn 
        public int Win { get; private set; }
        
        // number of rounds lost 
        public int Lost { get; private set; }

        // number of rounds won/drewn postflop
        public int PostFlopWin { get; private set; }

        // number of rounds lost postflop
        public int PostFlopLost { get; private set; }

        public PlayingStyle PlayingStyle { get; private set; }
        public WeightTable WeightTable { get; private set; }

        #endregion

        #region Constructor
        public PlayerModel(Player player)
        {
            Player = new Player(player);
            History = new List<Action>();
            VPIP = 50;
            PFR = 50;
            PFF = 50;
            WTSD = 50;
            WSD = 50;
            WWSF = 50;
            PSDF = 50;
            PostFlopWin = 0;
            PostFlopLost = 0;
            Win = 0;
            Lost = 0;
            PlayingStyle = PlayingStyle.Balanced;
            WeightTable = new WeightTable();
        }
        #endregion

        #region Methods
        public void AddPostFlopWin()
        {
            PostFlopWin++;
            UpdateWWSF();
        }

        public void AddPostFlopLost()
        {
            PostFlopLost++;
            UpdateWWSF();
        }

        public void AddWin()
        {
            Win++;
            UpdateWSD();
        }

        public void AddLost()
        {
            Lost++;
            UpdateWSD();
        }

        public void ReceiveAction(Action action)
        {
            // check if action belongs to player
            if (action.PlayerId != Player.Id)
                throw new ArgumentException("Action does not belong to model");

            // store action
            History.Add(new Action(action));

            // update VPIP
            UpdateVPIP();

            // update PFR
            UpdatePFR();

            // update PFF
            UpdatePFF();

            // update WTSD
            UpdateWTSD();

            // Update PSDF
            UpdatePSDF();

            // update playing style
            UpdatePlayingStyle();

            // update weight table
            WeightTable.ReceiveAction(this, action);
        }
        
        private void UpdateVPIP()
        {
            // find round count
            var roundCount = History.Count(x => x.Stage == RoundState.Preflop);

            // find number of raises in preflop
            var raiseCount = History.Count(x => x.Stage == RoundState.Preflop && (x.Type == ActionType.Raise || x.Type == ActionType.Call));

            // calculate VPIP
            VPIP = (((double)raiseCount / roundCount) * 100);
        }
        
        private void UpdatePFR()
        {
            // find round count
            var roundCount = History.Count(x => x.Stage == RoundState.Preflop);
            
            // find number of raises
            var raiseCount = History.Count(x => x.Stage == RoundState.Preflop && x.Type == ActionType.Raise);

            // calculate PFR
            PFR = (((double)raiseCount / roundCount) * 100);
        }
        
        private void UpdatePFF()
        {
            // find round count
            var roundCount = History.Count(x => x.Stage == RoundState.Preflop);

            // find number of folds
            var raiseCount = History.Count(x => x.Stage == RoundState.Preflop && x.Type == ActionType.Fold);

            // calculate PFF
            PFF = (((double)raiseCount / roundCount) * 100);
        }
        
        private void UpdateWTSD()
        {
            // find number of folds
            var foldCount = History.Count(x => x.Type == ActionType.Fold);

            // find number of rounds
            var roundCount = History.Count;

            WTSD =  100 - (((double)foldCount / roundCount) * 100);
        }

        private void UpdatePSDF()
        {
            // find number of folds postflop
            var foldCount = History.Count(x => x.Stage != RoundState.Preflop && x.Type == ActionType.Fold);

            // find number of rounds
            var roundCount = History.Count(x => x.Stage != RoundState.Preflop);

            PSDF = (((double)foldCount / roundCount) * 100);
        }


        private void UpdateWSD()
        {
            WSD = ((double)Win / (Win + Lost)) * 100;
        }

        private void UpdateWWSF()
        {
            WWSF = ((double)PostFlopWin / (PostFlopWin + PostFlopLost)) * 100;
        }

        private void UpdatePlayingStyle()
        {
            // VPIP
            var mid_vpip = VPIP < 70 && VPIP > 45;
            var low_vpip = VPIP <= 45;
            var high_vpip = VPIP >= 70;
            
            // PFR
            var high_pfr = PFR > 50;
            var low_pfr = PFR <= 50;

            if (high_vpip && high_pfr) PlayingStyle = PlayingStyle.LooseAggressive;
            if (high_vpip && low_pfr) PlayingStyle = PlayingStyle.LoosePassive;
            
            if (mid_vpip && high_pfr) PlayingStyle = PlayingStyle.TightAggressive;
            if (mid_vpip && low_pfr) PlayingStyle = PlayingStyle.TightPassive;

            if (low_vpip && high_pfr) PlayingStyle = PlayingStyle.TightAggressive;
            if (low_vpip && low_pfr) PlayingStyle = PlayingStyle.TightPassive;
        }
        #endregion

        #region Serialization
        public bool ShouldSerializeWeightTable()
        {
            return false;
        }

        public bool ShouldSerializeHistory()
        {
            return false;
        }

        #endregion
    }
}
