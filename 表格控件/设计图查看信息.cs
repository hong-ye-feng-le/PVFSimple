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
    public partial class 设计图查看信息 : Form
    {
        public 设计图查看信息()
        {
            InitializeComponent();
        }

        private void 设计图查看信息_Load(object sender, EventArgs e)
        {
            switch (Text)
            {
                case "设计图查看信息(如要使用默认内容，请复制“”中间的内容)":
                    new Grid_SheJiTu().设计图查看信息初始化(reoGridControl1);
                    break;
                case "任务查看信息(如要使用默认内容，请复制“”中间的内容)":
                    new Grid_RenWu().任务查看信息初始化(reoGridControl1);
                    break;
            }

        }
    }
}
