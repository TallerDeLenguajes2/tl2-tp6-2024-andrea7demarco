using Models;
using Microsoft.Data.Sqlite;
public class ProductosRepository
{
   private readonly string CadenaDeConexion = @"Data Source=Tienda.db;Cache=Shared";
   public void CrearProducto(Producto producto)
   {
      using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
      {
         var query = "INSERT INTO Productos(Descripcion, Precio) VALUES(@Descripcion, @Precio)";
         connection.Open();
         var command = new SqliteCommand(query, connection);
         command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
         command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
         command.ExecuteNonQuery();
         connection.Close();

      }

   }

   public List<Producto> ListarProducto()
   {
      List<Producto> listaProd = new List<Producto>();
      using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
      {
         string query = "SELECT * FROM Productos;";
         SqliteCommand command = new SqliteCommand(query, connection);
         connection.Open();
         using (SqliteDataReader reader = command.ExecuteReader())
         {
            while (reader.Read())
            {
               var prod = new Producto();
               prod.IdProducto = Convert.ToInt32(reader["idProducto"]);
               prod.Descripcion = reader["Descripcion"].ToString();
               prod.Precio = Convert.ToInt32(reader["Precio"]);
               listaProd.Add(prod);
            }
         }
         connection.Close();

      }
      return listaProd;
   }

   public void ModificarProducto(int id, Producto producto)
   {
      using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
      {
         var query = @"UPDATE Productos SET Descripcion = @descripcion, Precio = @precio 
         WHERE idProducto = @id";
         connection.Open();
         var command = new SqliteCommand(query, connection);
         command.Parameters.Add(new SqliteParameter("@id", producto.IdProducto));
         command.Parameters.Add(new SqliteParameter("@precio", producto.Precio));
         command.Parameters.Add(new SqliteParameter("@descripcion", producto.Descripcion));
         command.ExecuteNonQuery();
         connection.Close();
      }
   }

public Producto BuscaProductoPorID(int id)
{
    Producto producto = null;
    using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
    {
        string query = "SELECT * FROM Productos WHERE idProducto = @id";
        connection.Open();
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                producto = new Producto
                {
                    IdProducto = Convert.ToInt32(reader["idProducto"]),
                    Descripcion = reader["Descripcion"].ToString() ?? "No tiene descripcion",
                    Precio = Convert.ToInt32(reader["Precio"])
                };
            }
        }
    }
    return producto ?? throw new Exception("Producto no encontrado");
}

public void EliminarProductoPorID(int id)
{
   using(SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
   {
      connection.Open();
      string query = "DELETE FROM Productos WHERE idProducto = (@id)";
      var command = new SqliteCommand(query, connection);
      command.Parameters.AddWithValue("@id", id);
      command.ExecuteNonQuery();
      connection.Close();
   }
}

}

