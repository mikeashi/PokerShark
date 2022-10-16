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
using PokerShark.Core.Poker;
using PokerShark.Core.Poker.Deck;
using HoldemHand;
using PokerShark.Core.PyPoker;
using PokerShark.Core.Helpers;

namespace PokerShark
{
    internal class Program
    {
        static void Main()
        {
            // define pocket cards
            var pocket = new List<Card>() { new Card(StateCard.AceOfDiamonds), new Card(StateCard.QueenOfClubs) };
            // define board cards
            var board = new List<Card>() { new Card(StateCard.JackOfHearts), new Card(StateCard.FourOfClubs), new Card(StateCard.ThreeOfHearts) };
            
            WeightTable weightTable = new WeightTable();
            weightTable.UpdateTable(Core.PyPoker.StreetState.Preflop, 100, new FoldAction());
            List<double[]> weights = new List<double[]>() { weightTable.Table};
            Console.WriteLine(Oracle.RawHandStrength(pocket, board, 2));
            Console.WriteLine(Oracle.WeightedHandStrength(pocket, board, weights));
            Console.WriteLine(Oracle.HandPotential(pocket, board));
            Console.WriteLine(Oracle.WeightedHandPotential(pocket, board, weights));
        }

        static void Main_()
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
            Console.ReadLine();
        }
    }
}
