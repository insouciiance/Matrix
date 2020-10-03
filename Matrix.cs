using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Matrix
    {
        private readonly double[,] _matrix;

        public int Rows => _matrix.GetLength(0);
        public int Columns => _matrix.GetLength(1);
        public bool IsMatrixSquare => Rows == Columns;

        public Matrix(double[,] matrix)
        {
            _matrix = matrix ?? new double[0, 0];
        }

        public double GetDeterminant()
        {
            if (!IsMatrixSquare)
            {
                throw new Exception("Matrix was not square");
            }

            int[][] permutations = GetPermutations(Columns);
            int firstIndex = 0;

            double determinant = 0;

            for (int i = 0; i < permutations.Length; i++)
            {
                double addition = Math.Pow(-1, GetInversions(permutations[i]));

                for (int j = 0; j < permutations[i].Length; j++)
                {
                    addition *= _matrix[firstIndex, permutations[i][j]];
                    firstIndex += 1;
                }

                determinant += addition;
                firstIndex = 0;
            }

            return determinant;

            int GetInversions(int[] array)
            {
                if (array.Length == 1) return 0;

                int inversionsCount = 0;

                for (int i = 0; i < array.Length; i++)
                {
                    for (int j = i + 1; j < array.Length; j++)
                    {
                        if (array[i] > array[j])
                        {
                            inversionsCount++;
                        }
                    }
                }

                return inversionsCount;
            }

            int[][] GetPermutations(int indexesCount)
            {
                int[][] permutationsArray = new int[Factorial(indexesCount)][];
                int currentArrayIndex = 0;

                int[] arrayToPermute = new int[indexesCount];

                for (int i = 0; i < indexesCount; i++)
                {
                    arrayToPermute[i] = i;
                }

                void Swap(int index1, int index2)
                {
                    int temp = arrayToPermute[index1];
                    arrayToPermute[index1] = arrayToPermute[index2];
                    arrayToPermute[index2] = temp;
                }

                void Permute(int num)
                {
                    int[] permutedArray = new int[indexesCount];

                    if (num == 1)
                    {
                        Array.Copy(arrayToPermute, permutedArray, indexesCount);
                        permutationsArray[currentArrayIndex++] = permutedArray;
                    }
                    else
                    {
                        for (int i = 0; i < num; i++)
                        {
                            Permute(num - 1);
                            Swap(num % 2 == 0 ? 0 : i, num - 1);
                        }
                    }
                }

                int Factorial(int x) => x == 0 ? 1 : Factorial(x - 1) * x;

                Permute(indexesCount);

                return permutationsArray;
            }
        }

        public Matrix Sum(Matrix other)
        {
            return Sum(this, other);
        } 

        public static Matrix Sum(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
            {
                throw new Exception("Matrix did not match sum rules.");
            }

            double[,] resultMatrix = new double[m1.Columns, m1.Rows];

            for (int i = 0; i < m1.Rows; i++)
            {
                double[] firstMatrixRow = GetMatrixRow(m1, i);
                double[] secondMatrixRow = GetMatrixRow(m2, i);

                double[] sumArray = SumArrays(firstMatrixRow, secondMatrixRow);

                for (int j = 0; j < sumArray.Length; j++)
                {
                    resultMatrix[i, j] = sumArray[j];
                }
            }

            return new Matrix(resultMatrix);
        }

        public Matrix Multiply(Matrix other)
        {
            return Multiply(this, other);
        }

        public static Matrix Multiply(Matrix m1, Matrix m2)
        {
            if (m1.Columns != m2.Rows)
            {
                throw new Exception("Matrix did not match multiplication rules.");
            }

            double[,] resultMatrix = new double[m1.Rows, m2.Columns];

            for (int i = 0; i < resultMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < resultMatrix.GetLength(1); j++)
                {
                    double[] firstMatrixRow = GetMatrixRow(m1, i);
                    double[] secondMatrixColumn = GetMatrixColumn(m2, j);

                    double[] multipliedArray = MultiplyArrays(firstMatrixRow, secondMatrixColumn);
                    double multipliedArraySum = multipliedArray.Sum();

                    resultMatrix[i, j] = multipliedArraySum;
                }
            }

            return new Matrix(resultMatrix);
        }

        private static double[] GetMatrixRow(Matrix m, int rowIndex)
        {
            double[,] matrix = m.GetMatrix();
            double[] res = new double[m.Columns];

            for (int j = 0; j < m.Columns; j++)
            {
                res[j] = matrix[rowIndex, j];
            }

            return res;
        }

        private static double[] GetMatrixColumn(Matrix m, int columnIndex)
        {
            double[,] matrix = m.GetMatrix();
            double[] res = new double[m.Rows];

            for (int i = 0; i < m.Rows; i++)
            {
                res[i] = matrix[i, columnIndex];
            }

            return res;
        }

        private static double[] SumArrays(double[] array1, double[] array2)
        {
            if (array1.Length != array2.Length)
            {
                throw new Exception("Arrays did not match sum rules.");
            }

            double[] res = new double[array1.Length];

            for (int i = 0; i < res.Length; i++)
            {
                res[i] = array1[i] + array2[i];
            }

            return res;
        }

        private static double[] MultiplyArrays(double[] array1, double[] array2)
        {
            if (array1.Length != array2.Length)
            {
                throw new Exception("Arrays did not match multiplication rules.");
            }

            double[] res = new double[array1.Length];

            for (int i = 0; i < res.Length; i++)
            {
                res[i] = array1[i] * array2[i];
            }

            return res;
        }

        public Matrix PowerOf(int pow)
        {
            if (!IsMatrixSquare)
            {
                throw new Exception("Matrix was not square");
            }
            if (pow == 1) return new Matrix(GetMatrix());
            return Multiply(PowerOf(pow - 1), new Matrix(GetMatrix()));
        }

        public double[,] GetMatrix()
        {
            int columnsCount = _matrix.GetLength(0);
            int rowsCount = _matrix.GetLength(1);
            double[,] copy = new double[columnsCount, rowsCount];

            for (int i = 0; i < columnsCount; i++)
            {
                for (int j = 0; j < rowsCount; j++)
                {
                    copy[i, j] = _matrix[i, j];
                }
            }

            return copy;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                sb.Append($"{"(",-3}");

                for (int j = 0; j < _matrix.GetLength(1); j++)
                {
                    sb.Append($"{_matrix[i, j],-3} ");
                }

                sb.Append(")\n");
            }

            return sb.ToString();
        }
    }
}
