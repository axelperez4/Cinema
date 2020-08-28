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
        public bool? Adult { get; set; }
        public int? Runtime { get; set; }
        [JsonProperty("vote_average")]
        public decimal? VoteAvarage { get; set; }
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
        public string Genres { get; set; }
        [JsonProperty("popularity")]
        public decimal Popularity { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        public List<Genres> genres { get; set; }
    }

    public class Genres
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}