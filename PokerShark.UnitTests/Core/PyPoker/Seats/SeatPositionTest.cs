using PokerShark.Core.PyPoker;

namespace PokerShark.UnitTests.Core.PyPoker.Seats
{
    [TestClass]
    public class WeightedSelectionTest
    {
        #region two
        [TestMethod]
        public void TestMethod2_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 0;
            // Execute
            roundState.UpdatePositions();
            // Test
            Assert.AreEqual(2, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
        }

        [TestMethod]
        public void TestMethod2_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;
            // Execute
            roundState.UpdatePositions();
            // Test
            Assert.AreEqual(2, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
        }
        #endregion

        #region three

        [TestMethod]
        public void TestMethod3_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(3, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
        }

        [TestMethod]
        public void TestMethod3_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 2;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(3, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
        }

        [TestMethod]
        public void TestMethod3_3()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 2;
            roundState.BigBlindPosition = 0;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(3, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
        }
        #endregion

        #region four

        [TestMethod]
        public void TestMethod4_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(4, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
        }

        [TestMethod]
        public void TestMethod4_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 2;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(4, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
        }

        [TestMethod]
        public void TestMethod4_3()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 2;
            roundState.BigBlindPosition = 3;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(4, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
        }

        [TestMethod]
        public void TestMethod4_4()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 3;
            roundState.BigBlindPosition = 0;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(4, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
        }
        #endregion

        #region five

        [TestMethod]
        public void TestMethod5_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(5, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
        }

        [TestMethod]
        public void TestMethod5_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 2;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(5, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
        }

        [TestMethod]
        public void TestMethod5_3()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 2;
            roundState.BigBlindPosition = 3;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(5, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
        }

        [TestMethod]
        public void TestMethod5_4()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 3;
            roundState.BigBlindPosition = 4;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(5, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
        }

        [TestMethod]
        public void TestMethod5_5()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 4;
            roundState.BigBlindPosition = 0;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(5, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
        }
        #endregion

        #region six

        [TestMethod]
        public void TestMethod6_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(6, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
        }

        [TestMethod]
        public void TestMethod6_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 2;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(6, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
        }

        [TestMethod]
        public void TestMethod6_3()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 2;
            roundState.BigBlindPosition = 3;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(6, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
        }
        [TestMethod]
        public void TestMethod6_4()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 3;
            roundState.BigBlindPosition = 4;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(6, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
        }
        [TestMethod]
        public void TestMethod6_5()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 4;
            roundState.BigBlindPosition = 5;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(6, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
        }
        [TestMethod]
        public void TestMethod6_6()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 5;
            roundState.BigBlindPosition = 0;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(6, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
        }
        #endregion

        #region seven

        [TestMethod]
        public void TestMethod7_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(7, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[6].Position);
        }
        [TestMethod]
        public void TestMethod7_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 2;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(7, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
        }
        [TestMethod]
        public void TestMethod7_3()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 2;
            roundState.BigBlindPosition = 3;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(7, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
        }
        [TestMethod]
        public void TestMethod7_4()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 3;
            roundState.BigBlindPosition = 4;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(7, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
        }
        [TestMethod]
        public void TestMethod7_5()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 4;
            roundState.BigBlindPosition = 5;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(7, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
        }
        [TestMethod]
        public void TestMethod7_6()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 5;
            roundState.BigBlindPosition = 6;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(7, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
        }
        [TestMethod]
        public void TestMethod7_7()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 6;
            roundState.BigBlindPosition = 0;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(7, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[6].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
        }
        #endregion

        #region eight

