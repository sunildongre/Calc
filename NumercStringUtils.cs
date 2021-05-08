using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class NumercStringUtils
    {
        public bool IsStringNumeric(string str)
        {
            var arr = str.ToCharArray();
            for(int i = 0; i < arr.Length; i++)
            {
                if (!(arr[i] >= '0' && arr[i] <= '9'))
                    return false;
            }
            return true;
        }
        public string TrimLeadingZeros(string str)
        {
            var arr = str.ToCharArray();
            int i = 0;
            while (i <= arr.Length && arr[i] == '0')
            {
                i++;
            }

            if (i == 0)
            {
                return str;
            } 
            else if(i == str.Length + 1)
            {
                return "0";
            } 
            else
            {
                return str.Substring(i);
            }
        }

        public bool IsZeroString(string str)
        {
            return TrimLeadingZeros(str) == "0" ? true : false;
        }

        public bool OneGreaterThanTwo(string n, string d)
        {
            if (n.Length > d.Length)
            {
                return true;
            }
            else if (n.Length < d.Length)
            {
                return false;
            }
            else if (n.Length == d.Length)
            {
                var narr = n.ToCharArray();
                var darr = d.ToCharArray();
                for (int i = 0; i < narr.Length; i++)
                {
                    if (narr[i] > darr[i])
                    {
                        return true;
                    }
                    else if (darr[i] > narr[i])
                    {
                        return false;
                    }
                    else if (narr[i] == darr[i])
                    {
                        continue;
                    }
                }
                //should never reach here, n==d check above should have handled this
                return false;
            }
            return false;
        }
    }
}
