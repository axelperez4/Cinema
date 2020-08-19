namespace Cinema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pelicula
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pelicula()
        {
            Funciones = new List<Funcion>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Pelicula_id { get; set; }
        [StringLength(150)]
        public string Titulo { get; set; }
        [Required]
        public bool Activa { get; set; }
        public string Tagline { get; set; }
        public string Descripcion { get; set; }
        public bool Adultos { get; set; }
        public int Duracion { get; set; }
        public decimal Votacion { get; set; }

        public string PosterPath { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Funcion> Funciones { get; set; }
    }
}
