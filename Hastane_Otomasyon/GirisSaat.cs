using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hastane_Otomasyon
{
    public partial class GirisSaat : Form
    {
        private string connectionString = "Data Source=172.16.192.60; Initial Catalog=db_hastane; Integrated Security=FALSE; User ID=mehmetcu; password=Sql123456+";
        private List<Label> saatLabelListesi = new List<Label>(); //labelları listeyebilmek için liste fonksiyonu oluştur

        public GirisSaat()
        {
            InitializeComponent();
            saatLabelListesi.Add(label1);
            saatLabelListesi.Add(label2);
            saatLabelListesi.Add(label3);
            saatLabelListesi.Add(label18);
            saatLabelListesi.Add(label19);
            saatLabelListesi.Add(label20);
            saatLabelListesi.Add(label21);
            saatLabelListesi.Add(label22);
            saatLabelListesi.Add(label23);
            saatLabelListesi.Add(label24);
            saatLabelListesi.Add(label25);
            saatLabelListesi.Add(label26);
            
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            this.Hide();
            PatientForm frm = new PatientForm();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // seçilen günün başlangıç ve bitiş tarihlerini ayarla (startDate ve endDate)
            DateTime startDate = new DateTime(2023, 09, 04, 00, 00, 00); // Pazartesi gününün başlangıcı
            DateTime endDate = new DateTime(2023, 09, 04, 23, 59, 59);   // Pazartesi gününün sonu

            // seçilen günün toplam hasta sayısını hesapla ve lblToplam'a yazdır
            int toplamHasta = HastaSayisiniHesapla(startDate, endDate);
            lblToplam.Text = $"{toplamHasta}";

            // 00-02 saat aralığında giriş yapan hastaları hesapla ve sırasıyla labellara yazdır
            for (int saat = 0; saat < 24; saat += 2)
            {
                DateTime startTime = startDate.AddHours(saat); // saatin başlangıcı 
                DateTime endTime = startDate.AddHours(saat + 2); // ve saatin sonu için 2 arttırdık

                int saatArasiHasta = HastaSayisiniHesapla(startTime, endTime);

                if (saat / 2 < saatLabelListesi.Count) // Saati 2'ye böldük çünkü saat her 2 saatte bir artıyor.
                {
                    Label lblSaat = saatLabelListesi[saat / 2];
                    lblSaat.Text = $"{saatArasiHasta}";
                }
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            // Salı gününün başlangıç ve bitiş tarihlerini ayarla (startDate ve endDate)
            DateTime startDate = new DateTime(2023, 09, 05, 00, 00, 00); // Salı gününün başlangıcı
            DateTime endDate = new DateTime(2023, 09, 05, 23, 59, 59);   // Salı gününün sonu

            int toplamHasta = HastaSayisiniHesapla(startDate, endDate);
            lblToplam.Text = $"{toplamHasta}";

            // 00-02 saat aralığında giriş yapan hastaları hesapla ve label18'den başlayarak ilgili label'lara yazdır
            for (int saat = 0; saat < 24; saat += 2)
            {
                DateTime startTime = startDate.AddHours(saat);
                DateTime endTime = startDate.AddHours(saat + 2);

                int saatArasiHasta = HastaSayisiniHesapla(startTime, endTime);

                if (saat / 2 < saatLabelListesi.Count) // Saati 2'ye böldük çünkü saat her 2 saatte bir artıyor.
                {
                    Label lblSaat = saatLabelListesi[saat / 2];
                    lblSaat.Text = $"{saatArasiHasta}";
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DateTime startDate = new DateTime(2023, 09, 06, 00, 00, 00);
            DateTime endDate = new DateTime(2023, 09, 06, 23, 59, 59);

            int toplamHasta = HastaSayisiniHesapla(startDate, endDate);
            lblToplam.Text = $"{toplamHasta}";

            // 00-02 saat aralığında giriş yapan hastaları hesapla ve label18'den başlayarak ilgili label'lara yazdır
            for (int saat = 0; saat < 24; saat += 2)
            {
                DateTime startTime = startDate.AddHours(saat);
                DateTime endTime = startDate.AddHours(saat + 2);

                int saatArasiHasta = HastaSayisiniHesapla(startTime, endTime);

                if (saat / 2 < saatLabelListesi.Count) // Saati 2'ye böldük çünkü saat her 2 saatte bir artıyor.
                {
                    Label lblSaat = saatLabelListesi[saat / 2];
                    lblSaat.Text = $"{saatArasiHasta}";
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DateTime startDate = new DateTime(2023, 09, 07, 00, 00, 00);
            DateTime endDate = new DateTime(2023, 09, 07, 23, 59, 59);

            int toplamHasta = HastaSayisiniHesapla(startDate, endDate);
            lblToplam.Text = $"{toplamHasta}";

            // 00-02 saat aralığında giriş yapan hastaları hesapla ve label18'den başlayarak ilgili label'lara yazdır
            for (int saat = 0; saat < 24; saat += 2)
            {
                DateTime startTime = startDate.AddHours(saat);
                DateTime endTime = startDate.AddHours(saat + 2);

                int saatArasiHasta = HastaSayisiniHesapla(startTime, endTime);

                if (saat / 2 < saatLabelListesi.Count) // Saati 2'ye böldük çünkü saat her 2 saatte bir artıyor.
                {
                    Label lblSaat = saatLabelListesi[saat / 2];
                    lblSaat.Text = $"{saatArasiHasta}";
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime startDate = new DateTime(2023, 09, 08, 00, 00, 00);
            DateTime endDate = new DateTime(2023, 09, 08, 23, 59, 59);

            int toplamHasta = HastaSayisiniHesapla(startDate, endDate);
            lblToplam.Text = $"{toplamHasta}";

            // 00-02 saat aralığında giriş yapan hastaları hesapla ve label18'den başlayarak ilgili label'lara yazdır
            for (int saat = 0; saat < 24; saat += 2)
            {
                DateTime startTime = startDate.AddHours(saat);
                DateTime endTime = startDate.AddHours(saat + 2);

                int saatArasiHasta = HastaSayisiniHesapla(startTime, endTime);

                if (saat / 2 < saatLabelListesi.Count) // Saati 2'ye böldük çünkü saat her 2 saatte bir artıyor.
                {
                    Label lblSaat = saatLabelListesi[saat / 2];
                    lblSaat.Text = $"{saatArasiHasta}";
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime startDate = new DateTime(2023, 09, 09, 00, 00, 00);
            DateTime endDate = new DateTime(2023, 09, 09, 23, 59, 59);

            int toplamHasta = HastaSayisiniHesapla(startDate, endDate);
            lblToplam.Text = $"{toplamHasta}";

            // 00-02 saat aralığında giriş yapan hastaları hesapla ve label18'den başlayarak ilgili label'lara yazdır
            for (int saat = 0; saat < 24; saat += 2)
            {
                DateTime startTime = startDate.AddHours(saat);
                DateTime endTime = startDate.AddHours(saat + 2);

                int saatArasiHasta = HastaSayisiniHesapla(startTime, endTime);

                if (saat / 2 < saatLabelListesi.Count) // Saati 2'ye böldük çünkü saat her 2 saatte bir artıyor.
                {
                    Label lblSaat = saatLabelListesi[saat / 2];
                    lblSaat.Text = $"{saatArasiHasta}";
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime startDate = new DateTime(2023, 09, 10, 00, 00, 00);
            DateTime endDate = new DateTime(2023, 09, 10, 23, 59, 59);

            int toplamHasta = HastaSayisiniHesapla(startDate, endDate);
            lblToplam.Text = $"{toplamHasta}";

            // 00-02 saat aralığında giriş yapan hastaları hesapla ve label18'den başlayarak ilgili label'lara yazdır
            for (int saat = 0; saat < 24; saat += 2)
            {
                DateTime startTime = startDate.AddHours(saat);
                DateTime endTime = startDate.AddHours(saat + 2);

                int saatArasiHasta = HastaSayisiniHesapla(startTime, endTime);

                if (saat / 2 < saatLabelListesi.Count) // Saati 2'ye böldük çünkü saat her 2 saatte bir artıyor.
                {
                    Label lblSaat = saatLabelListesi[saat / 2];
                    lblSaat.Text = $"{saatArasiHasta}";
                }
            }

        }

        

        private int HastaSayisiniHesapla(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Belirli bir günün toplam hasta sayısını hesaplamak için sorgu
                string sqlQuery = "SELECT COUNT(*) FROM hastalar WHERE hastagiris >= @startDate AND hastagiris <= @endDate";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    int hastaSayisi = (int)command.ExecuteScalar();
                    return hastaSayisi;
                }
            }


        }
    }
}
