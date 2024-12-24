namespace KariyerPortali5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Sirket")]
    public partial class Sirket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sirket()
        {
            Ilan = new HashSet<Ilan>();
        }

        public int SirketId { get; set; }

        [StringLength(50)]
        public string SirketAdi { get; set; }

        public int? KonumId { get; set; }

        public int? SektorId { get; set; }

        [StringLength(100)]
        public string WebSite { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ilan> Ilan { get; set; }

        public virtual Konum Konum { get; set; }

        public virtual Sektor Sektor { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Sifre { get; set; }

    }
}
