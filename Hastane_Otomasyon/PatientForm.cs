using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Hastane_Otomasyon
{
    public partial class PatientForm : Form
    {
        private string connectionString = "Data Source=172.16.192.60; Initial Catalog=db_hastane; Integrated Security=FALSE; User ID=mehmetcu; password=Sql123456+";


        public PatientForm()
        {
            InitializeComponent();
        }       



        private void LoadPatientData()
        {
            string patientQuery = "SELECT * FROM hastalar";  //Hastaları sıralayacak sorgu

            using (SqlConnection connection = new SqlConnection(connectionString))   // Veritabanı bağlantısı oluşturma ve açma
            {
                SqlDataAdapter adapter = new SqlDataAdapter(patientQuery, connection);
                DataTable patientTable = new DataTable();
                adapter.Fill(patientTable);
                dataGridView1.DataSource = patientTable;  //datagride hastaların verilerini sıralamak için
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            LoadPatientData();   //butona tıklandığında hasta datasını yüklüyor.
        }

        private void ImageP() //kodda tekrarı önlemek amacıyla birden fazla kullandığım kodları fonksiyon olarak çağırdım
        {
            string hastatc = txtHastaTc.Text;
            // Hastanın resminin kaydedildiği yolu oluşturma
            string imagePath = Path.Combine(@"C:\Users\mehmetcu\Desktop\patientimage", $"{hastatc}.jpg");

            // Eğer resim dosyası mevcutsa
            if (File.Exists(imagePath))
            {
                // Resmi PictureBox2'ye yükleme ve boyutunu sıkıştırma
                pictureBox2.Image = Image.FromFile(imagePath);
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                // Eğer resim dosyası bulunmazsa, PictureBox2'yi temizleme
                pictureBox2.Image = null;
            }

        }
        private void PatientNotFound()  //kodda tekrarı önlemek amacıyla birden fazla kullandığım kodları fonksiyon olarak çağırdım
        {
            // Eğer hasta bulunmazsa, mesaj gösterme ve bileşenleri temizleme
            MessageBox.Show("Belirtilen hasta bulunamadı.");
            dataGridView1.DataSource = null;
            pictureBox2.Image = null;
        }



        private void btnAra_Click(object sender, EventArgs e)
        {
            // Aranacak hasta TC kimliği
            string hastatc = txtHastaTc.Text;
            string hastaisim = txtHastaIsim.Text;

            // Veritabanından hastayı aramak için SQL sorgusu oluşturma
            string searchQuery1 = $"SELECT * FROM hastalar WHERE hastaisim='{hastaisim}'";
            string searchQuery2 = $"SELECT * FROM hastalar WHERE hastatc='{hastatc}'";

            // Veritabanı bağlantısı oluşturma ve açma
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verileri almak için SqlDataAdapter ve DataTable kullanma

                if (checkBox1.Checked && checkBox2.Checked)
                {
                    MessageBox.Show("Lütfen arama  sadece tiplerinden birini seçiniz!");
                    return;
                }

                if (checkBox1.Checked)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(searchQuery1, connection);
                    DataTable searchTable = new DataTable();
                    adapter.Fill(searchTable);

                    // Eğer hastayı bulursak
                    if (searchTable.Rows.Count > 0)
                    {
                        // DataGridView'i aranan hastanın verileriyle doldurma
                        dataGridView1.DataSource = searchTable;
                        ImageP();
                        
                    }
                    else
                    {
                        PatientNotFound();
                    }

                }
                if (checkBox2.Checked)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(searchQuery2, connection);
                    DataTable searchTable = new DataTable();
                    adapter.Fill(searchTable);

                    // Eğer hastayı bulursak
                    if (searchTable.Rows.Count > 0)
                    {
                        // DataGridView'i aranan hastanın verileriyle doldurma
                        dataGridView1.DataSource = searchTable;
                        ImageP();

                    }
                    else
                    {
                        PatientNotFound();
                    }
                }
                if(!checkBox1.Checked && !checkBox2.Checked)
                {
                    MessageBox.Show("Lütfen arama tipini seçiniz!");
                    return;
                }
                


                // Veritabanı bağlantısını kapatma
                connection.Close();
            }

        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string hastaisim = txtHastaeis.Text;
            string hastayasStr = txtHastaYas.Text;
            string hastatc = txtHastaetc.Text;
            string hastatel = txtHastaetel.Text;
            string hastaadres = richTextBox1.Text;

            int minLength = 11, minLength2 = 10;


            if (string.IsNullOrWhiteSpace(hastatc) || string.IsNullOrWhiteSpace(hastaisim) || string.IsNullOrWhiteSpace(hastayasStr) ||
                string.IsNullOrWhiteSpace(hastatel) || string.IsNullOrWhiteSpace(hastaadres))

            {
                MessageBox.Show("Lütfen hasta verilerini eksiksiz giriniz!");
                return;
            }
            if (hastatc.Length < minLength)
            {
                MessageBox.Show("T.C. Kimlik No'yu eksiksiz giriniz!");
                return;
            }
            if (hastatel.Length < minLength2)
            {
                MessageBox.Show("Telefon numarası 10 haneli olmalıdır!");
                return;
            }

            string hastacins = " "; //hasta cinsiyetine boş string değer atayıp aşağıda hangi butonun tıklandığına göre yerini doldurabiliyor
            if (rdbtnErk.Checked) //Erkek seçildiyse
            {
                hastacins = "ERKEK";
            }
            if (rdbtnKad.Checked)
            {
                hastacins = "KADIN";
            }
            if(!rdbtnErk.Checked && !rdbtnKad.Checked)  //eğer hiçbiri seçilmezse olucaklar için
            {
                MessageBox.Show("Lütfen cinsiyet seçiniz!");
                return;
            }

            // TC kimlik numarasına sahip hastanın varlığını kontrol etmek için sorgu
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM hastalar WHERE hastatc = @hastatc";

                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@hastatc", hastatc);

                    int existingPatientCount = (int)checkCommand.ExecuteScalar();

                    if (existingPatientCount > 0)
                    {
                        MessageBox.Show("Bu TC Kimlik Numarasına sahip bir hasta zaten mevcut.");
                        return;
                    }
                }
            }



            if (DateTime.TryParse(hastayasStr, out DateTime hastayas))  //kullanıcının girdiği tarihi doğru bir şekilde ayrıştırma işlemi
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                     //Veritabanına ekleme sorgusu
                    string insertQuery = "INSERT INTO hastalar (hastaisim, hastatc, hastatel, hastayas, hastacinsiyet,hastaadres) " +
                                         "VALUES (@hastaisim, @hastatc, @hastatel, @hastayas, @hastacins, @hastaadres)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection)) // Ekleme sorgusu için SqlCommand oluşturma
                    {
                        command.Parameters.AddWithValue("@hastaisim", hastaisim);
                        command.Parameters.AddWithValue("@hastatc", hastatc);
                        command.Parameters.AddWithValue("@hastatel", hastatel);
                        command.Parameters.AddWithValue("@hastayas", hastayas); // Doğru tarih verisi
                        command.Parameters.AddWithValue("@hastacins", hastacins);
                        command.Parameters.AddWithValue("@hastaadres", hastaadres);

                        if (loadedImage != null) // Eğer resim yüklendiyse
                        {
                            using (MemoryStream ms = new MemoryStream())  // Bellek üzerinde geçici bir bellek akışı oluşturma
                            {
                                loadedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // // Resmi MemoryStream'e kaydetme, varsayılan olarak JPEG formatı
                                byte[] photo_aray = ms.ToArray();     // MemoryStream'i byte dizisine dönüştürme
                                 

                               
                                string imageName = $"{hastatc}.jpg";  // Resmi hastanın adıyla aynı şekilde kaydet
                                string savePath = Path.Combine(@"C:\Users\mehmetcu\Desktop\patientimage", imageName); // Dosya yolunu oluşturma
                                File.WriteAllBytes(savePath, photo_aray);  // Dosyayı belirtilen yola kaydetme
                            }
                        }
                        else
                        {
                         
                        }

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                LoadPatientData();
            }
            else
            {
                MessageBox.Show("Geçerli bir tarih giriniz!"); // Geçersiz tarih girildiğinde verilicek uyarı
            }
        }


        private void btnSil_Click(object sender, EventArgs e)
        {
            string hastaisim = txtHastaSisim.Text;
            string hastatc = txtHastaStc.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM hastalar WHERE hastaisim = @hastaisim AND hastatc = @hastatc";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection)) 
                {
                    command.Parameters.AddWithValue("@hastaisim", hastaisim); //girilen verilerin doğruluğuna göre veritabanından verileri silme işlemi
                    command.Parameters.AddWithValue("@hastatc", hastatc);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0) //eğer kolon etkilenirse yani başarılı silinirse kullanıcının karşısına çıkıcak mesaj
                    {
                        // Fotoğraf yolunu oluştur
                        string imagePath = @"C:\Users\mehmetcu\Desktop\patientimage\" + hastatc + ".jpg";

                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath); // Fotoğrafı silme işlemi
                        }
                        else
                        {
                            // eğer fotoğrafı yoksa devam edicek
                        }
                        MessageBox.Show("Hasta verisi başarıyla silindi.");
                        // Verileri yeniden yükleme işlemi

                    }
                    else
                    {
                        MessageBox.Show("Hasta verisi bulunamadı veya silinemedi."); //eksik veya yanlış bilgi girilirse karşısına çıkıcak mesaj
                    }
                }

                connection.Close();
            }
            LoadPatientData(); // her yapılan uygulamadan sonra verileri güncelliyorum
        }


        private void txtHastaetc_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Char.IsWhiteSpace(e.KeyChar);  //keypress özelliği textboxta herhangi bir tuşa basıldığında tetiklenir e.keychar özelliği ile tuşun karakter değeri alınır.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) //iscontrol ile tuşarı kontrol ederken isdigit ile sadece rakamlara izin verilir  
            {
                e.Handled = true; //farklı bir karakter girilirse e.handled true olduğu için girişi engeller.
            }
        }

        private void txtHastaetel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Char.IsWhiteSpace(e.KeyChar);  //keypress özelliği textboxta herhangi bir tuşa basıldığında tetiklenir e.keychar özelliği ile tuşun karakter değeri alınır.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) //iscontrol ile tuşarı kontrol ederken isdigit ile sadece rakamlara izin verilir  
            {
                e.Handled = true; //farklı bir karakter girilirse e.handled true olduğu için girişi engeller.
            }
        }

      
        private Image loadedImage;
        private void btnGozat_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Title = "Bir Resim Dosyası Seçin";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    loadedImage = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.Image = loadedImage;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // Resmi PictureBox boyutlarına sığacak şekilde sıkıştır

                    // Eğer gerekiyorsa, seçilen resmi veritabanına yükleme kodunu da buraya ekleyebilirsiniz.
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Resim yüklenirken bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void btnTemiz_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            txtHastaIsim.Text = null;
            txtHastaTc.Text = null;
            dataGridView1.DataSource = null;            
            checkBox1.Checked = false;
            checkBox2.Checked = false;
        }

        private void btnTemiz2_Click(object sender, EventArgs e)
        {
            rdbtnKad.Checked = false;
            rdbtnErk.Checked = false;
            txtHastaeis.Text = null;
            txtHastaetc.Text = null;
            txtHastaetel.Text = null;
            txtHastaetel.Text = null;
            pictureBox1.Image = null;
            dataGridView1.DataSource = null;
            richTextBox1.Text = null;
        }

        private void txtHastaIsim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar==' '); // Sadece alfabetik karakter girişi için
        }

        private void txtHastaeis_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == ' ');
        }

        private void txtHastaSisim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == ' ');
        }

        private void btnYenile3_Click(object sender, EventArgs e)
        {
            txtHastaSisim.Text = null;
            txtHastaStc.Text = null;
        }

        private void txtHastaTc_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Char.IsWhiteSpace(e.KeyChar);  //keypress özelliği textboxta herhangi bir tuşa basıldığında tetiklenir e.keychar özelliği ile tuşun karakter değeri alınır.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) //iscontrol ile tuşarı kontrol ederken isdigit ile sadece rakamlara izin verilir  
            {
                e.Handled = true; //farklı bir karakter girilirse e.handled true olduğu için girişi engeller.
            }
        }

        private void btnDetay_Click(object sender, EventArgs e)
        {
            this.Hide();

            GirisSaat giris = new GirisSaat();

            giris.ShowDialog();
        }
    }
    

}

