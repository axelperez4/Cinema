using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cinema.Models
{
    public class PeliculaVM
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Tagline { get; set; }
        public string Overview { get; set; }
        public bool Adult { get; set; }
        public int? Runtime { get; set; }
        [JsonProperty("vote_average")]
        public decimal? VoteAvarage { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
    }
}