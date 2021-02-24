using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class ArithmeticUtils
    {
        public void GetCarryBase10(ref int number, ref int opt, ref int carry)
        {
            opt = number % 10;
            carry = (number - opt) / 10;
        }
    }
}
