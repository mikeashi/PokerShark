using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluidHTN.Contexts;
using FluidHTN.Debug;
using FluidHTN.Factory;
using PokerShark.Core.HTN.Utility;
using PokerShark.Core.Poker;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;

namespace PokerShark.Core.HTN.Context
{
    public class PokerContext : PokerBaseContext
    {
        public PokerContext()
        {

        }
        #region state methods 
        
        public void UpdatePlayerModel(String name, PyAction action)
        {
            var models = GetPlayersModels();
            var playerId = action.PlayerId;
            var playerModel = models.Find(m => m.Id == playerId);

            // creates player model if it doesn't exist
            if (playerModel == null)
            {
                playerModel = new PlayerModel(name, action.PlayerId);
                models.Add(playerModel);
            }
            playerModel.UpdateHistory(action);
            SetPlayersModels(models);
        }


        public void ResetBoardCards()
        {
            SetState((int)State.BoardCards, new List<Card>());
        }

        public void ResetDecision()
        {
            SetDecision((0, 0, 0));
        }

        public void AddDeadCards(List<Card> deadCards)
        {
            List<Card> cards = (List<Card>)GetState((int)State.DeadCards);
            cards.AddRange(deadCards);
            SetState((int)State.DeadCards, cards);
        }

        public void StoreRoundHistory()
        {
            List<PyAction> history = (List<PyAction>)GetState((int)State.ActionHistory);
            var currentRound = GetCurrentRound();
            if (currentRound.ActionHistory.Count > 0)
            {
                history.AddRange(currentRound.ActionHistory);
                SetState((int)State.ActionHistory, history);
            }
        }

        #endregion

        #region State setters

        public void SetBoardCards(List<Card> boardCards)
        {
            SetState((int)State.BoardCards, boardCards);
        }

        public void SetDecision((float Fold, float Call, float Raise) decision)
        {
            SetState((int)State.Decision, decision);
        }

        public void SetRoundCount(int roundCount)
        {
            SetState((int)State.RoundCount, roundCount);
        }

        public void SetPocketCards(List<Card> pocketCards)
        {
            SetState((int)State.PocketCards, pocketCards);
        }

        public void SetSeats(List<Seat> seats)
        {
            SetState((int)State.Seats, seats);
        }

        public void SetCurrentRound(RoundState state)
        {
            SetState((int)State.CurrentRound, state);
            SetRoundCount(state.RoundCount);
            SetSeats(state.Seats);
            SetBoardCards(state.Board);
        }

        public void SetValidActions(List<PyAction> validActions)
        {
            SetState((int)State.ValidActions, validActions);
        }

        public void SetLastRoundWinners(List<Seat> winners)
        {
            SetState((int)State.LastRoundWinners, winners);
        }

        public void SetPlayersModels(List<PlayerModel> models)
        {
            SetState((int)State.PlayersModels, models);
        }

        #endregion

        #region State getters

        public List<PlayerModel> GetPlayersModels()
        {
            return (List<PlayerModel>)GetState((int)State.PlayersModels);
        }
        
        public Position GetPosition()
        {
            var position = Position.Early;
            List<Seat> seats = (List<Seat>)GetState((int)State.Seats);
            foreach (Seat seat in seats)
            {
                if (seat.Name == "PokerShark")
                {
                    return seat.Position;
                }
            }

            return position;
        }

        public StreetState GetStage()
        {
            return ((RoundState)GetState((int)State.CurrentRound)).StreetState;
        }

        public RoundState GetCurrentRound()
        {
            return ((RoundState)GetState((int)State.CurrentRound));
        }


        public (float Fold, float Call, float Raise) GetDecision()
        {
            return ((float Fold, float Call, float Raise))GetState((int)State.Decision);
        }

        public StaticUtilityFunction GetAttitude()
        {
            return new RiskNeutral();
        }

        #endregion
    }
}
