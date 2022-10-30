using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluidHTN;
using PokerShark.AI.HTN.Domain;
using PokerShark.Helpers;
using PokerShark.Poker;
using Action = PokerShark.Poker.Action;

namespace PokerShark.AI.HTN
{
    public class Planner
    {
        #region Properties
        private FluidHTN.Planner<Context, object> _planner;
        private FluidHTN.Domain<Context, object> _domain;
        #endregion

        #region Constructor
        public Planner()
        {
            _planner = new FluidHTN.Planner<Context, object>();
        }
        #endregion

        #region Methods
        public Action GetAction(Context context)
        {
            _domain = BuildDomain(context);

            // reset context
            context.Reset();
            context.Init();
            context.ResetDecision();

            int killSwitch = 5;
            
            while (!context.Done)
            {
                if (killSwitch == 0)
                    break;
                // run planner
                _planner.Tick(_domain, context);
                killSwitch--;
            }

            // parse decision
            var decision = context.GetDecision();
            var amount = context.GetRaiseAmount();

            // select an action based on decision
            return SelectAction(decision, context.GetValidActions(), amount);
        }
        private Domain<Context, object> BuildDomain(Context context)
        {
            return new DomainBuilder()
                    .CheckRaiseCutSelector()
                    .PreflopSequence()
                    .PostflopSequence(context)
                    .Build();
        }
        private Action SelectAction(Decision decision, List<Action> validActions, double amount)
        {
            Dictionary<Action, float> WeightedActions = new Dictionary<Action, float>();
            
            foreach(var a in validActions)
            {
                switch (a.Type)
                {
                    case ActionType.Fold:
                        WeightedActions.Add(a, (float) decision.Fold);
                        break;
                    case ActionType.Call:
                        WeightedActions.Add(a, (float) decision.Call);
                        break;
                    case ActionType.Raise:
                        WeightedActions.Add(a, (float) decision.Raise);
                        break;
                }
            }

            var selected = WeightedActions.RandomElementByWeight(e => e.Value).Key;

            if(selected.Type == ActionType.Raise)
            {
                if(amount == -1)
                {
                    return validActions[1];
                }
                
                return Action.GetRaiseAction(amount, amount);
            }
            
            return selected;
        }
        #endregion
    }
}
