using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calc
{

    public class LargeToOneNumberMultiplier : ILargeNumberComputer
    {
        public string Compute(IList<string> numbers)
        {
            if (numbers.Count != 2)
            {
                throw new Exception("Invalid number of arguments, 2 expected, found: " + numbers.Count);
            }
            StringMatrixTransformer smt = new StringMatrixTransformer();
            ArithmeticUtils au = new ArithmeticUtils();
            IList<IList<int>> reversed = smt.TransformStringListToReversedIntMatrix(numbers);

            StringBuilder sb = new StringBuilder();

            int carry = 0, y = 0;
            int n = reversed.ElementAtOrDefault(1).ElementAtOrDefault(0);
            IList<int> number = reversed.ElementAtOrDefault(0);
            foreach (int bn in number)
            {
                var x = carry;
                x += bn * n;
                au.GetCarryBase10(ref x, ref y, ref carry);
                sb.Append(y);
            }
            if (carry != 0)
            {
                sb.Append(carry);
            }
            return smt.ReverseString(sb.ToString());
        }
    }

}
