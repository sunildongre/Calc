using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class StringMatrixTransformer
    {
        public IList<IList<int>> TransformStringListToReversedIntMatrix(IList<string> lNums)
        {
            int lNumMax = lNums.OrderByDescending(n => n.Length).First().Length;

            IList<IList<int>> matrix = new List<IList<int>>();

            for (int i = 0; i < lNums.Count; i++)
            {
                matrix.Add(TransformStringtoReverseIntList(lNums[i]));
            }
            return matrix;
        }

        public IList<int> TransformStringtoReverseIntList(string number)
        {
            var arr = number.ToCharArray();
            Array.Reverse(arr);
            int arrLen = arr.Length;
            List<int> row = new List<int>();

            for (int j = 0; j < arrLen; j++)
            {
                row.Add(int.Parse(arr[j].ToString()));
            }
            return row;
        }

        public string ReverseString(string str)
        {
            char[] chars = str.ToCharArray();
            char[] result = new char[chars.Length];
            for (int i = 0, j = str.Length - 1; i < str.Length; i++, j--)
            {
                result[i] = chars[j];
            }
            return new string(result);
        }
    }
}
