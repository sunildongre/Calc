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
            var dt = DateTime.Now;
            IList<IList<int>> matrix = smt.TransformStringListToReversedIntMatrix(numbers, ProgramConsts.Instance.BlockSize);
            //IList<IList<int>> matrix = smt.TransformStringListToReversedIntMatrix(numbers);
            if (numbers.Count != matrix.Count)
                throw new Exception("output reversed strings less than input");

            
            CalcLogger.Instance.DebugConsoleLogLine("Transforming staged intermediaries into reversed int arrays took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");

            int lMax = 0, carry = 0;
            StringBuilder sb = new StringBuilder();

            foreach (List<int> l in matrix)
            {
                lMax = lMax < l.Count ? l.Count : lMax;
            }

            int carry_block = (int)Math.Pow(10, ProgramConsts.Instance.BlockSize);
            int padding_block = (int)Math.Pow(10, ProgramConsts.Instance.BlockSize - 1);

            for (var i = 0; i < lMax; i ++)
            {
                var x = carry;
                carry = 0;
                foreach (List<int> l in matrix)
                {
                    x += l.ElementAtOrDefault(i);
                }
                var y = 0;
                au.GetCarryBaseBlock(ref x, ref y, ref carry, carry_block);
                if(y < padding_block)
                    sb.Append('0');

                sb.Append(y);
            }

            if (carry != 0)
            {
                sb.Append(carry);
            }

            return smt.ReverseString(sb.ToString(), ProgramConsts.Instance.BlockSize);
        }
        public string Compute_old(IList<string> numbers)
        {
            StringMatrixTransformer smt = new StringMatrixTransformer();
            ArithmeticUtils au = new ArithmeticUtils();

            var dt = DateTime.Now;
            IList<IList<int>> matrix = smt.TransformStringListToReversedIntMatrix(numbers);
            CalcLogger.Instance.DebugConsoleLogLine("Transforming staged intermediaries into reversed int arrays took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");
            int lMax = 0, carry = 0;
            StringBuilder sb = new StringBuilder();

            foreach (List<int> l in matrix)
            {
                lMax = lMax < l.Count ? l.Count : lMax;
            }

            for (var i = 0; i < lMax; i++)
            {
                var x = carry;
                carry = 0;
                foreach (List<int> l in matrix)
                {
                    x += l.ElementAtOrDefault(i);
                }
                var y = 0;
                au.GetCarryBase10(ref x, ref y, ref carry);
                sb.Append(y);
            }
            if (carry != 0)
                sb.Append(carry);
            return smt.ReverseString(sb.ToString());
        }
    }
}
