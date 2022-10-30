﻿using FluidHTN;
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
    internal class RiskNeutralAttitude : ICondition<Object>
    {
        public string Name { get; } = "if bot attitude is risk neutral";

        public bool IsValid(IContext<object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetAttitude() is RiskNeutral;
            }
            throw new Exception("Unexpected context type!");
        }
    }
}