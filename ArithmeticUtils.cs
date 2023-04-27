using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private static ArithmeticUtils _instance;
        private static Int32Pair[,,] table = null;
        private static Dictionary<string, Int32Pair> getCarryBase10BlockCarryDict = new Dictionary<string, Int32Pair>();

        // attemptedusing lists, turns out it is slower than arrays :-) 
        // no surprises there
        private static readonly IList<IList<IList<Int32Pair>>> lTable = new List<IList<IList<Int32Pair>>>();
           
        public static ArithmeticUtils Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ArithmeticUtils();
                }
                return _instance;
            }
        }

        private ArithmeticUtils()
        {
            var dt = DateTime.Now;
            fillMod10Table();
            //fillMod10LTable();

            CalcLogger.Instance.DebugConsoleLogLine("Filling up carry look up table took: " + (DateTime.Now - dt).TotalMilliseconds + " ms");
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

        private static object loc = new object();
        public void GetCarryBase10forSingleMultiple(ref int carry, int bn, int n, ref int y)
        {
            if (table != null)
            {
                var pair = table[carry, bn, n];
                //var pair = lTable.ElementAt(carry).ElementAt(bn).ElementAt(n);

                carry = pair.Carry;
                y = pair.Opt;
            }
            else
            {
                var key = bn.ToString() + "_" + n.ToString() + "_" + carry.ToString();
                Int32Pair c = null;
                if (getCarryBase10BlockCarryDict.TryGetValue(key, out c))
                {
                    carry = c.Carry;
                    y = c.Opt;
                    return;
                }

                var val = (bn * n) + carry;
                y = val % (int)ProgramConsts.Instance.Base10BlockDigitCount;
                carry = val - y;

                lock (loc)
                {
                    getCarryBase10BlockCarryDict.Add(key, new Int32Pair() { Carry = carry, Opt = y });
                }
            }
        }


        // build the lookup tables
        // eventually move this out to a separate program
        // import the mods for all combinations in code as static
        private void fillMod10Table()
        {
            if (ProgramConsts.Instance.Base10BlockDigitCount <= 100)
            {
                table = new Int32Pair[(int)ProgramConsts.Instance.Base10BlockDigitCount,
                                                                    (int)ProgramConsts.Instance.Base10BlockDigitCount,
                                                                    (int)ProgramConsts.Instance.Base10BlockDigitCount];
                for (var h = 0; h < (int)ProgramConsts.Instance.Base10BlockDigitCount; h++)
                {
                    for (var i = 0; i < (int)ProgramConsts.Instance.Base10BlockDigitCount; i++)
                    {
                        for (var j = 0; j < (int)ProgramConsts.Instance.Base10BlockDigitCount; j++)
                        {
                            var mult = h + i * j;
                            var val = mult % (int)ProgramConsts.Instance.Base10BlockDigitCount;
                            table[h, i, j] = new Int32Pair() { Opt = val, Carry = (mult - val) / (int)ProgramConsts.Instance.Base10BlockDigitCount };
                        }
                    }
                }
            }
            else
                table = null;
        }

        private void fillMod10LTable()
        {
            for (var h = 0; h < (int)ProgramConsts.Instance.Base10BlockDigitCount; h++)
            {
                IList<IList<Int32Pair>> d1 = new List<IList<Int32Pair>>();
                for (var i = 0; i < (int)ProgramConsts.Instance.Base10BlockDigitCount; i++)
                {
                    IList<Int32Pair> d2 = new List<Int32Pair>();
                    for (var j = 0; j < (int)ProgramConsts.Instance.Base10BlockDigitCount; j++)
                    {
                        var mult = h + i * j;
                        var val = mult % (int)ProgramConsts.Instance.Base10BlockDigitCount;
                        var d3 = new Int32Pair() { Opt = val, Carry = (mult - val) / (int)ProgramConsts.Instance.Base10BlockDigitCount };
                        d2.Add(d3);
                    }
                    d1.Add(d2);
                }
                lTable.Add(d1);
            }
        }
        #endregion


        public void GetCarryBase10(ref int number, ref int opt, ref int carry)
        {
            opt = number % 10;
            carry = (number - opt) / 10;
        }

        public void GetCarryBaseBlock(ref int number, ref int opt, ref int carry, int block)
        {
            opt = number % block;
            carry = (number - opt) / block;
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


        public IDictionary<int, string> GetMultiples(string number, int tableLength)
        {
            LargeToOneNumberMultiplier lnm = new LargeToOneNumberMultiplier();
            IDictionary<int, string> multiples = new Dictionary<int, string>();
            var mLoc = new object();
            //0 and 10 are included to speed up multiplication
            // 1 - 9 to speed up division
            IList<int> ix = new List<int>();
            for (var i = 0; i < tableLength; i++)
                ix.Add(i);

            Parallel.ForEach(ix, (i, s, m) =>
            {
                var res = new KeyValuePair<int, string>(i, lnm.Compute(new List<string>() { number, i.ToString() }));
                lock (mLoc) { multiples.Add(res); }
            });
            return multiples;
        }
    }
}
