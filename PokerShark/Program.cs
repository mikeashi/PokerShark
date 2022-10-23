using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using PokerShark.Core.HTN;
using PokerShark.Core.Helpers;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PokerShark.Poker;
using PokerShark.Poker.Deck;
using RoundState = PokerShark.Poker.RoundState;
using PokerShark.Interfaces.PyPoker;
using PokerShark.Interfaces.PyPoker.RPC;
using PokerShark.Tests;

namespace PokerShark
{
    internal class Program
    {
        static void Main()
        {
            // initialize logger
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}")
               .CreateLogger();

            // initialize windows
            Windows.WindowsManager.Init();

            // start Pypoker game
            GameManager.StartGame();
        }
    }
}
