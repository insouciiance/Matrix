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
            Console.WriteLine(new Matrix(new double[,]
            {
                {5, 6, 5, 4, 4, 7, },
                {5, 7, 9, 9, 4, 4, },
                {9, 8, 6, 5, 0, 9, },
                {7, 7, 56, 5, 8, 9, },
                {4, 0, 0, 9, 7, 5, },
                {5, 9, 9, 8, 5, 4, },
                {3, 5, 5, 6, 6, 5 },
                {6, 4, 4, 5, 3, 6, },
                {87, 8, 8, 6, 5, 4 },
                {87, 8, 8, 6, 5, 4 },
            }).GetRank());

            Console.ReadKey();
        }
    }
}
