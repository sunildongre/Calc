﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calc
{
    public class Int32Pair
    {
        public long Carry
        {
            get; set;
        }
        public long Opt
        {
            get; set;
        }
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
         *public void GetCarryBase10(ref long number, ref long opt, ref long carry)
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
        //lookup for calculations involving block size 2 or 1 only
        //else it will default to calculating the carry and output
        public void GetCarryBasseBlock2(ref long carry, long bn, long n, ref long y)
        {
            if (table != null)
            {
                var pair = table[carry, bn, n];
                carry = pair.Carry;
                y = pair.Opt;
            }
            else
            {
                var val = (bn * n) + carry;
                y = val % (long)ProgramConsts.Instance.Base10BlockDigitCount;
                carry = (val - y) / (long)ProgramConsts.Instance.Base10BlockDigitCount;
            }
        }


        // build the lookup tables
        // eventually move this out to a separate program
        // import the mods for all combinations in code as static
        private void fillMod10Table()
        {
            if (ProgramConsts.Instance.Base10BlockDigitCount <= 100)
            {
                table = new Int32Pair[(long)ProgramConsts.Instance.Base10BlockDigitCount,
                                                                    (long)ProgramConsts.Instance.Base10BlockDigitCount,
                                                                    (long)ProgramConsts.Instance.Base10BlockDigitCount];
                for (var h = 0; h < (long)ProgramConsts.Instance.Base10BlockDigitCount; h++)
                {
                    for (var i = 0; i < (long)ProgramConsts.Instance.Base10BlockDigitCount; i++)
                    {
                        for (var j = 0; j < (long)ProgramConsts.Instance.Base10BlockDigitCount; j++)
                        {
                            var mult = h + i * j;
                            var val = mult % (long)ProgramConsts.Instance.Base10BlockDigitCount;
                            table[h, i, j] = new Int32Pair() { Opt = val, Carry = (mult - val) / (long)ProgramConsts.Instance.Base10BlockDigitCount };
                        }
                    }
                }
            }
            else
                table = null;
        }

        // with block size 3 this is hard to build since we need to calculate 10^9 combinations to start...!
        // this is thousand times more than a block size 2 which is in absolute terms 10^6 
        private void fillMod10LTable()
        {
            for (var h = 0; h < (long)ProgramConsts.Instance.Base10BlockDigitCount; h++)
            {
                IList<IList<Int32Pair>> d1 = new List<IList<Int32Pair>>();
                for (var i = 0; i < (long)ProgramConsts.Instance.Base10BlockDigitCount; i++)
                {
                    IList<Int32Pair> d2 = new List<Int32Pair>();
                    for (var j = 0; j < (long)ProgramConsts.Instance.Base10BlockDigitCount; j++)
                    {
                        var mult = h + i * j;
                        var val = mult % (long)ProgramConsts.Instance.Base10BlockDigitCount;
                        var d3 = new Int32Pair() { Opt = val, Carry = (mult - val) / (long)ProgramConsts.Instance.Base10BlockDigitCount };
                        d2.Add(d3);
                    }
                    d1.Add(d2);
                }
                lTable.Add(d1);
            }
        }
        #endregion

        public void GetCarryBaseBlock(long number, ref long opt, ref long carry, int block)
        {
            opt = number % block;
            carry = (number - opt) / block;
        }

        public IDictionary<long, string> GetMultiples(string number)
        {
            LargeToOneNumberMultiplier lnm = new LargeToOneNumberMultiplier();
            IDictionary<long, string> multiples = new Dictionary<long, string>();
            var mLoc = new object();
            //0 and 10 are included to speed up multiplication
            // 1 - 9 to speed up division
            IList<long> ix = new List<long>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Parallel.ForEach(ix, (i, s, m) =>
            {
                var res = new KeyValuePair<long, string>(i, lnm.Compute(new List<string>() { number, i.ToString() }));
                lock (mLoc) { multiples.Add(res); }
            });
            return multiples;
        }


        public IDictionary<long, string> GetMultiples(string number, long tableLength)
        {
            LargeToOneNumberMultiplier lnm = new LargeToOneNumberMultiplier();
            IDictionary<long, string> multiples = new Dictionary<long, string>();
            var mLoc = new object();
            //0 and 10 are included to speed up multiplication
            // 1 - 9 to speed up division
            IList<long> ix = new List<long>();
            for (var i = 0; i < tableLength; i++)
                ix.Add(i);

            Parallel.ForEach(ix, (i, s, m) =>
            {
                var res = new KeyValuePair<long, string>(i, lnm.Compute(new List<string>() { number, i.ToString() }));
                lock (mLoc) { multiples.Add(res); }
            });
            return multiples;
        }

        public IList<string> GetMultiples(string number, int tableLength)
        {
            LargeToOneNumberMultiplier lnm = new LargeToOneNumberMultiplier();
            IList<string> multiples = new List<string>(new string[tableLength]);
            var mLoc = new object();
            //0 and 10 are included to speed up multiplication
            // 1 - 9 to speed up division
            IList<int> ix = new List<int>();
            for (var i = 0; i < tableLength; i++)
                ix.Add(i);

            Parallel.ForEach(ix, (i, s, m) =>
            {
                multiples[i] = lnm.Compute(new List<string>() { number, i.ToString() });
            });
            return multiples;
        }
    }
}
