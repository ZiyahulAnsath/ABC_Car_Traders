﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_Car_Traders.Views.Admin;
using ABC_Car_Traders.DataAccess;
using ABC_Car_Traders.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ABC_Car_Traders.Views.Admin
{
    public partial class UC_ManageCarDetailsForm : UserControl
    {
        private readonly CarRepository _carRepository;
        private DataTable originalDataTable;

        public UC_ManageCarDetailsForm()
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
                originalDataTable = ToDataTable(_carRepository.GetCars());
                dgvCarDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            
        }


       
        private void btnAddCar_Click(object sender, EventArgs e)
        {
            if (!Dashboard.Instance.PnlContainer.Controls.ContainsKey("UC_AddCar"))
            {
                UC_AddCar uc = new UC_AddCar();
                uc.Dock = DockStyle.Fill;
                Dashboard.Instance.PnlContainer.Controls.Add(uc);

            }
            Dashboard.Instance.PnlContainer.Controls["UC_AddCar"].BringToFront();
        }


        //To List Car Details from the Datagridview function
        private void dgvCarDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvCarDetails.Rows[e.RowIndex];
            Car updatedCar = new Car
            {
                CarID = Convert.ToInt32(row.Cells["CarID"].Value),
                CarName = row.Cells["CarName"].Value.ToString(),
                Model = row.Cells["Model"].Value.ToString(),
                Brand = row.Cells["Brand"].Value.ToString(),
                FualType = row.Cells["FualType"].Value.ToString(),
                Description = row.Cells["Description"].Value.ToString(),
                Year = Convert.ToInt32(row.Cells["Year"].Value),
                BuyPrice = Convert.ToDecimal(row.Cells["BuyPrice"].Value),
                SellingPrice = Convert.ToDecimal(row.Cells["SellingPrice"].Value),
                Date = Convert.ToDateTime(row.Cells["Date"].Value),
                QuantityAvailable = Convert.ToInt32(row.Cells["QuantityAvailable"].Value)
            };

            if (row.Cells["Image"].Value != DBNull.Value)
            {
                updatedCar.Image = (byte[])row.Cells["Image"].Value;
            }

            _carRepository.UpdateCar(updatedCar);
            MessageBox.Show("Successfully Updated Car Details..!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //To List Car Details from the Datagridview and Delete function
        private void deleteSelectedVehicle_Click(object sender, EventArgs e)
        {
            if (dgvCarDetails.SelectedRows.Count > 0)
            {
                int carID = Convert.ToInt32(dgvCarDetails.SelectedRows[0].Cells["CarID"].Value);
                _carRepository.DeleteCar(carID);
                MessageBox.Show("Successfully Deleted Car Details from the databse in"+carID+"this id data..!");
                LoadCarData(); 
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //Search function
        private void txtSearchCar_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchCar.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                // Filter the original DataTable based on the search text
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("CarName LIKE '%{0}%' OR Model LIKE '%{0}%' OR Brand LIKE '%{0}%' OR FualType LIKE '%{0}%' OR Description LIKE '%{0}%'", searchText);
                dgvCarDetails.DataSource = dv.ToTable();
            }
            else
            {
                dgvCarDetails.DataSource = originalDataTable;
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


        public void allVehicles()
        {
            if (dgvCarDetails.Rows.Count == 0)
            {
                MessageBox.Show("No Record found", "Info");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF (*.pdf)|*.pdf";
            save.FileName = "TotalVehicles.pdf";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                    {
                        Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                        PdfWriter.GetInstance(document, fileStream);
                        document.Open();

                        PdfPTable pdfTable = new PdfPTable(dgvCarDetails.Columns.Count);
                        pdfTable.DefaultCell.Padding = 2;
                        pdfTable.WidthPercentage = 100;
                        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                        iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                        foreach (DataGridViewColumn col in dgvCarDetails.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, headerFont));
                            pdfTable.AddCell(cell);
                        }

                        iTextSharp.text.Font rowFont = FontFactory.GetFont("Monosarat", 10);

                        foreach (DataGridViewRow row in dgvCarDetails.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.Value != null)
                                {
                                    pdfTable.AddCell(new Phrase(cell.Value.ToString(), rowFont));
                                }
                                else
                                {
                                    pdfTable.AddCell(new Phrase("", rowFont));
                                }
                            }
                        }

                        document.Add(pdfTable);
                        document.Close();
                    }

                    MessageBox.Show("Data exported Successfully", "Info");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while Exporting Data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //Print All Cars
        private void btnPrintAllCars_Click(object sender, EventArgs e)
        {
            allVehicles(); 
        }

        private void btnReportCar_Click(object sender, EventArgs e)
        {
            ReportViewer report = new ReportViewer();
            report.Show();
        }
    }
}
