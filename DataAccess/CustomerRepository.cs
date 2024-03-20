using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ABC_Car_Traders.Models;

namespace ABC_Car_Traders.DataAccess
{
    class CustomerRepository
    {
        private string connectionString = "Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True";


        //Cutomer Login Functionality 
        public bool ValidateCustomerLogin(string username, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = "SELECT Password FROM Customers WHERE Username = @Username";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        var hashedPassword = (string)command.ExecuteScalar();

                        if (hashedPassword != null)
                        {
                            // Hash the provided password for comparison
                            string hashedInputPassword = PasswordHelper.HashPassword(password);

                            // Compare the hashed passwords
                            if (hashedPassword == hashedInputPassword)
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during customer login: " + ex.Message);
                }
            }
            return false;
        }

        //New Cutomer Registration Functionality 
        public void InsertCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Customers (Name, Gender, Username, Password, Email, Phone, Address) VALUES (@Name, @Gender, @Username, @Password, @Email, @Phone, @Address)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Gender", customer.Gender);
                    command.Parameters.AddWithValue("@Username", customer.Username);
                    command.Parameters.AddWithValue("@Password", customer.Password);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Phone", customer.Phone);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.ExecuteNonQuery();
                }
            }
        }



        //List All Cutomer Details Functionality 
        public List<Customer> GetAllCustomers(string searchQuery = "")
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Customers";

                // Add search criteria to the query if provided
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query += " WHERE Name LIKE @searchQuery OR Gender LIKE @searchQuery OR Username LIKE @searchQuery OR Phone LIKE @searchQuery OR Address LIKE @searchQuery OR Email LIKE @searchQuery"; 
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters for search criteria
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        command.Parameters.AddWithValue("@searchQuery", "%" + searchQuery + "%");
                    }

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
                            };

                            customers.Add(customer);
                        }
                    }
                }
            }

            return customers;
        }



        //Update Cutomer Details Functionality 
        public void UpdateCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"UPDATE Customers 
                              SET Name = @Name, 
                                  Gender = @Gender, 
                                  Password = @Password, 
                                  Email = @Email, 
                                  Phone = @Phone, 
                                  Address = @Address 
                              WHERE CustomerID = @CustomerID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", customer.Name);
                    command.Parameters.AddWithValue("@Gender", customer.Gender);
                    command.Parameters.AddWithValue("@Password", customer.Password);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@Phone", customer.Phone ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                    command.ExecuteNonQuery();
                }
            }
        }



        //Delete single Cutomer Detail Functionality 
        public void DeleteCustomer(int customerId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    command.ExecuteNonQuery();
                }
            }
        }

        //Count the All Customers
        public int GetTotalCustomerCount()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM Customers";
                using (var command = new SqlCommand(query, connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }


        // Get customer ID by username
        public int GetCustomerIdByUsername(string username)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT CustomerID FROM Customers WHERE Username = @Username";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
