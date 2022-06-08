﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Calc
{
    public class LargeNumberDivider : ILargeNumberComputer
    {
        private bool _decemalInQuotient = false;
        private IDictionary<long, string> multiples = new Dictionary<long, string>();
        NumercStringUtils nsu = new NumercStringUtils();
        int nPos = 0;
        string n, d, q;

        public string Compute(IList<string> numbers)
        {
            if (numbers.Count != 2)
            {
                throw new Exception("Division needs 2 numbers. First divided by the Second. Numbers found:" + numbers.Count);
            }            

            n = numbers[0];
            d = numbers[1];

            if (nsu.IsZeroString(n))
                return "0";
            else if (nsu.IsZeroString(d))
                throw new DivideByZeroException();

            if (n == d)
                return "1";

            // assumption n > d 
            BuildMultiples(d);
            return DoDivision(n, d);
        }

        private void BuildMultiples(string number)
        {
            LargeNumberMultiplier lnm = new LargeNumberMultiplier();
            object mLoc = new object();
            IList<int> ix = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Parallel.ForEach(ix, (i, s, m) =>
            {
                var res = new KeyValuePair<long, string>(i, lnm.Compute(new List<string>() { i.ToString(), number }));
                lock (mLoc) { multiples.Add(res); }
            });
        }


        string GetNextDividendString(string remainder, string divisor)
        {
            if (nPos >= n.Length)
                return null;
            if(remainder == null)
            {
                var n1 = n.Substring(0, d.Length);
                
                if(nsu.OneGreaterThanOrEqualToTwo(n1, d))
                {
                    nPos = d.Length;
                    return n1;
                }
                else
                {
                    nPos = d.Length + 1;
                    return n.Substring(0, d.Length + 1);
                }
            }
            else
            {
                if(nsu.OneGreaterThanTwo(remainder, d))
                {
                    throw new Exception("Remainder grater than denominator, you could have gone at least 1 better");
                }
                else
                {
                    bool dq = false;
                    do
                    {
                        remainder += n.Substring(nPos++, 1);

                        if (dq) 
                            q += "0";

                        dq = true;
                    } while (nsu.OneGreaterThanTwo(d, remainder));

                    return remainder;
                }
            }
        }

        private string DoDivision(string n, string d)
        {
            NumercStringUtils nsu = new NumercStringUtils();
            ILargeNumberComputer sb = new LargeNumberSubtractor();
            string ni = GetNextDividendString(null, d);
            string remainder = "";
            do
            {
                if (nsu.IsZeroString(ni)) break;

                for (int i = 1; i <= 10; i++)
                {
                    if (nsu.OneEqualToTwo(ni, multiples[i]))
                    {
                        remainder = "0";
                        q += i.ToString(); // exact match so we need that multiple
                        break;
                    }
                    else if (nsu.OneGreaterThanTwo(multiples[i], ni))
                    {
                        remainder = sb.Compute(new List<string>() { ni, multiples[i - 1] });
                        q += (i - 1).ToString(); // found larger multiple, so picki, the one before for successful subtraction
                        break;
                    }
                }

                ni = nsu.TrimLeadingZeros(GetNextDividendString(remainder, d));
            } while (ni != null && !ni.Equals(remainder));

            return q;
        }
    }
}
