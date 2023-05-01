using System;
using System.Collections.Generic;
using System.Text;

namespace Calc
{
    public class LargeNumberSubtractor : ILargeNumberComputer
    {
        public string Compute(IList<string> numbers)
        {
            if (numbers.Count > 2)
            {
                throw new Exception("Incorrect count of numbers to subtract, expecting 2, found: " + numbers.Count.ToString());
            }

            StringMatrixTransformer smt = new StringMatrixTransformer();
            NumercStringUtils nsu = new NumercStringUtils();

            bool negateAnswer = ReorderLargerFirst(ref numbers);
            IList<IList<long>> matrix = smt.TransformStringListToReversedIntMatrix(numbers);

            StringBuilder sb = new StringBuilder();

            int i = 0;
            for (; i < matrix[1].Count; i++)
            {
                if (matrix[0][i] < matrix[1][i])
                {
                    SetupCarryAt(matrix[0], i);
                }
                sb.Append(matrix[0][i] - matrix[1][i]);
            }

            // get the rest if there's any left in the larger number
            for (; i < matrix[0].Count; i++)
                sb.Append(matrix[0][i]);

            return nsu.TrimLeadingZeros(
                negateAnswer ?
                    "-" + smt.ReverseString(sb.ToString()) :
                    smt.ReverseString(sb.ToString()));
        }

        private void SetupCarryAt(IList<long> m, int i)
        {
            if (m[i + 1] <= 0)
                SetupCarryAt(m, i + 1);

            m[i] += 10;
            // should not cause array out of bounds exception since we have checked that 
            // matrix[1] is smaller than matrix[0] so we should always have more items in matrix[0]
            // if we need to carry, else last digit in matrix[0] will always be greater than that of matrix[1] 
            m[i + 1] -= 1;
        }

        private bool ReorderLargerFirst(ref IList<string> numbers)
        {
            bool negateAnswer = false;
            NumercStringUtils nsu = new NumercStringUtils();
            if (numbers[0].Length < numbers[1].Length ||
                !nsu.OneGreaterThanTwo(numbers[0], numbers[1]))
            {
                negateAnswer = true;
                // swap
                string t = numbers[0];
                numbers[0] = numbers[1];
                numbers[1] = t;
            }
            return negateAnswer;
        }
    }
}
