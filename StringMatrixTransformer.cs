﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class StringMatrixTransformer
    {

        public long [][] TransformStringListToReversedIntArray(IList<string> lNums, int blockSize = 1)
        {
            var matrix = new long[lNums.Count][];
            Parallel.ForEach(lNums, (l, s, i) =>
            {
                matrix[i] = TransformStringtoReverseIntArray(l, blockSize);                
            });

            return matrix;
        }

        public long[] TransformStringtoReverseIntArray(string number, int blockSize)
        {
            var len = blockSize != 1 ? 
                    number.Length % blockSize == 0 ? 
                        (int)(number.Length / blockSize) : 
                        (int)(number.Length / blockSize) + 1 : 
                    number.Length;

            var row = new long[len];
            int pos = 0;
            for (var i = number.Length; i > 0; i -= blockSize)
            {
                if (i - blockSize > 0)
                    row[pos++] = StringTolong(number.Substring(i - blockSize, blockSize));
                else
                    row[pos++] = StringTolong(number.Substring(0, blockSize + (i - blockSize)));
            }
            return row;
        }

        public IList<IList<long>> TransformStringListToReversedIntMatrix(IList<string> lNums, int blockSize = 1)
        {
            if (blockSize == 1)
                return _TransformStringListToReversedIntMatrix(lNums);

            var obj = new Object();
            IList<IList<long>> matrix = new List<IList<long>>();

            Parallel.ForEach(lNums, ln =>
            {
                var val = TransformStringtoReverseIntList(ln, blockSize);
                lock (obj)
                {
                    matrix.Add(val);
                }
            });

            return matrix;
        }

        public IList<long> TransformStringtoReverseIntList(string number, int blockSize)
        {
            List<long> row = new List<long>();

            for (var i = number.Length; i > 0; i -= blockSize)
            {
                if (i - blockSize > 0)
                    row.Add(StringTolong(number.Substring(i - blockSize, blockSize)));
                else
                    row.Add(StringTolong(number.Substring(0, blockSize + (i - blockSize))));
            }
            return row;
        }

        private IList<IList<long>> _TransformStringListToReversedIntMatrix(IList<string> lNums)
        {
            IList<IList<long>> matrix = new List<IList<long>>();
            var obj = new Object();
            Parallel.ForEach(lNums, ln =>
            {
                var val = TransformStringtoReverseIntList(ln);
                lock (obj)
                {
                    matrix.Add(val);
                }
            });

            return matrix;
        }

        public IList<long> TransformStringtoReverseIntList(string number)
        {
            List<long> row = new List<long>();

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
            for (var j = str.Length - 1; j >= 0; j--)
            {
                sb.Append(str[j]);
            }
            return sb.ToString();
        }

        public string ReverseString(string str, int blockSize)
        {
            StringBuilder sb = new StringBuilder();

            for (var i = str.Length; i > 0; i -= blockSize)
            {
                if (i - blockSize > 0)
                    sb.Append(str.Substring(i - blockSize, blockSize));
                else
                    sb.Append(str.Substring(0, blockSize + (i - blockSize)));
            }
            return sb.ToString();
        }

        public long StringTolong(string s)
        {
            long y = 0;
            for (var i = 0; i < s.Length; i++)
                y = y * 10 + (s[i] - '0');
            return y;
        }
    }
}
