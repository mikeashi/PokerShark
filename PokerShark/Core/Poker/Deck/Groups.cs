using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.Poker.Deck
{
    public abstract class Group
    {
        internal List<PocketHand> Hands { get; set; }
        public int Strength { get; }

        public Group(int strength)
        {
            Strength = strength;
            Hands = new List<PocketHand>();
        }

        public void AddPocketHand(PocketHand hand)
        {
            Hands.Add(hand);
        }

        public bool hasHand(List<Card> cards)
        {
            foreach(var hand in Hands)
            {
                if (hand.Equals(cards)) return true;
            }
            return false;
        }
    }

    public class Group1 : Group
    {
        public Group1():base(1)
        {
            // AA
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Ace));
            // KK
            AddPocketHand(new PocketHand(Rank.King, Rank.King));
            // QQ
            AddPocketHand(new PocketHand(Rank.Queen, Rank.Queen));
            // JJ
            AddPocketHand(new PocketHand(Rank.Jack, Rank.Jack));
            // AKs
            AddPocketHand(new PocketHand(Rank.Ace, Rank.King, true));
        }
    }

    public class Group2 : Group
    {
        public Group2() : base(2)
        {
            // TT
            AddPocketHand(new PocketHand(Rank.Ten, Rank.Ten));

            // AK 
            AddPocketHand(new PocketHand(Rank.Ace, Rank.King));

            // AQs
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Queen,true));

            // AJs
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Jack, true));

            // KQs
            AddPocketHand(new PocketHand(Rank.King, Rank.Queen, true));

        }
    }

    public class Group3 : Group
    {
        public Group3() : base(3)
        {
            // 99
            AddPocketHand(new PocketHand(Rank.Nine, Rank.Nine));

            // JTs
            AddPocketHand(new PocketHand(Rank.Jack, Rank.Ten,true));

            // QJs
            AddPocketHand(new PocketHand(Rank.Queen, Rank.Jack, true));

            // KJs
            AddPocketHand(new PocketHand(Rank.King, Rank.Jack, true));

            // ATs
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Ten, true));

            // AQ
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Queen));

        }
    }

    public class Group4 : Group
    {
        public Group4() : base(4)
        {
            // T9s
            AddPocketHand(new PocketHand(Rank.Ten, Rank.Nine,true));

            // KQ
            AddPocketHand(new PocketHand(Rank.King, Rank.Queen));

            // 88
            AddPocketHand(new PocketHand(Rank.Eight, Rank.Eight));

            // QTs
            AddPocketHand(new PocketHand(Rank.Queen, Rank.Ten,true));

            // 98s
            AddPocketHand(new PocketHand(Rank.Nine, Rank.Eight, true));

            // J9s
            AddPocketHand(new PocketHand(Rank.Jack, Rank.Nine, true));

            // AJ
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Jack));

            // KTs
            AddPocketHand(new PocketHand(Rank.King, Rank.Ten,true));
        }
    }

    public class Group5 : Group
    {
        public Group5() : base(5)
        {
            // 77
            AddPocketHand(new PocketHand(Rank.Seven, Rank.Seven));

            // 87s
            AddPocketHand(new PocketHand(Rank.Eight, Rank.Seven,true));

            // Q9s
            AddPocketHand(new PocketHand(Rank.Queen, Rank.Nine, true));

            // T8s
            AddPocketHand(new PocketHand(Rank.Ten, Rank.Eight, true));

            // KJ
            AddPocketHand(new PocketHand(Rank.King, Rank.Jack));

            // QJ
            AddPocketHand(new PocketHand(Rank.Queen, Rank.Jack));

            // JT
            AddPocketHand(new PocketHand(Rank.Jack, Rank.Ten));

            // 76s
            AddPocketHand(new PocketHand(Rank.Seven, Rank.Six,true));

            // 97s
            AddPocketHand(new PocketHand(Rank.Nine, Rank.Seven,true));

            // Axs (8,7,6,5,4,3,2)
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Eight, true));
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Seven, true));
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Six, true));
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Five, true));
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Four, true));
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Three, true));
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Two, true));

            // 65s
            AddPocketHand(new PocketHand(Rank.Six, Rank.Five, true));

        }
    }

    public class Group6 : Group
    {
        public Group6() : base(6)
        {
            // 66
            AddPocketHand(new PocketHand(Rank.Six, Rank.Six));

            // AT
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Ten));

            // 55
            AddPocketHand(new PocketHand(Rank.Five, Rank.Five));

            // 86s
            AddPocketHand(new PocketHand(Rank.Eight, Rank.Six,true));

            // KT
            AddPocketHand(new PocketHand(Rank.King, Rank.Ten));

            // QT
            AddPocketHand(new PocketHand(Rank.Queen, Rank.Ten));

            // 54s
            AddPocketHand(new PocketHand(Rank.Five, Rank.Four, true));

            // K9s
            AddPocketHand(new PocketHand(Rank.King, Rank.Nine, true));

            // J8s
            AddPocketHand(new PocketHand(Rank.Jack, Rank.Eight, true));

            // 75s
            AddPocketHand(new PocketHand(Rank.Seven, Rank.Five, true));
        }
    }

    public class Group7 : Group
    {
        public Group7() : base(7)
        {

            // 44
            AddPocketHand(new PocketHand(Rank.Four, Rank.Four));

            // J9
            AddPocketHand(new PocketHand(Rank.Jack, Rank.Nine));

            // 64s
            AddPocketHand(new PocketHand(Rank.Six, Rank.Four,true));

            // T9
            AddPocketHand(new PocketHand(Rank.Ten, Rank.Nine));

            // 53s
            AddPocketHand(new PocketHand(Rank.Five, Rank.Three, true));

            // 33
            AddPocketHand(new PocketHand(Rank.Three, Rank.Three));

            // 98
            AddPocketHand(new PocketHand(Rank.Nine, Rank.Eight));

            // 43s
            AddPocketHand(new PocketHand(Rank.Four, Rank.Three, true));

            // 22
            AddPocketHand(new PocketHand(Rank.Two, Rank.Two));

            // Kxs
            AddPocketHand(new PocketHand(Rank.King, Rank.Eight, true));
            AddPocketHand(new PocketHand(Rank.King, Rank.Seven, true));
            AddPocketHand(new PocketHand(Rank.King, Rank.Six, true));
            AddPocketHand(new PocketHand(Rank.King, Rank.Five, true));
            AddPocketHand(new PocketHand(Rank.King, Rank.Four, true));
            AddPocketHand(new PocketHand(Rank.King, Rank.Three, true));
            AddPocketHand(new PocketHand(Rank.King, Rank.Two, true));

            // T7s
            AddPocketHand(new PocketHand(Rank.Ten, Rank.Seven, true));

            // Q8s
            AddPocketHand(new PocketHand(Rank.Queen, Rank.Eight, true));

        }
    }

    public class Group8 : Group
    {
        public Group8() : base(8)
        {

            // 87
            AddPocketHand(new PocketHand(Rank.Eight, Rank.Seven));

            // A9
            AddPocketHand(new PocketHand(Rank.Ace, Rank.Nine));

            // Q9
            AddPocketHand(new PocketHand(Rank.Queen, Rank.Nine));

            // 76
            AddPocketHand(new PocketHand(Rank.Seven, Rank.Six));

            // 42s
            AddPocketHand(new PocketHand(Rank.Four, Rank.Two,true));

            // 32s
            AddPocketHand(new PocketHand(Rank.Three, Rank.Two, true));

            // 96s
            AddPocketHand(new PocketHand(Rank.Nine, Rank.Six, true));

            // 85s
            AddPocketHand(new PocketHand(Rank.Eight, Rank.Five, true));

            // J8
            AddPocketHand(new PocketHand(Rank.Jack, Rank.Eight));

            // J7s
            AddPocketHand(new PocketHand(Rank.Jack, Rank.Seven,true));

            // 65
            AddPocketHand(new PocketHand(Rank.Six, Rank.Five));

            // 54
            AddPocketHand(new PocketHand(Rank.Five, Rank.Four));

            // 74s
            AddPocketHand(new PocketHand(Rank.Seven, Rank.Four, true));

            // K9
            AddPocketHand(new PocketHand(Rank.King, Rank.Nine));

            // T8
            AddPocketHand(new PocketHand(Rank.Ten, Rank.Eight));
        }
    }

    public class Groups
    {
        public List<Group> _groups;

        public Groups()
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
                if (group.hasHand(pocket))
                {
                    if (strength > group.Strength) strength = group.Strength;
                }
            }
            return strength;
        }

    }
}
