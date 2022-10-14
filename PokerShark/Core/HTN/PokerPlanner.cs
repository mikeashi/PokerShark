using FluidHTN;
using PokerShark.Core.Helpers;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.HTN.Domain;
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
            Domain = BuildDomain();
            Planner = new Planner<PokerContext, Object>();
        }

        public PyAction GetAction(PokerContext ctx)
        {
            // Reset context 
            ctx.Reset();
            ctx.Init();
            ctx.ResetDecision();
            ctx.Done = false;
            
            // run planner
            while (!ctx.Done)
            {
                Planner.Tick(Domain, ctx);
            }

            // planner decision
            var decision = ctx.GetDecision();

            // available actions
            List<PyAction> actions = (List<PyAction>)ctx.GetState((int)State.ValidActions);
            
            // select an action based on decision
            return SelectAction(decision, actions);
        }

        private Domain<PokerContext, object> BuildDomain()
        {
            return new PokerDomainBuilder("PokerDomain")
                .Select("Check-Raise")
                    .Condition("Check-Raise", (ctx) => ctx.CheckRaise)
                        .Action("Raise")
                            .Do((ctx) => { 
                                ctx.CheckRaise = false;
                                ctx.SetDecision((0,0,1));
                                return TaskStatus.Success;
                            })
                        .End()
                .End()
                .PreflopSequence()
                .Build();
        }

        public static PyAction SelectAction((float Fold, float Call, float Raise) decision, List<PyAction> actions)
        {
            Dictionary<PyAction, float> WeightedActions = new Dictionary<PyAction, float>();

            foreach (var action in actions)
            {
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
