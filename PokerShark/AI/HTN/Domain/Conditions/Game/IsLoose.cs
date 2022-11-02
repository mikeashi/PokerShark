using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Game
{
    internal class IsLoose : ICondition<Object>
    {
        public string Name { get; } = "If game is loose";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                var models = c.GetPlayersModels();
                var loose = 0;
                var tight = 0;
                foreach (var m in models)
                {
                    if (m.PlayingStyle == PlayingStyle.LooseAggressive || m.PlayingStyle == PlayingStyle.LoosePassive)
                    {
                        loose++;
                    }
                    else
                    {
                        tight++;
                    }
                }
                return loose >= tight;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
