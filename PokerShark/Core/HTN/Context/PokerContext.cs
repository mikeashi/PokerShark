using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluidHTN.Contexts;
using FluidHTN.Debug;
using FluidHTN.Factory;
using PokerShark.Core.Helpers;
using PokerShark.Core.HTN.Utility;
using PokerShark.Core.Poker;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.PyPoker;

namespace PokerShark.Core.HTN.Context
{
    public class PokerContext : PokerBaseContext
    {
        public String Name = "PokerShark";
        public double ehs = -1;

        public PokerContext()
        {

        }
        #region state methods 

        public List<VariableCost> GetOdds(params(double cost, double probability)[] tuples)
        {
            var odds = new List<VariableCost>();
            foreach(var t in tuples)
            {
                odds.Add(new VariableCost(t.cost, t.probability));

            }
            return odds;
        }

        public double GetPotAmount()
        {
            var round = GetCurrentRound();
            var id = round.Seats.First(s => s.Name == Name).Id;

            return GetCurrentRound().Pot.Amount(id);
        }

        public List<VariableCost> BluffOdds(double amount)
        {
            // calculate Break even precentage.
            double be = (double)amount / GetPotAmount() + amount;

            // find fold percentage for all participating players.
            //var foldPrecentage = GetPlayersModels().Average(m => m.FoldPercentage);
            var foldPrecentage = GetPlayersModels().Max(m => m.FoldPercentage);

            if (be >= foldPrecentage)
            {
                return GetOdds((GetPotAmount() + amount, 1));
            }

            return GetOdds((-1*(GetPotAmount() + amount), 1));
        }

        public List<VariableCost> FoldOdds()
        {
            var round = GetCurrentRound();
            double factor = 1;
            int count = 0;
            // fold odds serve as a threshhold EV for calling or raising.
            // the threshhold is choosen dynamically based on the profile of the participating players.
            var players = round.Seats;
            foreach (var model in GetPlayersModels())
            {
                var p = players.First(s => s.Id == model.Id);
                if (p !=null && p.State != PlayerState.Folded)
                {
                   count++;
                   switch (model.PlayingStyle)
                    {
                        case PlayingStyle.LooseAggressive:
                            factor += 1;
                        break;
                        case PlayingStyle.LoosePassive:
                            factor -= 0.5;
                        break;
                        case PlayingStyle.TightAggressive:
                            factor += 4;
                        break;
                        case PlayingStyle.TightPassive:
                            factor += 3;
                        break;
                    }
                }
            }


            return GetOdds(
                (((double)factor/count) * 2, 1)
                );
        }

        public List<VariableCost> RaiseOdds(double amount)
        {
            // get ehs
            var ehs = GetEHS();

            return GetOdds((GetPotAmount() + amount , ehs), (-1 * amount, 1 - ehs));
        }

        public List<VariableCost> CallOdds()
        {
            // get ehs
            var ehs = GetEHS(); ;
            // get call amount
            double amount = GetCallAmount();
            return GetOdds((GetPotAmount() + amount, ehs), (-1 * amount, 1 - ehs));
        }

        public double GetCallAmount()
        {
            // get call amount
            double amount = 0;
            foreach (var action in GetValidActions())
            {
                if (action is CallAction)
                {
                    amount = action.Amount;
                    break;
                }
            }

            return amount;
        }

        public double GetMinRaiseAmount()
        {
            // get raise amount
            double amount = 0;
            foreach (var action in GetValidActions())
            {
                if (action is RaiseAction)
                {
                    amount = action.Amount;
                    break;
                }
            }
            return amount;
        }

        public double GetMaxRaiseAmount()
        {
            // get raise amount
            double amount = 0;
            foreach (var action in GetValidActions())
            {
                if (action is RaiseAction)
                {
                    amount = action.MaxAmount;
                    break;
                }
            }
            return amount;
        }

        public double GetEHS()
        {
            if (ehs == -1)
            {
                var round = GetCurrentRound();
                var odds = new List<VariableCost>();
                // get weights
                var weights = new List<double[]>();
                var players = round.Seats;
                foreach (var model in GetPlayersModels())
                {
                    if(players.First(s => s.Id == model.Id).State == PlayerState.Participating)
                    {
                        weights.Add(model.weightTable.Table);
                    }
                }
                // calculate ehs
                ehs = Oracle.EHS(GetPocket(), round.Board, weights);
            }
            return ehs;
        }

        public void SetRaiseAmount(params (int Factor, float Weight)[] amounts)
        {
            Dictionary<int, float> WeightedFactors = new Dictionary<int, float>();
            foreach (var amount in amounts)
            {
                WeightedFactors.Add(amount.Factor, amount.Weight);
            }

            var factor = WeightedFactors.RandomElementByWeight(e => e.Value).Key;

            RaiseDecisionAmount = factor * GetGameInfo().BigBlind;
        }
        
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
            ehs = -1;
            RaiseDecisionAmount = GetMinRaiseAmount();
            SetDecision((0, 0, 0));
            Done = false;
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
        public List<PyAction> GetValidActions()
        {
            return (List<PyAction>)GetState((int)State.ValidActions);
        }
        
        public GameInfo GetGameInfo()
        {
            return (GameInfo)GetState((int)State.GameInfo);
        }
        
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
                if (seat.Name == Name)
                {
                    return seat.Position;
                }
            }

            return position;
        }

        public Double GetPaid()
        {
            var round = GetCurrentRound();
            var history = round.ActionHistory;
            var game = GetGameInfo();
            
            // get player id
            var id = round.Seats.First(s => s.Name == Name).Id;

            // check is player small blind
            var smallBlind = round.Seats.First(s => s.IsSmallBlind);
            var isSmallBlind = smallBlind.Name == Name;

            // check is player big blind
            var bigBlind = round.Seats.First(s => s.IsBigBlind);
            var isBigBlind = bigBlind.Name == Name;

            double paid = game.Ante;

            if (isSmallBlind)
            {
                paid += game.SmallBlind;
            }

            if (isBigBlind)
            {
                paid += game.BigBlind;
            }


            return paid + history.Where(a => a.PlayerId == id).Sum(a => a.Paid);
        }

        public StreetState GetStage()
        {
            return ((RoundState)GetState((int)State.CurrentRound)).StreetState;
        }

        public RoundState GetCurrentRound()
        {
            return ((RoundState)GetState((int)State.CurrentRound));
        }

        public List<Card> GetPocket()
        {
            return ((List<Card>)GetState((int)State.PocketCards));  
        }

        public (float Fold, float Call, float Raise) GetDecision()
        {
            return ((float Fold, float Call, float Raise))GetState((int)State.Decision);
        }

        public StaticUtilityFunction GetAttitude()
        {
            var game = GetGameInfo();
            var round = GetCurrentRound();
            // get current stack 
            var initial = game.Seats.First(s => s.Name == Name).Stack;
            var current = round.Seats.First(s => s.Name == Name).Stack;

            if (current >= 2 * initial)
            {
                return new RiskSeeking();
            }

            if (current < initial / 2)
            {
                return new RiskAverse();
            }

            return new RiskNeutral();
        }

        #endregion
    }
}
