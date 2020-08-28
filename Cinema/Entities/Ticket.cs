namespace Cinema
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Ticket_id { get; set; }
        public int Funcion_id { get; set; }
        [Required]
        [StringLength(4)]
        public string Asiento_ubicacion { get; set; }
        public string Extras { get; set; }
        public decimal Total { get; set; }

        public virtual Funcion Funcion { get; set; }
    }
}
