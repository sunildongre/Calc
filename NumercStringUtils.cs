using System.Collections.Generic;

namespace Calc
{
    public class NumercStringUtils
    {

        public bool IsStringNumeric(string str)
        {
            var arr = str.ToCharArray();
            for (long i = 0; i < arr.Length; i++)
            {
                if (!(arr[i] >= '0' && arr[i] <= '9'))
                    return false;
            }
            return true;
        }

        public bool IsZeroString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '0')
                    return false;
            }
            return true;
        }

        public string TrimLeadingZeros(string str)
        {
            if (str == null)
                return str;

            var arr = str.ToCharArray();
            int i = 0;
            while (i < arr.Length && arr[i] == '0')
            {
                i++;
            }

            if (i == 0)
            {
                return str;
            }
            else if (i == str.Length + 1)
            {
                return "0";
            }
            else
            {
                return str.Substring(i);
            }
        }

        public bool OneEqualToTwo(string one, string two)
        {
            var len = one.Length > two.Length ? two.Length : one.Length;

            for (int i = 0; i < len; i++)
            {
                if (one[i] != two[i]) return false;
            }
            return true;
        }

        public bool OneGreaterThanTwo(string one, string two)
        {
            if (OneEqualToTwo(one, two))
                return false;
            else if (IsZeroString(one) && !IsZeroString(two))
                return false;
            else if (!IsZeroString(one) && IsZeroString(two))
                return true;
            else if (!IsZeroString(one) && !IsZeroString(two))
            {
                var o = TrimLeadingZeros(one);
                var t = TrimLeadingZeros(two);
                if (o.Length > t.Length)
                {
                    return true;
                }
                else if (o.Length < t.Length)
                {
                    return false;
                }
                else if (o.Length == t.Length)
                {
                    var oArr = o.ToCharArray();
                    var tArr = t.ToCharArray();
                    for (long i = 0; i < oArr.Length; i++)
                    {
                        if (oArr[i] > tArr[i])
                        {
                            return true;
                        }
                        else if (oArr[i] < tArr[i])
                        {
                            return false;
                        }
                        else if (oArr[i] == tArr[i])
                        {
                            continue;
                        }
                    }
                    return false;
                    //should never reach here, n==d check above should have handled this
                }
                else return false;
            }
            else return false;
        }

        public bool OneGreaterThanOrEqualToTwo(string one, string two)
        {
            return OneGreaterThanTwo(one, two) ? true : one.Equals(two);
        }

        // refactor to use Parallel.ForEach
        public IList<string> GetMultiples(string number, long multiples)
        {
            LargeNumberMultiplier lm = new LargeNumberMultiplier();
            IList<string> results = new List<string>();

            for (long i = 1; i <= multiples; i++)
            {
                results.Add(lm.Compute(new List<string>() { number, i.ToString() }));
            }
            return results;
        }
    }
}
