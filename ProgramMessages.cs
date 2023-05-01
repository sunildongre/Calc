using System;

namespace Calc
{
    public class ProgramMessages
    {
        private static ProgramMessages _instance = null;

        public static ProgramMessages Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProgramMessages();
                }
                return _instance;
            }
        }
        public void PrintOperandFormatIncorrect()
        {
            System.Console.WriteLine("Numbers not in correct format.");
        }

        public void PrintHelpText()
        {
            Console.WriteLine("Run the application like this:");
            Console.WriteLine("<number 1> <operator> <number 2>");
            Console.WriteLine("where number 1 and number 2 are numeric operands total not exceeding 250 characters.");
            Console.WriteLine("You may also put the expression in a file file and invoke it as:");
            Console.WriteLine("-f <filename>");
            Console.WriteLine("and operator is one of a/s/m/d where:");
            Console.WriteLine("a -or- + : Addition");
            Console.WriteLine("s -or- - : Subtractoin");
            Console.WriteLine("m -or- x -or- *: Multiplication");
            Console.WriteLine("d -or- / : Division");
            Console.WriteLine("For example: Calc.exe 1234567890122546443 x 3659235987365457298");
            Console.WriteLine("or : Calc.exe 1234567833546443 / 4928345298");
            Console.WriteLine("or : Calc.exe -f multiply.txt");
            Console.WriteLine("where multiply.txt contains a string like 1234567833546443 * 4928345298");
        }
    }
}
