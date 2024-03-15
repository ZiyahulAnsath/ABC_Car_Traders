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
using ABC_Car_Traders.Models;

namespace ABC_Car_Traders.Views.Customer
{
    public partial class CustomerRegisterForm : Form
    {
        private readonly CustomerController _customerController;


        public CustomerRegisterForm()
        {
            InitializeComponent();
            _customerController = new CustomerController();
        }

        //private void ClearData()
        //{
        //    txtPhoneNo.Text = "";
        //    txtAddress.Text = "";
        //    txtPassword.Text = "";
        //    txtUsername.Text = "";
        //    //rbFemale.sele = -1;
        //    txtName.Text = "";
        //    foreach (Control control in this.Controls)
        //    {
        //        if (control is RadioButton radioButton)
        //        {
        //            radioButton.Checked = false;
        //        }
        //    }          
        //}

        private void btnLoginPage_Click(object sender, EventArgs e)
        {
            AdminLoginForm roleLogin = new AdminLoginForm();
            roleLogin.Show();
            this.Hide();
        }

        private void btnRegisterCustomer_Click(object sender, EventArgs e)
        {
            string gender = rbMale.Checked ? "Male" : "Female";
            string hashedPassword = PasswordHelper.HashPassword(txtPassword.Text);

            var customer = new Models.Customer
            {
                Name = txtName.Text,
                Gender = gender,
                Username = txtUsername.Text,
                Password = hashedPassword,
                Email = txtAddress.Text, 
                Address = txtAddress.Text, 
                Phone = txtPhoneNo.Text,
                //Address = txtAddress.Text
            };
            _customerController.RegisterCustomer(customer);

            mbRegisterCustomer.Show();
            this.Close();
            AdminLoginForm login = new AdminLoginForm();
            login.Show();
        }
    }
}
