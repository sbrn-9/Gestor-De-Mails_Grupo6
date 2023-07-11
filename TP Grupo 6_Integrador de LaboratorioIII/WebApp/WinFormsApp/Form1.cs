using App.Core.Business;
using App.Core.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private MailBusiness _mailBusiness;
        private int currentPageIndex = 1;
        private bool _txtPageIndexBlock = false;
        SqlConnection conexion = new SqlConnection("Persist Security Info=True;Initial Catalog=Mails;Data Source=.; Integrated Security=True;TrustServerCertificate=True;");
        public Form1()
        {
            _mailBusiness = new MailBusiness();
            
            InitializeComponent();
            this.Load += Form1_Load;
            cbTamanioPagina.SelectedIndexChanged += cbTamanioPagina_SelectedIndexChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;
            btnSiguiente.Click += btnSiguiente_Click;
            btnAnterior.Click += btnAnterior_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbTamanioPagina.SelectedIndex = cbTamanioPagina.FindStringExact("10");//por default al inicio carga 10 páginas

            txtPageIndex.Text = currentPageIndex.ToString();
            
        }

        private void CargarDatos(int pageIndex)
        {
            var pageSize = int.Parse(cbTamanioPagina.SelectedItem.ToString());

            _txtPageIndexBlock = true;
            txtPageIndex.Text = pageIndex.ToString();
            _txtPageIndexBlock = false;

            string destinatario = UsuarioActual.NombreUsuario;

            string remitente = "";
            if (destinatario == "Pedro" || destinatario == "Juan") 
            {
                remitente = "Maria";
            }else if (destinatario == "Maria" || destinatario == "Juan")
            {
                remitente = "Pedro";
            }else if(destinatario  == "Maria" || destinatario == "Pedro")
            {
                remitente = "Juan";
            }
            

            var mails = _mailBusiness.Search(textToSearch: txtSearch.Text,
                                             pageSize: pageSize,
                                             pageIndex: pageIndex,
                                             destinatario: destinatario,
                                             remitente: remitente);

            dgvTabla.DataSource = mails;
        }

        
        private void cbTamanioPagina_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPageIndex = 1;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            currentPageIndex = 1;
            CargarDatos(currentPageIndex);
        }


        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            currentPageIndex++;
            CargarDatos(currentPageIndex);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (currentPageIndex == 1)
            {
                return;
            }
            currentPageIndex--;
            CargarDatos(currentPageIndex);
            txtPageIndex.Text = currentPageIndex.ToString();
        }

        private void txtPageIndex_TextChanged(object sender, EventArgs e)
        {
            if (_txtPageIndexBlock)
            {
                return;
            }
            currentPageIndex = int.Parse(txtPageIndex.Text);

            int index;

            if (int.TryParse(txtPageIndex.Text, out index))
            {
                currentPageIndex = index;
            }
            else
            {
                currentPageIndex = 1;
            }

            CargarDatos(currentPageIndex);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEnviar_Click_1(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
            string query = "INSERT INTO Mail (Remitente,Asunto,Contenido,Destinatario) VALUES (@remitente,@asunto,@contenido,@destinatario)";
            
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.AddWithValue("@remitente", TxtRemitente.Text);
            comando.Parameters.AddWithValue("@asunto", TxtAsunto.Text);
            comando.Parameters.AddWithValue("@contenido", TxtContenido.Text);
            comando.Parameters.AddWithValue("@destinatario", TxtDestinatario.Text);
            comando.ExecuteNonQuery();
            MessageBox.Show("Insertado");
            conexion.Close();
        }

        

        private void btnBandejaDeEntrada(object sender, EventArgs e)
        {
            currentPageIndex = 1;
            CargarDatos(currentPageIndex);
            string query = $"SELECT * FROM Mail WHERE Destinatario = '{UsuarioActual.NombreUsuario}'";

            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter();
            adaptador.SelectCommand = comando;
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            dgvTabla.DataSource = tabla;
        }

        private void btnBandejaDeSalida(object sender, EventArgs e)
        {
            currentPageIndex = 1;
            CargarDatos(currentPageIndex);
            string query = $"SELECT * FROM Mail WHERE Remitente = '{UsuarioActual.NombreUsuario}'";

            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter adaptador = new SqlDataAdapter();
            adaptador.SelectCommand = comando;
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            dgvTabla.DataSource = tabla;
        }
    }
}