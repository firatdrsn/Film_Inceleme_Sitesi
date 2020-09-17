using Film_Inceleme_V2.Models.DbContextKlasoru;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using Film_Inceleme_V2.Models;
using Film_Inceleme_V2.Models.ViewModel;
using System.Data.Entity;

namespace Film_Inceleme_V2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var db = new DatabaseContext())
            {
                if (Session["Film_Turu"] == null)
                    Session["Film_Turu"] = db.Film_Turu.ToList();
                if (Session["Son_Eklenen_Film"] == null)
                    Session["Son_Eklenen_Film"] = db.Filmler.FirstOrDefault(x => x.Film_ID == db.Filmler.Max(y => y.Film_ID));

                DateTime nowDate = DateTime.Now;
                var filmler = db.Filmler.Where(x => x.Vizyona_Giris_Tarihi <= nowDate && x.Vizyondan_Cikis_Tarihi >= nowDate).Include(x=>x.Yonetmenler).Include(x=>x.Ulkeler).ToList();

                return View(filmler);
            }
        }
        public ActionResult ListFilmTuru(int FilmTuruId, int Page = 0)
        {
            DatabaseContext db = new DatabaseContext();

            FilmListViewModel model = new FilmListViewModel();
            model.Title = db.Film_Turu.FirstOrDefault(x => x.Film_Turu_ID == FilmTuruId)?.Turu;
            model.TurId = FilmTuruId;
            model.Title = model.Title + " Türünde Filmler";
            
            var films = db.Filmler.Where(x => x.Film_Turu.Film_Turu_ID == FilmTuruId).ToList();

            model.Filmler = films.OrderBy(x => x.Film_ID).Skip(6 * Page).Take(6).ToList();
            int startPage = Page - 6;
            int endPage = Page + 6;
            int maxPage = Convert.ToInt32(Math.Ceiling(films.Count / 6F));

            model.StartPagination = startPage < 0 ? 1 : startPage;
            model.EndPagination = endPage > maxPage ? maxPage : endPage;
            model.OncekiSayfa = Page - 1;
            model.SonrakiSayfa = Page + 1;
            if (model.OncekiSayfa<model.StartPagination)
            {
                model.OncekiSayfa = model.StartPagination-1;
            }
            if (model.SonrakiSayfa >= model.EndPagination)
            {
                model.SonrakiSayfa = model.EndPagination-1;
            }
            
            return View(model);
        }


        public ActionResult GetFilmDetay(int FilmId)
        {
            FilmDetayViewModel model = new FilmDetayViewModel();
            using (var db = new DatabaseContext())
            {
                model.Film = db.Filmler.Where(x => x.Film_ID == FilmId).Include(x => x.Yonetmenler).Include(x=>x.Film_Turu).Include(x=>x.Ulkeler).FirstOrDefault();
                //model.Yorumlar = db.Yorumlar.Where(x => x.Yorum_Yapilan_Sayfa == FilmId.ToString() && x.Durumu == true).OrderByDescending(x=>x.Yorum_Tarihi).Include(x => x.Kullanici).ToList();
                model.Yorumlar = db.Yorumlar.Where(x => x.Yorum_Yapilan_Sayfa == FilmId.ToString() && x.Durumu == true).Include(x => x.Kullanici).ToList();
                model.Oyuncular = db.Oyuncu_Film.Where(x => x.Filmler.Film_ID == FilmId).Include(x => x.Oyuncular).Select(x => x.Oyuncular).ToList();


            }
            return View(model);
        }

        public ActionResult Giris(string link, string p)
        {
            TempData["link"] = link;
            TempData["p"] = p;

            return View();
        }
        [HttpPost]
        [ActionName("Giris")]
        public ActionResult GirisPost(string username, string password)
        {
            DatabaseContext db = new DatabaseContext();
            var Kullanici = db.Kullanici.Where(x => x.Kullanici_Adi == username && x.Parola == password && x.Durumu==true).FirstOrDefault();
            if (Kullanici != null)
            {
                Session["Kullanici"] = Kullanici;

                string link = TempData["link"] != null ? TempData["link"].ToString() : "";
                string p = TempData["p"] != null ? TempData["p"].ToString() : "";

                if (!String.IsNullOrEmpty(link) && !String.IsNullOrEmpty(p))
                {
                    return RedirectToAction(link, new { FilmId = p });
                }
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Message = "Hatalı Kullanıcı Adı veya Şifre!!!";
            }
            return View();
        }
        public ActionResult Cikis()
        {
            Session["Kullanici"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult YeniKullanici()
        {
            DatabaseContext db = new DatabaseContext();
            List<SelectListItem> IcerikTurListe = (from k in db.Gizli_Sorular
                                                   select new SelectListItem
                                                   {
                                                       Text = k.Gizli_Soru,
                                                       Value = k.Gizli_Soru,
                                                   }).ToList();
            ViewBag.Liste = IcerikTurListe;
            List<SelectListItem> IcerikTurListe2 = (from k in db.Sehirler
                                                    select new SelectListItem
                                                    {
                                                        Text = k.Sehir,
                                                        Value = k.Sehir,
                                                    }).ToList();
            ViewBag.SehirListe = IcerikTurListe2;
            return View();
        }
        [HttpPost]
        public ActionResult YeniKullanici(Kullanici kullanici, string Liste, string SehirListe)
        {

            if (kullanici != null)
            {
                DatabaseContext db = new DatabaseContext();
                List<SelectListItem> IcerikTurListe = (from k in db.Gizli_Sorular
                                                       select new SelectListItem
                                                       {
                                                           Text = k.Gizli_Soru,
                                                           Value = k.Gizli_Soru,
                                                       }).ToList();
                ViewBag.Liste = IcerikTurListe;
                List<SelectListItem> IcerikTurListe2 = (from k in db.Sehirler
                                                        select new SelectListItem
                                                        {
                                                            Text = k.Sehir,
                                                            Value = k.Sehir,
                                                        }).ToList();
                ViewBag.SehirListe = IcerikTurListe2;
                var gizli_soru = db.Gizli_Sorular.Where(x => x.Gizli_Soru == Liste).FirstOrDefault();
                var sehirler = db.Sehirler.Where(x => x.Sehir == SehirListe).FirstOrDefault();
                var yetkilendirme = db.Yetkilendirme.Where(x => x.Yetki_ID == 3).FirstOrDefault();
                Kullanici yeni = new Kullanici()
                {
                    Adi = kullanici.Adi,
                    Soyadi = kullanici.Soyadi,
                    Kullanici_Adi = kullanici.Kullanici_Adi,
                    Parola = kullanici.Parola,
                    E_Posta_Adresi = kullanici.E_Posta_Adresi,
                    Gizli_Cevap = kullanici.Gizli_Cevap,
                    Durumu = true,
                    Sehirler = sehirler,
                    Gizli_Sorular = gizli_soru,
                    Yetkilendirme = yetkilendirme
                };
                db.Kullanici.Add(yeni);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("/YeniKullanici");
            }

            return RedirectToAction("/Giris");
        }
        public ActionResult Arama(string aranan)
        {
            DatabaseContext db = new DatabaseContext();

            FilmListViewModel model = new FilmListViewModel();
            model.Filmler = db.Filmler.Where(x => x.Film_Adi.Contains(aranan)).ToList();
            model.Title = aranan + " ifadesi ile gelen filmler";

            model.StartPagination = -1;
            return View("ListFilmTuru", model);
        }
        public ActionResult YorumYap(string Yorum, string Sayfa)
        {
            if (Session["Kullanici"] != null)
            {

                using (var db = new DatabaseContext())
                {

                    var kullanici = (Kullanici)Session["Kullanici"];
                    var dbKUllanici = db.Kullanici.FirstOrDefault(x => x.Kullanici_ID == kullanici.Kullanici_ID);

                    Yorumlar yorum = new Yorumlar
                    {
                        Yorum = Yorum,
                        Kullanici = dbKUllanici,
                        Yorum_Tarihi = DateTime.Now,
                        Yorum_Yapilan_Sayfa = Sayfa,
                        Durumu = false
                    };
                    db.Yorumlar.Add(yorum);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("GetFilmDetay", new { FilmId = Sayfa });

        }

    }
}