using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Stockie.Data
{
    public class DatabaseHelper
    {
        private static string GetConnectionString(string dbFileName)
        {
            string appDataPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "App_Data");
            string dbPath = Path.Combine(appDataPath, dbFileName);
            
    return $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30";
        }

      private static string UsersConnectionString => GetConnectionString("StockieUsers.mdf");
        private static string StockConnectionString => GetConnectionString("StockieInventory.mdf");

        public static SqlConnection GetUsersConnection()
        {
            return new SqlConnection(UsersConnectionString);
      }

        public static SqlConnection GetStockConnection()
    {
            return new SqlConnection(StockConnectionString);
     }

        public static bool TestConnection()
        {
            try
  {
   EnsureDatabaseFilesExist();
                using (var conn = GetUsersConnection())
   {
      conn.Open();
          return true;
      }
       }
   catch (Exception)
            {
                return false;
            }
        }

        private static void EnsureDatabaseFilesExist()
        {
            string appDataPath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "App_Data");
  
         // Asegurarse de que el directorio App_Data existe
          if (!Directory.Exists(appDataPath))
     {
        Directory.CreateDirectory(appDataPath);
      }

       string usersDbPath = Path.Combine(appDataPath, "StockieUsers.mdf");
        string usersLogPath = Path.Combine(appDataPath, "StockieUsers_log.ldf");
            string stockDbPath = Path.Combine(appDataPath, "StockieInventory.mdf");
   string stockLogPath = Path.Combine(appDataPath, "StockieInventory_log.ldf");

            CreateDatabaseIfNotExists("StockieUsers", usersDbPath, usersLogPath);
            CreateDatabaseIfNotExists("StockieInventory", stockDbPath, stockLogPath);
        }

      private static void CreateDatabaseIfNotExists(string dbName, string dbPath, string logPath)
    {
            if (!File.Exists(dbPath))
    {
      using (var connection = new SqlConnection(@"Server=(LocalDB)\MSSQLLocalDB;Integrated Security=True;"))
      {
         connection.Open();

     using (var command = connection.CreateCommand())
     {
    command.CommandText = $@"
      CREATE DATABASE {dbName}
       ON PRIMARY (
  NAME={dbName}_Data,
            FILENAME='{dbPath}'
)
         LOG ON (
      NAME={dbName}_Log,
            FILENAME='{logPath}'
  )";

           command.ExecuteNonQuery();
             }
           }
         }
    }

        public static void InitializeDatabase()
  {
     EnsureDatabaseFilesExist();
  InitializeUsersDatabase();
            InitializeStockDatabase();
        }

        private static void InitializeUsersDatabase()
        {
            try
  {
                using (var conn = GetUsersConnection())
 {
    conn.Open();

      string createUsersTable = @"
 IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'users')
       BEGIN
         CREATE TABLE users (
           id INT IDENTITY(1,1) PRIMARY KEY,
     username VARCHAR(50) NOT NULL UNIQUE,
             password VARCHAR(255) NOT NULL,
  role VARCHAR(20) NOT NULL,
              is_active BIT DEFAULT 1,
       email VARCHAR(100),
         created_at DATETIME DEFAULT GETDATE()
     )
         END";

    using (var cmd = new SqlCommand(createUsersTable, conn))
     {
        cmd.ExecuteNonQuery();
           }

          // Create default users if they don't exist
    var defaultUsers = new[] {
      new { Username = "admin", Password = "admin123", Role = "Administrator", Email = "admin@stockie.com" },
                new { Username = "supervisor", Password = "supervisor123", Role = "Supervisor", Email = "supervisor@stockie.com" },
    new { Username = "operador", Password = "operador123", Role = "Operador", Email = "operador@stockie.com" }
     };

        foreach (var user in defaultUsers)
          {
        string checkUser = "SELECT COUNT(*) FROM users WHERE username = @username";
          using (var cmd = new SqlCommand(checkUser, conn))
     {
           cmd.Parameters.AddWithValue("@username", user.Username);
        if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
        {
   string createUser = @"
            INSERT INTO users (username, password, role, is_active, email)
      VALUES (@username, @password, @role, 1, @email)";
       using (var insertCmd = new SqlCommand(createUser, conn))
  {
             insertCmd.Parameters.AddWithValue("@username", user.Username);
   insertCmd.Parameters.AddWithValue("@password", user.Password);
        insertCmd.Parameters.AddWithValue("@role", user.Role);
     insertCmd.Parameters.AddWithValue("@email", user.Email);
      insertCmd.ExecuteNonQuery();
    }
       }
          }
         }
          }
  }
      catch (SqlException ex)
   {
         throw new Exception($"Error de base de datos de usuarios: {ex.Message}", ex);
        }
        }

        private static void InitializeStockDatabase()
        {
  try
{
        using (var conn = GetStockConnection())
            {
         conn.Open();

    string createStockTables = @"
         IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'calendar_events')
         BEGIN
       CREATE TABLE calendar_events (
    id INT IDENTITY(1,1) PRIMARY KEY,
       event_date DATE NOT NULL,
          event_type VARCHAR(50) NOT NULL,
   description TEXT,
             created_by INT,
   created_at DATETIME DEFAULT GETDATE(),
       status VARCHAR(20) DEFAULT 'Pending'
      )
        END

 IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'items')
 BEGIN
 CREATE TABLE items (
 id INT IDENTITY(1,1) PRIMARY KEY,
 nombre VARCHAR(100) NOT NULL,
 tipo VARCHAR(50),
 codigo_sn VARCHAR(100)
 )
 END";

         using (var cmd = new SqlCommand(createStockTables, conn))
     {
          cmd.ExecuteNonQuery();
              }
    }
            }
  catch (SqlException ex)
    {
       throw new Exception($"Error de base de datos de stock: {ex.Message}", ex);
            }
        }
    }
}