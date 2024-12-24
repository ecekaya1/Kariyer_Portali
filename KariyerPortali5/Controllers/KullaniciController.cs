using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class KullaniciController : Controller
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        // 1. Kullanıcı Listeleme
        public ActionResult Index()
        {
            var kullanicilar = dbHelper.GetAllKullanicilar();
            return View(kullanicilar);
        }

        // 2. Kullanıcı Ekleme Sayfası
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Kullanici kullanici)
        {
            dbHelper.AddKullanici(kullanici);
            return RedirectToAction("Index");
        }

        // 3. Kullanıcı Güncelleme Sayfası
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var kullanici = dbHelper.GetAllKullanicilar().Find(k => k.KullaniciId == id);
            return View(kullanici);
        }

        [HttpPost]
        public ActionResult Edit(Kullanici kullanici)
        {
            dbHelper.UpdateKullanici(kullanici);
            return RedirectToAction("Index");
        }

        // 4. Kullanıcı Silme
        public ActionResult Delete(int id)
        {
            dbHelper.DeleteKullanici(id);
            return RedirectToAction("Index");
        }
    }
}