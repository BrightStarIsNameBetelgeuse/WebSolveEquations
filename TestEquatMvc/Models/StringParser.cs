﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestEquatMvc.Models
{
    public class StringParser
    {
        private List<string> _strings;  //введенные строки

        public List<string> Strings
        {
            get { return _strings; }
            set { _strings = value; }
        }

        private List<string> list; //список с элементами в виде строк
        private List<double> blist;
        private List<double> matr;   //список коэффициентов матрицы
        private List<char> vars;  //список с переменными, чтобы отследить их количество и повторяемость

        public List<char> Vars
        {
            get { return vars; }
            set {  }
        }


        public StringParser(List<string> strings)
        {
            _strings = strings;
            for (int i = 0; i < _strings.Count; i++)
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
            for (int i = 0; i < _strings.Count; i++)
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
        /// Проверить, количество переменных больше кол-ва уравнений (строк)
        /// </summary>
        /// <returns></returns>
        public bool CheckVariables()
        {
            if ( vars.Count >= _strings.Count)
                return true;
            else return false;
        }

        /// <summary>
        /// Сокращение
        /// </summary>
        /// <param name="str"></param>
        private void Reduction(string str)
        {
            //for (int i = 0; i < str.Length; i++)
            //{
            //    if()
            //}
        }

        private void InitMatrix(string str)
        {
            list.Clear();
            string[] strs = str.Split('='); //разделяем на 2 половины по знаку равенства
                                                    //левая часть
            string left = DeleteSpaces(strs[0]);    //удаляем все пробелы, если есть

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
            for (int i = 0; i < _strings.Count; i++)
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
            int d = _strings.Count; //размерность матрицы
            double[,] matrix = new double[d, d];
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    matrix[i, j] = matr[d * i + j];
                }
            }
            return matrix;
        }

        public double[] GetVector()
        {
            int d = _strings.Count; //размерность матрицы
            double[] b = new double[d];
            for (int i = 0; i < d; i++)
            {
                b[i] = blist[i];
            }
            return b;
        }

        /// <summary>
        /// Проверка на наличие знака равенства
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool CheckEqualsSign(string str)
        {
            bool flag = false;
            for (int i = 0; i < str.Length; i++)
            {
                if(str[i] == '=') 
                { 
                    flag = true; 
                    break;
                }
            }
            return flag;
        }

    }

}