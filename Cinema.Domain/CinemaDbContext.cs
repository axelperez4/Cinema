namespace Cinema.Domain
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CinemaDbContext : DbContext
    {
        public CinemaDbContext()
            : base("name=CinemaDbContext")
        {
        }

        public virtual DbSet<Asiento> Asientos { get; set; }
        public virtual DbSet<Funcion> Funciones { get; set; }
        public virtual DbSet<Pelicula> Peliculas { get; set; }
        public virtual DbSet<Sala> Salas { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asiento>()
                .HasMany(e => e.Tickets)
                .WithRequired(e => e.Asiento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Funcion>()
                .HasMany(e => e.Tickets)
                .WithRequired(e => e.Funcion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pelicula>()
                .HasMany(e => e.Funciones)
                .WithRequired(e => e.Pelicula)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sala>()
                .HasMany(e => e.Asientos)
                .WithRequired(e => e.Sala)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sala>()
                .HasMany(e => e.Funciones)
                .WithRequired(e => e.Sala)
                .WillCascadeOnDelete(false);
        }
    }
}
