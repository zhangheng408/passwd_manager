using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace MM
{
    public partial class Form1 : Form
    {
        static MP m;
        ArrayList candidate;
        Boolean toClose;
        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;
            m = new MP();
            textBox3.Hide();
            toClose = false;

            this.Location = new Point(SystemInformation.WorkingArea.Width/4*3, SystemInformation.WorkingArea.Height / 5);
        }

        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void 隐藏主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toClose = true;
            Application.Exit();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 32 && e.KeyChar <= 126)
            {
                String input;
                if (!comboBox1.SelectedText.Equals(""))
                {
                    input = "" + e.KeyChar;
                    comboBox1.Text = "";
                }
                else
                {
                    input = comboBox1.Text + e.KeyChar;
                }
                comboBox1.Items.Clear();
                candidate = m.search(input);
                foreach (Object o in candidate)
                {
                    Password p= (Password)o;
                    comboBox1.Items.Add(p.getId() + "--" + p.getName());
                }
                comboBox1.DroppedDown = true;
                comboBox1.SelectionStart = comboBox1.Text.Length;
            }
            else if (e.KeyChar == 8)
            {
                String input = "";
                if (comboBox1.SelectedText.Equals(""))
                {
                    if (!comboBox1.Text.Equals(""))
                        input = comboBox1.Text.Substring(0, comboBox1.Text.Length - 1);
                }
                else
                {
                    input = comboBox1.Text.Substring(0, comboBox1.Text.Length - comboBox1.SelectedText.Length);
                }
                comboBox1.Items.Clear();
                candidate = m.search(input);
                foreach (Object o in candidate)
                {
                    Password p = (Password)o;
                    comboBox1.Items.Add(p.getId() + "--" + p.getName());
                }
                comboBox1.DroppedDown = true;
                comboBox1.SelectionStart = comboBox1.Text.Length;
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Password p = (Password)candidate[comboBox1.SelectedIndex];
            if (Key.getKey() == null)
            {
                MessageBox.Show("请先输入加密关键字");
                return;
            }
            //Console.WriteLine("c: " + BitConverter.ToString(Encoding.Unicode.GetBytes(p.getMPasswd())));
            //return;
            String s = p.getMPasswd();
            s = s.Replace("-", "");
            byte[] b = new byte[s.Length / 2];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
            }
            textBox1.Text = AES.DecryptStringFromBytes_Aes(b, Key.getKey(), Key.getIv());
                //textBox1.Text = AES.DecryptStringFromBytes_Aes(Encoding.Unicode.GetBytes(p.getMPasswd()), Key.getKey(), Key.getIv());
                textBox2.Text = p.getDescri();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPass a = new AddPass();
            a.Show();
            m = new MP();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Visible)
            {
                if (textBox3.Text.Length >= 8 && textBox3.Text.Length <= 16)
                {
                    Key.setkey(textBox3.Text);
                    textBox3.Text = "";
                }
                else
                {
                    MessageBox.Show("请保持关键字在8-16字符");
                    return;
                }
                textBox3.Hide();
                pictureBox2.Show();
            }
            else
            {
                textBox3.Show();
                textBox3.Focus();
                pictureBox2.Hide();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length >= 8 && textBox3.Text.Length <= 16)
            {
                Key.setkey(textBox3.Text);
                textBox3.Text = "";
            }
            else
            {
                MessageBox.Show("请保持关键字在8-16字符");
                return;
            }
        }

        public static void flashMP()
        {
            m = new MP();
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPass a = new AddPass();
            a.Show();
            m = new MP();
        }

        private void 密钥ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox3.Show();
            pictureBox2.Hide();
            pictureBox1.Image = Properties.Resources._19_2;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = !toClose;
            toClose = false;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox3.Text.Length >= 8 && textBox3.Text.Length <= 16)
                {
                    Key.setkey(textBox3.Text);
                    textBox3.Text = "";
                }
                else
                {
                    MessageBox.Show("请保持关键字在8-16字符");
                    return;
                }
                textBox3.Hide();
                pictureBox2.Show();
                
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            Key.setkey("12345678");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            Key.setkey("12345678");
        }

    }
}
