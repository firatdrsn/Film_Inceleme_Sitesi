using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Ulkeler")]
    public class Ulkeler
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Ulke_ID { get; set; }
        public string Ulke { get; set; }
    }
}