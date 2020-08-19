using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cinema.Tests
{
    [TestClass]
    public class BusinessUnitTests
    {
        CinemaDbContext cinemaDb = new CinemaDbContext();

        [TestMethod]
        public void Business_GetMoviesFromTmdb_Success()
        {
            //Arrange
            int testMovie1 = 299534; //Endgame
            ClearMovieMetadata(testMovie1); //ClearMetadata so we can check if meta data is being saved during API request
            var business = new Business();
            
            //Act
            var movieList = business.GetMoviesFromTmdb();

            //Assert
            Assert.IsTrue(movieList.Count > 0, "Ninguna película fue devuelta.");
            Assert.IsTrue(movieList.Any(x => x.Id == testMovie1), "La película de prueba no fue procesada");
            Assert.IsTrue(movieList.First(x => x.Id == testMovie1).Title != null);
        }

        private void ClearMovieMetadata(int movieId)
        {
            var movie = cinemaDb.Peliculas.SingleOrDefault(x => x.Pelicula_id == movieId);
            if (movie == null)
            {
                cinemaDb.Peliculas.Add(new Pelicula() { Pelicula_id = movieId });
            }
            else
            {
                movie.Titulo = null;
                movie.PosterPath = null;
                movie.Tagline = null;
                movie.Votacion = 0.00m;
                movie.Duracion = 0;
                movie.Descripcion = null;
            }

            cinemaDb.SaveChanges();
        }
    }
}
