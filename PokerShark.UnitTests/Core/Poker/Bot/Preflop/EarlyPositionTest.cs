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
    public class EarlyPositionTest
    {
        [TestMethod]
        public void NoCallsAlwaysRaiseOnAQ()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();
            
            // setup context
            PokerContext context = getEarlyPositionContext(getPoket(StateCard.AceOfClubs, StateCard.QueenOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);
               
            // assertions
            Assert.AreEqual((0, 0.0f, 1), context.GetDecision());
        }

        [TestMethod]
        public void NoCallsAlwaysRaiseOnKK()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContext(getPoket(StateCard.KingOfClubs, StateCard.KingOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.0f, 1), context.GetDecision());
        }


        [TestMethod]
        public void NoRaisesUsallyRaiseOnGroup1()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.KingOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.2f, 0.8f), context.GetDecision());
        }



        [TestMethod]
        public void NoRaisesUsallyRaiseOnGroup2()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.QueenOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.2f, 0.8f), context.GetDecision());
        }

        [TestMethod]
        public void NoRaisesUsallyRaiseOnAQ()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.QueenOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.2f, 0.8f), context.GetDecision());
        }

        [TestMethod]
        public void NoRaisesSometimesRaiseonGroup3()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.TenOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.4f, 0.6f), context.GetDecision());
        }


        [TestMethod]
        public void NoRaisesLooseCallorRaiseOnGroup4()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.JackOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.6f, 0.4f), context.GetDecision());
        }

        [TestMethod]
        public void NoRaisesLooseAggressiveFoldAJ()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.JackOfDiamonds));

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
        public void NoRaisesLoosePassiveCallOnStuitedConnectors()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextCalled(getPoket(StateCard.EightOfClubs, StateCard.SevenOfClubs));

            // set aggresive
            var action = new CallAction(20);
            action.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action);
            var action2 = new FoldAction();
            action2.Stage = StreetState.Flop;
            context.UpdatePlayerModel("player1", action2);
            action2 = new FoldAction();
            action2.Stage = StreetState.Flop;
            context.UpdatePlayerModel("player1", action2);

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.6f, 0.4f), context.GetDecision());
        }

        [TestMethod]
        public void NoRaisesLoosePassiveSometimesCallOnGroup5()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextCalled(getPoket(StateCard.AceOfClubs, StateCard.NineOfClubs));

            // set aggresive
            var action = new CallAction(20);
            action.Stage = StreetState.Preflop;
            context.UpdatePlayerModel("player1", action);
            var action2 = new FoldAction();
            action2.Stage = StreetState.Flop;
            context.UpdatePlayerModel("player1", action2);
            action2 = new FoldAction();
            action2.Stage = StreetState.Flop;
            context.UpdatePlayerModel("player1", action2);

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.4f, 0.6f, 0), context.GetDecision());
        }


        [TestMethod]
        public void OneRaiseCallOnAJs()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextRaised(getPoket(StateCard.AceOfClubs, StateCard.JackOfClubs));


            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 1, 0), context.GetDecision());
        }

        [TestMethod]
        public void OneRaiseUsuallyRaiseOnGroup1()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextRaised(getPoket(StateCard.AceOfClubs, StateCard.AceOfDiamonds));


            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.2f, 0.8f), context.GetDecision());
        }

        [TestMethod]
        public void OneRaiseLooseRaiserUsuallyRaiseWith99()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextRaised(getPoket(StateCard.NineOfClubs, StateCard.NineOfDiamonds));

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
        public void TwoRaisesLooseGameFoldOnAQ()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextRaisedTwice(getPoket(StateCard.AceOfClubs, StateCard.QueenOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((1, 0, 0), context.GetDecision());
        }

        [TestMethod]
        public void TwoRaisesLooseGameUsuallyRaiseOn3()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextRaisedTwice(getPoket(StateCard.AceOfClubs, StateCard.TenOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.2f, 0.8f), context.GetDecision());
        }

        [TestMethod]
        public void TwoRaisesTightGameFoldonAJs()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextRaisedTwice(getPoket(StateCard.AceOfClubs, StateCard.JackOfClubs));

            // set tight
            var action = new FoldAction();
            action.Stage = StreetState.Preflop;
            action.PlayerId = "1";
            context.UpdatePlayerModel("player1", action);
            
            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((1, 0, 0), context.GetDecision());
        }

        [TestMethod]
        public void TwoRaisesTightGameUsuallyRaiseonGroup1()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextRaisedTwice(getPoket(StateCard.AceOfClubs, StateCard.AceOfDiamonds));

            // set tight
            var action = new FoldAction();
            action.Stage = StreetState.Preflop;
            action.PlayerId = "1";
            context.UpdatePlayerModel("player1", action);

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.2f, 0.8f), context.GetDecision());
        }

        [TestMethod]
        public void TwoRaisesTightGameFoldOnWeakCards()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContextRaisedTwice(getPoket(StateCard.TwoOfHearts, StateCard.ThreeOfClubs));

            // set tight
            var action = new FoldAction();
            action.Stage = StreetState.Preflop;
            action.PlayerId = "1";
            context.UpdatePlayerModel("player1", action);

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((1, 0, 0), context.GetDecision());
        }

        private PokerContext getEarlyPositionContext(List<Card> pocket)
        {
            // setup context
            var context = new PokerContext();

            // setup game
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "1"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "2"));
            seats.Add(new Seat(1000, PlayerState.Participating, "PokerShark", "5"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "3"));
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

        private PokerContext getEarlyPositionContextCalled(List<Card> pocket)
        {
            // setup context
            var context = new PokerContext();

            // setup game
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "1"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "2"));
            seats.Add(new Seat(1000, PlayerState.Participating, "PokerShark", "5"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "3"));
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
        
        private PokerContext getEarlyPositionContextRaised(List<Card> pocket)
        {
            // setup context
            var context = new PokerContext();

            // setup game
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "1"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "2"));
            seats.Add(new Seat(1000, PlayerState.Participating, "PokerShark", "5"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "3"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "4"));
            GameInfo game = new GameInfo(5, 1000, 100, 10, 20, 0, seats);

            // initialize game
            context.Initialize(game);


            // initialize round
            context.SetRoundCount(1);
            context.SetPocketCards(pocket);
            context.SetSeats(seats);

            List<PyAction> history = new List<PyAction>();
            var action = new RaiseAction(20,40);
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

        private PokerContext getEarlyPositionContextRaisedTwice(List<Card> pocket)
        {
            // setup context
            var context = new PokerContext();

            // setup game
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "1"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "2"));
            seats.Add(new Seat(1000, PlayerState.Participating, "PokerShark", "5"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "3"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "4"));
            GameInfo game = new GameInfo(5, 1000, 100, 10, 20, 0, seats);

            // initialize game
            context.Initialize(game);


            // initialize round
            context.SetRoundCount(1);
            context.SetPocketCards(pocket);
            context.SetSeats(seats);

            List<PyAction> history = new List<PyAction>();
            var action = new RaiseAction(20, 40);
            action.Stage = StreetState.Preflop;
            action.PlayerId = "1";
            history.Add(action);

            var action2 = new RaiseAction(20, 40);
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

        private PokerContext getEarlyPositionRaiseContext(List<Card> pocket)
        {
            // setup context
            var context = new PokerContext();

            // setup game
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "1"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "2"));
            seats.Add(new Seat(1000, PlayerState.Participating, "PokerShark", "5"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "3"));
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

            // raise action 
            var actions = new List<PyAction>();
            var raiseAction = new RaiseAction(20, 800);
            raiseAction.PlayerId = "1";
            actions.Add(raiseAction);
            var round = new RoundState(0, 0, 1, StreetState.Preflop, seats, 0, 1, new List<Card>(), 0, 0, actions);

            context.SetCurrentRound(round);
            context.UpdatePlayerModel("Player1", raiseAction);
            
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
            return new PokerDomainBuilder("EarlyPosition")
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