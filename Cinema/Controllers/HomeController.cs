using Cinema.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult Confirm(int FuncionId, string Asiento_ubicacion)
        {
            TicketVM ticket = business.GenerarTicket(FuncionId, Asiento_ubicacion);

            return RedirectToAction("Index");
        }

        public FileStreamResult GenerarPdf(int func, string ubi)
        {
            MemoryStream stream = business.GenerarPdf(func, ubi);
            

            return new FileStreamResult(stream, "application/pdf");
        }
    }
}