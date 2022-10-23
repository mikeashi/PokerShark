using FluidHTN;
using FluidHTN.Factory;
using PokerShark.AI.HTN.Domain.Conditions;
using PokerShark.AI.HTN.Domain.Conditions.Game;
using PokerShark.AI.HTN.Domain.Conditions.Pockt;
using PokerShark.AI.HTN.Domain.Conditions.Position;
using PokerShark.AI.HTN.Domain.Conditions.Pot;
using PokerShark.AI.HTN.Domain.Conditions.Round;
using PokerShark.AI.HTN.Tasks;
using PokerShark.AI.HTN.Tasks.CompoundTasks;
using PokerShark.AI.HTN.Utility;
using PokerShark.Poker;
using PokerShark.Poker.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = FluidHTN.TaskStatus;

namespace PokerShark.AI.HTN.Domain
{
    internal class DomainBuilder : BaseDomainBuilder<DomainBuilder, Context, Object>
    {
        #region Constructors
        public DomainBuilder() : base("PockerDomain", new DefaultFactory())
        {
            // default domain factory.
        }
        #endregion

        #region Expected Utility Selector
        public DomainBuilder ExpectedUtilitySelector(String name)
        {
            this.CompoundTask<ExpectedUtilitySelector>(name);
            return this;
        }
        public DomainBuilder VariableCostAction(String name, List<VariableCost> costs)
        {
            if (this.Pointer is ExpectedUtilitySelector compoundTask)
            {
                var parent = new VariableCostTask(costs) { Name = name };
                _domain.Add(compoundTask, parent);
                _pointers.Add(parent);
                return this;
            }
            throw new Exception("Pointer is not a Expected Utility Selector, which is required for adding Utility Actions!");
        }
        #endregion

        #region Conditions

        // position
        public DomainBuilder IfInEarlyPosition()
        {
            Pointer.AddCondition(new InEarly());
            return this;
        }
        public DomainBuilder IfInMiddlePosition()
        {
            Pointer.AddCondition(new InMiddle());
            return this;
        }
        public DomainBuilder IfInLatePosition()
        {
            Pointer.AddCondition(new InMiddle());
            return this;
        }
        public DomainBuilder IfInBlindPosition()
        {
            Pointer.AddCondition(new InBlind());
            return this;
        }
        public DomainBuilder IfInBigBlindPosition()
        {
            Pointer.AddCondition(new InBigBlind());
            return this;
        }
        public DomainBuilder IfInSmallBlindPosition()
        {
            Pointer.AddCondition(new InSmallBlind());
            return this;
        }
        
        // Round
        public DomainBuilder IfInPreflop()
        {
            Pointer.AddCondition(new InPreflop());
            return this;
        }

        // Pot
        public DomainBuilder IfFirstToPot()
        {
            Pointer.AddCondition(new FirstToPot());
            return this;
        }
        public DomainBuilder IfCallsOnly()
        {
            Pointer.AddCondition(new OnlyCalls());
            return this;
        }
        public DomainBuilder IfFirstToRaise()
        {
            Pointer.AddCondition(new FirstToRaise());
            return this;
        }
        public DomainBuilder IfSecondToRaise()
        {
            Pointer.AddCondition(new SecondToRaise());
            return this;
        }
        public DomainBuilder IfTwoOrMoreRaises()
        {
            Pointer.AddCondition(new TwoOrMoreRaises());
            return this;
        }
        public DomainBuilder IfLooseRaiser()
        {
            Pointer.AddCondition(new LooseReiser());
            return this;
        }

        // Game
        public DomainBuilder IfLooseGame()
        {
            Pointer.AddCondition(new IsLoose());
            return this;
        }
        public DomainBuilder IfTightGame()
        {
            Pointer.AddCondition(new IsTight());
            return this;
        }
        public DomainBuilder IfAggressiveGame()
        {
            Pointer.AddCondition(new IsAggressive());
            return this;
        }
        public DomainBuilder IfPassiveGame()
        {
            Pointer.AddCondition(new IsPassive());
            return this;
        }

        // Pocket
        public DomainBuilder IfPocketIsOneOf(params Pocket[] pockets)
        {
            Pointer.AddCondition(new PocketIsOneOf() { Pockets = pockets.ToList() });
            return this;
        }
        public DomainBuilder IfPocketFromGroup(params int[] groups)
        {
            Pointer.AddCondition(new PocketFromGroup() { GroupsQuery = groups.ToList() });
            return this;
        }
        
