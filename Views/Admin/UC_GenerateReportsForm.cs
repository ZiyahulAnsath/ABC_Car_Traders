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
    public partial class UC_GenerateReportsForm : UserControl
    {
        // Instantiate the source form
        private UC_ManageCarDetailsForm sourceFormCar;
        private UC_ManageCarPartsDetailsForm sourceFormCarParts;
        private UC_ManageCustomerDetailsForm sourceFormCustomer;
        private UC_ManageCustomerOrderDetailsForm sourceFormCustomerOrder;


        public UC_GenerateReportsForm()
        {
            InitializeComponent();
            sourceFormCar = new UC_ManageCarDetailsForm();
            sourceFormCustomerOrder = new UC_ManageCustomerOrderDetailsForm();
            sourceFormCustomer = new UC_ManageCustomerDetailsForm();
            sourceFormCarParts = new UC_ManageCarPartsDetailsForm();
        }

        private void btnPrintAllVehicles_Click(object sender, EventArgs e)
        {
            sourceFormCar.allVehicles();
        }

        private void btnPrintAllParts_Click(object sender, EventArgs e)
        {
            sourceFormCarParts.allVehicleParts();
        }

        private void btnPrintAllCustomers_Click(object sender, EventArgs e)
        {
            sourceFormCustomer.allCustomers();
        }

        private void btnPrintCustomerAllOrders_Click(object sender, EventArgs e)
        {
            sourceFormCustomerOrder.allCustomerOrders();
        }
    }
}
