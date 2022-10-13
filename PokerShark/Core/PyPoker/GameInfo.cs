using Newtonsoft.Json.Linq;
using PokerShark.Core.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.PyPoker
{
    public class GameInfo
    {
        public double PlayersNumber { get; private set; }
        public double InitialStack { get; private set; }
        public double MaxRound { get; private set; }
        public double SmallBlind { get; private set; }
        public double BigBlind { get; private set; }
        public double Ante { get; private set; }
        public List<Seat> Seats { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playersNumber"></param>
        /// <param name="initialStack"></param>
        /// <param name="maxRound"></param>
        /// <param name="smallBlind"></param>
        /// <param name="bigBlind"></param>
        /// <param name="ante"></param>
        /// <param name="seats"></param>
        public GameInfo(double playersNumber, double initialStack, double maxRound, double smallBlind, double bigBlind, double ante, List<Seat> seats)
        {
            PlayersNumber = playersNumber;
            InitialStack = initialStack;
            MaxRound = maxRound;
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            Ante = ante;
            Seats = seats;
        }

        public void Verbose(int indent = 0)
        {
            Log.Verbose("Game Info:");
            Log.Verbose(StringHelper.Indent(indent + 3) + "Number of players: " + PlayersNumber);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Initial stack: " + InitialStack);
            Log.Verbose(StringHelper.Indent(indent + 3) + "MaxRound: " + MaxRound);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Small blind amount: " + SmallBlind);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Big blind amount: " + BigBlind);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Ante amount: " + Ante);
            Log.Verbose(StringHelper.Indent(indent + 3) + "Seats: ");
            foreach (var seat in Seats)
            {
                seat.Verbose(indent + 6);
                Log.Verbose(StringHelper.Indent(indent + 6) + "--------------------------------");
            }
        }

        public void Information(int indent = 0)
        {
            Log.Information(StringHelper.Indent(indent + 3) + "MaxRound: " + MaxRound);
            Log.Information(StringHelper.Indent(indent + 3) + "Small blind amount: " + SmallBlind);
            Log.Information(StringHelper.Indent(indent + 3) + "Ante amount: " + Ante);
            Log.Information(StringHelper.Indent(indent + 3) + "Players: ");
            foreach (var seat in Seats)
            {
                seat.Information(indent + 6);
                Log.Information(StringHelper.Indent(indent + 6) + "--------------------------------");
            }
        }

    }
}
