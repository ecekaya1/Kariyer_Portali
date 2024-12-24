using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class BasvurularimController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public BasvurularimController()
        {
            _dbHelper = new DatabaseHelper();
        }

        public ActionResult Index()
        {
            if (Session["SirketId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Login", "SirketLogin");
            }

            int sirketId = Convert.ToInt32(Session["SirketId"]);

            try
            {
                var basvurular = _dbHelper.GetBasvurularBySirketId(sirketId);
                return View(basvurular);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Başvurular alınırken bir hata oluştu: " + ex.Message;
                return View(new List<Basvuru>());
            }
        }

    }
}
