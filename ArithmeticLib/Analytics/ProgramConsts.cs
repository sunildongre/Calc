using System;

namespace ArithmeticLib
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

        public int BlockSize => 5;

        public int AdditionBlockSize => 15;
        
        public int Base10AdditionBlockDigitCount => (int)Math.Pow(10, AdditionBlockSize);

        public int Base10BlockDigitCount => (int)Math.Pow(10, BlockSize);
    }
}
