using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class AsientoVM
    {
        public int? Id { get; set; }
        public int? Sala_Id { get; set; }
        public string Ubicacion { get; set; }
    }
}