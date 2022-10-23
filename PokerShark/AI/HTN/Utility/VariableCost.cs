using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.AI.HTN.Utility
{
    internal class VariableCost
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
    }
}
