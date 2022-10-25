using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions.Game
{
    internal class OneOpponent : ICondition<Object>
    {
        public string Name { get; } = "If there is only one opponent";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                var game = c.GetGame();
                // we dont need to check if player is participating,
                // because if he is folded the game would be over by now.
                return game.GetOpponentModels().Count == 1;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
