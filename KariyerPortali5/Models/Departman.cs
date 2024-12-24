namespace KariyerPortali5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Departman")]
    public partial class Departman
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Departman()
        {
            Pozisyon = new HashSet<Pozisyon>();
        }

        public int DepartmanId { get; set; }

        [StringLength(20)]
        public string DepartmanAdi { get; set; }

        public int? SektorId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pozisyon> Pozisyon { get; set; }

        public virtual Sektor Sektor { get; set; }
    }
}
