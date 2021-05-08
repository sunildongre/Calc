using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return DoDivision(n, d);
        }

        

        private string DoDivision(string n, string d)
        {
            NumercStringUtils nsu = new NumercStringUtils();

            if (nsu.OneGreaterThanTwo(n, d))
            {
                //answer > 1
            }
            else
            {
                //answer < 1
            }

            return "";
        }
    }
}
