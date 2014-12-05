using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestEquatMvc.Models;

namespace TestEquatMvc.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Start()
        {
            return View();
        }


        //[HttpGet]
        public ActionResult Index(ContextSolveStrategy context)
        {
            return View(context);
        }

        [HttpPost]
        public ActionResult GetResult(string names)
        {
            ContextSolveStrategy contextSolveStrategy = new ContextSolveStrategy();

            //contextSolveStrategy.Dimension = 3;
            char[] chars = { '\r','\n'};
            string[] strs = names.Split(chars);
            List<string> equats = new List<string>();

            for (int i = 0; i < strs.Length; i++)
            {
                if(strs[i] != "")
                    equats.Add(strs[i]);
            }

            StringParser sp = new StringParser(equats);

            int dimension = equats.Count;   //количество уравнений == размерность матрицы

            double[,] matr = new double[dimension, dimension];
            double[] b = new double[dimension];
            ///проверка на корректность введенных данных
            for (int i = 0; i < equats.Count; i++)
            {
                if (equats[i].Length == 0)
                {
                    contextSolveStrategy.EmptyField = true;
                    break;
                }
            }

            if (contextSolveStrategy.CharField || contextSolveStrategy.EmptyField || !(sp.CheckVariables()))
            {
                contextSolveStrategy.Result = "The fields contain empty or incorrect";
                if (Request.IsAjaxRequest())
                {
                    //contextSolveStrategy.TypeMethod = action;
                    //if (Action == "Cramer's method")
                    //    contextSolveStrategy.TypeMethod = "Cramer";
                    //if (Action == "Gauss's method")
                    //    contextSolveStrategy.TypeMethod = "Gauss";
                    return PartialView("Result", contextSolveStrategy);
                }
                return PartialView("Result", contextSolveStrategy);
            }

            sp.InitMatrixs();
            matr = sp.GetMatrix();
            b = sp.GetVector();

            contextSolveStrategy.Solution.Vars = sp.Vars;

            try
            {
                contextSolveStrategy.SetStrategy(new GaussStrategy(matr, b));
                contextSolveStrategy.Solve();
            }
            catch (Exception ex)
            {
                contextSolveStrategy.Result = ex.Message;
            }
            return PartialView("Result", contextSolveStrategy);
        }

    }
}
