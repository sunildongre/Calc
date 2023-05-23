using System;

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

        public int BlockSize => 2;

        public int AdditionBlockSize => 10;

        public int Base10BlockDigitCount => (int)Math.Pow(10, BlockSize);
    }
}
