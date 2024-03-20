using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_Car_Traders.DataAccess;
using ABC_Car_Traders.Controllers;
using ABC_Car_Traders.Models;

namespace ABC_Car_Traders.Views.Customer
{
    public partial class UC_SearchCarDetailsForm : UserControl
    {

        private readonly CarRepository _carRepository;
        private DataTable originalDataTable;


        public UC_SearchCarDetailsForm()
        {
            InitializeComponent();
            _carRepository = new CarRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            originalDataTable = new DataTable();
            LoadCarData();

        }

        private void LoadCarData()
        {
            try
            {
                originalDataTable = ToDataTable(_carRepository.GetCarsCustomer());
                dgvCarDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }


        private DataTable ToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable();
            foreach (var prop in typeof(T).GetProperties())
            {
                table.Columns.Add(prop.Name);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (var prop in typeof(T).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }


        //Search car Details in to the Customer
        private void txtSearchCar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchCar.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("CarName LIKE '%{0}%' OR Model LIKE '%{0}%' OR Brand LIKE '%{0}%' OR FualType LIKE '%{0}%' OR Description LIKE '%{0}%'", searchText);
                dgvCarDetails.DataSource = dv.ToTable();
            }
            else
            {
                dgvCarDetails.DataSource = originalDataTable;
            }
        }

        private void btnCarOrder_Click(object sender, EventArgs e)
        {
            if (dgvCarDetails.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow selectedRow = dgvCarDetails.SelectedRows[0];

                    // Validate and convert cell values
                    int productId;
                    if (!int.TryParse(selectedRow.Cells["CarID"].Value?.ToString(), out productId))
                    {
                        MessageBox.Show("Invalid product ID.");
                        return;
                    }

                    string productName = selectedRow.Cells["CarName"].Value?.ToString();
                    string model = selectedRow.Cells["Model"].Value?.ToString();
                    string brand = selectedRow.Cells["Brand"].Value?.ToString();

                    decimal price;
                    if (!decimal.TryParse(selectedRow.Cells["SellingPrice"].Value?.ToString(), out price))
                    {
                        MessageBox.Show("Invalid price.");
                        return;
                    }

                    // Check if the selected customer exists
                    int customerId = 7; 
                    if (customerId == 0)
                    {
                        MessageBox.Show("Invalid customer.");
                        return;
                    }

                    // Create the OrderItem object
                    OrderItem orderItem = new OrderItem
                    {
                        CustomerID = customerId,
                        CustomerName = "", 
                        ProductID = productId,
                        ProductName = productName,
                        Model = model,
                        Brand = brand,
                        Price = price,
                        OrderDate = DateTime.Now, 
                        Status = 0 
                    };

                    // Assuming you have access to orderController, replace with actual instance creation if necessary
                    OrderController orderController = new OrderController("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
                    orderController.RegisterOrderItem(orderItem);

                    MessageBox.Show("Order placed successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to place order: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to place an order.");
            }
        }
    }
}