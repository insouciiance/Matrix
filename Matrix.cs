using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
            if (matrix == null)
            {
                _matrix = new double[0, 0];
                return;
            }

            int matrixRows = matrix.GetLength(0);
            int matrixColumns = matrix.GetLength(1);

            double[,] copy = new double[matrixRows, matrixColumns];

            for (int i = 0; i < matrixRows; i++)
            {
                for (int j = 0; j < matrixColumns; j++)
                {
                    copy[i, j] = matrix[i, j];
                }
            }

            _matrix = copy;
        }

        public double GetDeterminant()
        {
            return GetDeterminant(this);
        }

        public static double GetDeterminant(Matrix m)
        {
            if (!m.IsMatrixSquare)
            {
                throw new Exception("Matrix was not square");
            }

            if (m.Rows == 0)
            {
                return 0;
            }

            double[,] matrix = m.GetMatrix();

            int[][] permutations = GetPermutations(m.Columns);
            int firstIndex = 0;

            double determinant = 0;

            for (int i = 0; i < permutations.Length; i++)
            {
                double addition = Math.Pow(-1, GetInversions(permutations[i]));

                for (int j = 0; j < permutations[i].Length; j++)
                {
                    addition *= matrix[firstIndex, permutations[i][j]];
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

            double[,] resultMatrix = new double[m1.Rows, m1.Columns];

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

                    resultMatrix[i, j] = Math.Round(multipliedArraySum, 2);
                }
            }

            return new Matrix(resultMatrix);
        }

        public Matrix Multiply(double amount)
        {
            return Multiply(this, amount);
        }

        public static Matrix Multiply(Matrix m, double amount)
        {
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            double[,] matrix = m.GetMatrix();

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    matrix[i, j] = amount * matrix[i, j];
                }
            }

            return new Matrix(matrix);
        }

        private static double[] GetMatrixRow(Matrix m, int rowIndex)
        {
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            if (rowIndex >= m.Rows || rowIndex < 0)
            {
                throw new Exception("\"rowIndex\" was outside matrix' bounds");
            }

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
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            if (columnIndex >= m.Columns || columnIndex < 0)
            {
                throw new Exception("\"columnIndex\" was outside matrix' bounds");
            }

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
            if (array1 == null || array2 == null)
            {
                throw new Exception("Array was null");
            }

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
            if (array1 == null || array2 == null)
            {
                throw new Exception("Array was null");
            }

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
            return PowerOf(this, pow);
        }

        public static Matrix PowerOf(Matrix m, int pow)
        {
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            if (!m.IsMatrixSquare)
            {
                throw new Exception("Matrix was not square");
            }

            if (pow < 1)
            {
                throw new Exception("Matrix' power can not be less than 1");
            }

            if (pow == 1) return new Matrix(m.GetMatrix());

            return Multiply(PowerOf(m, pow - 1), new Matrix(m.GetMatrix()));
        }

        public Matrix GetInverseMatrix()
        {
            return GetInverseMatrix(this);
        }

        public static Matrix GetInverseMatrix(Matrix m)
        {
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            if (!m.IsMatrixSquare)
            {
                throw new Exception("Matrix was not square");
            }

            Matrix adjugateMatrix = m.GetAdjugateMatrix();
            Matrix inverseMatrix = adjugateMatrix.Multiply(1 / m.GetDeterminant());

            return inverseMatrix;
        }

        public Matrix GetAdjugateMatrix()
        {
            return GetAdjugateMatrix(this);
        }

        public static Matrix GetAdjugateMatrix(Matrix m)
        {
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            double[,] matrix = m.GetMatrix();

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    matrix[i, j] = m.GetAlgebraicComplement(i, j);
                }
            }

            return new Matrix(matrix).Transpose();
        }

        public Matrix Transpose()
        {
            return Transpose(this);
        }

        public static Matrix Transpose(Matrix m)
        {
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            double[,] transposedMatrix = new double[m.Columns, m.Rows];
            double[,] matrixToTranspose = m.GetMatrix();

            for (int i = 0; i < transposedMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < transposedMatrix.GetLength(1); j++)
                {
                    transposedMatrix[i, j] = matrixToTranspose[j, i];
                }
            }

            return new Matrix(transposedMatrix);
        }

        public double GetAlgebraicComplement(int rowIndex, int columnIndex)
        {
            return GetAlgebraicComplement(this, rowIndex, columnIndex);
        }

        public static double GetAlgebraicComplement(Matrix m, int rowIndex, int columnIndex)
        {
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            if (rowIndex >= m.Rows || columnIndex >= m.Columns)
            {
                throw new Exception("Row and/or column index was outside the matrix' bounds");
            }

            return Math.Pow(-1, rowIndex + columnIndex) * GetMinor(m, rowIndex, columnIndex);
        }

        public double GetMinor(int rowIndex, int columnIndex)
        {
            return GetMinor(this, rowIndex, columnIndex);
        }

        public static double GetMinor(Matrix m, int rowIndex, int columnIndex)
        {
            if (m == null)
            {
                throw new Exception("Matrix was null");
            }

            if (rowIndex >= m.Rows || columnIndex >= m.Columns)
            {
                throw new Exception("Row and/or column index was outside the matrix' bounds");
            }

            double[,] initialMatrix = m.GetMatrix();
            int rowsCount = initialMatrix.GetLength(0);
            int columnsCount = initialMatrix.GetLength(1);

            double[,] resultMatrix = new double[rowsCount - 1, columnsCount - 1];
            int rowsCounter = 0;
            int columnsCounter = 0;


            for (int i = 0; i < rowsCount; i++)
            {
                if (i == rowIndex)
                {
                    continue;
                }

                for (int j = 0; j < columnsCount; j++)
                {
                    if (j == columnIndex)
                    {
                        continue;
                    }

                    resultMatrix[rowsCounter, columnsCounter] = initialMatrix[i, j];
                    columnsCounter++;
                }

                rowsCounter++;
                columnsCounter = 0;
            }

            return GetDeterminant(new Matrix(resultMatrix));
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
