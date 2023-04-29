using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calc
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
            if (numbers[1] == "0")
                return "0";
            else if (numbers[1] == "1")
                return numbers[0];
            else if (numbers[1] == "10")
                return numbers[0] + "0";

            StringBuilder sb = new StringBuilder();
            long carry = 0, y = 0;
            var n = long.Parse(numbers[1]);
            IList<long> number = smt.TransformStringtoReverseIntList(numbers[0], ProgramConsts.Instance.BlockSize);
            foreach (var bn in number)
            {
                au.GetCarryBase10forSingleMultiple(ref carry, bn, n, ref y);
                sb.Append(smt.ReverseString(y.ToString().PadLeft(ProgramConsts.Instance.BlockSize, '0')));
            }
            if (carry != 0)
            {
                sb.Append(smt.ReverseString(carry.ToString()));
            }
            return smt.ReverseString(sb.ToString());
        }
    }

}
