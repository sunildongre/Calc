using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace Calc
{
    class Program
    {
        private static List<char> options = new List<char> { 'a', '+', 'x', 'm', 's', '-', '/', 'd', '*'};
        static void Main(string[] args)
        {
            string[] arguments;
            if(args.Length == 0)
            {
                PrintHelp();
                System.Console.ReadKey();
            }

            arguments = ValidateInutParameters(args);

            string num1 = arguments[0];
            CalcLogger.Instance.DebugConsoleLogLine(num1); 
            string num2 = arguments[2];
            CalcLogger.Instance.DebugConsoleLogLine(num2);
            char op = arguments[1].ToCharArray()[0];

            string result = Run(num1, num2, op);
            CalcLogger.Instance.DebugConsoleLogLine("Ans: " + result);
            System.Console.ReadKey();
        }

        #region Validator and Help messages
        private static string[] ValidateInutParameters(string[] args)
        {
            if (args.Length == 2 && args[0] == "-f")
            {
                return ReadFileToArgs(args);
            }

            // Length Validation
            if (args.Length != 3)
            {
                System.Console.WriteLine("Number of arguments passed is not 3.");
                PrintHelp();
                System.Environment.Exit(1);
            }
            if (!OrderValidator(args))
            {
                PrintHelp();
                System.Environment.Exit(1);
            }
            return args;
        }

        private static bool OrderValidator(string[] args)
        {
            string num1 = args[0];
            char[] operationArr = args[1].ToCharArray();
            string num2 = args[2];

            if (operationArr.Length != 1 || !options.Contains(operationArr[0]))
            {
                System.Console.WriteLine("Operator incorrectly specified.");
                return false;
            }

            Regex rgx = new Regex("[+-]?[0-9]+");
            if (!rgx.IsMatch(num1) || !rgx.IsMatch(num2))
            {
                PrintOperandFormatIncorrect();
                return false;
            }

            return true; ;
        }

        private static void PrintOperandFormatIncorrect()
        {
            System.Console.WriteLine("Numbers not in correct format.");
        }

        private static void PrintHelp()
        {
            System.Console.WriteLine("Run the application like this:");
            System.Console.WriteLine("<number 1> <operator> <number 2>");
            System.Console.WriteLine("where number 1 and number 2 are numeric operands total not exceeding 250 characters.");
            System.Console.WriteLine("You may also put the expression in a file file and invoke it as:");
            System.Console.WriteLine("-f <filename>");
            System.Console.WriteLine("and operator is one of a/s/m/d where:");
            System.Console.WriteLine("a -or- + : Addition");
            System.Console.WriteLine("s -or- - : Subtractoin");
            System.Console.WriteLine("m -or- x -or- *: Multiplication");
            System.Console.WriteLine("d -or- / : Division");
            System.Console.WriteLine("For example: Calc.exe 1234567890122546443 x 3659235987365457298");
            System.Console.WriteLine("or : Calc.exe 1234567833546443 / 4928345298");
            System.Console.WriteLine("or : Calc.exe -f multiply.txt");
            System.Console.WriteLine("where multiply.txt contains a string like 1234567833546443 * 4928345298");
        }
        #endregion


        private static string Run(string num1, string num2, char op)
        {
            string[] numbers = new string[] { num1, num2};
            switch(op)
            {
                case 'a':
                case '+':
                    LargeNumberAdder a = new LargeNumberAdder();
                    return a.Compute(numbers);
                    break;
                case 's':
                case '-':
                    LargeNumberSubtractor s = new LargeNumberSubtractor();
                    return s.Compute(numbers);
                    break;
                case 'x':
                case 'm':
                case '*':
                    LargeNumberMultiplier m = new LargeNumberMultiplier();
                    return m.Compute(numbers);
                    break;
                case '/':
                case 'd':
                    LargeNumberDivider d = new LargeNumberDivider();
                    return d.Compute(numbers);
                    break;
                default:
                    PrintHelp();
                    return "";
            }
        }

        private static string[] ReadFileToArgs(string[] args)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(args[1]);
                List<string> arguments = new List<string>();
                for(int i = 0; i < lines.Length; i++)
                {
                    string[] fragments = lines[i].Split(' ');
                    for(int j = 0; j <  fragments.Length; j++)
                        if(fragments[j].Length > 0)
                            arguments.Add(fragments[j]);
                }
                return arguments.ToArray();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Error Reading file. Exception details below:");
                System.Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
