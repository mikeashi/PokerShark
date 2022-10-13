using FluidHTN;
using FluidHTN.PrimitiveTasks;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.HTN.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace PokerShark.Core.HTN.Tasks
{


    public class VariableCostTask : PrimitiveTask<object>
    {
        List<VariableCost> PossibleCosts { get; set; }

        public VariableCostTask(List<VariableCost> possibleCosts)
        {
            if (possibleCosts.Sum(vc => vc.Probability) != 1) throw new Exception("Probability distribution does not add up to one.");
            PossibleCosts = possibleCosts;
        }

        public double GetExpectedCost(IContext<object> ctx)
        {
            if (ctx is PokerContext) return PossibleCosts.Sum(vc => ((PokerContext)ctx).GetAttitude().CalculateUtility(vc.Cost) * vc.Probability);
            return PossibleCosts.Sum(vc => new RiskNeutral().CalculateUtility(vc.Cost) * vc.Probability);
        }

        public double GetExpectedCost(StaticUtilityFunction uf)
        {
            return PossibleCosts.Sum(vc => uf.CalculateUtility(vc.Cost) * vc.Probability);
        }
    }
}
