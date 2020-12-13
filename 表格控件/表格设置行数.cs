using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVFSimple.表格控件
{
    public partial class 表格设置行数 : Form
    {
        public 表格设置行数()
        {
            InitializeComponent();
        }

        public int YesNo=0;

        private void button1_Click(object sender, EventArgs e)
        {
            if(maskedTextBox1.Text=="")
            {
                MessageBox.Show("请正确输入需要设置的行数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (Convert.ToInt32(maskedTextBox1.Text) < 1)
            {
                MessageBox.Show("请正确大于或等于1的行数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            YesNo = Convert.ToInt32(maskedTextBox1.Text);
            Close();

        }
    }
}
