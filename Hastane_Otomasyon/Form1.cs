using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Hastane_Otomasyon
{
    public partial class Form1 : Form
        
    {
        // Sql veritabanına ulaşmamızı sağlayan kod
        private string connectionString = "Data Source=172.16.192.60; Initial Catalog=db_hastane; Integrated Security=FALSE; User ID=mehmetcu; password=Sql123456+";


        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text; //textboxlara string değer atadık.
            string password = txtPassword.Text; 

            // SQL sorgusu ile kullanıcı girişi kontrolü
            string query = "SELECT * FROM calisanlar WHERE calisanno = @username AND calisansifre = @password";


            using (SqlConnection connection = new SqlConnection(connectionString))  // Veritabanı bağlantısı oluşturma ve açma
            {
                connection.Open(); //sql veritabanına bağlantıyı sağlayan fonksiyon.
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);  //buradaki parametre kullanımı eğer sistemdeki verilere uyuyorsa anlamında
                    command.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                       
                    {
                        this.Hide(); //yeni form açılınca ilk formu gizliyoruz

                        PatientForm frm=new PatientForm(); //yeni formu göster
                        frm.ShowDialog();

                        this.Close(); //ilk formu kapat

                       

                        
                    }
                    else
                    {
                        MessageBox.Show("Hatalı kullanıcı adı veya şifre!"); // hatalı giriş denemesinde kullanıcının karşısına çıkıcaklar
                    }

                }
            }
 
            
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Char.IsWhiteSpace(e.KeyChar);  //keypress özelliği textboxta herhangi bir tuşa basıldığında tetiklenir e.keychar özelliği ile tuşun karakter değeri alınır.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) //iscontrol ile tuşarı kontrol ederken isdigit ile sadece rakamlara izin verilir  
            {
                e.Handled = true; //farklı bir karakter girilirse e.handled true olduğu için girişi engeller.
            }
        }
    }
}

