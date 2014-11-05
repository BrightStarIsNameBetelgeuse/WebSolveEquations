using System;

namespace TestEquatMvc.Models
{
    public class GaussStrategy : ISolveStrategy
    {

        private double[,] matrix;  // главная матрица
        private double[] results;   // вектор неизвестных
        private double[] vector;   // вектор b
        private double eps;          // порядок точности для сравнения вещественных чисел 
        private int size;            // размерность задачи


        public GaussStrategy(double[,] matrix, double[] b_vector)
            : this(matrix, b_vector, 0.0001) {
        }
        public GaussStrategy(double[,] matrix, double[] b_vector, double eps)
        {
            if (matrix == null || b_vector == null)
                throw new ArgumentNullException("One of the parameters is null.");

            int b_length = b_vector.Length;
            int a_length = matrix.Length;
            if (a_length != b_length * b_length)
                throw new ArgumentException(@"The number of rows and columns in the matrix A must match the number of elements in the vector B.");

            //this.initial_a_matrix = a_matrix;  // запоминаем исходную матрицу
            this.matrix = (double[,])matrix.Clone(); // с её копией будем производить вычисления
            //this.initial_b_vector = b_vector;  // запоминаем исходный вектор
            this.vector = (double[])b_vector.Clone();  // с его копией будем производить вычисления
            this.results = new double[b_length];
            //this.u_vector = new double[b_length];
            this.size = b_length;
            this.eps = eps;
        }

        public double[] Solve()
        {
            int[] index = InitIndex();
            GaussForwardStroke(index);
            GaussBackwardStroke(index);
            return results;
        }

        public bool IsSolve()
        {
            throw new NotImplementedException();
        }

        public double[] ResultsVector
        {
            get
            {
                return results;
            }
        }

        // инициализация массива индексов столбцов
        private int[] InitIndex()
        {
            int[] index = new int[size];
            for (int i = 0; i < index.Length; ++i)
                index[i] = i;
            return index;
        }

        // поиск главного элемента в матрице
        private double FindR(int row, int[] index)
        {
            int max_index = row;
            double max = matrix[row, index[max_index]];
            double max_abs = Math.Abs(max);
            //if(row < size - 1)
            for (int cur_index = row + 1; cur_index < size; ++cur_index)
            {
                double cur = matrix[row, index[cur_index]];
                double cur_abs = Math.Abs(cur);
                if (cur_abs > max_abs)
                {
                    max_index = cur_index;
                    max = cur;
                    max_abs = cur_abs;
                }
            }

            if (max_abs < eps)
            {
                if (Math.Abs(vector[row]) > eps)
                    throw new GaussSolveNotFound("The system has no solutions.");
                else
                    throw new GaussSolveNotFound("The system has many solutions.");
            }

            // меняем местами индексы столбцов
            int temp = index[row];
            index[row] = index[max_index];
            index[max_index] = temp;

            return max;
        }

        // Прямой ход метода Гаусса
        private void GaussForwardStroke(int[] index)
        {
            // перемещаемся по каждой строке сверху вниз
            for (int i = 0; i < size; ++i)
            {
                // 1) выбор главного элемента
                double r = FindR(i, index);

                // 2) преобразование текущей строки матрицы A
                for (int j = 0; j < size; ++j)
                    matrix[i, j] /= r;

                // 3) преобразование i-го элемента вектора b
                vector[i] /= r;

                // 4) Вычитание текущей строки из всех нижерасположенных строк
                for (int k = i + 1; k < size; ++k)
                {
                    double p = matrix[k, index[i]];
                    for (int j = i; j < size; ++j)
                        matrix[k, index[j]] -= matrix[i, index[j]] * p;
                    vector[k] -= vector[i] * p;
                    matrix[k, index[i]] = 0.0;
                }
            }
        }

        // Обратный ход метода Гаусса
        private void GaussBackwardStroke(int[] index)
        {
            // перемещаемся по каждой строке снизу вверх
            for (int i = size - 1; i >= 0; --i)
            {
                // 1) задаётся начальное значение элемента x
                double x_i = vector[i];

                // 2) корректировка этого значения
                for (int j = i + 1; j < size; ++j)
                    x_i -= results[index[j]] * matrix[i, index[j]];
                results[index[i]] = x_i;
            }
        }


    }

    public class GaussSolveNotFound : Exception
    {
        public GaussSolveNotFound(string msg)
            : base("Solution is not found: \r\n" + msg)
        {
        }
    }
}