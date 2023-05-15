using System;
using System.Collections.Generic;
using System.Linq;

namespace Calc
{
    public partial class LargeNumberMultiplier : ILargeNumberComputer
    {
        public string Compute(IList<string> numbers)
        {
            if (numbers.Count < 2)
            {
                throw new Exception("Need at least 2 numbers to multiply, found: " + numbers.Count + " ms");
            }
            var dt = DateTime.Now;
            StringMatrixTransformer smt = new StringMatrixTransformer();
            ILargeNumberComputer a = new LargeNumberAdder();
            ArithmeticUtils au = ArithmeticUtils.Instance;

            var num = numbers[0];
            var dict = au.GetMultiples(num, ProgramConsts.Instance.Base10BlockDigitCount);
            CalcLogger.Instance.DebugConsoleLogLine("Build Multiples completed in: " + (DateTime.Now - dt).TotalMilliseconds + " ms");
            dt = DateTime.Now;

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
                //else if (pv == 100)
                //    return numbers[0] + "00";
                //else if (pv == 1000)
                //    return numbers[0] + "000";
            }
            // and if nothing matches then the brute force method follows

            var mtx = smt.TransformStringListToReversedIntArray(numbers, ProgramConsts.Instance.BlockSize);

            var m = mtx[1];
            IList<string> stageIntermediates = new List<string>();
            var zeroAppender = "";
            for (var i = 0; i < ProgramConsts.Instance.BlockSize; i++)
                zeroAppender += "0";

            var p = "";
            for (var j = 0; j < m.Count(); j++)
            {
                var mm = m[j];

                if (mm == 0) continue;

                while (p.Length < j * ProgramConsts.Instance.BlockSize)
                    p += zeroAppender;

                stageIntermediates.Add(dict[(int)mm] + p);
            }
            CalcLogger.Instance.DebugConsoleLogLine("Substituting Multiples for entire operand took: " + (DateTime.Now - dt).TotalMilliseconds);
            dt = DateTime.Now;
            num = a.Compute(stageIntermediates);
            CalcLogger.Instance.DebugConsoleLogLine("Adding up stage intermediaries took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");
            return num;
        }
        public long[] Compute(long[][] numbers)
        {
            if (numbers.Count() < 2)
            {
                throw new Exception("Need at least 2 numbers to multiply, found: " + numbers.Count() + " ms");
            }
            var dt = DateTime.Now;
            StringMatrixTransformer smt = new StringMatrixTransformer();
            ILargeNumberComputer a = new LargeNumberAdder();
            ArithmeticUtils au = ArithmeticUtils.Instance;

            var num = numbers[0];
            var dict = au.GetMultiples(num, ProgramConsts.Instance.Base10BlockDigitCount);
            CalcLogger.Instance.DebugConsoleLogLine("Build Multiples completed in: " + (DateTime.Now - dt).TotalMilliseconds + " ms");
            dt = DateTime.Now;

            // hack to speed up multiplication and division
            // ideally this should be replaced with pattern match (?1[0]+$) - I think
            // optimizations for patterns [multiples like: X00000... or X0000Y where X and Y are integers themselves] should follow
            //long pv = 0;
            //if (long.TryParse(numbers[1], out pv))
            //{

            //    if (pv == 0)
            //        return "0";
            //    else if (pv == 1)
            //        return numbers[0];
            //    else if (pv == 10)
            //        return numbers[0] + "0";
            //    //else if (pv == 100)
            //    //    return numbers[0] + "00";
            //    //else if (pv == 1000)
            //    //    return numbers[0] + "000";
            //}
            // and if nothing matches then the brute force method follows

            //var mtx = smt.TransformStringListToReversedIntArray(numbers, ProgramConsts.Instance.BlockSize);

            var m = numbers[1];

            /* Imagine you multipley (purposefully depicting block size = 2 calculation)
             * 
             * 
             *                          95  61  08  29  03   <- length is 5
             *                          x       12  34  56   <- length is 3
             * -------------------------------------------
             *                      53  54  20  64  25  68   <- length is 6 (5 + 0 + 1)
             *  +               32  50  76  81  87  02  00   <- length is 7 (5 + 1 + 1)
             *  +           11  47  32  99  48  36  00  00   <- length is 8 (5 + 2 + 1) 
             * -------------------------------------------
             *  Ans:        11  80  37  30  50  87  27  68   <- length is 9 (5 + 3 + 1)
             * -------------------------------------------
             * 
             * (5 + 2 + 1) => should be read as
             * 5 - Length of the first operand
             * 2 - Index of the second operand
             * 1 - Buffer in case the product and carry overflow, we just need 1
             * 
             * This answer could be 1 more digit long, assuming that intermediaries are all 99's
             * hence the +1 in the end is needed
             * long has a default value of 0, so we should be good with an additional cell.
             * that's how the length of the second dimention of the array is computed.
             */
            var w = m.Length + num.Length + 1;
            long[][] stageIntermediates = new long[m.Length][]; 

            //var zeroAppender = "";
            //for (var i = 0; i < ProgramConsts.Instance.BlockSize; i++)
            //    zeroAppender += "0";

            //IList<long> p = new List<long>();
            for (var j = 0; j < m.Count(); j++)
            {
                var mm = m[j];

                if (mm == 0) continue;

                var row = new long[dict[(int)mm].Length + j];

                for(int i = 0, k = 0 ; i < dict[(int)mm].Length; i++, k++) 
                {
                    row[j+i] = dict[(int)mm][i];
                }
                stageIntermediates[j] = row;
            }
            CalcLogger.Instance.DebugConsoleLogLine("Substituting Multiples for entire operand took: " + (DateTime.Now - dt).TotalMilliseconds);
            dt = DateTime.Now;
            var resetValues = smt.RealignBlockSizes(stageIntermediates, ProgramConsts.Instance.BlockSize, ProgramConsts.Instance.AdditionBlockSize);
            CalcLogger.Instance.DebugConsoleLogLine("Realigning blocks form " + ProgramConsts.Instance.BlockSize + " to " + ProgramConsts.Instance.AdditionBlockSize +  " took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");
            dt = DateTime.Now;
            num = a.Compute(resetValues);
            CalcLogger.Instance.DebugConsoleLogLine("Adding up stage intermediaries took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");
            return num;
        }
    }
}
