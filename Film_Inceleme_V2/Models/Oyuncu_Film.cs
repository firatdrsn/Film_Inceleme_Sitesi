using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Film_Inceleme_V2.Models
{
    [Table("Oyuncu_Film")]
    public class Oyuncu_Film
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Oyuncu_Film_ID { get; set; }
        public virtual Oyuncular Oyuncular { get; set; }
        public virtual Filmler Filmler { get; set; }
    }
}