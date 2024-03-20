using System;
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

        //public void PlaceorderItemOrder(int customerId, int orderItemId, int quantity)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        string query = @"INSERT INTO Orders (CustomerID, OrderDate) 
        //                         VALUES (@CustomerID, @OrderDate);
        //                         SELECT SCOPE_IDENTITY();";

        //        connection.Open();

        //        int orderId;
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CustomerID", customerId);
        //            command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
        //            orderId = Convert.ToInt32(command.ExecuteScalar());
        //        }

        //        string insertOrderItemsQuery = @"INSERT INTO orderItemOrderItems (OrderID, orderItemID, Quantity) 
        //                                         VALUES (@OrderID, @orderItemID, @Quantity)";
        //        using (SqlCommand command = new SqlCommand(insertOrderItemsQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@OrderID", orderId);
        //            command.Parameters.AddWithValue("@orderItemID", orderItemId);
        //            command.Parameters.AddWithValue("@Quantity", quantity);

        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        //public void PlaceorderItemPartOrder(int customerId, int orderItemPartId, int quantity)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        string query = @"INSERT INTO Orders (CustomerID, OrderDate) 
        //                         VALUES (@CustomerID, @OrderDate);
        //                         SELECT SCOPE_IDENTITY();";

        //        connection.Open();

        //        int orderId;
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CustomerID", customerId);
        //            command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
        //            orderId = Convert.ToInt32(command.ExecuteScalar());
        //        }

        //        string insertOrderItemsQuery = @"INSERT INTO orderItemPartOrderItems (OrderID, PartID, Quantity) 
        //                                         VALUES (@OrderID, @PartID, @Quantity)";
        //        using (SqlCommand command = new SqlCommand(insertOrderItemsQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@OrderID", orderId);
        //            command.Parameters.AddWithValue("@PartID", orderItemPartId);
        //            command.Parameters.AddWithValue("@Quantity", quantity);

        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        // Place order based on selected product from DataGridView
        //public void PlaceOrder(int customerId, int productId, int quantity, string orderType)
        //{
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        string query = @"INSERT INTO Orders (CustomerID, OrderDate) 
        //                     VALUES (@CustomerID, @OrderDate);
        //                     SELECT SCOPE_IDENTITY();";

        //        connection.Open();

        //        int orderId;
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CustomerID", customerId);
        //            command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
        //            orderId = Convert.ToInt32(command.ExecuteScalar());
        //        }

        //        string insertOrderItemsQuery = string.Empty;
        //        if (orderType == "orderItem")
        //        {
        //            insertOrderItemsQuery = @"INSERT INTO orderItemOrderItems (OrderID, orderItemID, Quantity) 
        //                                     VALUES (@OrderID, @ProductID, @Quantity)";
        //        }
        //        else if (orderType == "orderItemPart")
        //        {
        //            insertOrderItemsQuery = @"INSERT INTO orderItemPartOrderItems (OrderID, PartID, Quantity) 
        //                                     VALUES (@OrderID, @ProductID, @Quantity)";
        //        }

        //        using (SqlCommand command = new SqlCommand(insertOrderItemsQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@OrderID", orderId);
        //            command.Parameters.AddWithValue("@ProductID", productId);
        //            command.Parameters.AddWithValue("@Quantity", quantity);

        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        public void PlaceorderItemOrder(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO OrderItem (CustomerID,CustomerName,ProductID,ProductName,Model,Brand,Price,OrderDate,Status) 
                                 VALUES (@CustomerID,@CustomerName, @ProductID, @ProductName, @Model,@Brand,@Price,@OrderDate,@Status)";

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
    }
}
