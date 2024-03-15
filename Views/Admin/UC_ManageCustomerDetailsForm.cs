using System;
using System.Windows.Forms;
using ABC_Car_Traders.DataAccess;
using ABC_Car_Traders.Models;
using System.Data;
using System.Collections.Generic;

namespace ABC_Car_Traders.Views.Admin
{
    public partial class UC_ManageCustomerDetailsForm : UserControl
    {
        private readonly CustomerRepository _customerRepository;
        private DataTable originalDataTable;

        public UC_ManageCustomerDetailsForm()
        {
            InitializeComponent();
            _customerRepository = new CustomerRepository();
            originalDataTable = new DataTable();
            LoadCustomerData();
        }


        //Load All Customer Details
        private void LoadCustomerData()
        {
            try
            {
                originalDataTable = ToDataTable(_customerRepository.GetAllCustomers());
                dgvCustomerDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading customer data: " + ex.Message);
            }

        }

     
        private void dgvCustomerDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow editedRow = dgvCustomerDetails.Rows[e.RowIndex];
            Models.Customer updatedCustomer = new Models.Customer
            {
                CustomerID = Convert.ToInt32(editedRow.Cells["CustomerID"].Value),
                Name = Convert.ToString(editedRow.Cells["Name"].Value),
                Gender = Convert.ToString(editedRow.Cells["Gender"].Value),
                Username = Convert.ToString(editedRow.Cells["Username"].Value),
                Password = Convert.ToString(editedRow.Cells["Password"].Value),
                Email = Convert.ToString(editedRow.Cells["Email"].Value),
                Phone = Convert.ToString(editedRow.Cells["Phone"].Value),
                Address = Convert.ToString(editedRow.Cells["Address"].Value)
            };

            // Update the customer details in the database
            _customerRepository.UpdateCustomer(updatedCustomer);
            MessageBox.Show("Successfully Updated Customer Details..!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Delete customer function
        private void DeleteCustomer(int customerId)
        {
            try
            {
                _customerRepository.DeleteCustomer(customerId);
                LoadCustomerData(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting customer: " + ex.Message);
            }
        }

        //To List Car Details from the Datagridview and Delete function
        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomerDetails.SelectedRows.Count > 0)
            {
                int customerId = Convert.ToInt32(dgvCustomerDetails.SelectedRows[0].Cells["CustomerID"].Value);
                DeleteCustomer(customerId);
                MessageBox.Show("Successfully Deleted Customer from the databse in" + " " + customerId + " " + "this id data..!");
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.");
            }
        }


        //To List Car Details from the Datagridview function
        private DataTable ToDataTable(List<Models.Customer> customers)
        {
            DataTable table = new DataTable();
            foreach (var prop in typeof(Models.Customer).GetProperties())
            {
                table.Columns.Add(prop.Name);
            }

            foreach (Models.Customer item in customers)
            {
                DataRow row = table.NewRow();
                foreach (var prop in typeof(Models.Customer).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }


        // Filter the original DataTable based on the search text
        private void txtSearchCustomer_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchCustomer.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("Name LIKE '%{0}%' OR Gender LIKE '%{0}%' OR Username LIKE '%{0}%' OR Phone LIKE '%{0}%' OR Address LIKE '%{0}%'", searchText);
                dgvCustomerDetails.DataSource = dv.ToTable();
            }
            else
            {
                dgvCustomerDetails.DataSource = originalDataTable;
            }
        }


        
    }
}
