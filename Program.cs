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
                {3, -2 },
                {5, -4 }
            });

            Matrix m2 = new Matrix(new double[,]
            {
                {-1, 2 },
                {-5, 6 },
            });

            Matrix m3 = new Matrix(new double[,]
            {
                {1, 1 },
                {0, 1 }
            });


            Console.WriteLine(m3.GetInverseMatrix());
            Console.WriteLine(m3.PowerOf(2).Sum(m3.Multiply(-4)).Sum(m3.GetInverseMatrix().Multiply(2)));

            Console.WriteLine(m1.GetInverseMatrix());
            Console.WriteLine(m2.Multiply(m1.GetInverseMatrix()));

            Console.ReadKey();
        }
    }
}
