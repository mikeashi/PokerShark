using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Core.Helpers
{
    public class StringHelper
    {
        public static string Indent(int count)
        {
            return "".PadLeft(count);
        }
    }
    
}