        // Misc
        public DomainBuilder IfNoDecisionYet()
        {
            Pointer.AddCondition(new NoDecision());
            return this;
        }
        public DomainBuilder Occasionally()
        {
            Pointer.AddCondition(new Occasionally());
            return this;
        }



        #endregion

        #region Preflop
        public DomainBuilder PreflopSequence()
        {
            Select("Preflop");
            {
                IfNoDecisionYet();
                IfInPreflop();
                PreflopEarlyPosition();
                PreflopMiddlePosition();
                PreflopLatePosition();
                PreflopBlindPosition();
                Action("Check raise ocecunally");
                {
                    Condition("If Strong Raise", (ctx) => ctx.GetDecision().Raise > 0.7);
                    Do((ctx) =>
                    {
                        if (new Random().Next(1, 11) > 2)
                        {
                            ctx.SetDecision(new Decision() { Call=1, Fold=0,Raise=0});
                            ctx.SetCheckRaise();
                        }
                        return TaskStatus.Success;
                    });
                }
                End();
            }
            End();

            return this;
        }
        private DomainBuilder PreflopEarlyPosition()
        {
            Select("Preflop Early Position");
            {
                IfInEarlyPosition();

                Select("First to pot");
                {
                    IfFirstToPot();
                    Action("Always raise on AA, KK, QQ, AK, AQ");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Ace),
                                        new Pocket(Rank.King, Rank.King),
                                        new Pocket(Rank.Queen, Rank.Queen),
                                        new Pocket(Rank.Ace, Rank.King),
                                        new Pocket(Rank.Ace, Rank.Queen));
                        Do(AlwaysRaise3Or4BB);
                    }
                    End();

                }
                End();

