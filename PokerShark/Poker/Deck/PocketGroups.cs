using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Poker.Deck
{
    
    #region Pocket
    internal class Pocket
    {
        private Rank FirstCardRank;
        private Rank SecondCardRank;
        private bool Suited;

        public Pocket(Rank firstCardRank, Rank secondCardRank, bool suited = false)
        {
            FirstCardRank = firstCardRank;
            SecondCardRank = secondCardRank;
            Suited = suited;
        }

        public bool Equals(List<Card> cards)
        {
            if (Suited && cards[0].Suit != cards[1].Suit) return false;
            if (cards[0].Rank == FirstCardRank && cards[1].Rank == SecondCardRank) return true;
            if (cards[0].Rank == SecondCardRank && cards[1].Rank == FirstCardRank) return true;
            return false;
        }
    }
    #endregion

    #region Group
    internal abstract class Group
    {
        internal List<Pocket> Hands { get; set; }
        public int Strength { get; }

        public Group(int strength)
        {
            Strength = strength;
            Hands = new List<Pocket>();
        }

        public void AddPocket(Pocket hand)
        {
            Hands.Add(hand);
        }

        public bool hasPocket(List<Card> cards)
        {
            foreach (var hand in Hands)
            {
                if (hand.Equals(cards)) return true;
            }
            return false;
        }
    }
    #endregion

    #region Group1
    internal class Group1 : Group
    {
        public Group1() : base(1)
        {
            // AA
            AddPocket(new Pocket(Rank.Ace, Rank.Ace));
            // KK
            AddPocket(new Pocket(Rank.King, Rank.King));
            // QQ
            AddPocket(new Pocket(Rank.Queen, Rank.Queen));
            // JJ
            AddPocket(new Pocket(Rank.Jack, Rank.Jack));
            // AKs
            AddPocket(new Pocket(Rank.Ace, Rank.King, true));
        }
    }
    #endregion

    #region Group2
    internal class Group2 : Group
    {
        public Group2() : base(2)
        {
            // TT
            AddPocket(new Pocket(Rank.Ten, Rank.Ten));

            // AK 
            AddPocket(new Pocket(Rank.Ace, Rank.King));

            // AQs
            AddPocket(new Pocket(Rank.Ace, Rank.Queen, true));

            // AJs
            AddPocket(new Pocket(Rank.Ace, Rank.Jack, true));

            // KQs
            AddPocket(new Pocket(Rank.King, Rank.Queen, true));

        }
    }
    #endregion

    #region Group3
    internal class Group3 : Group
    {
        public Group3() : base(3)
        {
            // 99
            AddPocket(new Pocket(Rank.Nine, Rank.Nine));

            // JTs
            AddPocket(new Pocket(Rank.Jack, Rank.Ten, true));

            // QJs
            AddPocket(new Pocket(Rank.Queen, Rank.Jack, true));

            // KJs
            AddPocket(new Pocket(Rank.King, Rank.Jack, true));

            // ATs
            AddPocket(new Pocket(Rank.Ace, Rank.Ten, true));

            // AQ
            AddPocket(new Pocket(Rank.Ace, Rank.Queen));

        }
    }
    #endregion

    #region Group4
    internal class Group4 : Group
    {
        public Group4() : base(4)
        {
            // T9s
            AddPocket(new Pocket(Rank.Ten, Rank.Nine, true));

            // KQ
            AddPocket(new Pocket(Rank.King, Rank.Queen));

            // 88
            AddPocket(new Pocket(Rank.Eight, Rank.Eight));

            // QTs
            AddPocket(new Pocket(Rank.Queen, Rank.Ten, true));

            // 98s
            AddPocket(new Pocket(Rank.Nine, Rank.Eight, true));

            // J9s
            AddPocket(new Pocket(Rank.Jack, Rank.Nine, true));

            // AJ
            AddPocket(new Pocket(Rank.Ace, Rank.Jack));

            // KTs
            AddPocket(new Pocket(Rank.King, Rank.Ten, true));
        }
    }
    #endregion

    #region Group5
    internal class Group5 : Group
    {
        public Group5() : base(5)
        {
            // 77
            AddPocket(new Pocket(Rank.Seven, Rank.Seven));

            // 87s
            AddPocket(new Pocket(Rank.Eight, Rank.Seven, true));

            // Q9s
            AddPocket(new Pocket(Rank.Queen, Rank.Nine, true));

            // T8s
            AddPocket(new Pocket(Rank.Ten, Rank.Eight, true));

            // KJ
            AddPocket(new Pocket(Rank.King, Rank.Jack));

            // QJ
            AddPocket(new Pocket(Rank.Queen, Rank.Jack));

            // JT
            AddPocket(new Pocket(Rank.Jack, Rank.Ten));

            // 76s
            AddPocket(new Pocket(Rank.Seven, Rank.Six, true));

            // 97s
            AddPocket(new Pocket(Rank.Nine, Rank.Seven, true));

            // Axs (9,8,7,6,5,4,3,2)
            AddPocket(new Pocket(Rank.Ace, Rank.Nine, true));
            AddPocket(new Pocket(Rank.Ace, Rank.Eight, true));
            AddPocket(new Pocket(Rank.Ace, Rank.Seven, true));
            AddPocket(new Pocket(Rank.Ace, Rank.Six, true));
            AddPocket(new Pocket(Rank.Ace, Rank.Five, true));
            AddPocket(new Pocket(Rank.Ace, Rank.Four, true));
            AddPocket(new Pocket(Rank.Ace, Rank.Three, true));
            AddPocket(new Pocket(Rank.Ace, Rank.Two, true));

            // 65s
            AddPocket(new Pocket(Rank.Six, Rank.Five, true));

        }
    }
    #endregion

    #region Group6
    internal class Group6 : Group
    {
        public Group6() : base(6)
        {
            // 66
            AddPocket(new Pocket(Rank.Six, Rank.Six));

            // AT
            AddPocket(new Pocket(Rank.Ace, Rank.Ten));

            // 55
            AddPocket(new Pocket(Rank.Five, Rank.Five));

            // 86s
            AddPocket(new Pocket(Rank.Eight, Rank.Six, true));

            // KT
            AddPocket(new Pocket(Rank.King, Rank.Ten));

            // QT
            AddPocket(new Pocket(Rank.Queen, Rank.Ten));

            // 54s
            AddPocket(new Pocket(Rank.Five, Rank.Four, true));

            // K9s
            AddPocket(new Pocket(Rank.King, Rank.Nine, true));

            // J8s
            AddPocket(new Pocket(Rank.Jack, Rank.Eight, true));

            // 75s
            AddPocket(new Pocket(Rank.Seven, Rank.Five, true));
        }
    }
    #endregion

    #region Group7
    internal class Group7 : Group
    {
        public Group7() : base(7)
        {

            // 44
            AddPocket(new Pocket(Rank.Four, Rank.Four));

            // J9
            AddPocket(new Pocket(Rank.Jack, Rank.Nine));

            // 64s
            AddPocket(new Pocket(Rank.Six, Rank.Four, true));

            // T9
            AddPocket(new Pocket(Rank.Ten, Rank.Nine));

            // 53s
            AddPocket(new Pocket(Rank.Five, Rank.Three, true));

            // 33
            AddPocket(new Pocket(Rank.Three, Rank.Three));

            // 98
            AddPocket(new Pocket(Rank.Nine, Rank.Eight));

            // 43s
            AddPocket(new Pocket(Rank.Four, Rank.Three, true));

            // 22
            AddPocket(new Pocket(Rank.Two, Rank.Two));

            // Kxs
            AddPocket(new Pocket(Rank.King, Rank.Eight, true));
            AddPocket(new Pocket(Rank.King, Rank.Seven, true));
            AddPocket(new Pocket(Rank.King, Rank.Six, true));
            AddPocket(new Pocket(Rank.King, Rank.Five, true));
            AddPocket(new Pocket(Rank.King, Rank.Four, true));
            AddPocket(new Pocket(Rank.King, Rank.Three, true));
            AddPocket(new Pocket(Rank.King, Rank.Two, true));

            // T7s
            AddPocket(new Pocket(Rank.Ten, Rank.Seven, true));

            // Q8s
            AddPocket(new Pocket(Rank.Queen, Rank.Eight, true));

        }
    }
    #endregion

    #region Group8
    internal class Group8 : Group
    {
        public Group8() : base(8)
        {

            // 87
            AddPocket(new Pocket(Rank.Eight, Rank.Seven));

            // A9
            AddPocket(new Pocket(Rank.Ace, Rank.Nine));

            // Q9
            AddPocket(new Pocket(Rank.Queen, Rank.Nine));

            // 76
            AddPocket(new Pocket(Rank.Seven, Rank.Six));

            // 42s
            AddPocket(new Pocket(Rank.Four, Rank.Two, true));

            // 32s
            AddPocket(new Pocket(Rank.Three, Rank.Two, true));

            // 96s
            AddPocket(new Pocket(Rank.Nine, Rank.Six, true));

            // 85s
            AddPocket(new Pocket(Rank.Eight, Rank.Five, true));

            // J8
            AddPocket(new Pocket(Rank.Jack, Rank.Eight));

            // J7s
            AddPocket(new Pocket(Rank.Jack, Rank.Seven, true));

            // 65
            AddPocket(new Pocket(Rank.Six, Rank.Five));

            // 54
            AddPocket(new Pocket(Rank.Five, Rank.Four));

            // 74s
            AddPocket(new Pocket(Rank.Seven, Rank.Four, true));

            // K9
            AddPocket(new Pocket(Rank.King, Rank.Nine));

            // T8
            AddPocket(new Pocket(Rank.Ten, Rank.Eight));
        }
    }
    #endregion

    #region PocketGroups
    internal class PocketGroups
    {
        internal List<Group> _groups;

        public PocketGroups()
        {
            _groups = new List<Group>();
            _groups.Add(new Group1());
            _groups.Add(new Group2());
            _groups.Add(new Group3());
            _groups.Add(new Group4());
            _groups.Add(new Group5());
            _groups.Add(new Group6());
            _groups.Add(new Group7());
            _groups.Add(new Group8());
        }

        public int GetStrength(List<Card> pocket)
        {
            int strength = 9;
            foreach (var group in _groups)
            {
                if (group.hasPocket(pocket))
                {
                    if (strength > group.Strength) strength = group.Strength;
                }
            }
            return strength;
        }

    }
    #endregion
}
