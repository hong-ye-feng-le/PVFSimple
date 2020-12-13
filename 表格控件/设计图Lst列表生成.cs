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
    public partial class Lst列表生成 : Form
    {
        public Lst列表生成()
        {
            InitializeComponent();
        }

        private void 设计图Lst列表生成_Load(object sender, EventArgs e)
        {
            int RowCount = 0;
            List<string> Obj = new List<string>();
            switch (Text)
            {
                case "设计图Lst列表生成":
                    RowCount = Form1.SheJiTuLstInfo.Count;
                    Obj = Form1.SheJiTuLstInfo;
                    break;
                case "任务Lst列表生成":
                    RowCount = Form1.RenWuLstInfo.Count;
                    Obj = Form1.RenWuLstInfo;
                    break;
            }
            if (RowCount <= 0)
                return;


            //设置lst表格行数
            reoGridControl1.CurrentWorksheet.RowCount = RowCount;
            //设置lst表格列数
            reoGridControl1.CurrentWorksheet.ColumnCount = 2;
            //设置lst表格列宽
            reoGridControl1.CurrentWorksheet.SetColumnsWidth(0, 1, 100);
            reoGridControl1.CurrentWorksheet.SetColumnsWidth(1, 1, 800);
            //把lst信息加入的表格中
            for (int i = 0; i < RowCount; i++)
            {
                reoGridControl1.CurrentWorksheet[$"A{i + 1}"] = Obj[i].Remove(Obj[i].LastIndexOf("."));
                reoGridControl1.CurrentWorksheet[$"B{i + 1}"] = $"`{Obj[i]}`";
            }
        }

        private void LST加入button_Click(object sender, EventArgs e)
        {
            int RowCount = 0;
            List<string> Obj = new List<string>();
            switch (Text)
            {
                case "设计图Lst列表生成":
                    RowCount = Form1.SheJiTuLstInfo.Count;
                    Obj = Form1.SheJiTuLstInfo;
                    break;
                case "任务Lst列表生成":
                    RowCount = Form1.RenWuLstInfo.Count;
                    Obj = Form1.RenWuLstInfo;
                    break;
            }
            if (RowCount <= 0)
                return;


            string AddText = textBox1.Text;
            for (int i = 0; i < reoGridControl1.CurrentWorksheet.RowCount; i++)
            {
                reoGridControl1.CurrentWorksheet[i, 1] = $"`{AddText}{Obj[i]}`";
            }


            //气泡提示，由定时任务终止
            toolTip = new ToolTip();
            string ToolText = "已完成加入路径信息";
            toolTip.IsBalloon = true;
            toolTip.SetToolTip(LST加入button, ToolText);
            toolTip.Show(Text, LST加入button, 55, 15);
            if (Tooltimer.Enabled == false)
            {
                Tooltimer.Interval = 1500;
                Tooltimer.Start();
            }
                


        }

        static ToolTip toolTip;

        private void Tooltimer_Tick(object sender, EventArgs e)
        {
            if (toolTip == null)
                return;
            toolTip.Dispose();
            Tooltimer.Stop();
        }
    }
}
