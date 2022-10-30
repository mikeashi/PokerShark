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
    internal class DomainBuilderOld : BaseDomainBuilder<DomainBuilderOld, Context, Object>
    {
        #region Constructors
        public DomainBuilderOld() : base("PockerDomain", new DefaultFactory())
        {
            // default domain factory.
        }
        #endregion

        #region Expected Utility Selector
        public DomainBuilderOld ExpectedUtilitySelector(String name)
        {
            this.CompoundTask<ExpectedUtilitySelector>(name);
            return this;
        }
        public DomainBuilderOld VariableCostAction(String name, List<VariableCost> costs)
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
        public DomainBuilderOld IfInEarlyPosition()
        {
            Pointer.AddCondition(new InEarly());
            return this;
        }
        public DomainBuilderOld IfInMiddlePosition()
        {
            Pointer.AddCondition(new InMiddle());
            return this;
        }
        public DomainBuilderOld IfInLatePosition()
        {
            Pointer.AddCondition(new InLate());
            return this;
        }
        public DomainBuilderOld IfInBlindPosition()
        {
            Pointer.AddCondition(new InBlind());
            return this;
        }
        public DomainBuilderOld IfInBigBlindPosition()
        {
            Pointer.AddCondition(new InBigBlind());
            return this;
        }
        public DomainBuilderOld IfInSmallBlindPosition()
        {
            Pointer.AddCondition(new InSmallBlind());
            return this;
        }

        // Round
        public DomainBuilderOld IfInPreflop()
        {
            Pointer.AddCondition(new InPreflop());
            return this;
        }

        // Pot
        public DomainBuilderOld IfFirstToPot()
        {
            Pointer.AddCondition(new FirstToPot());
            return this;
        }
        public DomainBuilderOld IfCallsOnly()
        {
            Pointer.AddCondition(new OnlyCalls());
            return this;
        }
        public DomainBuilderOld IfFirstToRaise()
        {
            Pointer.AddCondition(new FirstToRaise());
            return this;
        }
        public DomainBuilderOld IfSecondToRaise()
        {
            Pointer.AddCondition(new SecondToRaise());
            return this;
        }
        public DomainBuilderOld IfTwoOrMoreRaises()
        {
            Pointer.AddCondition(new TwoOrMoreRaises());
            return this;
        }
        public DomainBuilderOld IfLooseRaiser()
        {
            Pointer.AddCondition(new LooseReiser());
            return this;
        }

        // Game
        public DomainBuilderOld IfLooseGame()
        {
            Pointer.AddCondition(new IsLoose());
            return this;
        }
        public DomainBuilderOld IfTightGame()
        {
            Pointer.AddCondition(new IsTight());
            return this;
        }
        public DomainBuilderOld IfAggressiveGame()
        {
            Pointer.AddCondition(new IsAggressive());
            return this;
        }
        public DomainBuilderOld IfPassiveGame()
        {
            Pointer.AddCondition(new IsPassive());
            return this;
        }
        public DomainBuilderOld IfOneOpponent()
        {
            Pointer.AddCondition(new OneOpponent());
            return this;
        }


        // Pocket
        public DomainBuilderOld IfPocketIsOneOf(params Pocket[] pockets)
        {
            Pointer.AddCondition(new PocketIsOneOf() { Pockets = pockets.ToList() });
            return this;
        }
        public DomainBuilderOld IfPocketFromGroup(params int[] groups)
        {
            Pointer.AddCondition(new PocketFromGroup() { GroupsQuery = groups.ToList() });
            return this;
        }

        // Misc
        public DomainBuilderOld IfNoDecisionYet()
        {
            Pointer.AddCondition(new NoDecision());
            return this;
        }
        public DomainBuilderOld Occasionally()
        {
            Pointer.AddCondition(new Occasionally());
            return this;
        }
        public DomainBuilderOld IfRaiseOrCallDecision()
        {
            Pointer.AddCondition(new RaiseOrCallDecision());
            return this;
        }
        public DomainBuilderOld IfTooFishy()
        {
            Pointer.AddCondition(new TooFishy());
            return this;
        }
        public DomainBuilderOld IfCallingFish()
        {
            Pointer.AddCondition(new RiskAverseAttitude());
            return this;
        }






        #endregion

        #region Preflop
        public DomainBuilderOld PreflopSequence()
        {
            IfNoDecisionYet();
            IfInPreflop();
            PreflopEarlyPosition();
            PreflopMiddlePosition();
            PreflopLatePosition();
            PreflopBlindPosition();
            CheckRaiseSelector();
            FishyTrap();
            return this;
        }
        private DomainBuilderOld PreflopEarlyPosition()
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
                        IfPocketFromGroup(1, 2);
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
                            IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack), new Pocket(Rank.King, Rank.Ten, true));
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
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack, true), new Pocket(Rank.King, Rank.Queen, true));
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
                            IfPocketFromGroup(1, 2, 3);
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
                            IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack, true), new Pocket(Rank.King, Rank.Queen, true));
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
        private DomainBuilderOld PreflopMiddlePosition()
        {
            Select("Preflop Middle Position");
            {
                IfInMiddlePosition();

                Action("Always Raise on Groups 1,2");
                {
                    IfPocketFromGroup(1, 2);
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
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack), new Pocket(Rank.King, Rank.Queen));
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
                            IfPocketIsOneOf(new Pocket(Rank.King, Rank.Jack), new Pocket(Rank.Ten, Rank.Eight, true));
                            Do(Fold);
                        }
                        End();

                        Action("Always Raise on Groups 4,5,6");
                        {
                            IfPocketFromGroup(4, 5, 6);
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
                            IfPocketFromGroup(4, 5);
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
                                        new Pocket(Rank.Ace, Rank.King, true));
                        Do(AlwaysRaise3Or4BB);
                    }
                    End();

                    Action("Occasionally Raise on T9s, 88");
                    {
                        Occasionally();
                        IfPocketIsOneOf(new Pocket(Rank.Ten, Rank.Nine, true), new Pocket(Rank.Eight, Rank.Eight));
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
        private DomainBuilderOld PreflopLatePosition()
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
                    IfPocketFromGroup(5, 6);
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
        private DomainBuilderOld PreflopBlindPosition()
        {
            Select("Preflop Blind Position");
            {
                IfInBlindPosition();

                Select("Big Blind");
                {
                    IfInBigBlindPosition();

                    Select("First to pot");
                    {
                        IfFirstToPot();
                        Action("Always Raise on 1, 2");
                        {
                            IfPocketFromGroup(1, 2);
                            Do(AlwaysRaise3Or4BB);
                        }
                        End();

                        Action("Call on 3,4,5");
                        {
                            IfPocketFromGroup(3, 4, 5);
                            Do(Call);
                        }
                        End();
                    }
                    End();

                    Select("No Raises Yet");
                    {
                        IfFirstToRaise();

                        Action("Call on 1,2,3,4");
                        {
                            IfPocketFromGroup(1, 2, 3, 4);
                            Do(Call);
                        }
                        End();
                    }
                    End();
                }
                End();

                Select("Small Blind");
                {
                    IfInSmallBlindPosition();

                    Select("First to pot");
                    {
                        IfFirstToPot();
                        Action("Always Raise on 1, 2,3,4");
                        {
                            IfPocketFromGroup(1, 2);
                            Do(AlwaysRaise3Or4BB);
                        }
                        End();

                        Action("Call on 5,6");
                        {
                            IfPocketFromGroup(5, 6);
                            Do(Call);
                        }
                        End();
                    }
                    End();

                    Select("No Raises Yet");
                    {
                        IfFirstToRaise();

                        Action("Call on 1,2,3,4");
                        {
                            IfPocketFromGroup(1, 2, 3, 4);
                            Do(Call);
                        }
                        End();
                    }
                    End();
                }
                End();

                //Select("Only One Opponent");
                //{
                //    IfNoDecisionYet();
                //    IfOneOpponent();
                //    Action("Call on 1,2,3,4,5");
                //    {
                //        IfPocketFromGroup(1, 2, 3, 4, 5);
                //        Do(Call);
                //    }
                //    End();
                //}
                //End();

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
        public DomainBuilderOld PostflopSequence(Context context)
        {
            ExpectedUtilitySelector("Postflop");
            {
                PostflopTasks(context);
            }
            End();
            return this;
        }
        public DomainBuilderOld PostflopTasks(Context context)
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
            var ehsC = Oracle.EffectiveHandStrength(pocket, board, opponents);
            var ehs = ehsC.Item1;

            // log evaluation 
            var logger = new StringBuilder();
            logger.AppendLine(roundState + ", ehs: " + ehsC + ", pot: " + pot);

            // Raise Actions
            for (int i = 0; min + i * bb < max; i++)
            {
                // dont consider big raises with low odds
                if (ehs < 0.6 && min + i * bb < max / 2)
                    break;

                logger.AppendLine(" raise: " + (min + i * bb) + ", " + String.Join(" , ",Oracle.RaiseOdds(ehs, min + i * bb, pot)));

                // Define action with correlating odds of the raise amount.
                VariableCostAction("Raise: " + (min + i * bb), Oracle.RaiseOdds(ehs, min + i * bb, pot));
                Do((ctx) =>
                {
                    if(ctx.GetRoundState() == RoundState.Flop)
                    {
                        // override premature raise
                        ctx.SetDecision(new Decision() { Call = 1, Raise = 0, Fold = 0 });
                    }
                    else if (ctx.GetRoundState() == RoundState.Turn)
                    {
                        ctx.SetDecision(new Decision() { Call = 0.9, Raise = 0.1, Fold = 0 });
                    }
                    else if (ctx.GetRoundState() == RoundState.River)
                    {
                        ctx.SetDecision(new Decision() { Call = 0.5, Raise = 0.5, Fold = 0 });
                    }
                    else
                    {
                        ctx.SetDecision(new Decision() { Call = 0.3, Raise = 0.7, Fold = 0 });
                    }

                    ctx.SetRaiseAmount(min + i * bb);
                    return TaskStatus.Success;
                });
                End();
            }


            // Call Action
            logger.AppendLine(" call: " + callAmount + ", " + String.Join(" , ", Oracle.CallOdds(ehs, callAmount, pot, opponents, bb)));
            VariableCostAction("Call: " + callAmount, Oracle.CallOdds(ehs, callAmount, pot, opponents, bb));
                Do((ctx) =>
                {
                        ctx.SetDecision(new Decision() { Call = 0.8, Raise = 0.2, Fold = 0 });
                        ctx.SetRaiseAmount(ctx.GetMinPossibleRaiseAmount());
                        return TaskStatus.Success;
                });
            End();

            // Fold Action
            logger.AppendLine(" fold " + String.Join(" , ", Oracle.FoldOdds(opponents)));
            VariableCostAction("Fold", Oracle.FoldOdds(opponents));
            Do(Fold);
            End();
            
            // log odds
            currentRound?.OddsLog.Add(logger.ToString());
            return this;
        }
        #endregion

        #region Too High Cut
        public static void TooHighCut(Context ctx)
        {
            // CUT if too high
            var stack = ctx.GetCurrentStack();
            var raise = ctx.GetMinPossibleRaiseAmount();
            var call = ctx.GetCallAmount();
            var amount = raise > call ? raise : call;
            if(amount > (stack / 2))
                ctx.SetDecision(new Decision() { Call = 0, Fold = 1, Raise = 0 });
        }
        #endregion

        #region FishyTrap
        public DomainBuilderOld FishyTrap()
        {
            Select("Catch some fish :)");
            {
                IfInPreflop();
                IfTooFishy();
                Action("Always raise on AA, KK, QQ, AK, AQ");
                {
                    IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Ace),
                                    new Pocket(Rank.King, Rank.King),
                                    new Pocket(Rank.Queen, Rank.Queen),
                                    new Pocket(Rank.Ace, Rank.King),
                                    new Pocket(Rank.Ace, Rank.Queen));
                    Do(RaiseLikeShark);
                }
                End();

                Action("Always Raise on Groups 1,2");
                {
                    IfPocketFromGroup(1, 2);
                    Do(RaiseLikeShark);
                }
                End();

                Select("Catch none bold fish");
                {
                    IfCallingFish();
                    Action("Always Raise on Groups 3,4");
                    {
                        IfPocketFromGroup(3,4);
                        Do(RaiseLikeShark);
                    }
                    End();

                    Action("Always call");
                    {
                        Do(CallAFish);
                    }
                    End();
                }
                End();

            }
            End();
            return this;
        }
        #endregion

        #region Check-Raise
        public DomainBuilderOld CheckRaiseCutSelector()
        {
            Select("Check-Raise");
            {
                Condition("Check-Raise", (ctx) => ctx.GetCheckRaise());
                Action("Always raise");
                {
                    Do((ctx) =>
                    {
                        ctx.UnsetCheckRaise();
                        ctx.SetDecision(new Decision() { Call = 0, Fold = 0, Raise = 1 });
                        ctx.SetRaiseAmount(ctx.GetMaxPossibleRaiseAmount());
                        return TaskStatus.Success;
                    });
                }
                End();
            }
            End();
            return this;
        }
        public DomainBuilderOld CheckRaiseSelector()
        {
            Select("Check-Raise");
            {
                Action("Check raise ocecunally");
                {
                    Condition("If Strong Raise", (ctx) => ctx.GetDecision().Raise > 0.7);
                    Do((ctx) =>
                    {
                        if (new Random().Next(1, 11) > 2)
                        {
                            ctx.SetDecision(new Decision() { Call = 1, Fold = 0, Raise = 0 });
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
        #endregion

        #region Effects
        private static TaskStatus AlwaysRaise3Or4BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0, Raise = 1 });
            ctx.SetRaiseAmount((3, 0.3f), (4, 0.7f));
            TooHighCut(ctx);
            return TaskStatus.Success;
        }
        private static TaskStatus UsuallyRaise3Or4BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.1, Raise = 0.9 });
            ctx.SetRaiseAmount((3, 0.3f), (4, 0.7f));
            TooHighCut(ctx);
            return TaskStatus.Success;
        }
        private static TaskStatus SometimesRaise3Or4BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.3, Raise = 0.7 });
            ctx.SetRaiseAmount((3, 0.5f), (4, 0.5f));
            TooHighCut(ctx);
            return TaskStatus.Success;
        }
        private static TaskStatus CallOrRaise2Or3BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.4, Raise = 0.6 });
            ctx.SetRaiseAmount((2, 0.6f), (3, 0.4f));
            TooHighCut(ctx);
            return TaskStatus.Success;
        }
        private static TaskStatus SometimesCall(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0.4, Call = 0.6, Raise = 0 });
            ctx.SetRaiseAmount(ctx.GetMinPossibleRaiseAmount());
            TooHighCut(ctx);
            return TaskStatus.Success;
        }
        private static TaskStatus Call(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.9, Raise = 0.1 });
            ctx.SetRaiseAmount(ctx.GetMinPossibleRaiseAmount());
            TooHighCut(ctx);
            return TaskStatus.Success;
        }
        private static TaskStatus Fold(Context ctx)
        {
            if(ctx.GetCallAmount() == 0)
            {
                // always call instade of folding
                // when it does not cost anything to call.
                ctx.SetDecision(new Decision() { Fold = 0, Call = 1, Raise = 0 });
            }
            else
            {
                ctx.SetDecision(new Decision() { Fold = 0.7, Call = 0.3, Raise = 0 });
            }
            TooHighCut(ctx);
            return TaskStatus.Success;
        }
        private static TaskStatus RaiseAFish(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0, Raise = 1 });
            ctx.SetRaiseAmount((4, 0.3f), (5, 0.7f));
            return TaskStatus.Success;
        }
        private static TaskStatus RaiseLikeShark(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0, Raise = 1 });
            ctx.SetRaiseAmount(ctx.GetMaxPossibleRaiseAmount());
            return TaskStatus.Success;
        }
        private static TaskStatus CallAFish(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 1, Raise = 0 });
            return TaskStatus.Success;
        }
        #endregion
    }
}
