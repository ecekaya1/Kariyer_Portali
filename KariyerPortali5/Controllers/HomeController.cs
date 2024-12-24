using System;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Dashboard()
        {
         /*   // Kullanıcı giriş kontrolü
            if (Session["UserId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen önce giriş yapın.";
                return RedirectToAction("Index", "Login"); // Kullanıcı giriş yapmadıysa login sayfasına yönlendir
            }

            int userId = Convert.ToInt32(Session["UserId"]);

            // Kullanıcı bilgilerini yükle
            DatabaseHelper dbHelper = new DatabaseHelper();
            Kullanici kullanici = dbHelper.GetAllKullanicilar().Find(k => k.KullaniciId == userId);

            if (kullanici == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı. Lütfen tekrar giriş yapın.";
                return RedirectToAction("Index", "Login");
            }*/

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Basvurularim()
        {
            return View();
        }
    }
}
