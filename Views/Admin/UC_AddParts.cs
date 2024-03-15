using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_Car_Traders.DataAccess;
using ABC_Car_Traders.Models;

namespace ABC_Car_Traders.Views.Admin
{
    public partial class UC_AddParts : UserControl
    {

        private readonly CarPartRepository _carPartRepository;

        public UC_AddParts()
        {
            InitializeComponent();
            _carPartRepository = new CarPartRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
        }

        //Clear Function
        private void ClearData()
        {
            txtPartsName.Text = "";
            txtBuyPrice.Text = "";
            txtSellingPrice.Text = "";
            rtxtDescription.Text = "";
            cmbBrand.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
            partsDateTimePiccker.Value = partsDateTimePiccker.MinDate;
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        //Add Car part Function
        private void btnSaveParts_Click(object sender, EventArgs e)
        {
            CarPart newCarPart = new CarPart
            {
                Model = cmbModel.SelectedItem.ToString(),
                Brand = cmbBrand.SelectedItem.ToString(),
                PartName = txtPartsName.Text,
                Description = rtxtDescription.Text,
                BuyPrice = Convert.ToDecimal(txtBuyPrice.Text),
                SellingPrice = Convert.ToDecimal(txtSellingPrice.Text),
                Date = DateTime.Parse(partsDateTimePiccker.Text),
                //QuantityAvailable = Convert.ToInt32(txtQuantityAvailable.Text),
                //Image = // Convert your image to byte array
            };

            _carPartRepository.AddCarPart(newCarPart);
            MessageBox.Show("New Parts successfully Added!");
            this.Hide();
            UC_ManageCarPartsDetailsForm carPartDetails = new UC_ManageCarPartsDetailsForm();
            carPartDetails.Show();
        }
    }
}
