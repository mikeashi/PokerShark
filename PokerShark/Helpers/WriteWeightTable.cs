using PokerShark.AI;
using PokerShark.Poker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = PokerShark.Poker.Action;

namespace PokerShark.Tests
{
    internal class WriteWeightTable
    {
        public static void WriteCSV()
        {
            // create player
            Player player = new Player("Player1", "1");

            // create model
            PlayerModel model = new PlayerModel(player);

            // add action
            //var action = new Action("1", "Player1", 20, RoundState.Preflop);
            //model.ReceiveAction(action);

            // write table to file
            File.WriteAllText(Path.Combine("table.csv"), model.WeightTable.ToCSV());

        }
    }
}
