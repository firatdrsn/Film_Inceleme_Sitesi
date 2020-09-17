using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Filmler")]
    public class Filmler
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Film_ID { get; set; }
        public string Film_Adi { get; set; }
        public virtual Yonetmenler Yonetmenler { get; set; }
        public DateTime Yapim_Yili { get; set; }
        public virtual Film_Turu Film_Turu { get; set; }
        public string Resim { get; set; }        
        public virtual Ulkeler Ulkeler { get; set; }
        public DateTime? Vizyona_Giris_Tarihi { get; set; }
        public DateTime? Vizyondan_Cikis_Tarihi { get; set; }
        public string Film_Detayi { get; set; }
        public double imdb { get; set; }
    }
}