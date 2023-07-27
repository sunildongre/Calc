using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArithmeticLib.Interface;

namespace ArithmeticLib.Calculators
{
    public class LargeNumberSubtractor : ILargeNumberComputer
    {
        //#region IList<string>
        public string Compute(IList<string> numbers)
        {
            //    if (numbers.Count > 2)
            //    {
            //        throw new Exception("Incorrect count of numbers to subtract, expecting 2, found: " + numbers.Count.ToString());
            //    }

            //    var smt = new StringMatrixTransformer();
            //    var nsu = new NumercStringUtils();

            //    var negateAnswer = ReorderLargerFirst(ref numbers);
            //    var matrix = smt.TransformStringListToReversedIntMatrix(numbers);

            //    var sb = new StringBuilder();

            //    var i = 0;
            //    for (; i < matrix[1].Count; i++)
            //    {
            //        if (matrix[0][i] < matrix[1][i])
            //        {
            //            SetupCarryAt(matrix[0], i);
            //        }
            //        sb.Append(matrix[0][i] - matrix[1][i]);
            //    }

            //    // get the rest if there's any left in the larger number
            //    for (; i < matrix[0].Count; i++)
            //        sb.Append(matrix[0][i]);

            //    return nsu.TrimLeadingZeros(
            //        negateAnswer ?
            //            "-" + smt.ReverseString(sb.ToString()) :
            //            smt.ReverseString(sb.ToString()));
            return null;
        }

        //private void SetupCarryAt(IList<long> m, int i)
        //{
        //    if (m[i + 1] <= 0)
        //        SetupCarryAt(m, i + 1);

        //    m[i] += 10;
        //    // should not cause array out of bounds exception since we have checked that 
        //    // matrix[1] is smaller than matrix[0] so we should always have more items in matrix[0]
        //    // if we need to carry, else last digit in matrix[0] will always be greater than that of matrix[1] 
        //    m[i + 1] -= 1;
        //}

        //private bool ReorderLargerFirst(ref IList<string> numbers)
        //{
        //    var negateAnswer = false;
        //    var nsu = new NumercStringUtils();
        //    if (numbers[0].Length < numbers[1].Length ||
        //        !nsu.OneGreaterThanTwo(numbers[0], numbers[1]))
        //    {
        //        negateAnswer = true;
        //        // swap
        //        var t = numbers[0];
        //        numbers[0] = numbers[1];
        //        numbers[1] = t;
        //    }
        //    return negateAnswer;
        //}
        //#endregion


        #region long[]
        private void SetupCarryAt(long[] m, int i)
        {
            if (m[i + 1] <= 0)
                SetupCarryAt(m, i + ProgramConsts.Instance.Base10BlockDigitCount);

            m[i] += ProgramConsts.Instance.Base10BlockDigitCount;
            // should not cause array out of bounds exception since we have checked that 
            // matrix[1] is smaller than matrix[0] so we should always have more items in matrix[0]
            // if we need to carry, else last digit in matrix[0] will always be greater than that of matrix[1] 
            m[i + 1] -= 1;
        }


        private bool ReorderLargerFirst(long[][] numbers)
        {
            var negateAnswer = false;
            var nsu = new NumercStringUtils();
            if (numbers[0].Length < numbers[1].Length ||
                !nsu.OneGreaterThanTwo(numbers[0], numbers[1]))
            {
                negateAnswer = true;
                // swap
                var t = numbers[0];
                numbers[0] = numbers[1];
                numbers[1] = t;
            }
            return negateAnswer;
        }

        public long[] Compute(long[][] numbers)
        {
            if (numbers.Length < 2 || numbers.Length > 2)
            {
                throw new Exception("Incorrect count of numbers to subtract, expecting 2, found: " + numbers.Length);
            }

            var negateAnswer = ReorderLargerFirst(numbers);
            var res = new List<long>();

            var i = 0;
            for (; i < numbers[1].Length; i++)
            {
                if (numbers[0][i] < numbers[1][i])
                {
                    SetupCarryAt(numbers[0], i);
                }
                res.Add(numbers[0][i] - numbers[1][i]);
            }

            // get the rest if there's any left in the larger number
            for (; i < numbers[0].Length; i++)
            {

                res.Add(numbers[0][i]);
            }
            return res.ToArray();
        }
        #endregion
    }
}