        [TestMethod]
        public void TestMethod8_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(8, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[7].Position);
        }
        [TestMethod]
        public void TestMethod8_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 2;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(8, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
        }
        [TestMethod]
        public void TestMethod8_3()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 2;
            roundState.BigBlindPosition = 3;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(8, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
        }
        [TestMethod]
        public void TestMethod8_4()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 3;
            roundState.BigBlindPosition = 4;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(8, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
        }
        [TestMethod]
        public void TestMethod8_5()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 4;
            roundState.BigBlindPosition = 5;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(8, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
        }
        [TestMethod]
        public void TestMethod8_6()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 5;
            roundState.BigBlindPosition = 6;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(8, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
        }
        [TestMethod]
        public void TestMethod8_7()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 6;
            roundState.BigBlindPosition = 7;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(8, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[6].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
        }
        [TestMethod]
        public void TestMethod8_8()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 7;
            roundState.BigBlindPosition = 0;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(8, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[7].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[6].Position);
        }
        #endregion

        #region nine

        [TestMethod]
        public void TestMethod9_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[8].Position);
        }
        [TestMethod]
        public void TestMethod9_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 2;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
        }
        [TestMethod]
        public void TestMethod9_3()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 2;
            roundState.BigBlindPosition = 3;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
        }
        [TestMethod]
        public void TestMethod9_4()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 3;
            roundState.BigBlindPosition = 4;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
        }
        [TestMethod]
        public void TestMethod9_5()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 4;
            roundState.BigBlindPosition = 5;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
        }
        [TestMethod]
        public void TestMethod9_6()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 5;
            roundState.BigBlindPosition = 6;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
        }
        [TestMethod]
        public void TestMethod9_7()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 6;
            roundState.BigBlindPosition = 7;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[6].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
        }
        [TestMethod]
        public void TestMethod9_8()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 7;
            roundState.BigBlindPosition = 8;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[7].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[6].Position);
        }
        [TestMethod]
        public void TestMethod9_9()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 8;
            roundState.BigBlindPosition = 0;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(9, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[8].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[7].Position);
        }
        #endregion

        #region ten

        [TestMethod]
        public void TestMethod10_1()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 0;
            roundState.BigBlindPosition = 1;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[9].Position);
        }
        [TestMethod]
        public void TestMethod10_2()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 1;
            roundState.BigBlindPosition = 2;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[1].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[9].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
        }
        [TestMethod]
        public void TestMethod10_3()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 2;
            roundState.BigBlindPosition = 3;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[2].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[9].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
        }
        [TestMethod]
        public void TestMethod10_4()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 3;
            roundState.BigBlindPosition = 4;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[3].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[9].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
        }
        [TestMethod]
        public void TestMethod10_5()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 4;
            roundState.BigBlindPosition = 5;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[4].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[9].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
        }
        [TestMethod]
        public void TestMethod10_6()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 5;
            roundState.BigBlindPosition = 6;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[5].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[9].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
        }
        [TestMethod]
        public void TestMethod10_7()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 6;
            roundState.BigBlindPosition = 7;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[6].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[9].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
        }
        [TestMethod]
        public void TestMethod10_8()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 7;
            roundState.BigBlindPosition = 8;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[7].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[8].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[9].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[6].Position);
        }
        [TestMethod]
        public void TestMethod10_9()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 8;
            roundState.BigBlindPosition = 9;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[8].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[9].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[7].Position);
        }
        [TestMethod]
        public void TestMethod10_10()
        {
            // Setup
            List<Seat> seats = new List<Seat>();
            seats.Add(new Seat(1000, PlayerState.Participating, "player1", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player2", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player3", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player4", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player5", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player6", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player7", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player8", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player9", "0"));
            seats.Add(new Seat(1000, PlayerState.Participating, "player10", "0"));
            RoundState roundState = RoundState.GetEmpty();
            roundState.Seats = seats;
            roundState.SmallBlindPosition = 9;
            roundState.BigBlindPosition = 0;

            // Execute
            roundState.UpdatePositions();

            // Test
            Assert.AreEqual(10, roundState.Seats.Count);
            Assert.AreEqual(Position.SmallBlind, roundState.Seats[9].Position);
            Assert.AreEqual(Position.BigBlind, roundState.Seats[0].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[1].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[2].Position);
            Assert.AreEqual(Position.Early, roundState.Seats[3].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[4].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[5].Position);
            Assert.AreEqual(Position.Middle, roundState.Seats[6].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[7].Position);
            Assert.AreEqual(Position.Late, roundState.Seats[8].Position);
        }
        #endregion
    }
}