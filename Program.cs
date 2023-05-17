namespace Calc
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] arguments;
            if (args.Length == 0)
            {
                ProgramMessages.Instance.PrintHelpText();
                System.Console.ReadKey();
                return;
            }
            var t1 = System.DateTime.Now;
            // need a fast input parser
            ArithmeticUtils au = ArithmeticUtils.Instance;
            // this will filter the operating mode and will be used by the Program IO handler when it is invoked
            arguments = ProgramInputValidator.Instance.ValidateInputParameters(args);

            var num1 = arguments[0];
            CalcLogger.Instance.DebugConsoleLogLine(num1);
            var num2 = arguments[2];
            CalcLogger.Instance.DebugConsoleLogLine(num2);
            var op = arguments[1].ToCharArray()[0];

            
            ProgramIOHandler.Instance.HandleOutput(args, Run(num1, num2, op));
            var t2 = System.DateTime.Now;
            System.Console.WriteLine("Time taken to execute the calculation: {0}", t2 - t1);
            System.Console.ReadKey();
        }

        private static string Run(string num1, string num2, char op)
        {
            var numbers = new string[] { num1, num2 };
            StringMatrixTransformer smt = new StringMatrixTransformer();
            var nums = smt.TransformStringListToReversedIntArray(numbers, ProgramConsts.Instance.BlockSize);

            switch (op)
            {
                case 'a':
                case '+':
                    LargeNumberAdder a = new LargeNumberAdder();
                    smt.RealignBlockSizes(nums, 2, 8);
                    return smt.TransformLongBlockArrayToString(a.Compute(nums), ProgramConsts.Instance.AdditionBlockSize);
                case 's':
                case '-':
                    LargeNumberSubtractor s = new LargeNumberSubtractor();
                    return smt.TransformLongBlockArrayToString(s.Compute(nums), ProgramConsts.Instance.AdditionBlockSize);
                case 'x':
                case 'm':
                case '*':
                    LargeNumberMultiplier m = new LargeNumberMultiplier();
                    return smt.TransformLongBlockArrayToString(m.Compute(nums), ProgramConsts.Instance.AdditionBlockSize);
                case '/':
                case 'd':
                    LargeNumberDivider d = new LargeNumberDivider();
                    return smt.TransformLongBlockArrayToString(d.Compute(nums), ProgramConsts.Instance.AdditionBlockSize);
                default:
                    ProgramMessages.Instance.PrintHelpText();
                    return "";
            }
        }
    }
}
