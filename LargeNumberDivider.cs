using System;
using System.Collections.Generic;

namespace Calc
{
    public class LargeNumberDivider : ILargeNumberComputer
    {
        private bool _decemalInQuotient = false;
        private List<string> multiples = new List<string>();
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
            for (int j = 1; j <= 10; j++)
            {
                multiples.Add(lnm.Compute(new List<string>() { j.ToString(), number }));
            }
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
            //else if(remainder == "0")
            //{ 
            //    return remainder;
            //}
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


                for (int i = 0; i < 10; i++)
                {
                    //if(ni.Equals(multiples[i]))
                    if (nsu.OneEqualToTwo(ni, multiples[i]))
                    {
                        remainder = "0";
                        q += (i + 1).ToString(); // exact match so we need that multiple
                        break;
                    }
                    else if (nsu.OneGreaterThanTwo(multiples[i], ni))
                    {
                        remainder = sb.Compute(new List<string>() { ni, multiples[i - 1] });
                        q += i.ToString(); // found larger multiple, so picki, the one before for successful subtraction
                        break;
                    }
                }

                ni = nsu.TrimLeadingZeros(GetNextDividendString(remainder, d));
            } while (ni != null && !ni.Equals(remainder));

            return q;
        }
    }
}
