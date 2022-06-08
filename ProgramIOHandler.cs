using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Calc
{
    public class ProgramIOHandler
    {
        private static string answerPrefix = "Ans: ";
        private static ProgramIOHandler _instance = null;

        public static ProgramIOHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProgramIOHandler();
                }
                return _instance;
            }
        }

        public string[] ReadFileToArgs(string[] args)
        {
            try
            {
                string[] lines = File.ReadAllLines(args[1]);
                List<string> arguments = new List<string>();
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] fragments = lines[i].Split(' ');

                    for (int j = 0; j < fragments.Length; j++)
                        if (fragments[j].Length > 0 && !fragments[j].Contains(answerPrefix))
                            arguments.Add(fragments[j]);
                        else
                            break;
                }
                return arguments.ToArray();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Error Reading file. Exception details below:");
                Console.WriteLine(e.Message);
            }
            return null;
        }


        public void HandleOutput(string[] args, string result)
        {
            result = answerPrefix + result;
            CalcLogger.Instance.DebugConsoleLogLine(result);

            if (ProgramInputValidator.Instance.Mode == RunMode.FILE)
            {
                using (StreamWriter sw = File.AppendText(args[1]))
                {
                    sw.WriteLine(string.Empty);
                    sw.WriteLine(result);
                    CalcLogger.Instance.DebugConsoleLogLine("Results Flushed to file..!");
                }
            }
        }

    }
}
