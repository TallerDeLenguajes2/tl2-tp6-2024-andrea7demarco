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
        connection.Open();
        
        // Inserción del presupuesto
        var queryPresupuesto = "INSERT INTO Presupuestos (idPresupuesto, FechaCreacion) VALUES (@idPresupuesto, @FechaCreacion)";
        using (var commandPresupuesto = new SqliteCommand(queryPresupuesto, connection))
        {
            commandPresupuesto.Parameters.Add(new SqliteParameter("@idPresupuesto", presupuesto.IdPresupuesto));
            commandPresupuesto.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuesto.FechaCreacion));
            commandPresupuesto.ExecuteNonQuery();
        }


        foreach (var detalle in presupuesto.Detalle)
        {
            var queryDetalle = "INSERT INTO PresupuestoDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad)";
            using (var commandDetalle = new SqliteCommand(queryDetalle, connection))
            {
                commandDetalle.Parameters.Add(new SqliteParameter("@idPresupuesto", presupuesto.IdPresupuesto));
                commandDetalle.Parameters.Add(new SqliteParameter("@idProducto", detalle.Producto?.IdProducto ?? 0)); // Asume que el producto tiene un IdProducto
                commandDetalle.Parameters.Add(new SqliteParameter("@Cantidad", detalle.Cantidad));
                commandDetalle.ExecuteNonQuery();
            }
        }
        
        connection.Close();
    }
}


public List<PresupuestoDetalle> ObtenerPresupuestoDetalle(int idPresupuesto)
{
    List<PresupuestoDetalle> listadoDeDetalles = new List<PresupuestoDetalle>();
    try
    {
        using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
        {
            var query = "SELECT * FROM PresupuestosDetalle A INNER JOIN Productos B ON A.idProducto = B.idProducto WHERE A.idPresupuesto = @id";
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
                    pres.Producto = prod;
                    pres.Cantidad = Convert.ToInt32(reader["Cantidad"]);
                    
                    listadoDeDetalles.Add(pres);
                }
            }
        }
    }
    catch (SqliteException ex)
    {
        Console.WriteLine($"SQLite Error: {ex.Message}");

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
            var query = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", idPresupuesto);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }


public Presupuesto BuscarPresupuestoPorID(int id)
{
    Presupuesto presupuesto = null;
    using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
    {
        string query = "SELECT * FROM Presupuestos WHERE idPresupuesto = @id";
        connection.Open();
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        using(SqliteDataReader reader = command.ExecuteReader())
        {
            if(reader.Read())
            {
                presupuesto = new Presupuesto
                {
                    IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                    FechaCreacion = reader["FechaCreacion"].ToString()
                };
            }
        }
    }
    return presupuesto ?? throw new Exception("Presupuesto no encontrado");
}


public void ModificarPresupuesto(int id, Presupuesto presupuesto)
{
    using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
    {
        var query = @"UPDATE Presupuestos SET FechaCreacion = @fecha
        WHERE idPresupuesto = @id";
        connection.Open();
        var command = new SqliteCommand(query, connection);
        command.Parameters.Add(new SqliteParameter("@id", presupuesto.IdPresupuesto));
        command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));
        command.ExecuteNonQuery();
        connection.Close();

    }

}







}


