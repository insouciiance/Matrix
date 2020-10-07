using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix;
using System.Numerics;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Matrix.GetRank(new Matrix(1, 3, 5, -1, 2, -1, -3, 4, 5, 1, -1, 7, 7, 7, 9, 1)));

            Console.ReadKey();
        }
    }
}
