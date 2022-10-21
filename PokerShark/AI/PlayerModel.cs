using PokerShark.Poker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = PokerShark.Poker.Action;

namespace PokerShark.AI
{
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
        public double LooseIndex { get; private set; }
        public double AggressionIndex { get; private set; }
        public double FoldPercentage { get; private set; }
        public PlayingStyle PlayingStyle { get; private set; }
        public WeightTable WeightTable { get; private set; }
        #endregion

        #region Constructor
        public PlayerModel(Player player)
        {
            Player = player;
            History = new List<Action>();
            LooseIndex = 50;
            AggressionIndex = 50;
            PlayingStyle = PlayingStyle.Balanced;
            WeightTable = new WeightTable();
        }
        #endregion

        #region Methods
        public void ReceiveAction(Action action)
        {
            // check if action belongs to player
            if (action.PlayerId != Player.Id)
                throw new ArgumentException("Action does not belong to model");

            // store action
            History.Add(new Action(action));

            // update loose index
            UpdateLooseIndex();

            // update aggression index
            UpdateAggressionIndex();

            // update fold precentage
            UpdateFoldPercentage();
            
            // update playing style
            UpdatePlayingStyle();

            // update weight table
            WeightTable.ReceiveAction(LooseIndex, action);
        }
        private void UpdateLooseIndex()
        {
            // find round count
            var roundCount = History.Count(x => x.Stage == RoundState.Preflop);

            // find number of raises in preflop
            var raiseCount = History.Count(x => x.Stage == RoundState.Preflop && (x.Type == ActionType.Raise || x.Type == ActionType.Call));

            // calculate loose index
            LooseIndex = (((double)raiseCount / roundCount) * 100);
        }
        private void UpdateAggressionIndex()
        {
            // find number of raises
            var raiseCount = History.Count(x => x.Type == ActionType.Raise);

            // calculate aggression index
            AggressionIndex = (((double)raiseCount / History.Count) * 100);
        }
        private void UpdateFoldPercentage()
        {
            // find number of folds
            var foldCount = History.Count(x => x.Type == ActionType.Fold && x.Stage != RoundState.Preflop);

            // calculate fold percentage
            FoldPercentage = (((double)foldCount / History.Count) * 100);
        }
        private void UpdatePlayingStyle()
        {
            // check if loose index is above 50
            if (LooseIndex > 50)
            {
                // check if aggression index is above 50
                if (AggressionIndex > 50)
                {
                    PlayingStyle = PlayingStyle.LooseAggressive;
                }
                else
                {
                    PlayingStyle = PlayingStyle.LoosePassive;
                }
            }
            else
            {
                // check if aggression index is above 50
                if (AggressionIndex > 50)
                {
                    PlayingStyle = PlayingStyle.TightAggressive;
                }
                else
                {
                    PlayingStyle = PlayingStyle.TightPassive;
                }
            }
        }
        #endregion
    }
}
