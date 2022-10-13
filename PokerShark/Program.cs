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


namespace PokerShark
{
    internal class Program
    {
        static void Main(string[] args)
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
