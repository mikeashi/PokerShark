using FluidHTN;
using PokerShark.Core.HTN;
using PokerShark.Core.HTN.Context;
using PokerShark.Core.HTN.Domain;
using PokerShark.Core.Poker;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;
using Serilog.Context;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Xml.Linq;
using TaskStatus = FluidHTN.TaskStatus;

namespace PokerShark.UnitTests.Core
{
    [TestClass]
    public class MiddlePositionTest
    {
        [TestMethod]
        public void Test1()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();
            
            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.AceOfClubs, StateCard.AceOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);
               
            // assertions
            Assert.AreEqual((0, 0.25f, 0.75f), context.GetDecision());
        }

        [TestMethod]
        public void Test2()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.SixOfClubs, StateCard.SixOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.6f, 0.4f), context.GetDecision());
        }

        [TestMethod]
        public void Test3()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.SixOfClubs, StateCard.SixOfDiamonds));

            // set tight
            var action = new RaiseAction(20, 80);
            action.PlayerId = "1";
            action.Stage = StreetState.Flop;
            context.UpdatePlayerModel("Player1", action);
            
            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.7f, 0.3f, 0), context.GetDecision());
        }

        [TestMethod]
        public void Test4()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.QueenOfHearts, StateCard.JackOfDiamonds));

            // set tight
            var action = new RaiseAction(20, 80);
            action.PlayerId = "1";
            action.Stage = StreetState.Flop;
            context.UpdatePlayerModel("Player1", action);

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.6f, 0.4f), context.GetDecision());
        }


        private PokerContext getMiddlePositionContext(List<Card> pocket)
        {
            // setup context
            var context = new PokerContext();

            // setup game
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "1"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "2"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "3"));
            seats.Add(new Seat(1000, PlayerState.Participating, "PokerShark", "5"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "4"));
            GameInfo game = new GameInfo(5, 1000, 100, 10, 20, 0, seats);

            // initialize game
            context.Initialize(game);


            // initialize round
            context.SetRoundCount(1);
            context.SetPocketCards(pocket);
            context.SetSeats(seats);

            // initialize street
            context.SetCurrentRound(new RoundState(0, 0, 1, StreetState.Preflop, seats, 0, 1, new List<Card>(), 0, 0, new List<PyAction>()));

            // initialize valid actions
            List<PyAction> validActions = new List<PyAction>();
            validActions.Add(new CallAction(20));
            validActions.Add(new FoldAction());
            validActions.Add(new RaiseAction(20, 800));
            context.SetValidActions(validActions);
            
            return context;
        }


        private static Domain<PokerContext, object> getPreflopDomain()
        {
            return new PokerDomainBuilder("MiddlePosition")
                            .PreflopSequence()
                            .Build();
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