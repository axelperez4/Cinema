namespace Cinema.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Asientos")]
    public partial class Asiento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Asiento()
        {
            Tickets = new HashSet<Ticket>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int asiento_id { get; set; }

        public int sala_id { get; set; }

        [Required]
        [StringLength(4)]
        public string ubicacion { get; set; }

        public virtual Sala Sala { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
