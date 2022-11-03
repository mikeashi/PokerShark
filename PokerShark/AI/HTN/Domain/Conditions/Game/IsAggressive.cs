using FluidHTN;
using FluidHTN.Conditions;

namespace PokerShark.AI.HTN.Domain.Conditions.Game
{
    internal class IsAggressive : ICondition<Object>
    {
        public string Name { get; } = "If game is aggressive";

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
                return aggressive > passive;
            }

            throw new Exception("Unexpected context type!");
        }
    }
}
