using System.Collections.Generic;

namespace Calc.Interface
{
    public interface ILargeNumberComputer
    {
        string Compute(IList<string> numbers);

        //string Compute(long[][] numbers);

        long[] Compute(long[][] numbers);
    }
}
