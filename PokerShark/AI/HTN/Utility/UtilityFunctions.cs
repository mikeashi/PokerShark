namespace PokerShark.AI.HTN.Utility
{
    public abstract class StaticUtilityFunction
    {
        public abstract double CalculateUtility(double cost);
    }

    public class RiskNeutral : StaticUtilityFunction
    {
        public override double CalculateUtility(double cost)
        {
            return cost;
        }
    }

    public class RiskSeeking : StaticUtilityFunction
    {
        private double attitude = 1;
        private double sensitivity = 0.01;

        public override double CalculateUtility(double cost)
        {
            return attitude * (Math.Exp(attitude * sensitivity * cost) - 1) / sensitivity;
        }
    }

    public class RiskAverse : StaticUtilityFunction
    {
        private double attitude = -1;
        private double sensitivity = 0.01;

        public override double CalculateUtility(double cost)
        {
            return attitude * (Math.Exp(attitude * sensitivity * cost) - 1) / sensitivity;
        }
    }

}
