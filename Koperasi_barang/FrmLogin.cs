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
    public partial class FrmLogin : Form
    {
        public static int idall;
        KoperasiEntities db;
        public FrmLogin()
        {
            db = new KoperasiEntities();
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var login = db.tblUsers.Where(a => a.username.Equals(textBox1.Text)).FirstOrDefault();
            if (login != null && login.password.Equals(textBox2.Text))
            {
                if (login.role == true)
                {
                    FrmMain main = new FrmMain();
                    MessageBox.Show("selamat ada berhail login");
                    idall = login.id;
                    Hide();
                    main.ShowDialog();
                    Close();
                }
                else 
                {
                    FrmMain main = new FrmMain();
                    MessageBox.Show("selamat ada berhail login");
                    idall = login.id;
                    main.ShowDialog();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("login gagal data tidak ada");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmRegister register = new FrmRegister();
            register.ShowDialog();
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            return;
        }
    }
}
