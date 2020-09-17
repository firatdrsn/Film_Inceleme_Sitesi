using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Oyuncular")]
    public class Oyuncular
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Oyuncular_ID { get; set; }
        [StringLength(20), Required] 
        public string Adi { get; set; }
         [StringLength(30), Required]
        public string Soyadi { get; set; }
       
        public DateTime Dogum_Tarihi { get; set; }
        public string Resim { get; set; }

    }
}