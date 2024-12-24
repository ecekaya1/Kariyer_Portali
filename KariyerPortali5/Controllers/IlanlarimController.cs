using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class IlanlarimController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public IlanlarimController()
        {
            _dbHelper = new DatabaseHelper();
        }

        // Şirketin ilanlarını listeleyen action
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["SirketId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Login", "Sirket");
            }

            int sirketId = Convert.ToInt32(Session["SirketId"]);
            List<Ilan> ilanlar = _dbHelper.GetIlanlarFromView();

            return View(ilanlar);
        }

        // İlan oluşturma formunu gösteren action
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Konumlar = _dbHelper.GetAllKonumlar();
            ViewBag.Pozisyonlar = _dbHelper.GetAllPozisyonlar();
            return View();
        }

        // Yeni ilan oluşturma işlemi
        [HttpPost]
        public ActionResult Create(Ilan ilan)
        {
            if (Session["SirketId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Login", "Sirket");
            }

            ilan.SirketId = Convert.ToInt32(Session["SirketId"]);
            ilan.YayınTarihi = BitConverter.GetBytes(DateTime.Now.ToBinary());

            if (ModelState.IsValid)
            {
                try
                {
                    _dbHelper.AddIlan(ilan);
                    TempData["SuccessMessage"] = "İlan başarıyla oluşturuldu.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "İlan oluşturulurken bir hata oluştu: " + ex.Message;
                }
            }

            ViewBag.Konumlar = _dbHelper.GetAllKonumlar();
            ViewBag.Pozisyonlar = _dbHelper.GetAllPozisyonlar();
            return View(ilan);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["SirketId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Index", "SirketLogin");
            }

            var ilan = _dbHelper.GetIlanById(id);

            if (ilan == null )
            {
                TempData["ErrorMessage"] = "İlan bulunamadı veya yetkiniz yok.";
                return RedirectToAction("Index");
            }

            ViewBag.Pozisyonlar = _dbHelper.GetAllPozisyonlar();
            ViewBag.Konumlar = _dbHelper.GetAllKonumlar();
            return View(ilan);
        }
        // silme işlemi
        [HttpPost]
        public ActionResult Delete(int ilanId)
        {
            try
            {
                _dbHelper.DeleteIlan(ilanId); // İlanı sil
                TempData["SuccessMessage"] = "İlan başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "İlan silinirken bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index"); // İlanlarım sayfasına yönlendir
        }


        // Düzenleme İşlemini Yap
        [HttpPost]
        public ActionResult Edit(Ilan model)
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
                    _dbHelper.UpdateIlan(model);
                    TempData["SuccessMessage"] = "İlan başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                }
            }

            ViewBag.Pozisyonlar = _dbHelper.GetAllPozisyonlar();
            ViewBag.Konumlar = _dbHelper.GetAllKonumlar();
            return View(model);
        }

    }
}
