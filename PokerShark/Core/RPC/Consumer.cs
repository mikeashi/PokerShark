using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PokerShark.Core.Helpers;
using System.Net.Sockets;
using PokerShark.Core.PyPoker;
using PokerShark.Core.Poker.Deck;
using PokerShark.Core.Poker;
using MathNet.Numerics.Statistics;
using PokerShark.Core.HTN.Context;
using System.Globalization;

namespace PokerShark.Core.RPC
{
    public class Consumer : EventingBasicConsumer
    {
        private PyPokerBot Bot;

        public Consumer(IModel model, PyPokerBot bot) : base(model)
        {
            // register message consumer
            Received += ConsumeMessage;
            Bot = bot;
        }

        private void ConsumeMessage(object sender, BasicDeliverEventArgs ea)
        {
            string response = null;
            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = Model.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                // get response
                var payload = Encoding.UTF8.GetString(body);
                response = GetResponse(props.Type, payload);
            }
            catch (Exception e)
            {
                // log error
                Log.Error(Encoding.UTF8.GetString(body));
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                response = "";
            }
            finally
            {
                // send response
                var responseBytes = Encoding.UTF8.GetBytes(response);
                Model.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                  basicProperties: replyProps, body: responseBytes);
                Model.BasicAck(deliveryTag: ea.DeliveryTag,
                  multiple: false);
            }
        }

        private string GetResponse(string type, string payload)
        {
            if (type != "game.started" && !Bot.InGame())
                return "Illegal message";

            // Log.Information("Received method call :" + type);
            switch (type)
            {
                // game started message contains game info.
                case "game.started":
                    HandleGameStartedMessage(payload);
                    return "game information received";

                // round started message contains round info.
                case "round.started":
                    HandleRoundStartedMessage(payload);
                    return "round information received";

                // street started message
                case "street.started":
                    HandleStreetStartedMessage(payload);
                    return "street information received";

                // declare action message
                case "declare.action":
                    return HandleDeclareActionMessage(payload);

                // game updated message
                case "game.updated":
                    HandleGameUpdatedMessage(payload);
                    return "game update received";

                // round result
                case "round.result":
                    HandleRoundResultMessage(payload);
                    return "";
            }
            return "unknown method";
        }
        private void HandleGameStartedMessage(string payload)
        {
            Bot.GameStarted(PyPokerHelper.GetGameInfo(JObject.Parse(payload)));
        }

        private void HandleRoundResultMessage(string payload)
        {
            var result = JObject.Parse(payload);
            var round = PyPokerHelper.getRoundState(result["round_state"]);
            var deadCards = new List<Card>();

            // add pocket cards to dead cards
            if (result["hand_info"].HasValues)
            {
                foreach(var hand in result["hand_info"])
                {
                    deadCards.Add(new Card((string)hand["hand"]["hand"]["formated_cards"][0]));
                    deadCards.Add(new Card((string)hand["hand"]["hand"]["formated_cards"][1]));
                }
            }
            // add board cards to dead cards
            foreach (var card in round.Board)
            {
                deadCards.Add(card);
            }

            // get winners 
            var winners = PyPokerHelper.getSeats(result["winners"]);
            Bot.RoundResult(round, deadCards, winners);
        }

        private void HandleGameUpdatedMessage(string payload)
        {
            var result = JObject.Parse(payload);
            var round = PyPokerHelper.getRoundState(result["round_state"]);
            var action = PyPokerHelper.getAction(result["new_action"], round.StreetState);
            Bot.GameUpdated(round, action);
        }

        private String HandleDeclareActionMessage(string payload)
        {
            var result = JObject.Parse(payload);
            var pocket = PyPokerHelper.getPocketCards(result["hole_card"]);
            var round = PyPokerHelper.getRoundState(result["round_state"]);
            var actions = PyPokerHelper.getValidActions(result["valid_actions"]);
            
            // get bot action
            var action = Bot.DeclareAction(actions, round, pocket);

            // if preflop store bot action and round in csv file
            if (round.StreetState == StreetState.Preflop)
            {
                var csv = new StringBuilder();
                var newLine = string.Format("{0},{1}", action.Name,String.Join(" - ", pocket));
                csv.AppendLine(newLine);
                File.AppendAllText("preflop.csv", csv.ToString());
            }else
            {
                var csv = new StringBuilder();
                var newLine = string.Format("{0},{1}, {2}", action.Name, String.Join(" - ", pocket), String.Join(" - ", round.Board));
                csv.AppendLine(newLine);
                File.AppendAllText("postflop.csv", csv.ToString());
            }

            LogBotAction(action);
            return action.ToString();
        }

        private void LogBotAction(PyAction action)
        {
            var bot = (Bot)Bot;
            var context = bot.Context;
            var round = context.GetCurrentRound();
            var csv = new StringBuilder();
            // round number
            csv.Append(round.RoundCount + " ,");
            // stage 
            csv.Append(round.StreetState + " ,");
            // pocket
            csv.Append(String.Join(" - ", context.GetPocket()) + " ,");
            // board
            csv.Append(String.Join(" - ", round.Board) + " ,");
            // pot
            csv.Append(String.Join(" - ", context.GetPotAmount()) + " ,");
            
            // fold odds + EV, call odds + EV , raise odds + EV, callbluffOdds, raisebluffOdds
            if (round.StreetState != StreetState.Preflop)
            {
                var raiseOdds = context.RaiseOdds(context.GetMinRaiseAmount());
                var callOdds = context.CallOdds();
                var foldOdds = context.FoldOdds();
                var callbluffOdds = context.BluffOdds(context.GetCallAmount()); 
                var raisebluffOdds = context.BluffOdds(context.GetMinRaiseAmount());

                csv.Append(String.Join(" |", foldOdds) + " E =" + Math.Round(foldOdds.Sum(vc => (context).GetAttitude().CalculateUtility(vc.Cost) * vc.Probability), 4).ToString(CultureInfo.InvariantCulture) + " ,");
                csv.Append(String.Join(" |", callOdds) + " E =" + Math.Round(callOdds.Sum(vc => (context).GetAttitude().CalculateUtility(vc.Cost) * vc.Probability), 4).ToString(CultureInfo.InvariantCulture) + " ,");
                csv.Append(String.Join(" |", raiseOdds) + " E =" + Math.Round(raiseOdds.Sum(vc => (context).GetAttitude().CalculateUtility(vc.Cost) * vc.Probability), 4).ToString(CultureInfo.InvariantCulture) + " ,");
                csv.Append(String.Join(" |", callbluffOdds) +" ,");
                csv.Append(String.Join(" |", raisebluffOdds) + " ,");
            }else
            {
                csv.Append(" ,");
                csv.Append(" ,");
                csv.Append(" ,");
                csv.Append(" ,");
                csv.Append(" ,");
            }

            // decision, action.

            var decision = context.GetDecision();
            csv.Append("Fold: " + decision.Fold.ToString(CultureInfo.InvariantCulture) + " | Call: "+ decision.Call.ToString(CultureInfo.InvariantCulture) + " | Raise: "+ decision.Raise.ToString(CultureInfo.InvariantCulture) + " ,");
            csv.Append(action.Name + " ,");

            // store row
            csv.AppendLine();
            File.AppendAllText("log.csv", csv.ToString());
        }

        private void HandleStreetStartedMessage(string payload)
        {
            var result = JObject.Parse(payload);
            Bot.StreetStarted(PyPokerHelper.getRoundState(result["round_state"]));
        }

        private void HandleRoundStartedMessage(string payload)
        {
            var result = JObject.Parse(payload);
            var roundCount = (int)result["round_count"];
            var pocket = PyPokerHelper.getPocketCards(result["hole_card"]);
            var seats = PyPokerHelper.getSeats(result["seats"]);
            Bot.RoundStarted(roundCount, pocket, seats);
        }
    }
}
