using Calc.Interface;
using Calc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Calculators
{
    public class LargeNumberAdder : ILargeNumberComputer
    {
        private readonly StringMatrixTransformer smt = new StringMatrixTransformer();
        private readonly ArithmeticUtils au = ArithmeticUtils.Instance;
        private readonly NumercStringUtils nsu = new NumercStringUtils();

        public string Compute(IList<string> numbers)
        {
            var dt = DateTime.Now;
            var mtx = smt.TransformStringListToReversedIntArray(numbers, ProgramConsts.Instance.AdditionBlockSize);

            if (numbers.Count != mtx.Count())
                throw new Exception("output reversed strings less than input");


            CalcLogger.Instance.DebugConsoleLogLine("Transforming staged intermediaries into reversed long arrays took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");

            long lMax = 0, carry = 0;
            var sb = new StringBuilder();

            foreach (var l in mtx)
            {
                lMax = lMax < l.Count() ? l.Count() : lMax;
            }

            var carry_block = (int)Math.Pow(10, ProgramConsts.Instance.AdditionBlockSize);
            var padding_block = (int)Math.Pow(10, ProgramConsts.Instance.AdditionBlockSize - 1);


            /*
             * Following code works 
             * performance regression...!
             * move it out to a different method
             */
            var pos_total = new long[lMax];
            Parallel.ForEach(pos_total, (l, s, i) =>
            {
                foreach (var lst in mtx)
                    pos_total[(int)i] += lst.ElementAtOrDefault((int)i);
            });

            long y = 0;
            for (var i = 0; i < lMax; i++)
            {
                au.GetCarryBaseBlock(pos_total[i] + carry, ref y, ref carry, carry_block);
                sb.Insert(0, y.ToString().PadLeft(ProgramConsts.Instance.AdditionBlockSize, '0'));
            }

            if (carry != 0)
            {
                sb.Insert(0, carry);
            }

            return nsu.TrimLeadingZeros(sb.ToString());
        }

        public long[] Compute(long[][] numbers)
        {
            var dt = DateTime.Now;
            var mtx = numbers;

            long lMax = 0, carry = 0;
            var sb = new StringBuilder();

            foreach (var l in mtx)
            {
                if (l == null) continue;
                lMax = lMax < l.Count() ? l.Count() : lMax;
            }

            var carry_block = (int)Math.Pow(10, ProgramConsts.Instance.AdditionBlockSize);
            var padding_block = (int)Math.Pow(10, ProgramConsts.Instance.AdditionBlockSize - 1);

            var pos_total = new long[lMax];
            Parallel.ForEach(pos_total, (l, s, i) =>
            {
                foreach (var lst in mtx)
                {
                    if (lst != null)
                    {
                        pos_total[(int)i] += lst.ElementAtOrDefault((int)i);
                    }
                }

            });

            long y = 0;
            IList<long> result = new List<long>();
            for (var i = 0; i < lMax; i++)
            {
                au.GetCarryBaseBlock(pos_total[i] + carry, ref y, ref carry, carry_block);
                result.Add(y);
            }

            if (carry != 0)
            {
                result.Add(carry);
            }
            return result.ToArray();
        }
    }
}
