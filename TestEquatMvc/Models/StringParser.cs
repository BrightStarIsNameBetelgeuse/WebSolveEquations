using System;
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
        private double[,] matr;   //список коэффициентов матрицы
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
            //matr = new List<double>();   //список коэффициентов матрицы
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
            matr = new double[_strings.Count, vars.Count];  //создаем матрицу
        }

        /// <summary>
        /// Проверка, если количество переменных больше кол-ва уравнений (строк)
        /// </summary>
        /// <returns></returns>
        public bool CheckVariables()
        {
            if ( vars.Count > _strings.Count)
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
            //}`
        }

        private void InitVector(string str, int k)
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

            

            int c = 0;
            foreach (var ch in vars)
            {
                int sum = 0;

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i][list[i].Length - 1] > '9')
                        if (ch == list[i][list[i].Length - 1])
                        {
                            if (list[i].Length > 1)
                                if (list[i][list[i].Length - 2] == '-')
                                {
                                    sum += -1;
                                }
                                else
                                {
                                    sum += Int32.Parse(list[i].Remove(list[i].Length - 1));
                                }
                            else
                            {
                                sum += 1;
                            }
                        }
                    
                }
                matr[k, c] = sum;
                c++;
            }

            //находим свободный член
            foreach (var v in list)
            {
                bool result = Int32.TryParse(v, out b);
                if (result)
                    b1 -= b;
            }
            blist.Add(b1);
        }

        public void InitMatrixs()
        {
            for (int i = 0; i < _strings.Count; i++)
            {
                InitVector(_strings[i],i);
            }
        }

        /// <summary>
        /// получить 2-хразмерную матрицу 
        /// </summary>
        /// <returns></returns>
        public double[,] GetMatrix()
        {
            return matr;
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