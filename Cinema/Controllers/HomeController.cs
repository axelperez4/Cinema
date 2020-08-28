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

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            //ViewBag.Message = "Your application description page.";
            OrdenVM orden = business.GenerarOrden(id.Value);

            return View(orden);
        }

        //public ActionResult Confirm(int FuncionId, string Asiento_ubicacion)
        public ActionResult Confirm(OrdenVM orden)
        {
            business.GenerarTicket(orden);

            return RedirectToAction("Index");
        }

        public FileStreamResult GenerarPdf(int func, string ubi, string tot, string ext)
        {
            MemoryStream stream = business.GenerarPdf(func, ubi, tot, ext);

            Response.ContentType = "pdf/application";
            Response.AddHeader("Content-Disposition", String.Format("filename=Ticket-{0}-{1}", func, ubi));
            return new FileStreamResult(stream, "application/pdf");
        }
    }
}