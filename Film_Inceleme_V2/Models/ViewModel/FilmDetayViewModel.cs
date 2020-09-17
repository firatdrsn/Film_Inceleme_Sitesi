using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Film_Inceleme_V2.Models.ViewModel
{
    public class FilmDetayViewModel
    {
        public Filmler Film { get; set; }
        public List<Yorumlar> Yorumlar { get; set; }
        public List<Oyuncular> Oyuncular { get; set; }
        public Yonetmenler Yonetmenler { get; set; }
        public Film_Turu Film_Turu { get; set; }
        public Ulkeler Ulkeler { get; set; }
    }
}