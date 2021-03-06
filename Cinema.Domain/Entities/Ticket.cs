namespace Cinema.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ticket_id { get; set; }

        public int funcion_id { get; set; }

        public int asiento_id { get; set; }

        public virtual Asiento Asiento { get; set; }

        public virtual Funcion Funcion { get; set; }
    }
}
