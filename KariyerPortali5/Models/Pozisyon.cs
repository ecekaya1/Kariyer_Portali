namespace KariyerPortali5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using KariyerPortali5.Models;

    [Table("Pozisyon")]
    public partial class Pozisyon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pozisyon()
        {
            Ilan = new HashSet<Ilan>();
        }

        public int PozisyonId { get; set; }
        public string PozisyonAdi { get; set; }

        [Required]
        [StringLength(50)]

        public int? DepartmanId { get; set; }

        public int? SektorId { get; set; }

        public virtual Departman Departman { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ilan> Ilan { get; set; }

        public virtual Sektor Sektor { get; set; }
    }
}
