using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestEquatMvc.Models
{
    public class StringParser
    {
        private String[] _strings;  //введенные строки

        public String[] Strings
        {
            get { return _strings; }
            set { _strings = value; }
        }

        private List<string> list; //список с элементами в виде строк
        private List<double> blist;
        private List<double> matr;   //список коэффициентов матрицы
        private List<char> vars;  //список с переменными, чтобы отследить их количество и повторяемость


        public StringParser(String[] strings)
        {
            _strings = strings;
            for (int i = 0; i < _strings.Length; i++)
            {
                _strings[i] = DeleteSpaces(_strings[i]);
            }
            list = new List<string>(); //список с элементами в виде строк
            blist = new List<double>();
            matr = new List<double>();   //список коэффициентов матрицы
            vars = new List<char>();  //список с переменными, чтобы отследить их количество и повторяемость
            InitVariables();
        }

        /// <summary>
        /// Удалить все пробелы из строки
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String DeleteSpaces(String str)
        {
            string[] tmp = str.Split(' '); //удаляем все пробелы
            str = "";
            foreach (var s in tmp)
            {
                str += s;
            }
            return str;
        }

        /// <summary>
        /// Инициализация списка переменных
        /// </summary>
        private void InitVariables()
        {
            for (int i = 0; i < _strings.Length; i++)
            {
                foreach (var ch in _strings[i])
                {
                    if (!(((ch == '+') || (ch == '-') || (ch == '=')) || ((ch >= '0') && (ch <= '9'))))
                    {
                        bool trig = false;
                        //if (vars.Count > 0)
                        foreach (var v in vars)
                        {
                            if (v == ch)
                            {
                                trig = true;
                            }
                        }
                        if (!trig) vars.Add(ch);
                    }
                }
            }
        }

        /// <summary>
        /// Проверить, совпадает ли количество строк с количеством переменных
        /// </summary>
        /// <returns></returns>
        public bool CheckVariables()
        {
            if (_strings.Length == vars.Count)
                return true;
            else return false;
        }

        private void InitMatrix(string str)
        {
            list.Clear();
            string[] strs = str.Split('=');

            string left = DeleteSpaces(strs[0]);

            string[] ss = left.Split('+');

            for (int i = 0; i < ss.Length; i++)
            {
                string[] sss = ss[i].Split('-');
                for (int j = 0; j < sss.Length; j++)
                {
                    if (sss[j] != "")
                        if (j == 0)
                        {
                            list.Add(sss[j]);
                        }
                        else
                        {
                            list.Add("-" + sss[j]);
                        }
                }
            }

            string right = DeleteSpaces(strs[1]);

            ss = right.Split('+');
            for (int i = 0; i < ss.Length; i++)
            {
                string[] sss = ss[i].Split('-');
                for (int j = 0; j < sss.Length; j++)
                {
                    if (sss[j] != "")
                        if (j == 0)
                        {
                            list.Add("-" + sss[j]);
                        }
                        else
                        {
                            list.Add(sss[j]);
                        }
                }
            }

            //парсинг
            int b, b1 = 0;  //b1 - переменная, которая хранит свободный член

            //находим свободный член
            foreach (var v in list)
            {
                bool result = Int32.TryParse(v, out b);
                if (result)
                    b1 -= b;
            }

            ///составляем матрицу элементов
            foreach (var v in vars)
            {
                bool t = false;
                string tmpstr = "";
                foreach (var s in list)
                {
                    if (s.ToCharArray()[s.Length - 1] == v)
                    {
                        t = true;
                        tmpstr = s;
                        break;
                    }
                }
                if (!t)
                    matr.Add(0);    //если переменная в уравнении отсутствует, вместо нее - 0
                else
                {
                    if (tmpstr.Length > 1)  //если у переменной в выражении есть коэффициенты
                    {
                        tmpstr = tmpstr.Remove(tmpstr.Length - 1);
                        if (tmpstr == "-")
                        {
                            tmpstr = "-1";
                        }
                        matr.Add(Int32.Parse(tmpstr));
                    }
                    else
                    {
                        matr.Add(1);
                    }
                }
            }
            blist.Add(b1);
        }

        public void InitMatrixs()
        {
            for (int i = 0; i < _strings.Length; i++)
            {
                InitMatrix(_strings[i]);
            }
        }

        /// <summary>
        /// получить 2-хразмерную матрицу 
        /// </summary>
        /// <returns></returns>
        public double[,] GetMatrix()
        {
            int d = _strings.Length; //размерность матрицы
            double[,] matrix = new double[d, d];
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    matrix[i, j] = matr[3 * i + j];
                }
            }
            return matrix;
        }

        public double[] GetVector()
        {
            int d = _strings.Length; //размерность матрицы
            double[] b = new double[d];
            for (int i = 0; i < d; i++)
            {
                b[i] = blist[i];
            }
            return b;
        }
    }

}