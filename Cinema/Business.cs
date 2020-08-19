using Cinema.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Cinema
{
    public class Business
    {
        public List<PeliculaVM> GetMoviesFromTmdb()
        {
            var cinemaContext = new CinemaDbContext();

            var listOfAvailableMovies = cinemaContext.Peliculas.Where(x => x.Activa).ToList();

            if (listOfAvailableMovies.Any(x => x.Titulo == null))
            {
                var baseUrl = @"https://api.themoviedb.org/3/movie/";
                var tmdbApiKey = GetApiKey();
                var language = "es"; //Español
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(baseUrl)
                };

                foreach (var movie in listOfAvailableMovies.Where(x => x.Titulo == null))
                {
                    var parameters = String.Format("{0}?api_key={1}&language={2}", movie.Pelicula_id, tmdbApiKey, language);

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    // List data response.
                    HttpResponseMessage response = client.GetAsync(parameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        var movieData = response.Content.ReadAsAsync<PeliculaVM>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll

                        //Update movie metadata to avoid consuming the API and improve performance
                        movie.Titulo = movieData.Title;
                        movie.Tagline = movieData.Tagline;
                        movie.PosterPath = movieData.PosterPath;
                        movie.Votacion = movieData.VoteAvarage ?? 0.00m;
                        movie.Duracion = movieData.Runtime ?? 0;
                        movie.Adultos = movieData.Adult;
                        movie.Descripcion = movieData.Overview;
                    }
                    else
                    {
                        movie.Descripcion = "Película no disponible";
                    }
                }
                client.Dispose();
            }

            cinemaContext.SaveChanges();
            var VmMovieList = listOfAvailableMovies.Select(x =>
                                                    new PeliculaVM()
                                                    {
                                                        Id = x.Pelicula_id,
                                                        Title = x.Titulo,
                                                        Tagline = x.Tagline,
                                                        Overview = x.Descripcion,
                                                        VoteAvarage = x.Votacion,
                                                        Runtime = x.Duracion,
                                                        Adult = x.Adultos,
                                                        PosterPath = x.PosterPath
                                                    }).ToList();

            return VmMovieList;
        }

        private object GetApiKey()
        {
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["TmdbApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("Error - Api Key is missing.");

            return apiKey;
        }
    }
}