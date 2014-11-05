namespace TestEquatMvc.Models
{
    /// <summary>
    /// Class that implements the matrix
    /// </summary>
    public class Matrix
    {
        private int _row, _column;

        /// <summary>
        /// Count of rows
        /// </summary>
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        /// <summary>
        /// Count of columns
        /// </summary>
        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        private double[,] _matr;

        public Matrix()
        {
        }

        public void SetMatrix(double[,] matrix)
        {
            _row = matrix.GetLength(0);
            _column = _row;
            _matr = matrix;
        }

        private double CalcDeterm(double[,] matr)
        {
            if (matr.Length == 4) //when second-order matrix
            {
                return matr[0, 0] * matr[1, 1] - matr[0, 1] * matr[1, 0];
            }
            int sign = 1;
            double result = 0;
            for (int i = 0; i < matr.GetLength(1); i++)
            {
                double[,] minor = GetMinore(matr, i);
                result += sign * matr[0, i] * CalcDeterm(minor);
                sign = -sign;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matr">The initial matrix</param>
        /// <param name="n"> Dimension of the minor</param>
        /// <returns></returns>
        private double[,] GetMinore(double[,] matr, int n)
        {
            var result = new double[matr.GetLength(0) - 1, matr.GetLength(0) - 1];
            for (int i = 1; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < n; j++)
                    result[i - 1, j] = matr[i, j];
                for (int j = n + 1; j < matr.GetLength(0); j++)
                    result[i - 1, j - 1] = matr[i, j];
            }
            return result;
        }

        /// <summary>
        /// Get the determinant of the matrix
        /// </summary>
        /// <returns></returns>
        public double GetDeterminant()
        {
            return CalcDeterm(_matr);
        }

        public double GetDeterminant(double[,] matrix)
        {
            return CalcDeterm(matrix);
        }

        public double[,] GetMatrix()
        {
            return _matr;
        }
    }
}