using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using PokerShark.AI;
using PokerShark.AI.HTN.Utility;
using PokerShark.Poker;
using Action = PokerShark.Poker.Action;

namespace PokerShark.Windows
{
    public class WindowsManager
    {
        private IConsole MainWindow;
        private IConsole LogsWindow;
        private IConsole ProfileWindow;
        private IConsole StateWindow;
        private IConsole BotStateWindow;
        private HighSpeedWriter Writer;
        private static WindowsManager? Instance;

        private WindowsManager()
        {
            //Writer = new HighSpeedWriter();
            //MainWindow = new Window(Writer);
            //MainWindow.CursorVisible = false;
            //var columns = MainWindow.SplitColumns(new Split(0), new Split(50));
            //var leftRows = columns[0].SplitRows(new Split(0, "PokerShark"), new Split(4, "PokerShark States"));
            ////var leftRows = columns[0].SplitRows(new Split(0, "PokerShark"));
            //LogsWindow = leftRows[0];
            //BotStateWindow = leftRows[1];
            //ProfileWindow = columns[1].SplitTop("Players Models");
            //StateWindow = columns[1].SplitBottom("Game States");

            Writer = new HighSpeedWriter();
            MainWindow = new Window(Writer);
            MainWindow.CursorVisible = false;
            var rows = MainWindow.SplitRows(new Split(0), new Split(4, "PokerShark States"));
            BotStateWindow = rows[1];
            var columns = rows[0].SplitColumns(new Split(0, "GameLog"), new Split(60));
            LogsWindow = columns[0];
            var side = columns[1].SplitRows(new Split(0, "Players Models"), new Split(4, "Result"));
            ProfileWindow = side[0];
            StateWindow = side[1];
            //StateWindow = columns[1].SplitBottom("Game States");
        }

        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new WindowsManager();
                Console.SetOut(new LogsTextWriter());
                Instance.InitPlayerModel();
                Instance.InitState();
            }
        }

        public static IConsole? GetLogsWindow()
        {
            return Instance?.LogsWindow;
        }

        public static HighSpeedWriter? GetWriter()
        {
            return Instance?.Writer;
        }

        public static void Flush()
        {
            Instance?.Writer.Flush();
        }

        public static void UpdateProfiles(List<PlayerModel> models)
        {
            Instance?.ProfileWindow.Clear();
            Instance?.ProfileWindow.Write(FormatPlayerModels(models));

            Flush();
        }

        public static void UpdateResults(List<Result> results)
        {
            Instance?.StateWindow.Clear();
            Instance?.StateWindow.Write(FormatResults(results));

            Flush();
        }


        public static void UpdateBotStates(StaticUtilityFunction attitude, PlayerModel? botModel)
        {
            if (botModel == null)
                return;
            Instance?.BotStateWindow.Clear();
            Instance?.BotStateWindow.WriteLine(FormatPlayerModel(botModel).Replace("\n", ""));

            Flush();
        }


        private void InitPlayerModel()
        {
            Instance?.ProfileWindow.Clear();
            Instance?.ProfileWindow.WriteLine("Game did not start yet.");

            Flush();
        }

        private void InitState()
        {
            Instance?.StateWindow.Clear();
            Instance?.StateWindow.WriteLine("Game did not start yet.");

            Instance?.BotStateWindow.Clear();
            Instance?.BotStateWindow.WriteLine("Game did not start yet.");
            
            Flush();
        }


        private static string FormatPlayerModels(List<PlayerModel> models)
        {
            var builder = new StringBuilder();

            foreach(var model in models)
            {
                builder.AppendLine(
                    String.Format("{0}: {1} \n {2}", model.Player.Name, model.PlayingStyle, FormatPlayerModel(model))
                    );

            }

            return builder.ToString();
        }

        private static string FormatPlayerModel(PlayerModel model)
        {
            var builder = new StringBuilder();

            builder.Append(
                String.Format("VPIP = {0}, PFR = {1}, PFF = {2}, WTSD = {3},\n PSDF = {4}, WSD = {5}, WWSF = {6}",
                   FormatNumber(model.VPIP),
                   FormatNumber(model.PFR),
                   FormatNumber(model.PFF),
                   FormatNumber(model.WTSD),
                   FormatNumber(model.PSDF),
                   FormatNumber(model.WSD),
                   FormatNumber(model.WWSF)
                   ));

            return builder.ToString();
        }





        private static string FormatResults(List<Result> results)
        {
            var builder = new StringBuilder();
            foreach(var r in results)
            {
                double roi = (r.Stack - r.InitialStack) / (r.won + r.drew + r.lost);
                builder.AppendLine(String.Format("Won = {0}, drew = {1}, lost = {2}, WonPerHand= {3}", r.won, r.drew, r.lost, FormatNumber(roi)));
            }

            return builder.ToString();
        }
        
        private static string FormatNumber(double n)
        {
            return FiniteOrDefault(n).ToString("0.00",System.Globalization.CultureInfo.InvariantCulture);
        }

        private static bool HasValue(double value)
        {
            return !Double.IsNaN(value) && !Double.IsInfinity(value);
        }

        private static double FiniteOrDefault(double value)
        {
            return HasValue(value) ? value : 0;
        }

        internal static void Clear()
        {
            Instance?.MainWindow.Clear();
            Flush();
        }
    }
}
