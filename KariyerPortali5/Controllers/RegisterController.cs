using System;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public RegisterController()
        {
            _dbHelper = new DatabaseHelper(); // DatabaseHelper sınıfını kullan
        }

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Kullanici model)
        {
            // Formun doğruluğunu kontrol et
            if (ModelState.IsValid)
            {
                try
                {
                    // Kullanıcıyı veritabanına kaydet
                    _dbHelper.AddKullanici(model);
                    TempData["SuccessMessage"] = "Kayıt işlemi başarılı. Şimdi giriş yapabilirsiniz.";
                    return RedirectToAction("Index", "Login"); // Login sayfasına yönlendir
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Kayıt sırasında bir hata oluştu: " + ex.Message;
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Lütfen tüm alanları doğru doldurun.";
            }

            return View(model);
        }
    }
}
