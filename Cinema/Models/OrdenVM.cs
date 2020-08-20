using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Cinema.Models
{
    public class OrdenVM
    {
        public int FuncionId { get; set; }
        public PeliculaVM Movie { get; set; }
        public IEnumerable<SelectListItem> Funciones { get; set; }
    }
}