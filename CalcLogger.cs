using System;
using System.Text;

namespace Calc
{
    public class CalcLogger
    {
        private static CalcLogger _instance = null;
        private StringBuilder sb;

        private string DEBUG = "DEBUG";
        private string INFO = "INFO";
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
#if DEBUG
            var sb = ConstructLogStatement(str, DEBUG);
            Console.WriteLine(sb.ToString());
            sb.Clear();
#endif
        }

        public void InfoConsoleLogLine(string str)
        {
            var sb = ConstructLogStatement(str, INFO);
            Console.WriteLine(sb.ToString());
            sb.Clear();
        }

        private StringBuilder ConstructLogStatement(string str, string level)
        {
            sb.Append(level);
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
