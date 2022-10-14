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
        public void Test1()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();
            
            // setup context
            PokerContext context = getEarlyPositionContext(getPoket(StateCard.AceOfClubs, StateCard.AceOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);
               
            // assertions
            Assert.AreEqual((0, 0.1f, 0.9f), context.GetDecision());
        }

        [TestMethod]
        public void Test2()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContext(getPoket(StateCard.AceOfClubs, StateCard.JackOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0, 0.25f, 0.75f), context.GetDecision());
        }


        [TestMethod]
        public void Test3()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContext(getPoket(StateCard.KingOfClubs, StateCard.QueenOfDiamonds));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.4f, 0.6f, 0), context.GetDecision());
        }


        [TestMethod]
        public void Test4()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContext(getPoket(StateCard.AceOfHearts, StateCard.QueenOfDiamonds));

            // set tight
            var action = new RaiseAction(20, 80);
            action.PlayerId = "1";
            action.Stage = StreetState.Flop;
            context.UpdatePlayerModel("Player1", action);

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.3f, 0.7f, 0), context.GetDecision());
        }


        [TestMethod]
        public void Test5()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContext(getPoket(StateCard.EightOfClubs, StateCard.SixOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.6f, 0.4f, 0), context.GetDecision());
        }


        [TestMethod]
        public void Test6()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionContext(getPoket(StateCard.SixOfDiamonds, StateCard.SevenOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.9f, 0.1f, 0), context.GetDecision());
        }

        [TestMethod]
        public void Test7()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionRaiseContext(getPoket(StateCard.QueenOfClubs, StateCard.JackOfClubs));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.2f, 0.7f, 0.1f), context.GetDecision());
        }


        [TestMethod]
        public void Test8()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionRaiseContext(getPoket(StateCard.KingOfClubs, StateCard.QueenOfHearts));

            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.9f, 0.1f, 0), context.GetDecision());
        }
        
        [TestMethod]
        public void Test9()
        {
            // setup domain
            Domain<PokerContext, object> domain = getPreflopDomain();

            // setup context
            PokerContext context = getEarlyPositionRaiseContext(getPoket(StateCard.AceOfDiamonds, StateCard.QueenOfHearts));

            // set tight
            var action = new FoldAction();
            action.PlayerId = "1";
            action.Stage = StreetState.Preflop;
            var action2 = new RaiseAction(40,40);
            action2.PlayerId = "1";
            action2.Stage = StreetState.Flop;
            
            context.UpdatePlayerModel("Player1", action);
            context.UpdatePlayerModel("Player1", action2);



            // start planning
            var planner = new Planner<PokerContext, Object>();
            planner.Tick(domain, context);

            // assertions
            Assert.AreEqual((0.9f, 0.1f, 0), context.GetDecision());
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