using System;
using System.Collections.Generic;

namespace Calc
{
    public class LargeNumberDivider : ILargeNumberComputer
    {
        private bool _decemalInQuotient = false;

        public string Compute(IList<string> numbers)
        {
            if (numbers.Count != 2)
            {
                throw new Exception("Division needs 2 numbers. First divided by the Second. Numbers found:" + numbers.Count);
            }

            NumercStringUtils nsu = new NumercStringUtils();

            string n = numbers[0];
            string d = numbers[1];

            if (nsu.IsZeroString(n))
                return "0";
            else if (nsu.IsZeroString(d))
                throw new DivideByZeroException();

            if (n == d)
                return "1";
            
            // assumption n > d 
            return DoDivision(n, d);
        }

        

        private string DoDivision(string n, string d)
        {
            NumercStringUtils nsu = new NumercStringUtils();
            int i = d.Length;
            string ni = n.Substring(0, i);

            if (!nsu.OneGreaterThanTwo(ni, d))
            {
                ni = n.Substring(i, ++i);
            }

            for(int j = 1; j < 10; j++)
            {

            }

            if (nsu.OneGreaterThanTwo(n, d))
            {
                //answer > 1
                
                //if(n.Length - d.Length > 1)
                //{ Find}

            }
            else
            {
                //answer < 1
                // lets deal with it later
            }

            return "";
        }
    }
}
