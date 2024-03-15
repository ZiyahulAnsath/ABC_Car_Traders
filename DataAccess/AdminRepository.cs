using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ABC_Car_Traders.DataAccess
{
    public class AdminRepository
    {
        private string connectionString = "Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True";

        public bool ValidateAdminLogin(string username, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM Admins WHERE Username = @Username AND Password = @Password";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
    }
}
