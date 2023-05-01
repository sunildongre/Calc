using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class ProgramConsts
    {
        private static ProgramConsts _instance = null;

        public static ProgramConsts Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProgramConsts();
                }
                return _instance;
            }
        }

        public int BlockSize => 3;

        public int AdditionBlockSize => 6;

        public int Base10BlockDigitCount => (int)Math.Pow(10, BlockSize);
    }
}
