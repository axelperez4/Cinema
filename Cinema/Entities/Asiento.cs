namespace Cinema
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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Asiento_id { get; set; }

        public int Sala_id { get; set; }

        [Required]
        [StringLength(4)]
        public string Ubicacion { get; set; }

        public virtual Sala Sala { get; set; }
    }
}
