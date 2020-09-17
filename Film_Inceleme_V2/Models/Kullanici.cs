using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Kullanici")]
    public class Kullanici
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Kullanici_ID { get; set;}
        [StringLength(20), Index(IsUnique =true),Required]
        public string Kullanici_Adi { get; set; }
        [StringLength(16), Required]
        public string Parola { get; set; }
        [StringLength(20), Required]
        public string Adi { get; set; }
        [StringLength(30), Required]
        public string Soyadi { get; set; }  
        public string E_Posta_Adresi { get; set; }
        public virtual Sehirler Sehirler { get; set; }
        public virtual Yetkilendirme Yetkilendirme { get; set; }
        public Boolean Durumu { get; set; }
        public virtual Gizli_Sorular Gizli_Sorular { get; set; }
        public string Gizli_Cevap { get; set; }
    }
}