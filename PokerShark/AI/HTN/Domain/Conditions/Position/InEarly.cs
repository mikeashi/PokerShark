﻿using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Position
{
    internal class InEarly : ICondition<Object>
    {
        public string Name { get; } = "If in early position";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                return c.GetPosition() == Poker.Position.Early;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
