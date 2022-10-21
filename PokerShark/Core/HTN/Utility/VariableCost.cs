using PokerShark.Core.PyPoker;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.HTN.Utility
{
    public class VariableCost
    {
        public double Cost { get; internal set; }
        public double Probability { get; internal set; }

        public VariableCost(double cost, double probability)
        {
            if (probability < 0 || probability > 1)
            {
                throw new Exception("Probability has to be bigger than zero and less than one.");
            }
            Cost = cost;
            Probability = probability;
        }

        public override string? ToString()
        {
            return String.Format("({0}x{1})", Cost.ToString(CultureInfo.InvariantCulture), Math.Round(Probability, 4).ToString(CultureInfo.InvariantCulture));
        }
    }
}
