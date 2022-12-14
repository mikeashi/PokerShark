using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Game
{
    internal class IsPassive : ICondition<Object>
    {
        public string Name { get; } = "If game is passive";

        public bool IsValid(IContext<Object> ctx)
        {
            if (ctx is Context c)
            {
                var models = c.GetPlayersModels();
                var aggressive = 0;
                var passive = 0;
                foreach (var m in models)
                {
                    if (m.PlayingStyle == PlayingStyle.LooseAggressive || m.PlayingStyle == PlayingStyle.TightAggressive)
                    {
                        aggressive++;
                    }
                    else
                    {
                        passive++;
                    }
                }
                return passive >= aggressive;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
