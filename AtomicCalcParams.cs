using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Properties
{
    public class AtomicCalcParams
    {
        string Op1 { get; set; }
        string Op2 { get; set; }    
        Operator Op { get; set; }
    }
}
