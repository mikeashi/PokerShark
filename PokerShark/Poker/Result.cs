using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Poker
{
    public class Result
    {
        public Player Player {  get; private set; }
        public int won { get; private set; }
        public int drew { get; private set; }
        public int lost { get; private set; }

        public Result(Player player)
        {
            Player = player;
            won = 0;
            drew = 0;
            lost = 0;
        }

        public void Won()
        {
            won++;
        }

        public void Drew()
        {
            drew++;
        }

        public void Lost()
        {
            lost++;
        }
    }
}
