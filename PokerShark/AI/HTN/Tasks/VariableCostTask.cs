using FluidHTN;
using FluidHTN.PrimitiveTasks;
using PokerShark.AI.HTN.Utility;

namespace PokerShark.AI.HTN.Tasks
{
    internal class VariableCostTask : PrimitiveTask<object>
    {
        List<VariableCost> PossibleCosts { get; set; }

        public VariableCostTask(List<VariableCost> possibleCosts)
        {
            if (possibleCosts.Sum(vc => vc.Probability) != 1)
                throw new Exception("Probability distribution does not add up to one.");
            PossibleCosts = possibleCosts;
        }

        public double GetExpectedCost(IContext<object> ctx)
        {
            if (ctx is Context)
                return PossibleCosts.Sum(vc => ((Context)ctx).GetAttitude().CalculateUtility(vc.Cost) * vc.Probability);
            throw new Exception("Incorrect context type.");
        }
    }

}

