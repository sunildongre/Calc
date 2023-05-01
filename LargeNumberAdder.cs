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
            ArithmeticUtils au = ArithmeticUtils.Instance;
            var dt = DateTime.Now;
            IList<IList<long>> matrix = smt.TransformStringListToReversedIntMatrix(numbers, ProgramConsts.Instance.AdditionBlockSize);

            if (numbers.Count != matrix.Count)
                throw new Exception("output reversed strings less than input");

            
            CalcLogger.Instance.DebugConsoleLogLine("Transforming staged intermediaries into reversed long arrays took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");

            long lMax = 0, carry = 0;
            StringBuilder sb = new StringBuilder();

            foreach (List<long> l in matrix)
            {
                lMax = lMax < l.Count ? l.Count : lMax;
            }

            int carry_block = (int)Math.Pow(10, ProgramConsts.Instance.AdditionBlockSize);
            int padding_block = (int)Math.Pow(10, ProgramConsts.Instance.AdditionBlockSize - 1);

            if (1 == 1)
            {
                for (var i = 0; i < lMax; i++)
                {
                    var x = carry;
                    carry = 0;
                    foreach (List<long> l in matrix)
                    {
                        x += l.ElementAtOrDefault(i);
                    }
                    long y = 0;
                    au.GetCarryBaseBlock(x, ref y, ref carry, carry_block);
                    //sb.Append(y.ToString().PadLeft(ProgramConsts.Instance.AdditionBlockSize, '0'));
                    sb.Insert(0, y.ToString().PadLeft(ProgramConsts.Instance.AdditionBlockSize, '0'));
                }
            }
            else
            {
                /*
                 * Following code works but seems to give incorrect answers for block_size 3 
                 * Also performance regression...!
                 * move it out to a different method
                 */
                List<long> pos_total = new List<long>(new long[lMax]);
                Parallel.ForEach(pos_total, (l, s, i) =>
                {
                    foreach (List<long> lst in matrix)
                        pos_total[(int)i] += lst.ElementAtOrDefault((int)i);
                });

                long y = 0;
                pos_total.ForEach(x =>
                {
                    x += carry;
                    au.GetCarryBaseBlock(x, ref y, ref carry, carry_block);
                    if (y < padding_block)
                        sb.Insert(0,'0');

                    //sb.Append(y);
                    sb.Insert(0, y);
                });

            }
            if (carry != 0)
            {
                //sb.Append(carry);
                sb.Insert(0, carry);
            }

            //return smt.ReverseString(sb.ToString(), ProgramConsts.Instance.AdditionBlockSize);
            return sb.ToString().TrimStart('0');
        }
    }
}
