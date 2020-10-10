using System;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Matrix.SolveLinearSystem(new Matrix(2, -1, 3, -5, 1, -1, -5, 0, 3, -2, -2, -5, 7, -5, -9, -10), new Matrix(1, 2, 3, 8)));

            Console.ReadKey();
        }
    }
}
