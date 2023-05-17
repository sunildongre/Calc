using System.Collections.Generic;
using System.Text.RegularExpressions;
using Calc.DataTypes;

namespace Calc.Utils
{
    public class ProgramInputValidator
    {
        private static List<char> options = new List<char> { 'a', '+', 'x', 'm', 's', '-', '/', 'd', '*' };

        private static ProgramInputValidator _instance = null;

        public RunMode Mode
        {
            get; set;
        }

        public static ProgramInputValidator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProgramInputValidator();
                }
                return _instance;
            }
        }

        public string[] ValidateInputParameters(string[] args)
        {
            if (args.Length == 2 && args[0] == "-f")
            {
                Mode = RunMode.FILE;
                return ProgramIOHandler.Instance.ReadFileToArgs(args);
            }

            // Length Validation
            if (args.Length != 3)
            {
                System.Console.WriteLine("Number of arguments passed is not 3.");
                ProgramMessages.Instance.PrintHelpText();
                System.Environment.Exit(1);
            }
            if (!OrderValidator(args))
            {
                ProgramMessages.Instance.PrintHelpText();
                System.Environment.Exit(1);
            }
            return args;
        }

        private bool OrderValidator(string[] args)
        {
            var num1 = args[0];
            var operationArr = args[1].ToCharArray();
            var num2 = args[2];

            if (operationArr.Length != 1 || !options.Contains(operationArr[0]))
            {
                System.Console.WriteLine("Operator incorrectly specified.");
                return false;
            }

            var rgx = new Regex("[+-]?[0-9]+");
            if (!rgx.IsMatch(num1) || !rgx.IsMatch(num2))
            {
                ProgramMessages.Instance.PrintOperandFormatIncorrect();
                return false;
            }

            return true; ;
        }

    }
}
