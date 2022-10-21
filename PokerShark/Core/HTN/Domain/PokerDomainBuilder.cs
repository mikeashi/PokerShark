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
        public PokerDomainBuilder IfCalls()
        {
            var condition = new IfCallsCondition();
            Pointer.AddCondition(condition);
            return this;
        }
        public PokerDomainBuilder IfNoCalls()
        {
            var condition = new NoCallesCondition();
            Pointer.AddCondition(condition);
            return this;
        }
        public PokerDomainBuilder IfAggressive()
        {
            var condition = new AggressiveCondition();
            Pointer.AddCondition(condition);
            return this;
        }
        public PokerDomainBuilder IfPassive()
        {
            var condition = new PassiveCondition();
            Pointer.AddCondition(condition);
            return this;
        }
        public PokerDomainBuilder Occasionally()
        {
            var condition = new OccasionallyCondition();
            Pointer.AddCondition(condition);
            return this;
        }
        public PokerDomainBuilder IfOneRaiseOnly()
        {
            var condition = new OneRaiseCondition();
            Pointer.AddCondition(condition);
            return this;
        }
        public PokerDomainBuilder IfLooseRaiser()
        {
            var condition = new LooseRaiserCondition();
            Pointer.AddCondition(condition);
            return this;
        }
        public PokerDomainBuilder IfTwoOrMoreRaises()
        {
            var condition = new TwoOrMoreRaisesCondition();
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
                Action("Check raise ocecunally");
                {
                    Condition("If Strong Raise", (ctx) => ctx.GetDecision().Raise > 0.5f);
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
            return this;
        }

        public PokerDomainBuilder PostflopSequence(PokerContext ctx)
        {
            if (ctx.GetCurrentRound().StreetState == PyPoker.StreetState.Preflop)
                return this;
            
            ExpectedUtilitySelector("Test");
            {
                VariableCostAction("max raise", ctx.RaiseOdds(ctx.GetMaxRaiseAmount()));
                Do((ctx) =>
                {
                    ctx.SetDecision((0, 0.2f, 0.8f));
                    return TaskStatus.Success;
                });
                End();
                
                VariableCostAction("mid raise", ctx.RaiseOdds(ctx.GetMaxRaiseAmount() + ctx.GetMinRaiseAmount() /2 ));
                Do((ctx) =>
                {
                    ctx.SetDecision((0, 0.2f, 0.8f));
                    return TaskStatus.Success;
                });
                End();
                
                VariableCostAction("min raise", ctx.RaiseOdds(ctx.GetMinRaiseAmount()));
                    Do((ctx) =>
                    {
                        ctx.SetDecision((0, 0.2f, 0.8f));
                        return TaskStatus.Success;
                    });
                End();
                
                VariableCostAction("call", ctx.CallOdds());
                    Do((ctx) => {
                        ctx.SetDecision((0, 0.8f, 0.2f));
                        return TaskStatus.Success;
                    });
                End();
                
                //VariableCostAction("raise bluff", ctx.BluffOdds(ctx.GetRaiseAmount()));
                //    IfNoDecisionYet();
                //    Do((ctx) => {
                //        ctx.SetDecision((0, 0.2f, 0.8f));
                //        return TaskStatus.Success;
                //    });
                //End();
                //VariableCostAction("call bluff", ctx.BluffOdds(ctx.GetCallAmount()));
                //    IfNoDecisionYet();
                //    Do((ctx) => {
                //        ctx.SetDecision((0, 0.8f, 0.2f));
                //        return TaskStatus.Success;
                //    });
                //End();
                
                VariableCostAction("fold", ctx.FoldOdds());
                        Do((ctx) => {
                            ctx.SetDecision((0.8f, 0.2f, 0));
                            return TaskStatus.Success;
                        });
                End();
            }
            End();
            return this;
        }

        public PokerDomainBuilder PreflopEarlyPosition()
        {
            Select("EarlyPosition");
            {
                IfInEarlyPosition();
                IfNoDecisionYet();

                // No one called yet.
                Select("No Calls Yet");
                {
                    IfNoCalls();
                    Action("Always raise on AA, KK, QQ, AK, AQ");
                    {
                        IfPocketIn( new PocketHand(Rank.Ace, Rank.Ace),
                                    new PocketHand(Rank.King, Rank.King),
                                    new PocketHand(Rank.Queen, Rank.Queen),
                                    new PocketHand(Rank.Ace, Rank.King),
                                    new PocketHand(Rank.Ace,Rank.Queen));
                        Do(AlwaysRaise);
                    }
                    End();
                }
                End();

                // No one raised yet
                Select("No Raises Yet");
                {
                    IfNoRaises();

                    Action("Usually Raise on Groups 1,2");
                    {
                        IfInGroups(1, 2);
                        Do(UsuallyRaise);
                    }
                    End();

                    Action("Usually Raise on AQ");
                    {
                        IfPocketIn(new PocketHand(Rank.Ace, Rank.Queen));
                        Do(UsuallyRaise);
                    }
                    End();

                    Action("Sometimes Raise on Group 3");
                    {
                        IfInGroups(3);
                        Do(SometimesRaise);
                    }
                    End();

                    Select("Loose Game");
                    {
                        IfLoose();

                        Action("Fold AJ, KTs if Aggressive");
                        {
                            IfAggressive();
                            IfPocketIn(new PocketHand(Rank.Ace,Rank.Jack), new PocketHand(Rank.King, Rank.Ten, true));
                            Do(Fold);
                        }
                        End();

                        Action("Call or Raise on group 4");
                        {
                            IfInGroups(4);
                            Do(CallOrRaise);
                        }
                        End();

                        Select("Passive Game");
                        {
                            IfPassive();

                            Action("Call or Raise on 87s 76s 65s");
                            {
                                IfPocketIn( new PocketHand(Rank.Eight, Rank.Seven, true),
                                            new PocketHand(Rank.Seven, Rank.Six, true),
                                            new PocketHand(Rank.Six, Rank.Five, true));
                                Do(SometimesRaise);
                            }
                            End();

                            Action("Sometimes call on group 5");
                            {
                                IfInGroups(5);
                                Do(SometimesCall);
                            }
                            End();
                        }
                        End();
                    }
                    End();

                    Select("Occasionally Raise suited connectors");
                    {
                        Action("Raise on suited connectors");
                        {
                            Occasionally();
                            IfPocketIn(new PocketHand(Rank.Eight, Rank.Seven, true),
                                            new PocketHand(Rank.Seven, Rank.Six, true),
                                            new PocketHand(Rank.Six, Rank.Five, true));
                            Do(AlwaysRaise);
                        }
                        End();
                    }
                    End();

                }
                End();

                // Only one raise
                Select("One Raise");
                {
                    IfOneRaiseOnly();
                    
                    Action("Call On AJs, KQs");
                    {
                        IfPocketIn(new PocketHand(Rank.Ace,Rank.Jack, true), new PocketHand(Rank.King, Rank.Queen, true));
                        Do(Call);
                    }
                    End();

                    Action("Usually Raise on group 1,2");
                    {
                        IfInGroups(1, 2);
                        Do(UsuallyRaise);
                    }
                    End();

                    Select("Defend against Loose Raiser");
                    {
                        IfLooseRaiser();
                        Action("Rasie on AQ, 99, 88");
                        {
                            IfPocketIn( new PocketHand(Rank.Ace, Rank.Queen),
                                        new PocketHand(Rank.Nine, Rank.Nine),
                                        new PocketHand(Rank.Eight, Rank.Eight));
                            Do(UsuallyRaise);
                        }
                        End();
                    }
                    End();
                }
                End();

                // Two or more raises
                Select("Two or more Raises");
                {
                    IfTwoOrMoreRaises();

                    Select("Loose Game");
                    {
                        IfLoose();
                        Action("Fold on AQ");
                        {
                            IfPocketIn(new PocketHand(Rank.Ace, Rank.Queen));
                            Do(Fold);
                        }
                        End();

                        Action("Usually raise on group 1,2,3");
                        {
                            IfInGroups(1, 2, 3);
                            Do(UsuallyRaise);
                        }
                        End();
                    }
                    End();


                    Select("Tight Game");
                    {
                        IfTight();
                        Action("Fold on AJs, KQs");
                        {
                            IfPocketIn(new PocketHand(Rank.Ace, Rank.Jack, true), new PocketHand(Rank.King, Rank.Queen, true));
                            Do(Fold);
                        }
                        End();

                        Action("Usually raise on group 1,2");
                        {
                            IfInGroups(1, 2);
                            Do(UsuallyRaise);
                        }
                        End();
                    }
                    End();
                }
                End();

                Action("Fold if no decision was made");
                {
                    IfNoDecisionYet();
                    Do(Fold);
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

                Action("Always Raise with group 1,2");
                {
                    IfInGroups(1, 2);
                    Do(AlwaysRaise);
                }
                End();

                Select("Raise on Callers");
                {
                    IfCalls();

                    Action("Always Raise on AQ");
                    {
                        IfPocketIn(new PocketHand(Rank.Ace, Rank.Queen));
                        Do(AlwaysRaise);
                    }
                    End();

                    Action("Call on JTs");
                    {
                        IfPocketIn(new PocketHand(Rank.Jack, Rank.Ten , true));
                        Do(Call);
                    }
                    End();

                    Action("Sometimes Raise on AJ, KQ");
                    {
                        IfPocketIn(new PocketHand(Rank.Ace, Rank.Jack), new PocketHand(Rank.King, Rank.Queen));
                        Do(SometimesRaise);
                    }
                    End();

                    Action("Sometimes Raise on group 3");
                    {
                        IfInGroups(3);
                        Do(SometimesRaise);
                    }
                    End();

                }
                End();

                Select("No Raises");
                {
                    IfNoRaises();

                    Action("Always raise group 3");
                    {
                        IfInGroups(3);
                        Do(AlwaysRaise);
                    }
                    End();

                    Select("Loose game");
                    {
                        IfLoose();

                        Action("Fold KJ, T8s on aggressive");
                        {
                            IfAggressive();
                            IfPocketIn(new PocketHand(Rank.King, Rank.Jack), new PocketHand(Rank.Ten, Rank.Eight, true));
                            Do(Fold);
                        }
                        End();

                        Action("Always raise groups 4,5,6");
                        {
                            IfInGroups(4, 5, 6);
                            Do(AlwaysRaise);
                        }
                        End();
                    }
                    End();

                    Select("Tight game");
                    {
                        IfTight();

                        Action("Usually raise groups 4,5");
                        {
                            IfInGroups(4, 5);
                            Do(UsuallyRaise);
                        }
                        End();
                    }
                    End();
                }
                End();

                Select("One Raiser");
                {
                    IfOneRaiseOnly();

                    Action("Defend against Loose Raiser");
                    {
                        IfLooseRaiser();
                        IfPocketIn(new PocketHand(Rank.Ace, Rank.Queen),
                                       new PocketHand(Rank.Nine, Rank.Nine),
                                       new PocketHand(Rank.Eight, Rank.Eight));
                        Do(UsuallyRaise);
                    }
                    End();
                }
                End();

                Select("Two or more Raises");
                {
                    IfTwoOrMoreRaises();

                    Action("Always raise on AA, KK, QQ, AKs, AK");
                    {
                        IfPocketIn( new PocketHand(Rank.Ace, Rank.Ace),
                                    new PocketHand(Rank.King, Rank.King),
                                    new PocketHand(Rank.Queen, Rank.Queen),
                                    new PocketHand(Rank.Ace, Rank.King, true),
                                    new PocketHand(Rank.Ace, Rank.King));
                        Do(AlwaysRaise);
                    }
                    End();

                    Action("Occasionally raise on T9s, 88");
                    {
                        Occasionally();
                        IfPocketIn(new PocketHand(Rank.Ten, Rank.Nine, true), new PocketHand(Rank.Eight, Rank.Eight));
                        Do(AlwaysRaise);
                    }
                    End();

                }
                End();
                
                Action("Fold if no decision was made");
                {
                    IfNoDecisionYet();
                    Do(Fold);
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

                Action("Raise on 1,2,3,4,5,6,7 if no raises");
                {
                    IfNoRaises();
                    IfInGroups(1, 2, 3, 4, 5, 6, 7);
                    
                    // Fold:0 , Call: 0.2, Raise: 0.8
                    Do(AlwaysRaise);
                }
                End();

                Action("Raise on 1,2,3");
                {
                    IfInGroups(1, 2, 3);

                    // Fold:0 , Call: 0.2, Raise: 0.8
                    Do(AlwaysRaise);
                }
                End();

                Action("Raise sometimes on 4");
                {
                    IfInGroups(4);

                    // Fold:0 , Call: 0.5, Raise: 0.5
                    Do(SometimesRaise);
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
                        IfCalls();
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
                        IfCalls();
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

        #region actions
       
        public static TaskStatus Fold(PokerContext context)
        {
            context.SetDecision((1, 0, 0));
            return TaskStatus.Success;
        }

        public static TaskStatus AlwaysRaise(PokerContext context)
        {
            context.SetDecision((0, 0, 1));
            // Raise 4xBB 80%, 3xBB 20% of the time 
            context.SetRaiseAmount((3, 0.2f), (4, 0.8f));
            return TaskStatus.Success;
        }

        public static TaskStatus UsuallyRaise(PokerContext context)
        {
            context.SetDecision((0, 0.1f, 0.9f));
            // Raise 4xBB 80%, 3xBB 20% of the time 
            context.SetRaiseAmount((3, 0.2f), (4, 0.8f));
            return TaskStatus.Success;
        }

        public static TaskStatus SometimesRaise(PokerContext context)
        {
            context.SetDecision((0, 0.3f, 0.7f));
            // Raise 4xBB 50%, 3xBB 50% of the time 
            context.SetRaiseAmount((3, 0.5f), (4, 0.5f));
            return TaskStatus.Success;
        }

        public static TaskStatus CallOrRaise(PokerContext context)
        {
            context.SetDecision((0, 0.4f, 0.6f));
            // Raise 2xBB 60%, 3xBB 40% of the time 
            context.SetRaiseAmount((2, 0.6f), (3, 0.4f));
            return TaskStatus.Success;
        }
        public static TaskStatus SometimesCall(PokerContext context)
        {
            context.SetDecision((0.4f, 0.6f, 0));
            return TaskStatus.Success;
        }

        public static TaskStatus Call(PokerContext context)
        {
            context.SetDecision((0, 1, 0));
            return TaskStatus.Success;
        }
        
        #endregion
    }
}
