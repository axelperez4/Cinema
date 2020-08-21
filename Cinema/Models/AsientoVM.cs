using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class AsientoVM
    {
        public int Funcion_Id { get; set; }
        public string Ubicacion { get; set; }
        public bool Disponible { get; set; }
    }
}