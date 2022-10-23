using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerShark.Windows
{
    public class LogsTextWriter : TextWriter
    {

        public override void Write(char value)
        {
            if (value == '\r')
            {
                return;
            }
            WindowsManager.GetLogsWindow().Write(value.ToString());
            WindowsManager.Flush();
        }

        public override void Write(string? value)
        {
            WindowsManager.GetLogsWindow().Write(value);
            WindowsManager.Flush();
        }

        public override void WriteLine(char value)
        {
            WindowsManager.GetLogsWindow().WriteLine(value.ToString());
            WindowsManager.Flush();
        }

        public override void WriteLine(string? value)
        {
            if (value != null)
            {
                WindowsManager.GetLogsWindow().WriteLine(value);
                WindowsManager.Flush();
            }
        }

        public override Encoding Encoding => Encoding.Default;
    }
}
