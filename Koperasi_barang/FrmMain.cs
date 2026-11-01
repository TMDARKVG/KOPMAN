using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Koperasi_barang.FrmLogin;

namespace Koperasi_barang
{
    public partial class FrmMain : Form
    {
        KoperasiEntities db;
        public FrmMain()
        {
            db = new KoperasiEntities();
            InitializeComponent();
        }
        private void panel(Form form) 
        {
            panel1.Controls.Clear();
            form.TopLevel = false;
            panel1.Controls.Add(form);
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel(new FrmPenjualan());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel(new FrmProduk());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel(new FrmReport());
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            var load = db.tblUsers.Where(a=>a.id.Equals(idall)).FirstOrDefault();
            label1.Text = load.fullname;
        }
    }
}
