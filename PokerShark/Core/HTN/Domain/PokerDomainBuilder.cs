using FluidHTN;
using FluidHTN.Factory;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.HTN.Domain.Conditions;
using PokerShark.Core.HTN.Tasks;
using PokerShark.Core.HTN.Tasks.CompoundTasks;
using PokerShark.Core.HTN.Utility;
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
                Select("EarlyPosition");
                {
                    IfInEarlyPosition();
                    IfNoDecisionYet();
                    Action("EarlyPositionAction");
                    {
                        Do((ctx) => {
                            Console.WriteLine("EarlyPositionAction");
                            ctx.SetDecision((0, 1, 0));
                            return TaskStatus.Success;
                        });
                    }
                    End();

                }
                End();

                Select("MiddlePosition");
                {
                    IfInMiddlePosition();
                    IfNoDecisionYet();
                    Action("MiddlePositionAction");
                    {
                        Do((ctx) => {
                            Console.WriteLine("MiddlePositionAction");
                            ctx.SetDecision((0, 1, 0));
                            return TaskStatus.Success;
                        });
                    }
                    End();

                }
                End();
                
                Select("LatePosition");
                {
                    IfInLatePosition();
                    IfNoDecisionYet();
                    Action("LatePositionAction");
                    {
                        Do((ctx) => {
                            Console.WriteLine("LatePositionAction");
                            ctx.SetDecision((0, 1, 0));
                            return TaskStatus.Success;
                        });
                    }
                    End();
                }
                End();
                
                Select("BlindPosition");
                {
                    IfInBlindPosition();
                    IfNoDecisionYet();
                    Action("BlindPositionAction");
                    {
                        Do((ctx) => {
                            Console.WriteLine("BlindPositionAction");
                            ctx.SetDecision((0, 1, 0));
                            return TaskStatus.Success;
                        });
                    }
                    End();
                }
                End();

                Select("NoDecisionYet");
                {
                    IfNoDecisionYet();
                    Action("Preflop");
                    {
                        Do((ctx) => {
                            ctx.SetDecision((0, 1, 0));
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

        #endregion
    }
}
