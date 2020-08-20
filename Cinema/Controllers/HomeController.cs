using Cinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cinema.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var movieList = business.GetMoviesFromTmdb();

            return View(movieList);
        }

        public ActionResult Details(int id)
        {
            //ViewBag.Message = "Your application description page.";
            OrdenVM orden = business.GenerarOrden(id);

            return View(orden);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}