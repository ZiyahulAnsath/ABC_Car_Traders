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
    public partial class UC_SearchCarPartsDetailsForm : UserControl
    {
        private readonly CarPartRepository _carPartRepository;
        private DataTable originalDataTable;


        public UC_SearchCarPartsDetailsForm()
        {
            InitializeComponent();
            _carPartRepository = new CarPartRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            originalDataTable = new DataTable();
            LoadCarPartsData();
        }

        //Load All Car Details
        public void LoadCarPartsData()
        {
            try
            {
                originalDataTable = ToDataTable(_carPartRepository.GetCustomerCarParts());
                dgvCarPartsDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private void txtSearchCarPart_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchCarPart.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                // Filter the original DataTable based on the search text
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("Model LIKE '%{0}%' OR Brand LIKE '%{0}%' OR PartName LIKE '%{0}%' OR Description LIKE '%{0}%'", searchText);
                dgvCarPartsDetails.DataSource = dv.ToTable();
            }
            else
            {
                dgvCarPartsDetails.DataSource = originalDataTable;
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

       
    }
}
