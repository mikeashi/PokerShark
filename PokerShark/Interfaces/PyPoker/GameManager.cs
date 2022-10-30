using PokerShark.Interfaces.PyPoker.RPC;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Interfaces.PyPoker
{
    internal class GameManager
    {
        private static bool cancelled = false;

        public static void StartGame()
        {

            try
            {
                // Start bot server
                Server server = new Server("localhost");
                Log.Information("Awaiting Poker server...");
            }
            catch (Exception e)
            {
                Log.Fatal("Could not connect to RPC Queue");
                Log.Fatal(e.Message);
            }

            while (!cancelled)
            {
                // keep game running forever
            }

        }
    }
}
