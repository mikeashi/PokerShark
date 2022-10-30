using FluidHTN;
using FluidHTN.Conditions;
using PokerShark.AI.HTN.Domain.Conditions.Game;
using PokerShark.AI.HTN.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions
{
    internal class NotTooRiskyMinRaise : ICondition<Object>
    {
        public string Name { get; } = "if raise recommendation too risky";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                var stack = c.GetCurrentStack();
                var initialStack = c.GetGame().InitialStack;
                var raise = c.GetMinPossibleRaiseAmount();
                var attitude = c.GetAttitude();
                
                if (raise <= c.GetGame().BigBlind * 2)
                    return false;
                
                if (attitude is RiskNeutral)
                {
                    return raise > stack / 4;
                }
                else if (attitude is RiskAverse)
                {
                    return raise > stack / 7;
                }
                else if (attitude is RiskSeeking)
                {
                    if (initialStack < stack)
                        return raise > stack / 3;
                    return raise > stack / 4;
                }
                else
                {
                    throw new Exception("Invalid attitude");
                }
            }
            throw new Exception("Unexpected context type!");
        }
    }
}
