using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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

namespace ABC_Car_Traders.Views.Admin
{
    public partial class UC_MainDashboard : UserControl
    {

        private readonly CarRepository _carRepository;
        private readonly CarPartRepository _carPartRepository;
        private readonly CustomerRepository _customerRepository;


        public UC_MainDashboard()
        {
            InitializeComponent();
            _carRepository = new CarRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            _carPartRepository = new CarPartRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            _customerRepository = new CustomerRepository();
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            // Update labels with total counts
            lblTotalCars.Text = _carRepository.GetTotalCarCount().ToString();
            lblTotalParts.Text = _carPartRepository.GetTotalCarPartCount().ToString();
            lblTotalCustomers.Text = _customerRepository.GetTotalCustomerCount().ToString();
        }
    }
}
