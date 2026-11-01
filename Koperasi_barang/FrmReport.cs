using iText.Kernel.Pdf;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using System.Windows.Forms.DataVisualization.Charting;

namespace Koperasi_barang
{
    public partial class FrmReport : Form
    {
        KoperasiEntities db;
        public FrmReport()
        {
            db = new KoperasiEntities();
            InitializeComponent();
        }
        private void LoadReport(DateTime? selectedDate = null)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add(new ChartArea("MainArea"));

           
            var query = from pro in db.tblProduks
                        join pur in db.tblDetail_transaksi on pro.id equals pur.id_produck
                        join sale in db.tblTransaksis on pur.id_transaksi equals sale.id  
                        where !selectedDate.HasValue || sale.tanggal_transaksi == selectedDate.Value.Date
                        select new
                        {
                            nama = pro.nama_barang,
                            amout = pur.kuantitas,
                            tanggal = sale.tanggal_transaksi  
                        };

            var report = query.GroupBy(a => a.nama)
                              .Select(k => new { pro = k.Key , pur = k.Sum(x => x.amout) })
                              .ToList();

            // Populate chart
            //var series = new Series("Total Amount by Product")
            //{
            //    ChartType = SeriesChartType.Column
            //};

            //foreach (var item in report)
            //{
            //    series.Points.AddXY(item.pro, item.pur);
            //}

            //chart1.Series.Add(series);

            // Populate DataGridView
            dataGridView1.DataSource = report;
        }

        private void GeneratePdfReport()
        {
            try
            {
                // Pilih lokasi penyimpanan file PDF
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    Title = "Save PDF Report"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                // Membuat PDF
                using (PdfWriter writer = new PdfWriter(saveFileDialog.FileName))
                using (PdfDocument pdfDoc = new PdfDocument(writer))
                {
                    Document document = new Document(pdfDoc);
                    PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    // Tambahkan Header Laporan
                    document.Add(new Paragraph("Laporan Harian").SetFontSize(16).SetFont(boldFont));
                    document.Add(new Paragraph($"Tanggal: {DateTime.Now:dd/MM/yyyy}\nLoket Kasir: 1\n\n").SetFontSize(12));

                    // Tabel Data
                    Table table = new Table(dataGridView1.Columns.Count).UseAllAvailableWidth();

                    // Header Tabel
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        table.AddHeaderCell(new Cell().Add(new Paragraph(column.HeaderText).SetFont(boldFont)));
                    }

                    // Baris Data
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(cell.Value?.ToString() ?? "")));
                        }
                    }

                    document.Add(table);

                    // Footer Total
                    document.Add(new Paragraph("\n"));
                    //document.Add(new Paragraph($"Sub Total: {labelSubTotal.Text}\nTotal Diskon: {labelDiskon.Text}\nTotal Bayar: {labelTotalBayar.Text}\n\n")
                    //    .SetFontSize(12));

                    // Tambahkan Ruang untuk Tanda Tangan
                    document.Add(new Paragraph("\n\n"));
                    document.Add(new Paragraph("Tanda Tangan Pemilik Toko:\n\n__________________________")
                        .SetFontSize(12));

                    document.Close();
                }

                MessageBox.Show("Laporan berhasil diekspor ke PDF!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GeneratePdfReport();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadReport(dateTimePicker1.Value);
        }

        private void FrmReport_Load(object sender, EventArgs e)
        {
            //LoadReport(DateTime.Now);
        }
    }
}
