namespace KariyerPortali5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Konum")]
    public partial class Konum
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Konum()
        {
            Ilan = new HashSet<Ilan>();
            Sirket = new HashSet<Sirket>();
        }

        public int KonumId { get; set; }

        [StringLength(20)]
        public string Ulke { get; set; }

        [StringLength(20)]
        public string Sehir { get; set; }

        [StringLength(20)]
        public string Ilce { get; set; }

        [StringLength(20)]
        public string Cadde { get; set; }

        [StringLength(20)]
        public string Mahalle { get; set; }

        public int? ApartmanNo { get; set; }

        public int? DaireNo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ilan> Ilan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sirket> Sirket { get; set; }
    }
}
