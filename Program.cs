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
            Matrix m1 = new Matrix(new double[,]
            {
                {2, 4, 3, 2},
                {1, 3, 2, 5},
                {-1, 1, 1, 89}
            });

            Matrix m2 = new Matrix(new double[,]
            {
                {2, 1, -1, 3},
                {4, 3, 1, 8 },
                {3, 2, 1, 4},
            });

            Matrix m3 = new Matrix(new double[,]
            {
                {1, 2, 2 },
                {3, 4, 6 },
                {5, 3, 0 }
            });

            Console.WriteLine(m3.GetAdjugateMatrix());
            Console.WriteLine(m3.GetDeterminant());
            Console.WriteLine(m3.GetInverseMatrix());

            Console.ReadKey();
        }
    }
}
