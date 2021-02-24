using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class CalcLogger
    {
        private static CalcLogger _instance = null;
        private StringBuilder sb;

        private string DEBUG = "DEBUG";
        private string TAB = "\t";

        private CalcLogger()
        {
            sb = new StringBuilder();
        }

        public static CalcLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CalcLogger();
                }
                return _instance;
            }
        }

        public void DebugConsoleLogLine(string str)
        {
            var sb = ConstructLogStatement(str);
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }

        private StringBuilder ConstructLogStatement(string str)
        {
            sb.Append(DEBUG);
            sb.Append(TAB);
            sb.Append(DateTime.Now.ToString());
            sb.Append(TAB);
            sb.Append("[");
            sb.Append(str.Length.ToString());
            sb.Append("]");
            sb.Append(TAB);
            sb.Append(str);
            return sb;
        }
    }
}
