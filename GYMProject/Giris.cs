using System.Drawing.Drawing2D;
using Microsoft.Data.SqlClient;

namespace GYMProject
{
    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }

        private void userNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            // Kullan�c� ad� ve �ifreyi al�n
            string username = userNameTextBox.Text;  // Kullan�c� ad� textBox'�
            string password = passwordTextBox.Text;  // �ifre textBox'�

            // Veritaban� ba�lant� dizesi (De�i�tirin: sunucu, veritaban� ad� vb.)
            string connectionString = GlobalVariables.ConnectionString;

            // SQL sorgusu
            string query = "SELECT COUNT(1) FROM UserAuth WHERE Username = @username AND Password = @password";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // Ba�lant�y� a�
                    conn.Open();

                    // SQL komutunu olu�tur
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password); // �ifrenin g�venli�i i�in hashing �nerilir

                    // Sonucu kontrol et
                    int result = Convert.ToInt32(cmd.ExecuteScalar());

                    // Giri� ba�ar�l�ysa
                    if (result > 0)
                    {
                        MessageBox.Show("Giri� ba�ar�l�!", "Ba�ar�l�", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        // AnaEkranAdmin formuna y�nlendir
                        AnaEkranAdmin adminForm = new AnaEkranAdmin();
                        adminForm.Show();
                        this.Hide();  // Giri� ekran�n� gizle
                    }
                    else
                    {
                        MessageBox.Show("Kullan�c� ad� veya �ifre hatal�!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata olu�tu: " + ex.Message);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.FromArgb(204, 255, 255, 255);

            int cornerRadius = 60;


            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90); // Sol �st
            path.AddArc(panel1.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90); // Sa� �st
            path.AddArc(panel1.Width - cornerRadius, panel1.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90); // Sa� alt
            path.AddArc(0, panel1.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90); // Sol alt
            path.CloseFigure();
            panel1.Region = new Region(path);
        }

        private bool isPasswordVisible = false;
        private void Giris_Load(object sender, EventArgs e)
        {
            userNameLabel.BackColor = Color.Transparent;
            passwordLabel.BackColor = Color.Transparent;
            logoBox.BackColor = Color.Transparent;
            logoName1.BackColor = Color.Transparent;
            logoName2.BackColor = Color.Transparent;


            passwordTextBox.PasswordChar = '*';

            // G�z simgesinin ilk halini ayarlama
            eyePictureBox.Image = Properties.Resources.hide;
            eyePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void eyePictureBox_Click(object sender, EventArgs e)
        {
            if (isPasswordVisible)
            {
                // �ifreyi gizle
                passwordTextBox.PasswordChar = '*';
                eyePictureBox.Image = Properties.Resources.hide;  // G�z kapal� simgesi
            }
            else
            {
                // �ifreyi g�ster
                passwordTextBox.PasswordChar = '\0';  // �ifreyi g�sterir
                eyePictureBox.Image = Properties.Resources.eye;  // G�z a��k simgesi
            }

            // Durum de�i�tir
            isPasswordVisible = !isPasswordVisible;
        }
    }
}
