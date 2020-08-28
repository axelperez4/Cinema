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
        public string Asiento_ubicacion { get; set; }
        public int FuncionEconomica { get; set; }
        public decimal Total { get; set; }
        public string Extras { get; set; }
        public PeliculaVM Movie { get; set; }
        public IEnumerable<SelectListItem> Funciones { get; set; }
        public IEnumerable<AsientoVM> Asientos { get; set; }
        public IEnumerable<Extra> ListaExtras { get; set; }
    }
}