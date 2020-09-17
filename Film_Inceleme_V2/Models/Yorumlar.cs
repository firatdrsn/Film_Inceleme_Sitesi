using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Yorumlar")]
    public class Yorumlar
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Yorum_ID { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public string Yorum { get; set; }
        public string Yorum_Yapilan_Sayfa { get; set; }
        public DateTime Yorum_Tarihi { get; set; }
        public Boolean Durumu { get; set; }


    }
}