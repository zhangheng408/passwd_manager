using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MM
{
    public partial class AddPass : Form
    {
        public AddPass()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals("")
                || textBox3.Text.Equals("") || textBox4.Text.Equals(""))
            {
                MessageBox.Show("信息不完整");
                return;
            }
            if (Key.getKey() == null)
            {
                MessageBox.Show("请先输入加密关键字");
                this.Dispose();
                return;
            }
            byte[] t = AES.EncryptStringToBytes_Aes(textBox3.Text, Key.getKey(), Key.getIv());
            MP.addPass(textBox1.Text + "###" + textBox2.Text + "###"  + BitConverter.ToString(t) + "###" + textBox4.Text+"###");
            Console.WriteLine("b: " + BitConverter.ToString(t));
            Form1.flashMP();
            MessageBox.Show("添加成功");
            this.Close();
        }
    }
}
