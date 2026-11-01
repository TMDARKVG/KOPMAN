using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Koperasi_barang.FrmLogin;

namespace Koperasi_barang
{
    public partial class FrmPenjualan : Form
    {
        KoperasiEntities db;
        List<int> idpro = new List<int>();
        public FrmPenjualan()
        {
            db = new KoperasiEntities();
            InitializeComponent();
        }
        private void clear()
        {
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = "0";
            numericUpDown1.Value = 0;
            textBox6.Text = "0";
        }

        private void FrmPenjualan_Load(object sender, EventArgs e)
        {
            var user = db.tblUsers.Where(a => a.id.Equals(idall)).FirstOrDefault();
            var cari = db.tblProduks.OrderBy(a => a.kode_barang).ToList();
            dataGridView2.DataSource = cari;
            textBox1.Text = user.fullname;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            var cari = db.tblProduks.Where(a => a.kode_barang.Equals(textBox2.Text)).FirstOrDefault();
            if (cari != null)
            {
                textBox3.Text = cari.nama_barang;
                textBox4.Text = cari.harga_satuan.ToString();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value >= 5)
            {
                decimal hrg = Convert.ToDecimal(textBox4.Text);
                decimal ax = Convert.ToDecimal(numericUpDown1.Value);
                decimal total = hrg * ax;
                decimal diskon = total * (10m / 100m);
                decimal hrgsetel = total - diskon;
                textBox6.Text = hrgsetel.ToString();
            }
            else
            {
                decimal hrg = Convert.ToDecimal(textBox4.Text);
                decimal ax = Convert.ToDecimal(numericUpDown1.Value);
                decimal total = hrg * ax;
                textBox6.Text = total.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {

            }
            else
            {
                dataGridView1.Rows.Add(textBox1.Text, textBox3.Text, textBox4.Text, numericUpDown1.Value, textBox6.Text);
                var ambil_aja = db.tblProduks.Where(a => a.kode_barang.Equals(textBox2.Text)).FirstOrDefault();
                if (textBox5.Text.Trim() != "")
                {
                    decimal get = Convert.ToDecimal(textBox6.Text);
                    decimal jumlah = get + Convert.ToDecimal(textBox5.Text);
                    textBox5.Refresh();
                    textBox5.Text = "";
                    textBox5.Text = jumlah.ToString();
                    int ambil = (int)numericUpDown1.Value;
                    int getj = Convert.ToInt16(textBox7.Text);
                    int tambahkan = ambil + getj;
                    textBox7.Refresh();
                    textBox7.Text = "";
                    textBox7.Text = tambahkan.ToString();
                    idpro.Add(ambil_aja.id);
                    //clear();
                }
                else
                {
                    idpro.Add(ambil_aja.id);
                    //clear();
                    textBox5.Text = textBox6.Text;
                    textBox7.Text = numericUpDown1.Value.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tblTransaksi trs = new tblTransaksi
            {
                id_user = idall,
                kuantitas = Convert.ToInt16(textBox7.Text),
                harga_total = Convert.ToDouble(textBox5.Text),
                tanggal_transaksi = DateTime.Now,
            };
            db.tblTransaksis.Add(trs);
            db.SaveChanges();
            int idgetbaru = trs.id;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                //string nama_pes = row.Cells["nama_pemesan"].Value.ToString();
                string nama_brg = row.Cells["nama_brg"].Value.ToString();
                double total = Convert.ToDouble(row.Cells["total"].Value.ToString());
                int jumlah = Convert.ToInt32(row.Cells["jumlah"].Value.ToString());
                if (idpro.Count < dataGridView1.Rows.Count)
                {
                    MessageBox.Show("ada data yang invalid");
                    return;
                }
                else
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        tblDetail_transaksi dtl = new tblDetail_transaksi
                        {
                            id_transaksi = idgetbaru,
                            id_produck = idpro[i],
                            nama_barang = nama_brg,
                            kuantitas = jumlah,
                            subtotal = total,
                        };
                        db.tblDetail_transaksi.Add(dtl);
                    }
                    db.SaveChanges();
                    dataGridView1.Rows.Clear();
                }
                MessageBox.Show("berhail");
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
