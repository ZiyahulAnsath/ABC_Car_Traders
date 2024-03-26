using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_Car_Traders.Controllers;
using ABC_orderItem_Traders.DataAccess;
using ABC_Car_Traders.Models;

namespace ABC_Car_Traders.Views.Customer
{
    public partial class UC_OrderType : UserControl
    {
        private readonly OrderRepository _orderRepository;
        private DataTable originalDataTable;

        public UC_OrderType()
        {
            InitializeComponent();
            _orderRepository = new OrderRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            originalDataTable = new DataTable();
            LoadOrdersData();
        }

        // Load All Order Details
        private void LoadOrdersData()
        {
            try
            {
                int customerID = SessionManager.LoggedInCustomerId;
                originalDataTable = ToDataTable(_orderRepository.GetSingleOrders(customerID));

                // Hide CustomerID column if it exists
                if (originalDataTable.Columns.Contains("CustomerID"))
                    originalDataTable.Columns["CustomerID"].ColumnMapping = MappingType.Hidden;

                // Hide CustomerName column if it exists
                if (originalDataTable.Columns.Contains("CustomerName"))
                    originalDataTable.Columns["CustomerName"].ColumnMapping = MappingType.Hidden;

                dgvCustomerOrder.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading OrderItem data: " + ex.Message);
            }
        }

        // To List Customer Order Details from the Datagridview function
        private DataTable ToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable();
            foreach (var prop in typeof(T).GetProperties())
            {
                DataColumn column;
                if (prop.Name == "Status")
                {
                    // Convert integer status to string representation
                    column = new DataColumn("Status", typeof(string));
                }
                else
                {
                    column = new DataColumn(prop.Name);
                }
                table.Columns.Add(column);
            }

            // Populate rows with data
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (prop.Name == "Status")
                    {
                        // Convert integer status to string representation
                        object value = prop.GetValue(item);
                        int status = (int)value;
                        string statusString = GetStatusString(status);
                        row["Status"] = statusString;
                    }
                    else
                    {
                        object value = prop.GetValue(item);
                        row[prop.Name] = value ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }

        private string GetStatusString(int status)
        {
            switch (status)
            {
                case 0:
                    return "Pending";
                case 1:
                    return "Success";
                case 2:
                    return "Cancel";
                default:
                    return "Unknown";
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            UpdateStatusForSelectedRows("Cancel");
        }

        private void UpdateStatusForSelectedRows(string newStatus)
        {
            foreach (DataGridViewRow selectedRow in dgvCustomerOrder.SelectedRows)
            {
                int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);
                try
                {
                    _orderRepository.UpdateStatus(orderId, newStatus);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred while updating status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit loop on error
                }
            }

            MessageBox.Show("Status Updated Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadOrdersData();
        }

        private void txtCustomerOrderSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtCustomerOrderSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("CustomerName LIKE '%{0}%' OR ProductName LIKE '%{0}%' OR Model LIKE '%{0}%' OR Brand LIKE '%{0}%' OR Status LIKE '%{0}%'", searchText);
                dgvCustomerOrder.DataSource = dv.ToTable();
            }
            else
            {
                dgvCustomerOrder.DataSource = originalDataTable;
            }
        }
    }
}