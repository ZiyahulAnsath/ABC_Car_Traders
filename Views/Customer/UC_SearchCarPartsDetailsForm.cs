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
    public partial class UC_SearchCarPartsDetailsForm : UserControl
    {
        private readonly CarPartRepository _carPartRepository;
        private DataTable originalDataTable;


        public UC_SearchCarPartsDetailsForm()
        {
            InitializeComponent();
            _carPartRepository = new CarPartRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            originalDataTable = new DataTable();
            LoadCarPartsData();
        }

        //Load All Car Details
        public void LoadCarPartsData()
        {
            try
            {
                originalDataTable = ToDataTable(_carPartRepository.GetCustomerCarParts());
                dgvCarPartsDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private void txtSearchCarPart_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchCarPart.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                // Filter the original DataTable based on the search text
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("Model LIKE '%{0}%' OR Brand LIKE '%{0}%' OR PartName LIKE '%{0}%' OR Description LIKE '%{0}%'", searchText);
                dgvCarPartsDetails.DataSource = dv.ToTable();
            }
            else
            {
                dgvCarPartsDetails.DataSource = originalDataTable;
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

        private void btnCarPartsOrder_Click(object sender, EventArgs e)
        {
            if (dgvCarPartsDetails.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow selectedRow = dgvCarPartsDetails.SelectedRows[0];

                    // Validate and convert cell values
                    int productId;
                    if (!int.TryParse(selectedRow.Cells["PartID"].Value?.ToString(), out productId))
                    {
                        MessageBox.Show("Invalid product ID.");
                        return;
                    }

                    string productName = selectedRow.Cells["PartName"].Value?.ToString();
                    string model = selectedRow.Cells["Model"].Value?.ToString();
                    string brand = selectedRow.Cells["Brand"].Value?.ToString();

                    decimal price;
                    if (!decimal.TryParse(selectedRow.Cells["SellingPrice"].Value?.ToString(), out price))
                    {
                        MessageBox.Show("Invalid price.");
                        return;
                    }

                    int customerId = SessionManager.LoggedInCustomerId;
                    string customerName = SessionManager.LoggedInCustomerName;
                    if (customerId == 0)
                    {
                        MessageBox.Show("Invalid customer.");
                        return;
                    }

                    // Create the OrderItem object
                    OrderItem orderItem = new OrderItem
                    {
                        CustomerID = customerId,
                        CustomerName = customerName,
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
