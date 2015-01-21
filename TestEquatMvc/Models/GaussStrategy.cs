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
        private int count_vars;


        public GaussStrategy(double[,] matrix, double[] b_vector)
            : this(matrix, b_vector, 0.0001) {
        }
        public GaussStrategy(double[,] matrix, double[] b_vector, double eps)
        {
            if (matrix == null || b_vector == null)
                throw new ArgumentNullException("One of the parameters is null.");
            count_vars = matrix.GetLength(1);

            int b_length = b_vector.Length;
            int a_length = matrix.GetLength(1);
            if (a_length != b_length * b_length)
            {
                //    throw new ArgumentException(@"The number of rows and columns in the matrix A must match the number of elements in the vector B.");
                //int row = matrix.GetUpperBound(0);
                //if (a_length > b_length) 
                //{
                //    throw new ArgumentException(@"The number of rows and columns in the matrix A must match the number of elements in the vector B.");
                //}
                
                //else
                //{
                //    
                //    //добавляем нули
                //    
                double[,] dmatrix = new double[b_length, b_length];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        dmatrix[i, j] = matrix[i, j];
                    }
                }
                matrix = dmatrix;
                dmatrix = null;
                //}
            }

            //this.initial_a_matrix = a_matrix;  // запоминаем исходную матрицу
            this.matrix = (double[,])matrix.Clone(); // с её копией будем производить вычисления
            //this.initial_b_vector = b_vector;  // запоминаем исходный вектор
            this.vector = (double[])b_vector.Clone();  // с его копией будем производить вычисления
            this.results = new double[count_vars];
            //this.u_vector = new double[b_length];
            this.size = b_length;
            this.eps = eps;
        }

        public double[] Solve()
        {
            int[] index = InitIndex();
            GaussForwardStroke2();
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

        /// <summary>
        /// поменять местами вектора в матрице
        /// </summary>
        /// <param name="old_index">индекс старой строки</param>
        /// <param name="new_index">индекс новой строки</param>
        private void Replace(int old_index, int new_index)
        {
            for (int i = 0; i < size; i++)
            {
                double t = matrix[old_index, i];
                matrix[old_index, i] = matrix[new_index, i];
                matrix[new_index, i] = t;
            }
        }

        /// <summary>
        /// Занести вектор в матрицу
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="ind"></param>
        private void SetVectorInMatrix(int ind, double[] vector)
        {
            for (int i = 0; i < size; i++)
            {
                matrix[ind, i] = vector[i];
            }
        }

        /// <summary>
        /// Получить вектор из матрицы
        /// </summary>
        /// <param name="ind">индекс по вертикали</param>
        /// <returns></returns>
        private double[] GetVectorFromMatrix(int ind)
        {
            double[] vector = new double[matrix.GetLength(0)]; 
            for (int i = 0; i < size; i++)
            {
                vector[i] = matrix[ind, i];
            }

            return vector;
        }

        private void GaussForwardStroke2()
        {
            double[] main_vector = new double[size]; //главный вектор, который будет умножаться и вычитаться

            int n = 0; //номер строки в матрице
            int delta = 0;

            //выбираем главный вектор
            double main_el = matrix[n,n];
            main_vector = GetVectorFromMatrix(n + delta);
            while (main_el == 0)
            {
                delta++;
                Replace(n, n + delta);

                main_vector = GetVectorFromMatrix(n+delta);
                main_el = main_vector[n];
            }

            int numMainVector = 0;
            double b_val = vector[n];

            //анализ след векторов
            for (int i = 1; i < size; i++)
            {
                main_el = main_vector[numMainVector];
                //делим вектор на первый его элемент
                for (int id = 0; id < size; id++)
                {
                    main_vector[id] /= main_el;
                }

                b_val /= main_el;
                vector[n] = b_val;
                SetVectorInMatrix(numMainVector, main_vector);

                double tmp_b = vector[i];

                double[] tmp_vector = GetVectorFromMatrix(i);
                double m = tmp_vector[numMainVector];

                m = m * (-1);
                //вычитание
                for (int j = 0; j < size; j++)
                {
                    tmp_vector[j] = tmp_vector[j] + m * main_vector[j];
                }
                tmp_b += m * b_val;

                SetVectorInMatrix(i,tmp_vector);    //заменяем на новое значение
                vector[i] = tmp_b;

                if (i == (size - 1))
                {
                    //bool zero = false;
                    int counter = 0;

                    for (int k = 0; k < size; k++)
                    {
                        if (matrix[i, k] != 0.0)
                        {
                            counter++;
                        }
                    }
                    if (counter > 1)
                    {
                        i = 1; 
                    }
                }
                numMainVector++;
                main_vector = tmp_vector;   //делаем главным вектор следующий
            }
        }

        // Обратный ход метода Гаусса
        private void GaussBackwardStroke(int[] index)
        {
            // перемещаемся по каждой строке снизу вверх
            for (int i = size - 1; i >= 0; i--)
            {
                double tmp_el = matrix[i, i];   //нормализация строки
                if (tmp_el == 0)
                {
                    //нет решений или несовместна
                    throw new GaussSolveNotFound("Система уравнений не имеет решений или несовместна");
                }

                for (int j = i; j < size; j++)
                {
                    matrix[i, j] /= tmp_el;
                }
                vector[i] /= tmp_el;

                for (int j = i - 1; j >=0; j--)  //вычитаем нормализованную строку из остальных верхних
                {
                    tmp_el = matrix[j, i];
                    for (int k = i; k < size; k++)   //вычитаем одну строку из другой
                        matrix[j, k] -= matrix[i, k] * tmp_el;
                    vector[j] -= vector[i] * tmp_el;
                }
                results = vector;
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