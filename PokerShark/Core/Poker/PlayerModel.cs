using PokerShark.Core.PyPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.Poker
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
        public string Name { get; internal set; }
        public string Id { get; internal set; }
        public List<PyAction> History { get; internal set; }
        public double LooseIndex { get; internal set; }
        public double AggressionIndex { get; internal set; }
        public double FoldPercentage { get; internal set; }
        public PlayingStyle PlayingStyle { get; internal set; }

        public WeightTable weightTable { get; internal set; }

        public PlayerModel(string name, string id)
        {
            Name = name;
            Id = id;
            History = new List<PyAction>();
            LooseIndex = 50;
            AggressionIndex = 50;
            PlayingStyle = PlayingStyle.Balanced;
            weightTable = new WeightTable();
        }

        public void UpdateHistory(PyAction action)
        {
            History.Add(action);
            UpdateLooseIndex();
            UpdateAggressionIndex();
            UpdatePlayingStyle();
            UpdateWeightTable(action);
            UpdateFoldPercentage();
        }

        private void UpdateWeightTable(PyAction action)
        {
            // reset weight table on each new hand
            if (action.Stage == StreetState.Preflop)
            {
                weightTable.Reset();
            }
            // update weight table
            weightTable.UpdateTable(action.Stage, LooseIndex, action);
        }

        private void UpdateLooseIndex()
        {
            // find round count
            var roundCount = History.Count(x => x.Stage == StreetState.Preflop);

            // find number of raises in preflop
            var raiseCount = History.Count(x => x.Stage == StreetState.Preflop && (x is RaiseAction || x is CallAction));

            // calculate loose index
            LooseIndex = (((double)raiseCount /roundCount) * 100);
        }

        private void UpdateAggressionIndex()
        {
            // find number of raises
            var raiseCount = History.Count(x => x is RaiseAction);

            // calculate aggression index
            AggressionIndex = (((double)raiseCount / History.Count) * 100);
        }

        private void UpdateFoldPercentage()
        {
            // find number of postflop folds
            var foldCount = History.Count(x => x is FoldAction && x.Stage != StreetState.Preflop);

            // calculate fold percentage
            FoldPercentage = (((double)foldCount / History.Count) * 100);
        }
        
        private void UpdatePlayingStyle()
        {
            if (LooseIndex > 50 && AggressionIndex > 50)
                PlayingStyle = PlayingStyle.LooseAggressive;
            else if (LooseIndex > 50 && AggressionIndex < 50)
                PlayingStyle = PlayingStyle.LoosePassive;
            else if (LooseIndex < 50 && AggressionIndex > 50)
                PlayingStyle = PlayingStyle.TightAggressive;
            else if (LooseIndex < 50 && AggressionIndex < 50)
                PlayingStyle = PlayingStyle.TightPassive;
            else
                PlayingStyle = PlayingStyle.Balanced;
        }
    }
}
