using System.Collections.Generic;
namespace TestEquatMvc.Models
{
    public class ContextSolveStrategy
    {
        public int Dimension { get; set; }
        public bool EmptyField { get; set; }
        public bool CharField { get; set; }
        
        private static ContextSolveStrategy context = null;

        public string Result { get; set; }

        public string TypeMethod { get; set; }

        public static ContextSolveStrategy GetInstance()
        {
            if (context == null)
                return new ContextSolveStrategy();
            else return context;
        }

        private ISolveStrategy _solveStrategy;

        public ContextSolveStrategy(/*ISolveStrategy solveStrategy*/)
        {
            //_solveStrategy = solveStrategy;
        }

        public void SetStrategy(ISolveStrategy strategy)
        {
            _solveStrategy = strategy;
        }

        public void Solve()
        {
            Result = "";
            double[] result = _solveStrategy.Solve();
            for (int i = 0; i < result.Length; i++)
            {
                Result += "x" + (i + 1) + " = " + result[i];
                if (i != result.Length-1)
                {
                    Result += "; ";
                }
            }
        }

    }
}