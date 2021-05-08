using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class LargeNumberAdder : ILargeNumberComputer
    {
        public string Compute(IList<string> numbers)
        {
            StringMatrixTransformer smt = new StringMatrixTransformer();
            ArithmeticUtils au = new ArithmeticUtils();

            IList<IList<int>> matrix = smt.TransformStringListToReversedIntMatrix(numbers);
            int lMax = 0, carry = 0;
            StringBuilder sb = new StringBuilder();

            foreach (List<int> l in matrix)
            {
                lMax = lMax < l.Count ? l.Count : lMax;
            }
            
            for(int i = 0; i < lMax; i ++)
            {
                int x = carry;
                carry = 0;
                foreach (List<int> l in matrix)
                {
                    x += l.ElementAtOrDefault(i);
                }
                int y = 0;
                au.GetCarryBase10(ref x, ref y, ref carry);
                sb.Append(y);
            }
            
            return smt.ReverseString(sb.ToString());
        }
    }
}
