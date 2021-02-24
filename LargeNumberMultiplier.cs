using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class LargeNumberMultiplier : ILargeNumberComputer
    {
        public class LargeToOneNumberMultiplier : ILargeNumberComputer
        {
            public string Compute(IList<string> numbers)
            {
                if(numbers.Count != 2)
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
                    int x = carry;
                    x += bn * n;
                    au.GetCarryBase10(ref x, ref y, ref carry);
                    sb.Append(y);
                }
                if(carry != 0)
                {
                    sb.Append(carry);
                }
                return smt.ReverseString(sb.ToString());
            }
        }

        public string Compute(IList<string> numbers)
        {
            if(numbers.Count < 2)
            {
                throw new Exception("Need at least 2 numbers to multiply, found: " + numbers.Count);
            }

            StringMatrixTransformer smt = new StringMatrixTransformer();
            ILargeNumberComputer a = new LargeNumberAdder();
            ArithmeticUtils au = new ArithmeticUtils();

            string num = numbers[0];
            IList<IList<int>> matrix = smt.TransformStringListToReversedIntMatrix(numbers);

            for(int i = 1; i < numbers.Count; i++)
            {
                IList<int> m = matrix.ElementAt(i);
                ILargeNumberComputer lom = new LargeToOneNumberMultiplier();
                IList<string> stageIntermediates = new List<string>();

                for(int j = 0; j < m.Count; j++)
                {
                    string prefix = "";
                    int n = m[j];
                    for (int k = 0; k < j; k++)
                    {
                        prefix += "0";
                    }

                    stageIntermediates.Add(lom.Compute(new List<string>() { num, n.ToString() }) + prefix);
                }
                num = a.Compute(stageIntermediates);
            }
            return num;
        }
    }
}
