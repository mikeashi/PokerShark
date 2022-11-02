using PokerShark.AI;
using PokerShark.Poker;
using PokerShark.Poker.Deck;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using Action = PokerShark.Poker.Action;

namespace PokerShark.Interfaces.PyPoker.RPC
{
    internal class Consumer : EventingBasicConsumer
    {
        private Bot Bot;

        public Consumer(IModel model, Bot bot) : base(model)
        {
            Received += ConsumeMessage;
            Bot = bot;
        }

        private void ConsumeMessage(object? sender, BasicDeliverEventArgs ea)
        {
            string response = "";
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
                Console.WriteLine("Error");
                Log.Error(Encoding.UTF8.GetString(body));
                Log.Error(e.Message);
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
            if (type != "game.started" && Bot.CurrentGame == null)
                return "Illegal message";

            switch (type)
            {
                case "game.started":
                    HandleGameStartedMessage(payload);
                    return "game information received";
                case "round.started":
                    HandleRoundStartedMessage(payload);
                    return "round information received";
                case "street.started":
                    HandleStreetStartedMessage(payload);
                    return "street information received";
                case "declare.action":
                    return HandleDeclareActionMessage(payload);
                case "game.updated":
                    HandleGameUpdatedMessage(payload);
                    return "game update received";
                case "round.result":
                    HandleRoundResultMessage(payload);
                    return "round result received";
            }
            return "unknown method";
        }

        private void HandleGameStartedMessage(string payload)
        {
            Bot.StartGame(Helper.GetGame(payload));
        }

        private void HandleRoundStartedMessage(string payload)
        {
            var message = Helper.GetRoundStartMessage(payload);
            int roundNumber = message.RoundCount;
            List<Card> pocket = Helper.GetCards(message.HoleCard);
            List<Player> players = Helper.GetPlayers(message.Seats);
            Bot.StartRound(roundNumber, pocket, players);
        }

        private void HandleStreetStartedMessage(string payload)
        {
            var message = Helper.GetRoundStateMessage(payload);

            int dealerPosition = message.RoundState.DealerBtn;
            int smallBlindPosition = message.RoundState.SmallBlindPos;
            int bigBlindPosition = message.RoundState.BigBlindPos;
            Poker.RoundState roundState = Helper.GetRoundState(message.RoundState.Street);
            List<Card> board = Helper.GetCards(message.RoundState.CommunityCard);
            Poker.Pot pot = Helper.GetPot(message.RoundState.Pot);

            Bot.StartStreet(dealerPosition, smallBlindPosition, bigBlindPosition, roundState, board, pot);
        }


        private void HandleGameUpdatedMessage(string payload)
        {
            var message = Helper.GetNewActionMessage(payload);

            Action action = Helper.GetAction(message);
            double updatedPlayerStack = Helper.GetPlayerStack(message, action.PlayerId);
            PlayerState updatedPlayerState = Helper.GetPlayerState(message, action.PlayerId);
            Poker.Pot updatedPot = Helper.GetPot(message.RoundState.Pot);

            Bot.ReceiveAction(action, updatedPlayerStack, updatedPlayerState, updatedPot);
        }

        private void HandleRoundResultMessage(string payload)
        {
            Bot.EndRound(Helper.GetWinners(payload), Helper.GetResultPlayers(payload));
        }


        private string HandleDeclareActionMessage(string payload)
        {
            var action = Bot.DeclareAction(Helper.GetValidActions(payload));
            return action.ToString();
        }
    }
}
