using PokerShark.Interfaces.PyPoker;
using Serilog;

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
