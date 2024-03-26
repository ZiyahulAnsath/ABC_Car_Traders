using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABC_orderItem_Traders.DataAccess;
using ABC_Car_Traders.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ABC_Car_Traders.Views.Admin
{
    public partial class UC_ManageCustomerOrderDetailsForm : UserControl
    {

        private readonly OrderRepository _orderRepository;
        private DataTable originalDataTable;


        public UC_ManageCustomerOrderDetailsForm()
        {
            InitializeComponent();
            _orderRepository = new OrderRepository("Data Source=LAPTOP-KGH138OG;Initial Catalog=abc_car_traders;Integrated Security=True");
            originalDataTable = new DataTable();
            LoadOrdersData();
        }


        //Load All Order Details
        private void LoadOrdersData()
        {
            try
            {
                originalDataTable = ToDataTable(_orderRepository.GetAllOrders());
                dgvOrderDetails.DataSource = originalDataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading OrderItem data: " + ex.Message);
            }
        }

      
        private void txtSearchOrders_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearchOrders.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                DataView dv = originalDataTable.DefaultView;
                dv.RowFilter = string.Format("CustomerName LIKE '%{0}%' OR ProductName LIKE '%{0}%' OR Model LIKE '%{0}%' OR Brand LIKE '%{0}%' OR Status LIKE '%{0}%'", searchText);
                dgvOrderDetails.DataSource = dv.ToTable();
            }
            else
            {
                dgvOrderDetails.DataSource = originalDataTable;
            }
        }

        //To List Customer Order Details from the Datagridview function
        private DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(typeof(T)))
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(typeof(T)))
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }


        //To update Customer Order Details from the Datagridview function
        private void dgvOrderDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure valid row index
            {
                DataGridViewRow editedRow = dgvOrderDetails.Rows[e.RowIndex];

                // Retrieve updated data from the DataGridView
                int orderId = Convert.ToInt32(editedRow.Cells["OrderID"].Value);
                int customerId = Convert.ToInt32(editedRow.Cells["CustomerID"].Value);
                string customerName = Convert.ToString(editedRow.Cells["CustomerName"].Value);
                int productId = Convert.ToInt32(editedRow.Cells["ProductID"].Value);
                string productName = Convert.ToString(editedRow.Cells["ProductName"].Value);
                string model = Convert.ToString(editedRow.Cells["Model"].Value);
                string brand = Convert.ToString(editedRow.Cells["Brand"].Value);
                decimal price = Convert.ToDecimal(editedRow.Cells["Price"].Value);
                DateTime orderDate = Convert.ToDateTime(editedRow.Cells["OrderDate"].Value);
                int status = Convert.ToInt32(editedRow.Cells["Status"].Value);

                // Create an instance of OrderItem with the updated data
                OrderItem updatedOrder = new OrderItem
                {
                    OrderID = orderId,
                    CustomerID = customerId,
                    CustomerName = customerName,
                    ProductID = productId,
                    ProductName = productName,
                    Model = model,
                    Brand = brand,
                    Price = price,
                    OrderDate = orderDate,
                    Status = status
                };

                try
                {
                    _orderRepository.UpdateOrder(updatedOrder);
                    MessageBox.Show("Successfully Updated Order Details", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteCustomerOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrderDetails.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvOrderDetails.SelectedRows[0];
                int orderId = Convert.ToInt32(selectedRow.Cells["OrderID"].Value);
                try
                {
                    _orderRepository.DeleteOrder(orderId);
                    MessageBox.Show("Order Deleted Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrdersData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //This events selected row exact status value and to show the understanable status
        private void dgvOrderDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvOrderDetails.Columns["Status"].Index && e.RowIndex >= 0)
            {
                object value = e.Value;
                if (value != DBNull.Value)
                {
                    int statusValue = Convert.ToInt32(value);
                    string statusString = GetStatusString(statusValue);
                    e.Value = statusString;
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = string.Empty;
                    e.FormattingApplied = true;
                }
            }
        }

        //To pass the exact Status
        private string GetStatusString(int statusValue)
        {
            switch (statusValue)
            {
                case 0:
                    return "Pending";
                case 1:
                    return "Success";
                case 2:
                    return "Cancel";
                default:
                    return "Unknown";
            }
        }


        //Print All the Value in PDF file format
        public void allCustomerOrders()
        {
            if (dgvOrderDetails.Rows.Count == 0)
            {
                MessageBox.Show("No Record found", "Info");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF (*.pdf)|*.pdf";
            save.FileName = "CustomerTotalOrders.pdf";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                    {
                        Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                        PdfWriter.GetInstance(document, fileStream);
                        document.Open();

                        PdfPTable pdfTable = new PdfPTable(dgvOrderDetails.Columns.Count);
                        pdfTable.DefaultCell.Padding = 2;
                        pdfTable.WidthPercentage = 100;
                        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                        iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                        foreach (DataGridViewColumn col in dgvOrderDetails.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, headerFont));
                            pdfTable.AddCell(cell);
                        }

                        iTextSharp.text.Font rowFont = FontFactory.GetFont("Monosarat", 10);

                        foreach (DataGridViewRow row in dgvOrderDetails.Rows)
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
        private void btnPrintAllOrders_Click(object sender, EventArgs e)
        {
            allCustomerOrders();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {

        }
    }
}