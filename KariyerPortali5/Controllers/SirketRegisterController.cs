using System;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class SirketRegisterController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public SirketRegisterController()
        {
            _dbHelper = new DatabaseHelper();
        }

        // GET: SirketRegister
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Sirket model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbHelper.AddSirket(model);
                    TempData["SuccessMessage"] = "Kayıt işlemi başarılı. Şimdi giriş yapabilirsiniz.";
                    return RedirectToAction("Index", "SirketLogin");
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
