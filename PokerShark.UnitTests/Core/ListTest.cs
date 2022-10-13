using PokerShark.Core.HTN;
using PokerShark.Core.Poker;
using PokerShark.Core.PyPoker;

namespace PokerShark.UnitTests.Core
{
    [TestClass]
    public class ListTest
    {
        [TestMethod]
        public void TestObject()
        {
            List<PlayerModel> models = new List<PlayerModel>();
            PlayerModel player = new PlayerModel("test", "12345");

            // find model
            var playerModel = player;
            models.Add(playerModel);

            Assert.AreEqual("test", playerModel.Name);

            // update model
            var a1 = new CallAction(20);
            a1.Stage = StreetState.Preflop;
            var a2 = new FoldAction();
            a1.Stage = StreetState.Preflop;
            var a3 = new RaiseAction(20,20);
            a1.Stage = StreetState.Preflop;
            
            playerModel.UpdateHistory(a1);
            playerModel.UpdateHistory(a2);
            playerModel.UpdateHistory(a3);

            var AggressionIndex = playerModel.AggressionIndex;
            var LooseIndex = playerModel.LooseIndex;

            // did the list reference change?
            var listPlayerModel = models.Find(m => m.Id == "12345");
            Assert.AreEqual("test", listPlayerModel.Name);
            Assert.AreEqual(AggressionIndex, listPlayerModel.AggressionIndex);
            Assert.AreEqual(LooseIndex, listPlayerModel.LooseIndex);


        }
    }
}