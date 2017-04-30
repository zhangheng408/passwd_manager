using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MM
{
    public partial class MMAdd : Form
    {
        private MM mm;
        public MMAdd(MM mm)
        {
            InitializeComponent();
            this.mm = mm;
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width * 3 / 4 + 55, Screen.PrimaryScreen.WorkingArea.Height /10 + 125);
            textBox3.PasswordChar = mm.pChar;
        }
        Password np;
        public MMAdd(Password p, MM mm)
        {
            np = p;
            InitializeComponent();
            this.mm = mm;
            this.Text = "修改账号";
            textBox3.PasswordChar = mm.pChar;
            textBox1.Text = p.name;
            textBox2.Text = p.id;
            textBox3.Text = p.Passwd;
            textBox4.Text = p.descri;
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width * 3 / 4 + 55, Screen.PrimaryScreen.WorkingArea.Height / 10 + 125);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (np != null)//修改
            {
                np.reset(textBox2.Text, textBox1.Text, textBox3.Text, textBox4.Text);
            }
            else//添加
            {
                Password p = new Password(MM.Aes.Encrypt(textBox1.Text), MM.Aes.Encrypt(textBox2.Text), MM.Aes.Encrypt(textBox3.Text), MM.Aes.Encrypt(textBox4.Text));
                p.Decrypt();
                mm.pds.add(p);
            }
            mm.pds.sync();
            this.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox3.PasswordChar = mm.pChar;
        }
    }
}
