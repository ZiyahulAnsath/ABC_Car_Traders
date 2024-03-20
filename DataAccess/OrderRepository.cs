using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ABC_Car_Traders.Models;

namespace ABC_orderItem_Traders.DataAccess
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //List All Order Table data
        public List<OrderItem> GetAllOrders(string searchQuery = "")
        {
            List<OrderItem> orders = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Orders";

                // Append the WHERE clause to filter based on the search query
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query += " WHERE CustomerName LIKE @searchQuery OR ProductName LIKE @searchQuery OR Model LIKE @searchQuery OR Brand LIKE @searchQuery OR Status LIKE @searchQuery";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        // Add the search query parameter
                        command.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                    }

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderItem order = new OrderItem
                            {
                                OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                                CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                Status = reader.GetInt32(reader.GetOrdinal("Status"))
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        //Insert Orders Table data
        public void PlaceOrderItem(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Orders (CustomerID, CustomerName, ProductID, ProductName, Model, Brand, Price, OrderDate, Status) 
                                 VALUES (@CustomerID, @CustomerName, @ProductID, @ProductName, @Model, @Brand, @Price, @OrderDate, @Status)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", orderItem.CustomerID);
                    command.Parameters.AddWithValue("@CustomerName", orderItem.CustomerName);
                    command.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
                    command.Parameters.AddWithValue("@ProductName", orderItem.ProductName);
                    command.Parameters.AddWithValue("@Model", orderItem.Model);
                    command.Parameters.AddWithValue("@Brand", orderItem.Brand);
                    command.Parameters.AddWithValue("@Price", orderItem.Price);
                    command.Parameters.AddWithValue("@OrderDate", orderItem.OrderDate);
                    command.Parameters.AddWithValue("@Status", orderItem.Status);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Update Order
        public void UpdateOrder(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE Orders SET CustomerID = @CustomerID, CustomerName = @CustomerName, 
                                 ProductID = @ProductID, ProductName = @ProductName, Model = @Model, 
                                 Brand = @Brand, Price = @Price, OrderDate = @OrderDate, Status = @Status 
                                 WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", orderItem.CustomerID);
                    command.Parameters.AddWithValue("@CustomerName", orderItem.CustomerName);
                    command.Parameters.AddWithValue("@ProductID", orderItem.ProductID);
                    command.Parameters.AddWithValue("@ProductName", orderItem.ProductName);
                    command.Parameters.AddWithValue("@Model", orderItem.Model);
                    command.Parameters.AddWithValue("@Brand", orderItem.Brand);
                    command.Parameters.AddWithValue("@Price", orderItem.Price);
                    command.Parameters.AddWithValue("@OrderDate", orderItem.OrderDate);
                    command.Parameters.AddWithValue("@Status", orderItem.Status);
                    command.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete Order
        public void DeleteOrder(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"DELETE FROM Orders WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Count total orders
        public int CountTotalOrders()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT COUNT(*) FROM Orders";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }
    }
}
