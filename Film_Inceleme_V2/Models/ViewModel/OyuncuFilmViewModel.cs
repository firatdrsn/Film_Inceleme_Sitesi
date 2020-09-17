using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Film_Inceleme_V2.Models.ViewModel
{
    public class OyuncuFilmViewModel
    {
        public List<Oyuncular> Oyuncular { get; set; }
        public Filmler Filmler { get; set; }
        public List<Oyuncu_Film> Oyuncu_Film { get; set; }
        public int StartPagination { get; set; }
        public int EndPagination { get; set; }
        public int OncekiSayfa { get; set; }
        public int SonrakiSayfa { get; set; }
    }
}