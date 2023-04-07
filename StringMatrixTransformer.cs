using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class StringMatrixTransformer
    {
        public IList<IList<int>> TransformStringListToReversedIntMatrix(IList<string> lNums, int blockSize = 1)
        {
            if(blockSize == 1)
                return _TransformStringListToReversedIntMatrix(lNums);
            
            Object obj = new Object();
            IList<IList<int>> matrix = new List<IList<int>>();

            Parallel.ForEach(lNums, ln => {
                var val = TransformStringtoReverseIntList(ln, blockSize);
                lock (obj)
                {
                    matrix.Add(val);
                }                
            });
            
            return matrix;
        }

        public IList<int> TransformStringtoReverseIntList(string number, int blockSize)
        {
            List<int> row = new List<int>();

            for (int i = number.Length; i > 0; i -= blockSize)
            {
                if (i - blockSize > 0)
                    row.Add(int.Parse(number.Substring(i - blockSize, blockSize)));
                else
                    row.Add(int.Parse(number.Substring(0, blockSize + (i - blockSize))));
            }
            return row;
        }

        private IList<IList<int>> _TransformStringListToReversedIntMatrix(IList<string> lNums)
        {
            IList<IList<int>> matrix = new List<IList<int>>();
            Object obj = new Object();
            Parallel.ForEach(lNums, ln => {
                var val = TransformStringtoReverseIntList(ln);
                lock (obj)
                {
                    matrix.Add(val); 
                }
            });

            return matrix;
        }

        public IList<int> TransformStringtoReverseIntList(string number)
        {
            List<int> row = new List<int>();

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

        public string ReverseString(string str, int blockSize)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = str.Length; i > 0; i -= blockSize)
            {
                if (i - blockSize > 0)
                    sb.Append(str.Substring(i - blockSize, blockSize));
                else
                    sb.Append(str.Substring(0, blockSize + (i - blockSize)));
            }
            return sb.ToString();
        }
    }
}
