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
    public enum Position
    {
        Early,
        Middle,
        Late,
        SmallBlind,
        BigBlind,
    }

    public enum PlayerState
    {
        Participating,
        Allin,
        Folded,
    }

    public class Seat
    {
        public double Stack { get; private set; }
        public PlayerState State { get; private set; }
        public string Name { get; private set; }
        public string Id { get; private set; }
        public Position Position { get; set; }

        public bool IsSmallBlind { get;  set; }
        public bool IsBigBlind { get;  set; }


        public Seat(double stack, PlayerState state, string name, string id)
        {
            Stack = stack;
            State = state;
            Name = name;
            Id = id;
        }

        public void Verbose(int indent = 0)
        {
            Log.Verbose(StringHelper.Indent(indent) + "Player name: " + Name);
            Log.Verbose(StringHelper.Indent(indent) + "Player id: " + Id);
            Log.Verbose(StringHelper.Indent(indent) + "Player State: " + State);
            Log.Verbose(StringHelper.Indent(indent) + "Player Stack: " + Stack);
        }

        public void Information(int indent = 0)
        {
            Log.Information(StringHelper.Indent(indent) + "Name: " + Name);
            Log.Information(StringHelper.Indent(indent) + "State: " + State);
            Log.Information(StringHelper.Indent(indent) + "Stack: " + Stack);
        }
    }
}
