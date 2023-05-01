using System.Collections.Generic;

namespace Calc
{
    public interface ILargeNumberComputer
    {
        string Compute(IList<string> numbers);
    }
}
