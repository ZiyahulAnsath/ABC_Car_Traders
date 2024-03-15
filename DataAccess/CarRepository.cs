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
    public class CarRepository
    {
        private readonly string _connectionString;

        public CarRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Admin To List Car Details from the DB
        public List<Car> GetCars(string searchQuery = "")
        {
            List<Car> cars = new List<Car>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Cars";

                // Append the WHERE clause to filter based on the search query
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query += " WHERE CarName LIKE @SearchQuery OR Model LIKE @SearchQuery OR Brand LIKE @SearchQuery OR FualType LIKE @SearchQuery OR Description LIKE @SearchQuery";
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
                            Car car = new Car
                            {
                                CarID = reader.GetInt32(reader.GetOrdinal("CarID")),
                                CarName = reader.GetString(reader.GetOrdinal("CarName")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                FualType = reader.GetString(reader.GetOrdinal("FualType")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                BuyPrice = reader.GetDecimal(reader.GetOrdinal("BuyPrice")),
                                SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                //Image = (byte[])reader["Image"],
                                QuantityAvailable = reader.GetInt32(reader.GetOrdinal("QuantityAvailable"))
                            };

                            cars.Add(car);
                        }
                    }
                }
            }

            return cars;
        }

        //Admin To List Car Details from the DB
        public List<Car> GetCarsCustomer (string searchQuery = "")
        {
            List<Car> cars = new List<Car>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT CarID, CarName, Model, Brand, FualType, Year, Description, SellingPrice, Date, QuantityAvailable FROM Cars";

                // Append the WHERE clause to filter based on the search query
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query += " WHERE CarName LIKE @SearchQuery OR Model LIKE @SearchQuery OR Brand LIKE @SearchQuery OR FualType LIKE @SearchQuery OR Description LIKE @SearchQuery";
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
                            Car car = new Car
                            {
                                CarID = reader.GetInt32(reader.GetOrdinal("CarID")),
                                CarName = reader.GetString(reader.GetOrdinal("CarName")),
                                Model = reader.GetString(reader.GetOrdinal("Model")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                FualType = reader.GetString(reader.GetOrdinal("FualType")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                SellingPrice = reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                //Image = (byte[])reader["Image"],
                                QuantityAvailable = reader.GetInt32(reader.GetOrdinal("QuantityAvailable"))
                            };

                            cars.Add(car);
                        }
                    }
                }
            }

            return cars;
        }


        //Save the new car from the DB
        public void AddCar(Car car)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Cars (CarName, Model, Brand, FualType, Description, Year, BuyPrice, SellingPrice, Date, Image, QuantityAvailable) 
                 VALUES (@CarName, @Model, @Brand, @FualType, @Description, @Year, @BuyPrice, @SellingPrice, @Date, @Image, @QuantityAvailable)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarName", car.CarName);
                    command.Parameters.AddWithValue("@Model", car.Model);
                    command.Parameters.AddWithValue("@Brand", car.Brand);
                    command.Parameters.AddWithValue("@FualType", car.FualType);
                    command.Parameters.AddWithValue("@Description", car.Description);
                    command.Parameters.AddWithValue("@Year", car.Year);
                    command.Parameters.AddWithValue("@BuyPrice", car.BuyPrice);
                    command.Parameters.AddWithValue("@SellingPrice", car.SellingPrice);
                    command.Parameters.AddWithValue("@Date", car.Date);

                    SqlParameter imageParameter = new SqlParameter("@Image", SqlDbType.VarBinary);
                    imageParameter.Value = (object)car.Image ?? DBNull.Value; 
                    command.Parameters.Add(imageParameter);

                    command.Parameters.AddWithValue("@QuantityAvailable", car.QuantityAvailable);

      
                    command.ExecuteNonQuery();
                }
            }
        }

        //Update the Single Car from the DB
        public void UpdateCar(Car car)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Cars SET CarName = @CarName, Model = @Model, Brand = @Brand, FualType = @FualType, 
                                 Description = @Description, Year = @Year, BuyPrice = @BuyPrice, SellingPrice = @SellingPrice, 
                                 Date = @Date, QuantityAvailable = @QuantityAvailable WHERE CarID = @CarID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarID", car.CarID);
                    command.Parameters.AddWithValue("@CarName", car.CarName);
                    command.Parameters.AddWithValue("@Model", car.Model);
                    command.Parameters.AddWithValue("@Brand", car.Brand);
                    command.Parameters.AddWithValue("@FualType", car.FualType);
                    command.Parameters.AddWithValue("@Description", car.Description);
                    command.Parameters.AddWithValue("@Year", car.Year);
                    command.Parameters.AddWithValue("@BuyPrice", car.BuyPrice);
                    command.Parameters.AddWithValue("@SellingPrice", car.SellingPrice);
                    command.Parameters.AddWithValue("@Date", car.Date);
                    //command.Parameters.AddWithValue("@Image", car.Image);
                    command.Parameters.AddWithValue("@QuantityAvailable", car.QuantityAvailable);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        //Delete the single Car from the DB
        public void DeleteCar(int carID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Cars WHERE CarID = @CarID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CarID", carID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        //Count the All Cars
        public int GetTotalCarCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM Cars";
                using (var command = new SqlCommand(query, connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }

    }
}
