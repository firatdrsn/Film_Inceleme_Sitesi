using Film_Inceleme_V2.Models;
using Film_Inceleme_V2.Models.DbContextKlasoru;
using Film_Inceleme_V2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;
using Film_Inceleme_V2.Filter;

namespace Film_Inceleme_V2.Controllers
{
    [IsAdminFilter]
    public class YoneticiController : Controller
    {
        [IgnoreFilter]
        public ActionResult Index()
        {
            DatabaseContext db = new DatabaseContext();

            List<SelectListItem> IcerikTurListe = (from k in db.Yetkilendirme
                                                   select new SelectListItem
                                                   {
                                                       Text = k.Yetkisi,
                                                       Value = k.Yetkisi,
                                                   }).ToList();
            ViewBag.YetkiListe = IcerikTurListe;
            return View(ViewBag);
        }

        [HttpPost]
        [IgnoreFilter]
        public ActionResult Index(string username, string password)
        {
            DatabaseContext db = new DatabaseContext();
            var Kullanici = db.Kullanici.Include(x => x.Yetkilendirme).Where(x => x.Kullanici_Adi == username && x.Parola == password && x.Yetkilendirme.Yetki_ID == 1).FirstOrDefault();
            if (Kullanici != null)
            {
                Session["Kullanici"] = Kullanici;
                return RedirectToAction("Anasayfa");

            }
            else
            {
                ViewBag.Message = "Hatalı Kullanıcı Adı veya Şifre!!!";
            }
            return View();
        }

        public ActionResult Anasayfa(int? SayfaNo)
        {
            int _sayfaNo = SayfaNo ?? 1;
            DatabaseContext db = new DatabaseContext();
            var KullaniciListesi = db.Kullanici.Include(x=>x.Yetkilendirme).OrderByDescending(m => m.Kullanici_ID).ToPagedList<Kullanici>(_sayfaNo, 10);
            return View(KullaniciListesi);
        }
        public ActionResult OyuncularListesi(int? SayfaNo)
        {
            DatabaseContext db = new DatabaseContext();
            int _sayfaNo = SayfaNo ?? 1;
            var OyuncuListesi = db.Oyuncular.OrderByDescending(m => m.Oyuncular_ID).ToPagedList<Oyuncular>(_sayfaNo, 10);
            return View(OyuncuListesi);
        }
        public ActionResult YeniOyuncuEkle()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult YeniOyuncuEkle(Oyuncular oyuncu, HttpPostedFileBase file)
        {
            DatabaseContext db = new DatabaseContext();
            if (oyuncu != null)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/images/" + ImageName);
                // save image in folder
                file.SaveAs(physicalPath);
                Oyuncular yeni = new Oyuncular()
                {
                    Adi = oyuncu.Adi,
                    Soyadi = oyuncu.Soyadi,
                    Dogum_Tarihi = Convert.ToDateTime(oyuncu.Dogum_Tarihi),
                    Resim = ImageName

                };
                db.Oyuncular.Add(yeni);
                db.SaveChanges();
                return RedirectToAction("OyuncularListesi");
            }
            else
            {
                return RedirectToAction("YeniOyuncuEkle");
            }
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
                var yetkilendirme = db.Yetkilendirme.Where(x => x.Yetki_ID == 2 ).FirstOrDefault();
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
                return RedirectToAction("Anasayfa");
            }

