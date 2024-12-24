using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using KariyerPortali5.Models;

namespace KariyerPortali5.Controllers
{
    public class LoginController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-HOIKTJS;Initial Catalog=KariyerPortali;Integrated Security=True";

        // GET: Login
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Dashboard", "Home"); // Zaten giriş yaptıysa Dashboard'a yönlendir
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string Email, string Sifre)
        {
            // Giriş bilgilerini doğrula
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Sifre))
            {
                ViewBag.ErrorMessage = "Email veya şifre boş bırakılamaz.";
                return View();
            }

            string query = "SELECT KullaniciId, Email, Isim, Soyisim FROM Kullanici WHERE Email = @Email AND Sifre = @Sifre";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        // Parametreleri ekleyin
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Sifre", Sifre);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Kullanıcı bilgilerini Session'da sakla
                                Session["UserId"] = reader["KullaniciId"];
                                Session["UserEmail"] = reader["Email"];
                                Session["UserName"] = reader["Isim"];
                                Session["UserSurname"] = reader["Soyisim"];

                                return RedirectToAction("Dashboard", "Home"); // Giriş başarılı
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Hatalı email veya şifre!";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Hata oluştu: " + ex.Message;
            }

            return View();
        }

        public ActionResult Logout()
        {
            // Oturum bilgilerini temizle
            Session.Clear();
            return RedirectToAction("Index", "Login"); // Login sayfasına yönlendir
        }

    }
}
