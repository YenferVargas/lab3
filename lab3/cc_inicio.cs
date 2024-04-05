using lab3;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace lab3
{
    public partial class cc_inicio : Form
    {
        public cc_inicio()
        {
            InitializeComponent();
            this.btnConectar.Click += new EventHandler(this.btnConectar_Click_1);
            this.chkAutenticacionWindows.CheckedChanged += new EventHandler(this.chkAutenticacionWindows_CheckedChanged);
        }

        private void cc_inicio_Load(object sender, EventArgs e)
        {
            // Código de inicialización si lo necesitas
        }

        private void chkAutenticacionWindows_CheckedChanged(object sender, EventArgs e)
        {
            // Cuando se marca el checkbox, limpiamos y deshabilitamos los campos de usuario y contraseña
            if (chkAutenticacionWindows.Checked)
            {
                txtUsuario.Clear();
                txtContraseña.Clear();
                txtUsuario.Enabled = false;
                txtContraseña.Enabled = false;
            }
            else
            {
                txtUsuario.Enabled = true;
                txtContraseña.Enabled = true;
            }
        }

        private void btnConectar_Click_1(object sender, EventArgs e)
        {
            string servidor = txtServidor.Text;
            string baseDatos = txtBaseDatos.Text;
            bool autenticacionWindows = chkAutenticacionWindows.Checked;



            string connectionString;

            if (autenticacionWindows)
            {
                // Usamos Integrated Security si la autenticación de Windows está marcada
                connectionString = $@"Server={servidor}; Database={baseDatos}; Integrated Security=True;";
            }
            else
            {
                // Usamos la autenticación de SQL Server si la autenticación de Windows no está marcada
                string usuario = txtUsuario.Text;
                string contraseña = txtContraseña.Text;
                connectionString = $@"Server={servidor}; Database={baseDatos}; User Id={usuario}; Password={contraseña};";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Conexión exitosa a la base de datos.");

                    // Ocultar el formulario actual
                    this.Hide();

                    // Crear y mostrar el nuevo formulario
                    productos formPrincipal = new productos(connectionString);
                    formPrincipal.FormClosed += (s, args) => this.Close(); // Asegurarse de que la aplicación se cierra cuando el formulario principal se cierra
                    formPrincipal.Show();
                   
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Error al intentar conectar a la base de datos: {ex.Message}");
                }
            }
        }
    }
}
