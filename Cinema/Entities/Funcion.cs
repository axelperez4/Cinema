namespace Cinema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Funciones")]
    public partial class Funcion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Funcion()
        {
            Tickets = new List<Ticket>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Funcion_id { get; set; }

        public int Sala_id { get; set; }

        public int Pelicula_id { get; set; }

        public decimal Precio { get; set; }

        public DateTime Fecha { get; set; }

        public virtual Pelicula Pelicula { get; set; }

        public virtual Sala Sala { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<Ticket> Tickets { get; set; }
    }
}
