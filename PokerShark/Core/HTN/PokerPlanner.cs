using FluidHTN;
using PokerShark.Core.Helpers;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.HTN.Domain;
using PokerShark.Core.HTN.Utility;
using PokerShark.Core.Poker;
using PokerShark.Core.PyPoker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = FluidHTN.TaskStatus;

namespace PokerShark.Core.HTN
{
    public class PokerPlanner
    {
        public Domain<PokerContext, object> Domain { get; internal set; }
        public Planner<PokerContext, object> Planner { get; internal set; }

        public PokerPlanner()
        {
            Planner = new Planner<PokerContext, Object>();
        }

        public PyAction GetAction(PokerContext ctx)
        {
            Domain = BuildDomain(ctx);

            // Reset context 
            ctx.Reset();
            ctx.Init();
            ctx.ResetDecision();
            
            // run planner
            //while (!ctx.Done)
            //{
                Planner.Tick(Domain, ctx);
            //}

            // planner decision
            var decision = ctx.GetDecision();

            // available actions
            List<PyAction> actions = ctx.GetValidActions();
            
            // select an action based on decision
            return SelectAction(decision, actions, ctx.RaiseDecisionAmount);
        }

        private Domain<PokerContext, object> BuildDomain(PokerContext ctx)
        {
            return new PokerDomainBuilder("PokerDomain")
                .Select("Check-Raise")
                    .Condition("Check-Raise", (ctx) => ctx.CheckRaise)
                        .Action("Raise")
                            .Do((ctx) =>
                            {
                                ctx.CheckRaise = false;
                                ctx.SetDecision((0, 0, 1));
                                ctx.RaiseDecisionAmount = ctx.GetMaxRaiseAmount();
                                return TaskStatus.Success;
                            })
                        .End()
                .End()
                .PreflopSequence()
                .PostflopSequence(ctx)
                .Build();
        }

        public static PyAction SelectAction((float Fold, float Call, float Raise) decision, List<PyAction> actions, double raiseAmount)
        {
            Dictionary<PyAction, float> WeightedActions = new Dictionary<PyAction, float>();

            foreach (var action in actions)
            {
                if (action is RaiseAction)
                {
                    action.Amount = raiseAmount;
                }
                
                switch (action.Name)
                {
                    case "fold":
                        WeightedActions.Add(action, decision.Fold);
                        break;
                    case "call":
                        WeightedActions.Add(action, decision.Call);
                        break;
                    case "raise":
                        WeightedActions.Add(action, decision.Raise);
                        break;
                }
            }
            return WeightedActions.RandomElementByWeight(e => e.Value).Key;
        }
    }
}
