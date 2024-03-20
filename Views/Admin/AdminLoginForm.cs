using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_Car_Traders.Views.Customer;
using ABC_Car_Traders.Controllers;
using ABC_Car_Traders.DataAccess;

namespace ABC_Car_Traders.Views.Admin
{
    public partial class AdminLoginForm : Form
    {
        private AdminController adminController;
        private CustomerController customerController;

        public AdminLoginForm()
        {
            InitializeComponent();
            adminController = new AdminController();
            customerController = new CustomerController();
        }

        private void btnSignupPage_Click(object sender, EventArgs e)
        {
            CustomerRegisterForm register = new CustomerRegisterForm();
            register.Show();
            this.Hide();
        }

        private void clearData()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";

            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;


            if (rbAdmin.Checked)
            {
                // Check credentials in admin table
                if (adminController.ValidateAdminLogin(username, password))
                {
                    Dashboard adminDashboard = new Dashboard();
                    adminDashboard.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password for admin.");
                    clearData();
                }
            }
            else if (rbCustomer.Checked)
            {
                // Check credentials in customer table
                if (customerController.ValidateCustomerLogin(username, password))
                {

                    CustomerDashboard customerDashboard = new CustomerDashboard();
                    customerDashboard.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password for customer.");
                    clearData();
                }
            }
            else
            {
                MessageBox.Show("Please select user type (admin/customer).");
            }

            //}
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}