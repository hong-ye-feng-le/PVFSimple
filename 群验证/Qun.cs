using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVFSimple.群验证
{
    public partial class Qun : Form
    {
        public Qun()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //窗口出现时设置web链接为群空间
            webBrowser1.Navigate("http://xui.ptlogin2.qq.com/cgi-bin/xlogin?appid=549000912&daid=5&style=40&s_url=http://qun.qzone.qq.com/group");
            //webBrowser1.Navigate("https://qun.qq.com/member.html#gid=759213683");

        }

        //设置QQ群号码后会进入此处并且会验证群号码以及QQ号
        private void WebBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if(webBrowser1 != null)
                if(webBrowser1.Url!=null)
                if (webBrowser1.Url.ToString().StartsWith("http://qun.qzone.qq.com"))
                {
                    webBrowser1.Visible = false;
                    HtmlElement groupList = webBrowser1.Document.GetElementById("my_group_list_container");
                    if (groupList == null)
                    {
                        return;
                    }
                    webBrowser1.ProgressChanged -= WebBrowser1_ProgressChanged;
                    string groupListText = webBrowser1.DocumentText;//获取所有的网站文本

                    Regex_new regex = new Regex_new();
                    regex.创建(groupListText, @" uin:(\d+)';", 1);
                    if(regex.取匹配数量()>0)
                    {
                        string groupid = "759213683";//得到群号
                        string QqId = regex.取子匹配文本(1, 1);//得到QQ
                        Form1.MysqlYz.qq = QqId;
                        Form1.MysqlYz.md5 = new Verification().GetMD5String();//获取机器码
                        //验证QQ群号码以及QQ号码
                        if (groupListText.Contains($"data-groupid=\"{ groupid}\"><"))
                        {
                            if (Form1.MysqlYz.ChaXun())//数据库中查询成功
                            {
                                Form1.MysqlYz.ChaR();
                                MessageBox.Show("尊敬的大佬，请尽情使用强大的功能吧~", "验证成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                Form1.MysqlYz.Yse();//恢复窗体正常
                                Close();//关闭子窗口
                            }
                            else
                            {
                                new Verification().End();
                                Close();
                            }
                        }
                        else
                        {
                            CuoWu();
                        }

                    }
                    else
                    {
                        CuoWu();
                    }

                }
        }

        private void CuoWu()
        {
            webBrowser1.Dispose();
            Form1.MysqlYz.ChaRError();
            MessageBox.Show("抱歉您未授权此工具", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

    }
}

