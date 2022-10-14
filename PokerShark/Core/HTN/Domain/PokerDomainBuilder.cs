using FluidHTN;
using FluidHTN.Factory;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.HTN.Domain.Conditions;
using PokerShark.Core.HTN.Tasks;
using PokerShark.Core.HTN.Tasks.CompoundTasks;
using PokerShark.Core.HTN.Utility;
using PokerShark.Core.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = FluidHTN.TaskStatus;

namespace PokerShark.Core.HTN.Domain
{
    public class PokerDomainBuilder : BaseDomainBuilder<PokerDomainBuilder, PokerContext, Object>
    {
        public PokerDomainBuilder(string domainName) : base(domainName, new DefaultFactory())
        {
            // Empty Constructor
        }

        #region Conditions
        
        public PokerDomainBuilder IfInPreflop()
        {
            var condition = new InPreflopCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfInEarlyPosition()
        {
            var condition = new InEarlyPositionCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfInMiddlePosition()
        {
            var condition = new InMiddlePositionCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfInLatePosition()
        {
            var condition = new InLatePositionCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfInBlindPosition()
        {
            var condition = new InBlindPositionCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfInSmallBlindPosition()
        {
            var condition = new InSmallBlindPositionCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfInBigBlindPosition()
        {
            var condition = new InBigBlindPositionCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfNoDecisionYet()
        {
            var condition = new NoDecisionYetCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfNoRaises()
        {
            var condition = new NoRaisesCondition();
            Pointer.AddCondition(condition);
            return this;
        }
        public PokerDomainBuilder IfOneOrMoreRaises()
        {
            var condition = new OneOrMoreRaisesCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfInGroups(params int[] groups)
        {
            var condition = new InGroupCondition() { GroupsQuery= groups.ToList<int>() };
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfPocketIn(params PocketHand[] cards)
        {
            var condition = new PocketInCondition() { Hands = cards.ToList<PocketHand>() };
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfLoose()
        {
            var condition = new LooseCondition();
            Pointer.AddCondition(condition);
            return this;
        }


        public PokerDomainBuilder IfTight()
        {
            var condition = new TightCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfSuitedHand()
        {
            var condition = new SuitedHandCondition();
            Pointer.AddCondition(condition);
            return this;
        }

        public PokerDomainBuilder IfNoCalls()
        {
            var condition = new NoCallesCondition();
            Pointer.AddCondition(condition);
            return this;
        }


        #endregion

        #region expected utility selector

        public PokerDomainBuilder ExpectedUtilitySelector(String name)
        {
            this.CompoundTask<ExpectedUtilitySelector>(name);
            return this;
        }

        public PokerDomainBuilder VariableCostAction(String name, List<VariableCost> costs)
        {
            if (this.Pointer is ExpectedUtilitySelector compoundTask)
            {
                var parent = new VariableCostTask(costs) { Name = name };
                _domain.Add(compoundTask, parent);
                _pointers.Add(parent);
                return this;
            }
            else
            {
                throw new Exception("Pointer is not a Expected Utility Selector, which is required for adding Utility Actions!");
            }
        }

        #endregion

        #region Sequences
        public PokerDomainBuilder PreflopSequence()
        {
            Select("PreflopSequence");
            {
                IfInPreflop();
                IfNoDecisionYet();
                PreflopEarlyPosition();
                PreflopMiddlePosition();
                PreflopLatePosition();
                PreflopBlindPosition();
                Select("Check-Raise");
                {
                    Condition("If Strong Raise", (ctx) => ctx.GetDecision().Raise > 0.5f);
                    Action("Check raise ocecunally");
                    {
                        Do((ctx) =>
                        {
                            if (new Random().Next(1, 11) > 2)
                            {
                                ctx.SetDecision((0, 1, 0));
                                ctx.CheckRaise = true;
                            }
                            return TaskStatus.Success;
                        });
                    }
                    End();
                }
                End();
            }
            End();
            return this;
        }

        public PokerDomainBuilder PreflopEarlyPosition()
        {
            // In early positions we are out of position on all bettings rounds,
            // so we need a superior hand to call or raise.
            Select("EarlyPosition");
            {
                IfInEarlyPosition();
                IfNoDecisionYet();
                Select("NoRaises");
                {
                    IfNoRaises();
                    
                    Action("Raise 2/3 of time on AKs, AQs, Ajs, KQs");
                    {
                        IfPocketIn( new PocketHand(Rank.Ace, Rank.King, true),
                                    new PocketHand(Rank.Ace, Rank.Queen, true),
                                    new PocketHand(Rank.Ace, Rank.Jack, true),
                                    new PocketHand(Rank.King, Rank.Queen, true));
                        
                        // Fold:0 , Call: 0.25, Raise: 0.75
                        Do((ctx) => { ctx.SetDecision((0, 0.25f, 0.75f)); return TaskStatus.Success;});
                    }
                    End();

                    Action("Usually Raise on 1,2");
                    {
                        IfInGroups(1, 2);
                        
                        // Fold:0 , Call: 0.1, Raise: 0.9
                        Do((ctx) => { ctx.SetDecision((0, 0.1f, 0.9f)); return TaskStatus.Success; });
                    }
                    End();

                    Action("Call on 1,2,3,4,5 in loose game");
                    {
                        IfLoose();
                        IfInGroups(1,2,3,4,5);
                        
                        // Fold:0.4 , Call: 0.6, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.4f, 0.6f, 0)); return TaskStatus.Success; });
                    }
                    End();

                    Action("Call on 1,2,3 in tight game");
                    {
                        IfTight();
                        IfInGroups(1, 2, 3);

                        // Fold:0.3 , Call: 0.7, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.3f, 0.7f, 0)); return TaskStatus.Success; });
                    }
                    End();

                    Action("Call 0.4 of the time on suited hand");
                    {
                        IfSuitedHand();

                        // Fold:0.6 , Call: 0.4, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.6f, 0.4f, 0)); return TaskStatus.Success; });
                    }
                    End();

                    Action("Call on 5,6");
                    {
                        IfInGroups(5, 6);

                        // Fold:0.7 , Call: 0.3, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.7f, 0.3f, 0)); return TaskStatus.Success; });
                    }
                    End();

                    Action("Call on 7");
                    {
                        IfInGroups(7);

                        // Fold:0.8 , Call: 0.2, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.8f, 0.2f, 0)); return TaskStatus.Success; });
                    }
                    End();

                    Action("Fold if no other decision has been made");
                    {
                        // Fold:0.9 , Call: 0.1, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.9f, 0.1f, 0)); return TaskStatus.Success; });
                    }
                    End();
                }
                End();
                
                
                Select("Raises");
                {
                    IfOneOrMoreRaises();
                    
                    Action("Call on 1,2 if tight game");
                    {
                        IfTight();
                        IfInGroups(1, 2);
                        
                        // Fold:0.1 , Call: 0.8, Raise: 0.1
                        Do((ctx) => { ctx.SetDecision((0.1f, 0.8f, 0.1f)); return TaskStatus.Success; });
                    }
                    End();

                    Action("Call on 1,2,3 if loose game");
                    {
                        IfLoose();
                        IfInGroups(1, 2, 3);

                        // Fold:0.2 , Call: 0.7, Raise: 0.1
                        Do((ctx) => { ctx.SetDecision((0.2f, 0.7f, 0.1f)); return TaskStatus.Success; });
                    }
                    End();
                    

                    Action("Fold if no other decision has been made");
                    {
                        // Fold:0.9 , Call: 0.1, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.9f, 0.1f, 0)); return TaskStatus.Success; });
                    }
                    End();

                }
                End();
            }
            End();
            return this;
        }
        
        public PokerDomainBuilder PreflopMiddlePosition()
        {
            // in middle positions we have a wider range of cards that we can play.
            Select("MiddlePosition");
            {
                IfInMiddlePosition();
                IfNoDecisionYet();
                
                Action("Raise on 1,2,3");
                {
                    IfInGroups(1, 2, 3);

                    // Fold:0 , Call: 0.25, Raise: 0.75
                    Do((ctx) => { ctx.SetDecision((0, 0.25f, 0.75f)); return TaskStatus.Success; });
                }
                End();

                Action("Call on 4,5 in tight game");
                {
                    IfTight();
                    IfInGroups(4, 5);

                    // Fold:0 , Call: 0.40, Raise: 0.60
                    Do((ctx) => { ctx.SetDecision((0, 0.6f, 0.4f)); return TaskStatus.Success; });
                }
                End();

                Action("Call on 4,5,6 in loose game");
                {
                    IfLoose();
                    IfInGroups(4, 5, 6);

                    // Fold:0 , Call: 0.60, Raise: 0.40
                    Do((ctx) => { ctx.SetDecision((0, 0.6f, 0.4f)); return TaskStatus.Success; });
                }
                End();


                Action("Call on 5,6");
                {
                    IfInGroups(5, 6);

                    // Fold:0.7 , Call: 0.3, Raise: 0
                    Do((ctx) => { ctx.SetDecision((0.7f, 0.3f, 0)); return TaskStatus.Success; });
                }
                End();

                Action("Call on 7");
                {
                    IfInGroups(7);

                    // Fold:0.8 , Call: 0.2, Raise: 0
                    Do((ctx) => { ctx.SetDecision((0.8f, 0.2f, 0)); return TaskStatus.Success; });
                }
                End();
                
                Action("Fold if no other decision has been made");
                {
                    // Fold:0.9 , Call: 0.1, Raise: 0
                    Do((ctx) => { ctx.SetDecision((0.9f, 0.1f, 0)); return TaskStatus.Success; });
                }
                End();

            }
            End();
            return this;
        }

        public PokerDomainBuilder PreflopLatePosition()
        {
            // in late positions we have excellent position on all bettings rounds,
            // which makes it easier to play a wider range of hands.
            

            Select("LatePosition");
            {
                IfInLatePosition();
                IfNoDecisionYet();

                Action("Raise on 1,2,3,4,5,6,7 if no callers");
                {
                    IfNoCalls();
                    IfInGroups(1, 2, 3, 4, 5, 6, 7);
                    
                    // Fold:0 , Call: 0.2, Raise: 0.8
                    Do((ctx) => { ctx.SetDecision((0, 0.2f, 0.8f)); return TaskStatus.Success; });
                }
                End();

                Action("Raise on 1,2,3");
                {
                    IfInGroups(1, 2, 3);

                    // Fold:0 , Call: 0.2, Raise: 0.8
                    Do((ctx) => { ctx.SetDecision((0, 0.2f, 0.8f)); return TaskStatus.Success; });
                }
                End();

                Action("Raise sometimes on 4");
                {
                    IfInGroups(4);

                    // Fold:0 , Call: 0.5, Raise: 0.5
                    Do((ctx) => { ctx.SetDecision((0, 0.5f, 0.5f)); return TaskStatus.Success; });
                }
                End();

                Action("Call on 5,6");
                {
                    IfInGroups(5, 6);

                    // Fold:0 , Call: 0.9, Raise: 0.1
                    Do((ctx) => { ctx.SetDecision((0, 0.9f, 0.1f)); return TaskStatus.Success; });
                }
                End();

                Action("Call on 7");
                {
                    IfInGroups(7);

                    // Fold:0.8 , Call: 0.2, Raise: 0
                    Do((ctx) => { ctx.SetDecision((0.8f, 0.2f, 0)); return TaskStatus.Success; });
                }
                End();

                Action("Fold if no other decision has been made");
                {
                    // Fold:0.9 , Call: 0.1, Raise: 0
                    Do((ctx) => { ctx.SetDecision((0.9f, 0.1f, 0)); return TaskStatus.Success; });
                }
                End();
            }
            End();

            return this;
        }

        public PokerDomainBuilder PreflopBlindPosition()
        {
            Select("BlindPosition");
            {
                IfInBlindPosition();
                IfNoDecisionYet();

                Select("BigBlind");
                {
                    IfInBigBlindPosition();

                    Select("NoRaises");
                    {
                        IfNoRaises();
                        
                        Action("Raise on 1,2");
                        {
                            IfInGroups(1, 2);

                            // Fold:0 , Call: 0.2, Raise: 0.8
                            Do((ctx) => { ctx.SetDecision((0, 0.2f, 0.8f)); return TaskStatus.Success; });
                        }
                        End();

                        Action("Call on 3,4,5");
                        {
                            IfInGroups(3,4,5);

                            // Fold:0.1 , Call: 0.8, Raise: 0.1
                            Do((ctx) => { ctx.SetDecision((0.1f, 0.8f, 0.1f)); return TaskStatus.Success; });
                        }
                        End();
                    }
                    End();

                    Select("Calls");
                    {
                        Action("Call on 1,2,3,4");
                        {
                            IfInGroups(1, 2 ,3 ,4);

                            // Fold:0.3 , Call: 0.7, Raise: 0
                            Do((ctx) => { ctx.SetDecision((0.3f, 0.7f, 0)); return TaskStatus.Success; });
                        }
                        End();
                    }
                    End();

                    Action("Fold");
                    {

                        // Fold:0.8 , Call: 0.2, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.8f, 0.2f, 0)); return TaskStatus.Success; });
                    }
                    End();
                }
                End();

                Select("SmallBlind");
                {
                    IfInSmallBlindPosition();

                    Select("NoRaises");
                    {
                        IfNoRaises();

                        Action("Raise on 1,2,3,4");
                        {
                            IfInGroups(1, 2, 3 ,4);

                            // Fold:0 , Call: 0.2, Raise: 0.8
                            Do((ctx) => { ctx.SetDecision((0, 0.2f, 0.8f)); return TaskStatus.Success; });
                        }
                        End();

                        Action("Call on 5,6");
                        {
                            IfInGroups(5,6);

                            // Fold:0.1 , Call: 0.8, Raise: 0.1
                            Do((ctx) => { ctx.SetDecision((0.1f, 0.8f, 0.1f)); return TaskStatus.Success; });
                        }
                        End();
                    }
                    End();

                    Select("Calls");
                    {
                        Action("Call on 1,2,3,4");
                        {
                            IfInGroups(1, 2, 3, 4);

                            // Fold:0.3 , Call: 0.7, Raise: 0
                            Do((ctx) => { ctx.SetDecision((0.3f, 0.7f, 0)); return TaskStatus.Success; });
                        }
                        End();
                    }
                    End();

                    Action("Fold");
                    {
                        // Fold:0.8 , Call: 0.2, Raise: 0
                        Do((ctx) => { ctx.SetDecision((0.8f, 0.2f, 0)); return TaskStatus.Success; });
                    }
                    End();
                }
                End();
            }
            End();
            return this;
        }

        #endregion
    }
}
