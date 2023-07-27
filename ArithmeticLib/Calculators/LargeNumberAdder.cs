using ArithmeticLib.Interface;
using ArithmeticLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArithmeticLib.Calculators
{
    public class LargeNumberAdder : ILargeNumberComputer
    {
        private readonly StringMatrixTransformer smt = new StringMatrixTransformer();
        private readonly ArithmeticUtils au = ArithmeticUtils.Instance;
        private readonly NumercStringUtils nsu = new NumercStringUtils();

        #region IList<string>
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
        #endregion

        #region long[]
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

            var carry_block = (long)Math.Pow(10, ProgramConsts.Instance.AdditionBlockSize);
            var padding_block = (long)Math.Pow(10, ProgramConsts.Instance.AdditionBlockSize - 1);

            var pos_total = new long[lMax];
            Parallel.ForEach(pos_total, (l, s, i) =>
            {
                foreach (var lst in mtx)
                {
                    if (lst != null)
                    {
                        pos_total[(int)i] = checked(pos_total[(int)i]+ lst.ElementAtOrDefault((int)i));

                        // enable below if you run over the limit for the largest intermediate sum for a column 
                        // most likely reducing the AdditionBlockSize will resolve the issue. 
                        // Theoritical limit is (long.MaxValue / 9) count of values in a column to add at 
                        // AdditionBlockSize = 1

                        //if (pos_total[(int)i].ToString().Length >= ProgramConsts.Instance.Base10AdditionBlockDigitCount)
                        //{
                        //    CalcLogger.Instance.DebugConsoleLogLine("intermediate :" + pos_total[(int)i]);
                        //}
                    }
                }

            });

            long y = 0;
            var res = new long[lMax+2];
            var ii = 0;
            for (; ii < lMax; ii++)
            {
                au.GetCarryBaseBlock(pos_total[ii] + carry, ref y, ref carry, carry_block);
                res[ii] = y;
            }

            if (carry != 0)
            {
                res[ii] = carry;
            }
            CalcLogger.Instance.DebugConsoleLogLine("Adding up results took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");
            return res;
        }
        #endregion
    }
}
