using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Film_Inceleme_V2.Models.ViewModel
{
    public class FilmListViewModel
    {
        public string Title { get; set; }
        public int TurId { get; set; }
        public string Description { get; set; }
        public List<Filmler> Filmler { get; set; }

        public int StartPagination { get; set; }
        public int EndPagination { get; set; }
        public int OncekiSayfa { get; set; }
        public int SonrakiSayfa { get; set; }
    }
}