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
        public ActionResult GetResult(string[] names, string action)
        {
            if (Request.IsAjaxRequest())
            {

            }
            ContextSolveStrategy contextSolveStrategy = new ContextSolveStrategy();
            contextSolveStrategy.Dimension = 3;
            StringParser sp = new StringParser(names);
            int d = contextSolveStrategy.Dimension;

            double[,] matr = new double[d, d];
            double[] b = new double[d];
            ///проверка на корректность введенных данных
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Length == 0)
                {
                    contextSolveStrategy.EmptyField = true;
                    break;
                }
            }

            if (contextSolveStrategy.CharField || contextSolveStrategy.EmptyField)
            {
                contextSolveStrategy.Result = "The fields contain empty or incorrect";
                if (Request.IsAjaxRequest())
                {
                    ViewBag.Comment = contextSolveStrategy.Result;
                    return PartialView(contextSolveStrategy);
                }
                return PartialView("Result", contextSolveStrategy);
            }

            sp.InitMatrixs();

            matr = sp.GetMatrix();

            b = sp.GetVector();

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
            catch (Exception ex)
            {
                contextSolveStrategy.Result = ex.Message;
            }
            return PartialView("Result", contextSolveStrategy);
        }

    }
}
