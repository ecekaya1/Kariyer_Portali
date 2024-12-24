using System;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class SirketProfilController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public SirketProfilController()
        {
            _dbHelper = new DatabaseHelper();
        }

        // Şirket Profilini Görüntüle
        public ActionResult Index()
        {
            if (Session["SirketId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Index", "SirketLogin");
            }

            int sirketId = Convert.ToInt32(Session["SirketId"]);
            Sirket sirket = _dbHelper.GetAllSirketler().Find(s => s.SirketId == sirketId);

            if (sirket == null)
            {
                TempData["ErrorMessage"] = "Şirket bilgileri bulunamadı.";
                return RedirectToAction("Dashboard", "SirketHome");
            }

            // Sektörler ve konumlar ViewBag'e aktarılıyor
            ViewBag.Sektorler = _dbHelper.GetAllSektorler();
            ViewBag.Konumlar = _dbHelper.GetAllKonumlar();

            return View(sirket);
        }


        // Şirket Profilini Güncelle
        [HttpPost]
        public ActionResult Update(Sirket model)
        {
            if (Session["SirketId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Index", "SirketLogin");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.SirketId = Convert.ToInt32(Session["SirketId"]); // Şirket ID'sini session'dan al
                    _dbHelper.UpdateSirket(model);
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
