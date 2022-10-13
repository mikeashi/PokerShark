using PokerShark.Core.HTN;
using PokerShark.Core.PyPoker;
using PokerShark.Core.Poker.Deck;

namespace PokerShark.UnitTests.Core.Poker.Deck
{
    [TestClass]
    public class GroupsTest
    {
        [TestMethod]
        public void TestPairs()
        {
            Groups groups = new Groups();

            // AA
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.AceOfClubs , StateCard.AceOfDiamonds )));
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.AceOfHearts , StateCard.AceOfDiamonds )));
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.AceOfSpades, StateCard.AceOfDiamonds )));
            
            // KK
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.KingOfClubs, StateCard.KingOfDiamonds )));
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.KingOfHearts, StateCard.KingOfSpades)));
            
            // QQ
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.QueenOfClubs, StateCard.QueenOfDiamonds)));
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.QueenOfHearts, StateCard.QueenOfDiamonds)));
            
            // JJ
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.JackOfClubs, StateCard.JackOfHearts)));
            
            // TT
            Assert.AreEqual(2, groups.GetStrength(getCardList(StateCard.TenOfClubs, StateCard.TenOfHearts)));
            
            // 99 
            Assert.AreEqual(3, groups.GetStrength(getCardList(StateCard.NineOfDiamonds, StateCard.NineOfSpades)));

            // 88
            Assert.AreEqual(4, groups.GetStrength(getCardList(StateCard.EightOfClubs, StateCard.EightOfDiamonds)));

            // 77
            Assert.AreEqual(5, groups.GetStrength(getCardList(StateCard.SevenOfHearts, StateCard.SevenOfSpades)));

            // 66
            Assert.AreEqual(6, groups.GetStrength(getCardList(StateCard.SixOfClubs, StateCard.SixOfSpades)));

            // 55
            Assert.AreEqual(6, groups.GetStrength(getCardList(StateCard.FiveOfClubs, StateCard.FiveOfSpades)));

            // 44
            Assert.AreEqual(7, groups.GetStrength(getCardList(StateCard.FourOfDiamonds, StateCard.FourOfSpades)));

            // 33
            Assert.AreEqual(7, groups.GetStrength(getCardList(StateCard.ThreeOfHearts, StateCard.ThreeOfSpades)));

            // 22
            Assert.AreEqual(7, groups.GetStrength(getCardList(StateCard.TwoOfHearts, StateCard.TwoOfSpades)));






        }

        [TestMethod]
        public void TestGroups()
        {
            Groups groups = new Groups();

            // AKs
            Assert.AreEqual(1, groups.GetStrength(getCardList(StateCard.AceOfClubs, StateCard.KingOfClubs)));

            // AK
            Assert.AreEqual(2, groups.GetStrength(getCardList(StateCard.AceOfClubs, StateCard.KingOfDiamonds)));

            // AQs
            Assert.AreEqual(2, groups.GetStrength(getCardList(StateCard.AceOfClubs, StateCard.QueenOfClubs)));

            // AQ
            Assert.AreEqual(3, groups.GetStrength(getCardList(StateCard.AceOfClubs, StateCard.QueenOfDiamonds)));
           
            // AJs
            Assert.AreEqual(2, groups.GetStrength(getCardList(StateCard.AceOfClubs, StateCard.JackOfClubs)));

            // AJ
            Assert.AreEqual(4, groups.GetStrength(getCardList(StateCard.AceOfClubs, StateCard.JackOfDiamonds)));

            // 74s
            Assert.AreEqual(8, groups.GetStrength(getCardList(StateCard.SevenOfClubs, StateCard.FourOfClubs)));
            Assert.AreEqual(9, groups.GetStrength(getCardList(StateCard.SevenOfClubs, StateCard.FourOfDiamonds)));


        }
        private List<Card> getCardList(StateCard card1, StateCard card2)
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card(card1));
            cards.Add(new Card(card2));
            return cards;
        }
    }
}