using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Car_Traders.Views.Customer
{
    public partial class CustomerDashboard : Form
    {
        public CustomerDashboard()
        {
            InitializeComponent();
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

        }
    }
}
