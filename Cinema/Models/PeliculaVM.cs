using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class PeliculaVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tagline { get; set; }
        public string Overview { get; set; }
        public bool Adult { get; set; }
        public int Runtime { get; set; }
        public decimal VoteAvarage { get; set; }

        public string PosterPath { get; set; }
    }
}