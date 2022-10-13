using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PokerShark.Core.PyPoker
{
    public abstract class PyAction
    {
        public double MinAmount { get; internal set; }
        public double MaxAmount { get; internal set; }
        public double Amount { get; internal set; }
        public string PlayerId { get; set; }
        public StreetState Stage { get; set; }
        public double Paid { get; set; }

        internal string Name;

        public override string ToString()
        {
            return "{\"action\": \"" + Name + "\" , \"amount\": \"" + Amount + "\"}";
        }

        public abstract string Information();
    }

    public class RaiseAction : PyAction
    {
        public RaiseAction(double minAmount, double maxAmount)
        {
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Amount = minAmount;
            Name = "raise";
        }

        public override string Information()
        {
            return "Raise: min = " + MinAmount + ", max = " + MaxAmount;
        }
    }

    public class CallAction : PyAction
    {
        public CallAction(double amount)
        {
            MinAmount = amount;
            MaxAmount = amount;
            Amount = amount;
            Name = "call";
        }

        public override string Information()
        {
            return "Call: " + Amount;
        }
    }

    public class FoldAction : PyAction
    {
        public FoldAction()
        {
            MinAmount = 0;
            MaxAmount = 0;
            Amount = 0;
            Name = "fold";
        }

        public override string Information()
        {
            return "Fold";
        }
    }
}
