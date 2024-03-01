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

namespace ABC_Car_Traders.Views.Customer
{
    public partial class CustomerRegisterForm : Form
    {
        public CustomerRegisterForm()
        {
            InitializeComponent();
        }

        private void btnLoginPage_Click(object sender, EventArgs e)
        {
            AdminLoginForm roleLogin = new AdminLoginForm();
            roleLogin.Show();
            this.Hide();
        }

        private void btnRegisterCustomer_Click(object sender, EventArgs e)
        {
            CustomerDashboard CustomerDash = new CustomerDashboard();
            CustomerDash.Show();
            this.Hide();
        }
    }
}
