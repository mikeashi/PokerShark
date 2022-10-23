﻿using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions.Pot
{
    internal class TwoOrMoreRaises : ICondition<Object>
    {
        public string Name { get; } = "If two or more raises has been made";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetHistory().Count(action => action.Type == Poker.ActionType.Raise) > 1;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
