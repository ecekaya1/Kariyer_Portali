namespace KariyerPortali5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Kullanici")]
    public partial class Kullanici
    {
        public int KullaniciId { get; set; }

        [Required]
        [StringLength(50)]
        public string Isim { get; set; }

        [Required]
        [StringLength(50)]
        public string Soyisim { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string TelefonNo { get; set; }

        public DateTime DogumTarihi { get; set; }

        [Column(TypeName = "text")]
        public string Yetenekler { get; set; }

        [StringLength(50)]
        public string Sifre { get; set; }
    }
}
