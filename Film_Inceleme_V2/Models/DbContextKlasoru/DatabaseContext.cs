using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Film_Inceleme_V2.Models.DbContextKlasoru
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Film_Turu> Film_Turu { get; set; }
        public DbSet<Filmler> Filmler { get; set; }
        public DbSet<Kullanici> Kullanici { get; set; }
        public DbSet<Oyuncu_Film> Oyuncu_Film { get; set; }
        public DbSet<Oyuncular> Oyuncular { get; set; }
        public DbSet<Sehirler> Sehirler { get; set; }
        public DbSet<Ulkeler> Ulkeler { get; set; }
        public DbSet<Yetkilendirme> Yetkilendirme { get; set; }
        public DbSet<Yonetmenler> Yonetmenler { get; set; }
        public DbSet<Yorumlar> Yorumlar { get; set; }
        public DbSet<Gizli_Sorular> Gizli_Sorular { get; set; }
    }
    public class VeriTabani : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            List<Kullanici> Kullanici = context.Kullanici.ToList();
            List<Film_Turu> Film_Turu = context.Film_Turu.ToList();
            List<Filmler> Filmler = context.Filmler.ToList();
            List<Oyuncu_Film> Oyuncu_Film = context.Oyuncu_Film.ToList();
            List<Oyuncular> Oyuncular = context.Oyuncular.ToList();
            List<Sehirler> Sehirler = context.Sehirler.ToList();
            List<Ulkeler> Ulkeler = context.Ulkeler.ToList();
            List<Yetkilendirme> Yetkilendirme = context.Yetkilendirme.ToList();
            List<Yonetmenler> Yonetmenler = context.Yonetmenler.ToList();
            List<Yorumlar> Yorumlar = context.Yorumlar.ToList();
            List<Gizli_Sorular> Gizli_Sorular = context.Gizli_Sorular.ToList();
        }
    }

}