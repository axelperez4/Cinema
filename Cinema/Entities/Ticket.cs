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

        public int Asiento_id { get; set; }

        public virtual Asiento Asiento { get; set; }

        public virtual Funcion Funcion { get; set; }
    }
}
