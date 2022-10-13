using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.HTN.Context
{
    public enum State : int
    {
        GameInfo,
        BoardCards,
        RoundCount,
        PocketCards,
        Seats,
        CurrentRound,
        ValidActions,
        DeadCards,
        LastRoundWinners,
        Decision,
        ActionHistory,
        PlayersModels,
    }
}
