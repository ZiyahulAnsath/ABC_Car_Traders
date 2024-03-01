using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Car_Traders.Views.Admin
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            UC_MainDashboard uc = new UC_MainDashboard();
            addUserControl(uc);
        }

        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            plDashboard.Controls.Clear();
            plDashboard.Controls.Add(userControl);
            userControl.BringToFront();
        }


   

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            UC_MainDashboard uc = new UC_MainDashboard();
            addUserControl(uc);
        }


        private void btnManageAllReports_Click(object sender, EventArgs e)
        {
            UC_GenerateReportsForm uc = new UC_GenerateReportsForm();
            addUserControl(uc);
        }

        private void btnManageVehicles_Click(object sender, EventArgs e)
        {
            UC_ManageCarDetailsForm uc = new UC_ManageCarDetailsForm();
            addUserControl(uc);
        }

        private void btnManageParts_Click(object sender, EventArgs e)
        {
            UC_ManageCarPartsDetailsForm uc = new UC_ManageCarPartsDetailsForm();
            addUserControl(uc);
        }

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            UC_ManageCustomerDetailsForm uc = new UC_ManageCustomerDetailsForm();
            addUserControl(uc);
        }

        private void btnManageAllOrders_Click(object sender, EventArgs e)
        {
            UC_ManageCustomerOrderDetailsForm uc = new UC_ManageCustomerOrderDetailsForm();
            addUserControl(uc);
        }

        private void btnAdminLogout_Click(object sender, EventArgs e)
        {
            //oginForm redirectForm = new oginForm();
            //this.Hide();
            //redirectForm.Show();
            
        }
    }
}
