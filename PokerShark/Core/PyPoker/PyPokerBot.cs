using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PokerShark.Core.Poker.Deck;

namespace PokerShark.Core.PyPoker
{

    public abstract class PyPokerBot
    {
        public abstract void GameStarted(GameInfo gameInfo);
        public abstract void RoundStarted(int roundCount, List<Card> pocketCards, List<Seat> Seats);
        public abstract void StreetStarted(RoundState state);
        public abstract PyAction DeclareAction(List<PyAction> validActions, RoundState state, List<Card> pocketCards);
        public abstract void GameUpdated(RoundState state, PyAction action);
        public abstract void RoundResult(RoundState state, List<Card> deadCards, List<Seat> winners);
        public abstract bool InGame();

    }
}
