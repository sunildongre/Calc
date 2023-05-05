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

        public string Compute_list(IList<string> numbers)
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
                else if (pv == 100)
                    return numbers[0] + "00";
                else if (pv == 1000)
                    return numbers[0] + "000";
            }
            // and if nothing matches then the brute force method follows

            IList<IList<long>> matrix = smt.TransformStringListToReversedIntMatrix(numbers, ProgramConsts.Instance.BlockSize);

            List<long> m = matrix.ElementAt(1).ToList();
            IList<string> stageIntermediates = new List<string>();
            var zeroAppender = "";
            for (var i = 0; i < ProgramConsts.Instance.BlockSize; i++)
                zeroAppender += "0";

            var p = "";
            for (var j = 0; j < m.Count; j++)
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
    }
}
