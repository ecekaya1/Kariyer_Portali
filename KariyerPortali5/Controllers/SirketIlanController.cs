using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class SirketIlanController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        public SirketIlanController()
        {
            _dbHelper = new DatabaseHelper();
        }

        // İlan oluşturma formunu gösteren action
        [HttpGet]
        public ActionResult Create()
        {
            // Konum ve pozisyon gibi dropdown verilerini doldur
            ViewBag.Konumlar = _dbHelper.GetAllKonumlar();
            ViewBag.Pozisyonlar = _dbHelper.GetAllPozisyonlar();
            return View();
        }

        // İlan oluşturma işlemini yapan action
        [HttpPost]
        public ActionResult Create(Ilan ilan)
        {
            if (Session["SirketId"] == null)
            {
                TempData["ErrorMessage"] = "Lütfen giriş yapın.";
                return RedirectToAction("Index", "SirketLogin");
            }

            ilan.SirketId = Convert.ToInt32(Session["SirketId"]); // Şirket ID'yi Session'dan al
            ilan.YayınTarihi = BitConverter.GetBytes(DateTime.Now.ToBinary());

            if (ModelState.IsValid)
            {
                try
                {
                    _dbHelper.AddIlan(ilan);
                    TempData["SuccessMessage"] = "İlan başarıyla oluşturuldu.";
                    return RedirectToAction("Dashboard", "SirketHome");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "İlan oluşturulurken bir hata oluştu: " + ex.Message;
                }
            }

            // Hata durumunda tekrar dropdownları doldur
            ViewBag.Konumlar = _dbHelper.GetAllKonumlar();
            ViewBag.Pozisyonlar = _dbHelper.GetAllPozisyonlar();
            return View(ilan);
        }
    }
}
