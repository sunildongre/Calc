﻿using System;
using System.Collections.Generic;
using System.IO;
using Calc.DataTypes;
using Calc.Utils;

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
                for (long i = 0; i < lines.Length; i++)
                {
                    string[] fragments = lines[i].Split(' ');

                    for (long j = 0; j < fragments.Length; j++)
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


        public void HandleOutput(string[] args, string result, DateTime t1)
        {
            result = answerPrefix + result;
            CalcLogger.Instance.DebugConsoleLogLine(result);

            if (ProgramInputValidator.Instance.Mode == RunMode.FILE)
            {
                using (StreamWriter sw = File.AppendText(args[1]))
                {
                    sw.WriteLine(string.Empty);
                    sw.WriteLine(result);
                    CalcLogger.Instance.InfoConsoleLogLine("Results calculated in: " + (DateTime.Now - t1).ToString() + " Flushed to file..!");
                }
            }
        }

    }
}
