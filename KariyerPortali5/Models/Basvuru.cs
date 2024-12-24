namespace KariyerPortali5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Basvuru")]
    public partial class Basvuru
    {
        public int BasvuruId { get; set; }

        public int? SirketId { get; set; }

        public int? KullaniciId { get; set; }

        [Key]
        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] BasvuruTarihi { get; set; }

        public int? BasvuruDurumuId { get; set; }

        public virtual BasvuruDurumu BasvuruDurumu { get; set; }
        public int? DepartmanId { get; set; }
        // Kullanici ile iliþki
        public virtual Kullanici Kullanici { get; set; }

        
    }
}
