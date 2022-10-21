using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Poker
{
    public class Pot
    {
        #region Properties
        public List<SidePot> SidePots { get; set; }
        public int Amount { get; private set; }
        #endregion

        #region Constructors
        public Pot(int amount)
        {
            Amount = amount;
            SidePots = new List<SidePot>();
        }

        public Pot(Pot pot)
        {
            Amount = pot.Amount;
            SidePots = new List<SidePot>();
            foreach (var sidePot in pot.SidePots)
            {
                SidePots.Add(new SidePot(sidePot));
            }
        }
        #endregion
        
        #region Methods
        public double getTotalForPlayer(string PlayerId)
        {
            return Amount + SidePots.Where(s => s.Eligibles.Contains(PlayerId)).Sum(s => s.Amount);
        }
        #endregion

        #region Serialization
        public bool ShouldSerializeSidePots()
        {
            return SidePots.Any();
        }
        #endregion

    }

    public class SidePot
    {
        #region Properties
        public int Amount { get; set; }
        public List<string> Eligibles { get; set; }
        #endregion

        #region Constructors
        public SidePot(int amount, List<string> eligibles)
        {
            Amount = amount;
            Eligibles = eligibles;
        }

        public SidePot(SidePot sidePot)
        {
            Amount = sidePot.Amount;
            Eligibles = new List<string>(sidePot.Eligibles);
        }
        #endregion
    }

}
