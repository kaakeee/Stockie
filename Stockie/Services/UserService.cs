using System;
using System.Data.SqlClient;
using Stockie.Models;

namespace Stockie.Services
{
    public class UserService
    {
        public static User AuthenticateUser(string username, string password)
        {
            try
            {
                using (var conn = Data.DatabaseHelper.GetUsersConnection())
                {
                    conn.Open();
                    string query = "SELECT id, username, role, is_active, email FROM users WHERE username = @username AND password = @password AND is_active = 1";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password); // Note: In production, use proper password hashing, TOTALMENTE INSEGURO, SI.//

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Username = reader.GetString(reader.GetOrdinal("username")),
                                    Role = (UserRole)Enum.Parse(typeof(UserRole), reader.GetString(reader.GetOrdinal("role"))),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                                    Email = reader.GetString(reader.GetOrdinal("email"))
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user: " + ex.Message);
            }
        }
    }
}