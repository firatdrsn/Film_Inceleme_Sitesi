using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Sehirler")]
    public class Sehirler
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sehir_ID { get; set; }
        public string Sehir { get; set; }
    }
}