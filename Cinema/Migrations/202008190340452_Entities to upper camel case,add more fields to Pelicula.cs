namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntitiestouppercamelcaseaddmorefieldstoPelicula : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Asientos", new[] { "sala_id" });
            DropIndex("dbo.Funciones", new[] { "sala_id" });
            DropIndex("dbo.Funciones", new[] { "pelicula_id" });
            DropIndex("dbo.Tickets", new[] { "funcion_id" });
            DropIndex("dbo.Tickets", new[] { "asiento_id" });
            AddColumn("dbo.Peliculas", "Tagline", c => c.String());
            AddColumn("dbo.Peliculas", "Descripcion", c => c.String());
            AddColumn("dbo.Peliculas", "Adultos", c => c.Boolean(nullable: false));
            AddColumn("dbo.Peliculas", "Duracion", c => c.Int(nullable: false));
            AddColumn("dbo.Peliculas", "Votacion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Peliculas", "PosterPath", c => c.String());
            AlterColumn("dbo.Peliculas", "Titulo", c => c.String(maxLength: 150));
            CreateIndex("dbo.Asientos", "Sala_id");
            CreateIndex("dbo.Funciones", "Sala_id");
            CreateIndex("dbo.Funciones", "Pelicula_id");
            CreateIndex("dbo.Tickets", "Funcion_id");
            CreateIndex("dbo.Tickets", "Asiento_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tickets", new[] { "Asiento_id" });
            DropIndex("dbo.Tickets", new[] { "Funcion_id" });
            DropIndex("dbo.Funciones", new[] { "Pelicula_id" });
            DropIndex("dbo.Funciones", new[] { "Sala_id" });
            DropIndex("dbo.Asientos", new[] { "Sala_id" });
            AlterColumn("dbo.Peliculas", "Titulo", c => c.String(nullable: false, maxLength: 150));
            DropColumn("dbo.Peliculas", "PosterPath");
            DropColumn("dbo.Peliculas", "Votacion");
            DropColumn("dbo.Peliculas", "Duracion");
            DropColumn("dbo.Peliculas", "Adultos");
            DropColumn("dbo.Peliculas", "Descripcion");
            DropColumn("dbo.Peliculas", "Tagline");
            CreateIndex("dbo.Tickets", "asiento_id");
            CreateIndex("dbo.Tickets", "funcion_id");
            CreateIndex("dbo.Funciones", "pelicula_id");
            CreateIndex("dbo.Funciones", "sala_id");
            CreateIndex("dbo.Asientos", "sala_id");
        }
    }
}
