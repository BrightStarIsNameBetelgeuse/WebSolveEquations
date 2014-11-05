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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dim">Введенная пользователем размерность, по умолчанию 2</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Start(string dim = "2")
        {
            ContextSolveStrategy contextSolveStrategy = new ContextSolveStrategy();
            //if(dim. - 48)
            contextSolveStrategy.Dimension = Int32.Parse(dim);
            return View("Index", contextSolveStrategy);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[MultiButton(MatchFormKey = "action", MatchFormValue = "Gauss's method")]
        public ActionResult Index(List<string> names, string action)
        {
            ContextSolveStrategy contextSolveStrategy = new ContextSolveStrategy();
            contextSolveStrategy.Dimension = 2;
            int d = contextSolveStrategy.Dimension;

            double[,] matr = new double[d,d];
            double[] b = new double[d];
            ///проверка на корректность введенных данных
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i].Length == 0)
                {
                    contextSolveStrategy.EmptyField = true;
                    break;
                }   
                char[] tmp = names[i].ToCharArray();
                foreach (var ch in tmp)
                {
                    if (((ch - 48) < 0) || ((ch - 48) > 9))
                    {
                        contextSolveStrategy.CharField = true;
                        break;
                    }         
                }
            }

            if (contextSolveStrategy.CharField || contextSolveStrategy.EmptyField)
            {
                contextSolveStrategy.Result = "The fields contain empty or incorrect";
                return View("Results", contextSolveStrategy);
            }


            ///заполнение матрицы
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    matr[i, j] = Double.Parse(names[i * (d + 1) + j]);
                }
            }

            ///заполнение вектора
            for (int i = 0; i < d; i++)
            {
                b[i] = Double.Parse(names[i * (d + 1) + d]);
            }

            if (action == "Cramer's method")
            {
                contextSolveStrategy.TypeMethod = "Cramer";
                contextSolveStrategy.SetStrategy(new CramerStrategy(matr, b));
            }
            if (action == "Gauss's method")
            {
                contextSolveStrategy.TypeMethod = "Gauss";
                contextSolveStrategy.SetStrategy(new GaussStrategy(matr, b));
            }

            try
            {
                contextSolveStrategy.Solve();
            }
            catch(Exception ex)
            {
                contextSolveStrategy.Result = ex.Message;
            }

            return View("Results", contextSolveStrategy);
        }
    }
}
