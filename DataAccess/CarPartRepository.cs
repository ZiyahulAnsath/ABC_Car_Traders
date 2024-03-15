using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ABC_Car_Traders.Models;
using System.Data;


namespace ABC_Car_Traders.DataAccess
{
    class CarPartRepository
    {
        private readonly string _connectionString;

        public CarPartRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<CarPart> GetCarParts(string searchKeyword = "")
        {
            List<CarPart> carParts = new List<CarPart>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT PartID, Model, Brand, PartName, Description, BuyPrice, SellingPrice, Date, QuantityAvailable, Image FROM CarParts";

                // Apply filter if search keyword is provided
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    query += " WHERE Model LIKE '%' + @SearchKeyword + '%' OR Brand LIKE '%' + @SearchKeyword + '%' OR PartName LIKE '%' + @SearchKeyword + '%'";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(searchKeyword))
                    {
                        command.Parameters.AddWithValue("@SearchKeyword", searchKeyword);
                    }

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CarPart carPart = new CarPart
                            {
                                PartID = reader.GetInt32(reader.GetOrdinal("PartID")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                PartName = reader.GetString(reader.GetOrdinal("PartName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                BuyPrice = reader.GetDecimal(reader.GetOrdinal("BuyPrice")),
                                SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                QuantityAvailable = reader.GetInt32(reader.GetOrdinal("QuantityAvailable")),
                                //Image = (byte[])reader["Image"]
                            };

                            carParts.Add(carPart);
                        }
                    }
                }
            }

            return carParts;
        }

        public List<CarPart> GetCustomerCarParts(string searchKeyword = "")
        {
            List<CarPart> carParts = new List<CarPart>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT PartID, Model, Brand, PartName, Description, SellingPrice, Date, QuantityAvailable, Image FROM CarParts";

                // Apply filter if search keyword is provided
                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    query += " WHERE Model LIKE '%' + @SearchKeyword + '%' OR Brand LIKE '%' + @SearchKeyword + '%' OR PartName LIKE '%' + @SearchKeyword + '%'";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(searchKeyword))
                    {
                        command.Parameters.AddWithValue("@SearchKeyword", searchKeyword);
                    }

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CarPart carPart = new CarPart
                            {
                                PartID = reader.GetInt32(reader.GetOrdinal("PartID")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                PartName = reader.GetString(reader.GetOrdinal("PartName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                QuantityAvailable = reader.GetInt32(reader.GetOrdinal("QuantityAvailable")),
                                //Image = (byte[])reader["Image"]
                            };

                            carParts.Add(carPart);
                        }
                    }
                }
            }

            return carParts;
        }


        public void AddCarPart(CarPart carPart)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO CarParts (Model, Brand, PartName, Description, BuyPrice, SellingPrice, Date, QuantityAvailable, Image) 
                                 VALUES (@Model, @Brand, @PartName, @Description, @BuyPrice, @SellingPrice, @Date, @QuantityAvailable, @Image)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Model", carPart.Model);
                    command.Parameters.AddWithValue("@Brand", carPart.Brand);
                    command.Parameters.AddWithValue("@PartName", carPart.PartName);
                    command.Parameters.AddWithValue("@Description", carPart.Description);
                    command.Parameters.AddWithValue("@BuyPrice", carPart.BuyPrice);
                    command.Parameters.AddWithValue("@SellingPrice", carPart.SellingPrice);
                    command.Parameters.AddWithValue("@Date", carPart.Date);
                    command.Parameters.AddWithValue("@QuantityAvailable", carPart.QuantityAvailable);

                    SqlParameter imageParameter = new SqlParameter("@Image", SqlDbType.VarBinary);
                    imageParameter.Value = (object)carPart.Image ?? DBNull.Value;
                    command.Parameters.Add(imageParameter);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void UpdateCarPart(CarPart carPart)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE CarParts SET Model = @Model, Brand = @Brand, PartName = @PartName, 
                         Description = @Description, BuyPrice = @BuyPrice, SellingPrice = @SellingPrice, 
                         Date = @Date, QuantityAvailable = @QuantityAvailable";

                // Check if the image is being updated
                if (carPart.Image != null)
                {
                    query += ", Image = @Image";
                }

                query += " WHERE PartID = @PartID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Model", carPart.Model);
                    command.Parameters.AddWithValue("@Brand", carPart.Brand);
                    command.Parameters.AddWithValue("@PartName", carPart.PartName);
                    command.Parameters.AddWithValue("@Description", carPart.Description);
                    command.Parameters.AddWithValue("@BuyPrice", carPart.BuyPrice);
                    command.Parameters.AddWithValue("@SellingPrice", carPart.SellingPrice);
                    command.Parameters.AddWithValue("@Date", carPart.Date);
                    command.Parameters.AddWithValue("@QuantityAvailable", carPart.QuantityAvailable);
                    command.Parameters.AddWithValue("@PartID", carPart.PartID);

                    // Add the @Image parameter only if it's being updated
                    if (carPart.Image != null)
                    {
                        // Convert the Image to a byte array if it's a string
                        if (carPart.Image is string)
                        {
                            byte[] imageBytes = Convert.FromBase64String(carPart.Image.ToString());
                            command.Parameters.AddWithValue("@Image", imageBytes);
                        }
                        else
                        {
                            // Otherwise, use the provided byte array
                            command.Parameters.AddWithValue("@Image", carPart.Image);
                        }
                    }

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteCarPart(int partID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM CarParts WHERE PartID = @PartID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PartID", partID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        //Count the All Car Parts
        public int GetTotalCarPartCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM CarParts";
                using (var command = new SqlCommand(query, connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }

    }
}
