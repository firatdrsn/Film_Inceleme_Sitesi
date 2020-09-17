using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Yetkilendirme")]
    public class Yetkilendirme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Yetki_ID { get; set; }
        public string Yetkisi { get; set; }
    }
}