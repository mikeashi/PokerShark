using FluidHTN;
using PokerShark.Core.HTN;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.HTN.Domain;
using PokerShark.Core.Poker;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;
using Serilog.Context;
using System.Diagnostics;
using System.Numerics;
using TaskStatus = FluidHTN.TaskStatus;

namespace PokerShark.UnitTests.Core
{
    [TestClass]
    public class BotPreflopTest
    {
        //[TestMethod]
        public void TestStartGame()
        {
            // setup game
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000,PlayerState.Participating,"player1","1"));
            seats.Add(new Seat(1000,PlayerState.Participating,"player2","2"));
            seats.Add(new Seat(1000,PlayerState.Participating,"player3","3"));
            seats.Add(new Seat(1000,PlayerState.Participating,"player4","4"));
            seats.Add(new Seat(1000,PlayerState.Participating,"PokerShark","5"));
            GameInfo game = new GameInfo(5,1000,100,10,20,0, seats);

            // Setup bot
            Bot bot = new Bot();

            // Start Game
            bot.GameStarted(game);

            // Start Round
            var pocket = getPoket(StateCard.AceOfClubs, StateCard.AceOfDiamonds);
            bot.RoundStarted(1, pocket, seats);

            // Start Street
            RoundState currentRound = new RoundState(0,0,1,StreetState.Preflop,seats,0,1,new List<Card>(),0,0, new List<PyAction>());
            bot.StreetStarted(currentRound);

            // Get Action
            List<PyAction> validActions = new List<PyAction>();
            validActions.Add(new CallAction(20));
            validActions.Add(new FoldAction());
            validActions.Add(new RaiseAction(20,800));
            var action = bot.DeclareAction(validActions, currentRound, pocket);
            Assert.IsTrue(action is FoldAction);
            Assert.AreEqual(Position.Late, bot.Context.GetPosition());
        }

        private List<Card> getPoket(StateCard card1, StateCard card2)
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card(card1));
            cards.Add(new Card(card2));
            return cards;
        }

    }
}