            return RedirectToAction("Anasayfa");
        }
        public ActionResult Oyuncu_Sil(int id)
        {
            DatabaseContext db = new DatabaseContext();
            var Oyuncu = db.Oyuncular.Where(x => x.Oyuncular_ID == id).FirstOrDefault();

            db.Entry(Oyuncu).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("OyuncularListesi");


        }
        public ActionResult Oyuncu_Duzenle(int? id, string resim)
        {
            DatabaseContext db = new DatabaseContext();
            Oyuncular kayit = db.Oyuncular.Where(k => k.Oyuncular_ID == id).SingleOrDefault();
            Oyuncular yeni = new Oyuncular()
            {
                Oyuncular_ID = kayit.Oyuncular_ID,
                Adi = kayit.Adi,
                Soyadi = kayit.Soyadi,
                Dogum_Tarihi = kayit.Dogum_Tarihi,
                Resim = kayit.Resim
            };
            return View(yeni);
        }
        [HttpPost]
        public ActionResult Oyuncu_Duzenle(Oyuncular veri, HttpPostedFileBase file)
        {
            DatabaseContext db = new DatabaseContext();
            Oyuncular kayit = db.Oyuncular.Where(k => k.Oyuncular_ID == veri.Oyuncular_ID).SingleOrDefault();
            kayit.Adi = veri.Adi;
            kayit.Soyadi = veri.Soyadi;
            kayit.Dogum_Tarihi = veri.Dogum_Tarihi;
            if (file != null)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/images/" + ImageName);
                // save image in folder
                file.SaveAs(physicalPath);
                kayit.Resim = ImageName;
            }
            db.SaveChanges();
            return RedirectToAction("OyuncularListesi");
        }

        public ActionResult Kullanici_Duzenle(int id)
        {
            DatabaseContext db = new DatabaseContext();
            Kullanici kayit = db.Kullanici.Where(k => k.Kullanici_ID == id).SingleOrDefault();


            List<SelectListItem> IcerikTurListe4 = (from k in db.Yetkilendirme
                                                    select new SelectListItem
                                                    {
                                                        Text = k.Yetkisi,
                                                        Value = k.Yetki_ID.ToString(),
                                                    }).ToList();
            ViewBag.YetkiList = IcerikTurListe4;


            Kullanici yeni = new Kullanici()
            {
                Kullanici_ID = kayit.Kullanici_ID,
                Kullanici_Adi = kayit.Kullanici_Adi,
                Adi = kayit.Adi,
                Soyadi = kayit.Soyadi,
                E_Posta_Adresi = kayit.E_Posta_Adresi,
                Durumu = kayit.Durumu,
                Gizli_Sorular = kayit.Gizli_Sorular,
                Gizli_Cevap = kayit.Gizli_Cevap,
                Parola = kayit.Parola,
                Sehirler = kayit.Sehirler,
                Yetkilendirme = kayit.Yetkilendirme
            };

            return View(yeni);
        }
        [HttpPost]
        public ActionResult Kullanici_Duzenle(Kullanici veri)
        {
            DatabaseContext db = new DatabaseContext();
            Kullanici kayit = db.Kullanici.Where(k => k.Kullanici_ID == veri.Kullanici_ID).SingleOrDefault();
            var yetkilendirme = db.Yetkilendirme.FirstOrDefault(x => x.Yetki_ID==veri.Yetkilendirme.Yetki_ID);
            kayit.Kullanici_ID = veri.Kullanici_ID;
            kayit.Kullanici_Adi = veri.Kullanici_Adi;
            kayit.Adi = veri.Adi;
            kayit.Soyadi = veri.Soyadi;
            kayit.Parola = veri.Parola;
            kayit.E_Posta_Adresi = veri.E_Posta_Adresi;
            kayit.Sehirler = veri.Sehirler;
            kayit.Yetkilendirme = yetkilendirme;
            db.SaveChanges();
            return RedirectToAction("Anasayfa");
        }
        public ActionResult PanelUst()
        {
            ViewBag.Yonetim = "Yönetim Paneli";
            return PartialView();
        }

        public ActionResult KullaniciSil(int id)
        {
            if (id.ToString() != null)
            {
                DatabaseContext db = new DatabaseContext();
                var kullanici = db.Kullanici.Where(x => x.Kullanici_ID == id).FirstOrDefault();
                if (kullanici != null)
                {
                    kullanici.Durumu = false;
                    db.SaveChanges();

                    return RedirectToAction("Anasayfa");
                }
                else
                {
                    return RedirectToAction("Anasayfa");
                }
            }
            else
            {
                return RedirectToAction("Anasayfa");

            }
        }

        public ActionResult FilmeOyuncuekle(int FilmId, int Page = 0)
        {
            DatabaseContext db = new DatabaseContext();
            Session["FilmId"] = FilmId;
            OyuncuFilmViewModel model = new OyuncuFilmViewModel();
            model.Filmler = db.Filmler.Where(x => x.Film_ID == FilmId).FirstOrDefault();
            var oyuncular = db.Oyuncular.ToList();
            model.Oyuncu_Film = db.Oyuncu_Film.Where(x => x.Filmler.Film_ID == FilmId).ToList();

            model.Oyuncular = oyuncular.OrderBy(x => x.Oyuncular_ID).Skip(9 * Page).Take(9).ToList();
            int startPage = Page - 9;
            int endPage = Page + 9;
            int maxPage = Convert.ToInt32(Math.Ceiling(oyuncular.Count / 9F));

            model.StartPagination = startPage < 0 ? 1 : startPage;
            model.EndPagination = endPage > maxPage ? maxPage : endPage;
            model.OncekiSayfa = Page - 1;
            model.SonrakiSayfa = Page + 1;
            if (model.OncekiSayfa < model.StartPagination)
            {
                model.OncekiSayfa = model.StartPagination - 1;
            }
            if (model.SonrakiSayfa >= model.EndPagination)
            {
                model.SonrakiSayfa = model.EndPagination - 1;
            }

            return View(model);
        }
        public ActionResult OyuncuCikar(int OyuncuId, int FilmId,int Page)
        {
            using (var db = new DatabaseContext())
            {
                var oyuncu_film = db.Oyuncu_Film.Where(x => x.Oyuncular.Oyuncular_ID == OyuncuId && x.Filmler.Film_ID == FilmId).FirstOrDefault();
                db.Oyuncu_Film.Remove(oyuncu_film);
                db.SaveChanges();
            }
                
                return RedirectToAction("FilmeOyuncuEkle", new { FilmId = FilmId, Page = Page });
            }
          
        public ActionResult Oyuncuekle(int OyuncuId, int FilmId,int Page)
        {
            DatabaseContext db = new DatabaseContext();
            var oyuncu = db.Oyuncular.Where(x => x.Oyuncular_ID == OyuncuId).FirstOrDefault();
            var film = db.Filmler.Where(x => x.Film_ID == FilmId).FirstOrDefault();
            Oyuncu_Film yeni = new Oyuncu_Film()
            {

                Oyuncular = oyuncu,
                Filmler = film

            };
            db.Oyuncu_Film.Add(yeni);
            db.SaveChanges();
            return RedirectToAction("FilmeOyuncuEkle", new { FilmId = FilmId ,Page=Page});
        }
        public ActionResult YeniFilm()
        {
            DatabaseContext db = new DatabaseContext();
            List<SelectListItem> IcerikTurListe3 = (from k in db.Ulkeler
                                                    select new SelectListItem
                                                    {
                                                        Text = k.Ulke,
                                                        Value = k.Ulke,
                                                    }).ToList();
            ViewBag.UlkeListe = IcerikTurListe3;
            List<SelectListItem> IcerikTurListe4 = (from k in db.Yonetmenler
                                                    select new SelectListItem
                                                    {
                                                        Text = k.Adi + " " + k.Soyadi,
                                                        Value = k.Adi,
                                                    }).ToList();
            ViewBag.YonetmenListe = IcerikTurListe4;
            List<SelectListItem> IcerikTurListe5 = (from k in db.Film_Turu
                                                    select new SelectListItem
                                                    {
                                                        Text = k.Turu,
                                                        Value = k.Turu,
                                                    }).ToList();
            ViewBag.Film_Turu = IcerikTurListe5;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult YeniFilm(Filmler filmler, string Film_Turu, string UlkeListe, string YonetmenListe,string Film_Detayi, DateTime? vizyona_giris_tarihi,DateTime? vizyondan_cikis_tarihi,double imdb, HttpPostedFileBase file)
        {

            if (filmler != null)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/images/" + ImageName);
                // save image in folder
                file.SaveAs(physicalPath);
                //Eklediğimiz Resni Server.MapPath methodu ile Dosya Adıyla birlikte kaydettik.
                //Ve veritabanına kayıt için dosya adımızı değişkene aktarıyoruz.
                DatabaseContext db = new DatabaseContext();
                List<SelectListItem> IcerikTurListe3 = (from k in db.Ulkeler
                                                        select new SelectListItem
                                                        {
                                                            Text = k.Ulke,
                                                            Value = k.Ulke,
                                                        }).ToList();
                ViewBag.UlkeListe = IcerikTurListe3;
                List<SelectListItem> IcerikTurListe4 = (from k in db.Yonetmenler
                                                        select new SelectListItem
                                                        {
                                                            Text = k.Adi,
                                                            Value = k.Adi,
                                                        }).ToList();
                ViewBag.YonetmenListe = IcerikTurListe4;
                List<SelectListItem> IcerikTurListe5 = (from k in db.Film_Turu
                                                        select new SelectListItem
                                                        {
                                                            Text = k.Turu,
                                                            Value = k.Turu,
                                                        }).ToList();
                ViewBag.Film_Turu = IcerikTurListe5;
                var ulkeler = db.Ulkeler.Where(x => x.Ulke == UlkeListe).FirstOrDefault();
                var yonetmenler = db.Yonetmenler.Where(x => x.Adi == YonetmenListe).FirstOrDefault();
                var film_turu = db.Film_Turu.Where(x => x.Turu == Film_Turu).FirstOrDefault();
                Filmler yeni = new Filmler()
                {
                    Film_Adi = filmler.Film_Adi,
                    Film_Turu = film_turu,
                    Ulkeler = ulkeler,
                    Yapim_Yili = filmler.Yapim_Yili,
                    Yonetmenler = yonetmenler,
                    Resim = ImageName,
                    Vizyona_Giris_Tarihi=vizyona_giris_tarihi,
                    Vizyondan_Cikis_Tarihi=vizyondan_cikis_tarihi,
                    Film_Detayi= Film_Detayi,
                    imdb=imdb
                };
                db.Filmler.Add(yeni);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("Filmler");
            }

            return RedirectToAction("Filmler");
        }
        public ActionResult Filmler(int? SayfaNo)
        {

            int _sayfaNo = SayfaNo ?? 1;
            DatabaseContext db = new DatabaseContext();

            var FilmListesi = db.Filmler.OrderByDescending(m => m.Film_ID).ToPagedList<Filmler>(_sayfaNo, 10);

            return View(FilmListesi);
        }
        public ActionResult Film_Sil(int id)
        {
            if (id.ToString() != null)
            {
                DatabaseContext db = new DatabaseContext();
                var Film = db.Filmler.Where(x => x.Film_ID == id).FirstOrDefault();
                if (Film != null)
                {
                    db.Entry(Film).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();

                    return RedirectToAction("Filmler");
                }
                else
                {
                    return RedirectToAction("Filmler");
                }
            }
            else
            {
                return RedirectToAction("Filmler");

            }

        }
        public ActionResult Film_Duzenle(int id)
        {
            DatabaseContext db = new DatabaseContext();


            Filmler yeni = db.Filmler.FirstOrDefault(x => x.Film_ID == id);
            List<SelectListItem> IcerikTurListe = (from k in db.Film_Turu
                                                   select new SelectListItem
                                                   {
                                                       Text = k.Turu,
                                                       Value = k.Film_Turu_ID.ToString(),
                                                       Selected = (yeni.Film_Turu.Turu == k.Turu) ? true : false
                                                   }).ToList();
            ViewBag.FilmListe = IcerikTurListe;
            List<SelectListItem> IcerikTurListe2 = (from k in db.Ulkeler
                                                    select new SelectListItem
                                                    {
                                                        Text = k.Ulke,
                                                        Value = k.Ulke_ID.ToString(),
                                                        Selected = (yeni.Ulkeler.Ulke == k.Ulke) ? true : false
                                                    }).ToList();
            ViewBag.UlkeListe = IcerikTurListe2;
            List<SelectListItem> IcerikTurListe3 = (from k in db.Yonetmenler
                                                    select new SelectListItem
                                                    {
                                                        Text = k.Adi,
                                                        Value = k.Yonetmenler_ID.ToString(),
                                                        Selected = (yeni.Yonetmenler.Adi == k.Adi) ? true : false
                                                    }).ToList();
            ViewBag.YonetmenListe = IcerikTurListe3;
           
            Filmler kayit = db.Filmler.Where(k => k.Film_ID == id).SingleOrDefault();


            return View(yeni);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Film_Duzenle(Filmler veri, HttpPostedFileBase file)
        {
            DatabaseContext db = new DatabaseContext();


            Filmler kayit = db.Filmler.Where(k => k.Film_ID == veri.Film_ID).SingleOrDefault();
            var film_turu = db.Film_Turu.FirstOrDefault(x => x.Film_Turu_ID == veri.Film_Turu.Film_Turu_ID);
            var ulkeler = db.Ulkeler.FirstOrDefault(x => x.Ulke_ID == veri.Ulkeler.Ulke_ID);
            var yonetmen = db.Yonetmenler.FirstOrDefault(x => x.Yonetmenler_ID== veri.Yonetmenler.Yonetmenler_ID);
            kayit.Film_Adi = veri.Film_Adi;
            kayit.Film_Turu = film_turu;
            kayit.Ulkeler = ulkeler;
            kayit.Yonetmenler = yonetmen;
            kayit.Film_Detayi = veri.Film_Detayi;
            kayit.imdb = veri.imdb;
            if (file != null)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/images/" + ImageName);
                // save image in folder
                file.SaveAs(physicalPath);
                kayit.Resim = ImageName;
            }
            db.SaveChanges();
            return RedirectToAction("Filmler");
        }

        public ActionResult YonetmenlerListesi(int? SayfaNo)
        {
            DatabaseContext db = new DatabaseContext();
            int _sayfaNo = SayfaNo ?? 1;
            var YonetmenlerListesi = db.Yonetmenler.OrderByDescending(m => m.Yonetmenler_ID).ToPagedList<Yonetmenler>(_sayfaNo, 10);
            return View(YonetmenlerListesi);
        }
        public ActionResult YeniYonetmenEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniYonetmenEkle(Yonetmenler yonetmen)
        {
            DatabaseContext db = new DatabaseContext();
            if (yonetmen != null)
            {

                Yonetmenler yeni = new Yonetmenler()
                {
                    Adi = yonetmen.Adi,
                    Soyadi = yonetmen.Soyadi,
                    Dogum_Tarihi = Convert.ToDateTime(yonetmen.Dogum_Tarihi),

                };
                db.Yonetmenler.Add(yeni);
                db.SaveChanges();
                return RedirectToAction("YonetmenlerListesi");
            }
            else
            {
                return RedirectToAction("YeniYonetmenEkle");
            }
        }
        public ActionResult Yonetmen_Sil(int id)
        {
            if (id.ToString() != null)
            {
                DatabaseContext db = new DatabaseContext();
                var Yonetmen = db.Yonetmenler.Where(x => x.Yonetmenler_ID == id).FirstOrDefault();
                if (Yonetmen != null)
                {
                    db.Entry(Yonetmen).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();

                    return RedirectToAction("YonetmenlerListesi");
                }
                else
                {
                    return RedirectToAction("YonetmenlerListesi");
                }
            }
            else
            {
                return RedirectToAction("YonetmenlerListesi");

            }

        }
        public ActionResult Yonetmen_Duzenle(int id)
        {
            using(var db = new DatabaseContext())
            {
                Yonetmenler kayit = db.Yonetmenler.Where(k => k.Yonetmenler_ID == id).SingleOrDefault();
                Yonetmenler yeni = new Yonetmenler()
                {
                    Yonetmenler_ID = kayit.Yonetmenler_ID,
                    Adi = kayit.Adi,
                    Soyadi = kayit.Soyadi,
                    Dogum_Tarihi = kayit.Dogum_Tarihi
                };
                return View(yeni);
            }
            
        }
        [HttpPost]
        public ActionResult Yonetmen_Duzenle(Yonetmenler veri)
        {
            using (var db = new DatabaseContext())
            {
                Yonetmenler kayit = db.Yonetmenler.Where(k => k.Yonetmenler_ID == veri.Yonetmenler_ID).SingleOrDefault();
                kayit.Adi = veri.Adi;
                kayit.Soyadi = veri.Soyadi;
                kayit.Dogum_Tarihi = veri.Dogum_Tarihi;
                db.SaveChanges();
                return RedirectToAction("YonetmenlerListesi");
            }
                
        }


        public ActionResult Yorumlar()
        {
            using (var db = new DatabaseContext())
            {
                var yorum = db.Yorumlar.Where(x => x.Durumu == false).Include(x => x.Kullanici).ToList();
                foreach (var item in yorum)
                {
                    int filmId = Convert.ToInt32(item.Yorum_Yapilan_Sayfa);
                    item.Yorum_Yapilan_Sayfa = db.Filmler.FirstOrDefault(x => x.Film_ID == filmId).Film_Adi;
                }
                return View(yorum);
            }
        }

        public ActionResult YorumOnayla(int Id)
        {
            using (var db = new DatabaseContext())
            {
                var yorum = db.Yorumlar.FirstOrDefault(x => x.Yorum_ID == Id);
                yorum.Durumu = true;
                db.SaveChanges();
            }

            return RedirectToAction("Yorumlar");
        }

        public ActionResult YorumSil(int Id)
        {
            using (var db = new DatabaseContext())
            {
                var yorum = db.Yorumlar.FirstOrDefault(x => x.Yorum_ID == Id);
                db.Yorumlar.Remove(yorum);
                db.SaveChanges();
            }

            return RedirectToAction("Yorumlar");
        }

        public ActionResult KullaniciAktifEt(int id)
        {
            if (id.ToString() != null)
            {
                DatabaseContext db = new DatabaseContext();
                var kullanici = db.Kullanici.Where(x => x.Kullanici_ID == id).FirstOrDefault();
                if (kullanici != null)
                {
                    kullanici.Durumu = true;
                    db.SaveChanges();

                    return RedirectToAction("Anasayfa");
                }
                else
                {
                    return RedirectToAction("Anasayfa");
                }
            }
            else
            {
                return RedirectToAction("Anasayfa");

            }
        }
    }
}