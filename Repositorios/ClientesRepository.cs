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

}