                Select("No Raises Yet");
                {
                    IfFirstToRaise();
                    Action("Usually Raise on Groups 1,2");
                    {
                        IfPocketFromGroup(1,2);
                        Do(UsuallyRaise3Or4BB);
                    }
                    End();

                    Action("Usually Raise on AQ");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Queen));
                        Do(UsuallyRaise3Or4BB);
                    }
                    End();

                    Action("Sometimes Raise on Groups 3");
                    {
                        IfPocketFromGroup(3);
                        Do(SometimesRaise3Or4BB);
                    }
                    End();

                    Select("Loose Game");
                    {
                        IfLooseGame();
                        
                        Action("Fold AJ, KTs if Aggressive");
                        {
                            IfAggressiveGame();
                            IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack), new Pocket(Rank.King,Rank.Ten,true));
                            Do(Fold);
                        }
                        End();

                        Action("Call or Raise on group 4");
                        {
                            IfPocketFromGroup(4);
                            Do(CallOrRaise2Or3BB);
                        }
                        End();

                        Select("Passive Game");
                        {
                            IfPassiveGame();

                            Action("Call or Raise on 87s 76s 65s");
                            {
                                IfPocketIsOneOf(new Pocket(Rank.Eight, Rank.Seven, true),
                                                new Pocket(Rank.Seven, Rank.Six, true),
                                                new Pocket(Rank.Six, Rank.Five, true));
                                Do(SometimesRaise3Or4BB);
                            }
                            End();

                            Action("Sometimes call on group 5");
                            {
                                IfPocketFromGroup(5);
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
                            IfPocketIsOneOf(new Pocket(Rank.Eight, Rank.Seven, true),
                                            new Pocket(Rank.Seven, Rank.Six, true),
                                            new Pocket(Rank.Six, Rank.Five, true));
                            Do(AlwaysRaise3Or4BB);
                        }
                        End();
                    }
                    End();

                }
                End();

                Select("Second to pot");
                {
                    IfSecondToRaise();

                    Action("Call On AJs, KQs");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace,Rank.Jack,true), new Pocket(Rank.King,Rank.Queen,true));
                        Do(Call);
                    }
                    End();

                    Action("Usually Raise on Groups 1,2");
                    {
                        IfPocketFromGroup(1, 2);
                        Do(UsuallyRaise3Or4BB);
                    }
                    End();

                    Select("Defend against Loose Raiser");
                    {
                        IfLooseRaiser();
                        Action("Rasie on AQ, 99, 88");
                        {
                            IfPocketIsOneOf(new Pocket(Rank.Ace,Rank.Queen), new Pocket(Rank.Nine, Rank.Nine), new Pocket(Rank.Eight,Rank.Eight));
                            Do(UsuallyRaise3Or4BB);
                        }
                        End();
                    }
                    End();
                }
                End();

                Select("Two or more Raises");
                {
                    IfTwoOrMoreRaises();

                    Select("Loose Game");
                    {
                        IfLooseGame();

                        Action("Fold on AQ");
                        {
                            IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Queen));
                            Do(Fold);
                        }
                        End();

                        Action("Usually Raise on Groups 1,2,3");
                        {
                            IfPocketFromGroup(1, 2,3);
                            Do(UsuallyRaise3Or4BB);
                        }
                        End();
                    }
                    End();

                    Select("Tight Game");
                    {
                        IfTightGame();
                        Action("Fold on AJs, KQs");
                        {
                            IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack,true), new Pocket(Rank.King,Rank.Queen,true));
                            Do(Fold);
                        }
                        End();

                        Action("Usually Raise on Groups 1,2");
                        {
                            IfPocketFromGroup(1, 2);
                            Do(UsuallyRaise3Or4BB);
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
        private DomainBuilder PreflopMiddlePosition()
        {
            Select("Preflop Middle Position");
            {
                IfInMiddlePosition();

                Action("Always Raise on Groups 1,2, 3");
                {
                    IfPocketFromGroup(1, 2, 3);
                    Do(AlwaysRaise3Or4BB);
                }
                End();

                Select("Raise on callers");
                {
                    IfCallsOnly();

                    Action("Always Raise on AQ");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Queen));
                        Do(AlwaysRaise3Or4BB);
                    }
                    End();

                    Action("Call on JTs");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Jack, Rank.Ten));
                        Do(Call);
                    }
                    End();

                    Action("Sometimes raise on AJ, KQ");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack), new Pocket(Rank.King,Rank.Queen));
                        Do(SometimesRaise3Or4BB);
                    }
                    End();

                    Action("Sometimes Raise on Groups 3");
                    {
                        IfPocketFromGroup(3);
                        Do(SometimesRaise3Or4BB);
                    }
                    End();

                }
                End();

                Select("First to pot");
                {
                    IfFirstToPot();

                    Action("Always Raise on Groups 3");
                    {
                        IfPocketFromGroup(3);
                        Do(AlwaysRaise3Or4BB);
                    }
                    End();

                    Select("Loose Game");
                    {
                        IfLooseGame();
                        Action("Fold KJ, T8s on aggressive");
                        {
                            IfAggressiveGame();
                            IfPocketIsOneOf(new Pocket(Rank.King,Rank.Jack), new Pocket(Rank.Ten,Rank.Eight,true));
                            Do(Fold);
                        }
                        End();

                        Action("Always Raise on Groups 4,5,6");
                        {
                            IfPocketFromGroup(4,5,6);
                            Do(AlwaysRaise3Or4BB);
                        }
                        End();
                    }
                    End();

                    Select("Tight Game");
                    {
                        IfTightGame();

                        Action("Usually raise groups 4,5");
                        {
                            IfPocketFromGroup(4,5);
                            Do(UsuallyRaise3Or4BB);
                        }
                        End();
                    }
                    End();

                }
                End();

                Select("Second to pot");
                {
                    IfSecondToRaise();

                    Select("Defend against Loose Raiser");
                    {
                        IfLooseRaiser();
                        Action("Rasie on AQ, 99, 88");
                        {
                            IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Queen), new Pocket(Rank.Nine, Rank.Nine), new Pocket(Rank.Eight, Rank.Eight));
                            Do(UsuallyRaise3Or4BB);
                        }
                        End();
                    }
                    End();
                }
                End();

                Select("Two or more Raises");
                {
                    IfTwoOrMoreRaises();

                    Action("Always raise on AA, KK, QQ, AK, AKs");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Ace),
                                        new Pocket(Rank.King, Rank.King),
                                        new Pocket(Rank.Queen, Rank.Queen),
                                        new Pocket(Rank.Ace, Rank.King),
                                        new Pocket(Rank.Ace, Rank.King,true));
                        Do(AlwaysRaise3Or4BB);
                    }
                    End();

                    Action("Occasionally Raise on T9s, 88");
                    {
                       Occasionally();
                       IfPocketIsOneOf(new Pocket(Rank.Ten, Rank.Nine,true),new Pocket(Rank.Eight, Rank.Eight));
                       Do(AlwaysRaise3Or4BB);
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
        private DomainBuilder PreflopLatePosition()
        {
            Select("Preflop Late Position");
            {
                IfInLatePosition();

                Action("Always Raise on 1,2,3,4,5,6,7 if no raises");
                {
                    IfFirstToRaise();
                    IfPocketFromGroup(1, 2, 3, 4, 5, 6, 7);
                    Do(AlwaysRaise3Or4BB);
                }
                End();

                Action("Always Raise on 1,2,3");
                {
                    IfPocketFromGroup(1, 2, 3);
                    Do(AlwaysRaise3Or4BB);
                }
                End();

                Action("Sometimes Raise on 4");
                {
                    IfPocketFromGroup(4);
                    Do(SometimesRaise3Or4BB);
                }
                End();

                Action("Call on 5,6");
                {
                    IfPocketFromGroup(5,6);
                    Do(Call);
                }
                End();

                Action("Call on 7");
                {
                    IfPocketFromGroup(7);
                    Do(SometimesCall);
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
        private DomainBuilder PreflopBlindPosition()
        {
            Select("Preflop Blind Position");
            {
                IfInBlindPosition();

                Action("Always Raise on 1, 2, 3, 4");
                {
                    IfPocketFromGroup(1, 2, 3, 4);
                    Do(AlwaysRaise3Or4BB);
                }
                End();

                Select("First to pot");
                {
                    IfFirstToPot();
                    Action("Call on 5");
                    {
                        IfPocketFromGroup(5);
                        Do(Call);
                    }
                    End();
                }
                End();

                Select("No Raises Yet");
                {
                    IfFirstToRaise();
                    Action("Call on 5,6");
                    {
                        IfPocketFromGroup(5,6);
                        Do(Call);
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
        #endregion

        #region Postflop
        public DomainBuilder PostflopSequence(Context context)
        {
            // get round information
            var game = context.GetGame();
            var currentRound = game.CurrentRound;
            var bb = game.BigBlind;
            var opponents = game.GetNotFoldedOpponentModels();
            var roundState = currentRound?.RoundState;
            var pocket = currentRound?.Pocket;
            var board = currentRound?.Board;
            var min = context.GetMinPossibleRaiseAmount();
            var max = context.GetMaxPossibleRaiseAmount();
            var pot = context.GetPotAmount();
            var callAmount = context.GetCallAmount();

            // dont calculate odds if game is still in preflop.
            if (roundState == RoundState.Preflop)
                return this;

            // calculate hand strength
            var ehs = Oracle.EffectiveHandStrength(pocket, board, opponents);

            // Raise Actions
            for (int i =0; min + i * bb < max; i++)
            {
                // Define action with correlating odds of the raise amount.
                VariableCostAction("Raise: " + (min + i * bb), Oracle.RaiseOdds(ehs, min + i * bb, pot));
                    Do((ctx) => {
                        ctx.SetDecision(new Decision() { Call =0.2, Raise=0.8, Fold=0 });
                        ctx.SetRaiseAmount(min + i * bb);
                        return TaskStatus.Success;
                    });
                End();
            }

            // Call Action
            VariableCostAction("Call: "+ callAmount, Oracle.CallOdds(ehs, callAmount, pot));
                Do(Call);
            End();

            // Fold Action
            VariableCostAction("Fold", Oracle.FoldOdds(opponents));
                Do(Fold);
            End();
            return this;
        }
        #endregion

        #region Effects
        public static TaskStatus AlwaysRaise3Or4BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0, Raise = 1 });
            ctx.SetRaiseAmount((3, 0.3f), (4, 0.7f));
            return TaskStatus.Success;
        }
        public static TaskStatus UsuallyRaise3Or4BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.1, Raise = 0.9 });
            ctx.SetRaiseAmount((3, 0.3f), (4, 0.7f));
            return TaskStatus.Success;
        }
        public static TaskStatus SometimesRaise3Or4BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.3, Raise = 0.7 });
            ctx.SetRaiseAmount((3, 0.5f), (4, 0.5f));
            return TaskStatus.Success;
        }
        public static TaskStatus CallOrRaise2Or3BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.4, Raise = 0.6 });
            ctx.SetRaiseAmount((2, 0.6f), (3, 0.4f));
            return TaskStatus.Success;
        }
        public static TaskStatus SometimesCall(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0.3, Call = 0.6, Raise = 0.1 });
            return TaskStatus.Success;
        }
        public static TaskStatus Call(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.9, Raise = 0.1 });
            return TaskStatus.Success;
        }
        public static TaskStatus Fold(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0.7, Call = 0.3, Raise = 0 });
            return TaskStatus.Success;
        }
        #endregion
    }
}
