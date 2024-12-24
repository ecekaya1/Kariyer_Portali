using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class SirketLoginController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public SirketLoginController()
        {
            _dbHelper = new DatabaseHelper();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string sifre)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
            {
                TempData["ErrorMessage"] = "Email ve şifre alanları boş bırakılamaz.";
                return View();
            }

            Sirket sirket = _dbHelper.GetSirketByEmailAndPassword(email, sifre);
            if (sirket != null)
            {
                Session["SirketId"] = sirket.SirketId;
                Session["SirketAdi"] = sirket.SirketAdi;

                return RedirectToAction("Dashboard", "SirketHome");
            }

            TempData["ErrorMessage"] = "Geçersiz email veya şifre.";
            return View();
        }


        public ActionResult Logout()
        {
            Session.Clear(); // Tüm oturum bilgilerini temizle
            return RedirectToAction("Index", "SirketLogin"); // Login sayfasına yönlendir
        }
        


    }
}
