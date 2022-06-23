using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class Int32Pair
    {
        public int Carry { get; set; }
        public int Opt{get; set; }
    }

    public class ArithmeticUtils
    {
        private static readonly Int32Pair[,,] table = new Int32Pair[10,10,10];

        public ArithmeticUtils()
        {
            fillMod10Table();
        }

        #region get multipl and carry for a single number multiplication
        /* The original function was like GetCarryBase10 below 
         * 
         *public void GetCarryBase10(ref int number, ref int opt, ref int carry)
          {
            opt = number % 10;
            carry = (number - opt) / 10;
          }
         * below we want to replicate this logic, but cache the carry and operator combinations.
         * we can do that because carry will never be >= 10, in fact it will never be more than 8
         * since carry is at max 8 we can compute and cache all combinations of operands and carry 
         * in the loop 
         * h = carry
         * i, j = operands in any order. 
         * then we avoid a mod operation and a subtract and devide opertion per 
         * multiplication to get carry value and output value 
         * It is reduced to a lookup
         */
        public void GetCarryBase10forSingleMultiple(ref int carry, int bn, int n, ref int y)
        {
            var pair = table[carry, bn, n];
            carry = pair.Carry;
            y = pair.Opt;
        }

        private static void fillMod10Table()
        {
            for (var h = 0; h < 10; h++)
            {
                for (var i = 0; i < 10; i++)
                {
                    for (var j = 0; j < 10; j++)
                    {
                        var mult = h + i * j;
                        var val = mult % 10;
                        table[h, i, j] = new Int32Pair() { Opt = val, Carry = (mult - val) / 10 };
                    }
                }
            }
        }
        #endregion


        public void GetCarryBase10(ref int number, ref int opt, ref int carry)
        {
            opt = number % 10;
            carry = (number - opt) / 10;
        }

        public IDictionary<int, string> GetMultiples(string number)
        {
            LargeToOneNumberMultiplier lnm = new LargeToOneNumberMultiplier();
            IDictionary<int, string> multiples = new Dictionary<int, string>();
            var mLoc = new object();
            //0 and 10 are included to speed up multiplication
            // 1 - 9 to speed up division
            IList<int> ix = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };  

            Parallel.ForEach(ix, (i, s, m) =>
            {
                var res = new KeyValuePair<int, string>(i, lnm.Compute(new List<string>() {number, i.ToString()}));
                lock (mLoc) { multiples.Add(res); }
            });
            return multiples;
        }
    }
}
