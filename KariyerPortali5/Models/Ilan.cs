namespace KariyerPortali5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ilan")]
    public partial class Ilan
    {
        public int IlanId { get; set; }

        public int SirketId { get; set; }

        public int? PozisyonId { get; set; }

        public int Maas { get; set; }

        public int? KonumId { get; set; }

        [Column(TypeName = "text")]
        public string Aciklama { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] YayınTarihi { get; set; }

        public DateTime? SonBasvuruTarihi { get; set; }

        [Column(TypeName = "text")]
        public string Nitelikler { get; set; }

        public virtual Konum Konum { get; set; }

        public virtual Pozisyon Pozisyon { get; set; }

        public virtual Sirket Sirket { get; set; }
    }
}
