namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddbooleanflagtoPeliculaInitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Asientos",
                c => new
                    {
                        asiento_id = c.Int(nullable: false),
                        sala_id = c.Int(nullable: false),
                        ubicacion = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.asiento_id)
                .ForeignKey("dbo.Salas", t => t.sala_id)
                .Index(t => t.sala_id);
            
            CreateTable(
                "dbo.Salas",
                c => new
                    {
                        sala_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.sala_id);
            
            CreateTable(
                "dbo.Funcions",
                c => new
                    {
                        funcion_id = c.Int(nullable: false),
                        sala_id = c.Int(nullable: false),
                        pelicula_id = c.Int(nullable: false),
                        precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.funcion_id)
                .ForeignKey("dbo.Peliculas", t => t.pelicula_id)
                .ForeignKey("dbo.Salas", t => t.sala_id)
                .Index(t => t.sala_id)
                .Index(t => t.pelicula_id);
            
            CreateTable(
                "dbo.Peliculas",
                c => new
                    {
                        pelicula_id = c.Int(nullable: false),
                        titulo = c.String(nullable: false, maxLength: 150),
                        activa = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.pelicula_id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        ticket_id = c.Int(nullable: false),
                        funcion_id = c.Int(nullable: false),
                        asiento_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ticket_id)
                .ForeignKey("dbo.Funcions", t => t.funcion_id)
                .ForeignKey("dbo.Asientos", t => t.asiento_id)
                .Index(t => t.funcion_id)
                .Index(t => t.asiento_id);
            
            CreateTable(
                "dbo.PeliculaVMs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Tagline = c.String(),
                        Overview = c.String(),
                        Adult = c.Boolean(nullable: false),
                        Runtime = c.Int(nullable: false),
                        VoteAvarage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PosterPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "asiento_id", "dbo.Asientos");
            DropForeignKey("dbo.Funcions", "sala_id", "dbo.Salas");
            DropForeignKey("dbo.Tickets", "funcion_id", "dbo.Funcions");
            DropForeignKey("dbo.Funcions", "pelicula_id", "dbo.Peliculas");
            DropForeignKey("dbo.Asientos", "sala_id", "dbo.Salas");
            DropIndex("dbo.Tickets", new[] { "asiento_id" });
            DropIndex("dbo.Tickets", new[] { "funcion_id" });
            DropIndex("dbo.Funcions", new[] { "pelicula_id" });
            DropIndex("dbo.Funcions", new[] { "sala_id" });
            DropIndex("dbo.Asientos", new[] { "sala_id" });
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.PeliculaVMs");
            DropTable("dbo.Tickets");
            DropTable("dbo.Peliculas");
            DropTable("dbo.Funcions");
            DropTable("dbo.Salas");
            DropTable("dbo.Asientos");
        }
    }
}
