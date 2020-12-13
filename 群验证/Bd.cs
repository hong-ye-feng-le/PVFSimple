using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVFSimple.群验证
{
    public partial class Bd : Form
    {
        public Bd()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (maskedTextBox1.Text == "")
            {
                MessageBox.Show("请输入QQ号后进行验证登陆!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }


            string groupid = "759213683";//得到群号
            string QqId = maskedTextBox1.Text;//得到QQ
            Form1.MysqlYz.qq = QqId;
            Form1.MysqlYz.md5 = new Verification().GetMD5String();//获取机器码

            if (Form1.MysqlYz.ChaXun())//数据库中查询成功
            {
                Form1.MysqlYz.ChaR();
                MessageBox.Show("尊敬的大佬，请尽情使用强大的功能吧~", "验证成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Form1.MysqlYz.Yse();//恢复窗体正常
                Close();//关闭子窗口
            }
            else
            {
                Form1.MysqlYz.ChaRError();
                MessageBox.Show("抱歉您未授权此工具", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                new Verification().End();
                Close();
            }


        }

        private void Bd_Shown(object sender, EventArgs e)//第一次加载窗口时
        {
            if(File.Exists(Directory.GetCurrentDirectory() + "\\TextQQ.ini"))
            {
                maskedTextBox1.Text = File.ReadAllText(Directory.GetCurrentDirectory() + "\\TextQQ.ini");
            }
        }

        private void Bd_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\TextQQ.ini", maskedTextBox1.Text);
        }
    }
}
