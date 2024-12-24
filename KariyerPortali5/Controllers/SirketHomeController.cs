using System;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class SirketHomeController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public SirketHomeController()
        {
            _dbHelper = new DatabaseHelper();
        }

        public ActionResult Dashboard()
        {
            // Şirket giriş kontrolü
            if (Session["SirketId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen önce giriş yapın.";
                return RedirectToAction("Index", "SirketLogin");
            }

            int sirketId = Convert.ToInt32(Session["SirketId"]);
            Sirket sirket = _dbHelper.GetSirket2ById(sirketId);

            if (sirket == null)
            {
                TempData["ErrorMessage"] = "Şirket bilgileri bulunamadı.";
                return RedirectToAction("Index", "SirketLogin");
            }

            return View(sirket);
        }
    }
}
