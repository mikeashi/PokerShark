using FluidHTN;
using FluidHTN.PrimitiveTasks;
using PokerShark.AI.HTN.Utility;

namespace PokerShark.AI.HTN.Tasks
{
    internal class VariableUtilityTask : PrimitiveTask<object>
    {
        List<VariableUtility> PossibleUtility { get; set; }

        public VariableUtilityTask(List<VariableUtility> possibleUtility)
        {
            if (possibleUtility.Sum(vc => vc.Probability) != 1)
                throw new Exception("Probability distribution does not add up to one.");
            PossibleUtility = possibleUtility;
        }

        public double GetExpectedUtility(IContext<object> ctx)
        {
            if (ctx is Context)
                return PossibleUtility.Sum(vc => ((Context)ctx).GetAttitude().CalculateUtility(vc.Utility) * vc.Probability);
            throw new Exception("Incorrect context type.");
        }
    }

}

