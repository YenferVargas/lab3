using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    // Dentro del archivo productos.cs

    public partial class productos : Form
    {
        private string _connectionString;

        public productos(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
        }

        private void productos_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PRODUCTO", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    // Asumiendo que tienes un DataGridView llamado 'dataGridViewProductos' en tu formulario de productos
                    dataGridViewProductos.DataSource = dt;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FiltrarProductos(txtFiltro.Text);
        }
        private void FiltrarProductos(string filtro)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Producto WHERE NombreProducto LIKE @filtro";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@filtro", $"%{filtro}%");
                    DataTable dataTable = new DataTable();
                    try
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dataTable);
                        dataGridViewProductos.DataSource = dataTable;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Error al filtrar los datos: {ex.Message}\nQuery: {cmd.CommandText}");
                    }
                }
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            FiltrarProductos(txtFiltro.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }
    }

}
