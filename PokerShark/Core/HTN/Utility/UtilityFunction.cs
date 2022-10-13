using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.HTN.Utility
{
    public abstract class StaticUtilityFunction
    {
        public abstract double CalculateUtility(double cost);
    }

    public class RiskNeutral : StaticUtilityFunction
    {

        public override double CalculateUtility(double cost)
        {
            return cost * -1;
        }
    }

    public class RiskSeeking : StaticUtilityFunction
    {
        private double attitude = 1;
        private double sensitivity = 0.01;

        public override double CalculateUtility(double cost)
        {
            cost = cost * -1;

            return attitude * (Math.Exp(attitude * sensitivity * cost) - 1) / sensitivity;
        }
    }

    public class RiskAverse : StaticUtilityFunction
    {
        private double attitude = -1;
        private double sensitivity = 0.01;

        public override double CalculateUtility(double cost)
        {
            cost = cost * -1;

            return attitude * (Math.Exp(attitude * sensitivity * cost) - 1) / sensitivity;
        }
    }
}
