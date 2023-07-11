using App.Core.Data;
using Microsoft.Data.SqlClient;

namespace WinFormsApp
{

    public partial class Form2 : Form
    {
        SqlConnection conexion = new SqlConnection("Persist Security Info=True;Initial Catalog=Mails;Data Source=.; Integrated Security=True;TrustServerCertificate=True;");
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
            string consulta = "SELECT * FROM UserTable WHERE UserName = @username AND UserPassword = @password";
            SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.AddWithValue("@username", txtUser.Text);
            comando.Parameters.AddWithValue("@password", txtPassword.Text);
            SqlDataReader reader = comando.ExecuteReader();

            if (reader.HasRows == true)
            {
                MessageBox.Show("Bienvenido");

                // Almacenar el nombre de usuario en una variable de sesión
                UsuarioActual.NombreUsuario = txtUser.Text;

                Form1 f1 = new Form1();
                f1.Show();
            }
            else
            {
                MessageBox.Show("Error de credenciales");
            }
            conexion.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
