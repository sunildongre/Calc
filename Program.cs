﻿using System.Collections;
using System.Collections.Generic;

namespace Calc
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] arguments;
            if(args.Length == 0)
            {
                ProgramMessages.Instance.PrintHelpText();
                System.Console.ReadKey();
                return;
            }

            // need a fast input parser

            // this will filter the operating mode and will be used by the Program IO handler when it is invoked
            arguments = ProgramInputValidator.Instance.ValidateInputParameters(args);

            string num1 = arguments[0];
            CalcLogger.Instance.DebugConsoleLogLine(num1); 
            string num2 = arguments[2];
            CalcLogger.Instance.DebugConsoleLogLine(num2);
            char op = arguments[1].ToCharArray()[0];

            ProgramIOHandler.Instance.HandleOutput(args, Run(num1, num2, op));
            
            System.Console.ReadKey();
        }

        private static string Run(string num1, string num2, char op)
        {
            string[] numbers = new string[] { num1, num2};
            switch(op)
            {
                case 'a':
                case '+':
                    LargeNumberAdder a = new LargeNumberAdder();
                    return a.Compute(numbers);
                case 's':
                case '-':
                    LargeNumberSubtractor s = new LargeNumberSubtractor();
                    return s.Compute(numbers);
                case 'x':
                case 'm':
                case '*':
                    LargeNumberMultiplier m = new LargeNumberMultiplier();
                    return m.Compute(numbers);
                case '/':
                case 'd':
                    LargeNumberDivider d = new LargeNumberDivider();
                    return d.Compute(numbers);
                default:
                    ProgramMessages.Instance.PrintHelpText();
                    return "";
            }
        }
    }
}
