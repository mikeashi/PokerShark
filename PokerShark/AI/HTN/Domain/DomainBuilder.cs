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
using System.Text;
using RoundState = PokerShark.Poker.RoundState;
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
        public DomainBuilder VariableUtilityAction(String name, List<VariableUtility> utilites)
        {
            if (this.Pointer is ExpectedUtilitySelector compoundTask)
            {
                var parent = new VariableUtilityTask(utilites) { Name = name };
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
            Pointer.AddCondition(new InLate());
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
        public DomainBuilder IfThreeBBCall()
        {
            Pointer.AddCondition(new ThreeBBCall());
            return this;
        }
        public DomainBuilder IfZeroCostCall()
        {
            Pointer.AddCondition(new ZeroCostCall());
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
        public DomainBuilder IfOneOpponent()
        {
            Pointer.AddCondition(new OneOpponent());
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
        public DomainBuilder IfPocketEquityGreaterThan(double equity)
        {
            Pointer.AddCondition(new PocketEquityGreaterThan() { Equity = equity });
            return this;
        }
        public DomainBuilder IfPocketEquityLessThan(double equity)
        {
            Pointer.AddCondition(new PocketEquityLessThan() { Equity = equity });
            return this;
        }


        // Recomanedations
        public DomainBuilder IfNoRecommendationYet()
        {
            Pointer.AddCondition(new NoRecommendation());
            return this;
        }
        public DomainBuilder IfFoldRecommendation()
        {
            Pointer.AddCondition(new FoldRecommendation());
            return this;
        }
        public DomainBuilder IfCallRecommendation()
        {
            Pointer.AddCondition(new CallRecommendation());
            return this;
        }
        public DomainBuilder IfRaiseRecommendation()
        {
            Pointer.AddCondition(new RaiseRecommendation());
            return this;
        }

        // Risk
        public DomainBuilder IfRiskAverse()
        {
            Pointer.AddCondition(new RiskAverseAttitude());
            return this;
        }
        public DomainBuilder IfRiskNeutral()
        {
            Pointer.AddCondition(new RiskNeutralAttitude());
            return this;
        }
        public DomainBuilder IfRiskSeeking()
        {
            Pointer.AddCondition(new RiskSeekingAttitude());
            return this;
        }
        public DomainBuilder IfCallTooRisky()
        {
            Pointer.AddCondition(new TooRiskyCall());
            return this;
        }
        public DomainBuilder IfRaiseRecommendationTooRisky()
        {
            Pointer.AddCondition(new TooRiskyRaiseRecommendation());
            return this;
        }
        public DomainBuilder IfMinRaiseNotTooRisky()
        {
            Pointer.AddCondition(new NotTooRiskyMinRaise());
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
        public DomainBuilder IfRaiseOrCallDecision()
        {
            Pointer.AddCondition(new RaiseOrCallDecision());
            return this;
        }
        public DomainBuilder IfTooFishy()
        {
            Pointer.AddCondition(new TooFishy());
            return this;
        }
        public DomainBuilder IfCallingFish()
        {
            Pointer.AddCondition(new CallingFish());
            return this;
        }
        #endregion

        #region Preflop
        public DomainBuilder PreflopSequence()
        {
            FishNet();
            PreflopEarlyPosition();
            PreflopMiddlePosition();
            PreflopLatePosition();
            PreflopBlindPosition();
            PreflopDecision();
            return this;
        }
        public DomainBuilder PreflopDecision()
        {
            Select("Fold Recommendation");
            {
                IfInPreflop();
                IfNoDecisionYet();
                IfFoldRecommendation();

                Action("call if it does not cost anything to call");
                {
                    IfZeroCostCall();
                    Do(Call);
                }
                End();

                Select("Risk Averse");
                {
                    IfRiskAverse();

                    Action("Fold if call is too expensive and risky");
                    {
                        IfPocketEquityLessThan(80);
                        Condition("call > 1/3 stack", (ctx) => { return ctx.GetCallAmount() > ctx.GetCurrentStack() / 3; });
                        Do(Fold);
                    }
                    End();

                    Action("Fold if pocket equity < 60");
                    {
                        IfPocketEquityLessThan(60);
                        Do(Fold);
                    }
                    End();

                    Action("Convert recommendation to call");
                    {
                        Do(Call);
                    }
                    End();
                }
                End();

                Select("Risk Neutral");
                {
                    IfRiskNeutral();

                    Action("Fold if call is too expensive and risky");
                    {
                        IfPocketEquityLessThan(80);
                        Condition("call > 1/2 stack", (ctx) => { return ctx.GetCallAmount() > ctx.GetCurrentStack() / 2; });
                        Do(Fold);
                    }
                    End();

                    Action("Fold if pocket equity < 60");
                    {
                        IfPocketEquityLessThan(60);
                        Do(Fold);
                    }
                    End();

                    Action("Covert recommendation to min raise when first to pot");
                    {
                        IfFirstToPot();
                        Do(MinRaise);
                    }
                    End();

                    Action("Convert recommendation to call");
                    {
                        Do(Call);
                    }
                    End();
                }
                End();

                Select("Risk Seeking");
                {
                    IfRiskSeeking();

                    Action("Fold if call is too expensive and risky");
                    {
                        IfPocketEquityLessThan(70);
                        Condition("call > 1/2 stack", (ctx) => { return ctx.GetCallAmount() > ctx.GetCurrentStack() / 2; });
                        Do(Fold);
                    }
                    End();

                    Action("Fold if pocket equity < 50");
                    {
                        IfPocketEquityLessThan(50);
                        Do(Fold);
                    }
                    End();

                    Action("Covert recommendation to 1or2BB raise when first to pot");
                    {
                        IfFirstToPot();
                        Do(Raise1Or2BB);
                    }
                    End();

                    Action("Covert recommendation to min raise");
                    {
                        Do(MinRaise);
                    }
                    End();
                }
                End();
            }
            End();

            Select("Call Recommendation");
            {
                IfInPreflop();
                IfNoDecisionYet();
                IfCallRecommendation();

                Action("Fold if call to risky: RiskAverse");
                {
                    IfCallTooRisky();
                    IfRiskAverse();
                    IfPocketEquityLessThan(80);
                    Do(Fold);
                }
                End();

                Action("Fold if call to risky: Risk Neutral");
                {
                    IfCallTooRisky();
                    IfRiskNeutral();
                    IfPocketEquityLessThan(70);
                    Do(Fold);
                }
                End();

                Action("Fold if call to risky: Risk Seeking");
                {
                    IfCallTooRisky();
                    IfRiskSeeking();
                    IfPocketEquityLessThan(60);
                    Do(Fold);
                }
                End();

                Action("Call if risk averse");
                {
                    IfRiskAverse();
                    Do(Call);
                }
                End();

                Action("Covert recommendation to min raise");
                {
                    IfRiskNeutral();
                    Do(MinRaise);
                }
                End();

                Action("Covert recommendation to 1or2BB raise");
                {
                    Do(Raise1Or2BB);
                }
                End();
            }
            End();

            Select("Raise Recommendation");
            {
                IfInPreflop();
                IfNoDecisionYet();
                IfRaiseRecommendation();

                Select("Too Risky Raise");
                {
                    IfRaiseRecommendationTooRisky();
                    Action("Raise min, if not too risky");
                    {
                        IfMinRaiseNotTooRisky();
                        Do(MinRaise);
                    }
                    End();

                    Action("Fold if call to risky: RiskAverse");
                    {
                        IfCallTooRisky();
                        IfRiskAverse();
                        IfPocketEquityLessThan(80);
                        Do(Fold);
                    }
                    End();

                    Action("Fold if call to risky: Risk Neutral");
                    {
                        IfCallTooRisky();
                        IfRiskNeutral();
                        IfPocketEquityLessThan(70);
                        Do(Fold);
                    }
                    End();

                    Action("Fold if call to risky: Risk Seeking");
                    {
                        IfCallTooRisky();
                        IfRiskSeeking();
                        IfPocketEquityLessThan(60);
                        Do(Fold);
                    }
                    End();

                    Action("Call");
                    {
                        Do(Call);
                    }
                    End();
                }
                End();

                Select("Check Raise");
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
                            ctx.Done = true;
                            return TaskStatus.Success;
                        });
                    }
                    End();
                }
                End();

                Action("Raise the recommended raise");
                {
                    Do(Raise);
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
                IfNoRecommendationYet();
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
                        Do(RecommendAlwaysRaise3Or4BB);
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
                        Do(RecommendUsuallyRaise3Or4BB);
                    }
                    End();

                    Action("Usually Raise on AQ");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Queen));
                        Do(RecommendUsuallyRaise3Or4BB);
                    }
                    End();

                    Action("Sometimes Raise on Groups 3");
                    {
                        IfPocketFromGroup(3);
                        Do(RecommendSometimesRaise3Or4BB);
                    }
                    End();

                    Select("Loose Game");
                    {
                        IfLooseGame();

                        Action("Fold AJ, KTs if Aggressive");
                        {
                            IfAggressiveGame();
                            IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack), new Pocket(Rank.King, Rank.Ten, true));
                            Do(RecommendFold);
                        }
                        End();

                        Action("Call or Raise on group 4");
                        {
                            IfPocketFromGroup(4);
                            Do(RecommendCallOrRaise2Or3BB);
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
                                Do(RecommendSometimesRaise3Or4BB);
                            }
                            End();

                            Action("Sometimes call on group 5");
                            {
                                IfPocketFromGroup(5);
                                Do(RecommendSometimesCall);
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
                            Do(RecommendAlwaysRaise3Or4BB);
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
                        Do(RecommendCall);
                    }
                    End();

                    Action("Usually Raise on Groups 1,2");
                    {
                        IfPocketFromGroup(1, 2);
                        Do(RecommendUsuallyRaise3Or4BB);
                    }
                    End();

                    Select("Defend against Loose Raiser");
                    {
                        IfLooseRaiser();
                        Action("Rasie on AQ, 99, 88");
                        {
                            IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Queen), new Pocket(Rank.Nine, Rank.Nine), new Pocket(Rank.Eight, Rank.Eight));
                            Do(RecommendUsuallyRaise3Or4BB);
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
                            Do(RecommendFold);
                        }
                        End();

                        Action("Usually Raise on Groups 1,2,3");
                        {
                            IfPocketFromGroup(1, 2, 3);
                            Do(RecommendUsuallyRaise3Or4BB);
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
                            Do(RecommendFold);
                        }
                        End();

                        Action("Usually Raise on Groups 1,2");
                        {
                            IfPocketFromGroup(1, 2);
                            Do(RecommendUsuallyRaise3Or4BB);
                        }
                        End();
                    }
                    End();

                }
                End();

                Action("Fold if no decision was made");
                {
                    IfNoDecisionYet();
                    Do(RecommendFold);
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
                IfNoRecommendationYet();
                IfInMiddlePosition();

                Action("Always Raise on Groups 1,2");
                {
                    IfPocketFromGroup(1, 2);
                    Do(RecommendAlwaysRaise3Or4BB);
                }
                End();

                Select("Raise on callers");
                {
                    IfCallsOnly();

                    Action("Always Raise on AQ");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Queen));
                        Do(RecommendAlwaysRaise3Or4BB);
                    }
                    End();

                    Action("Call on JTs");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Jack, Rank.Ten));
                        Do(RecommendCall);
                    }
                    End();

                    Action("Sometimes raise on AJ, KQ");
                    {
                        IfPocketIsOneOf(new Pocket(Rank.Ace, Rank.Jack), new Pocket(Rank.King, Rank.Queen));
                        Do(RecommendSometimesRaise3Or4BB);
                    }
                    End();

                    Action("Sometimes Raise on Groups 3");
                    {
                        IfPocketFromGroup(3);
                        Do(RecommendSometimesRaise3Or4BB);
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
                        Do(RecommendAlwaysRaise3Or4BB);
                    }
                    End();

                    Select("Loose Game");
                    {
                        IfLooseGame();
                        Action("Fold KJ, T8s on aggressive");
                        {
                            IfAggressiveGame();
                            IfPocketIsOneOf(new Pocket(Rank.King, Rank.Jack), new Pocket(Rank.Ten, Rank.Eight, true));
                            Do(RecommendFold);
                        }
                        End();

                        Action("Always Raise on Groups 4,5,6");
                        {
                            IfPocketFromGroup(4, 5, 6);
                            Do(RecommendAlwaysRaise3Or4BB);
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
                            Do(RecommendUsuallyRaise3Or4BB);
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
                            Do(RecommendUsuallyRaise3Or4BB);
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
                        Do(RecommendAlwaysRaise3Or4BB);
                    }
                    End();

                    Action("Occasionally Raise on T9s, 88");
                    {
                        Occasionally();
                        IfPocketIsOneOf(new Pocket(Rank.Ten, Rank.Nine, true), new Pocket(Rank.Eight, Rank.Eight));
                        Do(RecommendAlwaysRaise3Or4BB);
                    }
                    End();
                }
                End();

                Action("Fold if no decision was made");
                {
                    IfNoDecisionYet();
                    Do(RecommendFold);
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
                IfNoRecommendationYet();
                IfInLatePosition();

                Action("Always Raise on 1,2,3,4,5,6,7 if no raises");
                {
                    IfFirstToRaise();
                    IfPocketFromGroup(1, 2, 3, 4, 5, 6, 7);
                    Do(RecommendAlwaysRaise3Or4BB);
                }
                End();

                Action("Always Raise on 1,2,3");
                {
                    IfPocketFromGroup(1, 2, 3);
                    Do(RecommendAlwaysRaise3Or4BB);
                }
                End();

                Action("Sometimes Raise on 4");
                {
                    IfPocketFromGroup(4);
                    Do(RecommendSometimesRaise3Or4BB);
                }
                End();

                Action("Call on 5,6");
                {
                    IfPocketFromGroup(5, 6);
                    Do(RecommendCall);
                }
                End();

                Action("Call on 7");
                {
                    IfPocketFromGroup(7);
                    Do(RecommendSometimesCall);
                }
                End();


                Action("Fold if no decision was made");
                {
                    IfNoDecisionYet();
                    Do(RecommendFold);
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
                IfNoRecommendationYet();
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
                            Do(RecommendAlwaysRaise3Or4BB);
                        }
                        End();

                        Action("Call on 3,4,5");
                        {
                            IfPocketFromGroup(3, 4, 5);
                            Do(RecommendCall);
                        }
                        End();
                    }
                    End();

                    Select("If there is calls or raises");
                    {
                        IfFirstToRaise();

                        Action("Call on 1,2,3,4");
                        {
                            IfPocketFromGroup(1, 2, 3, 4);
                            Do(RecommendCall);
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
                            Do(RecommendAlwaysRaise3Or4BB);
                        }
                        End();

                        Action("Call on 5,6");
                        {
                            IfPocketFromGroup(5, 6);
                            Do(RecommendCall);
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
                            Do(RecommendCall);
                        }
                        End();
                    }
                    End();
                }
                End();

                Action("Fold if no decision was made");
                {
                    IfNoRecommendationYet();
                    IfNoDecisionYet();
                    Do(RecommendFold);
                }
                End();
            }
            End();
            return this;
        }
        #endregion

        #region Check-Raise
        public DomainBuilder CheckRaiseCutSelector()
        {
            Select("Check-Raise");
            {
                IfInPreflop();
                Condition("Check-Raise", (ctx) => ctx.GetCheckRaise());
                Action("Always raise");
                {
                    Do((ctx) =>
                    {
                        ctx.UnsetCheckRaise();
                        ctx.SetDecision(new Decision() { Call = 0, Fold = 0, Raise = 1 });
                        ctx.SetRaiseAmount(ctx.GetMaxPossibleRaiseAmount());
                        ctx.Done = true;
                        return TaskStatus.Success;
                    });
                }
                End();
            }
            End();
            return this;
        }
        #endregion

        #region Fish-Net :)
        public DomainBuilder FishNet()
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

        #region Postflop
        public DomainBuilder PostflopSequence(Context context)
        {
            ExpectedUtilitySelector("Postflop");
            {
                PostflopTasks(context);
            }
            End();
            return this;
        }
        public DomainBuilder PostflopTasks(Context context)
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

            PostFlopRaiseActions(min, max, callAmount, bb, ehs, pot, logger, context);
            PostFlopCallAction(callAmount, bb, ehs, pot, opponents, logger, context);
            PostFlopFoldAction(opponents, logger, context);

            // log odds
            currentRound?.OddsLog.Add(logger.ToString());
            return this;
        }
        public DomainBuilder PostFlopRaiseActions(double minRaise, double maxRaise, double callAmount, double BigBlind, double EHS, double pot, StringBuilder logger, Context ctx)
        {
            // Raise Actions
            for (int i = 0; minRaise + i * BigBlind < maxRaise; i++)
            {
                // dont consider big raises with low odds
                if (EHS < 0.7 && i > 3)
                    break;

                // dont consider big raises with low odds
                if (EHS < 0.8 && minRaise + i * BigBlind > maxRaise / 2)
                    break;

                var odds = Oracle.RaiseOdds(EHS, minRaise + i * BigBlind, callAmount, minRaise, maxRaise, ctx.GetPaid(), pot);
                logger.AppendLine("raise: " + (minRaise + i * BigBlind) + ", " + String.Join(" , ", odds) + ", EV:" + logEV(odds, ctx));

                // Define action with correlating odds of the raise amount.
                VariableUtilityAction("Raise: " + (minRaise + i * BigBlind), odds);
                Do((ctx) =>
                {
                    if (ctx.GetRoundState() == RoundState.Flop)
                    {
                        // override premature raise
                        ctx.SetDecision(new Decision() { Call = 0.7, Raise = 0.3, Fold = 0 });
                    }
                    else if (EHS > 0.8)
                    {
                        ctx.SetDecision(new Decision() { Call = 0.3, Raise = 0.7, Fold = 0 });
                    }
                    else if (ctx.GetRoundState() == RoundState.Turn)
                    {

                        ctx.SetDecision(new Decision() { Call = 0.6, Raise = 0.4, Fold = 0 });
                    }
                    else if (ctx.GetRoundState() == RoundState.River)
                    {
                        ctx.SetDecision(new Decision() { Call = 0.5, Raise = 0.5, Fold = 0 });
                    }
                    else
                    {
                        ctx.SetDecision(new Decision() { Call = 0.3, Raise = 0.7, Fold = 0 });
                    }
                    ctx.SetRaiseAmount(minRaise + (i * BigBlind));
                    ctx.Done = true;
                    return TaskStatus.Success;
                });
                End();
            }
            return this;
        }
        public DomainBuilder PostFlopCallAction(double callAmount, double BigBlind, double EHS, double pot, List<PlayerModel> opponents, StringBuilder logger, Context ctx)
        {
            var odds = Oracle.CallOdds(EHS, callAmount, ctx.GetPaid(), pot, opponents, BigBlind);
            logger.AppendLine("call: " + callAmount + ", " + String.Join(" , ", odds) + ", EV:" + logEV(odds, ctx));
            VariableUtilityAction("Call: " + callAmount, odds);
            Do((ctx) =>
            {
                ctx.SetDecision(new Decision() { Call = 0.8, Raise = 0.2, Fold = 0 });
                ctx.SetRaiseAmount(ctx.GetMinPossibleRaiseAmount());
                ctx.Done = true;
                return TaskStatus.Success;
            });
            End();
            return this;
        }
        public DomainBuilder PostFlopFoldAction(List<PlayerModel> opponents, StringBuilder logger, Context ctx)
        {
            // Fold Action
            var odds = Oracle.FoldOdds(opponents, ctx.GetPaid());
            logger.AppendLine("fold " + String.Join(" , ", odds));
            VariableUtilityAction("Fold", odds);
            Do(Fold);
            End();
            return this;
        }
        #endregion

        #region Recommandations
        private static TaskStatus RecommendAlwaysRaise3Or4BB(Context ctx)
        {
            ctx.SetRecomanndedDecision(new Decision() { Fold = 0, Call = 0, Raise = 1 });
            ctx.SetRecomanndedRaiseAmount((3, 0.3f), (4, 0.7f));
            return TaskStatus.Success;
        }
        private static TaskStatus RecommendUsuallyRaise3Or4BB(Context ctx)
        {
            ctx.SetRecomanndedDecision(new Decision() { Fold = 0, Call = 0.1, Raise = 0.9 });
            ctx.SetRecomanndedRaiseAmount((3, 0.3f), (4, 0.7f));
            return TaskStatus.Success;
        }
        private static TaskStatus RecommendSometimesRaise3Or4BB(Context ctx)
        {
            ctx.SetRecomanndedDecision(new Decision() { Fold = 0, Call = 0.3, Raise = 0.7 });
            ctx.SetRecomanndedRaiseAmount((3, 0.5f), (4, 0.5f));
            return TaskStatus.Success;
        }
        private static TaskStatus RecommendFold(Context ctx)
        {
            ctx.SetRecomanndedDecision(new Decision() { Fold = 0.7, Call = 0.3, Raise = 0 });
            return TaskStatus.Success;
        }
        private static TaskStatus RecommendCallOrRaise2Or3BB(Context ctx)
        {
            ctx.SetRecomanndedDecision(new Decision() { Fold = 0, Call = 0.4, Raise = 0.6 });
            ctx.SetRecomanndedRaiseAmount((2, 0.6f), (3, 0.4f));
            return TaskStatus.Success;
        }
        private static TaskStatus RecommendSometimesCall(Context ctx)
        {
            ctx.SetRecomanndedDecision(new Decision() { Fold = 0.4, Call = 0.6, Raise = 0 });
            ctx.SetRecomanndedRaiseAmount(ctx.GetMinPossibleRaiseAmount());
            return TaskStatus.Success;
        }
        private static TaskStatus RecommendCall(Context ctx)
        {
            ctx.SetRecomanndedDecision(new Decision() { Fold = 0, Call = 0.9, Raise = 0.1 });
            ctx.SetRecomanndedRaiseAmount(ctx.GetMinPossibleRaiseAmount());
            return TaskStatus.Success;
        }
        #endregion

        #region Actions
        private static TaskStatus Fold(Context ctx)
        {
            if (ctx.GetCallAmount() == 0)
            {
                ctx.SetDecision(new Decision() { Fold = 0, Call = 1, Raise = 0 });
            }
            else
            {
                ctx.SetDecision(new Decision() { Fold = 1, Call = 0, Raise = 0 });
            }
            ctx.Done = true;
            return TaskStatus.Success;
        }
        private static TaskStatus Call(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 1, Raise = 0 });
            ctx.Done = true;
            return TaskStatus.Success;
        }
        private static TaskStatus Raise(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0.1, Raise = 0.9 });
            ctx.SetRaiseAmount(ctx.GetRecomanndedRaiseAmount());
            ctx.Done = true;
            return TaskStatus.Success;
        }
        private static TaskStatus MinRaise(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0, Raise = 1 });
            ctx.SetRaiseAmount(ctx.GetMinPossibleRaiseAmount());
            ctx.Done = true;
            return TaskStatus.Success;
        }
        private static TaskStatus Raise1Or2BB(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0, Raise = 1 });
            if (ctx.GetMinPossibleRaiseAmount() > ctx.GetGame().BigBlind)
            {
                ctx.SetRaiseAmount(ctx.GetMinPossibleRaiseAmount() + ctx.GetGame().BigBlind);
            }
            else
            {
                ctx.SetRaiseAmount((1, 0.5f), (2, 0.5f));
            }
            ctx.Done = true;
            return TaskStatus.Success;
        }
        private static TaskStatus RaiseLikeShark(Context ctx)
        {
            ctx.SetDecision(new Decision() { Fold = 0, Call = 0, Raise = 1 });
            ctx.SetRaiseAmount(ctx.GetMaxPossibleRaiseAmount());
            ctx.Done = true;
            return TaskStatus.Success;
        }
        private static TaskStatus CallAFish(Context ctx)
        {
            if (ctx.GetCallAmount() > ctx.GetCurrentStack() / 2)
            {
                ctx.SetDecision(new Decision() { Fold = 0.8, Call = 0.2, Raise = 0 });

            }
            else
            {
                ctx.SetDecision(new Decision() { Fold = 0, Call = 1, Raise = 0 });

            }
            ctx.Done = true;
            return TaskStatus.Success;
        }
        private static string logEV(List<VariableUtility> PossibleUtility, Context ctx)
        {
            return PossibleUtility.Sum(vc => ((Context)ctx).GetAttitude().CalculateUtility(vc.Utility) * vc.Probability).ToString("0.00");
        }
        #endregion
    }
}
