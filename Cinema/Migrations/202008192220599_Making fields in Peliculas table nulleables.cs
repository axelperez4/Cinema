namespace Cinema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingfieldsinPeliculastablenulleables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Peliculas", "Adultos", c => c.Boolean());
            AlterColumn("dbo.Peliculas", "Duracion", c => c.Int());
            AlterColumn("dbo.Peliculas", "Votacion", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Peliculas", "Votacion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Peliculas", "Duracion", c => c.Int(nullable: false));
            AlterColumn("dbo.Peliculas", "Adultos", c => c.Boolean(nullable: false));
        }
    }
}
