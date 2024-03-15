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

namespace ABC_Car_Traders.Views.Customer
{
    public partial class UC_SearchCarDetailsForm : UserControl
    {

        private readonly CarRepository _carRepository;
        private DataTable originalDataTable;

        public UC_SearchCarDetailsForm()
        {
            InitializeComponent();
            _carRepository = new CarRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            originalDataTable = new DataTable();
            LoadCarData();

        }


        //Load All Car Details
        private void LoadCarData()
        {
            try
            {
                originalDataTable = ToDataTable(_carRepository.GetCarsCustomer());
                dgvCarDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }


        private DataTable ToDataTable<T>(IList<T> data)
        {
            DataTable table = new DataTable();
            foreach (var prop in typeof(T).GetProperties())
            {
                table.Columns.Add(prop.Name);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (var prop in typeof(T).GetProperties())
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }


        //Search car Details in to the Customer
        private void txtSearchCar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchCar.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("CarName LIKE '%{0}%' OR Model LIKE '%{0}%' OR Brand LIKE '%{0}%' OR FualType LIKE '%{0}%' OR Description LIKE '%{0}%'", searchText);
                dgvCarDetails.DataSource = dv.ToTable();
            }
            else
            {
                dgvCarDetails.DataSource = originalDataTable;
            }
        }
    }



}
