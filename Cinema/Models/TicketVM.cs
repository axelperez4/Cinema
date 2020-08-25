using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class TicketVM
    {
        public int Funcion_Id { get; set; }
        public string Ubicacion { get; set; }
        public DateTime Fecha { get; set; }
    }
}