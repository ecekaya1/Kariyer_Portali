using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class SirketController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public SirketController()
        {
            _dbHelper = new DatabaseHelper();
        }

        // GET: Şirketler
        public ActionResult Index(string sektor, string sehir, string sirketAdi)
        {
            try
            {
                // Şirket listesini ve filtreleri al
                var sirketler = _dbHelper.GetAllSirketler(sektor, sehir, sirketAdi);

                // Mevcut sektörleri ve şehirleri ViewBag ile gönder
                ViewBag.Sektorler = _dbHelper.GetAllSektorler();
                ViewBag.Sehirler = _dbHelper.GetAllSehirler();

                return View(sirketler);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return View(new List<Sirket>());
            }
        }
    }
}
