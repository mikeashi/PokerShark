using FluidHTN;
using FluidHTN.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Domain.Conditions.Game
{
    internal class IsTight : ICondition<Object>
    {
        public string Name { get; } = "If game is tight";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                var models = c.GetPlayersModels();
                var loose = 0;
                var tight = 0;
                foreach(var m in models)
                {
                    if(m.PlayingStyle == PlayingStyle.LooseAggressive || m.PlayingStyle == PlayingStyle.LoosePassive)
                    {
                        loose++;
                    }
                    else
                    {
                        tight++;
                    }
                }
                return tight > loose;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
