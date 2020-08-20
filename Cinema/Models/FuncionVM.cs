using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class FuncionVM
    {
        public int? Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}