using Cinema.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
                        movie.Adultos = movieData.Adult.Value;
                        movie.Descripcion = movieData.Overview;
                        movie.Generos = String.Join(", ", movieData.genres.Select(x => x.Name));
                        movie.Lanzamiento = movieData.ReleaseDate;
                        movie.Popularidad = movieData.Popularity;
                    }
                    else
                    {
                        movie.Descripcion = "Película no disponible";
                    }
                }
                client.Dispose();
            }

            cinemaContext.SaveChanges();
            var VmMovieList = listOfAvailableMovies.Select(x => GenerarPeliculaVM(x)).OrderByDescending(x => x.ReleaseDate).ToList();

            return VmMovieList;
        }

        internal string GenerarReporte()
        {
            var cinemaDb = new CinemaDbContext();
            var con = new SqlConnection(cinemaDb.Database.Connection.ConnectionString);
            var cmd = new SqlCommand("GenerarReporteDeIngresos", con); //Para editar reporte, cambiar sproc

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            con.Open();
            var reader = cmd.ExecuteReader();

            var sb = new StringBuilder();
            var tempList = new List<string>();

            //Escribir nombres de columnas
            for (int i = 0; i < reader.FieldCount; i++)
            {
                tempList.Add(reader.GetName(i));
            }
            sb.AppendLine(string.Join("|", tempList));

            while (reader.Read())
            {
                tempList.Clear();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    tempList.Add(reader[i].ToString());
                }
                sb.AppendLine(string.Join("|", tempList));
            }

            return sb.ToString();
        }

        internal MemoryStream GenerarPdf(int funcionId, string asiento_ubicacion, string total, string extras)
        {
            var cinemaContext = new CinemaDbContext();
            var funcion = cinemaContext.Funciones.First(x => x.Funcion_id == funcionId);

            string formattedExtras = null;
            if (!string.IsNullOrWhiteSpace(extras))
            {
                formattedExtras += "\n Extras:";
                foreach (var extra in extras.Split(','))
                {
                    formattedExtras += "\n      " + extra.Trim();
                }
            }

            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();
            document.AddTitle("Ticket de cine");
            document.Add(new Paragraph("Cinema de Axel"));
            document.Add(new Paragraph("Puede presentar el código QR mostrado abajo para pagar su reservación en caja. La información de su reserva es la siguiente:"));
            document.Add(new Paragraph(String.Format("Fecha:{0} \n Hora: {1} \n Asiento: {2} \n Sala: {3} {4} \n Pelicula: {5}{6} \n Total: Q.{7}",
                                                        funcion.Fecha.ToShortDateString(),
                                                        funcion.Fecha.TimeOfDay.ToString(),
                                                        asiento_ubicacion,
                                                        funcion.Sala_id,
                                                        funcion.Sala.Descripcion,
                                                        funcion.Pelicula.Titulo,
                                                        formattedExtras ?? "",
                                                        total)));
            document.Add(Image.GetInstance(@"C:\Users\axelp\Documents\Axel\Cine de Axel\Cinema\Cinema\Images\qr_launion.png"));
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return workStream;
        }

        public void GenerarTicket(OrdenVM orden)
        {
            var cinemaContext = new CinemaDbContext();
            var ticket = new Ticket()
            {
                Funcion_id = orden.FuncionId,
                Asiento_ubicacion = orden.Asiento_ubicacion,
                Total = orden.Total,
                Extras = orden.Extras
            };
            cinemaContext.Tickets.Add(ticket);
            cinemaContext.SaveChanges();
        }

        public OrdenVM GenerarOrden(int id)
        {
            var orden = new OrdenVM();
            var cinemaDb = new CinemaDbContext();
            var pelicula = cinemaDb.Peliculas.First(x => x.Pelicula_id == id);
            var funciones = pelicula.Funciones.Where(x => x.Fecha > DateTime.Now).ToList().OrderBy(x => x.Fecha);
            var extras = cinemaDb.Extras.Where(x => x.Activo).ToList();

            //Asignar valores para alimentar dropdowns
            orden.Movie = GenerarPeliculaVM(pelicula);
            orden.Asientos = ObtenerAsientos(cinemaDb, pelicula.Pelicula_id);
            orden.ListaExtras = extras;
            orden.Funciones = funciones.Select(x => 
                                new System.Web.Mvc.SelectListItem
                                {
                                    Text = String.Format("<b>Fecha:</b> {0} | <b>Hora:</b> {1} | <b>Tipo:</b> {2} | <b>Precio:</b> Q.{3}", x.Fecha.ToShortDateString(), x.Fecha.TimeOfDay.ToString(), x.Sala.Descripcion, x.Precio.ToString()),
                                    Value = x.Funcion_id.ToString()
                                }
            );
            if (funciones.Any())
                orden.FuncionEconomica = funciones.OrderBy(x => x.Precio).ThenBy(x => x.Fecha).FirstOrDefault().Funcion_id;

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
                PosterPath = pelicula.PosterPath,
                Genres = pelicula.Generos,
                ReleaseDate = pelicula.Lanzamiento
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