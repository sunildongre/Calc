using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calc
{
    public partial class LargeNumberMultiplier : ILargeNumberComputer
    {

        public string Compute(IList<string> numbers)
        {
            if (numbers.Count < 2)
            {
                throw new Exception("Need at least 2 numbers to multiply, found: " + numbers.Count);
            }

            StringMatrixTransformer smt = new StringMatrixTransformer();
            ILargeNumberComputer a = new LargeNumberAdder();
            ArithmeticUtils au = new ArithmeticUtils();

            string num = numbers[0];

            // hack to speed up division
            // ideally this should be replaced with pattern match (?1[0]+$) - I think
            // optimizations for patterns [multiples like: X00000... or X0000Y where X and Y are integers themselves] should follow
            if (numbers[1] == "0")
                return "0";
            else if (numbers[1] == "1")
                return numbers[0];
            else if (numbers[1] == "10")
                return numbers[0] + "0";

            // and if nothing matches then the brute force method follows

            IList<IList<int>> matrix = smt.TransformStringListToReversedIntMatrix(numbers);

            for (int i = 1; i < numbers.Count; i++)
            {
                IList<int> m = matrix.ElementAt(i);
                ILargeNumberComputer lom = new LargeToOneNumberMultiplier();
                IList<string> stageIntermediates = new List<string>();
                object listlock = new object();

                Parallel.ForEach(m, (mm, s, index) =>
                {
                    string prefix = "";
                    for (int k = 0; k < index; k++)
                    {
                        prefix += "0";
                    }
                    var res = lom.Compute(new List<string>() { num, mm.ToString() }) + prefix;
                    lock (listlock) { stageIntermediates.Add(res); }

                });

                num = a.Compute(stageIntermediates);
            }
            return num;
        }
    }
}
