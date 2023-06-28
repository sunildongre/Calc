using Calc.Utils;
using Calc.Calculators;

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

            
            ProgramIOHandler.Instance.HandleOutput(args, Run(num1, num2, op), t1);
            var t2 = System.DateTime.Now;

            System.Console.ReadKey();
        }

        private static string Run(string num1, string num2, char op)
        {
            var numbers = new string[] { num1, num2 };
            StringMatrixTransformer smt = new StringMatrixTransformer();
            NumercStringUtils nsu = new NumercStringUtils();
            var nums = smt.TransformStringListToReversedIntArray(numbers, ProgramConsts.Instance.BlockSize);
#if DEBUG
            CalcLogger.Instance.DebugConsoleLogLine("General Block Size: " + ProgramConsts.Instance.BlockSize); 
            CalcLogger.Instance.DebugConsoleLogLine("Addition Block Size: " + ProgramConsts.Instance.AdditionBlockSize);
            CalcLogger.Instance.DebugConsoleLogLine("Input numbers arg1 Length: " + num1.Length);
            CalcLogger.Instance.DebugConsoleLogLine("Converted Input arg1 Length: " + nums[0].Length);
            CalcLogger.Instance.DebugConsoleLogLine("Input numbers arg2 Length: " + num2.Length);
            CalcLogger.Instance.DebugConsoleLogLine("Converted Input arg2 Length: " + nums[1].Length);
#endif
            switch (op)
            {
                case 'a':
                case '+':
                    LargeNumberAdder a = new LargeNumberAdder();
                    smt.RealignBlockSizes(nums, 2, 8);
                    return nsu.TrimLeadingZeros(
                        smt.TransformLongBlockArrayToString(
                            a.Compute(nums), 
                            ProgramConsts.Instance.AdditionBlockSize)
                        );
                case 's':
                case '-':
                    LargeNumberSubtractor s = new LargeNumberSubtractor();
                    return nsu.TrimLeadingZeros(
                        smt.TransformLongBlockArrayToString(
                            s.Compute(nums), 
                            ProgramConsts.Instance.BlockSize)
                        );
                case 'x':
                case 'm':
                case '*':
                    LargeNumberMultiplier m = new LargeNumberMultiplier();
                    return nsu.TrimLeadingZeros(
                        smt.TransformLongBlockArrayToString(
                            m.Compute(nums), 
                            ProgramConsts.Instance.AdditionBlockSize)
                        );
                case '/':
                case 'd':
                    LargeNumberDivider d = new LargeNumberDivider();
                    return nsu.TrimLeadingZeros(
                        smt.TransformLongBlockArrayToString(
                            d.Compute(nums), 
                            ProgramConsts.Instance.BlockSize)
                        );
                default:
                    ProgramMessages.Instance.PrintHelpText();
                    return "";
            }
        }
    }
}
