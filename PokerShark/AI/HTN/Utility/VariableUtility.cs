using System.Globalization;

namespace PokerShark.AI.HTN.Utility
{
    internal class VariableUtility
    {
        public double Utility { get; internal set; }
        public double Probability { get; internal set; }

        public VariableUtility(double utility, double probability)
        {
            if (probability < 0 || probability > 1)
            {
                throw new Exception("Probability has to be bigger than zero and less than one.");
            }
            Utility = utility;
            Probability = probability;
        }

        public override string ToString()
        {
            return $"Cost: {Utility.ToString("0.00", CultureInfo.InvariantCulture)}, Probability: {Probability.ToString("0.00", CultureInfo.InvariantCulture)}";
        }
    }
}
