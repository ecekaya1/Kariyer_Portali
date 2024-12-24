using System;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class ProfilController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public ProfilController()
        {
            _dbHelper = new DatabaseHelper();
        }

        // GET: Profil
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Index", "Login");
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            Kullanici kullanici = _dbHelper.GetAllKullanicilar().Find(k => k.KullaniciId == userId);

            if (kullanici == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bilgileri bulunamadı.";
                return RedirectToAction("Dashboard", "Home");
            }

            return View(kullanici);
        }

        // POST: Profil Güncelle
        [HttpPost]
        public ActionResult Update(Kullanici model)
        {
            if (Session["UserId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.KullaniciId = Convert.ToInt32(Session["UserId"]); // Kullanıcı ID'sini session'dan al
                    _dbHelper.UpdateKullanici(model);
                    TempData["SuccessMessage"] = "Profil bilgileri başarıyla güncellendi.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen tüm alanları doldurun.";
            }

            return RedirectToAction("Index");
        }
    }
}
