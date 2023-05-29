using Calc.Interface;
using Calc.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calc.Calculators
{

    public class LargeToOneNumberMultiplier : ILargeNumberComputer
    {
        static readonly StringMatrixTransformer smt = new StringMatrixTransformer();
        static readonly ArithmeticUtils au = ArithmeticUtils.Instance;

        public string Compute(IList<string> numbers)
        {
            if (numbers.Count != 2)
            {
                throw new Exception("Invalid number of arguments, 2 expected, found: " + numbers.Count);
            }

            // hack to speed up multiplication and division
            // ideally this should be replaced with pattern match (?1[0]+$) - I think
            // optimizations for patterns [multiples like: X00000... or X0000Y where X and Y are integers themselves] should follow
            long pv = 0;
            if (long.TryParse(numbers[1], out pv))
            {
                if (pv == 0)
                    return "0";
                else if (pv == 1)
                    return numbers[0];
                else if (pv == 10)
                    return numbers[0] + "0";
                else if (pv == 100)
                    return numbers[0] + "00";
                else if (pv == 1000)
                    return numbers[0] + "000";
            }

            var sb = new StringBuilder();
            long carry = 0, y = 0;
            var n = smt.StringTolong(numbers[1]);
            var number = smt.TransformStringtoReverseIntList(numbers[0], ProgramConsts.Instance.BlockSize);

            foreach (var bn in number)
            {
                au.GetCarryBasseBlock(ref carry, bn, n, ref y);
                sb.Insert(0, y.ToString().PadLeft(ProgramConsts.Instance.BlockSize, '0'));
            }
            if (carry != 0)
            {
                sb.Insert(0, carry.ToString());
            }
            return sb.ToString();
        }

        public long[] Compute(long[][] numbers)
        {
            long carry = 0, y = 0, i = 0;
            var n = numbers[1][0];
            var opt = new long[numbers[0].Length + 1];

            for (; i < numbers[0].Length; i++)
            {
                au.GetCarryBasseBlock(ref carry, numbers[0][i], n, ref y);
                opt[i] = y;
            }
            if (carry != 0)
            {
                opt[i] = carry;
            }
            return opt;
        }
    }
}
