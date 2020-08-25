using Cinema.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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
            var VmMovieList = listOfAvailableMovies.Select(x => GenerarPeliculaVM(x)).ToList();

            return VmMovieList;
        }

        internal MemoryStream GenerarPdf(int funcionId, string asiento_ubicacion)
        {
            var cinemaContext = new CinemaDbContext();
            var funcion = cinemaContext.Funciones.First(x => x.Funcion_id == funcionId);

            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();
            document.Add(new Paragraph("Cinema de Axel"));
            document.Add(new Paragraph("Puede presentar el código QR mostrado abajo para ingresar a sala. La información de su función es la siguiente:"));
            document.Add(new Paragraph(String.Format("Fecha:{0} \n Hora: {1} \n Asiento: {2} \n Sala: {3}, Pelicula: {4}",
                                                        funcion.Fecha.ToShortDateString(),
                                                        funcion.Fecha.TimeOfDay.ToString(),
                                                        asiento_ubicacion,
                                                        funcion.Sala_id,
                                                        funcion.Pelicula.Titulo)));
            document.Add(Image.GetInstance(@"C:\Users\axelp\Documents\Axel\Cine de Axel\Cinema\Cinema\Images\qr_launion.png"));
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return workStream;
        }

        public TicketVM GenerarTicket(int funcionId, string asiento_ubicacion)
        {
            var cinemaContext = new CinemaDbContext();
            var ticket = new Ticket()
            {
                Funcion_id = funcionId,
                Asiento_ubicacion = asiento_ubicacion
            };
            cinemaContext.Tickets.Add(ticket);
            cinemaContext.SaveChanges();

            var ticketVM = new TicketVM()
            {
                Funcion_Id = ticket.Funcion_id,
                Ubicacion = ticket.Asiento_ubicacion,
                Fecha = cinemaContext.Funciones.First(x => x.Funcion_id == funcionId).Fecha
            };

            return ticketVM;
        }

        public OrdenVM GenerarOrden(int id)
        {
            var orden = new OrdenVM();
            var cinemaDb = new CinemaDbContext();
            var pelicula = cinemaDb.Peliculas.First(x => x.Pelicula_id == id);
            var funciones = pelicula.Funciones.Where(x => x.Fecha > DateTime.Now).ToList().OrderBy(x => x.Fecha);

            //Asignar valores para alimentar dropdowns
            orden.Movie = GenerarPeliculaVM(pelicula);
            orden.Asientos = ObtenerAsientos(cinemaDb, pelicula.Pelicula_id);
            orden.Funciones = funciones.Select(x => 
                                new System.Web.Mvc.SelectListItem
                                {
                                    Text = String.Format("Fecha: {0} | Hora: {1} | Precio: Q.{2}", x.Fecha.ToShortDateString(), x.Fecha.TimeOfDay.ToString(), x.Precio.ToString()),
                                    Value = x.Funcion_id.ToString()
                                }
            );
            
            return orden;
        }

        private IEnumerable<AsientoVM> ObtenerAsientos(CinemaDbContext cinemaDb, int pelicula_id)
        {
            var asientos = cinemaDb.Database.SqlQuery<AsientoVM>("exec ObtenerAsientos @pelicula_id", new object[] { new SqlParameter("pelicula_id", pelicula_id) }).ToList();

            return asientos;
        }

        private object GetApiKey()
        {
            string apiKey = System.Configuration.ConfigurationManager.AppSettings["TmdbApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("Error - Api Key is missing.");

            return apiKey;
        }

        private PeliculaVM GenerarPeliculaVM(Pelicula pelicula)
        {
            return new PeliculaVM
            {
                Id = pelicula.Pelicula_id,
                Title = pelicula.Titulo,
                Tagline = pelicula.Tagline,
                Overview = pelicula.Descripcion,
                VoteAvarage = pelicula.Votacion,
                Runtime = pelicula.Duracion,
                Adult = pelicula.Adultos,
                PosterPath = pelicula.PosterPath
            };
        }

        private FuncionVM GenerarFuncionVM(Funcion funcion)
        {
            return new FuncionVM
            {
                Id = funcion.Funcion_id,
                Price = funcion.Precio,
                Date = funcion.Fecha
            };
        }
    }
}