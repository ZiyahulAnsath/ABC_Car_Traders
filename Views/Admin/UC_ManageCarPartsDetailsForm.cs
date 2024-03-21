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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace ABC_Car_Traders.Views.Admin
{
    public partial class UC_ManageCarPartsDetailsForm : UserControl
    {
        private readonly CarPartRepository _carPartRepository;
        private DataTable originalDataTable;


        public UC_ManageCarPartsDetailsForm()
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
                originalDataTable = ToDataTable(_carPartRepository.GetCarParts());
                dgvCarPartsDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private void btnAddParts_Click(object sender, EventArgs e)
        {
            if (!Dashboard.Instance.PnlContainer.Controls.ContainsKey("UC_AddParts"))
            {
                UC_AddParts uc = new UC_AddParts();
                uc.Dock = DockStyle.Fill;
                Dashboard.Instance.PnlContainer.Controls.Add(uc);

            }
            Dashboard.Instance.PnlContainer.Controls["UC_AddParts"].BringToFront();
        }


        //To List Car parts Details from the Datagridview function
        private void dgvCarPartsDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvCarPartsDetails.Rows[e.RowIndex];
            CarPart updatedCarPart = new CarPart
            {
                PartID = Convert.ToInt32(row.Cells["PartID"].Value),
                Model = row.Cells["Model"].Value.ToString(),
                Brand = row.Cells["Brand"].Value.ToString(),
                PartName = row.Cells["PartName"].Value.ToString(),
                Description = row.Cells["Description"].Value.ToString(),
                BuyPrice = Convert.ToDecimal(row.Cells["BuyPrice"].Value),
                SellingPrice = Convert.ToDecimal(row.Cells["SellingPrice"].Value),
                Date = Convert.ToDateTime(row.Cells["Date"].Value),
                QuantityAvailable = Convert.ToInt32(row.Cells["QuantityAvailable"].Value)
            };

            // Check if the Image cell is empty
            if (row.Cells["Image"].Value != DBNull.Value)
            {
                updatedCarPart.Image = (byte[])row.Cells["Image"].Value;
            }
            _carPartRepository.UpdateCarPart(updatedCarPart);
            MessageBox.Show("Successfully Updated Car Part Details..!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //To List Car parts Details from the Datagridview and Delete function
        private void btnDeletePart_Click(object sender, EventArgs e)
        {
            if (dgvCarPartsDetails.SelectedRows.Count > 0)
            {
                int partID = Convert.ToInt32(dgvCarPartsDetails.SelectedRows[0].Cells["PartID"].Value);
                MessageBox.Show("Successfully Deleted Car Part Details from the databse in" +" "+ partID +" "+ "this id data..!");
                _carPartRepository.DeleteCarPart(partID);
                LoadCarPartsData(); 
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void txtSearchParts_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchParts.Text.Trim();

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


        //Print Methoed
        public void allVehicleParts()
        {
            if (dgvCarPartsDetails.Rows.Count == 0)
            {
                MessageBox.Show("No Record found", "Info");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF (*.pdf)|*.pdf";
            save.FileName = "TotalParts.pdf";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                    {
                        Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                        PdfWriter.GetInstance(document, fileStream);
                        document.Open();

                        PdfPTable pdfTable = new PdfPTable(dgvCarPartsDetails.Columns.Count);
                        pdfTable.DefaultCell.Padding = 2;
                        pdfTable.WidthPercentage = 100;
                        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                        iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                        foreach (DataGridViewColumn col in dgvCarPartsDetails.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, headerFont));
                            pdfTable.AddCell(cell);
                        }

                        iTextSharp.text.Font rowFont = FontFactory.GetFont("Monosarat", 10);

                        foreach (DataGridViewRow row in dgvCarPartsDetails.Rows)
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
        private void btnPrintAllParts_Click(object sender, EventArgs e)
        {
            allVehicleParts();
        }
    }
}
