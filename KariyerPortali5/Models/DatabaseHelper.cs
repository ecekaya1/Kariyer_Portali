using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using KariyerPortali5.Models;

namespace KariyerPortali5.Models
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            // Bağlantı dizesini web.config dosyasından alıyoruz
            connectionString = ConfigurationManager.ConnectionStrings["KariyerModel1"].ConnectionString;
        }

        // 1. Tüm Kullanıcıları Getir
        public List<Kullanici> GetAllKullanicilar()
        {
            List<Kullanici> kullanicilar = new List<Kullanici>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Kullanici";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    kullanicilar.Add(new Kullanici
                    {
                        KullaniciId = Convert.ToInt32(reader["KullaniciId"]),
                        Isim = reader["Isim"].ToString(),
                        Soyisim = reader["Soyisim"].ToString(),
                        Email = reader["Email"].ToString(),
                        TelefonNo = reader["TelefonNo"].ToString(),
                        Yetenekler = reader["Yetenekler"].ToString()
                    });
                }
            }

            return kullanicilar;
        }

    


        // 4. Kullanıcı Sil
        public void DeleteKullanici(int kullaniciId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Kullanici WHERE KullaniciId = @KullaniciId";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@KullaniciId", kullaniciId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // 5. Tüm İlanları Getir
        public List<Ilan> GetAllIlanlar()
        {
            List<Ilan> ilanlar = new List<Ilan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        i.IlanId, i.Maas, i.Aciklama, i.SonBasvuruTarihi, i.Nitelikler, 
                        s.SirketAdi, 
                        p.PozisyonAdi, 
                        se.SektorAdi
                    FROM Ilan i
                    INNER JOIN Sirket s ON i.SirketId = s.SirketId
                    LEFT JOIN Pozisyon p ON i.PozisyonId = p.PozisyonId
                    LEFT JOIN Sektor se ON s.SektorId = se.SektorId";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ilanlar.Add(new Ilan
                    {
                        IlanId = Convert.ToInt32(reader["IlanId"]),
                        Maas = Convert.ToInt32(reader["Maas"]),
                        Aciklama = reader["Aciklama"].ToString(),
                        SonBasvuruTarihi = reader["SonBasvuruTarihi"] as DateTime?,
                        Nitelikler = reader["Nitelikler"].ToString(),
                        Sirket = new Sirket
                        {
                            SirketAdi = reader["SirketAdi"].ToString(),
                            Sektor = new Sektor
                            {
                                SektorAdi = reader["SektorAdi"]?.ToString()
                            }
                        },
                        Pozisyon = new Pozisyon
                        {
                            PozisyonAdi = reader["PozisyonAdi"]?.ToString()
                        }
                    });

                }
            }

            return ilanlar;
        }
        // Tekil bir ilanı Id'ye göre getir
        public Ilan GetIlanById(int id)
        {
            Ilan ilan = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT 
                i.IlanId, i.Maas, i.Aciklama, i.SonBasvuruTarihi, i.Nitelikler, 
                s.SirketAdi, 
                p.PozisyonAdi, 
                se.SektorAdi
            FROM Ilan i
            INNER JOIN Sirket s ON i.SirketId = s.SirketId
            LEFT JOIN Pozisyon p ON i.PozisyonId = p.PozisyonId
            LEFT JOIN Sektor se ON s.SektorId = se.SektorId
            WHERE i.IlanId = @IlanId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IlanId", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ilan = new Ilan
                    {
                        IlanId = Convert.ToInt32(reader["IlanId"]),
                        Maas = Convert.ToInt32(reader["Maas"]),
                        Aciklama = reader["Aciklama"].ToString(),
                        SonBasvuruTarihi = reader["SonBasvuruTarihi"] as DateTime?,
                        Nitelikler = reader["Nitelikler"].ToString(),
                        Sirket = new Sirket
                        {
                            SirketAdi = reader["SirketAdi"].ToString(),
                            Sektor = new Sektor
                            {
                                SektorAdi = reader["SektorAdi"]?.ToString()
                            }
                        },
                        Pozisyon = new Pozisyon
                        {
                            PozisyonAdi = reader["PozisyonAdi"]?.ToString()
                        }
                    };
                }
            }

            return ilan;
        }


        public void BasvuruYap(int kullaniciId, int ilanId, int? basvuruDurumuId = null)
        {
            // Eğer BasvuruDurumuId sağlanmadıysa, varsayılan değeri alıyoruz
            if (!basvuruDurumuId.HasValue)
            {
                basvuruDurumuId = GetDefaultBasvuruDurumuId();
            }

            // IlanId'ye bağlı olarak SirketId'yi alıyoruz
            int sirketId = GetSirketIdByIlanId(ilanId);

            string query = @"
    INSERT INTO Basvuru (KullaniciId, IlanId, BasvuruDurumuId, SirketId) 
    VALUES (@KullaniciId, @IlanId, @BasvuruDurumuId, @SirketId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciId", kullaniciId);
                    command.Parameters.AddWithValue("@IlanId", ilanId);
                    command.Parameters.AddWithValue("@BasvuruDurumuId", basvuruDurumuId);
                    command.Parameters.AddWithValue("@SirketId", sirketId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        private int GetSirketIdByIlanId(int ilanId)
        {
            string query = "SELECT SirketId FROM Ilan WHERE IlanId = @IlanId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IlanId", ilanId);
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            throw new Exception("IlanId'ye bağlı SirketId bulunamadı!");
        }



        // Varsayılan BasvuruDurumuId'yi almak için bir metod
        private int GetDefaultBasvuruDurumuId()
        {
            string query = "SELECT BasvuruDurumuId FROM BasvuruDurumu WHERE Durum = 'Beklemede'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            throw new Exception("Varsayılan BasvuruDurumu bulunamadı!");
        }





        public List<Ilan> GetBasvurularByKullaniciId(int kullaniciId)
        {
            List<Ilan> basvurular = new List<Ilan>();

            string query = @"
        SELECT 
            i.IlanId, 
            i.Aciklama, 
            s.SirketAdi, 
            p.PozisyonAdi 
        FROM Basvuru b
        JOIN Ilan i ON b.IlanId = i.IlanId
        JOIN Sirket s ON i.SirketId = s.SirketId
        LEFT JOIN Pozisyon p ON i.PozisyonId = p.PozisyonId
        WHERE b.KullaniciId = @KullaniciId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciId", kullaniciId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            basvurular.Add(new Ilan
                            {
                                IlanId = Convert.ToInt32(reader["IlanId"]),
                                Aciklama = reader["Aciklama"].ToString(),
                                Sirket = new Sirket
                                {
                                    SirketAdi = reader["SirketAdi"].ToString()
                                },
                                Pozisyon = new Pozisyon
                                {
                                    PozisyonAdi = reader["PozisyonAdi"].ToString()
                                }
                            });
                        }
                    }
                }
            }

            return basvurular;
        }




        // Kullanıcı ID'sine göre kullanıcıyı getir
        public int GetKullaniciIdByEmail(string email)
        {
            string query = "SELECT KullaniciId FROM Kullanici WHERE Email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            return 0; // Kullanıcı bulunamazsa 0 döner
        }

        public List<Ilan> GetAllIlanlar(string sektor = null, string sehir = null, string pozisyon = null, string maasSiralama = null)
        {
            List<Ilan> ilanlar = new List<Ilan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT 
            i.IlanId, 
            i.Maas, 
            i.Aciklama, 
            i.SonBasvuruTarihi, 
            i.Nitelikler, 
            i.YayınTarihi,
            s.SirketAdi, 
            p.PozisyonAdi, 
            se.SektorAdi,
            k.Ulke,
            k.Sehir
        FROM Ilan i
        INNER JOIN Sirket s ON i.SirketId = s.SirketId
        LEFT JOIN Pozisyon p ON i.PozisyonId = p.PozisyonId
        LEFT JOIN Sektor se ON s.SektorId = se.SektorId
        LEFT JOIN Konum k ON i.KonumId = k.KonumId
        WHERE 1=1";

                // Filtreleme parametreleri
                if (!string.IsNullOrEmpty(sektor))
                {
                    query += " AND se.SektorAdi = @Sektor";
                }
                if (!string.IsNullOrEmpty(sehir))
                {
                    query += " AND k.Sehir = @Sehir";
                }
                if (!string.IsNullOrEmpty(pozisyon))
                {
                    query += " AND p.PozisyonAdi LIKE @Pozisyon";
                }

                // Maaş sıralaması
                if (maasSiralama == "desc")
                {
                    query += " ORDER BY i.Maas DESC"; // En yüksekten aşağıya
                }
                else if (maasSiralama == "asc")
                {
                    query += " ORDER BY i.Maas ASC"; // Aşağıdan yukarıya
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(sektor))
                    {
                        cmd.Parameters.AddWithValue("@Sektor", sektor);
                    }
                    if (!string.IsNullOrEmpty(sehir))
                    {
                        cmd.Parameters.AddWithValue("@Sehir", sehir);
                    }
                    if (!string.IsNullOrEmpty(pozisyon))
                    {
                        cmd.Parameters.AddWithValue("@Pozisyon", "%" + pozisyon + "%");
                    }

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ilanlar.Add(new Ilan
                            {
                                IlanId = Convert.ToInt32(reader["IlanId"]),
                                Maas = Convert.ToInt32(reader["Maas"]),
                                Aciklama = reader["Aciklama"].ToString(),
                                SonBasvuruTarihi = reader["SonBasvuruTarihi"] as DateTime?,
                                Nitelikler = reader["Nitelikler"].ToString(),
                                YayınTarihi = reader["YayınTarihi"] as byte[],
                                Sirket = new Sirket
                                {
                                    SirketAdi = reader["SirketAdi"].ToString(),
                                    Sektor = new Sektor
                                    {
                                        SektorAdi = reader["SektorAdi"]?.ToString()
                                    }
                                },
                                Pozisyon = new Pozisyon
                                {
                                    PozisyonAdi = reader["PozisyonAdi"]?.ToString()
                                },
                                Konum = new Konum
                                {
                                    Ulke = reader["Ulke"]?.ToString(),
                                    Sehir = reader["Sehir"]?.ToString()
                                }
                            });
                        }
                    }
                }
            }

            return ilanlar;
        }

        public bool HasUserAppliedForJob(int kullaniciId, int ilanId)
        {
            string query = "SELECT COUNT(*) FROM Basvuru WHERE KullaniciId = @KullaniciId AND IlanId = @IlanId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KullaniciId", kullaniciId);
                    command.Parameters.AddWithValue("@IlanId", ilanId);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    return count > 0; // Eğer başvuru varsa true döner
                }
            }
        }

        public void AddKullanici(Kullanici kullanici)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Kullanici (Isim, Soyisim, Email, TelefonNo, DogumTarihi, Yetenekler, Sifre) VALUES (@Isim, @Soyisim, @Email, @TelefonNo, @DogumTarihi, @Yetenekler, @Sifre)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Isim", kullanici.Isim);
                cmd.Parameters.AddWithValue("@Soyisim", kullanici.Soyisim);
                cmd.Parameters.AddWithValue("@Email", kullanici.Email);
                cmd.Parameters.AddWithValue("@TelefonNo", kullanici.TelefonNo);
                cmd.Parameters.AddWithValue("@DogumTarihi", kullanici.DogumTarihi);
                cmd.Parameters.AddWithValue("@Yetenekler", string.IsNullOrEmpty(kullanici.Yetenekler) ? (object)DBNull.Value : kullanici.Yetenekler);
                cmd.Parameters.AddWithValue("@Sifre", kullanici.Sifre);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateKullanici(Kullanici kullanici)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            UPDATE Kullanici 
            SET 
                Isim = @Isim, 
                Soyisim = @Soyisim, 
                Email = @Email, 
                TelefonNo = @TelefonNo, 
                DogumTarihi = @DogumTarihi, 
                Yetenekler = @Yetenekler, 
                Sifre = @Sifre 
            WHERE KullaniciId = @KullaniciId";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Isim", kullanici.Isim);
                cmd.Parameters.AddWithValue("@Soyisim", kullanici.Soyisim);
                cmd.Parameters.AddWithValue("@Email", kullanici.Email);
                cmd.Parameters.AddWithValue("@TelefonNo", kullanici.TelefonNo);
                cmd.Parameters.AddWithValue("@DogumTarihi", kullanici.DogumTarihi);
                cmd.Parameters.AddWithValue("@Yetenekler", string.IsNullOrEmpty(kullanici.Yetenekler) ? (object)DBNull.Value : kullanici.Yetenekler);
                cmd.Parameters.AddWithValue("@Sifre", kullanici.Sifre);
                cmd.Parameters.AddWithValue("@KullaniciId", kullanici.KullaniciId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<Sirket> GetAllSirketler(string sektor = null, string sehir = null, string sirketAdi = null)
        {
            List<Sirket> sirketler = new List<Sirket>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT 
            s.SirketId, 
            s.SirketAdi, 
            se.SektorAdi, 
            k.Ulke, 
            k.Sehir, 
            s.WebSite
        FROM Sirket s
        LEFT JOIN Sektor se ON s.SektorId = se.SektorId
        LEFT JOIN Konum k ON s.KonumId = k.KonumId
        WHERE 1=1";

                // Dinamik filtreler
                if (!string.IsNullOrEmpty(sektor))
                {
                    query += " AND se.SektorAdi = @Sektor";
                }
                if (!string.IsNullOrEmpty(sehir))
                {
                    query += " AND k.Sehir = @Sehir";
                }
                if (!string.IsNullOrEmpty(sirketAdi))
                {
                    query += " AND s.SirketAdi LIKE @SirketAdi";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(sektor))
                    {
                        cmd.Parameters.AddWithValue("@Sektor", sektor);
                    }
                    if (!string.IsNullOrEmpty(sehir))
                    {
                        cmd.Parameters.AddWithValue("@Sehir", sehir);
                    }
                    if (!string.IsNullOrEmpty(sirketAdi))
                    {
                        cmd.Parameters.AddWithValue("@SirketAdi", "%" + sirketAdi + "%");
                    }

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        sirketler.Add(new Sirket
                        {
                            SirketId = Convert.ToInt32(reader["SirketId"]),
                            SirketAdi = reader["SirketAdi"].ToString(),
                            Sektor = new Sektor { SektorAdi = reader["SektorAdi"]?.ToString() },
                            Konum = new Konum
                            {
                                Ulke = reader["Ulke"]?.ToString(),
                                Sehir = reader["Sehir"]?.ToString()
                            },
                            WebSite = reader["WebSite"]?.ToString()
                        });
                    }
                }
            }
            return sirketler;
        }

        public List<string> GetAllSektorler()
        {
            List<string> sektorler = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT SektorAdi FROM Sektor";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sektorler.Add(reader["SektorAdi"].ToString());
                }
            }
            return sektorler;
        }

        public List<string> GetAllSehirler()
        {
            List<string> sehirler = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT Sehir FROM Konum";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sehirler.Add(reader["Sehir"].ToString());
                }
            }
            return sehirler;
        }
        public Sirket GetSirketById(int sirketId)
        {
            Sirket sirket = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT s.SirketId, s.SirketAdi, se.SektorAdi, k.Ulke, k.Sehir, s.WebSite, s.Email, s.Sifre
            FROM Sirket s
            LEFT JOIN Sektor se ON s.SektorId = se.SektorId
            LEFT JOIN Konum k ON s.KonumId = k.KonumId
            WHERE s.SirketId = @SirketId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SirketId", sirketId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    sirket = new Sirket
                    {
                        SirketId = Convert.ToInt32(reader["SirketId"]),
                        SirketAdi = reader["SirketAdi"].ToString(),
                        WebSite = reader["WebSite"]?.ToString(),
                        Email = reader["Email"].ToString(),
                        Sifre = reader["Sifre"].ToString(),
                        Sektor = new Sektor { SektorAdi = reader["SektorAdi"]?.ToString() },
                        Konum = new Konum
                        {
                            Ulke = reader["Ulke"]?.ToString(),
                            Sehir = reader["Sehir"]?.ToString()
                        }
                    };
                }
            }
            return sirket;
        }

        public void UpdateSirket(Sirket sirket)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            UPDATE Sirket 
            SET SirketAdi = @SirketAdi, WebSite = @WebSite, Email = @Email, 
                Sifre = @Sifre, SektorId = (SELECT SektorId FROM Sektor WHERE SektorAdi = @SektorAdi)
            WHERE SirketId = @SirketId";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@SirketAdi", sirket.SirketAdi);
                cmd.Parameters.AddWithValue("@WebSite", sirket.WebSite ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", sirket.Email);
                cmd.Parameters.AddWithValue("@Sifre", sirket.Sifre);
                cmd.Parameters.AddWithValue("@SektorAdi", sirket.Sektor?.SektorAdi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SirketId", sirket.SirketId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Sirket GetSirketByEmailAndPassword(string email, string sifre)
        {
            Sirket sirket = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Sirket WHERE Email = @Email AND Sifre = @Sifre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Sifre", sifre);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    sirket = new Sirket
                    {
                        SirketId = Convert.ToInt32(reader["SirketId"]),
                        SirketAdi = reader["SirketAdi"].ToString(),
                        Email = reader["Email"].ToString(),
                        Sifre = reader["Sifre"].ToString()
                    };
                }
            }

            return sirket;
        }
        public Sirket GetSirket2ById(int sirketId)
        {
            Sirket sirket = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Sirket WHERE SirketId = @SirketId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SirketId", sirketId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    sirket = new Sirket
                    {
                        SirketId = Convert.ToInt32(reader["SirketId"]),
                        SirketAdi = reader["SirketAdi"].ToString(),
                        WebSite = reader["WebSite"]?.ToString(),
                        Email = reader["Email"].ToString(),
                        Sifre = reader["Sifre"].ToString()
                    };
                }
            }

            return sirket;
        }
        public void AddIlan(Ilan ilan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            INSERT INTO Ilan (SirketId, PozisyonId, Maas, KonumId, Aciklama, SonBasvuruTarihi, Nitelikler) 
            VALUES (@SirketId, @PozisyonId, @Maas, @KonumId, @Aciklama, @SonBasvuruTarihi, @Nitelikler)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SirketId", ilan.SirketId);
                cmd.Parameters.AddWithValue("@PozisyonId", ilan.PozisyonId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Maas", ilan.Maas);
                cmd.Parameters.AddWithValue("@KonumId", ilan.KonumId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Aciklama", ilan.Aciklama ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SonBasvuruTarihi", ilan.SonBasvuruTarihi ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Nitelikler", ilan.Nitelikler ?? (object)DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public List<Konum> GetAllKonumlar()
        {
            List<Konum> konumlar = new List<Konum>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT KonumId, Sehir FROM Konum";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    konumlar.Add(new Konum
                    {
                        KonumId = Convert.ToInt32(reader["KonumId"]),
                        Sehir = reader["Sehir"].ToString()
                    });
                }
            }
            return konumlar;
        }

        public List<Pozisyon> GetAllPozisyonlar()
        {
            List<Pozisyon> pozisyonlar = new List<Pozisyon>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT PozisyonId, PozisyonAdi FROM Pozisyon";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pozisyonlar.Add(new Pozisyon
                    {
                        PozisyonId = Convert.ToInt32(reader["PozisyonId"]),
                        PozisyonAdi = reader["PozisyonAdi"].ToString()
                    });
                }
            }
            return pozisyonlar;
        }
        /* public List<Ilan> GetIlanlarBySirketId(int sirketId)
         {
             List<Ilan> ilanlar = new List<Ilan>();

             using (SqlConnection conn = new SqlConnection(connectionString))
             {
                 string query = @"
         SELECT 
             i.IlanId, 
             i.Maas, 
             i.Aciklama, 
             i.SonBasvuruTarihi, 
             i.Nitelikler, 
             p.PozisyonAdi, 
             k.Sehir 
         FROM Ilan i
         LEFT JOIN Pozisyon p ON i.PozisyonId = p.PozisyonId
         LEFT JOIN Konum k ON i.KonumId = k.KonumId
         WHERE i.SirketId = @SirketId";

                 SqlCommand cmd = new SqlCommand(query, conn);
                 cmd.Parameters.AddWithValue("@SirketId", sirketId);

                 conn.Open();

                 SqlDataReader reader = cmd.ExecuteReader();
                 while (reader.Read())
                 {
                     ilanlar.Add(new Ilan
                     {
                         IlanId = Convert.ToInt32(reader["IlanId"]),
                         Maas = Convert.ToInt32(reader["Maas"]),
                         Aciklama = reader["Aciklama"].ToString(),
                         SonBasvuruTarihi = reader["SonBasvuruTarihi"] as DateTime?,
                         Nitelikler = reader["Nitelikler"].ToString(),
                         Pozisyon = new Pozisyon
                         {
                             PozisyonAdi = reader["PozisyonAdi"]?.ToString()
                         },
                         Konum = new Konum
                         {
                             Sehir = reader["Sehir"]?.ToString()
                         }
                     });
                 }
             }

             return ilanlar;
         } */


        public List<Ilan> GetIlanlarFromView()
        {
            List<Ilan> ilanlar = new List<Ilan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM vw_IlanWithDetails";

                SqlCommand cmd = new SqlCommand(query, conn);   
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ilanlar.Add(new Ilan
                    {
                        IlanId = Convert.ToInt32(reader["IlanId"]),
                        SirketId = Convert.ToInt32(reader["SirketId"]),
                        PozisyonId = reader["PozisyonId"] as int?,
                        Maas = Convert.ToInt32(reader["Maas"]),
                        KonumId = reader["KonumId"] as int?,
                        Aciklama = reader["Aciklama"]?.ToString(),
                        YayınTarihi = reader["YayınTarihi"] as byte[],
                        SonBasvuruTarihi = reader["SonBasvuruTarihi"] as DateTime?,
                        Nitelikler = reader["Nitelikler"]?.ToString(),
                        Sirket = new Sirket
                        {
                            SirketAdi = reader["SirketAdi"]?.ToString(),
                            Sektor = new Sektor
                            {
                                SektorAdi = reader["SektorAdi"]?.ToString()
                            }
                        },
                        Pozisyon = new Pozisyon
                        {
                            PozisyonAdi = reader["PozisyonAdi"]?.ToString()
                        },
                        Konum = new Konum
                        {
                            Ulke = reader["Ulke"]?.ToString(),
                            Sehir = reader["Sehir"]?.ToString()
                        }
                    });
                }
            }

            return ilanlar;
        }




        /*public List<Basvuru> GetBasvurularBySirketId(int sirketId)
        {
            List<Basvuru> basvurular = new List<Basvuru>();

            string query = @"
    SELECT 
        b.BasvuruId, 
        b.KullaniciId, 
        b.BasvuruTarihi, 
        d.Durum AS BasvuruDurumu,
        k.Isim AS KullaniciIsim,
        k.Soyisim AS KullaniciSoyisim,
        k.Email AS KullaniciEmail
    FROM Basvuru b
    LEFT JOIN Kullanici k ON b.KullaniciId = k.KullaniciId
    LEFT JOIN BasvuruDurumu d ON b.BasvuruDurumuId = d.BasvuruDurumuId
    WHERE b.SirketId = @SirketId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SirketId", sirketId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    basvurular.Add(new Basvuru
                    {
                        BasvuruId = Convert.ToInt32(reader["BasvuruId"]),
                        KullaniciId = Convert.ToInt32(reader["KullaniciId"]),
                        BasvuruDurumu = new BasvuruDurumu
                        {
                            Durum = reader["BasvuruDurumu"]?.ToString()
                        },
                        Kullanici = new Kullanici
                        {
                            Isim = reader["KullaniciIsim"]?.ToString(),
                            Soyisim = reader["KullaniciSoyisim"]?.ToString(),
                            Email = reader["KullaniciEmail"]?.ToString()
                        }
                    });
                }
            }

            return basvurular;
        } */
        public List<Basvuru> GetBasvurularBySirketId(int sirketId)
        {
            List<Basvuru> basvurular = new List<Basvuru>();

            string procedureName = "BasvurularBySirketId"; // Prosedür adı

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(procedureName, conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure; // Prosedür çağrısı olduğunu belirtir
                cmd.Parameters.AddWithValue("@SirketId", sirketId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    basvurular.Add(new Basvuru
                    {
                        BasvuruId = Convert.ToInt32(reader["BasvuruId"]),
                        KullaniciId = Convert.ToInt32(reader["KullaniciId"]),
                        BasvuruDurumu = new BasvuruDurumu
                        {
                            Durum = reader["BasvuruDurumu"]?.ToString()
                        },
                        Kullanici = new Kullanici
                        {
                            Isim = reader["KullaniciIsim"]?.ToString(),
                            Soyisim = reader["KullaniciSoyisim"]?.ToString(),
                            Email = reader["KullaniciEmail"]?.ToString()
                        }
                    });
                }
            }

            return basvurular;
        }


        public void AddSirket(Sirket sirket)
        {
            string query = @"
        INSERT INTO Sirket (SirketAdi, Email, Sifre, WebSite, KonumId, SektorId)
        VALUES (@SirketAdi, @Email, @Sifre, @WebSite, @KonumId, @SektorId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SirketAdi", sirket.SirketAdi ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", sirket.Email);
                    command.Parameters.AddWithValue("@Sifre", sirket.Sifre);
                    command.Parameters.AddWithValue("@WebSite", sirket.WebSite ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@KonumId", sirket.KonumId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SektorId", sirket.SektorId ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteIlan(int ilanId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Basvuru tablosundaki ilişkili kayıtları sil
                string deleteBasvuruQuery = "DELETE FROM Basvuru WHERE IlanId = @IlanId";
                SqlCommand deleteBasvuruCmd = new SqlCommand(deleteBasvuruQuery, conn);
                deleteBasvuruCmd.Parameters.AddWithValue("@IlanId", ilanId);
                deleteBasvuruCmd.ExecuteNonQuery();

                // Şimdi ilanı sil
                string deleteIlanQuery = "DELETE FROM Ilan WHERE IlanId = @IlanId";
                SqlCommand deleteIlanCmd = new SqlCommand(deleteIlanQuery, conn);
                deleteIlanCmd.Parameters.AddWithValue("@IlanId", ilanId);
                deleteIlanCmd.ExecuteNonQuery();
            }
        }

        public List<Ilan> GetIlanlarWithHighSalary()
        {
            List<Ilan> ilanlar = new List<Ilan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT 
            i.IlanId, 
            MAX(i.Maas) AS Maas, 
            p.PozisyonAdi, 
            s.SirketAdi, 
            k.Sehir
        FROM Ilan i
        INNER JOIN Sirket s ON i.SirketId = s.SirketId
        LEFT JOIN Pozisyon p ON i.PozisyonId = p.PozisyonId
        LEFT JOIN Konum k ON i.KonumId = k.KonumId
        GROUP BY i.IlanId, p.PozisyonAdi, s.SirketAdi, k.Sehir
        HAVING MAX(i.Maas) > 100000";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ilanlar.Add(new Ilan
                    {
                        IlanId = Convert.ToInt32(reader["IlanId"]),
                        Maas = Convert.ToInt32(reader["Maas"]),
                        Sirket = new Sirket
                        {
                            SirketAdi = reader["SirketAdi"].ToString()
                        },
                        Pozisyon = new Pozisyon
                        {
                            PozisyonAdi = reader["PozisyonAdi"].ToString()
                        },
                        Konum = new Konum
                        {
                            Sehir = reader["Sehir"]?.ToString()
                        }
                    });
                }
            }

            return ilanlar;
        }


        public void UpdateIlan(Ilan ilan)
        {
            string query = @"
        UPDATE Ilan 
        SET 
            PozisyonId = @PozisyonId, 
            KonumId = @KonumId, 
            Maas = @Maas, 
            SonBasvuruTarihi = @SonBasvuruTarihi, 
            Aciklama = @Aciklama
        WHERE IlanId = @IlanId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PozisyonId", ilan.PozisyonId);
                    cmd.Parameters.AddWithValue("@KonumId", ilan.KonumId);
                    cmd.Parameters.AddWithValue("@Maas", ilan.Maas);
                    cmd.Parameters.AddWithValue("@SonBasvuruTarihi", (object)ilan.SonBasvuruTarihi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Aciklama", ilan.Aciklama);
                    cmd.Parameters.AddWithValue("@IlanId", ilan.IlanId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }
}
