using Models;
using Microsoft.Data.Sqlite;
public class PresupuestoRepository
{
    private readonly string CadenaDeConexion = @"Data Source=Tienda.db;Cache=Shared";


    public List<Presupuesto> ListarPresupuesto()
    {
        List<Presupuesto> listaPresupuesto = new List<Presupuesto>();
        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            string query = "SELECT * FROM Presupuestos;";
            SqliteCommand command = new SqliteCommand(query, connection);
            connection.Open();
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuesto.FechaCreacion = Convert.ToString(reader["FechaCreacion"]);
                    listaPresupuesto.Add(presupuesto);
                }
            }
            connection.Close();

        }
        return listaPresupuesto;
    }



    public void CrearPresupuesto(Presupuesto presupuesto)
    {
        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            var query = "INSERT INTO Presupuestos(idPresupuesto, NombreDestinatario, FechaCreacion) VALUES(@idPresupuesto, @NombreDestinatario, @FechaCreacion)";
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", presupuesto.IdPresupuesto));
            command.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
            command.ExecuteNonQuery();
            connection.Close();

        }

    }

    public List<PresupuestoDetalle> ObtenerPresupuestoDetalle(int idPresupuesto)
    {
        List<PresupuestoDetalle> listadoDeDetalles = new List<PresupuestoDetalle>();
        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            var query = "SELECT * FROM PresupuestosDetalles A INNER JOIN Productos B ON A.idProducto = @id = B.idProducto WHERE A.idProducto = @id";
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", idPresupuesto));
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var prod = new Producto();
                    prod.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    prod.Descripcion = reader["Descripcion"].ToString();
                    prod.Precio = Convert.ToInt32(reader["Precio"]);
                    var pres = new PresupuestoDetalle();
                    pres.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                    listadoDeDetalles.Add(pres);
                }
            }
        }
        return listadoDeDetalles;
    }


    public void AgregarPresupuestoYProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            var query = "INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@pres, @prod, @cant)";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@presupuesto", idPresupuesto));
            command.Parameters.Add(new SqliteParameter("@producto", idProducto));
            command.Parameters.Add(new SqliteParameter("@cantidad", cantidad));
            command.ExecuteNonQuery();
            connection.Close();

        }
    }

    public void EliminarPresupuesto(int idPresupuesto)
    {
        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            var query = "DELETE FROM Presupuesto, PresupuestosDetalle WHERE idPresupuesto = @id";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", idPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }





}


