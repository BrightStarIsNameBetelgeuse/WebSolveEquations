using System;
using System.Collections.Generic;
namespace TestEquatMvc.Models
{
    public class ContextSolveStrategy
    {
        public int Dimension { get; set; }

        public bool EmptyField { get; set; }

        public bool CharField { get; set; }

        public string TypeMethod { get; set; }

        private Solution solution;

        public Solution Solution
        {
            get { return solution; }
            set { solution = value; }
        }

        private String result;

        public String Result
        {
            get { return result; }
            set { result = value; }
        }

        private static ContextSolveStrategy context = null;

        public static ContextSolveStrategy GetInstance()
        {
            if (context == null)
                return new ContextSolveStrategy();
            else return context;
        }

        private ISolveStrategy _solveStrategy;

        public ContextSolveStrategy()
        {
            solution = new Solution();
        }

        public void SetStrategy(ISolveStrategy strategy)
        {
            _solveStrategy = strategy;
        }

        public void Solve()
        {
            double[] res = _solveStrategy.Solve();
            
            for (int i = 0; i < res.Length; i++)
            {
                solution.Sol.Add(res[i]);
            }

            result = GetResultString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String GetResultString()
        {
            String str = "";

            for (int i = 0; i < solution.Vars.Count; i++)
            {
                str += solution.Vars[i];
                for (int j = 0; j < solution.Sol.Count; j++)
                {
                    if (j == i)
                        str += " = " + solution.Sol[j];
                }
                str += " ";
            }
            return str;
        }

    }
}