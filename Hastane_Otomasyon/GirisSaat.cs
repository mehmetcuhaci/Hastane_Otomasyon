using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hastane_Otomasyon
{
    public partial class GirisSaat : Form
    {
        public GirisSaat()
        {
            InitializeComponent();
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            this.Hide();

            PatientForm frm = new PatientForm(); //yeni formu göster
            frm.ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button button1 = (Button)sender;
            string gunAdi = button1.Text;

            DateTime basTarih = new DateTime(2023, 09, 04, 23, 59, 00);
            DateTime bitisTarih = new DateTime(2023, 09, 10, 23, 59, 00);

            TimeSpan aralik = TimeSpan.FromHours(2); // 2 saatlik aralık 

            string connectionString= "Data Source=172.16.192.60; Initial Catalog=db_hastane; Integrated Security=FALSE; User ID=mehmetcu; password=Sql123456+";

            int toplamGirdi = 0; // toplam girdiyi hesaplamak için 

            using (SqlConnection connection= new SqlConnection(connectionString))
            {
                connection.Open();

                while (basTarih <= bitisTarih)
                {

                    DateTime baslangicSaat = basTarih; // başlangıç saatimiz ilk tarihe göre
                    DateTime bitisSaat = basTarih.Add(aralik); // bitiş saatimiz eklenen aralığa göre


                    string sql = "SELECT COUNT(*) FROM hastalar WHERE hastagiris BETWEEN @baslangicSaat AND @bitisSaat";

                    SqlCommand command =new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@baslangicSaat", baslangicSaat); 
                    command.Parameters.AddWithValue("@bitisSaat", bitisSaat);

                    

                    int girisSayisi = (int)command.ExecuteScalar();

                    int saatGirdi = girisSayisi;
                    label18.Text = saatGirdi.ToString();

                    toplamGirdi += girisSayisi;

                    basTarih = basTarih.AddHours(2);  // en son 2 saat daha ekleyip başa dönüyor






                }


            }

            lblToplam.Text =toplamGirdi.ToString();

        }
            


    }
 }

