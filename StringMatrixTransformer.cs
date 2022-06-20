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
            IList<IList<int>> matrix = new List<IList<int>>();

            for (int i = 0; i < lNums.Count; i++)
            {
                matrix.Add(TransformStringtoReverseIntList(lNums[i]));
            }
            return matrix;
        }

        public IList<int> TransformStringtoReverseIntList(string number)
        {
            List<int> row = new List<int>();
            StringBuilder sb = new StringBuilder();

            for (var j = number.Length - 1; j >= 0; j--)
            {
                switch (number[j])
                {
                    case '0': row.Add(0); break;
                    case '1': row.Add(1); break;
                    case '2': row.Add(2); break;
                    case '3': row.Add(3); break;
                    case '4': row.Add(4); break;
                    case '5': row.Add(5); break;
                    case '6': row.Add(6); break;
                    case '7': row.Add(7); break;
                    case '8': row.Add(8); break;
                    case '9': row.Add(9); break;
                }
            }
            return row;
        }

        public string ReverseString(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (var j = str.Length - 1; j >= 0;  j--)
            {
                sb.Append(str[j]);
            }
            return sb.ToString();
        }
    }
}
