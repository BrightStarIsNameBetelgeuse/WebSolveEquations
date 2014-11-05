using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestEquatMvc.Models
{
    public class CramerStrategy : ISolveStrategy
    {
        /// <summary>
        /// свободные коэффициенты
        /// </summary>
        public double[] Coefs
        {
            set { _coefs = value; }
        }

        
        /// <summary>
        /// Матрица уравнений
        /// </summary>
        private readonly Matrix _matrixEquat;
        private double[] _determs; //массив определителей
        private double[] _coefs; //массив коэффициентов b

        public CramerStrategy(double[,] matrix, double[] coefs)
        {
            _matrixEquat = new Matrix();
            _matrixEquat.SetMatrix(matrix);
            Coefs = coefs;
        }

        public double[] Solve()
        {
            var determ = _matrixEquat.GetDeterminant();
            InitArrayDeterms();

            double chcksum = 0;
            var result = new double[_matrixEquat.GetMatrix().GetLength(0)];
            for (var i = 0; i < _matrixEquat.Column; i++)
            {
                if (determ == 0)
                {
                        if (result[i] >= 0)
                            chcksum += _determs[i];
                        else chcksum -= _determs[i];
                    }
                    if ((determ == 0) && (chcksum == 0))
                    {
                        throw new CramerSolveNotFound("The system has many solutions.");
                    }
                    if ((determ == 0) && (chcksum != 0))
                    {
                        throw new CramerSolveNotFound("The system has no solutions.");
                    }
                    //return result;
                }
                for (var i = 0; i < _matrixEquat.Column; i++)
                {
                    result[i] = (double)_determs[i] / determ;
                }
                return result;
            }

            public bool IsSolve()
            {
                if (_matrixEquat.Column == _matrixEquat.Row)
                    return true;
                return false;
            }

            /// <summary>
            /// Заменить n-й столбец на массив коэффициентов b 
            /// </summary>
            /// <param name="n">номер столбца, который заменяется</param>
            /// <return>Result matrix</return>
            private double[,] Replace(int n)
            {
                var tmpMatrix = new double[_matrixEquat.GetMatrix().GetLength(0), _matrixEquat.GetMatrix().GetLength(0)];
                for (int i = 0; i < _matrixEquat.GetMatrix().GetLength(0); i++)
                {
                    for (int j = 0; j < _matrixEquat.GetMatrix().GetLength(0); j++)
                    {
                        tmpMatrix[i, j] = _matrixEquat.GetMatrix()[i, j];
                    }
                }
                for (int i = 0; i < tmpMatrix.GetLength(0); i++)
                {
                    if (i == n) //если дошли до нужного столбца, заменяем 
                    {
                        for (int j = 0; j < tmpMatrix.GetLength(0); j++)
                        {
                            tmpMatrix[j, i] = _coefs[j];
                        }
                        return tmpMatrix;
                    }
                }
                return tmpMatrix;
            }

            //инициализация массива определителей матриц
            private void InitArrayDeterms()
            {

                _determs = new double[_matrixEquat.Column];
                for (var i = 0; i < _matrixEquat.GetMatrix().GetLength(0); i++)
                {
                    _determs[i] = _matrixEquat.GetDeterminant(Replace(i));
                }
            }

            public Matrix GetMatrixEquat()
            {
                return _matrixEquat;
            }
    }

    public class CramerSolveNotFound : Exception
    {
        public CramerSolveNotFound(string msg)
            : base("Solution is not found: \r\n" + msg)
        {

        }
    }
}