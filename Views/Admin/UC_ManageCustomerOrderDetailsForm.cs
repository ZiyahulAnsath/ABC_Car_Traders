using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_orderItem_Traders.DataAccess;
using ABC_Car_Traders.Models;


namespace ABC_Car_Traders.Views.Admin
{
    public partial class UC_ManageCustomerOrderDetailsForm : UserControl
    {

        private readonly OrderRepository _orderRepository;
        private DataTable originalDataTable;


        public UC_ManageCustomerOrderDetailsForm()
        {
            InitializeComponent();
            _orderRepository = new OrderRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            originalDataTable = new DataTable();
            LoadOrdersData();
        }


        //Load All Order Details
        private void LoadOrdersData()
        {
            try
            {
                originalDataTable = ToDataTable(_orderRepository.GetAllOrders());
                dgvOrderDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading OrderItem data: " + ex.Message);
            }
        }

      
        private void txtSearchOrders_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchOrders.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("CustomerName LIKE '%{0}%' OR ProductName LIKE '%{0}%' OR Model LIKE '%{0}%' OR Brand LIKE '%{0}%' OR Status LIKE '%{0}%'", searchText);
                dgvOrderDetails.DataSource = dv.ToTable();
            }
            else
            {
                dgvOrderDetails.DataSource = originalDataTable;
            }
        }

        //To List Customer Order Details from the Datagridview function
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


        //To update Customer Order Details from the Datagridview function
        private void dgvOrderDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure valid row index
            {
                DataGridViewRow editedRow = dgvOrderDetails.Rows[e.RowIndex];

                // Retrieve updated data from the DataGridView
                int orderId = Convert.ToInt32(editedRow.Cells["OrderID"].Value);
                int customerId = Convert.ToInt32(editedRow.Cells["CustomerID"].Value);
                string customerName = Convert.ToString(editedRow.Cells["CustomerName"].Value);
                int productId = Convert.ToInt32(editedRow.Cells["ProductID"].Value);
                string productName = Convert.ToString(editedRow.Cells["ProductName"].Value);
                string model = Convert.ToString(editedRow.Cells["Model"].Value);
                string brand = Convert.ToString(editedRow.Cells["Brand"].Value);
                decimal price = Convert.ToDecimal(editedRow.Cells["Price"].Value);
                DateTime orderDate = Convert.ToDateTime(editedRow.Cells["OrderDate"].Value);
                int status = Convert.ToInt32(editedRow.Cells["Status"].Value);

                // Create an instance of OrderItem with the updated data
                OrderItem updatedOrder = new OrderItem
                {
                    OrderID = orderId,
                    CustomerID = customerId,
                    CustomerName = customerName,
                    ProductID = productId,
                    ProductName = productName,
                    Model = model,
                    Brand = brand,
                    Price = price,
                    OrderDate = orderDate,
                    Status = status
                };

                try
                {
                    _orderRepository.UpdateOrder(updatedOrder);
                    MessageBox.Show("Successfully Updated Order Details", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteCustomerOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetails.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvOrderDetails.SelectedRows[0];
                int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);
                try
                {
                    _orderRepository.DeleteOrder(orderId);
                    MessageBox.Show("Order Deleted Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrdersData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
