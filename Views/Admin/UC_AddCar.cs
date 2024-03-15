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
    public partial class UC_AddCar : UserControl
    {

        private readonly CarRepository _carRepository;


        public UC_AddCar()
        {
            InitializeComponent();
            _carRepository = new CarRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
        }

        //Clear Function
        private void ClearData()
        {
            txtBuyPrice.Text = "";
            txtSellingPrice.Text = "";
            txtVehicleName.Text = "";
            rtxtDescription.Text = "";
            cmbBrand.SelectedIndex = -1;
            cmbColor.SelectedIndex = -1;
            cmbFualType.SelectedIndex = -1;
            cmbModel.SelectedIndex = -1;
            carDateTimePicker.Value = carDateTimePicker.MinDate;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }


        //Add Car Function
        private void btnSave_Click(object sender, EventArgs e)
        {
            Car newCar = new Car
            {
                CarName = txtVehicleName.Text,
                Model = cmbModel.SelectedItem.ToString(),
                Brand = cmbBrand.SelectedItem.ToString(),
                FualType = cmbFualType.SelectedItem.ToString(),
                Description = rtxtDescription.Text,
                //Year = Convert.ToInt32(txtYear.Text),
                BuyPrice = Convert.ToDecimal(txtBuyPrice.Text),
                SellingPrice = Convert.ToDecimal(txtSellingPrice.Text),
                Date = DateTime.Parse(carDateTimePicker.Text),
                //Image = // Convert your image to byte array,
               //QuantityAvailable = Convert.ToInt32(txtQuantityAvailable.Text)
            };

            _carRepository.AddCar(newCar);
            MessageBox.Show("New Vehicle successfully Added!");
            this.Hide();
            UC_ManageCarDetailsForm carDetails = new UC_ManageCarDetailsForm();
            carDetails.Show();
           

        }
    }
}
