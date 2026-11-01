using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Koperasi_barang
{
    public partial class FrmProduk : Form
    {
        KoperasiEntities db;
        int idu;
        public FrmProduk()
        {
            db = new KoperasiEntities();
            InitializeComponent();
        }
        private void load()
        {
            var load = db.tblProduks.OrderBy(a => a.kode_barang).Select(a => new { a.kode_barang, a.nama_barang, a.stok, a.harga_satuan, a.kategori }).ToList();
            dataGridView1.DataSource = load;
        }
        private void clear()
        {
            textBox2.Text = string.Empty;
            numericUpDown1.Value = 0;
            textBox3.Text = "0";
            textBox1.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cari = db.tblProduks.Where(a => a.kode_barang.Equals(textBox1.Text)).FirstOrDefault();
            if (cari == null)
            {
                tblProduk pro = new tblProduk
                {
                    kode_barang = textBox1.Text,
                    nama_barang = textBox2.Text,
                    stok = (int)numericUpDown1.Value,
                    harga_satuan = Convert.ToDouble(textBox3.Text),
                    kategori = comboBox1.Text,
                };
                db.tblProduks.Add(pro);
                db.SaveChanges();
                MessageBox.Show("data berhail di masukan");
                load();
                clear();
            }
            else
            {
                MessageBox.Show("kode ada yang sama coba di bedakan");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lastCode = db.tblProduks.Where(a => a.kode_barang.StartsWith("BRG")).OrderByDescending(a => a.kode_barang)
            .Select(a => a.kode_barang).FirstOrDefault();
            string numericPart = new string(lastCode.Where(char.IsDigit).ToArray());
            int nextNumber = int.Parse(numericPart) + 1;
            textBox1.Text = $"BRG00{nextNumber.ToString()}";
            textBox1.ReadOnly = true;
        }

        private void FrmProduk_Load(object sender, EventArgs e)
        {
            load();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Trim() == "")
            {
                load();
            }
            else
            {
                var cari = db.tblProduks.Where(a => a.kode_barang.Contains(textBox4.Text)).ToList();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells["kode_barang"].Value.ToString();
                var cari = db.tblProduks.Where(a => a.kode_barang == id).FirstOrDefault();
                if (cari != null)
                {
                    textBox1.Text = cari.kode_barang;
                    textBox2.Text = cari.nama_barang;
                    numericUpDown1.Value = (int)cari.stok;
                    textBox3.Text = cari.harga_satuan.ToString();
                    comboBox1.Text = cari.kategori;
                    textBox1.ReadOnly = true;
                    idu = cari.id;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var cari = db.tblProduks.Where(a => a.id.Equals(idu)).FirstOrDefault();
            if (cari != null)
            {
                cari.nama_barang = textBox2.Text;
                cari.stok = (int)numericUpDown1.Value;
                cari.harga_satuan = Convert.ToDouble(textBox3.Text);
                cari.kategori = comboBox1.Text;
                db.SaveChanges();
                MessageBox.Show("data berhail di update");
                load();
                clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (idu == 0)
            {
                MessageBox.Show("coba pilih terlebih dahulu data nya");
            }
            else
            {
                var cari = db.tblProduks.Where(a => a.id.Equals(idu)).FirstOrDefault();
                if (cari != null)
                {
                    db.tblProduks.Remove(cari);
                    db.SaveChanges();
                    MessageBox.Show("data sudah di hapus");
                    load();
                    clear();
                }
            }
        }
    }
}
