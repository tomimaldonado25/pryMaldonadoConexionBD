using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace pryMaldonadoConexionBD
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=localhost;Database=Comercio;Trusted_Connection=True;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("✅ Conexión exitosa a la base de datos.");
                }
                MostrarContactos(); // Carga contactos al iniciar
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error al conectar: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaId) " +
                                     "VALUES (@nombre, @descripcion, @precio, @stock, @categoriaId)";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@nombre", "Mouse inalámbrico");
                cmd.Parameters.AddWithValue("@descripcion", "Mouse óptico USB");
                cmd.Parameters.AddWithValue("@precio", 150000);
                cmd.Parameters.AddWithValue("@stock", 20);
                cmd.Parameters.AddWithValue("@categoriaId", 1);
                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ Producto agregado.");
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string selectQuery = "SELECT P.Nombre, P.Precio, P.Stock, C.Nombre AS Categoria FROM Productos P " +
                                     "JOIN Categorias C ON P.CategoriaId = C.Id";
                SqlCommand cmd = new SqlCommand(selectQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MessageBox.Show($"{reader["Nombre"]} - ${reader["Precio"]} - Stock: {reader["Stock"]} - Categoría: {reader["Categoria"]}");
                }
                reader.Close();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE Productos SET Precio = @precio, Descripcion = @desc WHERE Nombre = @nombre";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@precio", 175000);
                cmd.Parameters.AddWithValue("@desc", "Mouse inalámbrico actualizado");
                cmd.Parameters.AddWithValue("@nombre", "Mouse inalámbrico");
                cmd.ExecuteNonQuery();
                MessageBox.Show("🔄 Producto actualizado.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM Productos WHERE Nombre = @nombre";
                SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@nombre", "Mouse inalámbrico");
                cmd.ExecuteNonQuery();
                MessageBox.Show("❌ Producto eliminado.");
            }
        }

        private void MostrarContactos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Nombre, Apellido, Telefono, Correo, Categoria FROM Contactos";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
    }
}

