using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_Car_Traders.Views.Admin;
using ABC_Car_Traders.Controllers;

namespace ABC_Car_Traders.Views.Customer
{
    public partial class CustomerDashboard : Form
    {
       
        public CustomerDashboard()
        {
            InitializeComponent();


            //Stored Session manager
            //if (SessionManager.LoggedInCustomerId != 0)
            //{
            //    label2.Text = "Welcome, ID: " + SessionManager.LoggedInCustomerId;
            //}
            if (SessionManager.LoggedInCustomerName != null)
            {
                lblHeading.Text = "Welcome To ABC Car Traders: " + SessionManager.LoggedInCustomerName;
            }

            UC_CustomerDashboard uc = new UC_CustomerDashboard();
            addUserControl(uc);
        }
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            plCustomerDashboard.Controls.Clear();
            plCustomerDashboard.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            UC_CustomerDashboard uc = new UC_CustomerDashboard();
            addUserControl(uc);
        }

        private void btnSearchCar_Click(object sender, EventArgs e)
        {
            UC_SearchCarDetailsForm uc = new UC_SearchCarDetailsForm();
            addUserControl(uc);
        }

        private void btnSearchPart_Click(object sender, EventArgs e)
        {
            UC_SearchCarPartsDetailsForm uc = new UC_SearchCarPartsDetailsForm();
            addUserControl(uc);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            UC_OrderType uc = new UC_OrderType();
            addUserControl(uc);
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            UC_EditProfile uc = new UC_EditProfile();
            addUserControl(uc);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session
            SessionManager.LoggedInCustomerName = null;

            this.Hide();
            AdminLoginForm login = new AdminLoginForm();
            login.Show();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
