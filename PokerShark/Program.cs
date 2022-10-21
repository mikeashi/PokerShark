using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using PokerShark.Core.RPC;
using PokerShark.Core.HTN;
using PokerShark.Core.Helpers;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PokerShark.Poker;
using PokerShark.Poker.Deck;
using RoundState = PokerShark.Poker.RoundState;

namespace PokerShark
{
    internal class Program
    {
        static void Main()
        {
            // initialize logger
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .CreateLogger();
            
            List<Player> players = new List<Player>();
            players.Add(new Player("Player1", "1"));
            players.Add(new Player("Player2", "2"));
            Game game = new Game(2, 100, 10, 10, 20, 0, players);

            PokerShark.AI.Bot bot = new PokerShark.AI.Bot();
            bot.StartGame(game);

            List<Card> pocket = new List<Card>();
            pocket.Add(new Card(StateCard.TwoOfSpades));
            pocket.Add(new Card(StateCard.FiveOfSpades));

            List<Card> board = new List<Card>();
            board.Add(new Card(StateCard.JackOfSpades));
            board.Add(new Card(StateCard.KingOfHearts));
            board.Add(new Card(StateCard.TenOfHearts));

            bot.StartRound(1, pocket, players);
            bot.StartStreet(0,0,1, RoundState.Preflop, board, new Poker.Pot(50));
            bot.ReceiveAction(new Poker.Action("1", "Player1", RoundState.Preflop), 50, Poker.PlayerState.Folded, new Poker.Pot(100));
            bot.ReceiveAction(new Poker.Action("2", "Player2",20, RoundState.Preflop), 50, Poker.PlayerState.Participating, new Poker.Pot(120));
            bot.EndRound(new List<Player>() { new Player("Player2", "2") });

            // encode game to json
            string json = JsonConvert.SerializeObject(game, Formatting.Indented);
            Console.WriteLine(json);

        }


        static void Main___()
        {
            // initialize logger
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .CreateLogger();

            // log message
            Log.Information("you can exit at any time by hitting [enter]");

            
            try
            {
                Server server = new Server("localhost");
                Log.Information("Awaiting Poker server...");
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
            }
            //
            var exit = false;
            while (!exit)
            {
                //var input = Console.ReadLine();
               // if (input == "x")
               // {
               //     exit = true;
               // }
            }
        }
    }
}
