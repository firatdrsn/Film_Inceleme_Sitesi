using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Film_Turu")]
    public class Film_Turu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Film_Turu_ID { get; set; }
        public string Turu { get; set; }
    }
}