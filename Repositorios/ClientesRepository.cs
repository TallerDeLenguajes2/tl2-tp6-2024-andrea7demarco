using Models;
using Microsoft.Data.Sqlite;
public class ClientesRepository
{
    private readonly string CadenaDeConexion = @"Data Source=Tienda.db;Cache=Shared";

   public List<Cliente> ListarCliente()
   {
      List<Cliente> listaClientes = new List<Cliente>();
      using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
      {
         string query = "SELECT * FROM Clientes;";
         SqliteCommand command = new SqliteCommand(query, connection);
         connection.Open();
         using (SqliteDataReader reader = command.ExecuteReader())
         {
            while (reader.Read())
            {
               var cliente = new Cliente();
               cliente.IdCliente = Convert.ToInt32(reader["ClienteId"]);
               cliente.Nombre = reader["Nombre"].ToString();
               cliente.Email = reader["Email"].ToString();
               cliente.Telefono = reader["Telefono"].ToString();
               listaClientes.Add(cliente);
            }
         }
         connection.Close();

      }
      return listaClientes;
   }


   public void CrearCliente(Cliente cliente)
   {
      using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
      {
         var query = "INSERT INTO Clientes(Nombre, Email, Telefono) VALUES(@Nombre, @Email, @Telefono)";
         connection.Open();
         var command = new SqliteCommand(query, connection);
         command.Parameters.Add(new SqliteParameter("@Nombre", cliente.Nombre));
         command.Parameters.Add(new SqliteParameter("@Email", cliente.Email));
         command.Parameters.Add(new SqliteParameter("@Telefono", cliente.Telefono));
         command.ExecuteNonQuery();
         connection.Close();

      }

   }


   public void ModificarCliente(int id, Cliente cliente)
   {
      using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
      {
         var query = @"UPDATE Clientes SET Nombre = @Nombre, Email = @Email, Telefono = @Telefono
         WHERE ClienteId = @id";
         connection.Open();
         var command = new SqliteCommand(query, connection);
         command.Parameters.Add(new SqliteParameter("@id", cliente.IdCliente));
         command.Parameters.Add(new SqliteParameter("@Nombre", cliente.Nombre));
         command.Parameters.Add(new SqliteParameter("@Email", cliente.Email));
         command.Parameters.Add(new SqliteParameter("@Telefono", cliente.Telefono));
         command.ExecuteNonQuery();
         connection.Close();
      }
   }

public Cliente BuscaClientePorID(int id)
{
    Cliente cliente = null;
    using (SqliteConnection connection = new SqliteConnection(CadenaDeConexion))
    {
        string query = "SELECT * FROM Clientes WHERE ClienteId = @id";
        connection.Open();
        var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                cliente = new Cliente
                {
                    IdCliente = Convert.ToInt32(reader["ClienteId"]),
                    Nombre = reader["Nombre"].ToString() ?? "No tiene descripcion",
                    Email = reader["Email"].ToString() ?? "No tiene mail",
                    Telefono = reader["Telefono"].ToString() ?? "No tiene teléfono",

                };
            }
        }
    }
    return cliente ?? throw new Exception("Cliente no encontrado");
}

}



/*public Producto BuscaProductoPorID(int id)
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
*/