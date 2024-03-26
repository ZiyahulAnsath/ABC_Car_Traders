﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABC_Car_Traders.DataAccess;
using ABC_Car_Traders.Models;
using System.Windows.Forms;

namespace ABC_Car_Traders.Views.Customer
{
    public partial class UC_EditProfile : UserControl
    {
        private CustomerRepository customerRepository;
        private int customerId;


        public UC_EditProfile()
        {
            InitializeComponent();
            customerRepository = new CustomerRepository();
            this.customerId = customerId;
            PopulateUserProfile();
        }

        private void PopulateUserProfile()
        {
        //    //Fetch user details from the database using customer ID
        //    Customer customer = customerRepository.GetCustomerNameById(customerId);

        //    // Populate form fields with fetched user details
        //    txtCustomerName.Text = customer.Name;
        //    // Populate other fields similarly
        //    txtCustomerAddress.Text = customer.Address;
        //    txtCustomerMobileNo.Text = customer.Phone;
        //    txtCustomerUsername.Text = customer.Username;
        ////Note: Password is typically not retrieved for security reasons
        }


        private void ClearData()
        {
            txtCustomerAddress.Text = "";
            txtCustomerMobileNo.Text = "";
            txtCustomerName.Text = "";
            txtCustomerPassword.Text = "";
            txtCustomerUsername.Text = "";
            foreach (Control control in this.Controls)
            {
                if (control is RadioButton radioButton)
                {
                    radioButton.Checked = false;
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {

        }
    }
}
