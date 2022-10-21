using PokerShark.Core.Helpers;
using PokerShark.Core.Poker.Deck;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PokerShark.Core.PyPoker
{
    

    public class RoundState
    {
        public int DealerPosition { get; private set; }
        public int SmallBlindPosition { get; set; }
        public int BigBlindPosition { get; set; }
        public StreetState StreetState { get; private set; }
        public List<Seat> Seats { get; set; }
        public int NextPlayer { get; private set; }
        public int RoundCount { get; private set; }
        public List<Card> Board { get; private set; }
        public Pot Pot { get; private set; }
        
        public List<PyAction> ActionHistory { get; private set; }

        public RoundState(int dealerPosition, int smallBlindPosition, int bigBlindPosition, StreetState streetState, List<Seat> seats, int nextPlayer, int roundCount, List<Card> board, Pot pot, List<PyAction> actionHistory)
        {
            DealerPosition = dealerPosition;
            SmallBlindPosition = smallBlindPosition;
            BigBlindPosition = bigBlindPosition;
            StreetState = streetState;
            Seats = seats;
            NextPlayer = nextPlayer;
            RoundCount = roundCount;
            Board = board;
            Pot = pot;
            ActionHistory = actionHistory;
            if (Seats.Count > 0) UpdatePositions();
        }

        public static RoundState GetEmpty()
        {
            return new RoundState(0,0,0,StreetState.Preflop, new List<Seat>(),0,0, new List<Card>(),new Pot(), new List<PyAction>());
        }

        public void UpdatePositions()
        {
            Seats[SmallBlindPosition].IsSmallBlind = true;
            Seats[BigBlindPosition].IsBigBlind = true;
            
            Seats[SmallBlindPosition].Position = Position.SmallBlind;
            Seats[BigBlindPosition].Position = Position.BigBlind;

            switch (Seats.Count)
            {
                case 3:
                    Seats[(BigBlindPosition + 1) % 3].Position = Position.Early;
                    break;
                case 4:
                    Seats[(BigBlindPosition + 1) % 4].Position = Position.Early;
                    Seats[(BigBlindPosition + 2) % 4].Position = Position.Middle ;
                    break;
                case 5:
                    Seats[(BigBlindPosition + 1) % 5].Position = Position.Early;
                    Seats[(BigBlindPosition + 2) % 5].Position = Position.Middle;
                    Seats[(BigBlindPosition + 3) % 5].Position = Position.Late;
                    break;
                case 6:
                    Seats[(BigBlindPosition + 1) % 6].Position = Position.Early;
                    Seats[(BigBlindPosition + 2) % 6].Position = Position.Early;
                    Seats[(BigBlindPosition + 3) % 6].Position = Position.Middle;
                    Seats[(BigBlindPosition + 4) % 6].Position = Position.Late;
                    break;
                case 7:
                    Seats[(BigBlindPosition + 1) % 7].Position = Position.Early;
                    Seats[(BigBlindPosition + 2) % 7].Position = Position.Early;
                    Seats[(BigBlindPosition + 3) % 7].Position = Position.Middle;
                    Seats[(BigBlindPosition + 4) % 7].Position = Position.Late;
                    Seats[(BigBlindPosition + 5) % 7].Position = Position.Late;
                    break;
                case 8:
                    Seats[(BigBlindPosition + 1) % 8].Position = Position.Early;
                    Seats[(BigBlindPosition + 2) % 8].Position = Position.Early;
                    Seats[(BigBlindPosition + 3) % 8].Position = Position.Middle;
                    Seats[(BigBlindPosition + 4) % 8].Position = Position.Middle;
                    Seats[(BigBlindPosition + 5) % 8].Position = Position.Late;
                    Seats[(BigBlindPosition + 6) % 8].Position = Position.Late;
                    break;
                case 9:
                    Seats[(BigBlindPosition + 1) % 9].Position = Position.Early;
                    Seats[(BigBlindPosition + 2) % 9].Position = Position.Early;
                    Seats[(BigBlindPosition + 3) % 9].Position = Position.Early;
                    Seats[(BigBlindPosition + 4) % 9].Position = Position.Middle;
                    Seats[(BigBlindPosition + 5) % 9].Position = Position.Middle;
                    Seats[(BigBlindPosition + 6) % 9].Position = Position.Late;
                    Seats[(BigBlindPosition + 7) % 9].Position = Position.Late;
                    break;
                case 10:
                    Seats[(BigBlindPosition + 1) % 10].Position = Position.Early;
                    Seats[(BigBlindPosition + 2) % 10].Position = Position.Early;
                    Seats[(BigBlindPosition + 3) % 10].Position = Position.Early;
                    Seats[(BigBlindPosition + 4) % 10].Position = Position.Middle;
                    Seats[(BigBlindPosition + 5) % 10].Position = Position.Middle;
                    Seats[(BigBlindPosition + 6) % 10].Position = Position.Middle;
                    Seats[(BigBlindPosition + 7) % 10].Position = Position.Late;
                    Seats[(BigBlindPosition + 8) % 10].Position = Position.Late;
                    break;
            }
        }

        public void Verbose(int indent = 0)
        {
            Log.Verbose("Street Info:");
            Log.Verbose(StringHelper.Indent(indent + 3) + "Dealer position: " + DealerPosition);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Small blind position: " + SmallBlindPosition);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Big blind position: " + BigBlindPosition);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Street State: " + StreetState);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Next player: " + NextPlayer);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Round count: " + RoundCount);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Seats: ");
            foreach (var seat in Seats)
            {
                seat.Verbose(indent + 6);
                Log.Verbose(StringHelper.Indent(indent + 6) + "--------------------------------");
            }
        }

        public void Information(int indent = 0)
        {
            Log.Information("Street Info:");
            Log.Information(StringHelper.Indent(indent + 3) + "State: " + StreetState);
            Log.Information(StringHelper.Indent(indent + 3) + "Round count: " + RoundCount);
            Log.Information(StringHelper.Indent(indent + 3) + "Players: ");
            int i = 0;
            foreach (var seat in Seats)
            {
                seat.Information(indent + 6);
                if (i == DealerPosition)
                    Log.Information(StringHelper.Indent(indent + 6) + "Dealer");
                if (i == SmallBlindPosition)
                    Log.Information(StringHelper.Indent(indent + 6) + "Small blind");
                if (i == BigBlindPosition)
                    Log.Information(StringHelper.Indent(indent + 6) + "Big blind");
                Log.Information(StringHelper.Indent(indent + 6) + "--------------------------------");
                i++;
            }
        }
    }
}
