using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace MM
{
    public partial class MM : Form
    {
        public Passwds pds;
        public char pChar='\0';
        public bool zClose = true;
        bool zcClose = false;
        public static AES Aes;
        public string skin="skin.ssk";
        public string mmFile = "MM.rmx";
        public MM()
        {
            InitializeComponent();
            this.TopMost = true;
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            textBox3.Focus();
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width * 3 / 4, Screen.PrimaryScreen.WorkingArea.Height / 10);
            load_set();
            Thread t = new Thread(new ThreadStart(init));
            t.Start();
            
        }

        private void load_set()
        {
            try
            {
                StreamReader sr = new StreamReader("set.dat");
                string l=sr.ReadLine();
                if (l != null && l.Length > 0)
                {
                    pChar = l.ToCharArray()[0];
                }
                l = sr.ReadLine();
                if (l != null && l.ToLower().Equals("false"))
                {
                    zClose = false;
                }
                l = sr.ReadLine();
                if (l != null && l.Length > 0)
                {
                    skin = l;
                }
                l = sr.ReadLine();
                if (l != null && l.Length > 0)
                {
                    mmFile = l;
                }
                sr.Close();
            }
            catch
            {

            }
            setPchar();
            setSkin();
        }

        public void sync_set()
        {
            StreamWriter sw = new StreamWriter(new FileStream("set.dat", FileMode.Create));
            sw.WriteLine("" + pChar);
            sw.WriteLine("" + zClose);
            sw.WriteLine(skin);
            sw.WriteLine(mmFile);
            sw.Close();
        }

        private void init()
        {
            textBox3.PasswordChar = pChar;
            toolTip1.SetToolTip(pictureBox1, "输入管理密钥");
            toolTip1.SetToolTip(pictureBox2, "删除账号");
            toolTip1.SetToolTip(pictureBox3, "修改账号");
            toolTip1.SetToolTip(pictureBox4, "添加账号");
            toolTip1.SetToolTip(pictureBox5, "复制账号");
            toolTip1.SetToolTip(pictureBox6, "复制密码");
            Aes = new AES("~");
            pds = new Passwds(mmFile, this);
        }

        //开始输入密钥
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            textBox3.Show();
            textBox3.Focus();
            comboBox1.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            comboBox1.Enabled = false;
            np = null;
        }
        //完成密钥输入
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (!textBox3.Text.Equals(""))
                {
                    //设置密钥
                    Aes.setKey(textBox3.Text);
                    Thread t = new Thread(new ThreadStart(pds.chKey));
                    t.Start();
                    
                    comboBox1.Enabled = true;
                }
                textBox3.Text = "";
                textBox3.Hide();
                pictureBox2.Show();
                pictureBox3.Show();
                pictureBox4.Show();
                comboBox1.Focus();
            }
        }
        //最小化时隐藏
        private void MM_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }
        //关闭主界面
        private void MM_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!zClose&&!zcClose)
            {
                this.Hide();
                e.Cancel = true;
                zcClose = false;
            }
        }
        //清空所有设置
        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            //设置key
            Aes.setKey("~");
        }
        //正常退出
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zcClose = true;
            Application.Exit();
        }
        //点击托盘图标
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        //设置
        private void SettoolStripMenuItem_Click(object sender, EventArgs e)
        {
            MMSet ms = new MMSet(this);
            ms.Owner = this;
            ms.Show();
        }
        //下拉列表候选账号
        private List<Password> cans;    
        //下拉列表实时选择
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
                cans = pds.search(input);
                foreach (Password p in cans)
                {
                    comboBox1.Items.Add(p.id + "--" + p.name);
                }
                /*
                candidate = m.search(input);
                foreach (Object o in candidate)
                {
                    Password p = (Password)o;
                    comboBox1.Items.Add(p.getId() + "--" + p.getName());
                }
                 * */
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
                /*
                candidate = m.search(input);
                foreach (Object o in candidate)
                {
                    Password p = (Password)o;
                    comboBox1.Items.Add(p.getId() + "--" + p.getName());
                }
                 * */
                cans = pds.search(input);
                foreach (Password p in cans)
                {
                    comboBox1.Items.Add(p.id + "--" + p.name);
                }
                comboBox1.DroppedDown = true;
                comboBox1.SelectionStart = comboBox1.Text.Length;
            }
        }
        //当前选中账号
        Password np = null;
        //选中一个账号
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            np = cans[comboBox1.SelectedIndex];
            textBox1.Text = np.Passwd;
            textBox2.Text = np.descri;
        }
        //复制账号
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (np == null)
            {
                return;
            }

            Clipboard.Clear();
            Clipboard.SetDataObject(np.id);
        }
        //复制密码
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (np == null)
            {
                return;
            }

            Clipboard.Clear();
            Clipboard.SetDataObject(np.Passwd);
        }
        //添加账号
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            MMAdd ma = new MMAdd(this);
            ma.Owner = this;
            ma.Show();
        }
        //修改账号
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (np == null)
            {
                return;
            }
            MMAdd ma = new MMAdd(np, this);
            ma.Owner = this;
            ma.Show();

            ma.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MMAdd_FormClosed);
        }
        //修改账号后，同步主界面
        private void MMAdd_FormClosed(object sender, FormClosedEventArgs e)
        {
            comboBox1.Text = np.id + "--" + np.name;
            textBox1.Text = np.Passwd;
            textBox2.Text = np.descri;
        }
        //删除账号
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (np == null)
            {
                return;
            }
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("是否删除此账号", "删除账号", messButton);
            if (dr != DialogResult.OK)
            {
                return;
            }
            pds.remove(np);
            np = null;
            comboBox1.Text = "";
            comboBox1.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            pds.sync();
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            if (np == null)
            {
                return;
            }
            pictureBox5.Image = Properties.Resources.smallCopy;
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            if (np == null)
            {
                return;
            }
            Thread.Sleep(300);
            pictureBox5.Image = Properties.Resources.copy;
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            if (np == null)
            {
                return;
            }
            pictureBox6.Image = Properties.Resources.smallCopy;
        }

        private void pictureBox6_MouseUp(object sender, MouseEventArgs e)
        {
            if (np == null)
            {
                return;
            }
            Thread.Sleep(300);
            pictureBox6.Image = Properties.Resources.copy;
        }


        public void setPchar()
        {
            textBox3.PasswordChar = pChar;
        }

        public void setSkin()
        {
            skinEngine1.SkinFile = skin;
            skinEngine1.SkinAllForm = true;
        }
        
    }
}
