using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.PyPoker
{
    public class Main
    {
        public int amount { get; set; }
    }

    public class Pot
    {
        public Main main { get; set; }
        public List<Side> side { get; set; }

        public int Amount(string id)
        {
            return main.amount + side.Where(s => s.eligibles.Contains(id)).Sum(s => s.amount);
        }
    }

    public class Side
    {
        public int amount { get; set; }
        public List<string> eligibles { get; set; }
    }

}
