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
        public void AlwaysRaiseWithGroup1()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();
            
            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.AceOfClubs, StateCard.AceOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);
               
            // assertions
            Assert.AreEqual((0, 0, 1), context.GetDecision());
        }

        [TestMethod]
        public void IfCallersAlwaysRaiseonAQ()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.QueenOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0, 1), context.GetDecision());
        }

        [TestMethod]
        public void IfCallersCallOnJTs()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContextCalled(getPoket(StateCard.JackOfClubs, StateCard.TenOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 1, 0), context.GetDecision());
        }

        [TestMethod]
        public void IfCallersSometimesRaiseOnAJ()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.JackOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.4f, 0.6f), context.GetDecision());
        }

        [TestMethod]
        public void IfCallersSometimesRaiseOnGroup3()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContextCalled(getPoket(StateCard.QueenOfDiamonds, StateCard.JackOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.4f, 0.6f), context.GetDecision());
        }

        [TestMethod]
        public void IfNoRaiseAlwaysRaiseGroup3()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.QueenOfDiamonds, StateCard.JackOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0, 1), context.GetDecision());
        }

        [TestMethod]
        public void IfNoRaiseLooseAggressiveFoldKJ()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.KingOfClubs, StateCard.JackOfDiamonds));

            // set aggresive
            var action = new RaiseAction(20, 50);
            action.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action);
            
            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((1, 0, 0), context.GetDecision());
        }

        [TestMethod]
        public void IfNoRaiseLooseAlwaysRaiseGroup6()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.KingOfClubs, StateCard.NineOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0, 1), context.GetDecision());
        }

        [TestMethod]
        public void IfNoRaiseTightUsuallyFoldOn6()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.KingOfClubs, StateCard.NineOfClubs));

            // set aggresive
            var action = new FoldAction();
            action.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action);
            var action2 = new FoldAction();
            action2.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action2);
            action2 = new FoldAction();
            action2.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action2);
            
            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((1, 0, 0), context.GetDecision());
        }

        [TestMethod]
        public void IfNoRaiseTightUsuallyRaiseGroup4()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContext(getPoket(StateCard.KingOfClubs, StateCard.TenOfClubs));

            // set aggresive
            var action = new FoldAction();
            action.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action);
            var action2 = new FoldAction();
            action2.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action2);
            action2 = new FoldAction();
            action2.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action2);

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.2f, 0.8f), context.GetDecision());
        }


        [TestMethod]
        public void OneRaiseLooseRaiserUsuallyRaiseAQ()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContextRaised(getPoket(StateCard.AceOfClubs, StateCard.QueenOfDiamonds));

            // set aggresive
            var action = new CallAction(20);
            action.Stage = StreetState.Preflop;
            action.PlayerId = "1";
            context.UpdatePlayerModel("player1", action);
            var action2 = new FoldAction();
            action2.Stage = StreetState.Flop;
            action2.PlayerId = "1";
            context.UpdatePlayerModel("player1", action2);
            action2 = new FoldAction();
            action2.PlayerId = "1";
            action2.Stage = StreetState.Flop;
            context.UpdatePlayerModel("player1", action2);
            
            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.2f, 0.8f), context.GetDecision());
        }


        [TestMethod]
        public void TwoRaisesAlwaysRaiseOnAA()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContextRaisedTwice(getPoket(StateCard.AceOfClubs, StateCard.AceOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0, 1), context.GetDecision());
        }

        [TestMethod]
        public void TwoRaisesAlwaysRaiseFoldOnWeakHands()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getMiddlePositionContextRaisedTwice(getPoket(StateCard.TwoOfClubs, StateCard.FiveOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((1, 0, 0), context.GetDecision());
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

        private PokerContext getMiddlePositionContextCalled(List<Card> pocket)
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

            List<PyAction> history = new List<PyAction>();
            var action = new CallAction(20);
            action.Stage = StreetState.Preflop;
            action.PlayerId = "1";
            history.Add(action);
            
            // initialize street
            context.SetCurrentRound(new RoundState(0, 0, 1, StreetState.Preflop, seats, 0, 1, new List<Card>(), 0, 0, history));

            // initialize valid actions
            List<PyAction> validActions = new List<PyAction>();
            validActions.Add(new CallAction(20));
            validActions.Add(new FoldAction());
            validActions.Add(new RaiseAction(20, 800));
            context.SetValidActions(validActions);

            return context;
        }

        private PokerContext getMiddlePositionContextRaised(List<Card> pocket)
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

            List<PyAction> history = new List<PyAction>();
            var action = new RaiseAction(20,20);
            action.Stage = StreetState.Preflop;
            action.PlayerId = "1";
            history.Add(action);

            // initialize street
            context.SetCurrentRound(new RoundState(0, 0, 1, StreetState.Preflop, seats, 0, 1, new List<Card>(), 0, 0, history));

            // initialize valid actions
            List<PyAction> validActions = new List<PyAction>();
            validActions.Add(new CallAction(20));
            validActions.Add(new FoldAction());
            validActions.Add(new RaiseAction(20, 800));
            context.SetValidActions(validActions);

            return context;
        }


        private PokerContext getMiddlePositionContextRaisedTwice(List<Card> pocket)
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

            List<PyAction> history = new List<PyAction>();
            var action = new RaiseAction(20, 20);
            action.Stage = StreetState.Preflop;
            action.PlayerId = "1";
            history.Add(action);

            var action2 = new RaiseAction(20, 20);
            action2.Stage = StreetState.Preflop;
            action2.PlayerId = "1";
            history.Add(action2);

            // initialize street
            context.SetCurrentRound(new RoundState(0, 0, 1, StreetState.Preflop, seats, 0, 1, new List<Card>(), 0, 0, history));

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