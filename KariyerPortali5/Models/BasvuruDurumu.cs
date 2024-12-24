namespace KariyerPortali5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BasvuruDurumu")]
    public partial class BasvuruDurumu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BasvuruDurumu()
        {
            Basvuru = new HashSet<Basvuru>();
        }

        public int BasvuruDurumuId { get; set; }

        [Required]
        [StringLength(50)]
        public string Durum { get; set; }

        public DateTime? SonGuncellemeTarihi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Basvuru> Basvuru { get; set; }
    }
}
