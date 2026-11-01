using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Koperasi_barang
{
    public partial class FrmRegister : Form
    {
        KoperasiEntities db;
        public FrmRegister()
        {
            db = new KoperasiEntities();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length >= 8 && textBox2.Text.Length >= 8)
            {
                var chcek = db.tblUsers.Where(a => a.username.Equals(textBox1.Text)).FirstOrDefault();
                if (chcek == null)
                {
                    tblUser user = new tblUser
                    {
                        username = textBox1.Text,
                        password = textBox3.Text,
                        fullname = textBox4.Text,
                        number_phone = textBox5.Text,
                        date_of_birth = dateTimePicker1.Value,
                        role = false,
                    };
                    db.tblUsers.Add(user);
                    db.SaveChanges();
                    MessageBox.Show("user berhail terdaftar");
                }
                else
                {
                    MessageBox.Show("Username telah ada coba berikan user name yang lain");
                }
            }
            else 
            {
                MessageBox.Show("password harus lebih dari 8 charter");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLogin login = new FrmLogin();
            Close();
            login.ShowDialog();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox2.Checked ? '\0' : '*';
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
    }
}
