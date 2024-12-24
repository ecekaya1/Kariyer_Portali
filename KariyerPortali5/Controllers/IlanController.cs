using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class IlanController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public IlanController()
        {
            _dbHelper = new DatabaseHelper();  // Initialize the DatabaseHelper instance
        }

        [HttpPost]
        public ActionResult BasvuruYap(int ilanId, int basvuruDurumuId = 1) // Varsayılan değer atanıyor
        {
            if (Session["UserId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Index", "Login"); // Giriş yapılmamışsa login sayfasına yönlendir
            }

            int kullaniciId = Convert.ToInt32(Session["UserId"]);

            try
            {
                // Daha önce başvuru yapılıp yapılmadığını kontrol et
                if (_dbHelper.HasUserAppliedForJob(kullaniciId, ilanId))
                {
                    TempData["ErrorMessage"] = "Bu ilana daha önce başvuru yaptınız.";
                    return RedirectToAction("Details", "Ilan", new { id = ilanId });
                }

                // Başvuru yap
                _dbHelper.BasvuruYap(kullaniciId, ilanId, basvuruDurumuId);
                TempData["SuccessMessage"] = "Başvurunuz başarıyla kaydedildi!";
                return RedirectToAction("Basvurularim", "Ilan"); // Başvurularım sayfasına yönlendir
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("Details", "Ilan", new { id = ilanId });
            }
        }

        public ActionResult Index(string sektor = null, string sehir = null, string pozisyon = null, string maasSiralama = null, bool filtreMaaş = false)
        {
            try
            {
                List<Ilan> ilanlar;

                if (filtreMaaş)
                {
                    // Maaşı 100.000'den fazla olan ilanları getir
                    ilanlar = _dbHelper.GetIlanlarWithHighSalary();
                }
                else
                {
                    // Diğer filtreleme seçenekleri
                    ilanlar = _dbHelper.GetAllIlanlar(sektor, sehir, pozisyon, maasSiralama);
                }

                return View(ilanlar);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return View(new List<Ilan>());
            }
        }


        public ActionResult Basvurularim()
        {
            if (Session["UserId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Index", "Login");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            var basvurular = _dbHelper.GetBasvurularByKullaniciId(userId);
            return View(basvurular);
        }

        private int GetCurrentUserId()
        {
            // TempData veya Session'dan giriş yapmış kullanıcının ID'sini alın
            if (TempData["UserEmail"] != null)
            {
                string email = TempData["UserEmail"].ToString();
                return _dbHelper.GetKullaniciIdByEmail(email);  // Fetch user ID by email
            }
            return 0; // Kullanıcı bulunamazsa 0 döner
        }

        public ActionResult Details(int id)
        {
            try
            {
                Ilan ilan = _dbHelper.GetIlanById(id); // Veritabanından ilanı al
                if (ilan == null)
                {
                    TempData["ErrorMessage"] = "İlan bulunamadı.";
                    return RedirectToAction("Index");
                }
                return View(ilan); // İlan detaylarını view'e gönder
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
