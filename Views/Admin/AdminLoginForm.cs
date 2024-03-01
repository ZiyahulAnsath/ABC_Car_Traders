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
using ABC_Car_Traders.Views.Customer;

namespace ABC_Car_Traders.Views.Admin
{
    public partial class AdminLoginForm : Form
    {
        public AdminLoginForm()
        {
            InitializeComponent();
        }

        private void btnSignupPage_Click(object sender, EventArgs e)
        {
            CustomerRegisterForm register = new CustomerRegisterForm();
            register.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Dashboard adminDash = new Dashboard();
            adminDash.Show();
            this.Hide();
        }
    }
}
