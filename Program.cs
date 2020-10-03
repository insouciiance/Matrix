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
                {23, 23},
                {5, 22 },
            });

            Console.WriteLine(m1.PowerOf(100));
            Console.WriteLine(m1.PowerOf(100).GetDeterminant());

            Console.ReadKey();
        }
    }
}
