using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MM
{
    public partial class MMSet : Form
    {
        private MM mm;
        public MMSet(MM mm)
        {
            InitializeComponent();
            this.mm = mm;
            this.TopMost = true;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width * 3 / 4 + 10, Screen.PrimaryScreen.WorkingArea.Height / 10 + 10);

            textBox1.Text = mm.pds.file;

            load(mm);
        }

        private void load(MM mm)
        {
            if (mm.pChar == '\0')
            {
                comboBox1.Text = "无";
            }
            else
            {
                comboBox1.Text = "" + mm.pChar;
            }

            if (mm.zClose)
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        //选中密码文件
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图片文件(*.rmx)|*.rmx";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                mm.mmFile = ofd.FileName;
                textBox1.Text = ofd.FileName;
                mm.pds.load(ofd.FileName);
                Thread t = new Thread(new ThreadStart(mm.pds.chKey));
                t.Start();
            }
        }
        //改变关闭模式
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            mm.zClose = radioButton2.Checked;
           
        }
        //改变密码回显字符
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("无"))
            {
                mm.pChar = '\0';
            }
            else
            {
                mm.pChar = comboBox1.Text.ToCharArray()[0];
            }

            mm.setPchar();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
            {
                MessageBox.Show("新密钥为空");
                return;
            }
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("是否使用新的管理密钥", "应用新密钥", messButton);
            if (dr != DialogResult.OK)
            {
                return;
            }
            MM.Aes.setKey(textBox2.Text);
            //MM.pds.setNewKey();
            Thread t = new Thread(new ThreadStart(mm.pds.setNewKey));
            t.Start();
        }

        private void MMSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            mm.sync_set();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "皮肤文件(*.ssk)|*.ssk";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                mm.skin = ofd.FileName;
                mm.setSkin();
            }
        }
    }
}
