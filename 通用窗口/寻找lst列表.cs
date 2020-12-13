using System;
using System.Windows.Forms;

namespace PVFSimple.通用窗口
{
    public partial class 寻找lst列表 : Form
    {
        public 寻找lst列表()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string IdMoBan = "例如：                                                            \r\n2638038\t\r\n2638039\t\r\n2638040\t\r\n2638041\t\r\n2638042\t\r\n2638043\t\r\n2638044\t\r\n2638045\t\r\n2638046\t\r\n2638047\t\r\n2638048\t\r\n2638049\t\r\n2638050\t\r\n2638051\t\r\n2638052\t\r\n2638053\t\r\n2638054\t\r\n2638055\t\r\n\r\n或者以下这种也支持:\r\n2638047    `ani/ani/ani.ani`";
            MessageBox.Show(IdMoBan, "编号模板", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string PathMoBan = "例如：\r\nstackable/twdf/chn_2010_tiger_avatar_package_fighter_01.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_fighter_02.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_fighter_03.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_fighter_04.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_gunner_01.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_gunner_02.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_gunner_03.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_gunner_04.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_mage_01.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_mage_02.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_mage_03.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_mage_04.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_priest_01.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_priest_02.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_priest_03.stk\r\nstackable/twdf/chn_2010_tiger_avatar_package_priest_04.stk\r\n\r\n\r\n此两个斜杠都可行“/”“\\”，大小写不区分；";
            MessageBox.Show(PathMoBan, "路径模板", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            string PathMoBan = "例如：\r\n" +
                "10028\t`character/common/jacket/cloth/robe_wool.equ`\r\n" +
                "10029\t`character/common/jacket/cloth/robe_nhendon.equ`\r\n" +
                "10030\t`character/common/jacket/cloth/dress_eflame.equ`\r\n" +
                "10031\t`character/common/jacket/cloth/robe_hwool.equ`\r\n" +
                "10032\t`character/common/jacket/cloth/sht_sbskyarn.equ`\r\n" +
                "10033\t`character/common/jacket/cloth/sht_skyarn.equ`\r\n" +
                "10034\t`character/common/jacket/cloth/tunic_iseal.equ`\r\n" +
                "\r\n\r\n此两个斜杠都可行“/”“\\”，大小写不区分；";
            MessageBox.Show(PathMoBan, "lst列表模板", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保存", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)== DialogResult.Yes)
            {
                switch (Text)
                {
                    case "寻找LST":
                        switch (button3.Text)
                        {
                            case "保存lst列表内容":
                                Form1.Lst_FileLstText = richTextBox1.Text.Replace("\\", "/");
                                break;
                            case "保存被寻找内容":
                                Form1.Lst_BeiXunZhaoText = richTextBox1.Text.Replace("\\", "/").ToLower();
                                break;
                        }
                        break;
                    case "加入LST":
                        switch (button3.Text)
                        {
                            case "保存lst列表内容":
                                Form1.AddLst_FileLstText = richTextBox1.Text.Replace("\\", "/");
                                break;
                            case "保存待加入内容":
                                Form1.AddLst_BeiXunZhaoText = richTextBox1.Text.Replace("\\", "/");
                                break;
                        }
                        break;
                    case "Etc套装内容加入":
                        switch (button3.Text)
                        {
                            case "保存Etc列表内容":
                                Form1.EtcLstText = richTextBox1.Text.Replace("\\", "/");
                                break;
                            case "保存Etc待加入内容":
                                Form1.AddEtcLstText = richTextBox1.Text.Replace("\\", "/");
                                break;
                        }
                        break;
                }
                
                Close();
            }
            
        }

        
    }
}
