using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PVFSimple.项目Dnf;

namespace PVFSimple.通用窗口
{
    public partial class 批量载入 : Form
    {
     
        public 批量载入()
        {
            InitializeComponent();
        }

        private void DaoChu_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否真的要导出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            StringBuilder text = new StringBuilder();
            string AllText = richTextBox1.Text;
            if (AllText != "")
            {
                System.Collections.Generic.List<string> GetAllId=null;
                switch (DaoChu_button.Text)
                {
                    case "导出查询内容":
                        GetAllId = new System.Collections.Generic.List<string>(AllText.Split(new char[] {'\r','\n' },StringSplitOptions.RemoveEmptyEntries));
                        break;
                    default:
                        GetAllId = new Dnf().LstChongPaiLieGetId(AllText);
                        break;
                }
                
                if (GetAllId.Count > 0)
                {
                    GetAllId.ForEach((Text)=> text.Append("-" + Text));

                    text.Remove(0, 1);
                }
            }
            Form1.JieShouText = text.ToString();
            Close();
        }
    }
}
