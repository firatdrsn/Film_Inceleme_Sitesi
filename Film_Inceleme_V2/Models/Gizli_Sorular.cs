using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Film_Inceleme_V2.Models
{
    [Table("Gizli_Sorular")]
    public class Gizli_Sorular
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Gizli_Soru_ID { get; set; }
        public string Gizli_Soru { get; set; }
    }
}