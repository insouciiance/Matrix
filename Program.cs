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
                {1, 2, 3 },
                {4, 5, 6 },
                {7, 8, 9 }
            });

            Console.WriteLine(m1.Sum(m1));

            Console.ReadKey();
        }
    }
}
