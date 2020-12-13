using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;//文件操作
using System.Text.RegularExpressions;//正则表达式
using Microsoft.WindowsAPICodePack.Dialogs;//浏览文件夹窗口
using ADOX;//access数据库创建
using System.Threading.Tasks;//多线程
using System.Drawing;
using System.Threading;
using unvell.ReoGrid;
using Microsoft.VisualBasic;//转繁体
using MySql.Data;
using MySql.Data.MySqlClient;
using PVFSimple.表格控件;
using PVFSimple.数据库验证;
using PVFSimple.通用窗口;
using PVFSimple.项目Dnf;






/* 基本知识
 * 注释快捷键：
 * ctrl+k+c 加入选中注释
 * ctrl+k+u 取消选中注释
 * ctrl+k+d 修正格式
 * ctrl+m+o 缩起方法
 * 
 * 
 * —————————————————————
 * 运算符 + - * /
 * 对比 > < == >= <= !=
 * 逻辑对比 &&(且) ||(或) !(与或非)
 * 赋值 =
 * 
 * 注：
 * ==也可以写成.Equals()
 * 
 * count++（这里是count=count+1）
 * 
 * —————————————————————
 * 变量类型
 * int（整数型）
 * char（字符）
 * string（字符串类型）
 * double（双精度小数型）
 * bool（逻辑型）false：假  true：真
 * —————————————————————
 * 常用备注：
 * 
 * 变量拼接>>>>>>>>>>>>>>>>>>>>>
 * string a = "无";
 * string b = "视";
 * string 变量 = $"第{a}我{b}";
 * >>>>>>>>>>>>>>>>>>>>>>>>>>>>>
 * 
 * 数组
 * string[] a = new string[数];创建了多少个数组空间
 * string[] a = new string[数]{1,2};创建的数组并且连续赋值
 * string[] a = {1,2};简要写法
 * 
 */

/*逻辑判断
 * >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
 * 如果、否则：
 * if(A=b)
 * {
 * }else if(a>b)
 * {
 * }
 * 
 * 范围选取如果否则;代表意义：给予一个变量如果是1-3范围那么输出；适合小范围判定
 * switch(a)
 * {
 * 
 * case 1:
 * case 2:
 * case 3:
 * 输出：“1111”
 * break;
 * default:
 * 反之
 * break;
 * }
 * >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
 * 
 * 
 * **/

/*循环
 * 
 * 判断循环
 * 
 * while(真)不执行只对比为真就进入循环
 * {
 * }
 * break;（跳出循环）
 * continue;到循环尾
 * 
 * do（无条件先执行代码，之后再进行判断是否继续）
 * {
 * 
 * }
 * while（真）;
 * 
 * 括号内设置变量 循环完后立即释放
 * for(int a =1; a <= 10; a++)
 *{
 * }
 * 
 * 
 * a.length;(取数组成员数)
 * 
 * **/

/*类；例如易语言的子程序
 * 
 * public class 子程序名（代表全局可调用）
 * public srting a;（为参数外部可调用）静态特征
 * public void a;（为动态特征可以理解为易语言子程序进行的操作）
 * 
 *  
 * 外部调用
 * a 变量名 = new a();（实例化对象 a 类型）
 * a.name = "输入";赋值
 * 
 * **/

/*常用命令
 * 
 * 制表符 换行符 + “\t\r\n”
 * 
 * 
 * 
 * Directory.GetCurrentDirectory();(获取应用程序所在目录)
 * 
 * Convert.To 为类型转换  int整数 或 string文本
 * 
 * >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
 * 记录运行时间
 *System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
 *time.Start();
 *time.stop();
 *time.ElapsedMilliseconds();
 * 
 *
 * >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 
 * 
 */

/*文件枚举Directory操作
 * 
 *  转为大写
 *  文本.ToUpper();
 *  转为小写
 *  文本.ToLower();
 *  txt.Replace("1","2");替换所有文本
 *  txt.IndexOf("1");寻找文本返回位置
 *  txt.Contains("2");寻找文本返回真假
 *  txt.StartsWith("#pv");寻找文本开头返回真假
 *  txt.EndsWith("12");寻找文本结尾返回真假
 * 
 *  取父路径
 *  Directory.GetParent(@"C:\Users\MSI-NB\Desktop\普雷");
 *  
 *  创建目录、目录不存在并创建子目录
 *  Directory.CreateDirectory(@"C:\Users\MSI-NB\Desktop\普雷");
 *  
 *  删除整个目录
 *  Directory.Delete(@"C:\Users\MSI-NB\Desktop\普雷 - 副本", true);
 *  
 *  删除空目录
 *  Directory.Delete(@"C:\Users\MSI-NB\Desktop\普雷 - 副本");
 *  
 *  复制文件到新目录，同名并且覆盖
 *  File.Copy(@"C:\Users\MSI-NB\Desktop\普雷\查找出来的lst列表.txt", @"C:\Users\MSI-NB\Desktop\查找出来的lst列表.txt", true);
 *  
 *  返回扩展名
 *  Path.GetExtension(@"C:\Users\MSI-NB\Desktop\普雷\img路径.txt");
 *  返回文件名+扩展名
 *  Path.GetFileName(@"C:\Users\MSI-NB\Desktop\普雷\img路径.txt");
 *  
 *  读取文件内的所有文本
 *  File.ReadAllText(@"C:\Users\MSI-NB\Desktop\普雷\img路径.txt");
 *  把内容写到文件；存在就覆盖
 *  File.WriteAllText(@"C:\Users\MSI-NB\Desktop\普雷\img路径.txt",内容);
 *  
 *  打开文件夹：
 *  System.Diagnostics.Process.Start("文件夹的路径");
 *  打开文件夹中某个文件：
 *  System.Diagnostics.Process.Start(“文件夹路径”+"/"+"文件名称");
 *  打开文件夹并选中单个文件：
 *  System.Diagnostics.Process.Start("Explorer", "/select,"+ “文件夹路径"+"\"+"要选中的文件名称");
 *  或
 *  System.Diagnostics.Process.Start("Explorer.exe", "/select,"+ 文件夹路径+"\"+要选中的文件名称);
 *  
 *  
 *  
 *    //获取程序运行目录
 * //richTextBox1.AppendText(Directory.GetCurrentDirectory());
 *
 * //string[] 全路径 = directory.getfiles(@"c:\users\msi-nb\desktop\普雷");
 *
 * //for (int i = 0; i < 全路径.length; i++)
 * //{
 * //    richtextbox1.appendtext(全路径[i] +"\r\n");
 * //}
 *
 *
 * //获取目录里面的所有目录
 * //List<string> dirs = new List<string>(Directory.EnumerateDirectories(@"c:\users\msi-nb\desktop\普雷"));
 *
 * //foreach (var dir in dirs)
 * //{
 * //    richTextBox1.AppendText(dir.ToString() + "\r\n");
 *
 * //}
 *
 *
 * //正规枚举文件全路径以及文件名、目录
 * //List<string> dirs = new List<string>(Directory.EnumerateFiles(@"C:\Users\MSI-NB\Desktop\普雷\map\gcontents\190307_preyrai\dungeo*\animation\1phaze_buff_dungeon", "*.*", SearchOption.AllDirectories));
 * //foreach (var dir in dirs)
 * //{
 * //    richTextBox1.AppendText(dir.ToString() + "\r\n"); 全路径
 * //    richTextBox1.AppendText(dir.ToLower() + "\r\n"); 全路径转为小写
 * //    richTextBox1.AppendText(dir.Substring(dir.LastIndexOf("\\") + 1) + "\r\n"); 文件名
 * //    richTextBox1.AppendText(dir.Replace(dir.Substring(dir.LastIndexOf("\\")), "") + "\r\n"); 路径
 * //    dirs.Count()数量
 * //}
 *             List<string> dirs = new List<string>(Directory.EnumerateFiles(@"C:\Users\MSI-NB\Desktop\普雷", "*.*", SearchOption.AllDirectories));
 *          for (int i = 0; i < dirs.Count(); i++)
 *          {
 *              richTextBox1.AppendText(dirs[i] + "\r\n");
 *              
 *          }
 * **/

/*提示框
 * 
MessageBox.Show("消息内容", "返回值 确定1",MessageBoxButtons.OK,MessageBoxIcon.Question);
 
MessageBox.Show("消息内容",, "返回值 确定1 取消2",MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
 
MessageBox.Show("消息内容", "返回值 终止3 重试4 忽略5",MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
 
MessageBox.Show("消息内容", "返回值 是6 否7 取消2",MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
 
MessageBox.Show("消息内容", "返回值 是6 否7",MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
 
MessageBox.Show("消息内容", "返回值 重试4 取消2",MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);

//图片样式
MessageBoxIcon.Question
 
MessageBoxIcon.Asterisk
 
MessageBoxIcon.Information
 
MessageBoxIcon.Error
 
MessageBoxIcon.Stop
 
MessageBoxIcon.Hand
 
MessageBoxIcon.Exclamation
 
MessageBoxIcon.Warning
 
MessageBoxIcon.None

 * **/

/*简繁体
 * using Microsoft.VisualBasic;
 * string   s   =   "繁体"; 
 *      s   =   Strings.StrConv(s,   VbStrConv.Wide,   0);   //   半角转全角 
 *      s   =   Strings.StrConv(s,   VbStrConv.TraditionalChinese,   0);   //   简体转繁体 
 *      s   =   Strings.StrConv(s,   VbStrConv.ProperCase ,   0);   //   首字母大写 
 *      s   =   Strings.StrConv(s,   VbStrConv.Narrow ,   0);   //   全角转半角 
 *      s   =   Strings.StrConv(s,   VbStrConv.SimplifiedChinese,   0);   //   繁体转简体
 * 
 * **/

/*定位文件
 *  //定位并打开目录
 * System.Diagnostics.Process.Start("explorer.exe",movepath );
 *  //定位并选中文件
 * System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
 * **/

/*剪辑板
 * 取剪辑板文本
 * string str = Clipboard.GetText();
 * 置剪辑板文本
 * Clipboard.SetDataObject(str);
 * 
 * **/

/* 自动收缩
 * #region 开始
 * 代码
 * #endregion
 * **/

/* 集合中所有元素出现次数
 * 
 * foreach (var item in listt.GroupBy(s => s))
 * {
 * Console.WriteLine("{0}:{1}次", item.Key, item.Count());
 * }
 * **/

/* NuGet 安装包
 * 
 * 生成exe时合并DLL第三方插件
 * Costura.Fody
 * 
 * API窗口可浏览文件也可浏览文件夹
 * Microsoft.WindowsAPlCodePack-Core
 * Microsoft.WindowsAPlCodePack -Shell
 * 
 * MySql数据库连接
 * MySql.Data
 * 
 * 表格插件最好用的免费插件
 * unvell.ReoGrid.dll
 * 
 * 轻量级最好用的sqlite数据库
 * System.Data.SQLite
 * 
 * 
 * **/

/* 延时
 * //CheckForIllegalCrossThreadCalls = false;
 * Thread.Sleep(1000);
 * **/

/* 引用资源中的文件
 * Properties.Resources.(文件名)就可以取到
 * **/

namespace PVFSimple
{

    public partial class Form1 : Form
    {

        public static Form1 form1;
        public static ReoGridControl reoGridContro_Newl;
        public static ReoGridControl reoGridContro_New2;
        public static ReoGridControl reoGridContro_New3;

        public static MysqlYz MysqlYz = new MysqlYz();

        //选择文件夹的目录
        public static string ApiDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面

        //窗口批量载入编号以及全路径
        public static 批量载入 IDpath = new 批量载入();
        public static string JieShouText;
        //窗口导出礼包查询代码
        public static string JieShouStkIdText = "";
        public static string JieShouEquIdText = "";
        //窗口寻找lst列表
        public static 导出礼包查询代码 StkLiBaoInfoForm;
        //窗口寻找lst列表
        public static string Lst_FileLstText = "";
        public static string Lst_BeiXunZhaoText = "";
        public static StringBuilder Lst_EndYesText = new StringBuilder();
        public static StringBuilder Lst_EndNoText = new StringBuilder();
        //窗口加入lst列表
        public static string AddLst_FileLstText = "";
        public static string AddLst_BeiXunZhaoText = "";
        public static StringBuilder AddLst_EndYesText = new StringBuilder();
        //窗口Etc套装加入
        public static string EtcLstText = "";
        public static string AddEtcLstText = "";
        public static StringBuilder EndEtcLstText = new StringBuilder();


        //窗口设计图lst列表
        public static List<string> SheJiTuLstInfo = new List<string>();
        //窗口任务lst列表
        public static List<string> RenWuLstInfo = new List<string>();

        public Form1()
        {
            InitializeComponent();
            form1 = this;
            reoGridContro_Newl = reoGridControl1;
            reoGridContro_New2 = reoGridControl2;
            reoGridContro_New3 = reoGridControl3;
        }


        //程序即将关闭前的操作
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StringBuilder TextInfo = new StringBuilder();
            TextInfo.Append(副本编号.Text + "\r\n");
            TextInfo.Append(PVF文件所在目录.Text + "\r\n");
            TextInfo.Append(提取并导出的目录.Text + "\r\n");
            TextInfo.Append(城镇编号txt.Text + "\r\n");
            TextInfo.Append(城镇pvf文件txt.Text + "\r\n");
            TextInfo.Append(城镇导出目录txt.Text + "\r\n");
            TextInfo.Append(怪物编号text.Text + "\r\n");
            TextInfo.Append(怪物pvf文件目录text.Text + "\r\n");
            TextInfo.Append(怪物导出目录text.Text + "\r\n");
            TextInfo.Append(特效编号text.Text + "\r\n");
            TextInfo.Append(特效pvf文件目录text.Text + "\r\n");
            TextInfo.Append(特效导出目录text.Text + "\r\n");
            TextInfo.Append(APC编号text.Text + "\r\n");
            TextInfo.Append(APCpvf文件目录text.Text + "\r\n");
            TextInfo.Append(APC导出目录text.Text + "\r\n");
            TextInfo.Append(ANI全路径文件text.Text + "\r\n");
            TextInfo.Append(ANIpvf文件目录text.Text + "\r\n");
            TextInfo.Append(ANI导出目录text.Text + "\r\n");
            TextInfo.Append(装备编号text.Text + "\r\n");
            TextInfo.Append(装备pvf文件目录text.Text + "\r\n");
            TextInfo.Append(装备导出目录text.Text + "\r\n");
            TextInfo.Append(寻找礼包编号textmaskedTextBox.Text + "\r\n");
            TextInfo.Append(寻找礼包pvf所在目录textmaskedTextBox.Text + "\r\n");
            TextInfo.Append(提取宝珠编号textmaskedTextBox.Text + "\r\n");
            TextInfo.Append(提取宝珠pvf所在目录textmaskedTextBox.Text + "\r\n");
            TextInfo.Append(提取宝珠导出目录textmaskedTextBox.Text + "\r\n");
            TextInfo.Append(NPK操作查找内容maskedTextBox.Text + "\r\n");
            TextInfo.Append(NPKimg全路径文本maskedTextBox.Text + "\r\n");
            TextInfo.Append(NPKimg目录maskedTextBox.Text + "\r\n");
            TextInfo.Append(NPKimg提取并导出目录maskedTextBox.Text + "\r\n");
            TextInfo.Append(设计图导出目录maskedTextBox.Text + "\r\n");
            TextInfo.Append("[ImagePacks2]" + "\r\n");
            foreach (string item in NPK操作img2目录comboBox.Items)
            {
                TextInfo.Append(item + "\r\n");
            }
            TextInfo.Append("[/ImagePacks2]" + "\r\n");
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\TextInfo.ini", TextInfo.ToString());


        }

        //程序启动时的操作
        private void Form1_Shown(object sender, EventArgs e)
        {
            MysqlYz.Yse();
            //Location = new Point(-999, -999);
            //Opacity = (double)0;
            //Width = 1;
            //Height = 1;


            //两种方式验证登陆
            //if (MysqlYz.ChaXunMoShi() == 1)
            //{
            //    using (群验证.Bd yanZheng = new 群验证.Bd())
            //    {
            //        yanZheng.ShowDialog();
            //    }
            //}
            //else if (MysqlYz.ChaXunMoShi() == 2)
            //{
            //    using (群验证.Qun yanZheng = new 群验证.Qun())
            //    {
            //        yanZheng.ShowDialog();
            //    }
            //}

            //if (Opacity == 0)
            //{
            //    Close();
            //}
            //MysqlYz.Yse();


            if (File.Exists(Directory.GetCurrentDirectory() + "\\TextInfo.ini"))
            {
                string[] TextInfo = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\TextInfo.ini");
                副本编号.Text = TextInfo[0];
                PVF文件所在目录.Text = TextInfo[1];
                提取并导出的目录.Text = TextInfo[2];
                城镇编号txt.Text = TextInfo[3];
                城镇pvf文件txt.Text = TextInfo[4];
                城镇导出目录txt.Text = TextInfo[5];
                怪物编号text.Text = TextInfo[6];
                怪物pvf文件目录text.Text = TextInfo[7];
                怪物导出目录text.Text = TextInfo[8];
                特效编号text.Text = TextInfo[9];
                特效pvf文件目录text.Text = TextInfo[10];
                特效导出目录text.Text = TextInfo[11];
                APC编号text.Text = TextInfo[12];
                APCpvf文件目录text.Text = TextInfo[13];
                APC导出目录text.Text = TextInfo[14];
                ANI全路径文件text.Text = TextInfo[15];
                ANIpvf文件目录text.Text = TextInfo[16];
                ANI导出目录text.Text = TextInfo[17];
                装备编号text.Text = TextInfo[18];
                装备pvf文件目录text.Text = TextInfo[19];
                装备导出目录text.Text = TextInfo[20];
                寻找礼包编号textmaskedTextBox.Text = TextInfo[21];
                寻找礼包pvf所在目录textmaskedTextBox.Text = TextInfo[22];
                提取宝珠编号textmaskedTextBox.Text = TextInfo[23];
                提取宝珠pvf所在目录textmaskedTextBox.Text = TextInfo[24];
                提取宝珠导出目录textmaskedTextBox.Text = TextInfo[25];
                NPK操作查找内容maskedTextBox.Text = TextInfo[26];
                NPKimg全路径文本maskedTextBox.Text = TextInfo[27];
                NPKimg目录maskedTextBox.Text = TextInfo[28];
                NPKimg提取并导出目录maskedTextBox.Text = TextInfo[29];
                设计图导出目录maskedTextBox.Text = TextInfo[30];
                for (int i = 31; i < TextInfo.Length; i++)
                {
                    if (TextInfo[i].IndexOf("\\") != -1)
                    {
                        NPK操作img2目录comboBox.Items.Add(TextInfo[i]);
                    }
                }
                if (NPK操作img2目录comboBox.Items.Count > 0)
                    NPK操作img2目录comboBox.SelectedIndex = 0;

            }


        }


        public static ToolTip toolTip_New;
        //气泡定时释放
        private void 气泡timer_Tick(object sender, EventArgs e)
        {
            if (toolTip_New == null)
                return;
            toolTip_New.Dispose();
            气泡timer.Stop();
        }
        public void AddToolInfo(string ToolText, int EndTime, int Offsetx, int Offsety, Control control)
        {
            toolTip_New = new ToolTip
            {
                IsBalloon = true
            };
            toolTip_New.SetToolTip(control, ToolText);
            toolTip_New.Show(ToolText, control, Offsetx, Offsety);
            if (气泡timer.Enabled == false)
            {
                气泡timer.Interval = EndTime;
                气泡timer.Start();
            }
        }

        //——————————————副本提取功能区————————————————

        //批量载入编号或全路径
        private void 副本编号textbutton_Click(object sender, EventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出副本编号";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                副本编号.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void PVF文件所在目录_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                PVF文件所在目录.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 提取并导出的目录_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择提取完成后需要导出的目录；例如：E:\DNF\提取输出目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                提取并导出的目录.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }

        }

        private void 副本提取使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地提取>为了确保文件提取的完整性，请导出以下目录：\r\n" +
                "dungeon\r\n" +
                "map\r\n" +
                "monster\r\n" +
                "passiveobject\r\n" +
                "aicharacter\r\n" +
                "equipment/monster\r\n\r\n" +
                "<在线提取>\r\n" +
                "则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号和目录即可";
            MessageBox.Show(Text, "副本提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 开始提取副本_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取副本？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool 是否可执行提取副本 = false;


                //开始初始判断是否提取副本
                if (副本编号.Text != "")
                {
                    if (PVF文件所在目录.Text != "")
                    {
                        if (Directory.Exists(PVF文件所在目录.Text))
                        {

                            if (Directory.Exists(PVF文件所在目录.Text + @"\dungeon"))
                            {

                                if (提取并导出的目录.Text != "")
                                {

                                    if (Directory.Exists(提取并导出的目录.Text))
                                    {

                                        MysqlYz.XY(6);
                                        是否可执行提取副本 = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("所选副本提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        提取并导出的目录.Focus();
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    提取并导出的目录.Focus();
                                }


                            }
                            else
                            {
                                MessageBox.Show("所选副本pvf文件目录下无“dungeon”目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                PVF文件所在目录.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("所选副本pvf文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            PVF文件所在目录.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入副本pvf文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        PVF文件所在目录.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取的副本编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    副本编号.Focus();
                }


                //如果用户提供都正确那么就开始提取
                if (是否可执行提取副本)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    System.Diagnostics.Stopwatch xhtime = new System.Diagnostics.Stopwatch();
                    time.Start();

                    double dgntqtime;
                    double mobobjapcxhtime;
                    double imgtime = 0;

                    Regex_new regex = new Regex_new();
                    Dnf dnf = new Dnf();


                    string pvfpath = PVF文件所在目录.Text.ToLower();
                    string movepath = 提取并导出的目录.Text;

                    List<List<string>> dgn_lst_alltext = new List<List<string>>();
                    List<List<string>> map_lst_alltext = new List<List<string>>();
                    List<List<string>> mob_lst_alltext = new List<List<string>>();
                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> apc_lst_alltext = new List<List<string>>();

                    //读入副本lst列表内容
                    if (File.Exists(pvfpath + @"\dungeon\dungeon.lst"))
                        dgn_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\dungeon\dungeon.lst").Replace("/", @"\"));
                    //读入地图lst列表内容
                    if (File.Exists(pvfpath + @"\map\map.lst"))
                        map_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\map\map.lst").Replace("/", @"\"));
                    //读入怪物lst列表内容
                    if (File.Exists(pvfpath + @"\monster\monster.lst"))
                        mob_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", @"\"));
                    //读入特效lst列表内容
                    if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst"))
                        obj_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", @"\"));
                    //读入人偶lst列表内容
                    if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst"))
                        apc_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", @"\"));


                    List<string> dgn_id = new List<string>();
                    List<string> map_id = new List<string>();
                    List<string> mob_id = new List<string>();
                    List<string> obj_id = new List<string>();
                    List<string> apc_id = new List<string>();

                    List<string> dgn_lst = new List<string>();
                    List<string> map_lst = new List<string>();
                    List<string> mob_lst = new List<string>();
                    List<string> obj_lst = new List<string>();
                    List<string> apc_lst = new List<string>();

                    List<string> dgn_path = new List<string>();
                    List<string> map_path = new List<string>();
                    List<string> mob_path = new List<string>();
                    List<string> obj_path = new List<string>();
                    List<string> apc_path = new List<string>();
                    List<string> tbl_path = new List<string>();
                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> ptl_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> key_path = new List<string>();
                    List<string> ai_path = new List<string>();

                    List<string> mob_equ_path = new List<string>();

                    List<string> end_all_path = new List<string>();

                    string 查找出来的lst列表内容 = "";

                    progressBar1.Value = 10;//进度条进度增加

                    //获取用户提供的副本编号
                    dgn_id.AddRange(dnf.LstChongPaiLieGetId(副本编号.Text));

                    //通过副本编号获取lst 跟全路径
                    dnf.Id_lst_编号查找(pvfpath + "\\dungeon\\", dgn_lst_alltext, dgn_id, dgn_lst, dgn_path);

                    //获取副本文件内的深渊map编号以及门的obj编号
                    dnf.寻找编号(dgn_path, map_id, obj_id, null, null);

                    //获取副本文件内的tbl、ani全路径
                    dnf.寻找全路径(pvfpath, dgn_path, tbl_path, null, null, ani_path, null, null, null, null, null);

                    //如果从副本文件内找到深渊map编号
                    if (map_id.Count > 0)
                    {
                        //从lst集合中查找出编号对应的全路径，存在就加入
                        dnf.Id_lst_编号查找(pvfpath + "\\map\\", map_lst_alltext, map_id, null, map_path);
                        map_id.Clear();
                    }

                    progressBar1.Value = 20;//进度条进度增加
                    //开始枚举map文件，查找副本对应的map文件路径
                    List<string> map临时 = new List<string>(Directory.EnumerateFiles(pvfpath + "\\map", "*.map", SearchOption.AllDirectories));
                    foreach (string item in map临时)
                    {
                        string mappath = item.ToLower();//取出全路径并且转为小写
                        string mapalltext = File.ReadAllText(mappath);//读入所有内容

                        if (mapalltext.Contains("[dungeon]"))//如果找到了副本标签
                        {
                            foreach (string dgnid in dgn_id)//获取用户提供的副本ID
                            {
                                //正则匹配副本编号
                                regex.创建(mapalltext, @"\[dungeon\](\r\n|\r\n.+\t)" + dgnid + "\t", 1);
                                if (regex.取匹配数量() > 0)//如果匹配数量大于0
                                {
                                    if (map_path.Contains(mappath) == false)
                                    {
                                        map_path.Add(mappath);//加入map全路径
                                    }
                                }
                            }
                        }
                    }
                    progressBar1.Value = 30;//进度条进度增加
                    //获取map文件中的 ani、til
                    dnf.寻找全路径(pvfpath, map_path, null, null, null, ani_path, null, til_path, null, null, null);

                    //通过枚举寻找出来的map全路径，获取map的编号以及lst
                    dnf.Id_lst_路径查找(pvfpath + "\\map\\", map_lst_alltext, map_id, map_lst, map_path);

                    //通过map全路径查找出 人偶、怪物、特效编号
                    dnf.寻找编号(map_path, null, obj_id, mob_id, apc_id);


                    //如果obj mob apc编号存在那么进入循环遍历（这一步已经遍历出所有mob、obj、apc）
                    xhtime.Start();
                    if (obj_id.Count > 0 || mob_id.Count > 0 || apc_id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;
                        //循环遍历出所有关联编号
                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = obj_id.Count;
                            mob_id_count = mob_id.Count;
                            apc_id_count = apc_id.Count;

                            dnf.怪物寻找编号(pvfpath + "\\monster\\", mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                            dnf.特效寻找编号(pvfpath + "\\passiveobject\\", obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                            dnf.人偶寻找编号(pvfpath + "\\aicharacter\\", apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                        } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                    }
                    mobobjapcxhtime = xhtime.ElapsedMilliseconds;
                    xhtime.Stop();

                    progressBar1.Value = 40;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(pvfpath, mob_path, tbl_path, null, atk_path, ani_path, null, null, null, ai_path, mob_equ_path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(pvfpath, obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                    //如果副本内有人偶就获取ai、key
                    if (apc_id.Count > 0)
                    {
                        dnf.寻找全路径(pvfpath, apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                    }

                    //获取act中的atk、ani
                    dnf.寻找全路径(pvfpath, act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(pvfpath, ptl_path, null, null, null, ani_path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(ani_path, als_path, 2, false);

                    //寻找ai
                    dnf.寻找ai(ai_path);
                    progressBar1.Value = 50;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    end_all_path.AddRange(dgn_path);
                    end_all_path.AddRange(map_path);
                    end_all_path.AddRange(mob_path);
                    end_all_path.AddRange(obj_path);
                    end_all_path.AddRange(apc_path);
                    end_all_path.AddRange(tbl_path);
                    end_all_path.AddRange(act_path);
                    end_all_path.AddRange(atk_path);
                    end_all_path.AddRange(ani_path);
                    end_all_path.AddRange(als_path);
                    end_all_path.AddRange(ptl_path);
                    end_all_path.AddRange(til_path);
                    end_all_path.AddRange(key_path);
                    end_all_path.AddRange(ai_path);

                    //lst信息加入字符串变量，并且准备输出
                    if (dgn_lst.Count > 0)
                    {
                        查找出来的lst列表内容 = 查找出来的lst列表内容 + "----------------------------------以下内容加入PVF【dungeon/dungeon.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + dgn_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in dgn_lst)
                        {
                            查找出来的lst列表内容 = 查找出来的lst列表内容 + item.ToString() + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到副本lst\r\n\r\n\r\n";
                    }

                    if (map_lst.Count > 0)
                    {
                        查找出来的lst列表内容 += "----------------------------------以下内容加入PVF【map/map.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + map_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in map_lst)
                        {
                            查找出来的lst列表内容 += item.ToString() + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到地图lst\r\n\r\n\r\n";
                    }

                    if (mob_lst.Count > 0)
                    {
                        查找出来的lst列表内容 += "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in mob_lst)
                        {
                            查找出来的lst列表内容 += item.ToString() + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到怪物lst\r\n\r\n\r\n";
                    }

                    if (obj_lst.Count > 0)
                    {
                        查找出来的lst列表内容 += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in obj_lst)
                        {
                            查找出来的lst列表内容 += item.ToString() + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到特效lst\r\n\r\n\r\n";
                    }

                    if (apc_lst.Count > 0)
                    {
                        查找出来的lst列表内容 += "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in apc_lst)
                        {
                            查找出来的lst列表内容 += item.ToString() + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到人偶lst\r\n\r\n\r\n";
                    }

                    progressBar1.Value = 60;//进度条进度增加

                    string endpath;
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(6);
                    //开始输出所有文件
                    foreach (string item in end_all_path)
                    {
                        //取出原文件所在全路径，并且转为小写
                        endpath = item.ToLower();

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (File.Exists(endpath) == true)
                        {
                            //转换为提取导出的全路径
                            endmovepath = endpath.Replace(pvfpath, movepath);
                            //转换为提取导出的父路径
                            moveDirectory = Directory.GetParent(endmovepath).ToString();
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.Copy(endpath, endmovepath, true);
                        }


                    }
                    progressBar1.Value = 70;//进度条进度增加


                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", 查找出来的lst列表内容.Replace("\\", "/"));

                    //结束副本提取测量时间
                    dgntqtime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (副本提取完后查找img路径.Checked)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);


                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取副本，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的副本提取工作；\r\n循环遍历mob、obj、apc编号用时：" + (double)mobobjapcxhtime / 1000 + "秒；\r\n提取副本总用时：" + (double)dgntqtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的副本提取工作；\r\n循环遍历mob、obj、apc编号用时：" + (double)mobobjapcxhtime / 1000 + "秒；\r\n提取副本总用时：" + (double)dgntqtime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }

                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        private void 开始在线提取副本button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或导出的目录路径，并且开始提取副本？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool 是否可执行提取副本 = false;

                //开始初始判断是否提取副本
                if (副本编号.Text != "")
                {
                    if (提取并导出的目录.Text != "")
                    {

                        if (Directory.Exists(提取并导出的目录.Text) == true)
                        {
                            MysqlYz.XY(6);
                            是否可执行提取副本 = true;
                        }
                        else
                        {
                            MessageBox.Show("所选副本提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            提取并导出的目录.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        提取并导出的目录.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("请输入需要提取的副本编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    副本编号.Focus();
                }


                //如果用户提供都正确那么就开始提取
                if (是否可执行提取副本)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    System.Diagnostics.Stopwatch xhtime = new System.Diagnostics.Stopwatch();
                    time.Start();

                    double dgntqtime;
                    double mobobjapcxhtime;
                    double imgtime = 0;

                    Regex_new regex = new Regex_new();
                    DnfHttp dnf = new DnfHttp();

                    string movepath = 提取并导出的目录.Text;

                    List<List<string>> dgn_lst_alltext = new List<List<string>>();
                    List<List<string>> map_lst_alltext = new List<List<string>>();
                    List<List<string>> mob_lst_alltext = new List<List<string>>();
                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> apc_lst_alltext = new List<List<string>>();

                    //读入副本lst列表内容
                    if (dnf.FileExists("dungeon/dungeon.lst"))
                    {
                        dgn_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("dungeon/dungeon.lst").Replace("\\", "/"));
                    }
                    //读入地图lst列表内容
                    if (dnf.FileExists("map/map.lst"))
                    {
                        map_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("map/map.lst").Replace("\\", "/"));
                    }
                    //读入怪物lst列表内容
                    if (dnf.FileExists("monster/monster.lst"))
                    {
                        mob_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("monster/monster.lst").Replace("\\", "/"));
                    }
                    //读入特效lst列表内容
                    if (dnf.FileExists("passiveobject/passiveobject.lst"))
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("passiveobject/passiveobject.lst").Replace("\\", "/"));
                    }
                    //读入人偶lst列表内容
                    if (dnf.FileExists("aicharacter/aicharacter.lst"))
                    {
                        apc_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("aicharacter/aicharacter.lst").Replace("\\", "/"));
                    }
                    progressBar1.Value = 10;//进度条进度增加

                    List<string> dgn_id = new List<string>();
                    List<string> map_id = new List<string>();
                    List<string> mob_id = new List<string>();
                    List<string> obj_id = new List<string>();
                    List<string> apc_id = new List<string>();

                    List<string> dgn_lst = new List<string>();
                    List<string> map_lst = new List<string>();
                    List<string> mob_lst = new List<string>();
                    List<string> obj_lst = new List<string>();
                    List<string> apc_lst = new List<string>();

                    List<string> dgn_path = new List<string>();
                    List<string> map_path = new List<string>();
                    List<string> mob_path = new List<string>();
                    List<string> obj_path = new List<string>();
                    List<string> apc_path = new List<string>();
                    List<string> tbl_path = new List<string>();
                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> ptl_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> key_path = new List<string>();
                    List<string> ai_path = new List<string>();

                    List<string> mob_equ_path = new List<string>();

                    List<string> end_all_path = new List<string>();

                    string 查找出来的lst列表内容 = "";


                    //获取用户提供的副本编号
                    dgn_id.AddRange(dnf.LstChongPaiLieGetId(副本编号.Text));

                    //通过副本编号获取lst 跟全路径
                    dnf.Id_lst_编号查找("dungeon/", dgn_lst_alltext, dgn_id, dgn_lst, dgn_path);

                    //获取副本文件内的深渊map编号以及门的obj编号
                    dnf.寻找编号(dgn_path, map_id, obj_id, null, null);

                    //获取副本文件内的tbl、ani全路径
                    dnf.寻找全路径(dgn_path, tbl_path, null, null, ani_path, null, null, null, null, null);

                    //如果从副本文件内找到深渊map编号
                    if (map_id.Count > 0)
                    {
                        //从lst集合中查找出编号对应的全路径，存在就加入
                        dnf.Id_lst_编号查找("map/", map_lst_alltext, map_id, null, map_path);

                        map_id.Clear();
                    }
                    progressBar1.Value = 20;//进度条进度增加
                    //开始枚举map文件，查找副本对应的map文件路径
                    List<string> map临时 = new List<string>(dnf.DirectoryEnumerateFiles("map/", "map", false));
                    foreach (string item in map临时)
                    {
                        string mappath = item.ToLower();//取出全路径并且转为小写
                        string mapalltext = dnf.FileReadAllText(mappath);//读入所有内容

                        if (mapalltext.Contains("[dungeon]") == true)//如果找到了副本标签
                        {
                            foreach (string dgnid in dgn_id)//获取用户提供的副本ID
                            {
                                //正则匹配副本编号
                                regex.创建(mapalltext, @"\[dungeon\](\r\n|\r\n.+\t)" + dgnid + "\t", 1);
                                if (regex.取匹配数量() > 0)//如果匹配数量大于0
                                {
                                    if (map_path.Contains(mappath) == false)
                                    {
                                        map_path.Add(mappath);//加入map全路径
                                    }
                                }
                            }
                        }
                    }
                    progressBar1.Value = 40;//进度条进度增加
                    //获取map文件中的 ani、til
                    dnf.寻找全路径(map_path, null, null, null, ani_path, null, til_path, null, null, null);

                    //通过枚举寻找出来的map全路径，获取map的编号以及lst
                    dnf.Id_lst_路径查找("map/", map_lst_alltext, map_id, map_lst, map_path);

                    //通过map全路径查找出 人偶、怪物、特效编号
                    dnf.寻找编号(map_path, null, obj_id, mob_id, apc_id);

                    //如果obj mob apc编号存在那么进入循环遍历（这一步已经遍历出所有mob、obj、apc）
                    xhtime.Start();
                    if (obj_id.Count > 0 || mob_id.Count > 0 || apc_id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;
                        //循环遍历出所有关联编号
                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = obj_id.Count;
                            mob_id_count = mob_id.Count;
                            apc_id_count = apc_id.Count;

                            dnf.怪物寻找编号(mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                            dnf.特效寻找编号(obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                            dnf.人偶寻找编号(apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                        } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                    }
                    mobobjapcxhtime = xhtime.ElapsedMilliseconds;
                    xhtime.Stop();
                    progressBar1.Value = 50;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(mob_path, tbl_path, null, atk_path, ani_path, null, null, null, ai_path, mob_equ_path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                    //如果副本内有人偶就获取ai、key
                    if (apc_id.Count > 0)
                    {
                        dnf.寻找全路径(apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                    }

                    //获取act中的atk、ani
                    dnf.寻找全路径(act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(ptl_path, null, null, null, ani_path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(ani_path, als_path, 2, false);

                    //寻找ai
                    dnf.寻找ai(ai_path);
                    progressBar1.Value = 60;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    end_all_path.AddRange(dgn_path);
                    end_all_path.AddRange(map_path);
                    end_all_path.AddRange(mob_path);
                    end_all_path.AddRange(obj_path);
                    end_all_path.AddRange(apc_path);
                    end_all_path.AddRange(tbl_path);
                    end_all_path.AddRange(act_path);
                    end_all_path.AddRange(atk_path);
                    end_all_path.AddRange(ani_path);
                    end_all_path.AddRange(als_path);
                    end_all_path.AddRange(ptl_path);
                    end_all_path.AddRange(til_path);
                    end_all_path.AddRange(key_path);
                    end_all_path.AddRange(ai_path);
                    progressBar1.Value = 70;//进度条进度增加
                    //lst信息加入字符串变量，并且准备输出
                    if (dgn_lst.Count > 0)
                    {
                        查找出来的lst列表内容 = 查找出来的lst列表内容 + "----------------------------------以下内容加入PVF【dungeon/dungeon.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + dgn_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in dgn_lst)
                        {
                            查找出来的lst列表内容 += item + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到副本lst\r\n\r\n\r\n";
                    }

                    if (map_lst.Count > 0)
                    {
                        查找出来的lst列表内容 += "----------------------------------以下内容加入PVF【map/map.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + map_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in map_lst)
                        {
                            查找出来的lst列表内容 += item + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到地图lst\r\n\r\n\r\n";
                    }

                    if (mob_lst.Count > 0)
                    {
                        查找出来的lst列表内容 += "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in mob_lst)
                        {
                            查找出来的lst列表内容 += item + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到怪物lst\r\n\r\n\r\n";
                    }

                    if (obj_lst.Count > 0)
                    {
                        查找出来的lst列表内容 += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in obj_lst)
                        {
                            查找出来的lst列表内容 += item + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到特效lst\r\n\r\n\r\n";
                    }

                    if (apc_lst.Count > 0)
                    {
                        查找出来的lst列表内容 += "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in apc_lst)
                        {
                            查找出来的lst列表内容 += item + "\r\n";
                        }
                        查找出来的lst列表内容 += "\r\n\r\n";
                    }
                    else
                    {
                        查找出来的lst列表内容 += "未找到人偶lst\r\n\r\n\r\n";
                    }
                    progressBar1.Value = 80;//进度条进度增加

                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(6);
                    //开始输出所有文件
                    foreach (string item in end_all_path)
                    {

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (dnf.FileExists(item))
                        {
                            //转换为提取导出的全路径
                            endmovepath = movepath + "/" + item;
                            //转换为提取导出的父路径
                            moveDirectory = dnf.DirectoryGetParent(endmovepath);
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                        }


                    }
                    progressBar1.Value = 90;//进度条进度增加

                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", 查找出来的lst列表内容.Replace("\\", "/"));

                    //结束副本提取测量时间
                    dgntqtime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (副本提取完后查找img路径.Checked)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取副本，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的副本提取工作；\r\n循环遍历mob、obj、apc编号用时：" + (double)mobobjapcxhtime / 1000 + "秒；\r\n提取副本总用时：" + (double)dgntqtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的副本提取工作；\r\n循环遍历mob、obj、apc编号用时：" + (double)mobobjapcxhtime / 1000 + "秒；\r\n提取副本总用时：" + (double)dgntqtime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }

                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        //——————————————副本提取功能区————————————————

        //——————————————城镇提取功能区————————————————
        private void 城镇编号textbutton_Click(object sender, EventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出城镇编号";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                城镇编号txt.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void 城镇pvf文件txt_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                城镇pvf文件txt.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 城镇导出目录txt_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                城镇导出目录txt.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 城镇提取使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地提取>为了确保文件提取的完整性，请导出以下目录：\r\n" +
                "town\r\n" +
                "map\r\n" +
                "npc\r\n" +
                "passiveobject\r\n\r\n" +
                "<在线提取>\r\n" +
                "则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号和目录即可";
            MessageBox.Show(Text, "城镇提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 开始提取城镇_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取城镇？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool 是否可执行提取城镇 = false;

                //开始初始判断是否提取城镇
                if (城镇编号txt.Text != "")
                {
                    if (城镇pvf文件txt.Text != "")
                    {

                        if (Directory.Exists(城镇pvf文件txt.Text))
                        {

                            if (Directory.Exists(城镇pvf文件txt.Text + @"\town"))
                            {

                                if (城镇导出目录txt.Text != "")
                                {

                                    if (Directory.Exists(城镇导出目录txt.Text))
                                    {
                                        MysqlYz.XY(7);
                                        是否可执行提取城镇 = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("所选城镇提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        城镇导出目录txt.Focus();
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    城镇导出目录txt.Focus();
                                }


                            }
                            else
                            {
                                MessageBox.Show("所选城镇pvf文件目录下无“town”目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                城镇pvf文件txt.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("所选城镇PVF文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            城镇pvf文件txt.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入城镇PVF文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        城镇pvf文件txt.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取的城镇编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    城镇编号txt.Focus();
                }

                if (是否可执行提取城镇 == true)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long twntime;
                    long imgtime = 0;

                    Regex_new regex = new Regex_new();
                    Dnf dnf = new Dnf();

                    string pvfpath = 城镇pvf文件txt.Text.ToLower();
                    string movepath = 城镇导出目录txt.Text.ToLower();

                    List<List<string>> twn_lst_alltext = new List<List<string>>();
                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> npc_lst_alltext = new List<List<string>>();

                    if (File.Exists(pvfpath + @"\town\town.lst") == true)
                    {
                        twn_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\town\town.lst").Replace("/", "\\"));
                    }

                    if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst") == true)
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", "\\"));
                    }

                    if (File.Exists(pvfpath + @"\npc\npc.lst") == true)
                    {
                        npc_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\npc\npc.lst").Replace("/", "\\"));
                    }
                    progressBar1.Value = 10;//进度条进度增加
                    string endtetxlst = "";

                    List<string> twn_id = new List<string>();
                    List<string> obj_id = new List<string>();
                    List<string> npc_id = new List<string>();

                    List<string> twn_lst = new List<string>();
                    List<string> obj_lst = new List<string>();
                    List<string> npc_lst = new List<string>();

                    List<string> twn_path = new List<string>();
                    List<string> obj_path = new List<string>();
                    List<string> npc_path = new List<string>();

                    List<string> map_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ptl_path = new List<string>();

                    List<string> end_path = new List<string>();

                    regex.创建(城镇编号txt.Text, @"[0-9]+", 0);
                    regex.匹配加入集合(twn_id, false, 0);

                    dnf.Id_lst_编号查找(pvfpath + "\\town\\", twn_lst_alltext, twn_id, twn_lst, twn_path);
                    progressBar1.Value = 30;//进度条进度增加
                    if (twn_path.Count > 0)
                    {
                        dnf.Twn寻找map_ani(pvfpath, twn_path, map_path, ani_path, true);
                        dnf.城镇map寻找id和路径(map_path, obj_id, npc_id, ani_path, til_path);
                        progressBar1.Value = 40;//进度条进度增加

                        if (npc_id.Count > 0)
                        {
                            dnf.Id_lst_编号查找(pvfpath + "\\npc\\", npc_lst_alltext, npc_id, npc_lst, npc_path);
                            dnf.寻找全路径(pvfpath, npc_path, null, null, null, ani_path, null, null, null, null, null);
                        }
                        progressBar1.Value = 50;//进度条进度增加
                        if (obj_id.Count > 0)
                        {
                            dnf.Id_lst_编号查找(pvfpath + "\\passiveobject\\", obj_lst_alltext, obj_id, obj_lst, obj_path);
                            dnf.特效寻找编号(pvfpath, obj_lst_alltext, obj_id, obj_lst, obj_path, null, null, act_path, ptl_path);
                            dnf.寻找全路径(pvfpath, obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);
                            dnf.寻找全路径(pvfpath, act_path, null, null, null, ani_path, null, null, null, null, null);
                            dnf.寻找全路径(pvfpath, ptl_path, null, null, null, ani_path, null, null, null, null, null);
                        }
                        progressBar1.Value = 60;//进度条进度增加
                        if (ani_path.Count > 0)
                        {
                            dnf.寻找als(ani_path, als_path, 2, false);
                        }

                        //准备输出所有文件
                        end_path.AddRange(twn_path);
                        end_path.AddRange(obj_path);
                        end_path.AddRange(npc_path);
                        end_path.AddRange(map_path);
                        end_path.AddRange(ani_path);
                        end_path.AddRange(als_path);
                        end_path.AddRange(til_path);
                        end_path.AddRange(act_path);
                        end_path.AddRange(atk_path);
                        end_path.AddRange(ptl_path);
                        progressBar1.Value = 70;//进度条进度增加
                        //如果获取的输出全路径集合存在就输出
                        if (end_path.Count > 0)
                        {
                            string endpath;
                            string endmovepath;
                            string moveDirectory;
                            MysqlYz.XY(7);
                            //开始输出所有文件
                            foreach (var item in end_path)
                            {
                                //取出原文件所在全路径，并且转为小写
                                endpath = item.ToLower();

                                //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                                if (File.Exists(endpath) == true)
                                {
                                    //转换为提取导出的全路径
                                    endmovepath = endpath.Replace(pvfpath, movepath);
                                    //转换为提取导出的父路径
                                    moveDirectory = Directory.GetParent(endmovepath).ToString();
                                    //创建提取导出的父路径所在目录
                                    Directory.CreateDirectory(moveDirectory);
                                    //复制文件到指定目录，如果有同名文件就覆盖
                                    File.Copy(endpath, endmovepath, true);
                                }

                            }
                        }
                        progressBar1.Value = 80;//进度条进度增加
                        //收集lst信息
                        //城镇lst
                        if (twn_lst.Count > 0)
                        {
                            endtetxlst += "----------------------------------以下内容加入PVF【town/town.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + twn_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                            foreach (var item in twn_lst)
                            {
                                endtetxlst += item.ToString() + "\r\n";
                            }
                            endtetxlst += "\r\n\r\n";
                        }
                        else
                        {
                            endtetxlst += "未找到城镇lst\r\n\r\n";
                        }

                        //npclst
                        if (npc_lst.Count > 0)
                        {
                            endtetxlst += "----------------------------------以下内容加入PVF【npc/npc.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + npc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                            foreach (var item in npc_lst)
                            {
                                endtetxlst += item.ToString() + "\r\n";
                            }
                            endtetxlst += "\r\n\r\n";
                        }
                        else
                        {
                            endtetxlst += "未找到npc lst\r\n\r\n";
                        }

                        //特效lst
                        if (obj_lst.Count > 0)
                        {
                            endtetxlst += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                            foreach (var item in obj_lst)
                            {
                                endtetxlst += item.ToString() + "\r\n";
                            }
                            endtetxlst += "\r\n\r\n";
                        }
                        else
                        {
                            endtetxlst += "未找到特效lst\r\n\r\n";
                        }

                        //输出lst列表内容,并且\替换为/
                        if (endtetxlst != "")
                        {
                            File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));
                        }
                        progressBar1.Value = 90;//进度条进度增加
                        twntime = time.ElapsedMilliseconds;

                        //根据用户选择查找img路径，并获取img提取用时
                        if (城镇提取完后查找img路径.Checked == true)
                        {
                            time.Restart();

                            StringBuilder npkimgpath = new StringBuilder();

                            dnf.寻找img路径(movepath, "*.*", npkimgpath);


                            if (npkimgpath.Length > 0)
                            {
                                File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                            }

                            //获取查找img路径的时间
                            imgtime = time.ElapsedMilliseconds;
                            time.Stop();
                        }
                        else
                        {
                            time.Stop();
                        }

                        progressBar1.Value = 100;//进度条进度增加
                        //弹出信息框，结束提取城镇，判断img提取时间是否存在
                        if (imgtime == 0)
                        {

                            MessageBox.Show("已完成指定的城镇提取工作；\r\n提取城镇总用时：" + (double)twntime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                        else
                        {

                            MessageBox.Show("已完成指定的城镇提取工作；\r\n提取城镇总用时：" + (double)twntime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }


                        //提取完后根据用户选择是否定位导出后的目录
                        if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("explorer.exe", movepath);
                        }

                    }
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        private void 开始在线提取城镇button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取城镇？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool 是否可执行提取城镇 = false;

                //开始初始判断是否提取城镇
                if (城镇编号txt.Text != "")
                {

                    if (城镇导出目录txt.Text != "")
                    {

                        if (Directory.Exists(城镇导出目录txt.Text))
                        {
                            MysqlYz.XY(7);
                            是否可执行提取城镇 = true;
                        }
                        else
                        {
                            MessageBox.Show("所选城镇提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            城镇导出目录txt.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        城镇导出目录txt.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取的城镇编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    城镇编号txt.Focus();
                }

                if (是否可执行提取城镇 == true)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long twntime;
                    long imgtime = 0;

                    Regex_new regex = new Regex_new();
                    DnfHttp dnf = new DnfHttp();

                    string movepath = 城镇导出目录txt.Text.ToLower();

                    List<List<string>> twn_lst_alltext = new List<List<string>>();
                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> npc_lst_alltext = new List<List<string>>();

                    if (dnf.FileExists("town/town.lst"))
                    {
                        twn_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("town/town.lst").Replace("\\", "/"));
                    }

                    if (dnf.FileExists("passiveobject/passiveobject.lst"))
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("passiveobject/passiveobject.lst").Replace("\\", "/"));
                    }

                    if (dnf.FileExists("npc/npc.lst"))
                    {
                        npc_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("npc/npc.lst").Replace("\\", "/"));
                    }
                    progressBar1.Value = 20;//进度条进度增加
                    string endtetxlst = "";

                    List<string> twn_id = new List<string>();
                    List<string> obj_id = new List<string>();
                    List<string> npc_id = new List<string>();

                    List<string> twn_lst = new List<string>();
                    List<string> obj_lst = new List<string>();
                    List<string> npc_lst = new List<string>();

                    List<string> twn_path = new List<string>();
                    List<string> obj_path = new List<string>();
                    List<string> npc_path = new List<string>();

                    List<string> map_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ptl_path = new List<string>();

                    List<string> end_path = new List<string>();

                    regex.创建(城镇编号txt.Text, @"[0-9]+", 0);
                    regex.匹配加入集合(twn_id, false, 0);

                    dnf.Id_lst_编号查找("town/", twn_lst_alltext, twn_id, twn_lst, twn_path);

                    if (twn_path.Count > 0)
                    {
                        dnf.Twn寻找map_ani(twn_path, map_path, ani_path, true);
                        dnf.城镇map寻找id和路径(map_path, obj_id, npc_id, ani_path, til_path);
                        progressBar1.Value = 30;//进度条进度增加

                        if (npc_id.Count > 0)
                        {
                            dnf.Id_lst_编号查找("npc/", npc_lst_alltext, npc_id, npc_lst, npc_path);
                            dnf.寻找全路径(npc_path, null, null, null, ani_path, null, null, null, null, null);
                        }
                        progressBar1.Value = 40;//进度条进度增加
                        if (obj_id.Count > 0)
                        {
                            dnf.Id_lst_编号查找("passiveobject/", obj_lst_alltext, obj_id, obj_lst, obj_path);
                            dnf.特效寻找编号(obj_lst_alltext, obj_id, obj_lst, obj_path, null, null, act_path, ptl_path);
                            dnf.寻找全路径(obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);
                            dnf.寻找全路径(act_path, null, null, null, ani_path, null, null, null, null, null);
                            dnf.寻找全路径(ptl_path, null, null, null, ani_path, null, null, null, null, null);
                        }
                        progressBar1.Value = 50;//进度条进度增加
                        if (ani_path.Count > 0)
                        {
                            dnf.寻找als(ani_path, als_path, 2, false);
                        }

                        //准备输出所有文件
                        end_path.AddRange(twn_path);
                        end_path.AddRange(obj_path);
                        end_path.AddRange(npc_path);
                        end_path.AddRange(map_path);
                        end_path.AddRange(ani_path);
                        end_path.AddRange(als_path);
                        end_path.AddRange(til_path);
                        end_path.AddRange(act_path);
                        end_path.AddRange(atk_path);
                        end_path.AddRange(ptl_path);
                        progressBar1.Value = 60;//进度条进度增加
                        //如果获取的输出全路径集合存在就输出
                        if (end_path.Count > 0)
                        {
                            string endmovepath;
                            string moveDirectory;
                            MysqlYz.XY(7);
                            //开始输出所有文件
                            foreach (var item in end_path)
                            {

                                //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                                if (dnf.FileExists(item))
                                {
                                    //转换为提取导出的全路径
                                    endmovepath = movepath + "/" + item;
                                    //转换为提取导出的父路径
                                    moveDirectory = dnf.DirectoryGetParent(endmovepath);
                                    //创建提取导出的父路径所在目录
                                    Directory.CreateDirectory(moveDirectory);
                                    //复制文件到指定目录，如果有同名文件就覆盖
                                    File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                                }

                            }
                        }
                        progressBar1.Value = 70;//进度条进度增加
                        //收集lst信息
                        //城镇lst
                        if (twn_lst.Count > 0)
                        {
                            endtetxlst += "----------------------------------以下内容加入PVF【town/town.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + twn_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                            foreach (var item in twn_lst)
                            {
                                endtetxlst += item.ToString() + "\r\n";
                            }
                            endtetxlst += "\r\n\r\n";
                        }
                        else
                        {
                            endtetxlst += "未找到城镇lst\r\n\r\n";
                        }

                        //npclst
                        if (npc_lst.Count > 0)
                        {
                            endtetxlst += "----------------------------------以下内容加入PVF【npc/npc.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + npc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                            foreach (var item in npc_lst)
                            {
                                endtetxlst += item.ToString() + "\r\n";
                            }
                            endtetxlst += "\r\n\r\n";
                        }
                        else
                        {
                            endtetxlst += "未找到npc lst\r\n\r\n";
                        }

                        //特效lst
                        if (obj_lst.Count > 0)
                        {
                            endtetxlst += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                            foreach (var item in obj_lst)
                            {
                                endtetxlst += item.ToString() + "\r\n";
                            }
                            endtetxlst += "\r\n\r\n";
                        }
                        else
                        {
                            endtetxlst += "未找到特效lst\r\n\r\n";
                        }

                        //输出lst列表内容,并且\替换为/
                        if (endtetxlst != "")
                        {
                            File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));
                        }

                        twntime = time.ElapsedMilliseconds;
                        progressBar1.Value = 80;//进度条进度增加
                        //根据用户选择查找img路径，并获取img提取用时
                        if (城镇提取完后查找img路径.Checked)
                        {
                            time.Restart();

                            StringBuilder npkimgpath = new StringBuilder();

                            dnf.寻找img路径(movepath, "*.*", npkimgpath);

                            if (npkimgpath.Length > 0)
                            {
                                File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                            }

                            //获取查找img路径的时间
                            imgtime = time.ElapsedMilliseconds;
                            time.Stop();
                        }
                        else
                        {
                            time.Stop();
                        }
                        progressBar1.Value = 100;//进度条进度增加

                        //弹出信息框，结束提取城镇，判断img提取时间是否存在
                        if (imgtime == 0)
                        {

                            MessageBox.Show("已完成指定的城镇提取工作；\r\n提取城镇总用时：" + (double)twntime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }
                        else
                        {

                            MessageBox.Show("已完成指定的城镇提取工作；\r\n提取城镇总用时：" + (double)twntime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                        }


                        //提取完后根据用户选择是否定位导出后的目录
                        if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("explorer.exe", movepath);
                        }

                    }
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        //——————————————城镇提取功能区————————————————


        //——————————————怪物提取功能区————————————————
        private void 怪物编号textbutton_Click(object sender, EventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出怪物编号";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                怪物编号text.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void 怪物pvf文件目录text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                怪物pvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 怪物导出目录text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                怪物导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 怪物使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地提取>为了确保文件提取的完整性，请导出以下目录：\r\n" +
                "monster\r\n" +
                "passiveobject\r\n" +
                "aicharacter\r\n" +
                "equipment/monster\r\n\r\n" +
                "<在线提取>\r\n" +
                "则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号和目录即可";
            MessageBox.Show(Text, "怪物提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 怪物开始提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取怪物？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoMob_tq = false;

                //开始初始判断是否提取怪物
                if (怪物编号text.Text != "")
                {
                    if (怪物pvf文件目录text.Text != "")
                    {

                        if (Directory.Exists(怪物pvf文件目录text.Text) == true)
                        {

                            if (Directory.Exists(怪物pvf文件目录text.Text + @"\monster") == true)
                            {

                                if (怪物导出目录text.Text != "")
                                {

                                    if (Directory.Exists(怪物导出目录text.Text) == true)
                                    {
                                        MysqlYz.XY(8);
                                        YesNoMob_tq = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("所选怪物提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        怪物导出目录text.Focus();
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    怪物导出目录text.Focus();
                                }


                            }
                            else
                            {
                                MessageBox.Show("所选怪物pvf文件目录下无“monster”目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                怪物pvf文件目录text.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("所选怪物PVF文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            怪物pvf文件目录text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入怪物PVF文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        怪物pvf文件目录text.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取的怪物编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    怪物编号text.Focus();
                }

                if (YesNoMob_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    Dnf dnf = new Dnf();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long mobtime;
                    long imgtime = 0;

                    string pvfpath = 怪物pvf文件目录text.Text.ToLower();
                    string movepath = 怪物导出目录text.Text.ToLower();

                    List<List<string>> mob_lst_alltext = new List<List<string>>();
                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> apc_lst_alltext = new List<List<string>>();

                    if (File.Exists(pvfpath + @"\monster\monster.lst"))
                    {
                        mob_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", "\\"));
                    }
                    if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst"))
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", "\\"));
                    }
                    if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst"))
                    {
                        apc_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", "\\"));
                    }
                    progressBar1.Value = 10;//进度条进度增加
                    List<string> mob_id = new List<string>();
                    List<string> obj_id = new List<string>();
                    List<string> apc_id = new List<string>();

                    List<string> mob_lst = new List<string>();
                    List<string> obj_lst = new List<string>();
                    List<string> apc_lst = new List<string>();

                    List<string> mob_path = new List<string>();
                    List<string> obj_path = new List<string>();
                    List<string> apc_path = new List<string>();
                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ptl_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> ai_path = new List<string>();
                    List<string> key_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> tbl_path = new List<string>();
                    List<string> mob_equ_path = new List<string>();

                    List<string> end_path = new List<string>();
                    string endtetxlst = "";

                    //获取用户提供的怪物编号
                    mob_id.AddRange(dnf.LstChongPaiLieGetId(怪物编号text.Text));


                    progressBar1.Value = 20;//进度条进度增加
                    //开始do while循环遍历所有关联编号
                    if (obj_id.Count > 0 || mob_id.Count > 0 || apc_id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;

                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = obj_id.Count;
                            mob_id_count = mob_id.Count;
                            apc_id_count = apc_id.Count;

                            dnf.怪物寻找编号(pvfpath + "\\monster\\", mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                            dnf.特效寻找编号(pvfpath + "\\passiveobject\\", obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                            dnf.人偶寻找编号(pvfpath + "\\aicharacter\\", apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                        } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                    }
                    progressBar1.Value = 30;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(pvfpath, mob_path, tbl_path, null, atk_path, ani_path, null, null, null, ai_path, mob_equ_path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(pvfpath, obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                    //如果找到了人偶就获取ai、key
                    if (apc_id.Count > 0)
                    {
                        dnf.寻找全路径(pvfpath, apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                    }

                    //获取act中的atk、ani
                    dnf.寻找全路径(pvfpath, act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(pvfpath, ptl_path, null, null, null, ani_path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(ani_path, als_path, 2, false);

                    //寻找ai
                    dnf.寻找ai(ai_path);
                    progressBar1.Value = 50;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    end_path.AddRange(mob_path);
                    end_path.AddRange(obj_path);
                    end_path.AddRange(apc_path);
                    end_path.AddRange(tbl_path);
                    end_path.AddRange(act_path);
                    end_path.AddRange(atk_path);
                    end_path.AddRange(ani_path);
                    end_path.AddRange(als_path);
                    end_path.AddRange(ptl_path);
                    end_path.AddRange(til_path);
                    end_path.AddRange(key_path);
                    end_path.AddRange(ai_path);
                    progressBar1.Value = 60;//进度条进度增加
                    if (mob_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in mob_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到怪物lst\r\n\r\n\r\n";
                    }

                    if (obj_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in obj_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到特效lst\r\n\r\n\r\n";
                    }

                    if (apc_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in apc_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到人偶lst\r\n\r\n\r\n";
                    }
                    progressBar1.Value = 70;//进度条进度增加

                    string endpath;
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(8);
                    //开始输出所有文件
                    foreach (string item in end_path)
                    {
                        //取出原文件所在全路径，并且转为小写
                        endpath = item.ToString().ToLower();

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (File.Exists(endpath) == true)
                        {
                            //转换为提取导出的全路径
                            endmovepath = endpath.Replace(pvfpath, movepath);
                            //转换为提取导出的父路径
                            moveDirectory = Directory.GetParent(endmovepath).ToString();
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.Copy(endpath, endmovepath, true);
                        }
                    }
                    progressBar1.Value = 80;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                    //结束怪物提取测量时间
                    mobtime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (怪物查找img路径checkBox.Checked)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);
                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取副本，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的怪物提取工作；\r\n提取怪物总用时：" + (double)mobtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的怪物提取工作；\r\n提取怪物总用时：" + (double)mobtime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }

                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        private void 怪物开始在线提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取怪物？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoMob_tq = false;

                //开始初始判断是否提取怪物
                if (怪物编号text.Text != "")
                {
                    if (怪物导出目录text.Text != "")
                    {

                        if (Directory.Exists(怪物导出目录text.Text) == true)
                        {
                            MysqlYz.XY(8);
                            YesNoMob_tq = true;
                        }
                        else
                        {
                            MessageBox.Show("所选怪物提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            怪物导出目录text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        怪物导出目录text.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("请输入需要提取的怪物编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    怪物编号text.Focus();
                }

                if (YesNoMob_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    DnfHttp dnf = new DnfHttp();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long mobtime;
                    long imgtime = 0;

                    string movepath = 怪物导出目录text.Text.ToLower();

                    List<List<string>> mob_lst_alltext = new List<List<string>>();
                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> apc_lst_alltext = new List<List<string>>();

                    if (dnf.FileExists("monster/monster.lst"))
                    {
                        mob_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("monster/monster.lst").Replace("\\", "/"));
                    }
                    if (dnf.FileExists("passiveobject/passiveobject.lst"))
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("passiveobject/passiveobject.lst").Replace("\\", "/"));
                    }
                    if (dnf.FileExists("aicharacter/aicharacter.lst"))
                    {
                        apc_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("aicharacter/aicharacter.lst").Replace("\\", "/"));
                    }
                    progressBar1.Value = 10;//进度条进度增加
                    List<string> mob_id = new List<string>();
                    List<string> obj_id = new List<string>();
                    List<string> apc_id = new List<string>();

                    List<string> mob_lst = new List<string>();
                    List<string> obj_lst = new List<string>();
                    List<string> apc_lst = new List<string>();

                    List<string> mob_path = new List<string>();
                    List<string> obj_path = new List<string>();
                    List<string> apc_path = new List<string>();
                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ptl_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> ai_path = new List<string>();
                    List<string> key_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> tbl_path = new List<string>();
                    List<string> mob_equ_path = new List<string>();

                    List<string> end_path = new List<string>();
                    string endtetxlst = "";
                    progressBar1.Value = 20;//进度条进度增加
                    //获取用户提供的怪物编号
                    mob_id.AddRange(dnf.LstChongPaiLieGetId(怪物编号text.Text));

                    //开始do while循环遍历所有关联编号
                    if (obj_id.Count > 0 || mob_id.Count > 0 || apc_id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;

                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = obj_id.Count;
                            mob_id_count = mob_id.Count;
                            apc_id_count = apc_id.Count;

                            dnf.怪物寻找编号(mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                            dnf.特效寻找编号(obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                            dnf.人偶寻找编号(apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                        } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                    }
                    progressBar1.Value = 30;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(mob_path, tbl_path, null, atk_path, ani_path, null, null, null, ai_path, mob_equ_path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                    //如果找到了人偶就获取ai、key
                    if (apc_id.Count > 0)
                    {
                        dnf.寻找全路径(apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                    }
                    progressBar1.Value = 40;//进度条进度增加
                    //获取act中的atk、ani
                    dnf.寻找全路径(act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(ptl_path, null, null, null, ani_path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(ani_path, als_path, 2, false);

                    //寻找ai
                    dnf.寻找ai(ai_path);
                    progressBar1.Value = 50;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    end_path.AddRange(mob_path);
                    end_path.AddRange(obj_path);
                    end_path.AddRange(apc_path);
                    end_path.AddRange(tbl_path);
                    end_path.AddRange(act_path);
                    end_path.AddRange(atk_path);
                    end_path.AddRange(ani_path);
                    end_path.AddRange(als_path);
                    end_path.AddRange(ptl_path);
                    end_path.AddRange(til_path);
                    end_path.AddRange(key_path);
                    end_path.AddRange(ai_path);
                    progressBar1.Value = 60;//进度条进度增加
                    if (mob_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in mob_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到怪物lst\r\n\r\n\r\n";
                    }

                    if (obj_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in obj_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到特效lst\r\n\r\n\r\n";
                    }

                    if (apc_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in apc_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到人偶lst\r\n\r\n\r\n";
                    }

                    progressBar1.Value = 70;//进度条进度增加
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(8);
                    //开始输出所有文件
                    foreach (string item in end_path)
                    {

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (dnf.FileExists(item))
                        {
                            //转换为提取导出的全路径
                            endmovepath = movepath + "/" + item;
                            //转换为提取导出的父路径
                            moveDirectory = dnf.DirectoryGetParent(endmovepath);
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                        }
                    }
                    progressBar1.Value = 80;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                    //结束怪物提取测量时间
                    mobtime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (怪物查找img路径checkBox.Checked == true)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取副本，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的怪物提取工作；\r\n提取怪物总用时：" + (double)mobtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的怪物提取工作；\r\n提取怪物总用时：" + (double)mobtime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }

                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        //——————————————怪物提取功能区————————————————


        //——————————————特效提取功能区————————————————
        private void 特效编号textbutton_Click(object sender, EventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出特效编号";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                特效编号text.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void 特效pvf文件目录text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                特效pvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 特效导出目录text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                特效导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }


        private void 特效使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地提取>为了确保文件提取的完整性，请导出以下目录：\r\n" +
                "monster\r\n" +
                "passiveobject\r\n" +
                "aicharacter\r\n" +
                "equipment/monster\r\n\r\n" +
                "<在线提取>\r\n" +
                "则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号和目录即可";
            MessageBox.Show(Text, "特效提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 特效开始提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取特效？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoObj_tq = false;

                //开始初始判断是否提取特效
                if (特效编号text.Text != "")
                {
                    if (特效pvf文件目录text.Text != "")
                    {

                        if (Directory.Exists(特效pvf文件目录text.Text) == true)
                        {

                            if (Directory.Exists(特效pvf文件目录text.Text + @"\passiveobject") == true)
                            {

                                if (特效导出目录text.Text != "")
                                {

                                    if (Directory.Exists(特效导出目录text.Text) == true)
                                    {
                                        MysqlYz.XY(9);
                                        YesNoObj_tq = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("所选特效提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        特效导出目录text.Focus();
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    特效导出目录text.Focus();
                                }


                            }
                            else
                            {
                                MessageBox.Show("所选特效pvf文件目录下无“passiveobject”目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                特效pvf文件目录text.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("所选特效PVF文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            特效pvf文件目录text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入特效PVF文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        特效pvf文件目录text.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取的特效编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    特效编号text.Focus();
                }

                if (YesNoObj_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    Dnf dnf = new Dnf();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long objtime;
                    long imgtime = 0;

                    string pvfpath = 特效pvf文件目录text.Text.ToLower();
                    string movepath = 特效导出目录text.Text.ToLower();

                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> mob_lst_alltext = new List<List<string>>();
                    List<List<string>> apc_lst_alltext = new List<List<string>>();

                    if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst"))
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", "\\"));
                    }
                    if (File.Exists(pvfpath + @"\monster\monster.lst"))
                    {
                        mob_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", "\\"));
                    }
                    if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst"))
                    {
                        apc_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", "\\"));
                    }
                    progressBar1.Value = 10;//进度条进度增加
                    List<string> obj_id = new List<string>();
                    List<string> mob_id = new List<string>();
                    List<string> apc_id = new List<string>();

                    List<string> obj_lst = new List<string>();
                    List<string> mob_lst = new List<string>();
                    List<string> apc_lst = new List<string>();

                    List<string> obj_path = new List<string>();
                    List<string> mob_path = new List<string>();
                    List<string> apc_path = new List<string>();
                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ptl_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> ai_path = new List<string>();
                    List<string> key_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> tbl_path = new List<string>();
                    List<string> mob_equ_path = new List<string>();

                    List<string> end_path = new List<string>();
                    string endtetxlst = "";
                    progressBar1.Value = 20;//进度条进度增加
                    //获取用户提供的特效编号
                    obj_id.AddRange(dnf.LstChongPaiLieGetId(特效编号text.Text));

                    //开始do while循环遍历所有关联编号
                    if (obj_id.Count > 0 || mob_id.Count > 0 || apc_id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;

                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = obj_id.Count;
                            mob_id_count = mob_id.Count;
                            apc_id_count = apc_id.Count;

                            dnf.怪物寻找编号(pvfpath + "\\monster\\", mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                            dnf.特效寻找编号(pvfpath + "\\passiveobject\\", obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                            dnf.人偶寻找编号(pvfpath + "\\aicharacter\\", apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                        } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                    }
                    progressBar1.Value = 30;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(pvfpath, mob_path, tbl_path, null, atk_path, ani_path, null, null, null, ai_path, mob_equ_path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(pvfpath, obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                    //如果找到了人偶就获取ai、key
                    if (apc_id.Count > 0)
                    {
                        dnf.寻找全路径(pvfpath, apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                    }
                    progressBar1.Value = 40;//进度条进度增加
                    //获取act中的atk、ani
                    dnf.寻找全路径(pvfpath, act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(pvfpath, ptl_path, null, null, null, ani_path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(ani_path, als_path, 2, false);

                    //寻找ai
                    dnf.寻找ai(ai_path);
                    progressBar1.Value = 50;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    end_path.AddRange(mob_path);
                    end_path.AddRange(obj_path);
                    end_path.AddRange(apc_path);
                    end_path.AddRange(tbl_path);
                    end_path.AddRange(act_path);
                    end_path.AddRange(atk_path);
                    end_path.AddRange(ani_path);
                    end_path.AddRange(als_path);
                    end_path.AddRange(ptl_path);
                    end_path.AddRange(til_path);
                    end_path.AddRange(key_path);
                    end_path.AddRange(ai_path);
                    progressBar1.Value = 60;//进度条进度增加
                    if (obj_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in obj_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到特效lst\r\n\r\n\r\n";
                    }

                    if (mob_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in mob_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到怪物lst\r\n\r\n\r\n";
                    }

                    if (apc_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in apc_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到人偶lst\r\n\r\n\r\n";
                    }
                    progressBar1.Value = 70;//进度条进度增加

                    string endpath;
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(9);
                    //开始输出所有文件
                    foreach (string item in end_path)
                    {
                        //取出原文件所在全路径，并且转为小写
                        endpath = item.ToString().ToLower();

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (File.Exists(endpath) == true)
                        {
                            //转换为提取导出的全路径
                            endmovepath = endpath.Replace(pvfpath, movepath);
                            //转换为提取导出的父路径
                            moveDirectory = Directory.GetParent(endmovepath).ToString();
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.Copy(endpath, endmovepath, true);
                        }
                    }
                    progressBar1.Value = 80;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                    //结束特效提取测量时间
                    objtime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (特效查找img路径checkBox.Checked == true)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取特效，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的特效提取工作；\r\n提取特效总用时：" + (double)objtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的特效提取工作；\r\n提取特效总用时：" + (double)objtime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        private void 特效开始在线提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取特效？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoObj_tq = false;

                //开始初始判断是否提取特效
                if (特效编号text.Text != "")
                {

                    if (特效导出目录text.Text != "")
                    {

                        if (Directory.Exists(特效导出目录text.Text))
                        {
                            MysqlYz.XY(9);
                            YesNoObj_tq = true;
                        }
                        else
                        {
                            MessageBox.Show("所选特效提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            特效导出目录text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        特效导出目录text.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取的特效编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    特效编号text.Focus();
                }

                if (YesNoObj_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    DnfHttp dnf = new DnfHttp();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long objtime;
                    long imgtime = 0;

                    string movepath = 特效导出目录text.Text.ToLower();

                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> mob_lst_alltext = new List<List<string>>();
                    List<List<string>> apc_lst_alltext = new List<List<string>>();

                    if (dnf.FileExists("passiveobject/passiveobject.lst"))
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("passiveobject/passiveobject.lst").Replace("\\", "/"));
                    }
                    if (dnf.FileExists("monster/monster.lst"))
                    {
                        mob_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("monster/monster.lst").Replace("\\", "/"));
                    }
                    if (dnf.FileExists("aicharacter/aicharacter.lst"))
                    {
                        apc_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("aicharacter/aicharacter.lst").Replace("\\", "/"));
                    }
                    progressBar1.Value = 10;//进度条进度增加
                    List<string> obj_id = new List<string>();
                    List<string> mob_id = new List<string>();
                    List<string> apc_id = new List<string>();

                    List<string> obj_lst = new List<string>();
                    List<string> mob_lst = new List<string>();
                    List<string> apc_lst = new List<string>();

                    List<string> obj_path = new List<string>();
                    List<string> mob_path = new List<string>();
                    List<string> apc_path = new List<string>();
                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ptl_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> ai_path = new List<string>();
                    List<string> key_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> tbl_path = new List<string>();
                    List<string> mob_equ_path = new List<string>();

                    List<string> end_path = new List<string>();
                    string endtetxlst = "";
                    progressBar1.Value = 20;//进度条进度增加
                    //获取用户提供的特效编号
                    obj_id.AddRange(dnf.LstChongPaiLieGetId(特效编号text.Text));

                    progressBar1.Value = 30;//进度条进度增加
                    //开始do while循环遍历所有关联编号
                    if (obj_id.Count > 0 || mob_id.Count > 0 || apc_id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;

                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = obj_id.Count;
                            mob_id_count = mob_id.Count;
                            apc_id_count = apc_id.Count;

                            dnf.怪物寻找编号(mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                            dnf.特效寻找编号(obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                            dnf.人偶寻找编号(apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                        } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                    }
                    progressBar1.Value = 40;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(mob_path, tbl_path, null, atk_path, ani_path, null, null, null, ai_path, mob_equ_path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                    //如果找到了人偶就获取ai、key
                    if (apc_id.Count > 0)
                    {
                        dnf.寻找全路径(apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                    }
                    progressBar1.Value = 50;//进度条进度增加
                    //获取act中的atk、ani
                    dnf.寻找全路径(act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(ptl_path, null, null, null, ani_path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(ani_path, als_path, 2, false);

                    //寻找ai
                    dnf.寻找ai(ai_path);
                    progressBar1.Value = 60;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    end_path.AddRange(mob_path);
                    end_path.AddRange(obj_path);
                    end_path.AddRange(apc_path);
                    end_path.AddRange(tbl_path);
                    end_path.AddRange(act_path);
                    end_path.AddRange(atk_path);
                    end_path.AddRange(ani_path);
                    end_path.AddRange(als_path);
                    end_path.AddRange(ptl_path);
                    end_path.AddRange(til_path);
                    end_path.AddRange(key_path);
                    end_path.AddRange(ai_path);
                    progressBar1.Value = 70;//进度条进度增加
                    if (obj_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in obj_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到特效lst\r\n\r\n\r\n";
                    }

                    if (mob_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in mob_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到怪物lst\r\n\r\n\r\n";
                    }

                    if (apc_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in apc_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到人偶lst\r\n\r\n\r\n";
                    }
                    progressBar1.Value = 80;//进度条进度增加
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(9);
                    //开始输出所有文件
                    foreach (string item in end_path)
                    {

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (dnf.FileExists(item))
                        {
                            //转换为提取导出的全路径
                            endmovepath = movepath + "/" + item;
                            //转换为提取导出的父路径
                            moveDirectory = dnf.DirectoryGetParent(endmovepath);
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                        }
                    }
                    progressBar1.Value = 90;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                    //结束特效提取测量时间
                    objtime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (特效查找img路径checkBox.Checked == true)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取特效，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的特效提取工作；\r\n提取特效总用时：" + (double)objtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的特效提取工作；\r\n提取特效总用时：" + (double)objtime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        //——————————————特效提取功能区————————————————


        //——————————————APC提取功能区————————————————
        private void APC编号textbutton_Click(object sender, EventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出APC编号";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                APC编号text.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void APCpvf文件目录text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                APCpvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void APC导出目录text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                APC导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }


        private void APC使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地提取>为了确保文件提取的完整性，请导出以下目录：\r\n" +
                "monster\r\n" +
                "passiveobject\r\n" +
                "aicharacter\r\n" +
                "equipment/monster\r\n\r\n" +
                "<在线提取>\r\n" +
                "则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号和目录即可";
            MessageBox.Show(Text, "APC提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void APC开始提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取APC人偶？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoApc_tq = false;

                //开始初始判断是否提取特效
                if (APC编号text.Text != "")
                {
                    if (APCpvf文件目录text.Text != "")
                    {

                        if (Directory.Exists(APCpvf文件目录text.Text) == true)
                        {

                            if (Directory.Exists(APCpvf文件目录text.Text + @"\aicharacter") == true)
                            {

                                if (APC导出目录text.Text != "")
                                {

                                    if (Directory.Exists(APC导出目录text.Text) == true)
                                    {
                                        MysqlYz.XY(10);
                                        YesNoApc_tq = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("所选APC提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        APC导出目录text.Focus();
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    APC导出目录text.Focus();
                                }


                            }
                            else
                            {
                                MessageBox.Show("所选APCpvf文件目录下无“aicharacter”目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                APCpvf文件目录text.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("所选APCpvf文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            APCpvf文件目录text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入APCpvf文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        APCpvf文件目录text.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取的APC编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    APC编号text.Focus();
                }

                if (YesNoApc_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    Dnf dnf = new Dnf();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long apctime;
                    long imgtime = 0;

                    string pvfpath = APCpvf文件目录text.Text.ToLower();
                    string movepath = APC导出目录text.Text.ToLower();

                    List<List<string>> apc_lst_alltext = new List<List<string>>();
                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> mob_lst_alltext = new List<List<string>>();
                    progressBar1.Value = 10;//进度条进度增加

                    if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst"))
                    {
                        apc_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", "\\"));
                    }
                    if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst"))
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", "\\"));
                    }
                    if (File.Exists(pvfpath + @"\monster\monster.lst"))
                    {
                        mob_lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", "\\"));
                    }

                    progressBar1.Value = 20;//进度条进度增加
                    List<string> apc_id = new List<string>();
                    List<string> obj_id = new List<string>();
                    List<string> mob_id = new List<string>();

                    List<string> apc_lst = new List<string>();
                    List<string> obj_lst = new List<string>();
                    List<string> mob_lst = new List<string>();

                    List<string> apc_path = new List<string>();
                    List<string> obj_path = new List<string>();
                    List<string> mob_path = new List<string>();

                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ptl_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> ai_path = new List<string>();
                    List<string> key_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> tbl_path = new List<string>();
                    List<string> mob_equ_path = new List<string>();

                    List<string> end_path = new List<string>();
                    string endtetxlst = "";

                    //获取用户提供的APC编号
                    apc_id.AddRange(dnf.LstChongPaiLieGetId(APC编号text.Text));

                    progressBar1.Value = 30;//进度条进度增加
                    //开始do while循环遍历所有关联编号
                    if (obj_id.Count > 0 || mob_id.Count > 0 || apc_id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;

                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = obj_id.Count;
                            mob_id_count = mob_id.Count;
                            apc_id_count = apc_id.Count;

                            dnf.怪物寻找编号(pvfpath + "\\monster\\", mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                            dnf.特效寻找编号(pvfpath + "\\passiveobject\\", obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                            dnf.人偶寻找编号(pvfpath + "\\aicharacter\\", apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                        } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                    }
                    progressBar1.Value = 40;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(pvfpath, mob_path, tbl_path, null, atk_path, ani_path, null, null, null, ai_path, mob_equ_path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(pvfpath, obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                    //如果找到了人偶就获取ai、key
                    if (apc_id.Count > 0)
                    {
                        dnf.寻找全路径(pvfpath, apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                    }
                    progressBar1.Value = 50;//进度条进度增加
                    //获取act中的atk、ani
                    dnf.寻找全路径(pvfpath, act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(pvfpath, ptl_path, null, null, null, ani_path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(ani_path, als_path, 2, false);

                    //寻找ai
                    dnf.寻找ai(ai_path);
                    progressBar1.Value = 60;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    end_path.AddRange(mob_path);
                    end_path.AddRange(obj_path);
                    end_path.AddRange(apc_path);
                    end_path.AddRange(tbl_path);
                    end_path.AddRange(act_path);
                    end_path.AddRange(atk_path);
                    end_path.AddRange(ani_path);
                    end_path.AddRange(als_path);
                    end_path.AddRange(ptl_path);
                    end_path.AddRange(til_path);
                    end_path.AddRange(key_path);
                    end_path.AddRange(ai_path);
                    progressBar1.Value = 70;//进度条进度增加
                    if (apc_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in apc_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到人偶lst\r\n\r\n\r\n";
                    }

                    if (obj_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in obj_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到特效lst\r\n\r\n\r\n";
                    }

                    if (mob_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in mob_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到怪物lst\r\n\r\n\r\n";
                    }

                    progressBar1.Value = 80;//进度条进度增加
                    string endpath;
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(10);
                    //开始输出所有文件
                    foreach (string item in end_path)
                    {
                        //取出原文件所在全路径，并且转为小写
                        endpath = item.ToString().ToLower();

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (File.Exists(endpath) == true)
                        {
                            //转换为提取导出的全路径
                            endmovepath = endpath.Replace(pvfpath, movepath);
                            //转换为提取导出的父路径
                            moveDirectory = Directory.GetParent(endmovepath).ToString();
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.Copy(endpath, endmovepath, true);
                        }
                    }
                    progressBar1.Value = 90;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                    //结束APC提取测量时间
                    apctime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (APC查找img路径checkBox.Checked == true)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取APC，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的APC提取工作；\r\n提取APC总用时：" + (double)apctime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的APC提取工作；\r\n提取APC总用时：" + (double)apctime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        private void APC开始在线提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入编号或目录路径，并且开始提取APC人偶？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoApc_tq = false;

                //开始初始判断是否提取特效
                if (APC编号text.Text != "")
                {
                    if (APC导出目录text.Text != "")
                    {

                        if (Directory.Exists(APC导出目录text.Text) == true)
                        {
                            MysqlYz.XY(10);
                            YesNoApc_tq = true;
                        }
                        else
                        {
                            MessageBox.Show("所选APC提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            APC导出目录text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        APC导出目录text.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取的APC编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    APC编号text.Focus();
                }

                if (YesNoApc_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    DnfHttp dnf = new DnfHttp();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long apctime;
                    long imgtime = 0;

                    string movepath = APC导出目录text.Text.ToLower();

                    List<List<string>> apc_lst_alltext = new List<List<string>>();
                    List<List<string>> obj_lst_alltext = new List<List<string>>();
                    List<List<string>> mob_lst_alltext = new List<List<string>>();

                    progressBar1.Value = 10;//进度条进度增加
                    if (dnf.FileExists("aicharacter/aicharacter.lst"))
                    {
                        apc_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("aicharacter/aicharacter.lst").Replace("\\", "/"));
                    }
                    if (dnf.FileExists("passiveobject/passiveobject.lst"))
                    {
                        obj_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("passiveobject/passiveobject.lst").Replace("\\", "/"));
                    }
                    if (dnf.FileExists("monster/monster.lst"))
                    {
                        mob_lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("monster/monster.lst").Replace("\\", "/"));
                    }

                    progressBar1.Value = 20;//进度条进度增加
                    List<string> apc_id = new List<string>();
                    List<string> obj_id = new List<string>();
                    List<string> mob_id = new List<string>();

                    List<string> apc_lst = new List<string>();
                    List<string> obj_lst = new List<string>();
                    List<string> mob_lst = new List<string>();

                    List<string> apc_path = new List<string>();
                    List<string> obj_path = new List<string>();
                    List<string> mob_path = new List<string>();

                    List<string> act_path = new List<string>();
                    List<string> atk_path = new List<string>();
                    List<string> ptl_path = new List<string>();
                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();
                    List<string> ai_path = new List<string>();
                    List<string> key_path = new List<string>();
                    List<string> til_path = new List<string>();
                    List<string> tbl_path = new List<string>();
                    List<string> mob_equ_path = new List<string>();

                    List<string> end_path = new List<string>();
                    string endtetxlst = "";
                    progressBar1.Value = 30;//进度条进度增加
                    //获取用户提供的APC编号
                    apc_id.AddRange(dnf.LstChongPaiLieGetId(APC编号text.Text));

                    //开始do while循环遍历所有关联编号
                    if (obj_id.Count > 0 || mob_id.Count > 0 || apc_id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;

                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = obj_id.Count;
                            mob_id_count = mob_id.Count;
                            apc_id_count = apc_id.Count;

                            dnf.怪物寻找编号(mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                            dnf.特效寻找编号(obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                            dnf.人偶寻找编号(apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                        } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                    }
                    progressBar1.Value = 40;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(mob_path, tbl_path, null, atk_path, ani_path, null, null, null, ai_path, mob_equ_path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                    //如果找到了人偶就获取ai、key
                    if (apc_id.Count > 0)
                    {
                        dnf.寻找全路径(apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                    }
                    progressBar1.Value = 50;//进度条进度增加
                    //获取act中的atk、ani
                    dnf.寻找全路径(act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(ptl_path, null, null, null, ani_path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(ani_path, als_path, 2, false);

                    //寻找ai
                    dnf.寻找ai(ai_path);
                    progressBar1.Value = 60;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    end_path.AddRange(mob_path);
                    end_path.AddRange(obj_path);
                    end_path.AddRange(apc_path);
                    end_path.AddRange(tbl_path);
                    end_path.AddRange(act_path);
                    end_path.AddRange(atk_path);
                    end_path.AddRange(ani_path);
                    end_path.AddRange(als_path);
                    end_path.AddRange(ptl_path);
                    end_path.AddRange(til_path);
                    end_path.AddRange(key_path);
                    end_path.AddRange(ai_path);
                    progressBar1.Value = 70;//进度条进度增加
                    if (apc_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in apc_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到人偶lst\r\n\r\n\r\n";
                    }

                    if (obj_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in obj_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到特效lst\r\n\r\n\r\n";
                    }

                    if (mob_lst.Count > 0)
                    {
                        endtetxlst += "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (string item in mob_lst)
                        {
                            endtetxlst += item + "\r\n";
                        }
                        endtetxlst += "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst += "未找到怪物lst\r\n\r\n\r\n";
                    }

                    progressBar1.Value = 80;//进度条进度增加
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(10);
                    //开始输出所有文件
                    foreach (string item in end_path)
                    {

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (dnf.FileExists(item))
                        {
                            //转换为提取导出的全路径
                            endmovepath = movepath + "/" + item;
                            //转换为提取导出的父路径
                            moveDirectory = dnf.DirectoryGetParent(endmovepath);
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                        }
                    }
                    progressBar1.Value = 90;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                    //结束APC提取测量时间
                    apctime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (APC查找img路径checkBox.Checked == true)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取APC，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的APC提取工作；\r\n提取APC总用时：" + (double)apctime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的APC提取工作；\r\n提取APC总用时：" + (double)apctime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        //——————————————APC提取功能区————————————————


        //——————————————ANI提取功能区————————————————
        private void ANI全路径文件text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = false,//设置不能选择文件，可以选择文件
                Title = @"请选择ANI全路径文件；例如：E:\DNF\ANI全路径.txt"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                ANI全路径文件text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }
        private void ANIpvf文件目录text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                ANIpvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void ANI导出目录text_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录为桌面
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                ANI导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }


        private void ANI使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地提取>\r\n" +
                "此功能是为了自定义用户扩展例如：\r\n" +
                "手动提取自己编写的ani文件或者编写的技能ani文件或者解包中的ani文件\r\n\r\n" +
                "<在线提取>\r\n则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号和目录即可";
            MessageBox.Show(Text, "ANI提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void ANI开始提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入ani文本路径或目录路径，并且开始提取ani或als？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoAni_tq = false;

                //开始初始判断是否提取特效
                if (ANI全路径文件text.Text != "")
                {

                    if (File.Exists(ANI全路径文件text.Text))
                    {

                        if (ANIpvf文件目录text.Text != "")
                        {

                            if (Directory.Exists(ANIpvf文件目录text.Text))
                            {

                                if (ANI导出目录text.Text != "")
                                {

                                    if (Directory.Exists(ANI导出目录text.Text))
                                    {
                                        MysqlYz.XY(11);
                                        YesNoAni_tq = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("所选ANI提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        ANI导出目录text.Focus();
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ANI导出目录text.Focus();
                                }

                            }
                            else
                            {
                                MessageBox.Show("所选ANIpvf文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ANIpvf文件目录text.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("请输入ANIpvf文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ANIpvf文件目录text.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("提供的ANI全路径文本文件不存在，请重新选择", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ANI全路径文件text.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("请输入需要提取ANI全路径的文本文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ANI全路径文件text.Focus();
                }

                if (YesNoAni_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    Dnf dnf = new Dnf();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long anitime;
                    long imgtime = 0;

                    string pvfpath = ANIpvf文件目录text.Text.ToLower();
                    string movepath = ANI导出目录text.Text.ToLower();

                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();

                    List<string> end_path = new List<string>();
                    progressBar1.Value = 10;//进度条进度增加

                    if (pvf中的全路径radioButton.Checked)
                    {
                        foreach (string item in File.ReadAllLines(ANI全路径文件text.Text))
                        {
                            if (ani_path.Contains(pvfpath + "\\" + item))
                            {
                                ani_path.Add(pvfpath + "\\" + item);
                            }
                        }
                    }
                    else if (Windows中的全路径radioButton.Checked)
                    {
                        ani_path = new List<string>(File.ReadLines(ANI全路径文件text.Text));
                    }
                    progressBar1.Value = 20;//进度条进度增加
                    dnf.寻找als(ani_path, als_path, 2, false);
                    progressBar1.Value = 40;//进度条进度增加

                    end_path.AddRange(ani_path);
                    end_path.AddRange(als_path);
                    progressBar1.Value = 60;//进度条进度增加
                    string endpath;
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(11);
                    //开始输出所有文件
                    foreach (string item in end_path)
                    {
                        //取出原文件所在全路径，并且转为小写
                        endpath = item.ToLower();

                        //要复制的文件是否存在
                        if (File.Exists(endpath))
                        {
                            //转换为提取导出的全路径
                            endmovepath = endpath.Replace(pvfpath, movepath);
                            //转换为提取导出的父路径
                            moveDirectory = Directory.GetParent(endmovepath).ToString();
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.Copy(endpath, endmovepath, true);
                        }
                    }
                    progressBar1.Value = 80;//进度条进度增加
                    //结束APC提取测量时间
                    anitime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (ANI查找img路径checkBox.Checked == true)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取ani;als，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的ani;als提取工作；\r\n提取ani;als总用时：" + (double)anitime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的ani;als提取工作；\r\n提取ani;als总用时：" + (double)anitime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }

                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        private void ANI开始在线提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已正确输入ani文本路径或目录路径，并且开始提取ani或als？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoAni_tq = false;

                //开始初始判断是否提取特效
                if (pvf中的全路径radioButton.Checked)
                {

                    if (ANI全路径文件text.Text != "")
                    {

                        if (File.Exists(ANI全路径文件text.Text))
                        {

                            if (ANI导出目录text.Text != "")
                            {

                                if (Directory.Exists(ANI导出目录text.Text))
                                {
                                    MysqlYz.XY(11);
                                    YesNoAni_tq = true;
                                }
                                else
                                {
                                    MessageBox.Show("所选ANI提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ANI导出目录text.Focus();
                                }

                            }
                            else
                            {
                                MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ANI导出目录text.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("提供的ANI全路径文本文件不存在，请重新选择", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ANI全路径文件text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入需要提取ANI全路径的文本文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ANI全路径文件text.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("在线提取必须选择PVF中的全路径；\r\n而且提供的ani全路径文本也要是以下格式：\r\nmonster/astray_gbl_devotee/animation/stay.ani", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }




                if (YesNoAni_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    DnfHttp dnf = new DnfHttp();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long anitime;
                    long imgtime = 0;

                    string movepath = ANI导出目录text.Text.ToLower();

                    List<string> ani_path = new List<string>();
                    List<string> als_path = new List<string>();

                    List<string> end_path = new List<string>();
                    progressBar1.Value = 10;//进度条进度增加

                    if (pvf中的全路径radioButton.Checked)
                    {
                        ani_path.AddRange(new List<string>(File.ReadLines(ANI全路径文件text.Text)));
                    }
                    progressBar1.Value = 20;//进度条进度增加
                    dnf.寻找als(ani_path, als_path, 2, false);
                    progressBar1.Value = 30;//进度条进度增加

                    end_path.AddRange(ani_path);
                    end_path.AddRange(als_path);
                    progressBar1.Value = 50;//进度条进度增加
                    string endmovepath;
                    string moveDirectory;
                    MysqlYz.XY(11);
                    progressBar1.Value = 60;//进度条进度增加
                    //开始输出所有文件
                    foreach (string item in end_path)
                    {

                        //要复制的文件是否存在
                        if (dnf.FileExists(item))
                        {
                            //转换为提取导出的全路径
                            endmovepath = movepath + "/" + item;
                            //转换为提取导出的父路径
                            moveDirectory = dnf.DirectoryGetParent(endmovepath);
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                        }
                    }
                    progressBar1.Value = 80;//进度条进度增加
                    //结束APC提取测量时间
                    anitime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (ANI查找img路径checkBox.Checked == true)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取ani;als，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的ani;als提取工作；\r\n提取ani;als总用时：" + (double)anitime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的ani;als提取工作；\r\n提取ani;als总用时：" + (double)anitime / 1000 + "秒；\r\nimg路径查找用时：" + (double)imgtime / 1000 + "秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }

                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        //——————————————ANI提取功能区————————————————


        //——————————————装备提取功能区————————————————

        private void 装备编号textbutton_Click(object sender, EventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出装备编号";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                装备编号text.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void 装备pvf文件目录text_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//获得全局目录
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                装备pvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//路径加入到全局变量
            }
        }

        private void 装备导出目录text_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//获得全局目录
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF提取后导出的目录；例如：E:\DNF\提取后导出的目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                装备导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//路径加入到全局变量
            }
        }


        private void 装备提取使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地提取>为了确保文件提取的完整性，请导出以下目录：\r\n" +
                "equipment\r\n" +
                "appendage\r\n" +
                "monster\r\n" +
                "passiveobject\r\n" +
                "aicharacter\r\n" +
                "如果提取套装属性文件则还需导出此文件：etc/equipmentpartset.etc\r\n" +
                "如果选择了提取宠物cre则还需导出此目录：creature\r\n\r\n" +
                "<在线提取>\r\n" +
                "则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号和目录即可";
            MessageBox.Show(Text, "装备提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 装备开始提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已输入的编号或目录路径正确，并且开始提取装备？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoEqu_tq = false;

                //开始初始判断是否提取装备
                if (装备编号text.Text != "")
                {
                    if (装备pvf文件目录text.Text != "")
                    {

                        if (Directory.Exists(装备pvf文件目录text.Text))
                        {

                            if (装备导出目录text.Text != "")
                            {

                                if (Directory.Exists(装备导出目录text.Text))
                                {
                                    MysqlYz.XY(12);
                                    YesNoEqu_tq = true;
                                }
                                else
                                {
                                    MessageBox.Show("所选equ装备提取并导出的目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    装备导出目录text.Focus();
                                }

                            }
                            else
                            {
                                MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                装备导出目录text.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("所选pvf文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            装备pvf文件目录text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入pvf文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        装备pvf文件目录text.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取equ编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    装备编号text.Focus();
                }

                if (YesNoEqu_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();
                    long Equtime;
                    long Imgtime = 0;
                    long EquImgtime = 0;

                    Dnf dnf = new Dnf();
                    bool MoXingImgTiQu = 装备提取模型checkBox.Checked;
                    bool MingWenImgTiQu = 装备查找img路径checkBox.Checked;
                    bool TiQuCre = 装备提取宠物crecheckBox.Checked;
                    string pvfpath = 装备pvf文件目录text.Text.ToLower();
                    string movepath = 装备导出目录text.Text;
                    progressBar1.Value = 10;//进度条进度增加
                    List<List<string>> Equ_Lst_alltext = new List<List<string>>();
                    List<List<string>> Obj_Lst_alltext = new List<List<string>>();
                    List<List<string>> Mob_Lst_alltext = new List<List<string>>();
                    List<List<string>> Apc_Lst_alltext = new List<List<string>>();
                    List<List<string>> Apd_Lst_alltext = new List<List<string>>();
                    List<List<string>> Cre_Lst_alltext = new List<List<string>>();

                    //读入装备lst列表内容
                    if (File.Exists(pvfpath + @"\equipment\equipment.lst"))
                    {
                        Equ_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\equipment\equipment.lst").Replace("/", @"\"));
                    }
                    //读入特效lst列表内容
                    if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst"))
                    {
                        Obj_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", @"\"));
                    }
                    //读入怪物lst列表内容
                    if (File.Exists(pvfpath + @"\monster\monster.lst"))
                    {
                        Mob_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", @"\"));
                    }
                    //读入人偶lst列表内容
                    if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst"))
                    {
                        Apc_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", @"\"));
                    }
                    //读入APDlst列表内容
                    if (File.Exists(pvfpath + @"\appendage\appendage.lst"))
                    {
                        Apd_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\appendage\appendage.lst").Replace("/", @"\"));
                    }

                    progressBar1.Value = 20;//进度条进度增加
                    List<string> Equ_Id = new List<string>();
                    List<string> Obj_Id = new List<string>();
                    List<string> Mob_Id = new List<string>();
                    List<string> Apc_Id = new List<string>();
                    List<string> Apd_Id = new List<string>();
                    List<string> Part_Set_Id = new List<string>();
                    List<string> Cre_Id = new List<string>();

                    List<string> Equ_Lst = new List<string>();
                    List<string> Obj_Lst = new List<string>();
                    List<string> Mob_Lst = new List<string>();
                    List<string> Apc_Lst = new List<string>();
                    List<string> Apd_Lst = new List<string>();
                    List<string> Cre_Lst = new List<string>();

                    List<string> Equ_Path = new List<string>();
                    List<string> Obj_Path = new List<string>();
                    List<string> Mob_Path = new List<string>();
                    List<string> Apc_Path = new List<string>();
                    List<string> Apd_Path = new List<string>();
                    List<string> Cre_Path = new List<string>();
                    List<string> Skl_Path = new List<string>();
                    List<string> Tbl_Path = new List<string>();
                    List<string> Act_Path = new List<string>();
                    List<string> Atk_Path = new List<string>();
                    List<string> Ani_Path = new List<string>();
                    List<string> Als_Path = new List<string>();
                    List<string> Ptl_Path = new List<string>();
                    List<string> Til_Path = new List<string>();
                    List<string> Key_Path = new List<string>();
                    List<string> Ai_Path = new List<string>();
                    List<string> Part_Set_Path = new List<string>();
                    List<string> Mob_Equ_Path = new List<string>();

                    List<string> End_All_Path = new List<string>();

                    StringBuilder Part_Set_XinXi = new StringBuilder();
                    StringBuilder End_File_Text = new StringBuilder();


                    //获取用户提供的装备编号
                    Equ_Id.AddRange(dnf.LstChongPaiLieGetId(装备编号text.Text));

                    //通过装备编号获取lst 跟全路径
                    dnf.Id_lst_编号查找(pvfpath + "\\equipment\\", Equ_Lst_alltext, Equ_Id, Equ_Lst, Equ_Path);

                    if (TiQuCre)
                    {
                        dnf.Equ宠物蛋寻找宠物Equ(Equ_Path, Equ_Id);
                        Equ_Lst.Clear();
                        Equ_Path.Clear();
                        dnf.Id_lst_编号查找(pvfpath + "\\equipment\\", Equ_Lst_alltext, Equ_Id, Equ_Lst, Equ_Path);
                    }
                    progressBar1.Value = 30;//进度条进度增加
                    //通过装备路径 找出apd、mob、apc、obj编号 以及ptl全路径 还有etc套装ID ani全路径
                    dnf.Equ寻找编号和路径(pvfpath, Equ_Path, Apd_Id, Mob_Id, Apc_Id, Obj_Id, Ptl_Path, Part_Set_Id, Ani_Path);

                    //通过APD ID寻找lst 跟全路径 以及APD文件内的ani路径
                    dnf.Id_lst_编号查找(pvfpath + "\\appendage\\", Apd_Lst_alltext, Apd_Id, Apd_Lst, Apd_Path);
                    //按时保留apd中的ani路径查找功能
                    //if (Apd_Path.Count>0)
                    //{
                    //    foreach (var item in Apd_Path)
                    //    {
                    //        if (File.Exists(item)==true)
                    //        {
                    //            string AllApdText = File.ReadAllText(item);
                    //            regex.创建(AllApdText, @"\[effect animation\]\r\n`(.+\.ani)`",1);

                    //        }
                    //    }
                    //}


                    //通过套装id查找 套装属性文件以及套装信息
                    dnf.寻找Etc套装编号路径(pvfpath, Part_Set_Id, Part_Set_Path, Part_Set_XinXi);
                    progressBar1.Value = 40;//进度条进度增加


                    if (TiQuCre)
                    {
                        //读入Crelst列表内容
                        if (File.Exists(pvfpath + @"\creature\creature.lst"))
                        {
                            Cre_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\creature\creature.lst").Replace("/", @"\"));
                        }

                        //通过装备路径获取宠物cre编号
                        dnf.Equ寻找Cre编号(Equ_Path, Cre_Id);

                        //通过宠物编号获取lst 跟全路径
                        dnf.Id_lst_编号查找(pvfpath + "\\creature\\", Cre_Lst_alltext, Cre_Id, Cre_Lst, Cre_Path);

                        //通过宠物cre路径获取所有有关路径
                        dnf.Cre寻找路径(Cre_Path, Ani_Path, Atk_Path, Skl_Path, Ptl_Path);

                        //通过宠物skl技能文件需找ani或obj编号、objptl路径
                        dnf.CreSkl寻找路径或编号(Skl_Path, Ani_Path, Ptl_Path, Obj_Id);

                    }
                    progressBar1.Value = 50;//进度条进度增加

                    //判断obj mob apc编号是否找到；找到就进入遍历循环
                    if (Obj_Id.Count > 0 || Mob_Id.Count > 0 || Apc_Id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;
                        //循环遍历出所有关联编号
                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = Obj_Id.Count;
                            mob_id_count = Mob_Id.Count;
                            apc_id_count = Apc_Id.Count;

                            dnf.怪物寻找编号(pvfpath + "\\monster\\", Mob_Lst_alltext, Mob_Id, Mob_Lst, Mob_Path, Obj_Id, Apc_Id, Act_Path, Ptl_Path);
                            dnf.特效寻找编号(pvfpath + "\\passiveobject\\", Obj_Lst_alltext, Obj_Id, Obj_Lst, Obj_Path, Mob_Id, Apc_Id, Act_Path, Ptl_Path);
                            dnf.人偶寻找编号(pvfpath + "\\aicharacter\\", Apc_Lst_alltext, Apc_Id, Apc_Lst, Apc_Path, Mob_Id, Obj_Id, Act_Path, Ptl_Path, Key_Path);

                        } while (obj_id_count != Obj_Id.Count || mob_id_count != Mob_Id.Count || apc_id_count != Apc_Id.Count);
                    }
                    progressBar1.Value = 60;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(pvfpath, Mob_Path, Tbl_Path, null, Atk_Path, Ani_Path, null, null, null, Ai_Path, Mob_Equ_Path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(pvfpath, Obj_Path, null, null, Atk_Path, Ani_Path, null, Til_Path, null, null, null);

                    //如果装备内有人偶就获取ai、key
                    if (Apc_Id.Count > 0)
                    {
                        dnf.寻找全路径(pvfpath, Apc_Path, null, null, null, null, null, null, Key_Path, Ai_Path, null);
                    }
                    progressBar1.Value = 70;//进度条进度增加
                    //获取act中的atk、ani
                    dnf.寻找全路径(pvfpath, Act_Path, null, null, Atk_Path, Ani_Path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(pvfpath, Ptl_Path, null, null, null, Ani_Path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(Ani_Path, Als_Path, 2, false);

                    //寻找ai
                    dnf.寻找ai(Ai_Path);
                    progressBar1.Value = 80;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    End_All_Path.AddRange(Equ_Path);
                    End_All_Path.AddRange(Obj_Path);
                    End_All_Path.AddRange(Mob_Path);
                    End_All_Path.AddRange(Apc_Path);
                    End_All_Path.AddRange(Apd_Path);
                    End_All_Path.AddRange(Cre_Path);
                    End_All_Path.AddRange(Skl_Path);
                    End_All_Path.AddRange(Tbl_Path);
                    End_All_Path.AddRange(Act_Path);
                    End_All_Path.AddRange(Atk_Path);
                    End_All_Path.AddRange(Ani_Path);
                    End_All_Path.AddRange(Als_Path);
                    End_All_Path.AddRange(Ptl_Path);
                    End_All_Path.AddRange(Til_Path);
                    End_All_Path.AddRange(Key_Path);
                    End_All_Path.AddRange(Ai_Path);
                    End_All_Path.AddRange(Part_Set_Path);
                    End_All_Path.AddRange(Mob_Equ_Path);

                    //lst信息加入字符串变量，并且准备输出
                    if (Equ_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【equipment/equipment.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Equ_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Equ_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到装备lst\r\n\r\n\r\n");
                    }

                    if (TiQuCre)
                    {
                        if (Cre_Lst.Count > 0)
                        {
                            End_File_Text.Append("----------------------------------以下内容加入PVF【creature/creature.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Cre_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                            foreach (string item in Cre_Lst)
                            {
                                End_File_Text.Append(item + "\r\n");
                            }
                            End_File_Text.Append("\r\n\r\n");
                        }
                        else
                        {
                            End_File_Text.Append("未找到宠物CreLst\r\n\r\n\r\n");
                        }
                    }

                    if (Obj_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Obj_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Obj_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到特效lst\r\n\r\n\r\n");
                    }

                    if (Mob_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Mob_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Mob_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到怪物lst\r\n\r\n\r\n");
                    }

                    if (Apc_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Apc_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Apc_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到人偶lst\r\n\r\n\r\n");
                    }

                    if (Apd_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【appendage/appendage.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Apd_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Apd_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到状态APDlst\r\n\r\n\r\n");
                    }

                    if (Part_Set_XinXi.Length > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【etc/equipmentpartset.etc】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Part_Set_Id.Count.ToString() + "】-------------------------------------------------------\r\n");

                        End_File_Text.Append(Part_Set_XinXi.ToString());
                    }
                    else
                    {
                        End_File_Text.Append("未找到etc套装编号\r\n\r\n\r\n");
                    }
                    progressBar1.Value = 85;//进度条进度增加
                    MysqlYz.XY(12);
                    //开始输出所有文件
                    foreach (string item in End_All_Path)
                    {
                        string endpath;
                        string endmovepath;
                        string moveDirectory;
                        //取出原文件所在全路径，并且转为小写
                        endpath = item.ToLower();

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (File.Exists(endpath) == true)
                        {
                            //转换为提取导出的全路径
                            endmovepath = endpath.Replace(pvfpath, movepath);
                            //转换为提取导出的父路径
                            moveDirectory = Directory.GetParent(endmovepath).ToString();
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.Copy(endpath, endmovepath, true);
                        }

                    }
                    progressBar1.Value = 90;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(1)查找出来的lst列表.txt", End_File_Text.ToString().Replace("\\", "/"));

                    //结束副本提取测量时间
                    Equtime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (MingWenImgTiQu == true)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(2)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        Imgtime = time.ElapsedMilliseconds;
                    }


                    if (MoXingImgTiQu)
                    {
                        time.Restart();
                        StringBuilder FanKuiCuoWu = new StringBuilder();
                        List<string> ImgPath = new List<string>();
                        dnf.EquXunZhaoImgPath(Equ_Path, ImgPath, FanKuiCuoWu);
                        if (ImgPath.Count > 0)
                        {
                            StringBuilder FileImgPathText = new StringBuilder();
                            FileImgPathText.Append("寻找出来的装备模型img总数为：" + ImgPath.Count.ToString() + "个\r\n\r\n");
                            foreach (string item in ImgPath)
                            {
                                FileImgPathText.Append(item + "\r\n");
                            }
                            File.WriteAllText(movepath + "\\(3)提取的装备模型img全路径.txt", FileImgPathText.ToString());
                        }
                        if (FanKuiCuoWu.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(4)模型图层错误的装备.txt", FanKuiCuoWu.ToString());
                        }

                        EquImgtime = time.ElapsedMilliseconds;
                    }

                    string Imgtimetext = "";
                    string Equimgtimetext = "";

                    if (Imgtime != 0)
                    {
                        Imgtimetext = "提取的明文img全路径总用时：" + (double)Imgtime / 1000 + "秒；\r\n";
                    }

                    if (EquImgtime != 0)
                    {
                        Equimgtimetext = "提取装备模型img总用时：" + (double)EquImgtime / 1000 + "秒；";
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取副本，判断img提取时间是否存在
                    MessageBox.Show("已完成指定的装备提取工作；\r\n提取装备总用时：" + (double)Equtime / 1000 + "秒；\r\n" + Imgtimetext + Equimgtimetext, "完成", MessageBoxButtons.OK, MessageBoxIcon.None);

                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        new DingWei().Explorer(movepath);
                    }

                    time.Stop();
                }
            }
            progressBar1.Visible = false;//进度条可视假

        }


        private void 装备开始在线提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已输入的编号或目录路径正确，并且开始提取装备？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoEqu_tq = false;

                //开始初始判断是否提取装备
                if (装备编号text.Text != "")
                {
                    if (装备导出目录text.Text != "")
                    {

                        if (Directory.Exists(装备导出目录text.Text))
                        {
                            MysqlYz.XY(12);
                            YesNoEqu_tq = true;
                        }
                        else
                        {
                            MessageBox.Show("所选equ装备提取并导出的目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            装备导出目录text.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        装备导出目录text.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("请输入需要提取equ编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    装备编号text.Focus();
                }

                if (YesNoEqu_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();
                    long Equtime;
                    long Imgtime = 0;
                    long EquImgtime = 0;

                    DnfHttp dnf = new DnfHttp();
                    bool MoXingImgTiQu = 装备提取模型checkBox.Checked;
                    bool MingWenImgTiQu = 装备查找img路径checkBox.Checked;
                    bool TiQuCre = 装备提取宠物crecheckBox.Checked;

                    string movepath = 装备导出目录text.Text;

                    List<List<string>> Equ_Lst_alltext = new List<List<string>>();
                    List<List<string>> Obj_Lst_alltext = new List<List<string>>();
                    List<List<string>> Mob_Lst_alltext = new List<List<string>>();
                    List<List<string>> Apc_Lst_alltext = new List<List<string>>();
                    List<List<string>> Apd_Lst_alltext = new List<List<string>>();
                    List<List<string>> Cre_Lst_alltext = new List<List<string>>();
                    progressBar1.Value = 10;//进度条进度增加
                    //读入装备lst列表内容
                    if (dnf.FileExists("equipment/equipment.lst"))
                    {
                        Equ_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("equipment/equipment.lst").Replace("\\", "/"));
                    }
                    //读入特效lst列表内容
                    if (dnf.FileExists("passiveobject/passiveobject.lst"))
                    {
                        Obj_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("passiveobject/passiveobject.lst").Replace("\\", "/"));
                    }
                    //读入怪物lst列表内容
                    if (dnf.FileExists("monster/monster.lst"))
                    {
                        Mob_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("monster/monster.lst").Replace("\\", "/"));
                    }
                    //读入人偶lst列表内容
                    if (dnf.FileExists("aicharacter/aicharacter.lst"))
                    {
                        Apc_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("aicharacter/aicharacter.lst").Replace("\\", "/"));
                    }
                    //读入APDlst列表内容
                    if (dnf.FileExists("appendage/appendage.lst"))
                    {
                        Apd_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("appendage/appendage.lst").Replace("\\", "/"));
                    }
                    progressBar1.Value = 20;//进度条进度增加

                    List<string> Equ_Id = new List<string>();
                    List<string> Obj_Id = new List<string>();
                    List<string> Mob_Id = new List<string>();
                    List<string> Apc_Id = new List<string>();
                    List<string> Apd_Id = new List<string>();
                    List<string> Part_Set_Id = new List<string>();
                    List<string> Cre_Id = new List<string>();

                    List<string> Equ_Lst = new List<string>();
                    List<string> Obj_Lst = new List<string>();
                    List<string> Mob_Lst = new List<string>();
                    List<string> Apc_Lst = new List<string>();
                    List<string> Apd_Lst = new List<string>();
                    List<string> Cre_Lst = new List<string>();

                    List<string> Equ_Path = new List<string>();
                    List<string> Obj_Path = new List<string>();
                    List<string> Mob_Path = new List<string>();
                    List<string> Apc_Path = new List<string>();
                    List<string> Apd_Path = new List<string>();
                    List<string> Cre_Path = new List<string>();
                    List<string> Skl_Path = new List<string>();
                    List<string> Tbl_Path = new List<string>();
                    List<string> Act_Path = new List<string>();
                    List<string> Atk_Path = new List<string>();
                    List<string> Ani_Path = new List<string>();
                    List<string> Als_Path = new List<string>();
                    List<string> Ptl_Path = new List<string>();
                    List<string> Til_Path = new List<string>();
                    List<string> Key_Path = new List<string>();
                    List<string> Ai_Path = new List<string>();
                    List<string> Part_Set_Path = new List<string>();
                    List<string> Mob_Equ_Path = new List<string>();

                    List<string> End_All_Path = new List<string>();

                    StringBuilder Part_Set_XinXi = new StringBuilder();
                    StringBuilder End_File_Text = new StringBuilder();


                    //获取用户提供的装备编号
                    Equ_Id.AddRange(dnf.LstChongPaiLieGetId(装备编号text.Text));

                    //通过装备编号获取lst 跟全路径
                    dnf.Id_lst_编号查找("equipment/", Equ_Lst_alltext, Equ_Id, Equ_Lst, Equ_Path);

                    if (TiQuCre)
                    {
                        dnf.Equ宠物蛋寻找宠物Equ(Equ_Path, Equ_Id);
                        Equ_Lst.Clear();
                        Equ_Path.Clear();
                        dnf.Id_lst_编号查找("equipment/", Equ_Lst_alltext, Equ_Id, Equ_Lst, Equ_Path);
                    }
                    progressBar1.Value = 30;//进度条进度增加
                    //通过装备路径 找出apd、mob、apc、obj编号 以及ptl全路径 还有etc套装ID ani全路径
                    dnf.Equ寻找编号和路径(Equ_Path, Apd_Id, Mob_Id, Apc_Id, Obj_Id, Ptl_Path, Part_Set_Id, Ani_Path);

                    //通过APD ID寻找lst 跟全路径 以及APD文件内的ani路径
                    dnf.Id_lst_编号查找("appendage/", Apd_Lst_alltext, Apd_Id, Apd_Lst, Apd_Path);
                    //按时保留apd中的ani路径查找功能
                    //if (Apd_Path.Count>0)
                    //{
                    //    foreach (var item in Apd_Path)
                    //    {
                    //        if (File.Exists(item)==true)
                    //        {
                    //            string AllApdText = File.ReadAllText(item);
                    //            regex.创建(AllApdText, @"\[effect animation\]\r\n`(.+\.ani)`",1);

                    //        }
                    //    }
                    //}


                    //通过套装id查找 套装属性文件以及套装信息
                    dnf.寻找Etc套装编号路径(Part_Set_Id, Part_Set_Path, Part_Set_XinXi);

                    progressBar1.Value = 40;//进度条进度增加
                    if (TiQuCre)
                    {
                        //读入Crelst列表内容
                        if (dnf.FileExists("creature/creature.lst"))
                        {
                            Cre_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("creature/creature.lst").Replace("\\", "/"));
                        }

                        //通过装备路径获取宠物cre编号
                        dnf.Equ寻找Cre编号(Equ_Path, Cre_Id);

                        //通过宠物编号获取lst 跟全路径
                        dnf.Id_lst_编号查找("creature/", Cre_Lst_alltext, Cre_Id, Cre_Lst, Cre_Path);

                        //通过宠物cre路径获取所有有关路径
                        dnf.Cre寻找路径(Cre_Path, Ani_Path, Atk_Path, Skl_Path, Ptl_Path);

                        //通过宠物skl技能文件需找ani或obj编号、objptl路径
                        dnf.CreSkl寻找路径或编号(Skl_Path, Ani_Path, Ptl_Path, Obj_Id);

                    }

                    progressBar1.Value = 50;//进度条进度增加
                    //判断obj mob apc编号是否找到；找到就进入遍历循环
                    if (Obj_Id.Count > 0 || Mob_Id.Count > 0 || Apc_Id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;
                        //循环遍历出所有关联编号
                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = Obj_Id.Count;
                            mob_id_count = Mob_Id.Count;
                            apc_id_count = Apc_Id.Count;

                            dnf.怪物寻找编号(Mob_Lst_alltext, Mob_Id, Mob_Lst, Mob_Path, Obj_Id, Apc_Id, Act_Path, Ptl_Path);
                            dnf.特效寻找编号(Obj_Lst_alltext, Obj_Id, Obj_Lst, Obj_Path, Mob_Id, Apc_Id, Act_Path, Ptl_Path);
                            dnf.人偶寻找编号(Apc_Lst_alltext, Apc_Id, Apc_Lst, Apc_Path, Mob_Id, Obj_Id, Act_Path, Ptl_Path, Key_Path);

                        } while (obj_id_count != Obj_Id.Count || mob_id_count != Mob_Id.Count || apc_id_count != Apc_Id.Count);
                    }
                    progressBar1.Value = 60;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(Mob_Path, Tbl_Path, null, Atk_Path, Ani_Path, null, null, null, Ai_Path, Mob_Equ_Path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(Obj_Path, null, null, Atk_Path, Ani_Path, null, Til_Path, null, null, null);

                    //如果装备内有人偶就获取ai、key
                    if (Apc_Id.Count > 0)
                    {
                        dnf.寻找全路径(Apc_Path, null, null, null, null, null, null, Key_Path, Ai_Path, null);
                    }

                    //获取act中的atk、ani
                    dnf.寻找全路径(Act_Path, null, null, Atk_Path, Ani_Path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(Ptl_Path, null, null, null, Ani_Path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(Ani_Path, Als_Path, 2, false);

                    //寻找ai
                    dnf.寻找ai(Ai_Path);
                    progressBar1.Value = 70;//进度条进度增加
                    //合并到单独输出集合,并且准备输出
                    End_All_Path.AddRange(Equ_Path);
                    End_All_Path.AddRange(Obj_Path);
                    End_All_Path.AddRange(Mob_Path);
                    End_All_Path.AddRange(Apc_Path);
                    End_All_Path.AddRange(Apd_Path);
                    End_All_Path.AddRange(Cre_Path);
                    End_All_Path.AddRange(Skl_Path);
                    End_All_Path.AddRange(Tbl_Path);
                    End_All_Path.AddRange(Act_Path);
                    End_All_Path.AddRange(Atk_Path);
                    End_All_Path.AddRange(Ani_Path);
                    End_All_Path.AddRange(Als_Path);
                    End_All_Path.AddRange(Ptl_Path);
                    End_All_Path.AddRange(Til_Path);
                    End_All_Path.AddRange(Key_Path);
                    End_All_Path.AddRange(Ai_Path);
                    End_All_Path.AddRange(Part_Set_Path);
                    End_All_Path.AddRange(Mob_Equ_Path);

                    //lst信息加入字符串变量，并且准备输出
                    if (Equ_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【equipment/equipment.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Equ_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Equ_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到装备lst\r\n\r\n\r\n");
                    }

                    if (TiQuCre)
                    {
                        if (Cre_Lst.Count > 0)
                        {
                            End_File_Text.Append("----------------------------------以下内容加入PVF【creature/creature.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Cre_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                            foreach (string item in Cre_Lst)
                            {
                                End_File_Text.Append(item + "\r\n");
                            }
                            End_File_Text.Append("\r\n\r\n");
                        }
                        else
                        {
                            End_File_Text.Append("未找到宠物CreLst\r\n\r\n\r\n");
                        }
                    }

                    if (Obj_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Obj_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Obj_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到特效lst\r\n\r\n\r\n");
                    }

                    if (Mob_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Mob_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Mob_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到怪物lst\r\n\r\n\r\n");
                    }

                    if (Apc_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Apc_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Apc_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到人偶lst\r\n\r\n\r\n");
                    }

                    if (Apd_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【appendage/appendage.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Apd_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Apd_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到状态APDlst\r\n\r\n\r\n");
                    }

                    if (Part_Set_XinXi.Length > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【etc/equipmentpartset.etc】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Part_Set_Id.Count.ToString() + "】-------------------------------------------------------\r\n");

                        End_File_Text.Append(Part_Set_XinXi.ToString());
                    }
                    else
                    {
                        End_File_Text.Append("未找到etc套装编号\r\n\r\n\r\n");
                    }

                    progressBar1.Value = 80;//进度条进度增加
                    MysqlYz.XY(12);
                    //开始输出所有文件
                    foreach (string item in End_All_Path)
                    {
                        string endmovepath;
                        string moveDirectory;

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (dnf.FileExists(item))
                        {
                            //转换为提取导出的全路径
                            endmovepath = movepath + "/" + item;
                            //转换为提取导出的父路径
                            moveDirectory = dnf.DirectoryGetParent(endmovepath);
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                        }

                    }

                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(1)查找出来的lst列表.txt", End_File_Text.ToString().Replace("\\", "/"));
                    progressBar1.Value = 90;//进度条进度增加
                    //结束副本提取测量时间
                    Equtime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (MingWenImgTiQu)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(2)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        Imgtime = time.ElapsedMilliseconds;
                    }


                    if (MoXingImgTiQu)
                    {
                        time.Restart();
                        StringBuilder FanKuiCuoWu = new StringBuilder();
                        List<string> ImgPath = new List<string>();
                        dnf.EquXunZhaoImgPath(movepath, Equ_Path, ImgPath, FanKuiCuoWu);
                        if (ImgPath.Count > 0)
                        {
                            StringBuilder FileImgPathText = new StringBuilder();
                            FileImgPathText.Append("寻找出来的装备模型img总数为：" + ImgPath.Count.ToString() + "个\r\n\r\n");
                            foreach (string item in ImgPath)
                            {
                                FileImgPathText.Append(item + "\r\n");
                            }
                            File.WriteAllText(movepath + "\\(3)提取的装备模型img全路径.txt", FileImgPathText.ToString());
                        }
                        if (FanKuiCuoWu.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(4)模型图层错误的装备.txt", FanKuiCuoWu.ToString());
                        }

                        EquImgtime = time.ElapsedMilliseconds;
                    }

                    string Imgtimetext = "";
                    string Equimgtimetext = "";

                    if (Imgtime != 0)
                    {
                        Imgtimetext = "提取的明文img全路径总用时：" + (double)Imgtime / 1000 + "秒；\r\n";
                    }

                    if (EquImgtime != 0)
                    {
                        Equimgtimetext = "提取装备模型img总用时：" + (double)EquImgtime / 1000 + "秒；";
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取副本，判断img提取时间是否存在
                    MessageBox.Show("已完成指定的装备提取工作；\r\n提取装备总用时：" + (double)Equtime / 1000 + "秒；\r\n" + Imgtimetext + Equimgtimetext, "完成", MessageBoxButtons.OK, MessageBoxIcon.None);

                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        new DingWei().Explorer(movepath);
                    }

                    time.Stop();
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        //——————————————装备提取功能区————————————————


        //——————————————礼包提取功能区————————————————


        private void 寻找礼包记录信息button_Click(object sender, EventArgs e)
        {
            Regex_new regex = new Regex_new();
            StkLiBaoInfoForm = new 导出礼包查询代码();
            StkLiBaoInfoForm.richTextBox2.Text = JieShouStkIdText;
            StkLiBaoInfoForm.richTextBox3.Text = JieShouEquIdText;
            regex.创建(JieShouStkIdText, "\r\n", 1);
            if (regex.取匹配数量() > 0)
            {
                StkLiBaoInfoForm.label1.Text = "stk代码:" + regex.取匹配数量() + "个";
            }
            else
            {
                StkLiBaoInfoForm.label1.Text = "stk代码:0个";
            }

            regex.创建(JieShouEquIdText, "\r\n", 1);
            if (regex.取匹配数量() > 0)
            {
                StkLiBaoInfoForm.label2.Text = "equ代码:" + regex.取匹配数量() + "个";
            }
            else
            {
                StkLiBaoInfoForm.label2.Text = "equ代码:0个";
            }
            StkLiBaoInfoForm.Show();
        }

        private void 寻找礼包编号button_Click(object sender, EventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出礼包编号";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                寻找礼包编号textmaskedTextBox.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void 寻找礼包pvf所在目录textmaskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//获得全局目录
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                寻找礼包pvf所在目录textmaskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//路径加入到全局变量
            }
        }

        private void 寻找礼包使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地寻找>\r\n" +
                "按照使用要求导出指定目录例如：\r\n" +
                "只提取礼包内的stk代码则只需导出：stackable目录\r\n" +
                "只提取礼包内的equ代码则只需导出：equipment目录\r\n" +
                "如果stk、equ代码都要提取则两个目录都需要导出\r\n\r\n" +
                "<在线寻找>\r\n则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号即可";
            MessageBox.Show(Text, "寻找礼包内代码使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 寻找礼包开始本地寻找button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已输入的编号或目录路径正确，并且开始寻找指定的编号？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoId_tq = false;

                //开始初始判断是否寻找编号
                if (寻找礼包编号textmaskedTextBox.Text != "")
                {
                    if (寻找礼包pvf所在目录textmaskedTextBox.Text != "")
                    {

                        if (Directory.Exists(寻找礼包pvf所在目录textmaskedTextBox.Text))
                        {
                            MysqlYz.XY(13);
                            YesNoId_tq = true;
                        }
                        else
                        {
                            MessageBox.Show("所选pvf文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            寻找礼包pvf所在目录textmaskedTextBox.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入pvf文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        寻找礼包pvf所在目录textmaskedTextBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要寻找的礼包stk编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    寻找礼包编号textmaskedTextBox.Focus();
                }

                if (YesNoId_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    Regex_new regex = new Regex_new();
                    Dnf dnf = new Dnf();

                    bool YesNoEquId = 寻找礼包equ代码checkBox.Checked;
                    bool YesNoStkId = 寻找礼包stk代码checkBox.Checked;
                    bool YesNoShenCeng = 寻找礼包深层搜索checkBox.Checked;
                    int YesNoDaoChuStk = 0;
                    if (寻找礼包不导出radioButton.Checked)
                    {
                        YesNoDaoChuStk = 0;
                    }
                    else if (寻找礼包导出礼包radioButton.Checked)
                    {
                        YesNoDaoChuStk = 1;
                    }
                    else if (寻找礼包所有radioButton.Checked)
                    {
                        YesNoDaoChuStk = 2;
                    }

                    string PvfPath = 寻找礼包pvf所在目录textmaskedTextBox.Text.ToLower();
                    progressBar1.Value = 10;//进度条进度增加
                    StringBuilder EquIdInfo = new StringBuilder();
                    StringBuilder StkIdInfo = new StringBuilder();

                    List<List<string>> EquLstAllText = new List<List<string>>();
                    List<List<string>> StkLstAllText = new List<List<string>>();

                    List<string> StkId = new List<string>();
                    List<string> StkLst = new List<string>();
                    List<string> StkPath = new List<string>();

                    List<string> New_StkLst = new List<string>();
                    List<string> New_StkPath = new List<string>();

                    List<string> LinShiId = new List<string>();

                    progressBar1.Value = 20;//进度条进度增加
                    if (File.Exists(PvfPath + "\\stackable\\stackable.lst"))
                    {
                        StkLstAllText = dnf.LstChongPaiLie(File.ReadAllText(PvfPath + "\\stackable\\stackable.lst"));
                    }
                    if (File.Exists(PvfPath + "\\equipment\\equipment.lst"))
                    {
                        EquLstAllText = dnf.LstChongPaiLie(File.ReadAllText(PvfPath + "\\equipment\\equipment.lst"));
                    }


                    regex.创建(寻找礼包编号textmaskedTextBox.Text, "[0-9]+", 1);
                    if (regex.取匹配数量() > 0)
                    {
                        regex.匹配加入集合(StkId, false, 0);
                    }

                    progressBar1.Value = 30;//进度条进度增加
                    if (YesNoShenCeng)
                    {
                        dnf.Stk礼包循环遍历StkId(PvfPath + "\\stackable\\", StkId, StkLstAllText);
                        dnf.Id_lst_编号查找(PvfPath + "\\stackable\\", StkLstAllText, StkId, StkLst, StkPath);
                        dnf.Stk礼包获取AllId(StkPath, LinShiId);
                    }
                    else
                    {
                        dnf.Id_lst_编号查找(PvfPath + "\\stackable\\", StkLstAllText, StkId, StkLst, StkPath);
                        dnf.Stk礼包获取AllId(StkPath, LinShiId);
                    }
                    progressBar1.Value = 50;//进度条进度增加
                    if (YesNoEquId && EquLstAllText.Count > 0)
                        dnf.IdBianBieType(PvfPath + "\\equipment\\", LinShiId, EquLstAllText, EquIdInfo);

                    progressBar1.Value = 60;//进度条进度增加
                    if (YesNoStkId && StkLstAllText.Count > 0)
                        dnf.IdBianBieType(PvfPath + "\\stackable\\", LinShiId, StkLstAllText, StkIdInfo, New_StkLst, New_StkPath);

                    progressBar1.Value = 70;//进度条进度增加
                    MysqlYz.XY(13);

                    if (YesNoDaoChuStk > 0)
                    {
                        using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
                        {
                            InitialDirectory = ApiDirectoryPath,//获得全局目录
                            IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                            Title = @"请选择导出礼包文件的目录；例如：E:\DNF\导出礼包文件的目录"//设置标题
                        };
                        if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
                        {
                            if (YesNoDaoChuStk == 2)
                                StkPath.AddRange(New_StkPath);

                            //开始输出所有文件
                            foreach (string item in StkPath)
                            {
                                string endpath;
                                string endmovepath;
                                string moveDirectory;
                                //取出原文件所在全路径，并且转为小写
                                endpath = item.ToLower();

                                //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                                if (File.Exists(endpath))
                                {
                                    //转换为提取导出的全路径
                                    endmovepath = endpath.Replace(PvfPath, file_folder.FileName);
                                    //转换为提取导出的父路径
                                    moveDirectory = Directory.GetParent(endmovepath).ToString();
                                    //创建提取导出的父路径所在目录
                                    Directory.CreateDirectory(moveDirectory);
                                    //复制文件到指定目录，如果有同名文件就覆盖
                                    File.Copy(endpath, endmovepath, true);
                                }
                            }
                            StringBuilder End_File_Text = new StringBuilder();

                            if (StkLst.Count > 0)
                            {
                                End_File_Text.Append("----------------------------------以下内容加入PVF【stackable/stackable.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + StkLst.Count.ToString() + "】-------------------------------------------------------\r\n");
                                foreach (string item in StkLst)
                                {
                                    End_File_Text.Append(item + "\r\n");
                                }
                                End_File_Text.Append("\r\n\r\n");

                                if (YesNoDaoChuStk == 2)
                                {
                                    End_File_Text.Append(
                                        "----------------------------------其余的消耗品lst----------------------------------\r\n---------------------------------------------------以下总数为【" + New_StkLst.Count.ToString() + "】-------------------------------------------------------\r\n");
                                    New_StkLst.ForEach((Text) =>
                                    {
                                        End_File_Text.Append(Text + "\r\n");
                                    });
                                    End_File_Text.Append("\r\n\r\n");
                                }
                            }
                            else
                            {
                                End_File_Text.Append("未找到消耗品礼包lst\r\n\r\n\r\n");
                            }

                            File.WriteAllText(file_folder.FileName + "\\(1)查找出来的lst列表.txt", End_File_Text.ToString());

                        }
                    }
                    progressBar1.Value = 80;//进度条进度增加
                    if (EquIdInfo.Length > 0 || StkIdInfo.Length > 0)
                    {
                        StkLiBaoInfoForm = new 导出礼包查询代码();
                        JieShouStkIdText = StkIdInfo.ToString();
                        JieShouEquIdText = EquIdInfo.ToString();
                        StkLiBaoInfoForm.richTextBox2.Text = JieShouStkIdText;
                        StkLiBaoInfoForm.richTextBox3.Text = JieShouEquIdText;
                        regex.创建(JieShouStkIdText, "\r\n", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            StkLiBaoInfoForm.label1.Text = "stk代码:" + regex.取匹配数量() + "个";
                        }
                        else
                        {
                            StkLiBaoInfoForm.label1.Text = "stk代码:0个";
                        }

                        regex.创建(JieShouEquIdText, "\r\n", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            StkLiBaoInfoForm.label2.Text = "equ代码:" + regex.取匹配数量() + "个";
                        }
                        else
                        {
                            StkLiBaoInfoForm.label2.Text = "equ代码:0个";
                        }

                        StkLiBaoInfoForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("抱歉未找出equ或stk代码信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取副本，判断img提取时间是否存在
                    MessageBox.Show("已完成寻找编号的工作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        private void 寻找礼包开始在线寻找button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已输入的编号或目录路径正确，并且开始寻找指定的编号？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoId_tq = false;

                //开始初始判断是否寻找编号
                if (寻找礼包编号textmaskedTextBox.Text != "")
                {
                    MysqlYz.XY(13);
                    YesNoId_tq = true;
                }
                else
                {
                    MessageBox.Show("请输入需要寻找的礼包stk编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    寻找礼包编号textmaskedTextBox.Focus();
                }

                if (YesNoId_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    Regex_new regex = new Regex_new();
                    DnfHttp dnf = new DnfHttp();

                    bool YesNoEquId = 寻找礼包equ代码checkBox.Checked;
                    bool YesNoStkId = 寻找礼包stk代码checkBox.Checked;
                    bool YesNoShenCeng = 寻找礼包深层搜索checkBox.Checked;
                    int YesNoDaoChuStk = 0;
                    if (寻找礼包不导出radioButton.Checked)
                    {
                        YesNoDaoChuStk = 0;
                    }
                    else if (寻找礼包导出礼包radioButton.Checked)
                    {
                        YesNoDaoChuStk = 1;
                    }
                    else if (寻找礼包所有radioButton.Checked)
                    {
                        YesNoDaoChuStk = 2;
                    }

                    progressBar1.Value = 10;//进度条进度增加
                    StringBuilder EquIdInfo = new StringBuilder();
                    StringBuilder StkIdInfo = new StringBuilder();

                    List<List<string>> EquLstAllText = new List<List<string>>();
                    List<List<string>> StkLstAllText = new List<List<string>>();

                    List<string> StkId = new List<string>();
                    List<string> StkLst = new List<string>();
                    List<string> StkPath = new List<string>();

                    List<string> New_StkLst = new List<string>();
                    List<string> New_StkPath = new List<string>();

                    List<string> LinShiId = new List<string>();
                    progressBar1.Value = 20;//进度条进度增加

                    if (dnf.FileExists("stackable/stackable.lst"))
                    {
                        StkLstAllText = dnf.LstChongPaiLie(dnf.FileReadAllText("stackable/stackable.lst"));
                    }
                    if (dnf.FileExists("equipment/equipment.lst"))
                    {
                        EquLstAllText = dnf.LstChongPaiLie(dnf.FileReadAllText("equipment/equipment.lst"));
                    }
                    progressBar1.Value = 30;//进度条进度增加

                    regex.创建(寻找礼包编号textmaskedTextBox.Text, "[0-9]+", 1);
                    if (regex.取匹配数量() > 0)
                    {
                        regex.匹配加入集合(StkId, false, 0);
                    }
                    progressBar1.Value = 40;//进度条进度增加

                    if (YesNoShenCeng)
                    {
                        dnf.Stk礼包循环遍历StkId("stackable/", StkId, StkLstAllText);
                        dnf.Id_lst_编号查找("stackable/", StkLstAllText, StkId, StkLst, StkPath);
                        dnf.Stk礼包获取AllId(StkPath, LinShiId);
                    }
                    else
                    {
                        dnf.Id_lst_编号查找("stackable/", StkLstAllText, StkId, StkLst, StkPath);
                        dnf.Stk礼包获取AllId(StkPath, LinShiId);
                    }
                    progressBar1.Value = 50;//进度条进度增加
                    if (YesNoEquId && EquLstAllText.Count > 0)
                    {
                        dnf.IdBianBieType("equipment/", LinShiId, EquLstAllText, EquIdInfo);
                    }
                    progressBar1.Value = 60;//进度条进度增加
                    if (YesNoStkId && StkLstAllText.Count > 0)
                    {
                        dnf.IdBianBieType("stackable/", LinShiId, StkLstAllText, StkIdInfo, New_StkLst, New_StkPath);
                    }
                    MysqlYz.XY(13);
                    progressBar1.Value = 70;//进度条进度增加
                    if (YesNoDaoChuStk > 0)
                    {
                        using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
                        {
                            InitialDirectory = ApiDirectoryPath,//获得全局目录
                            IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                            Title = @"请选择导出礼包文件的目录；例如：E:\DNF\导出礼包文件的目录"//设置标题
                        };
                        if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
                        {
                            if (YesNoDaoChuStk == 2)
                                StkPath.AddRange(New_StkPath);

                            //开始输出所有文件
                            foreach (string item in StkPath)
                            {
                                string endmovepath;
                                string moveDirectory;

                                //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                                if (dnf.FileExists(item))
                                {
                                    //转换为提取导出的全路径
                                    endmovepath = file_folder.FileName + "/" + item;
                                    //转换为提取导出的父路径
                                    moveDirectory = dnf.DirectoryGetParent(endmovepath);
                                    //创建提取导出的父路径所在目录
                                    Directory.CreateDirectory(moveDirectory);
                                    //复制文件到指定目录，如果有同名文件就覆盖
                                    File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                                }
                            }
                            StringBuilder End_File_Text = new StringBuilder();

                            if (StkLst.Count > 0)
                            {
                                End_File_Text.Append("----------------------------------以下内容加入PVF【stackable/stackable.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + StkLst.Count.ToString() + "】-------------------------------------------------------\r\n");
                                foreach (string item in StkLst)
                                {
                                    End_File_Text.Append(item + "\r\n");
                                }
                                End_File_Text.Append("\r\n\r\n");

                                if (YesNoDaoChuStk == 2)
                                {
                                    End_File_Text.Append(
                                        "----------------------------------其余的消耗品lst----------------------------------\r\n---------------------------------------------------以下总数为【" + New_StkLst.Count.ToString() + "】-------------------------------------------------------\r\n");
                                    New_StkLst.ForEach((Text) =>
                                    {
                                        End_File_Text.Append(Text + "\r\n");
                                    });
                                    End_File_Text.Append("\r\n\r\n");
                                }
                            }
                            else
                            {
                                End_File_Text.Append("未找到消耗品礼包lst\r\n\r\n\r\n");
                            }

                            File.WriteAllText(file_folder.FileName + "\\(1)查找出来的lst列表.txt", End_File_Text.ToString());

                        }
                    }
                    progressBar1.Value = 80;//进度条进度增加
                    if (EquIdInfo.Length > 0 || StkIdInfo.Length > 0)
                    {
                        StkLiBaoInfoForm = new 导出礼包查询代码();
                        JieShouStkIdText = StkIdInfo.ToString();
                        JieShouEquIdText = EquIdInfo.ToString();
                        StkLiBaoInfoForm.richTextBox2.Text = JieShouStkIdText;
                        StkLiBaoInfoForm.richTextBox3.Text = JieShouEquIdText;
                        regex.创建(JieShouStkIdText, "\r\n", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            StkLiBaoInfoForm.label1.Text = "stk代码:" + regex.取匹配数量() + "个";
                        }
                        else
                        {
                            StkLiBaoInfoForm.label1.Text = "stk代码:0个";
                        }

                        regex.创建(JieShouEquIdText, "\r\n", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            StkLiBaoInfoForm.label2.Text = "equ代码:" + regex.取匹配数量() + "个";
                        }
                        else
                        {
                            StkLiBaoInfoForm.label2.Text = "equ代码:0个";
                        }

                        StkLiBaoInfoForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("抱歉未找出equ或stk代码信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取副本，判断img提取时间是否存在
                    MessageBox.Show("已完成寻找编号的工作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        //——————————————礼包提取功能区————————————————


        //——————————————附魔宝珠提取功能区————————————————
        private void 提取宝珠编号button_Click(object sender, EventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出宝珠编号";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                提取宝珠编号textmaskedTextBox.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void 提取宝珠pvf所在目录textmaskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//获得全局目录
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                提取宝珠pvf所在目录textmaskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//路径加入到全局变量
            }
        }

        private void 提取宝珠导出目录textmaskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//获得全局目录
                IsFolderPicker = true,//设置不能选择文件，可以选择文件夹
                Title = @"请选择提取后导出的目录；例如：E:\DNF\提取并导出目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                提取宝珠导出目录textmaskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//路径加入到全局变量
            }
        }

        private void 提取宝珠使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "本工具提取只支持pvfUtility303版本；\r\n" +
                "pvfUtility2018.8、pvfUtility2020版本还需在设置中开启兼容模式\r\n\r\n\r\n" +
                "<本地提取>为了确保文件提取的完整性，请导出以下目录：\r\n" +
                "stackable\r\n\r\n" +
                "<在线提取>\r\n" +
                "则不用导出任何目录；只需使用pvfUtility2020版本；\r\n" +
                "并且填写需要导出的编号和目录即可";
            MessageBox.Show(Text, "宝珠提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 提取宝珠开始本地提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已输入的编号或目录路径正确，并且开始提取宝珠？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoBaoZhu_tq = false;

                //开始初始判断是否提取装备
                if (提取宝珠编号textmaskedTextBox.Text != "")
                {
                    if (提取宝珠pvf所在目录textmaskedTextBox.Text != "")
                    {

                        if (Directory.Exists(提取宝珠pvf所在目录textmaskedTextBox.Text))
                        {

                            if (提取宝珠导出目录textmaskedTextBox.Text != "")
                            {

                                if (Directory.Exists(提取宝珠导出目录textmaskedTextBox.Text))
                                {
                                    MysqlYz.XY(14);
                                    YesNoBaoZhu_tq = true;
                                }
                                else
                                {
                                    MessageBox.Show("所选stk宝珠提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    提取宝珠导出目录textmaskedTextBox.Focus();
                                }

                            }
                            else
                            {
                                MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                提取宝珠导出目录textmaskedTextBox.Focus();
                            }

                        }
                        else
                        {
                            MessageBox.Show("所选pvf文件所在目录不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            提取宝珠pvf所在目录textmaskedTextBox.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入pvf文件所在目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        提取宝珠pvf所在目录textmaskedTextBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("请输入需要提取stk宝珠编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    提取宝珠编号textmaskedTextBox.Focus();
                }

                if (YesNoBaoZhu_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long StkBaoZhutime;
                    long Imgtime = 0;

                    Dnf dnf = new Dnf();

                    bool YesNpImgTiQu = 提取宝珠寻找img路径checkBox.Checked;
                    string pvfpath = 提取宝珠pvf所在目录textmaskedTextBox.Text.ToLower();
                    string movepath = 提取宝珠导出目录textmaskedTextBox.Text;
                    progressBar1.Value = 10;//进度条进度增加
                    List<List<string>> Stk_Lst_alltext = new List<List<string>>();
                    List<List<string>> Obj_Lst_alltext = new List<List<string>>();
                    List<List<string>> Mob_Lst_alltext = new List<List<string>>();
                    List<List<string>> Apc_Lst_alltext = new List<List<string>>();
                    List<List<string>> Apd_Lst_alltext = new List<List<string>>();

                    //读入装备lst列表内容
                    if (File.Exists(pvfpath + @"\stackable\stackable.lst") == true)
                    {
                        Stk_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\stackable\stackable.lst").Replace("/", @"\"));
                    }
                    //读入特效lst列表内容
                    if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst") == true)
                    {
                        Obj_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", @"\"));
                    }
                    //读入怪物lst列表内容
                    if (File.Exists(pvfpath + @"\monster\monster.lst") == true)
                    {
                        Mob_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", @"\"));
                    }
                    //读入人偶lst列表内容
                    if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst") == true)
                    {
                        Apc_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", @"\"));
                    }
                    //读入APDlst列表内容
                    if (File.Exists(pvfpath + @"\appendage\appendage.lst") == true)
                    {
                        Apd_Lst_alltext = dnf.LstChongPaiLie(File.ReadAllText(pvfpath + @"\appendage\appendage.lst").Replace("/", @"\"));
                    }
                    progressBar1.Value = 20;//进度条进度增加

                    List<string> Stk_Id = new List<string>();
                    List<string> Obj_Id = new List<string>();
                    List<string> Mob_Id = new List<string>();
                    List<string> Apc_Id = new List<string>();
                    List<string> Apd_Id = new List<string>();

                    List<string> Stk_Lst = new List<string>();
                    List<string> Obj_Lst = new List<string>();
                    List<string> Mob_Lst = new List<string>();
                    List<string> Apc_Lst = new List<string>();
                    List<string> Apd_Lst = new List<string>();

                    List<string> Stk_Path = new List<string>();
                    List<string> Obj_Path = new List<string>();
                    List<string> Mob_Path = new List<string>();
                    List<string> Apc_Path = new List<string>();
                    List<string> Apd_Path = new List<string>();
                    List<string> Tbl_Path = new List<string>();
                    List<string> Act_Path = new List<string>();
                    List<string> Atk_Path = new List<string>();
                    List<string> Ani_Path = new List<string>();
                    List<string> Als_Path = new List<string>();
                    List<string> Ptl_Path = new List<string>();
                    List<string> Til_Path = new List<string>();
                    List<string> Key_Path = new List<string>();
                    List<string> Ai_Path = new List<string>();
                    List<string> Mob_Equ_Path = new List<string>();

                    List<string> End_All_Path = new List<string>();

                    StringBuilder End_File_Text = new StringBuilder();

                    progressBar1.Value = 30;//进度条进度增加
                    //获取用户提供的宝珠编号
                    Stk_Id.AddRange(dnf.LstChongPaiLieGetId(提取宝珠编号textmaskedTextBox.Text));


                    //通过宝珠编号获取全路径
                    dnf.Id_lst_编号查找(pvfpath + "\\stackable\\", Stk_Lst_alltext, Stk_Id, null, Stk_Path);


                    //通过宝珠文件路径获取卡片路径
                    dnf.Stk宝珠寻找卡片Id(Stk_Path, Stk_Id);


                    //通过宝珠和卡片编号获取lst跟全路径
                    dnf.Id_lst_编号查找(pvfpath + "\\stackable\\", Stk_Lst_alltext, Stk_Id, Stk_Lst, Stk_Path);
                    progressBar1.Value = 40;//进度条进度增加

                    //通过装备路径 找出apd、mob、apc、obj编号 以及ptl全路径 还有etc套装ID ani全路径
                    dnf.Equ寻找编号和路径(pvfpath, Stk_Path, Apd_Id, Mob_Id, Apc_Id, Obj_Id, Ptl_Path, null, Ani_Path);


                    //通过APD ID寻找lst 跟全路径 以及APD文件内的ani路径
                    dnf.Id_lst_编号查找(pvfpath + "\\appendage\\", Apd_Lst_alltext, Apd_Id, Apd_Lst, Apd_Path);

                    progressBar1.Value = 50;//进度条进度增加
                    //判断obj mob apc编号是否找到；找到就进入遍历循环
                    if (Obj_Id.Count > 0 || Mob_Id.Count > 0 || Apc_Id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;
                        //循环遍历出所有关联编号
                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = Obj_Id.Count;
                            mob_id_count = Mob_Id.Count;
                            apc_id_count = Apc_Id.Count;

                            dnf.怪物寻找编号(pvfpath + "\\monster\\", Mob_Lst_alltext, Mob_Id, Mob_Lst, Mob_Path, Obj_Id, Apc_Id, Act_Path, Ptl_Path);
                            dnf.特效寻找编号(pvfpath + "\\passiveobject\\", Obj_Lst_alltext, Obj_Id, Obj_Lst, Obj_Path, Mob_Id, Apc_Id, Act_Path, Ptl_Path);
                            dnf.人偶寻找编号(pvfpath + "\\aicharacter\\", Apc_Lst_alltext, Apc_Id, Apc_Lst, Apc_Path, Mob_Id, Obj_Id, Act_Path, Ptl_Path, Key_Path);

                        } while (obj_id_count != Obj_Id.Count || mob_id_count != Mob_Id.Count || apc_id_count != Apc_Id.Count);
                    }
                    progressBar1.Value = 60;//进度条进度增加
                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(pvfpath, Mob_Path, Tbl_Path, null, Atk_Path, Ani_Path, null, null, null, Ai_Path, Mob_Equ_Path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(pvfpath, Obj_Path, null, null, Atk_Path, Ani_Path, null, Til_Path, null, null, null);

                    //如果宝珠内有人偶就获取ai、key
                    if (Apc_Id.Count > 0)
                    {
                        dnf.寻找全路径(pvfpath, Apc_Path, null, null, null, null, null, null, Key_Path, Ai_Path, null);
                    }
                    progressBar1.Value = 70;//进度条进度增加
                    //获取act中的atk、ani
                    dnf.寻找全路径(pvfpath, Act_Path, null, null, Atk_Path, Ani_Path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(pvfpath, Ptl_Path, null, null, null, Ani_Path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(Ani_Path, Als_Path, 2, false);

                    //寻找ai
                    dnf.寻找ai(Ai_Path);

                    //合并到单独输出集合,并且准备输出
                    End_All_Path.AddRange(Stk_Path);
                    End_All_Path.AddRange(Obj_Path);
                    End_All_Path.AddRange(Mob_Path);
                    End_All_Path.AddRange(Apc_Path);
                    End_All_Path.AddRange(Apd_Path);
                    End_All_Path.AddRange(Tbl_Path);
                    End_All_Path.AddRange(Act_Path);
                    End_All_Path.AddRange(Atk_Path);
                    End_All_Path.AddRange(Ani_Path);
                    End_All_Path.AddRange(Als_Path);
                    End_All_Path.AddRange(Ptl_Path);
                    End_All_Path.AddRange(Til_Path);
                    End_All_Path.AddRange(Key_Path);
                    End_All_Path.AddRange(Ai_Path);
                    End_All_Path.AddRange(Mob_Equ_Path);

                    progressBar1.Value = 80;//进度条进度增加
                    //lst信息加入字符串变量，并且准备输出
                    if (Stk_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【stackable/stackable.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Stk_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Stk_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到stk宝珠lst\r\n\r\n\r\n");
                    }

                    if (Obj_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Obj_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Obj_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到特效lst\r\n\r\n\r\n");
                    }

                    if (Mob_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Mob_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Mob_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到怪物lst\r\n\r\n\r\n");
                    }

                    if (Apc_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Apc_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Apc_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到人偶lst\r\n\r\n\r\n");
                    }

                    if (Apd_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【appendage/appendage.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Apd_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Apd_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到状态APDlst\r\n\r\n\r\n");
                    }
                    MysqlYz.XY(14);
                    //开始输出所有文件
                    foreach (string item in End_All_Path)
                    {
                        string endpath;
                        string endmovepath;
                        string moveDirectory;
                        //取出原文件所在全路径，并且转为小写
                        endpath = item.ToLower();

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (File.Exists(endpath) == true)
                        {
                            //转换为提取导出的全路径
                            endmovepath = endpath.Replace(pvfpath, movepath);
                            //转换为提取导出的父路径
                            moveDirectory = Directory.GetParent(endmovepath).ToString();
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.Copy(endpath, endmovepath, true);
                        }

                    }
                    progressBar1.Value = 90;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", End_File_Text.ToString().Replace("\\", "/"));

                    //结束副本提取测量时间
                    StkBaoZhutime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (YesNpImgTiQu)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        Imgtime = time.ElapsedMilliseconds;
                    }

                    string Imgtimetext = "";

                    if (Imgtime != 0)
                    {
                        Imgtimetext = "提取的img全路径总用时：" + (double)Imgtime / 1000 + "秒；\r\n";
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取宝珠，判断img提取时间是否存在
                    MessageBox.Show("已完成指定的stk宝珠提取工作；\r\n提取宝珠总用时：" + (double)StkBaoZhutime / 1000 + "秒；\r\n" + Imgtimetext, "完成", MessageBoxButtons.OK, MessageBoxIcon.None);

                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        new DingWei().Explorer(movepath);
                    }

                    time.Stop();

                }
            }
        }

        private void 提取宝珠开始在线提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认已输入的编号或目录路径正确，并且开始提取宝珠？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoBaoZhu_tq = false;

                //开始初始判断是否提取装备
                if (提取宝珠编号textmaskedTextBox.Text != "")
                {


                    if (提取宝珠导出目录textmaskedTextBox.Text != "")
                    {

                        if (Directory.Exists(提取宝珠导出目录textmaskedTextBox.Text))
                        {
                            YesNoBaoZhu_tq = true;
                        }
                        else
                        {
                            MessageBox.Show("所选stk宝珠提取并导出目录不存在；为了提取正常操作请选择存在的目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            提取宝珠导出目录textmaskedTextBox.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("提取并导出的目录不可空；请选择一个目录作为文件导出目录", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        提取宝珠导出目录textmaskedTextBox.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("请输入需要提取stk宝珠编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    提取宝珠编号textmaskedTextBox.Focus();
                }

                if (YesNoBaoZhu_tq)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    long StkBaoZhutime;
                    long Imgtime = 0;

                    DnfHttp dnf = new DnfHttp();

                    bool YesNpImgTiQu = 提取宝珠寻找img路径checkBox.Checked;
                    string movepath = 提取宝珠导出目录textmaskedTextBox.Text;

                    List<List<string>> Stk_Lst_alltext = new List<List<string>>();
                    List<List<string>> Obj_Lst_alltext = new List<List<string>>();
                    List<List<string>> Mob_Lst_alltext = new List<List<string>>();
                    List<List<string>> Apc_Lst_alltext = new List<List<string>>();
                    List<List<string>> Apd_Lst_alltext = new List<List<string>>();
                    progressBar1.Value = 10;//进度条进度增加
                    //读入装备lst列表内容
                    if (dnf.FileExists("stackable/stackable.lst"))
                    {
                        Stk_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("stackable/stackable.lst").Replace("\\", "/"));
                    }
                    //读入特效lst列表内容
                    if (dnf.FileExists("passiveobject/passiveobject.lst"))
                    {
                        Obj_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("passiveobject/passiveobject.lst").Replace("\\", "/"));
                    }
                    //读入怪物lst列表内容
                    if (dnf.FileExists("monster/monster.lst"))
                    {
                        Mob_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("monster/monster.lst").Replace("\\", "/"));
                    }
                    //读入人偶lst列表内容
                    if (dnf.FileExists("aicharacter/aicharacter.lst"))
                    {
                        Apc_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("aicharacter/aicharacter.lst").Replace("\\", "/"));
                    }
                    //读入APDlst列表内容
                    if (dnf.FileExists("appendage/appendage.lst"))
                    {
                        Apd_Lst_alltext = dnf.LstChongPaiLie(dnf.FileReadAllText("appendage/appendage.lst").Replace("\\", "/"));
                    }
                    progressBar1.Value = 20;//进度条进度增加

                    List<string> Stk_Id = new List<string>();
                    List<string> Obj_Id = new List<string>();
                    List<string> Mob_Id = new List<string>();
                    List<string> Apc_Id = new List<string>();
                    List<string> Apd_Id = new List<string>();

                    List<string> Stk_Lst = new List<string>();
                    List<string> Obj_Lst = new List<string>();
                    List<string> Mob_Lst = new List<string>();
                    List<string> Apc_Lst = new List<string>();
                    List<string> Apd_Lst = new List<string>();

                    List<string> Stk_Path = new List<string>();
                    List<string> Obj_Path = new List<string>();
                    List<string> Mob_Path = new List<string>();
                    List<string> Apc_Path = new List<string>();
                    List<string> Apd_Path = new List<string>();
                    List<string> Tbl_Path = new List<string>();
                    List<string> Act_Path = new List<string>();
                    List<string> Atk_Path = new List<string>();
                    List<string> Ani_Path = new List<string>();
                    List<string> Als_Path = new List<string>();
                    List<string> Ptl_Path = new List<string>();
                    List<string> Til_Path = new List<string>();
                    List<string> Key_Path = new List<string>();
                    List<string> Ai_Path = new List<string>();
                    List<string> Mob_Equ_Path = new List<string>();

                    List<string> End_All_Path = new List<string>();

                    StringBuilder End_File_Text = new StringBuilder();


                    //获取用户提供的宝珠编号
                    Stk_Id.AddRange(dnf.LstChongPaiLieGetId(提取宝珠编号textmaskedTextBox.Text));

                    progressBar1.Value = 30;//进度条进度增加

                    //通过宝珠编号获取全路径
                    dnf.Id_lst_编号查找("stackable/", Stk_Lst_alltext, Stk_Id, null, Stk_Path);


                    //通过宝珠文件路径获取卡片路径
                    dnf.Stk宝珠寻找卡片Id(Stk_Path, Stk_Id);


                    //通过宝珠和卡片编号获取lst跟全路径
                    dnf.Id_lst_编号查找("stackable/", Stk_Lst_alltext, Stk_Id, Stk_Lst, Stk_Path);


                    //通过装备路径 找出apd、mob、apc、obj编号 以及ptl全路径 还有etc套装ID ani全路径
                    dnf.Equ寻找编号和路径(Stk_Path, Apd_Id, Mob_Id, Apc_Id, Obj_Id, Ptl_Path, null, Ani_Path);


                    //通过APD ID寻找lst 跟全路径 以及APD文件内的ani路径
                    dnf.Id_lst_编号查找("appendage/", Apd_Lst_alltext, Apd_Id, Apd_Lst, Apd_Path);

                    progressBar1.Value = 40;//进度条进度增加
                    //判断obj mob apc编号是否找到；找到就进入遍历循环
                    if (Obj_Id.Count > 0 || Mob_Id.Count > 0 || Apc_Id.Count > 0)
                    {
                        //首先创建各个编号所记录的变量
                        int obj_id_count;
                        int mob_id_count;
                        int apc_id_count;
                        //循环遍历出所有关联编号
                        do
                        {
                            //为了确保循环继续进行，先赋值
                            obj_id_count = Obj_Id.Count;
                            mob_id_count = Mob_Id.Count;
                            apc_id_count = Apc_Id.Count;

                            dnf.怪物寻找编号(Mob_Lst_alltext, Mob_Id, Mob_Lst, Mob_Path, Obj_Id, Apc_Id, Act_Path, Ptl_Path);
                            dnf.特效寻找编号(Obj_Lst_alltext, Obj_Id, Obj_Lst, Obj_Path, Mob_Id, Apc_Id, Act_Path, Ptl_Path);
                            dnf.人偶寻找编号(Apc_Lst_alltext, Apc_Id, Apc_Lst, Apc_Path, Mob_Id, Obj_Id, Act_Path, Ptl_Path, Key_Path);

                        } while (obj_id_count != Obj_Id.Count || mob_id_count != Mob_Id.Count || apc_id_count != Apc_Id.Count);
                    }

                    //获取mob文件中的ani、tbl、ai、equani、atk
                    dnf.寻找全路径(Mob_Path, Tbl_Path, null, Atk_Path, Ani_Path, null, null, null, Ai_Path, Mob_Equ_Path);

                    //获取obj文件中的ani、til、atk
                    dnf.寻找全路径(Obj_Path, null, null, Atk_Path, Ani_Path, null, Til_Path, null, null, null);
                    progressBar1.Value = 50;//进度条进度增加
                    //如果宝珠内有人偶就获取ai、key
                    if (Apc_Id.Count > 0)
                    {
                        dnf.寻找全路径(Apc_Path, null, null, null, null, null, null, Key_Path, Ai_Path, null);
                    }

                    //获取act中的atk、ani
                    dnf.寻找全路径(Act_Path, null, null, Atk_Path, Ani_Path, null, null, null, null, null);

                    //获取ptl中的ani
                    dnf.寻找全路径(Ptl_Path, null, null, null, Ani_Path, null, null, null, null, null);

                    //寻找ani的als
                    dnf.寻找als(Ani_Path, Als_Path, 2, false);

                    //寻找ai
                    dnf.寻找ai(Ai_Path);

                    //合并到单独输出集合,并且准备输出
                    End_All_Path.AddRange(Stk_Path);
                    End_All_Path.AddRange(Obj_Path);
                    End_All_Path.AddRange(Mob_Path);
                    End_All_Path.AddRange(Apc_Path);
                    End_All_Path.AddRange(Apd_Path);
                    End_All_Path.AddRange(Tbl_Path);
                    End_All_Path.AddRange(Act_Path);
                    End_All_Path.AddRange(Atk_Path);
                    End_All_Path.AddRange(Ani_Path);
                    End_All_Path.AddRange(Als_Path);
                    End_All_Path.AddRange(Ptl_Path);
                    End_All_Path.AddRange(Til_Path);
                    End_All_Path.AddRange(Key_Path);
                    End_All_Path.AddRange(Ai_Path);
                    End_All_Path.AddRange(Mob_Equ_Path);

                    progressBar1.Value = 60;//进度条进度增加
                    //lst信息加入字符串变量，并且准备输出
                    if (Stk_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【stackable/stackable.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Stk_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Stk_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到stk宝珠lst\r\n\r\n\r\n");
                    }

                    if (Obj_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Obj_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Obj_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到特效lst\r\n\r\n\r\n");
                    }

                    if (Mob_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Mob_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Mob_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到怪物lst\r\n\r\n\r\n");
                    }

                    if (Apc_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Apc_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Apc_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到人偶lst\r\n\r\n\r\n");
                    }

                    if (Apd_Lst.Count > 0)
                    {
                        End_File_Text.Append("----------------------------------以下内容加入PVF【appendage/appendage.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + Apd_Lst.Count.ToString() + "】-------------------------------------------------------\r\n");
                        foreach (string item in Apd_Lst)
                        {
                            End_File_Text.Append(item + "\r\n");
                        }
                        End_File_Text.Append("\r\n\r\n");
                    }
                    else
                    {
                        End_File_Text.Append("未找到状态APDlst\r\n\r\n\r\n");
                    }
                    progressBar1.Value = 70;//进度条进度增加
                    //开始输出所有文件
                    foreach (string item in End_All_Path)
                    {
                        string endmovepath;
                        string moveDirectory;

                        //有些人obj写了编号却没有文件，这里加一个判断：要复制的文件是否存在
                        if (dnf.FileExists(item))
                        {
                            //转换为提取导出的全路径
                            endmovepath = movepath + "/" + item;
                            //转换为提取导出的父路径
                            moveDirectory = dnf.DirectoryGetParent(endmovepath);
                            //创建提取导出的父路径所在目录
                            Directory.CreateDirectory(moveDirectory);
                            //复制文件到指定目录，如果有同名文件就覆盖
                            File.WriteAllText(endmovepath, dnf.FileReadAllText(item));
                        }

                    }
                    progressBar1.Value = 80;//进度条进度增加
                    //输出lst列表内容,并且\替换为/
                    File.WriteAllText(movepath + "\\(2)查找出来的lst列表.txt", End_File_Text.ToString().Replace("\\", "/"));

                    //结束副本提取测量时间
                    StkBaoZhutime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (YesNpImgTiQu)
                    {
                        time.Restart();

                        StringBuilder npkimgpath = new StringBuilder();

                        dnf.寻找img路径(movepath, "*.*", npkimgpath);

                        if (npkimgpath.Length > 0)
                        {
                            File.WriteAllText(movepath + "\\(1)npk当中的img全路径.txt", npkimgpath.ToString());
                        }

                        //获取查找img路径的时间
                        Imgtime = time.ElapsedMilliseconds;
                    }

                    string Imgtimetext = "";

                    if (Imgtime != 0)
                    {
                        Imgtimetext = "提取的img全路径总用时：" + (double)Imgtime / 1000 + "秒；\r\n";
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    //弹出信息框，结束提取宝珠，判断img提取时间是否存在
                    MessageBox.Show("已完成指定的stk宝珠提取工作；\r\n提取宝珠总用时：" + (double)StkBaoZhutime / 1000 + "秒；\r\n" + Imgtimetext, "完成", MessageBoxButtons.OK, MessageBoxIcon.None);

                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        new DingWei().Explorer(movepath);
                    }

                    time.Stop();

                }
            }
            progressBar1.Visible = false;//进度条可视假
        }


        //——————————————附魔宝珠提取功能区————————————————


        //——————————————NPK操作功能区————————————————


        private void NPK操作使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "选择一个存在NPK文件的目录；创建一个数据库可查询NPK内的img全路径信息；并且支持选择性复制，导出txt文本，定位文件功能；\r\n\r\n" +
                "<查询>\r\n" +
                "<img全路径>\r\n" +
                "例如：sprite/character/fighter/atequipment/weapon/arm/fm_arms0003a.img\r\n\r\n" +
                "<img名称>\r\n" +
                "例如：fm_arms0003a.img\r\n\r\n" +
                "<npk文件名>\r\n" +
                "例如：sprite_character_fighter_atequipment_weapon_arm.NPK\r\n\r\n" +
                "<模糊查询>\r\n" +
                "选择以上查询类别后可随意输入连续包含的内容进行模糊查询，注：此功能如果查询过多模糊相识时会造成卡顿\r\n\r\n" +
                "<精准查询>\r\n" +
                "建议使用此查询模式搜索精准可靠如果搜索img名称则填写fm_arms0003a.img\r\n\r\n";
            MessageBox.Show(Text, "NPK查询使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void NPK操作创建数据库button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要创建img数据库？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                if (NPK操作img2目录comboBox.SelectedIndex >= 0)
                {
                    bool mdb = false;
                    string ImagePacks2 = NPK操作img2目录comboBox.SelectedItem.ToString();

                    if (Directory.GetFiles(ImagePacks2, "*.sqlite").Length > 0)
                    {
                        if (MessageBox.Show("选择目录下已经存在数据库文件，是否继续创建？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            mdb = true;
                        }
                    }
                    else
                    {
                        mdb = true;
                    }

                    MysqlYz.XY(16);

                    if (mdb)
                    {

                        if (Directory.Exists(ImagePacks2))
                        {

                            if (Directory.GetFiles(ImagePacks2, "*.npk").Length > 0)
                            {
                                progressBar1.Value = 0;//进度条初始0
                                progressBar1.Visible = true;//进度条可视真
                                progressBar1.Maximum = 100;

                                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                                time.Start();

                                string filename = "剑圣60-img数据库" + DateTime.Now.ToString("yyyy年MM月dd日") + DateTime.Now.ToString("hh点mm分ss秒") + ".sqlite";
                                string filePath = ImagePacks2 + "\\" + filename;
                                progressBar1.Value = 10;//进度条进度增加

                                //    ADOX.Column[] userSetColumns =
                                //    {
                                ////这块大家注意数据类型的问题
                                //new ADOX.Column(){Name="id",Type=DataTypeEnum.adInteger,DefinedSize=20},
                                //new ADOX.Column(){Name="img全路径",Type=DataTypeEnum.adVarWChar,DefinedSize=255},
                                //new ADOX.Column(){Name="img名称",Type=DataTypeEnum.adVarWChar ,DefinedSize=255},
                                //new ADOX.Column(){Name="npk文件名",Type=DataTypeEnum.adVarWChar ,DefinedSize=255},
                                //};

                                //Access access = new Access();
                                Sqlite数据库.Sqlite sqlite = new Sqlite数据库.Sqlite();
                                progressBar1.Value = 20;//进度条进度增加
                                //access.CreateAccessTable(filePath, "ImagePacks2", userSetColumns);
                                //access.CreateAccessTable(filePath, "ImagePacks2", userSetColumns);
                                sqlite.CreateSqliteTable(filePath);

                                progressBar1.Value = 30;//进度条进度增加
                                //NpkFile npkFile = new NpkFile();
                                //npkFile.AddAccess(ImagePacks2, filePath);
                                //枚举npk文件
                                List<string> dirs = new List<string>(Directory.EnumerateFiles(ImagePacks2, "*.npk", SearchOption.TopDirectoryOnly));
                                dirs.Sort();

                                sqlite.AddSqlite(dirs, filePath);

                                progressBar1.Value = 80;//进度条进度增加

                                List<string> dirss = new List<string>(Directory.EnumerateFiles(NPK操作img2目录comboBox.SelectedItem.ToString(), "*.sqlite", SearchOption.TopDirectoryOnly));
                                if (dirss.Count > 0)
                                {
                                    dirss.Sort();
                                    NPK操作数据库mdbcomboBox.Items.Clear();
                                    foreach (var item in dirss)
                                    {
                                        NPK操作数据库mdbcomboBox.Items.Add(item.Substring(item.LastIndexOf("\\") + 1));
                                    }

                                }
                                progressBar1.Value = 100;//进度条进度增加
                                MessageBox.Show("已完成img全路径创建数据库工作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "询问", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                                NPK操作数据库mdbcomboBox.SelectedIndex = NPK操作数据库mdbcomboBox.FindString(filename);

                                time.Stop();

                            }
                            else
                            {
                                NPK操作img2目录comboBox.Items.RemoveAt(NPK操作img2目录comboBox.SelectedIndex);
                                NPK操作img2目录comboBox.Text = "";
                                MessageBox.Show("抱歉，您选择的目录下无“.npk”文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        else
                        {
                            NPK操作img2目录comboBox.Items.RemoveAt(NPK操作img2目录comboBox.SelectedIndex);
                            NPK操作img2目录comboBox.Text = "";
                            MessageBox.Show("抱歉，您选择的目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("抱歉，您没有选择目录，请重新选择一个目录作为创建数据库的依据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        private void NPK操作删除数据库button_Click(object sender, EventArgs e)
        {
            if (NPK操作数据库mdbcomboBox.Items.Count <= 0)
            {
                AddToolInfo("您没有选择数据库", 1500, 50, 30, NPK操作删除数据库button);
                return;
            }


            string SqliteFileName = NPK操作数据库mdbcomboBox.SelectedItem.ToString();
            if (MessageBox.Show($"真的要删除Sqlite数据库吗？\r\n{SqliteFileName}", "注：此操作不可恢复", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            string SqlitePath = NPK操作img2目录comboBox.SelectedItem.ToString();
            if (Directory.Exists(SqlitePath) == false)
            {
                MessageBox.Show($"所选的：{SqlitePath}不存在；\r\n请重新选择。", "错误", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                return;
            }

            if (File.Exists(SqlitePath + "\\" + SqliteFileName) == false)
            {
                MessageBox.Show($"需要被删除的数据库不存在：\r\n{SqlitePath}\\{SqliteFileName}\r\n请重新选择。", "错误", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                NPK操作数据库mdbcomboBox.Items.RemoveAt(NPK操作数据库mdbcomboBox.SelectedIndex);
                if (NPK操作数据库mdbcomboBox.Items.Count > 0)
                    NPK操作数据库mdbcomboBox.SelectedIndex = 0;
                return;
            }

            File.Delete(SqlitePath + "\\" + SqliteFileName);
            MessageBox.Show("数据库文件已被删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            NPK操作数据库mdbcomboBox.Items.RemoveAt(NPK操作数据库mdbcomboBox.SelectedIndex);
            if (NPK操作数据库mdbcomboBox.Items.Count > 0)
                NPK操作数据库mdbcomboBox.SelectedIndex = 0;
        }

        private void NPK操作img2目录button_MouseDown(object sender, MouseEventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = true,//设置只能选择文件夹
                Title = @"请选择ImagePacks2目录；例如：E:\DNF\ImagePacks2"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {

                if (NPK操作img2目录comboBox.Items.Contains(file_folder.FileName) == false)
                {
                    NPK操作img2目录comboBox.Items.Add(file_folder.FileName);//获取的路径加入文本框
                    NPK操作img2目录comboBox.SelectedIndex = NPK操作img2目录comboBox.Items.Count - 1;//设置选中刚刚加入的列表

                    ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
                    NPK操作img2目录comboBox.SelectedIndexChanged += new EventHandler(NPK操作img2目录comboBox_SelectedIndexChanged);
                }
                else
                {
                    NPK操作img2目录comboBox.SelectedIndex = NPK操作img2目录comboBox.FindStringExact(file_folder.FileName);//设置选中刚刚加入的列表
                    ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
                    NPK操作img2目录comboBox.SelectedIndexChanged += new EventHandler(NPK操作img2目录comboBox_SelectedIndexChanged);

                }

            }
        }

        private void NPK操作img2目录comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(NPK操作img2目录comboBox.SelectedItem.ToString()) == false)
            {

                NPK操作img2目录comboBox.Items.RemoveAt(NPK操作img2目录comboBox.SelectedIndex);
                NPK操作img2目录comboBox.Text = "";
                MessageBox.Show("抱歉，您选择的目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (Directory.GetFiles(NPK操作img2目录comboBox.SelectedItem.ToString(), "*.npk").Length <= 0)
            {
                NPK操作img2目录comboBox.Items.RemoveAt(NPK操作img2目录comboBox.SelectedIndex);
                NPK操作img2目录comboBox.Text = "";
                MessageBox.Show("抱歉，您选择的目录下无“.npk”文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            List<string> SqlFileName = new List<string>(Directory.GetFiles(NPK操作img2目录comboBox.SelectedItem.ToString(), "*.sqlite"));
            if (SqlFileName.Count > 0)
            {
                SqlFileName.Sort();
                NPK操作数据库mdbcomboBox.Items.Clear();

                SqlFileName.ForEach((FileName) => NPK操作数据库mdbcomboBox.Items.Add(FileName.Substring(FileName.LastIndexOf("\\") + 1)));

                NPK操作数据库mdbcomboBox.SelectedIndex = 0;
            }
            else
            {
                while (NPK操作数据库mdbcomboBox.Items.Count > 0)
                    NPK操作数据库mdbcomboBox.Items.RemoveAt(0);
            }
        }

        private void NPK操作查找内容button_MouseDown(object sender, MouseEventArgs e)
        {
            IDpath.DaoChu_button.Text = "导出查询内容";
            IDpath.ShowDialog();
            if (JieShouText != "")
            {
                NPK操作查找内容maskedTextBox.Text = JieShouText;
            }
            JieShouText = "";
        }

        private void NPK操作模糊查找radioButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("模糊查找：数据量过多时可能会造成卡顿；是否继续使用模糊查找？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                NPK操作模糊查找radioButton.Checked = true;

        }

        private void NPK操作查询button_Click(object sender, EventArgs e)
        {
            if (NPK操作数据库mdbcomboBox.Items.Count <= 0)
            {
                AddToolInfo("请先创建一个数据库再进行查询操作", 1500, 50, 20, NPK操作查询button);
                return;
            }


            string SqliteFilePath = NPK操作img2目录comboBox.SelectedItem.ToString() + "\\" + NPK操作数据库mdbcomboBox.SelectedItem.ToString();
            MysqlYz.XY(16);
            if (File.Exists(SqliteFilePath) == false)
            {
                NPK操作数据库mdbcomboBox.Items.RemoveAt(NPK操作数据库mdbcomboBox.SelectedIndex);

                if (NPK操作数据库mdbcomboBox.Items.Count >= 0)
                    NPK操作数据库mdbcomboBox.SelectedIndex = 0;

                MessageBox.Show("您所选择的数据库文件不存在，请重新选择或创建", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (NPK操作查找内容maskedTextBox.Text == "")
            {
                MessageBox.Show("请输入需要查找的内容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (NPK操作查找内容maskedTextBox.Text.Length <= 2 && NPK操作模糊查找radioButton.Checked)
            {
                MessageBox.Show("您输入查找的内容过少，为了避免工具加载过多数据，请更换需要查找的内容；", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }


            //Access access = new Access();
            //DataRowCollection ds = access.ChaXunText(MdbPath, ChaZhaoZiDuan, NPK操作查找内容maskedTextBox.Text, NPK操作模糊查找radioButton.Checked);

            NPK操作列表框ListView.Items.Clear();

            string ChaZhaoZiDuan = "";
            if (NPK操作img全路径radioButton.Checked)
            {
                ChaZhaoZiDuan = "img全路径";
            }
            else if (NPK操作img名称radioButton.Checked)
            {
                ChaZhaoZiDuan = "img名称";
            }
            else if (NPK操作npk文件名radioButton.Checked)
            {
                ChaZhaoZiDuan = "npk文件名";
            }

            Sqlite数据库.Sqlite sqlite = new Sqlite数据库.Sqlite();
            DataRowCollection ds = sqlite.ChaXunText(SqliteFilePath, ChaZhaoZiDuan, NPK操作查找内容maskedTextBox.Text, NPK操作模糊查找radioButton.Checked);

            for (int i = 0; i < ds.Count; i++)
            {
                NPK操作列表框ListView.Items.Add(new ListViewItem()
                {
                    Text = ds[i]["id"].ToString(),
                    SubItems =
                    {
                        ds[i]["img全路径"].ToString(),
                        ds[i]["img名称"].ToString(),
                        ds[i]["npk文件名"].ToString()
                    }
                });

            }

            AddToolInfo($"成功查询总数：{ds.Count.ToString()}", 1500, 50, 20, NPK操作查询button);
            label23.Text = "查询总数：" + ds.Count.ToString();
        }

        private void NPK查询contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (NPK操作列表框ListView.SelectedItems.Count <= 0)
                return;

            NPK查询contextMenuStrip.Close();

            StringBuilder Clipboardtext = new StringBuilder();

            using SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录
                Title = @"另存为查找结果",//设置标题
                Filter = "文本文件|*.txt",
                DefaultExt = ".txt",
                FileName = "img查找结果" + DateTime.Now.ToString("hh点mm分ss秒")
            };


            switch (e.ClickedItem.Text)
            {
                case "定位NPK文件":

                    string FileNpk = NPK操作img2目录comboBox.SelectedItem.ToString() + "\\" + NPK操作列表框ListView.SelectedItems[0].SubItems[3].Text;
                    new DingWei().ExplorerFile(FileNpk);
                    break;
                case "复制选中img全路径":

                    foreach (ListViewItem item in NPK操作列表框ListView.SelectedItems)
                        Clipboardtext.Append(item.SubItems[1].Text + "\r\n");

                    Clipboard.SetDataObject(Clipboardtext.ToString());

                    break;
                case "复制选中img名称":

                    foreach (ListViewItem item in NPK操作列表框ListView.SelectedItems)
                        Clipboardtext.Append(item.SubItems[2].Text + "\r\n");

                    Clipboard.SetDataObject(Clipboardtext.ToString());

                    break;
                case "复制选中npk文件名":

                    foreach (ListViewItem item in NPK操作列表框ListView.SelectedItems)
                        Clipboardtext.Append(item.SubItems[3].Text + "\r\n");

                    Clipboard.SetDataObject(Clipboardtext.ToString());

                    break;
                case "导出所有查找结果":

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (ListViewItem item in NPK操作列表框ListView.Items)
                        {
                            foreach (ListViewItem.ListViewSubItem items in item.SubItems)
                                Clipboardtext.Append(items.Text + "\t");

                            Clipboardtext.Append("\r\n");
                        }

                        File.WriteAllText(saveFileDialog.FileName, Clipboardtext.ToString());
                    }

                    break;
                case "全选":

                    NPK操作列表框ListView.Focus();
                    foreach (ListViewItem item in NPK操作列表框ListView.Items)
                        item.Selected = true;
                    break;
            }
        }

        //——————————————NPK操作功能区————————————————



        //——————————————NPK提取img功能区————————————————

        private void NPKimg全路径文本maskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = false,//设置只能选择文件
                Title = @"请选择存放img全路径的文本文件；例如：E:\DNF\img全路径文件.txt"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                NPKimg全路径文本maskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void NPKimg目录maskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = true,//设置只能选择文件夹
                Title = @"请选择寻找或提取的目录；例如：E:\DNF\ImagePacks2"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                NPKimg目录maskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void NPKimg提取并导出目录maskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = true,//设置只能选择文件夹
                Title = @"请选择最后提取完要导出的目录；例如：E:\DNF\导出的目录"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                NPKimg提取并导出目录maskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void NPKimg全路径提取使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "分为三个模式提取img全路径；\r\n\r\n" +
                "<明文文本img全路径>只提取文本内的img全路径例如:\r\n" +
                "`item/creature/creature_egg.img`\r\n\r\n" +
                "<装备模型img全路径>只提取武器或时装的模型img\r\n\r\n" +
                "<NPK内的img全路径>只提取NPK文件内的img全路径\r\n\r\n" +
                "用于结合img提取或img删除功能使用";
            MessageBox.Show(Text, "img全路径提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void NPKimg开始删除使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "分为两种模式删除：\r\n\r\n" +
                "<删除第一个找到的>\r\n" +
                "在寻找过程中只删除第一个找到的img；不会删除重复的img\r\n\r\n" +
                "<删除所有>\r\n" +
                "在寻找过程中出现1次或n次的img将全部删除";
            MessageBox.Show(Text, "删除img使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void NPKimg开始去重复使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "此功能极其强大；精简客户端首选；\r\n\r\n" +
                "此功能会检索一个目录下所有NPK内的img(不包括子目录)\r\n" +
                "寻找出出现1次以上的img，将删除除了第一个以外的所有img\r\n" +
                "并且单个NPK内重复的img也会被删除\r\n\r\n" +
                "实测：15G左右的NPK目录，仅需36秒左右；精简到了13G";
            MessageBox.Show(Text, "去除重复使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void NPKimg加入使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "此功能超级强大；适合强迫症用户；\r\n\r\n" +
                "用途一：\r\n" +
                "可以把一个目录下所有的NPK(不包含子目录)全部加入到另外一个目录中；\r\n" +
                "可根据原文件名或按照img全路径去加入；\r\n" +
                "例如img全路径为“sprite/character/effg.img”会加入到sprite_character.NPK文件中\r\n" +
                "并且如果没有此NPK文件则可选择新建或不新建，当选择不新建时会生成不存在NPK文档记录；\r\n\r\n" +
                "用途二：\r\n" +
                "此功能也可重新排列NPK，把一个客户端内的所有NPK加入到另外一个目录中；\r\n" +
                "设置按照img全路径名写出，并且NPK如果不存在则创建；\r\n" +
                "这样可直观的看到所需要的NPK或不需要的NPK；人工查找或精简工作则更加清晰明了\r\n\r\n" +
                "其他用途请自行测试~";
            MessageBox.Show(Text, "img加入使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void NPKimg使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "此功能超级强大；对于精简时装或精简客户端或提取资源将及其方便；\r\n\r\n" +
                "<合并为一个NPK>\r\n" +
                "将提取的img全部合成一个NPK，并且保存到指定目录\r\n\r\n" +
                "<按原NPK文件名>\r\n" +
                "img从哪个NPK中提取，就生成同NPK名写出\r\n\r\n" +
                "<img全路径保存>\r\n" +
                "例如img全路径为“sprite/character/effg.img”会加入到sprite_character.NPK文件中\r\n\r\n\r\n" +
                "以上所有操作碰到同名NPK则会覆盖；如需不想覆盖则需要使用<img加入>功能";
            MessageBox.Show(Text, "img提取使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void NPKimg合并使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "此功能可以把多个NPK合并为一个NPK\r\n可以自定义设置合并的NPK大小达到没有提取工具无法编辑NPK的效果；\r\n\r\n" +
                "<大小>\r\n" +
                "建议最好500MB一个NPK，太大的话可能客户端读取会慢\r\n\r\n" +
                "如果不设置自动新增则合并指定目录下的所有NPK为一个NPK";
            MessageBox.Show(Text, "img合并使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 开始提取并保存button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要继续提取？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                //设置一个变量，看是否继续提取
                bool YesNoTiQu = false;

                //判断用户提供的路径是否都正确
                if (File.Exists(NPKimg全路径文本maskedTextBox.Text))
                {
                    if (Directory.Exists(NPKimg目录maskedTextBox.Text))
                    {
                        if (Directory.GetFiles(NPKimg目录maskedTextBox.Text, "*.npk").Length > 0)
                        {
                            if (Directory.Exists(NPKimg提取并导出目录maskedTextBox.Text))
                            {
                                MysqlYz.XY(17);
                                YesNoTiQu = true;
                            }
                            else
                            {
                                MessageBox.Show("抱歉您所选择的提取并导出目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                NPKimg提取并导出目录maskedTextBox.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("抱歉您所选择的ImagePacks2目录下无“.NPK”文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            NPKimg目录maskedTextBox.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("抱歉您所选择的ImagePacks2目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        NPKimg目录maskedTextBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("抱歉您所选择的img全路径文本不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    NPKimg全路径文本maskedTextBox.Focus();
                }

                //如果是提取
                if (YesNoTiQu)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    NpkFile npkFile = new NpkFile();
                    StringBuilder NpkNo = new StringBuilder();
                    StringBuilder ImgNo = new StringBuilder();
                    List<string> NpkNameJiHe = new List<string>();
                    List<string> ImgNameJiHe = new List<string>();

                    Dictionary<string, NpkImgFile> ImgZhengLi;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();//记录时间
                    time.Start();//开始

                    string Imgtext = File.ReadAllText(NPKimg全路径文本maskedTextBox.Text);
                    string ImagePacks2 = NPKimg目录maskedTextBox.Text;
                    string DaoChuPath = NPKimg提取并导出目录maskedTextBox.Text;

                    uint YesNoHeBing = 0;
                    if (NPKimg合并为一个NPK保存radioButton.Checked)
                    {
                        YesNoHeBing = 0;
                    }
                    else if (NPKimg按原NPK文件名输出radioButton.Checked)
                    {
                        YesNoHeBing = 1;
                    }
                    else if (NPKimg全路径保存NPK文件名radioButton.Checked)
                    {
                        YesNoHeBing = 2;
                    }
                    MysqlYz.XY(17);
                    progressBar1.Value = 10;//进度条进度增加
                    List<string> dirs = new List<string>(Directory.EnumerateFiles(ImagePacks2, "*.npk", SearchOption.TopDirectoryOnly));
                    dirs.Sort();
                    progressBar1.Value = 20;//进度条进度增加
                    npkFile.Img所有全路径(dirs, NpkNameJiHe, ImgNameJiHe);
                    progressBar1.Value = 40;//进度条进度增加
                    ImgZhengLi = npkFile.NpkImgZhengliData(Imgtext, ImagePacks2, NpkNameJiHe, ImgNameJiHe, NpkNo, ImgNo);
                    progressBar1.Value = 60;//进度条进度增加
                    npkFile.DaoChuNpkImg(ImgZhengLi, DaoChuPath, YesNoHeBing);
                    progressBar1.Value = 80;//进度条进度增加
                    if (NpkNo.Length > 0)
                    {
                        File.WriteAllText(DaoChuPath + "\\" + DateTime.Now.ToString("hh_mm_ss_") + "提取的npk信息.txt", NpkNo.ToString());
                    }

                    if (ImgNo.Length > 0)
                    {
                        File.WriteAllText(DaoChuPath + "\\" + DateTime.Now.ToString("hh_mm_ss_") + "提取的img信息.txt", ImgNo.ToString());
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    MessageBox.Show("已提取完所有img，总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    time.Stop();

                    new DingWei().Explorer(DaoChuPath);

                }
            }
            progressBar1.Visible = false;//进度条可视假   

        }

        private void NPKimg开始删除button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要继续删除操作？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                //设置一个变量，看是否继续删除
                bool YesNoShanChu = false;

                //判断用户提供的路径是否都正确
                if (File.Exists(NPKimg全路径文本maskedTextBox.Text))
                {
                    if (Directory.Exists(NPKimg目录maskedTextBox.Text))
                    {
                        if (Directory.GetFiles(NPKimg目录maskedTextBox.Text, "*.npk").Length > 0)
                        {
                            MysqlYz.XY(18);
                            YesNoShanChu = true;
                        }
                        else
                        {
                            MessageBox.Show("抱歉您所选择的ImagePacks2目录下无“.NPK”文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            NPKimg目录maskedTextBox.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("抱歉您所选择的ImagePacks2目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        NPKimg目录maskedTextBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("抱歉您所选择的img全路径文本不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    NPKimg全路径文本maskedTextBox.Focus();
                }


                //如果是删除
                if (YesNoShanChu)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();

                    NpkFile npkFile = new NpkFile();
                    List<string> NpkNameJiHe = new List<string>();
                    List<string> ImgNameJiHe = new List<string>();
                    progressBar1.Value = 10;//进度条进度增加
                    StringBuilder ImgNo = new StringBuilder();

                    string AllImgPath = NPKimg全路径文本maskedTextBox.Text;
                    string AllImgPathText = File.ReadAllText(AllImgPath);
                    progressBar1.Value = 20;//进度条进度增加
                    string ImagePacks2 = NPKimg目录maskedTextBox.Text;
                    List<string> NpkPath = new List<string>(Directory.EnumerateFiles(ImagePacks2, "*.npk", SearchOption.TopDirectoryOnly));
                    NpkPath.Sort();
                    progressBar1.Value = 40;//进度条进度增加
                    uint MoShi;

                    if (NPKimg删除第一个找到的radioButton.Checked)
                    {
                        MoShi = 0;
                    }
                    else
                    {
                        MoShi = 2;
                    }
                    MysqlYz.XY(18);
                    progressBar1.Value = 50;//进度条进度增加
                    npkFile.Img所有全路径(NpkPath, NpkNameJiHe, ImgNameJiHe);
                    progressBar1.Value = 70;//进度条进度增加
                    npkFile.DeleteImg(ImagePacks2, npkFile.NpkImgZhengLiDeleteList(AllImgPathText, NpkNameJiHe, ImgNameJiHe, ImgNo, MoShi));
                    progressBar1.Value = 90;//进度条进度增加
                    if (ImgNo.Length > 0)
                    {
                        string XieDaoFlie = AllImgPath.Remove(AllImgPath.LastIndexOf("\\") + 1) + DateTime.Now.ToString("hh点mm分ss秒") + "不存在的img.txt";
                        File.WriteAllText(XieDaoFlie, ImgNo.ToString());
                        new DingWei().ExplorerFile(XieDaoFlie);
                    }
                    progressBar1.Value = 100;//进度条进度增加
                    MessageBox.Show("已删除所有指定img；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒" + "\r\n", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    time.Stop();
                }
            }
            progressBar1.Visible = false;//进度条可视假   
        }

        private void NPKimg开始去重复button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要开始去重复？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                //设置一个变量，看是否继续去重
                bool YesNoQuChongFu = false;

                //判断用户提供的路径是否都正确
                if (Directory.Exists(NPKimg目录maskedTextBox.Text))
                {
                    if (Directory.GetFiles(NPKimg目录maskedTextBox.Text, "*.npk").Length > 0)
                    {
                        MysqlYz.XY(19);
                        YesNoQuChongFu = true;
                    }
                    else
                    {
                        MessageBox.Show("抱歉您所选择的ImagePacks2目录下无“.NPK”文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        NPKimg目录maskedTextBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("抱歉您所选择的ImagePacks2目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    NPKimg目录maskedTextBox.Focus();
                }

                //如果是去重
                if (YesNoQuChongFu)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    System.Diagnostics.Stopwatch ChongFuImg = new System.Diagnostics.Stopwatch();
                    time.Start();

                    NpkFile npkFile = new NpkFile();

                    List<string> NpkNameJiHe = new List<string>();
                    List<string> ImgNameJiHe = new List<string>();
                    List<string> AllDeleteImgPath = new List<string>();

                    StringBuilder ImgNo = new StringBuilder();

                    string ImagePacks2 = NPKimg目录maskedTextBox.Text;
                    progressBar1.Value = 10;//进度条进度增加
                    List<string> NpkPath = new List<string>(Directory.EnumerateFiles(ImagePacks2, "*.npk", SearchOption.TopDirectoryOnly));
                    NpkPath.Sort();
                    progressBar1.Value = 20;//进度条进度增加
                    MysqlYz.XY(19);
                    progressBar1.Value = 30;//进度条进度增加
                    //考虑到一个NPK内可能会出现多个一样的img路径，先进行一次删除工作(退而其次，换了一个稳定方法)
                    foreach (string item in NpkPath)
                    {
                        try
                        {
                            if (npkFile.NpkImg是否重复(item))
                                npkFile.写出(item, npkFile.打开EX(item));
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"在去除单个NPK重复img时发生错误，错误文件路径：{item}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }

                    progressBar1.Value = 50;//进度条进度增加
                    //接下来进行不同NPK的精简工作

                    npkFile.Img所有全路径(NpkPath, NpkNameJiHe, ImgNameJiHe);

                    progressBar1.Value = 70;//进度条进度增加

                    ChongFuImg.Start();
                    foreach (var item in ImgNameJiHe.GroupBy(s => s))
                    {
                        if (item.Count() > 1)
                        {
                            AllDeleteImgPath.Add(item.Key);
                        }
                    }
                    long ChongFuImgTime = ChongFuImg.ElapsedMilliseconds;
                    ChongFuImg.Stop();
                    progressBar1.Value = 80;//进度条进度增加
                    npkFile.DeleteImg(ImagePacks2, npkFile.NpkImgZhengLiDeleteList(AllDeleteImgPath, NpkNameJiHe, ImgNameJiHe, ImgNo, 1));

                    progressBar1.Value = 100;//进度条进度增加
                    MessageBox.Show("已删除所有重复img；" +
                        "查找出重复img用时：" + (double)ChongFuImgTime / 1000 + "秒；" +
                        "总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒\r\n\r\n" +
                        "注：如果单纯去重复导致游戏内某个地方补丁缺失，黑掉，是因为客户端内存在加密NPK。解决方法：自行补上缺失的补丁，或者找加密者解密" + "\r\n",
                        "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    time.Stop();
                }
            }
            progressBar1.Visible = false;//进度条可视假   
        }

        private void NPKimg全路径提取button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否开始寻找img全路径？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                //设置一个变量，看是否继续提取
                bool YesNoTiQu = false;

                //判断用户提供的路径是否都正确
                if (Directory.Exists(NPKimg目录maskedTextBox.Text))
                {

                    if (Directory.Exists(NPKimg提取并导出目录maskedTextBox.Text))
                    {
                        MysqlYz.XY(21);
                        YesNoTiQu = true;
                    }
                    else
                    {
                        MessageBox.Show("抱歉您所选择的导出目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        NPKimg提取并导出目录maskedTextBox.Focus();
                    }

                }
                else
                {
                    MessageBox.Show("抱歉您所选择的寻找或提取目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    NPKimg目录maskedTextBox.Focus();
                }

                //如果是提取
                if (YesNoTiQu)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    Regex_new regex = new Regex_new();
                    NpkFile npkFile = new NpkFile();
                    StringBuilder FanKuiCuoWu = new StringBuilder();
                    List<string> ImgNameJiHe = new List<string>();

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();//记录时间
                    time.Start();//开始

                    string XunZhaoPath = NPKimg目录maskedTextBox.Text;
                    string DaoChuPath = NPKimg提取并导出目录maskedTextBox.Text;

                    uint MoShi = 0;
                    if (NPKimg装备模型全路径radioButton.Checked)
                    {
                        MoShi = 0;
                    }
                    else if (NPKimg明文全路径radioButton.Checked)
                    {
                        MoShi = 1;
                    }
                    else if (NPKimgNPKimg全路径radioButton.Checked)
                    {
                        MoShi = 2;
                    }
                    progressBar1.Value = 10;//进度条进度增加
                    MysqlYz.XY(21);
                    progressBar1.Value = 20;//进度条进度增加
                    npkFile.Img所有全路径(XunZhaoPath, ImgNameJiHe, MoShi, FanKuiCuoWu);
                    progressBar1.Value = 40;//进度条进度增加
                    if (FanKuiCuoWu.Length > 0)
                    {
                        File.WriteAllText(DaoChuPath + "\\" + DateTime.Now.ToString("hh_mm_ss_") + "错误反馈.txt", FanKuiCuoWu.ToString());
                    }
                    progressBar1.Value = 60;//进度条进度增加
                    FanKuiCuoWu.Clear();
                    foreach (string item in ImgNameJiHe)
                    {
                        FanKuiCuoWu.Append(item + "\r\n");
                    }
                    FanKuiCuoWu.Insert(0, "寻找img路径遍历的文件总数为：" + ImgNameJiHe.Count.ToString() + "个\r\n\r\n", 1);

                    progressBar1.Value = 80;//进度条进度增加
                    string DaoChuFile = DaoChuPath + "\\" + DateTime.Now.ToString("hh_mm_ss_");
                    string XieChuText = regex.替换(FanKuiCuoWu.ToString(), "[/]{2,}|^[/]+", "");
                    progressBar1.Value = 100;//进度条进度增加
                    switch (MoShi)
                    {
                        case 0:
                            File.WriteAllText(DaoChuFile + "装备模型img全路径.txt", XieChuText);
                            MessageBox.Show("已完成装备模型img全路径查找工作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            time.Stop();
                            new DingWei().ExplorerFile(DaoChuFile + "装备模型img全路径.txt");
                            break;
                        case 1:
                            File.WriteAllText(DaoChuFile + "明文img全路径.txt", XieChuText);
                            MessageBox.Show("已完成明文img全路径查找工作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            time.Stop();
                            new DingWei().ExplorerFile(DaoChuFile + "明文img全路径.txt");
                            break;
                        case 2:
                            File.WriteAllText(DaoChuFile + "NPKimg全路径.txt", XieChuText);
                            MessageBox.Show("已完成NPKimg全路径查找工作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            time.Stop();
                            new DingWei().ExplorerFile(DaoChuFile + "NPKimg全路径.txt");
                            break;
                    }
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        private void NPKimg加入开始加入button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("真的要开始NPK的img加入到另外一个目录中吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                //设置一个变量，看是否继续提取
                bool YesNoJiaRu = false;

                //判断用户提供的路径是否都正确
                if (Directory.Exists(NPKimg目录maskedTextBox.Text))
                {
                    if (Directory.GetFiles(NPKimg目录maskedTextBox.Text, "*.npk").Length > 0)
                    {
                        if (Directory.Exists(NPKimg提取并导出目录maskedTextBox.Text))
                        {
                            if (Directory.GetFiles(NPKimg目录maskedTextBox.Text, "*.npk").Length > 0)
                            {
                                MysqlYz.XY(20);
                                YesNoJiaRu = true;
                            }
                            else
                            {
                                MessageBox.Show("抱歉您所选择要加入的目录下无“.NPK”文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                NPKimg提取并导出目录maskedTextBox.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("抱歉您所选择要加入的目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            NPKimg提取并导出目录maskedTextBox.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("抱歉您所选择的原NPK目录下无“.NPK”文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        NPKimg目录maskedTextBox.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("抱歉您所选择的原NPK目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    NPKimg目录maskedTextBox.Focus();
                }

                if (YesNoJiaRu)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();


                    NpkFile npkFile = new NpkFile();


                    bool XinJian = NPKimg加入不存在创建checkBox.Checked;
                    string YuanPath = NPKimg目录maskedTextBox.Text;
                    string JiaRuPath = NPKimg提取并导出目录maskedTextBox.Text;
                    uint JiaRuMoShi;
                    uint ChongTuChuLi;
                    progressBar1.Value = 10;//进度条进度增加

                    if (NPKimg加入按原文件radioButton.Checked)
                    {
                        JiaRuMoShi = 0;
                    }
                    else
                    {
                        JiaRuMoShi = 1;
                    }
                    progressBar1.Value = 20;//进度条进度增加
                    if (NPKimg加入删除对方radioButton.Checked)
                    {
                        ChongTuChuLi = 0;
                    }
                    else
                    {
                        ChongTuChuLi = 1;
                    }

                    MysqlYz.XY(20);
                    List<string> dirs = new List<string>(Directory.EnumerateFiles(YuanPath, "*.npk", SearchOption.TopDirectoryOnly));
                    dirs.Sort();
                    progressBar1.Value = 30;//进度条进度增加
                    if (XinJian)
                    {
                        progressBar1.Value = 60;//进度条进度增加
                        foreach (string item in dirs)
                        {
                            try
                            {
                                npkFile.DaoChuAddNpkImg(npkFile.打开返回字典(item), JiaRuPath, JiaRuMoShi, ChongTuChuLi);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("发生错误，错误文件路径为：" + item, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }

                        }
                        progressBar1.Value = 100;//进度条进度增加
                        MessageBox.Show("已完成加入操作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        string NoNpkName = "\\" + DateTime.Now.ToString("hh_mm_ss_") + "不存在的npk.txt";
                        StringBuilder NoNpk = new StringBuilder();
                        foreach (string item in dirs)
                        {
                            npkFile.DaoChuAddNpkImg(npkFile.打开返回字典(item), JiaRuPath, JiaRuMoShi, ChongTuChuLi, NoNpk);
                        }
                        progressBar1.Value = 60;//进度条进度增加
                        if (NoNpk.Length > 0)
                        {
                            File.WriteAllText(YuanPath + NoNpkName, NoNpk.ToString());
                            new DingWei().ExplorerFile(YuanPath + NoNpkName);
                        }
                        progressBar1.Value = 100;//进度条进度增加
                        MessageBox.Show("已完成加入操作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                    }

                    time.Stop();

                }

            }
            progressBar1.Visible = false;//进度条可视假
        }

        private void NPKimg合并checkBox_Click(object sender, EventArgs e)
        {
            bool YesNo = false;
            if (NPKimg合并checkBox.Checked)
            {
                YesNo = true;
            }

            NPKimg合并label大小.Enabled = YesNo;
            NPKimg合并maskedTextBox.Enabled = YesNo;
            NPKimg合并labelmb.Enabled = YesNo;
        }

        private void NPKimg合并button_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("真的要开始合并NPK吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;


            //判断用户提供的路径是否都正确
            if (Directory.Exists(NPKimg目录maskedTextBox.Text))
            {
                if (Directory.GetFiles(NPKimg目录maskedTextBox.Text, "*.npk").Length > 0)
                {
                    MysqlYz.XY(22);
                }
                else
                {
                    MessageBox.Show("抱歉您所选择需要合并的NPK目录下无“.NPK”文件，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    NPKimg目录maskedTextBox.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("抱歉您所选择需要合并的NPK目录不存在，请重新选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                NPKimg目录maskedTextBox.Focus();
                return;
            }

            NpkFile npkFile = new NpkFile();

            using SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = ApiDirectoryPath,//设置初始目录
                Title = @"保存合并NPK",//设置标题
                Filter = "NPK文件|*.NPK",
                DefaultExt = ".NPK",
                FileName = "合并NPK" + DateTime.Now.ToString("hh点mm分ss秒")
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                time.Start();

                string NpkPath = NPKimg目录maskedTextBox.Text;
                string BaoCunFilePath = saveFileDialog.FileName;
                StringBuilder NoNpk = new StringBuilder();

                MysqlYz.XY(22);

                long MaxSize = 0;

                if (NPKimg合并checkBox.Checked)
                    MaxSize = Convert.ToInt32(NPKimg合并maskedTextBox.Text) * 1048576;

                npkFile.HeBingNpk(new List<string>(Directory.EnumerateFiles(NpkPath, "*.npk", SearchOption.TopDirectoryOnly)), BaoCunFilePath, NoNpk, progressBar1, MaxSize);

                if (NoNpk.Length > 0)
                    File.WriteAllText(BaoCunFilePath.Remove(BaoCunFilePath.LastIndexOf("\\") + 1) + DateTime.Now.ToString("hh_mm_ss_") + "错误NPK信息.txt", NoNpk.ToString());

                MessageBox.Show("已完成合并NPK工作；总用时：" + (double)time.ElapsedMilliseconds / 1000 + "秒", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
            }

        }


        //——————————————NPK提取img功能区————————————————


        //——————————————lst列表寻找————————————————
        private void Lst列表寻找列表内容button_Click(object sender, EventArgs e)
        {
            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = "寻找LST",
                Height = 545
            };
            Lst_XunZhao.button3.Text = "保存lst列表内容";
            Lst_XunZhao.label1.Text = "一行一个编号或路径";
            Lst_XunZhao.button3.Visible = true;
            Lst_XunZhao.label1.Visible = false;
            Lst_XunZhao.button1.Visible = false;
            Lst_XunZhao.button2.Visible = false;
            Lst_XunZhao.richTextBox1.Text = Lst_FileLstText;
            Lst_XunZhao.ShowDialog();
            if (Lst_FileLstText != "")
            {
                if (Lst列表寻找列表内容button.Text.Contains("☆") == false)
                    Lst列表寻找列表内容button.Text += "☆";
            }
            else
            {
                Lst列表寻找列表内容button.Text = "lst列表内容";
            }

        }

        private void Lst列表寻找被寻找的编号或路径button_Click(object sender, EventArgs e)
        {
            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = "寻找LST",
                Height = 545
            };
            Lst_XunZhao.button3.Text = "保存被寻找内容";
            Lst_XunZhao.label1.Text = "一行一个编号或路径";
            Lst_XunZhao.button3.Visible = true;
            Lst_XunZhao.label1.Visible = true;
            Lst_XunZhao.button1.Visible = true;
            Lst_XunZhao.button2.Visible = true;
            Lst_XunZhao.richTextBox1.Text = Lst_BeiXunZhaoText;
            Lst_XunZhao.ShowDialog();
            if (Lst_BeiXunZhaoText != "")
            {
                if (Lst列表寻找被寻找的编号或路径button.Text.Contains("☆") == false)
                    Lst列表寻找被寻找的编号或路径button.Text += "☆";
            }
            else
            {
                Lst列表寻找被寻找的编号或路径button.Text = "寻找的编号或路径";
            }


        }

        private void Lst列表寻找结果信息button_Click(object sender, EventArgs e)
        {
            string Text = "lst列表找到的信息";
            Regex_new regex = new Regex_new();
            regex.创建(Lst_EndYesText.ToString(), "\r\n", 0);
            if (regex.取匹配数量() > 0)
            {
                Text = "lst列表找到的信息；寻找总数：" + regex.取匹配数量() + "个";
            }

            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = Text,
                Height = 510
            };
            Lst_XunZhao.button3.Visible = false;
            Lst_XunZhao.label1.Visible = false;
            Lst_XunZhao.button1.Visible = false;
            Lst_XunZhao.button2.Visible = false;
            Lst_XunZhao.richTextBox1.Text = Lst_EndYesText.ToString();
            Lst_XunZhao.ShowDialog();


        }

        private void Lst列表寻找使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "在lst列表选择框内：\r\n" +
                "<lst列表内容>按钮点击后需放入例如：equipment/equipment.lst中的内容\r\n" +
                "<寻找的编号或路径>按钮点击后会弹出窗口并且有载入模板\r\n" +
                "<...>此按钮是储存上一次查询结果\r\n\r\n\r\n" +
                "此功能无使用多线程；改变了原有寻找代码极大的提升了查询速度；\r\n" +
                "测试4000多条在8W左右的lst列表中查询只需要10秒左右的时间";
            MessageBox.Show(Text, "Lst列表寻找使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void Lst列表寻找开始寻找button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供好lst列表以及需要寻找的内容，并且真的要开始寻找lst吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                bool YesNoXunZhaoLst = false;
                if (Lst_FileLstText != "")
                {
                    if (Lst_BeiXunZhaoText != "")
                    {
                        MysqlYz.XY(15);
                        YesNoXunZhaoLst = true;
                    }
                    else
                    {
                        MessageBox.Show("寻找的编号或路径不存在，请重新保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Lst列表寻找被寻找的编号或路径button.PerformClick();
                    }
                }
                else
                {
                    MessageBox.Show("lst列表内容不存在，请重新保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Lst列表寻找列表内容button.PerformClick();
                }

                if (YesNoXunZhaoLst)
                {
                    progressBar1.Value = 0;//进度条初始0
                    progressBar1.Visible = true;//进度条可视真
                    progressBar1.Maximum = 100;

                    System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                    time.Start();
                    progressBar1.Value = 10;//进度条进度增加
                    Lst_EndYesText.Clear();
                    Lst_EndNoText.Clear();
                    progressBar1.Value = 20;//进度条进度增加
                    Regex_new regex = new Regex_new();
                    Dnf dnf = new Dnf();
                    List<List<string>> PaiLieLstInfo = dnf.LstChongPaiLie(Lst_FileLstText);
                    progressBar1.Value = 50;//进度条进度增加

                    if (Lst列表寻找根据编号radioButton.Checked)
                    {
                        List<string> TextGetId = dnf.LstChongPaiLieGetId(Lst_BeiXunZhaoText);
                        progressBar1.Value = 70;//进度条进度增加
                        if (TextGetId.Count > 0)
                        {
                            foreach (string item in TextGetId)
                            {
                                int pos = PaiLieLstInfo[0].IndexOf(item);
                                if (pos != -1)
                                {
                                    Lst_EndYesText.Append(PaiLieLstInfo[0][pos] + "\t`" + PaiLieLstInfo[1][pos] + "`\r\n");
                                }
                                else
                                {
                                    Lst_EndNoText.Append(item + "\r\n");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("提供根据路径寻找的内容错误，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Lst列表寻找被寻找的编号或路径button.PerformClick();
                        }

                    }
                    else if (Lst列表寻找根据路径radioButton.Checked)
                    {
                        List<string> TextGetPath = dnf.LstChongPaiLieGetPath(Lst_BeiXunZhaoText);
                        progressBar1.Value = 70;//进度条进度增加
                        if (TextGetPath.Count > 0)
                        {
                            foreach (string item in TextGetPath)
                            {
                                int pos = PaiLieLstInfo[2].IndexOf(item);
                                if (pos != -1)
                                {
                                    Lst_EndYesText.Append(PaiLieLstInfo[0][pos] + "\t`" + PaiLieLstInfo[1][pos] + "`\r\n");
                                }
                                else
                                {
                                    Lst_EndNoText.Append(item + "\r\n");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("提供根据路径寻找的内容错误，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Lst列表寻找被寻找的编号或路径button.PerformClick();
                        }

                    }


                    if (Lst_EndYesText.Length > 0)
                    {
                        progressBar1.Value = 100;//进度条进度增加
                        regex.创建(Lst_EndYesText.ToString(), "\r\n", 0);
                        寻找lst列表 Lst_XunZhao = new 寻找lst列表
                        {
                            Text = $"lst列表找到的信息；寻找总数：{regex.取匹配数量()}个；总用时：{(double)time.ElapsedMilliseconds / 1000}秒",
                            Height = 499
                        };
                        Lst_XunZhao.button3.Visible = false;
                        Lst_XunZhao.label1.Visible = false;
                        Lst_XunZhao.button1.Visible = false;
                        Lst_XunZhao.button2.Visible = false;
                        Lst_XunZhao.richTextBox1.Text = Lst_EndYesText.ToString();
                        Lst_XunZhao.Show();
                    }

                    if (Lst_EndNoText.Length > 0)
                    {
                        regex.创建(Lst_EndNoText.ToString(), "\r\n", 0);
                        寻找lst列表 Lst_XunZhao = new 寻找lst列表
                        {
                            Text = "lst列表未找到的信息；未找到总数：" + regex.取匹配数量() + "个",
                            Height = 499
                        };
                        Lst_XunZhao.button3.Visible = false;
                        Lst_XunZhao.label1.Visible = false;
                        Lst_XunZhao.button1.Visible = false;
                        Lst_XunZhao.button2.Visible = false;
                        Lst_XunZhao.richTextBox1.Text = Lst_EndNoText.ToString();
                        Lst_XunZhao.Show();
                    }
                }
            }
            progressBar1.Visible = false;//进度条可视假
        }

        //——————————————lst列表寻找————————————————

        //——————————————lst列表加入————————————————

        private void Lst列表加入缓存button_Click(object sender, EventArgs e)
        {

            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = "加入lst缓存信息",
                Height = 510
            };
            Lst_XunZhao.button3.Visible = false;
            Lst_XunZhao.label1.Visible = false;
            Lst_XunZhao.button1.Visible = false;
            Lst_XunZhao.button2.Visible = false;
            Lst_XunZhao.richTextBox1.Text = AddLst_EndYesText.ToString();
            Lst_XunZhao.ShowDialog();
        }

        private void Lst列表加入使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "Lst列表加入注意事项：\r\n" +
                "<加入功能>\r\n" +
                "1、点击lst列表内容按钮，保存完整的lst列表内容\r\n" +
                "2、点击待加入的内容按钮，保存需要加入的内容\r\n" +
                "之后就可点击开始加入按钮加入完成；可自己选择性加入，最好是默认选项；\r\n\r\n" +
                "<正序排序>\r\n" +
                "点击lst列表内容按钮，保存完整的lst列表内容后即可点击正序排序按钮进行排序\r\n\r\n" +
                "<倒序排序>\r\n" +
                "点击lst列表内容按钮，保存完整的lst列表内容后即可点击倒序排序按钮进行排序\r\n\r\n" +
                "<打乱排序>\r\n" +
                "点击lst列表内容按钮，保存完整的lst列表内容后即可点击打乱排序按钮进行排序\r\n\r\n" +
                "注：请按以上正确操作避免意外错误，其中<打乱排序>不限制只是lst，可应用其他环境，打乱规则按照整行打乱。(速度极快)";
            MessageBox.Show(Text, "Lst列表加入使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void Lst列表加入lst内容button_Click(object sender, EventArgs e)
        {
            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = "加入LST",
                Height = 545
            };
            Lst_XunZhao.button3.Text = "保存lst列表内容";
            Lst_XunZhao.label1.Text = "一行一个编号指向路径";
            Lst_XunZhao.button3.Visible = true;
            Lst_XunZhao.label1.Visible = false;
            Lst_XunZhao.button1.Visible = false;
            Lst_XunZhao.button2.Visible = false;
            Lst_XunZhao.richTextBox1.Text = AddLst_FileLstText;
            Lst_XunZhao.ShowDialog();
            if (AddLst_FileLstText != "")
            {
                if (Lst列表加入lst内容button.Text.Contains("☆") == false)
                    Lst列表加入lst内容button.Text += "☆";
            }
            else
            {
                Lst列表加入lst内容button.Text = "lst列表内容";
            }
        }

        private void Lst列表待加入内容button_Click(object sender, EventArgs e)
        {
            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = "加入LST",
                Height = 545
            };
            Lst_XunZhao.button3.Text = "保存待加入内容";
            Lst_XunZhao.label1.Text = "一行一个编号指向路径";
            Lst_XunZhao.button3.Visible = true;
            Lst_XunZhao.Button4.Visible = true;
            Lst_XunZhao.label1.Visible = true;
            Lst_XunZhao.button1.Visible = false;
            Lst_XunZhao.button2.Visible = false;
            Lst_XunZhao.richTextBox1.Text = AddLst_BeiXunZhaoText;
            Lst_XunZhao.ShowDialog();
            if (AddLst_BeiXunZhaoText != "")
            {
                if (Lst列表待加入内容button.Text.Contains("☆") == false)
                    Lst列表待加入内容button.Text += "☆";
            }
            else
            {
                Lst列表待加入内容button.Text = "待加入的内容";
            }
        }

        private void Lst列表加入正序排序button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供好lst列表的内容，并且真的要开始正序排序lst吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (AddLst_FileLstText.Length <= 0)
            {
                MessageBox.Show("lst列表为空无法继续操作；请载入lst列表内容后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            AddLst_EndYesText = new Dnf().LstChongPaiLiePaiXu(AddLst_FileLstText, 0);

            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = $"加入LST；正序排序结果；总用时：{(double)time.ElapsedMilliseconds / 1000}秒",
                Height = 499
            };
            Lst_XunZhao.richTextBox1.Text = AddLst_EndYesText.ToString();

            time.Stop();
            Lst_XunZhao.ShowDialog();

        }

        private void Lst列表加入倒序排序button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供好lst列表的内容，并且真的要开始倒序排序lst吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (AddLst_FileLstText.Length <= 0)
            {
                MessageBox.Show("lst列表为空无法继续操作；请载入lst列表内容后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MysqlYz.XY(25);

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            MysqlYz.XY(25);

            AddLst_EndYesText = new Dnf().LstChongPaiLiePaiXu(AddLst_FileLstText, 1);

            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = $"加入LST；倒序排序结果；总用时：{(double)time.ElapsedMilliseconds / 1000}秒",
                Height = 499
            };
            Lst_XunZhao.richTextBox1.Text = AddLst_EndYesText.ToString();

            time.Stop();
            Lst_XunZhao.ShowDialog();
        }

        private void Lst列表加入打乱排序button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供好lst列表的内容，并且真的要开始打乱排序lst吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (AddLst_FileLstText.Length <= 0)
            {
                MessageBox.Show("lst列表为空无法继续操作；请载入lst列表内容后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MysqlYz.XY(25);

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            MysqlYz.XY(25);
            AddLst_EndYesText = new Dnf().LstChongPaiLiePaiXu(AddLst_FileLstText, 2);

            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = $"加入LST；打乱排序结果；总用时：{(double)time.ElapsedMilliseconds / 1000}秒",
                Height = 499
            };
            Lst_XunZhao.richTextBox1.Text = AddLst_EndYesText.ToString();

            time.Stop();
            Lst_XunZhao.ShowDialog();
        }

        private void Lst列表加入单行去重button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供好lst列表的内容，并且真的要开始单行去重吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (AddLst_FileLstText.Length <= 0)
            {
                MessageBox.Show("lst列表为空无法继续操作；请载入lst列表内容后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MysqlYz.XY(25);

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            List<string> NoC = new List<string>();

            //把所有不重复文本加入到集合中
            foreach (string item in AddLst_FileLstText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!NoC.Contains(item)) NoC.Add(item);
            }

            AddLst_EndYesText.Clear();//清除

            NoC.ForEach((Text) => AddLst_EndYesText.Append(Text + "\r\n"));//加入到字符串类中

            using 寻找lst列表 Lst_XunZhao = new 寻找lst列表
            {
                Text = $"单行去重；去重结果；总用时：{(double)time.ElapsedMilliseconds / 1000}秒",
                Height = 499
            };
            Lst_XunZhao.richTextBox1.Text = AddLst_EndYesText.ToString();

            time.Stop();
            Lst_XunZhao.ShowDialog();
        }

        private void Lst列表加入开始加入button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供好lst列表以及待加入的内容，并且真的要开始加入吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (AddLst_FileLstText.Length <= 0)
            {
                MessageBox.Show("lst列表为空无法继续操作；请载入后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (AddLst_BeiXunZhaoText.Length <= 0)
            {
                MessageBox.Show("待加入的lst为空无法继续操作；请载入后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MysqlYz.XY(25);

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            int IdHuoPath = 0;
            int DeleteYouMy = 0;
            if (Lst列表加入根据整行radioButton.Checked)
                IdHuoPath = 1;
            if (Lst列表加入删除自己radioButton.Checked)
                DeleteYouMy = 1;

            AddLst_EndYesText.Clear();

            Dnf dnf = new Dnf();

            StringBuilder ChongFuLst = new StringBuilder();//用于记录原lst重复的
            StringBuilder AddChongFuLst = new StringBuilder();//用于记录待加入重复的
            switch (IdHuoPath)
            {
                case 0:
                    Dictionary<string, string> IdPathLst = dnf.LstSplit(AddLst_FileLstText, ChongFuLst);
                    Dictionary<string, string> AddIdPathLst = dnf.LstSplit(AddLst_BeiXunZhaoText, AddChongFuLst);

                    foreach (string item in AddIdPathLst.Keys)
                    {
                        if (IdPathLst.ContainsKey(item))
                        {
                            switch (DeleteYouMy)
                            {
                                case 0:
                                    IdPathLst[item] = AddIdPathLst[item];
                                    continue;
                                case 1:
                                    continue;
                            }
                        }
                        else
                        {
                            IdPathLst.Add(item, AddIdPathLst[item]);
                        }
                    }

                    MysqlYz.XY(25);

                    foreach (string item in IdPathLst.Keys)
                    {
                        AddLst_EndYesText.Append(item + "\t" + IdPathLst[item] + "\r\n");
                    }

                    if (AddLst_EndYesText.ToString().StartsWith("#PVF_File") == false)
                        AddLst_EndYesText.Insert(0, "#PVF_File\r\n", 1);

                    break;
                case 1:
                    List<string> HangIdPathLst = new List<string>(AddLst_FileLstText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
                    List<string> HangAddIdPathLst = new List<string>(AddLst_BeiXunZhaoText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

                    List<string> HangIdPathLstX = new List<string>();
                    HangIdPathLst.ForEach(ToXiaoXie => HangIdPathLstX.Add(ToXiaoXie.ToLower().Replace("\\", "/")));
                    List<string> HangAddIdPathLstX = new List<string>();
                    HangAddIdPathLst.ForEach(ToXiaoXie => HangAddIdPathLstX.Add(ToXiaoXie.ToLower().Replace("\\", "/")));

                    //寻找重复的lst
                    dnf.寻找ChongFu(HangIdPathLstX, ChongFuLst);
                    dnf.寻找ChongFu(HangAddIdPathLstX, AddChongFuLst);

                    int Poss = 0;
                    HangAddIdPathLstX.ForEach(Text =>
                    {
                        int Pos = HangIdPathLstX.IndexOf(Text);
                        if (Pos != -1)
                        {
                            switch (DeleteYouMy)
                            {
                                case 0:
                                    HangIdPathLst[Pos] = HangAddIdPathLst[Poss];
                                    break;
                                case 1:
                                    break;
                            }
                        }
                        else
                        {
                            HangIdPathLst.Add(HangAddIdPathLst[Poss]);
                        }
                        Poss++;
                    });

                    MysqlYz.XY(25);

                    HangIdPathLst.ForEach(Text => AddLst_EndYesText.Append(Text + "\r\n"));
                    break;
            }

            寻找lst列表 Lst_XunZhao = null;
            Regex_new regex = new Regex_new();

            Lst_XunZhao = new 寻找lst列表
            {
                Text = $"加入LST；已完成待加入内容；总用时：{(double)time.ElapsedMilliseconds / 1000}秒",
                Height = 499
            };
            Lst_XunZhao.richTextBox1.Text = AddLst_EndYesText.ToString();
            time.Stop();
            Lst_XunZhao.Show();

            if (ChongFuLst.Length > 0)
            {
                regex.创建(ChongFuLst.ToString(), "\r\n", 0);
                Lst_XunZhao = new 寻找lst列表
                {
                    Text = $"Lst列表内容存在重复lst；已删除：{regex.取匹配数量()}个",
                    Height = 499
                };
                Lst_XunZhao.richTextBox1.Text = ChongFuLst.ToString();
                Lst_XunZhao.Show();
            }

            if (AddChongFuLst.Length > 0)
            {
                regex.创建(AddChongFuLst.ToString(), "\r\n", 0);
                Lst_XunZhao = new 寻找lst列表
                {
                    Text = $"待加入的内容存在重复lst；已删除：{regex.取匹配数量()}个",
                    Height = 499
                };
                Lst_XunZhao.richTextBox1.Text = AddChongFuLst.ToString();
                Lst_XunZhao.Show();
            }

        }

        //——————————————lst列表加入————————————————

        //——————————————设计图制作————————————————

        private void 设计图使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "注意事项：\r\n" +
                "<1>设计图制作为一行一个文件，请认真填写\r\n" +
                "<2>鼠标右键可以选择菜单，来设置行数；制作多少个设计图就设置多少行\r\n" +
                "<3>前三页是必填内容，最后一页是可额外扩展项目，适用于副职业设计图\r\n" +
                "<4>其中任何选项都可空；唯独写出文件名不可空；切记！！！\r\n" +
                "<5>选择导出到PVF时，导出目录应设置为以下例子：\r\n" +
                "stackable/recipe\r\n" +
                "如果设置不正确可能导致导出失败！！！\r\n\r\n" +
                "<6>鼠标右键可以查看详细信息，可查看一些常用设置；以及默认内容填充";
            MessageBox.Show(Text, "设计图制作使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 设计图导出目录maskedTextBox_Click(object sender, EventArgs e)
        {
            if (设计图导出到本地radioButton.Checked != true)
                return;
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = true,//设置只能选择文件夹
                Title = @"请选择导出设计图文件的目录；例如：E:\PVF\stackable"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                设计图导出目录maskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 设计图lst生成button_Click(object sender, EventArgs e)
        {
            using Lst列表生成 Form = new Lst列表生成() { Text = "设计图Lst列表生成" };
            Form.ShowDialog();
        }

        private void 设计图开始导出button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认设计图信息都填写完成，并且开始导出生成设计图文件？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (设计图导出目录maskedTextBox.Text == "")
            {
                MessageBox.Show("设计图导出目录不能为空，请设置一个目录作为导出目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                设计图导出目录maskedTextBox.Focus();
                return;
            }

            if (设计图导出到本地radioButton.Checked)
            {
                if (Directory.Exists(设计图导出目录maskedTextBox.Text) == false)
                {
                    MessageBox.Show("设计图导出目录不存在，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    设计图导出目录maskedTextBox.Focus();
                    return;
                }
            }
            else if (设计图导出到PVFradioButton.Checked)
            {
                //if (Directory.Exists(设计图导出目录maskedTextBox.Text) == false)
                //{
                //    MessageBox.Show("lst列表内容不存在，请重新保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    设计图导出目录maskedTextBox.Focus();
                //    return;
                //}
            }

            Grid_SheJiTu grid = new Grid_SheJiTu();

            int CuoWuPos = grid.验证FileName(reoGridControl1);
            if (CuoWuPos != 0)
            {
                MessageBox.Show($"写出的设计图文件名不能为空，或者重复；在第{CuoWuPos}行发生错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[0];//设置选择夹为第一个
                reoGridControl1.Worksheets[0].FocusPos = new CellPosition($"R{CuoWuPos + 1}");//获取焦点
                return;
            }

            string CuoWuPos2 = grid.验证CaiLiaoCount(reoGridControl1);
            if (CuoWuPos2 != "0")
            {
                string CuoWuType = CuoWuPos2.Remove(1);
                CuoWuPos2 = CuoWuPos2.Substring(1);
                switch (CuoWuType)
                {
                    case "0":
                        MessageBox.Show($"写出的设计图物品代码跟数量不能为空，请重新填写；在第{Convert.ToInt32(CuoWuPos2.Substring(1)) + 1}行发生错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case "1":
                        MessageBox.Show($"写出的设计图物品代码跟数量不正确；在第{Convert.ToInt32(CuoWuPos2.Substring(1)) + 1}行发生错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
                reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[Convert.ToInt32(CuoWuPos2.Remove(1))];//设置选择夹为第错误的
                reoGridControl1.Worksheets[Convert.ToInt32(CuoWuPos2.Remove(1))].FocusPos = new CellPosition($"A{Convert.ToInt32(CuoWuPos2.Substring(1)) + 1}");//获取焦点
                return;
            }

            string CuoWuPos3 = grid.验证AllText(reoGridControl1);
            if (CuoWuPos3 != "0")
            {
                MessageBox.Show($"检查中第一页存在空单元格，请认真填写后再写出；第{Convert.ToInt32(CuoWuPos3.Remove(1)) + 1}行、第{Convert.ToInt32(CuoWuPos3.Substring(1)) + 1}列发生错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[0];//设置选择夹为第错误的
                reoGridControl1.Worksheets[0].FocusPos = new CellPosition(Convert.ToInt32(CuoWuPos3.Remove(1)), Convert.ToInt32(CuoWuPos3.Substring(1)));//获取焦点
                return;
            }


            bool YesNoLst = false;
            if (MessageBox.Show("是否在生成设计图文件时同时也生成lst列表？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                YesNoLst = true;


            MysqlYz.XY(23);

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            bool YesNoFanTi = 设计图转为繁体checkBox.Checked;

            progressBar1.Value = 0;//进度条初始0
            progressBar1.Visible = true;//进度条可视真
            progressBar1.Maximum = 100;

            string MovePath = 设计图导出目录maskedTextBox.Text;


            Dictionary<string, string> SjtFileInfo = new Dictionary<string, string>();
            grid.AddSheJiTuInfo(reoGridControl1, SjtFileInfo);
            progressBar1.Value = 50;

            if (SjtFileInfo.Count > 0)
            {
                MysqlYz.XY(23);

                SheJiTuLstInfo.Clear();

                if (设计图导出到本地radioButton.Checked)
                {
                    foreach (string FileName in SjtFileInfo.Keys)
                    {
                        SheJiTuLstInfo.Add(FileName);
                        switch (YesNoFanTi)
                        {
                            case true:
                                File.WriteAllText(MovePath + "\\" + FileName, Strings.StrConv(SjtFileInfo[FileName], VbStrConv.TraditionalChinese, 0));
                                break;
                            case false:
                                File.WriteAllText(MovePath + "\\" + FileName, SjtFileInfo[FileName]);
                                break;
                        }
                    }

                }
                else if (设计图导出到PVFradioButton.Checked)
                {

                }

            }

            progressBar1.Value = 100;

            MessageBox.Show($"已完成设计图写出工作；总写出{SjtFileInfo.Count}个设计图；总用时：{(double)time.ElapsedMilliseconds / 1000}秒", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            progressBar1.Visible = false;//进度条可视真

            if (YesNoLst)
            {
                using Lst列表生成 Form = new Lst列表生成() { Text = "设计图Lst列表生成" };
                Form.ShowDialog();
            }
        }

        private void 填充默认必填ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int Col1;
            int Col2 = 0;
            string RowText1;
            string RowText2 = "";
            switch (e.ClickedItem.Text)
            {
                case "蓝字描述":
                    Col1 = 1;
                    RowText1 = "有一定機率製作成功。 成功時維持強化/增幅/提鍊/魔法屬性，裝備變成封裝狀態。 失敗時除了武器之外的材料道具會消失。";
                    break;
                case "掉落等级":
                    Col1 = 3;
                    RowText1 = "1";
                    break;
                case "物品品级":
                    Col1 = 4;
                    RowText1 = "3";
                    break;
                case "使用职业":
                    Col1 = 5;
                    RowText1 = "all";
                    break;
                case "交易类型":
                    Col1 = 6;
                    RowText1 = "trade";
                    break;
                case "使用等级":
                    Col1 = 7;
                    RowText1 = "1";
                    break;
                case "冷却时间":
                    Col1 = 8;
                    RowText1 = "1000";
                    break;
                case "购买价格":
                    Col1 = 9;
                    RowText1 = "1000000";
                    break;
                case "卖出价格":
                    Col1 = 10;
                    RowText1 = "5";
                    break;
                case "图标":
                    Col1 = 11;
                    Col2 = 12;
                    RowText1 = "Item/stackable/recipe.img";
                    RowText2 = "28";
                    break;
                case "掉落图标":
                    Col1 = 13;
                    Col2 = 14;
                    RowText1 = "Item/FieldImage.img";
                    RowText2 = "28";
                    break;
                case "移动声音":
                    Col1 = 15;
                    RowText1 = "PAPER_TOUCH";
                    break;
                case "设计图样式":
                    Col1 = 16;
                    RowText1 = "weaving";
                    break;
                default:
                    return;
            }

            if (Col1 + Col2 != 0)
            {
                if (Col1 > Col2)
                {
                    if (Col1 != 1)
                    {
                        for (int i = 0; i < reoGridControl1.Worksheets[0].RowCount; i++)
                        {
                            reoGridControl1.Worksheets[0][i, Col1] = RowText1;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < reoGridControl1.Worksheets[0].RowCount; i++)
                        {
                            reoGridControl1.Worksheets[0][i, Col1] = RowText1;
                            if (reoGridControl1.Worksheets[0].GetCellText(i, Col1 + 1) == "")
                                reoGridControl1.Worksheets[0][i, Col1 + 1] = "    ";
                        }
                    }


                }
                else if (Col1 < Col2)
                {
                    for (int i = 0; i < reoGridControl1.Worksheets[0].RowCount; i++)
                    {
                        reoGridControl1.Worksheets[0][i, Col1] = RowText1;
                        reoGridControl1.Worksheets[0][i, Col2] = RowText2;
                    }
                }
                reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[0];//设置当前表为第一页
                reoGridControl1.CurrentWorksheet.FocusPos = new CellPosition(0, Col1);//获取焦点
            }
        }

        private void 填充额外可选ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int Col1;
            int Col2 = 0;
            string RowText1;
            string RowText2 = "";
            switch (e.ClickedItem.Text)
            {
                case "图标边框":
                    Col1 = 0;
                    Col2 = 1;
                    RowText1 = "Item/IconMark.img";
                    RowText2 = "11";
                    break;
                case "物品堆叠":
                    Col1 = 4;
                    RowText1 = "1000";
                    break;
                case "成功率":
                    Col1 = 5;
                    RowText1 = "100";
                    break;
                case "公告":
                    Col1 = 6;
                    RowText1 = "1";
                    break;
                case "保留能力":
                    Col1 = 7;
                    RowText1 = "1";
                    break;
                case "随机强化":
                    Col1 = 10;
                    RowText1 = "use upgrade table";
                    break;
                default:
                    return;
            }

            if (Col1 + Col2 != 0)
            {
                if (Col1 > Col2)
                {
                    for (int i = 0; i < reoGridControl1.Worksheets[3].RowCount; i++)
                    {
                        reoGridControl1.Worksheets[3][i, Col1] = RowText1;
                    }

                }
                else if (Col1 < Col2)
                {
                    for (int i = 0; i < reoGridControl1.Worksheets[3].RowCount; i++)
                    {
                        reoGridControl1.Worksheets[3][i, Col1] = RowText1;
                        reoGridControl1.Worksheets[3][i, Col2] = RowText2;
                    }
                }
                reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[3];//设置当前表为第四页
                reoGridControl1.CurrentWorksheet.FocusPos = new CellPosition(0, Col1);//获取焦点
            }
        }

        private void 设计图表格contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            using var Form = new 表格设置行数();

            switch (e.ClickedItem.Text)
            {
                case "设置行数":
                    Form.ShowDialog();
                    if (Form.YesNo > 0)
                        new 表格控件.Grid_SheJiTu().设置行数(reoGridControl1, Form.YesNo);
                    return;
                case "重置设计图表格":
                    new 表格控件.Grid_SheJiTu().重置设计图表格(reoGridControl1);
                    return;
                case "查看详细信息":
                    new 设计图查看信息()
                    {
                        Text = "设计图查看信息(如要使用默认内容，请复制“”中间的内容)"
                    }.Show();
                    return;
                case "填充所有必填默认":
                    for (int i = 0; i < reoGridControl1.Worksheets[0].RowCount; i++)
                    {
                        reoGridControl1.Worksheets[0][i, 1] = "有一定機率製作成功。 成功時維持強化/增幅/提鍊/魔法屬性，裝備變成封裝狀態。 失敗時除了武器之外的材料道具會消失。";
                        reoGridControl1.Worksheets[0][i, 3] = "1";
                        reoGridControl1.Worksheets[0][i, 4] = "3";
                        reoGridControl1.Worksheets[0][i, 5] = "all";
                        reoGridControl1.Worksheets[0][i, 6] = "trade";
                        reoGridControl1.Worksheets[0][i, 7] = "1";
                        reoGridControl1.Worksheets[0][i, 8] = "1000";
                        reoGridControl1.Worksheets[0][i, 9] = "1000000";
                        reoGridControl1.Worksheets[0][i, 10] = "5";
                        reoGridControl1.Worksheets[0][i, 11] = "Item/stackable/recipe.img";
                        reoGridControl1.Worksheets[0][i, 12] = "28";
                        reoGridControl1.Worksheets[0][i, 13] = "Item/FieldImage.img";
                        reoGridControl1.Worksheets[0][i, 14] = "28";
                        reoGridControl1.Worksheets[0][i, 15] = "PAPER_TOUCH";
                        reoGridControl1.Worksheets[0][i, 16] = "weaving";
                        if (reoGridControl1.Worksheets[0].GetCellText(i, 2) == "")
                            reoGridControl1.Worksheets[0][i, 2] = "    ";
                    }
                    reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[0];//设置当前表为第一页
                    reoGridControl1.CurrentWorksheet.FocusPos = new CellPosition(0, 0);//获取焦点
                    return;
                case "填充所有默认额外可选":
                    for (int i = 0; i < reoGridControl1.Worksheets[3].RowCount; i++)
                    {
                        reoGridControl1.Worksheets[3][i, 0] = "Item/IconMark.img";
                        reoGridControl1.Worksheets[3][i, 1] = "11";
                        reoGridControl1.Worksheets[3][i, 4] = "1000";
                        reoGridControl1.Worksheets[3][i, 5] = "100";
                        reoGridControl1.Worksheets[3][i, 6] = "1";
                        reoGridControl1.Worksheets[3][i, 7] = "1";
                        reoGridControl1.Worksheets[3][i, 10] = "use upgrade table";
                    }
                    reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[3];//设置当前表为第四页
                    reoGridControl1.CurrentWorksheet.FocusPos = new CellPosition(0, 0);//获取焦点
                    return;
                default:
                    return;
            }
        }


        //——————————————设计图制作————————————————

        //——————————————任务制作————————————————

        private void 任务lst生成button_Click(object sender, EventArgs e)
        {
            using Lst列表生成 Form = new Lst列表生成() { Text = "任务Lst列表生成" };
            Form.ShowDialog();
        }

        private void 任务使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
                "注意事项：\r\n" +
                "<1>任务制作为一行一个文件，请认真填写\r\n" +
                "<2>鼠标右键可以选择菜单，来设置行数；制作多少个任务就设置多少行\r\n" +
                "<3>其中任何选项都可空；唯独写出文件名不可空；切记！！！\r\n" +
                "<4>选择导出到PVF时，导出目录应设置为以下例子：\r\n" +
                "n_quest/event\r\n" +
                "如果设置不正确可能导致导出失败！！！\r\n\r\n" +
                "<5>鼠标右键可以查看详细信息，可查看一些常用设置；以及默认内容填充";
            MessageBox.Show(Text, "任务制作使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 任务类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string AddText = "";
            switch (任务类型comboBox.SelectedIndex)
            {
                case 1:
                    AddText = "epic";
                    break;
                case 2:
                    AddText = "achievement";
                    break;
                case 3:
                    AddText = "daily";
                    break;
                case 4:
                    AddText = "common unique";
                    break;
                case 5:
                    AddText = "normaly repeat";
                    break;
                case 6:
                    AddText = "training";
                    break;
                case 7:
                    AddText = "urgent";
                    break;
                case 8:
                    AddText = "title";
                    break;
            }

            for (int i = 0; i < reoGridControl2.Worksheets[0].RowCount; i++)
            {
                reoGridControl2.Worksheets[0][i, 0] = AddText;
            }
            reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[0];//设置当前表为第一个
            reoGridControl2.CurrentWorksheet.FocusPos = new CellPosition(0, 0);//获取焦点
        }

        private void 完成条件类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (完成条件类型comboBox.SelectedIndex == 0)
                return;

            while (完成条件子类型comboBox.Items.Count != 1)
            {
                完成条件子类型comboBox.Items.RemoveAt(1);
            }

            string AddText1;
            string AddText2 = "";
            switch (完成条件类型comboBox.SelectedIndex)
            {
                case 1:
                    AddText1 = "meet npc";
                    AddText2 = "-1";
                    reoGridControl2.Worksheets[1].ColumnCount = 2;
                    reoGridControl2.Worksheets[1].ColumnCount = 2 + 1;
                    reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "NPC编号";
                    reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 80;
                    break;
                case 2:
                    AddText1 = "seeking";
                    reoGridControl2.Worksheets[1].ColumnCount = 1;
                    reoGridControl2.Worksheets[1].ColumnCount = 2;
                    reoGridControl2.Worksheets[1].ColumnHeaders[1].Text = "完成子类型";
                    reoGridControl2.Worksheets[1].ColumnHeaders[1].Width = 100;
                    完成条件子类型comboBox.Items.Add("正常收集物品");
                    完成条件子类型comboBox.Items.Add("真·宠物进阶(equ换equ)");
                    break;
                case 3:
                    AddText1 = "condition under clear";
                    reoGridControl2.Worksheets[1].ColumnCount = 1;
                    reoGridControl2.Worksheets[1].ColumnCount = 2;
                    reoGridControl2.Worksheets[1].ColumnHeaders[1].Text = "完成子类型";
                    reoGridControl2.Worksheets[1].ColumnHeaders[1].Width = 100;
                    完成条件子类型comboBox.Items.Add("限时通关副本");
                    完成条件子类型comboBox.Items.Add("被攻击次数不超出完成");
                    完成条件子类型comboBox.Items.Add("不使用复活币");
                    完成条件子类型comboBox.Items.Add("组队通关副本");
                    完成条件子类型comboBox.Items.Add("通关副本");
                    完成条件子类型comboBox.Items.Add("副本通关次数");
                    完成条件子类型comboBox.Items.Add("达到连击率百分比");
                    完成条件子类型comboBox.Items.Add("达到背击次数");
                    完成条件子类型comboBox.Items.Add("达到破招次数");
                    完成条件子类型comboBox.Items.Add("达到连击次数");
                    完成条件子类型comboBox.Items.Add("通过房间数");
                    完成条件子类型comboBox.Items.Add("副本内不超过使用消耗品次数");
                    完成条件子类型comboBox.Items.Add("达到群体攻击次数");
                    完成条件子类型comboBox.Items.Add("破坏建筑物次数以内");
                    break;
                case 4:
                    AddText1 = "hunt enemy";
                    AddText2 = "-1";
                    reoGridControl2.Worksheets[1].ColumnCount = 2;
                    reoGridControl2.Worksheets[1].ColumnCount = 2 + 15;
                    for (int i = 2; i < 17; i += 5)
                    {
                        reoGridControl2.Worksheets[1].ColumnHeaders[i].Text = "副本编号";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i].Width = 90;
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 1].Text = "副本难度";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 1].Width = 90;
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 2].Text = "敌人代码";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 2].Width = 90;
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 3].Text = "敌人类型";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 3].Width = 90;
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 4].Text = "杀敌数量";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 4].Width = 90;
                    }
                    break;
                case 5:
                    AddText1 = "hunt monster";
                    AddText2 = "-1";
                    reoGridControl2.Worksheets[1].ColumnCount = 2;
                    reoGridControl2.Worksheets[1].ColumnCount = 2 + 12;
                    for (int i = 2; i < 12; i += 4)
                    {
                        reoGridControl2.Worksheets[1].ColumnHeaders[i].Text = "副本编号";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i].Width = 90;
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 1].Text = "副本难度";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 1].Width = 90;
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 2].Text = "怪物代码";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 2].Width = 90;
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 3].Text = "杀怪数量";
                        reoGridControl2.Worksheets[1].ColumnHeaders[i + 3].Width = 90;
                    }
                    break;
                case 6:
                    AddText1 = "pvp rank";
                    AddText2 = "-1";
                    reoGridControl2.Worksheets[1].ColumnCount = 2;
                    reoGridControl2.Worksheets[1].ColumnCount = 2 + 1;
                    reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "PK等级";
                    reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 80;
                    break;
                case 7:
                    AddText1 = "clear map";
                    reoGridControl2.Worksheets[1].ColumnCount = 1;
                    reoGridControl2.Worksheets[1].ColumnCount = 2;
                    reoGridControl2.Worksheets[1].ColumnHeaders[1].Text = "完成子类型";
                    reoGridControl2.Worksheets[1].ColumnHeaders[1].Width = 100;
                    完成条件子类型comboBox.Items.Add("单个map房间");
                    完成条件子类型comboBox.Items.Add("多个map房间");
                    break;
                case 8:
                    AddText1 = "amplify item";
                    reoGridControl2.Worksheets[1].ColumnCount = 1;
                    reoGridControl2.Worksheets[1].ColumnCount = 2;
                    reoGridControl2.Worksheets[1].ColumnHeaders[1].Text = "完成子类型";
                    reoGridControl2.Worksheets[1].ColumnHeaders[1].Width = 100;
                    完成条件子类型comboBox.Items.Add("强化成功");
                    完成条件子类型comboBox.Items.Add("(碎)报废装备");
                    break;
                default:
                    return;
            }

            for (int i = 0; i < reoGridControl2.Worksheets[1].RowCount; i++)
            {
                reoGridControl2.Worksheets[1][i, 0] = AddText1;
            }

            if (AddText2 != "")
            {
                for (int i = 0; i < reoGridControl2.Worksheets[1].RowCount; i++)
                {
                    reoGridControl2.Worksheets[1][i, 1] = AddText2;
                }

                完成条件子类型comboBox.SelectedIndex = 0;
                完成条件子类型comboBox.Enabled = false;

            }
            else
            {
                完成条件子类型comboBox.SelectedIndex = 0;
                完成条件子类型comboBox.Enabled = true;
                reoGridControl2.Worksheets[1].ColumnCount = 2;
                for (int i = 0; i < reoGridControl2.Worksheets[1].RowCount; i++)
                {
                    reoGridControl2.Worksheets[1][i, 1] = "";
                }
            }

            if (完成条件类型comboBox.SelectedIndex != 2)
                new 表格控件.Grid_RenWu().宠物进化No(reoGridControl2, 完成奖励类型comboBox);

            reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[1];//设置当前表为第一个
            reoGridControl2.CurrentWorksheet.FocusPos = new CellPosition(0, 0);//获取焦点 
        }

        private void 完成条件子类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string AddText = "";
            switch (完成条件类型comboBox.SelectedIndex)
            {
                case 2:
                    switch (完成条件子类型comboBox.SelectedIndex)
                    {
                        case 1:
                            AddText = "-1";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 40;
                            for (int i = 2; i < 42; i += 2)
                            {
                                reoGridControl2.Worksheets[1].ColumnHeaders[i].Text = "物品代码";
                                reoGridControl2.Worksheets[1].ColumnHeaders[i].Width = 100;
                                reoGridControl2.Worksheets[1].ColumnHeaders[i + 1].Text = "物品数量";
                                reoGridControl2.Worksheets[1].ColumnHeaders[i + 1].Width = 100;
                            }
                            break;
                        case 2:
                            AddText = "-1";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 2;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "宠物Equ代码";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 110;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "宠物Equ数量(不可更改)";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 135;
                            for (int i = 0; i < reoGridControl2.Worksheets[1].RowCount; i++)
                            {
                                reoGridControl2.Worksheets[1][i, 3] = "1";
                            }

                            new 表格控件.Grid_RenWu().宠物进化Yes(reoGridControl2, 完成奖励类型comboBox);

                            break;
                    }
                    break;
                case 3:
                    switch (完成条件子类型comboBox.SelectedIndex)
                    {
                        case 1:
                            AddText = "0";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "多少毫秒";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 100;
                            break;
                        case 2:
                            AddText = "1";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "被攻击次数上限";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 130;
                            break;
                        case 3:
                            AddText = "4";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 2;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            break;
                        case 4:
                            AddText = "5";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "要求人数";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 100;
                            break;
                        case 5:
                            AddText = "6";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 2;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            break;
                        case 6:
                            AddText = "6";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "通关次数";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 100;
                            break;
                        case 7:
                            AddText = "7";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "连击率；百分比";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 130;
                            break;
                        case 8:
                            AddText = "8";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "背击次数";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 100;
                            break;
                        case 9:
                            AddText = "9";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "破招次数";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 100;
                            break;
                        case 10:
                            AddText = "10";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "连击次数";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 100;
                            break;
                        case 11:
                            AddText = "11";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "通过多少房间数";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 130;
                            break;
                        case 12:
                            AddText = "13";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 4;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "消耗品代码";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 110;
                            reoGridControl2.Worksheets[1].ColumnHeaders[5].Text = "消耗品使用次数";
                            reoGridControl2.Worksheets[1].ColumnHeaders[5].Width = 130;
                            break;
                        case 13:
                            AddText = "15";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 4;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "默认；不明";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 110;
                            reoGridControl2.Worksheets[1].ColumnHeaders[5].Text = "群体攻击次数";
                            reoGridControl2.Worksheets[1].ColumnHeaders[5].Width = 130;
                            for (int i = 0; i < reoGridControl2.Worksheets[1].RowCount; i++)
                            {
                                reoGridControl2.Worksheets[1][i, 4] = "-1";
                            }
                            break;
                        case 14:
                            AddText = "16";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 4;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "副本编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "副本难度";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "特效Obj代码";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 120;
                            reoGridControl2.Worksheets[1].ColumnHeaders[5].Text = "破坏次数上限";
                            reoGridControl2.Worksheets[1].ColumnHeaders[5].Width = 130;
                            break;
                    }
                    break;
                case 7:
                    switch (完成条件子类型comboBox.SelectedIndex)
                    {
                        case 1:
                            AddText = "-1";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 1;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "Map编号";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            break;
                        case 2:
                            AddText = "0";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 20;
                            for (int i = 2; i < 22; i++)
                            {
                                reoGridControl2.Worksheets[1].ColumnHeaders[i].Text = "Map编号";
                                reoGridControl2.Worksheets[1].ColumnHeaders[i].Width = 90;
                            }
                            break;
                    }
                    break;
                case 8:
                    switch (完成条件子类型comboBox.SelectedIndex)
                    {
                        case 1:
                            AddText = "0";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "强化等级";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "装备品级";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "装备等级";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 100;
                            break;
                        case 2:
                            AddText = "3";
                            reoGridControl2.Worksheets[1].ColumnCount = 2;
                            reoGridControl2.Worksheets[1].ColumnCount = 2 + 3;
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Text = "强化等级";
                            reoGridControl2.Worksheets[1].ColumnHeaders[2].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Text = "装备品级";
                            reoGridControl2.Worksheets[1].ColumnHeaders[3].Width = 100;
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Text = "装备等级";
                            reoGridControl2.Worksheets[1].ColumnHeaders[4].Width = 100;
                            break;
                    }
                    break;
                default:
                    return;
            }

            for (int i = 0; i < reoGridControl2.Worksheets[1].RowCount; i++)
            {
                reoGridControl2.Worksheets[1][i, 1] = AddText;
            }

            if (完成条件类型comboBox.SelectedIndex == 2 && 完成条件子类型comboBox.SelectedIndex != 2)
                new 表格控件.Grid_RenWu().宠物进化No(reoGridControl2, 完成奖励类型comboBox);

            reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[1];//设置当前表为第一个
            reoGridControl2.CurrentWorksheet.FocusPos = new CellPosition(0, 1);//获取焦点 
        }

        private void 完成奖励类型comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (完成奖励类型comboBox.SelectedIndex == 0)
                return;

            string AddText1 = "";
            string AddText2 = "";
            switch (完成奖励类型comboBox.SelectedIndex)
            {
                case 1:
                    AddText1 = "item";
                    reoGridControl2.Worksheets[2].ColumnCount = 1;
                    reoGridControl2.Worksheets[2].ColumnCount = 1 + 60;
                    for (int i = 1; i < 61; i += 2)
                    {
                        reoGridControl2.Worksheets[2].ColumnHeaders[i].Text = "物品代码";
                        reoGridControl2.Worksheets[2].ColumnHeaders[i].Width = 100;
                        reoGridControl2.Worksheets[2].ColumnHeaders[i + 1].Text = "物品数量";
                        reoGridControl2.Worksheets[2].ColumnHeaders[i + 1].Width = 80;
                    }
                    break;
                case 2:
                    AddText1 = "hell challenge";
                    AddText2 = "1";
                    reoGridControl2.Worksheets[2].ColumnCount = 2;
                    reoGridControl2.Worksheets[2].ColumnHeaders[1].Text = "固定；不可更改";
                    reoGridControl2.Worksheets[2].ColumnHeaders[1].Width = 130;
                    break;
            }

            for (int i = 0; i < reoGridControl2.Worksheets[2].RowCount; i++)
            {
                reoGridControl2.Worksheets[2][i, 0] = AddText1;
            }

            if (AddText2 != "")
            {
                for (int i = 0; i < reoGridControl2.Worksheets[2].RowCount; i++)
                {
                    reoGridControl2.Worksheets[2][i, 1] = AddText2;
                }
            }

            reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[2];//设置当前表为第三个
            reoGridControl2.CurrentWorksheet.FocusPos = new CellPosition(0, 0);//获取焦点 
        }

        private void 任务contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "填充默认必填" || e.ClickedItem.Text == "填充默认额外可选")
                return;

            using var Form = new 表格设置行数();

            switch (e.ClickedItem.Text)
            {
                case "设置行数":
                    Form.ShowDialog();
                    if (Form.YesNo > 0)
                        new Grid_SheJiTu().设置行数(reoGridControl2, Form.YesNo);
                    new Grid_SheJiTu().设置行数(reoGridControl3, Form.YesNo);
                    break;
                case "重置任务表格":
                    new Grid_RenWu().初始化任务表格框架(reoGridControl2, reoGridControl3);
                    break;
                case "查看详细信息":
                    new 设计图查看信息()
                    {
                        Text = "任务查看信息(如要使用默认内容，请复制“”中间的内容)"
                    }.Show();
                    break;
                case "填充所有默认必填":
                    for (int i = 0; i < reoGridControl2.Worksheets[0].RowCount; i++)
                    {
                        reoGridControl2.Worksheets[0][i, 1] = "2";
                        reoGridControl2.Worksheets[0][i, 2] = "-1";
                        reoGridControl2.Worksheets[0][i, 3] = "all";
                        reoGridControl2.Worksheets[0][i, 4] = "-1";
                        reoGridControl2.Worksheets[0][i, 5] = "1";
                        reoGridControl2.Worksheets[0][i, 6] = "99";
                    }

                    reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[0];//设置当前表为第一个
                    reoGridControl2.CurrentWorksheet.FocusPos = new CellPosition(0, 0);//获取焦点
                    break;
                case "填充所有默认额外可选":
                    for (int i = 0; i < reoGridControl2.Worksheets[3].RowCount; i++)
                    {
                        reoGridControl2.Worksheets[3][i, 0] = "1";
                        reoGridControl2.Worksheets[3][i, 1] = "1";
                    }

                    reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[3];//设置当前表为第四个
                    reoGridControl2.CurrentWorksheet.FocusPos = new CellPosition(0, 0);//获取焦点
                    break;
            }
        }

        private void 填充默认必填ToolStripMenuItem1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string AddText = "";
            int PosCol = 0;
            switch (e.ClickedItem.Text)
            {
                case "接取npc":
                    AddText = "2";
                    PosCol = 1;
                    break;
                case "完成npc":
                    AddText = "-1";
                    PosCol = 2;
                    break;
                case "接取职业":
                    AddText = "all";
                    PosCol = 3;
                    break;
                case "接取的职业类型":
                    AddText = "-1";
                    PosCol = 4;
                    break;
                case "最低接取等级":
                    AddText = "1";
                    PosCol = 5;
                    break;
                case "最高接取等级":
                    AddText = "99";
                    PosCol = 6;
                    break;
            }

            for (int i = 0; i < reoGridControl2.Worksheets[0].RowCount; i++)
                reoGridControl2.Worksheets[0][i, PosCol] = AddText;

            reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[0];//设置当前表为第一个
            reoGridControl2.CurrentWorksheet.FocusPos = new CellPosition(0, PosCol);//获取焦点 
        }

        private void 填充默认额外可选ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string AddText = "";
            int PosCol = 0;
            switch (e.ClickedItem.Text)
            {
                case "活动任务":
                    AddText = "1";
                    break;
                case "无法放弃":
                    AddText = "1";
                    PosCol = 1;
                    break;
            }
            for (int i = 0; i < reoGridControl2.Worksheets[3].RowCount; i++)
                reoGridControl2.Worksheets[3][i, PosCol] = AddText;

            reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[3];//设置当前表为第四个
            reoGridControl2.CurrentWorksheet.FocusPos = new CellPosition(0, PosCol);//获取焦点 
        }

        private void 任务导出目录maskedTextBox_Click(object sender, EventArgs e)
        {
            if (任务导出本地radioButton.Checked != true)
                return;
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = true,//设置只能选择文件夹
                Title = @"请选择导出任务文件的目录；例如：E:\PVF\n_quest"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                任务导出目录maskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 任务开始导出button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认任务信息都填写完成，并且开始导出生成任务文件？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (任务导出目录maskedTextBox.Text == "")
            {
                MessageBox.Show("任务导出目录不能为空，请设置一个目录作为导出目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                任务导出目录maskedTextBox.Focus();
                return;
            }

            if (任务导出本地radioButton.Checked)
            {
                if (Directory.Exists(任务导出目录maskedTextBox.Text) == false)
                {
                    MessageBox.Show("任务导出目录不存在，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    任务导出目录maskedTextBox.Focus();
                    return;
                }
            }
            else if (设计图导出到PVFradioButton.Checked)
            {
                //if (Directory.Exists(任务导出目录maskedTextBox.Text) == false)
                //{
                //    MessageBox.Show("lst列表内容不存在，请重新保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    设计图导出目录maskedTextBox.Focus();
                //    return;
                //}
            }

            Grid_RenWu grid = new Grid_RenWu();

            int CuoWuPos = grid.验证FileName(reoGridControl2);
            if (CuoWuPos != 0)
            {
                MessageBox.Show($"写出的任务文件名不能为空，或者重复；在第{CuoWuPos}行发生错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                reoGridControl2.CurrentWorksheet = reoGridControl2.Worksheets[0];//设置选择夹为第一个
                reoGridControl2.Worksheets[0].FocusPos = new CellPosition($"R{CuoWuPos + 1}");//获取焦点
                return;
            }

            string CuoWuPos2 = grid.验证AllText(reoGridControl2);
            if (CuoWuPos2 != "0")
            {
                MessageBox.Show($"检查中第一页存在空单元格，请认真填写后再写出；第{Convert.ToInt32(CuoWuPos2.Remove(1)) + 1}行、第{Convert.ToInt32(CuoWuPos2.Substring(1)) + 1}列发生错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[0];//设置选择夹为第错误的
                reoGridControl1.Worksheets[0].FocusPos = new CellPosition(Convert.ToInt32(CuoWuPos2.Remove(1)), Convert.ToInt32(CuoWuPos2.Substring(1)));//获取焦点
                return;
            }


            bool YesNoLst = false;
            if (MessageBox.Show("是否在生成任务文件时同时也生成lst列表？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                YesNoLst = true;

            MysqlYz.XY(26);

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            bool YesNoFanTi = 任务转为繁体checkBox.Checked;

            progressBar1.Value = 0;//进度条初始0
            progressBar1.Visible = true;//进度条可视真
            progressBar1.Maximum = 100;

            string MovePath = 任务导出目录maskedTextBox.Text;


            Dictionary<string, string> RwFileInfo = new Dictionary<string, string>();
            grid.AddSheJiTuInfo(reoGridControl2, reoGridControl3, RwFileInfo);
            progressBar1.Value = 50;

            if (RwFileInfo.Count > 0)
            {
                MysqlYz.XY(26);

                RenWuLstInfo.Clear();

                if (设计图导出到本地radioButton.Checked)
                {
                    foreach (string FileName in RwFileInfo.Keys)
                    {
                        RenWuLstInfo.Add(FileName);
                        switch (YesNoFanTi)
                        {
                            case true:
                                File.WriteAllText(MovePath + "\\" + FileName, Strings.StrConv(RwFileInfo[FileName], VbStrConv.TraditionalChinese, 0));
                                break;
                            case false:
                                File.WriteAllText(MovePath + "\\" + FileName, RwFileInfo[FileName]);
                                break;
                        }

                    }

                }
                else if (设计图导出到PVFradioButton.Checked)
                {

                }

            }

            progressBar1.Value = 100;

            MessageBox.Show($"已完成任务写出工作；总写出{RwFileInfo.Count}个任务；总用时：{(double)time.ElapsedMilliseconds / 1000}秒", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            progressBar1.Visible = false;//进度条可视假

            if (YesNoLst)
            {
                using Lst列表生成 Form = new Lst列表生成() { Text = "任务Lst列表生成" };
                Form.ShowDialog();
            }

        }

        //——————————————任务制作————————————————

        //——————————————etc套装加入————————————————
        private void Etc套装内容Button_Click(object sender, EventArgs e)
        {
            using 寻找lst列表 EtcInfo = new 寻找lst列表
            {
                Text = "Etc套装内容加入",
                Height = 545
            };
            EtcInfo.button3.Text = "保存Etc列表内容";
            EtcInfo.label1.Text = "直接粘贴PVF；Etc套装文件的所有内容";
            EtcInfo.button3.Visible = true;
            EtcInfo.label1.Visible = false;
            EtcInfo.button1.Visible = false;
            EtcInfo.button2.Visible = false;
            EtcInfo.richTextBox1.Text = EtcLstText;
            EtcInfo.ShowDialog();
            if (EtcLstText != "")
            {
                if (Etc套装内容Button.Text.Contains("☆") == false)
                    Etc套装内容Button.Text += "☆";
            }
            else
            {
                Etc套装内容Button.Text = "Etc套装内容信息";
            }
        }

        private void Etc套装待加入内容Button_Click(object sender, EventArgs e)
        {
            using 寻找lst列表 EtcInfo = new 寻找lst列表
            {
                Text = "Etc套装内容加入",
                Height = 545
            };
            EtcInfo.button3.Text = "保存Etc待加入内容";
            EtcInfo.label1.Text = "直接粘贴待加入的Etc套装内容";
            EtcInfo.button3.Visible = true;
            EtcInfo.label1.Visible = false;
            EtcInfo.button1.Visible = false;
            EtcInfo.button2.Visible = false;
            EtcInfo.richTextBox1.Text = AddEtcLstText;
            EtcInfo.ShowDialog();
            if (AddEtcLstText != "")
            {
                if (Etc套装待加入内容Button.Text.Contains("☆") == false)
                    Etc套装待加入内容Button.Text += "☆";
            }
            else
            {
                Etc套装待加入内容Button.Text = "待加入的套装信息";
            }
        }

        private void Etc套装缓存信息Button_Click(object sender, EventArgs e)
        {
            using 寻找lst列表 EtcInfo = new 寻找lst列表
            {
                Text = "加入完成的Etc套装缓存信息",
                Height = 510
            };
            EtcInfo.button3.Visible = false;
            EtcInfo.label1.Visible = false;
            EtcInfo.button1.Visible = false;
            EtcInfo.button2.Visible = false;
            EtcInfo.richTextBox1.Text = EndEtcLstText.ToString();
            EtcInfo.ShowDialog();
        }

        private void Etc套装使用说明Button_Click(object sender, EventArgs e)
        {
            string Text =
                "注意事项：\r\n" +
                "<1>复制PVF的Etc套装文件的所有内容保存到“Etc套装内容Button”按钮中；\r\n" +
                "文件路径为：etc/equipmentpartset.etc\r\n\r\n" +
                "<2>复制需要加入的套装信息；例如：\r\n" +
                "[equipment part set]\r\n3\t`character/partset/rareset.equ`\r\n`稀有時裝-帽子`\r\n`[hat avatar]`\r\n3\t2\t`稀有時裝-頭部`\r\n`[hair avatar]`\r\n3\t2\t`稀有時裝-臉部`\r\n`[face avatar]`\r\n3\t2\t`稀有時裝-胸部`\r\n`[breast avatar]`\r\n3\t2\t`稀有時裝-上衣`\r\n`[coat avatar]`\r\n3\t2\t`稀有時裝-下裝`\r\n`[pants avatar]`\r\n3\t2\t`稀有時裝-腰部`\r\n`[waist avatar]`\r\n3\t2\t`稀有時裝-鞋`\r\n`[shoes avatar]`\r\n3\t2\t\r\n[/equipment part set]\r\n\r\n" +
                "<3>最后选择遇到重复Etc编号套装属性时删除自己还是删除对方；一般为删除对方\r\n\r\n" +
                "注：pvfUtility格式必须改为为兼容模式";
            MessageBox.Show(Text, "Etc套装加入使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void Etc套装开始加入Button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供好Pvf的Etc套装文件、以及待加入内容，并且真的要开始加入吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (EtcLstText.Length <= 0)
            {
                MessageBox.Show("Etc套装内容为空无法继续操作；请载入后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (AddEtcLstText.Length <= 0)
            {
                MessageBox.Show("待加入Etc套装内容为空无法继续操作；请载入后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MysqlYz.XY(27);

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            int DeleteYouMy = 0;
            if (Etc套装删除对方radioButton.Checked)
                DeleteYouMy = 1;

            Dnf dnf = new Dnf();

            Dictionary<string, string> EtcAllInfo = dnf.Etc套装整理(EtcLstText);
            Dictionary<string, string> AddEtcAllInfo = dnf.Etc套装整理(AddEtcLstText);

            foreach (string item in AddEtcAllInfo.Keys)
            {
                if (EtcAllInfo.ContainsKey(item))
                {
                    if (DeleteYouMy == 0)
                        continue;

                    EtcAllInfo[item] = AddEtcAllInfo[item];
                }
                else
                {
                    EtcAllInfo.Add(item, AddEtcAllInfo[item]);
                }
            }

            EndEtcLstText.Clear().Append("#PVF_File\r\n\r\n");

            foreach (string item in EtcAllInfo.Values)
                EndEtcLstText.Append(item);


            using 寻找lst列表 EtcInfo = new 寻找lst列表
            {
                Text = $"加入Etc套装信息；已完成，总用时：{(double)time.ElapsedMilliseconds / 1000}秒",
                Height = 499
            };
            EtcInfo.richTextBox1.Text = EndEtcLstText.ToString();

            time.Stop();
            EtcInfo.ShowDialog();
        }


        //——————————————etc套装加入————————————————


        //——————————————修复动作裸体————————————————
        private void 修复裸体动作文件maskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = true,//设置只能选择文件夹
                Title = @"请选择需要修复的动作文件目录；例如：E:\PVF\character\swordman\animation"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                修复裸体动作文件maskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 修复裸体导出目录maskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = true,//设置只能选择文件夹
                Title = @"请选择修复完后并导出的所在目录；例如：E:\PVF\已修复文件"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                修复裸体导出目录maskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }

        private void 修复裸体使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
            "注意事项：\r\n" +
            "<1>使用pvfUtility工具导出.Chr文件内的ani文件，其余als附加的不要导出；不支持多职业，只支持单职业生成。\r\n" +
            "<2>导出完后选择动作文件文件目录以及生产目录 自动生成equipment目录下所有时装动作文件。\r\n" +
            "<3>生成完后导入到PVF中，并且在对应职业.lay中加入列表即可，跟.chr内的etc motion一样的序号顺序即可";
            MessageBox.Show(Text, "修复裸体使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void 修复裸体本地生成button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供好动作ani文件目录、以及导出目录，并且真的要开始修复并生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (修复裸体动作文件maskedTextBox.Text == "")
            {
                MessageBox.Show("您没有提供需要修复的动作ani文件目录；请填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                修复裸体动作文件maskedTextBox.Focus();
                return;
            }
            else if (Directory.Exists(修复裸体动作文件maskedTextBox.Text) == false)
            {
                MessageBox.Show("您提供需要修复的动作ani文件目录不存在；请重新填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                修复裸体动作文件maskedTextBox.Focus();
                return;
            }

            if (修复裸体导出目录maskedTextBox.Text == "")
            {
                MessageBox.Show("您没有提供修复完成后的导出目录；请填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                修复裸体导出目录maskedTextBox.Focus();
                return;
            }
            else if (Directory.Exists(修复裸体导出目录maskedTextBox.Text) == false)
            {
                MessageBox.Show("您提供修复完成后的导出目录不存在；请重新填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                修复裸体导出目录maskedTextBox.Focus();
                return;
            }

            string AniPath = 修复裸体动作文件maskedTextBox.Text;
            string DaoChuPath = 修复裸体导出目录maskedTextBox.Text;
            int ChrId = 修复裸体选择角色comboBox.SelectedIndex;

            if (ChrId == -1)
            {
                MessageBox.Show("您没有选择需要修复的角色；请选择后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MysqlYz.XY(28);

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            Dnf dnf = new Dnf();

            var ActFile = new Dictionary<string, 修复裸体>();//创建字典

            dnf.修复裸体ActFileZl(AniPath, ActFile);//遍历一遍需要修复的文件 并且加入到字典中

            dnf.修复裸体生成修复File(DaoChuPath, ActFile, dnf.修复裸体GetChrKey(ChrId));//开始导出


            if (MessageBox.Show($"已完成所有的Chr动作ani文件的修复工作；\r\n总用时：{(double)time.ElapsedMilliseconds / 1000}秒\r\n是否打开修复完成导出的目录？", "完成", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                new DingWei().Explorer(DaoChuPath);

            time.Stop();
        }


        //——————————————修复动作裸体————————————————

        //——————————————Nut打乱————————————————
        private void Nut打乱所在目录maskedTextBox_Click(object sender, EventArgs e)
        {
            using CommonOpenFileDialog file_folder = new CommonOpenFileDialog
            {
                InitialDirectory = ApiDirectoryPath,
                IsFolderPicker = true,//设置只能选择文件夹
                Title = @"请选择需要打乱的文件目录；例如：E:\PVF\sqr"//设置标题
            };
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                Nut打乱所在目录maskedTextBox.Text = file_folder.FileName;//获取的路径加入文本框
                ApiDirectoryPath = file_folder.FileName;//获取的路径加入静态全局
            }
        }
        private void Nut打乱使用说明button_Click(object sender, EventArgs e)
        {
            string Text =
            "注意事项：\r\n" +
            "";
            MessageBox.Show(Text, "Nut打乱使用说明", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        private void Nut打乱开始打乱button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否已经提供并设置好需求选项及目录，并且真的要开始打乱nut文件吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;

            if (Nut打乱所在目录maskedTextBox.Text == "")
            {
                MessageBox.Show("您没有提供需要打乱nut文件的目录；请填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Nut打乱所在目录maskedTextBox.Focus();
                return;
            }
            else if (Directory.Exists(Nut打乱所在目录maskedTextBox.Text) == false)
            {
                MessageBox.Show("您提供需要打乱nut文件的目录不存在；请重新填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Nut打乱所在目录maskedTextBox.Focus();
                return;
            }

            if(Nut打乱扩展名comboBox.Text=="" && Nut打乱扩展名comboBox.SelectedIndex==-1)
            {
                MessageBox.Show("抱歉您没有提供寻找文件的扩展名；请重新填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Nut打乱扩展名comboBox.Focus();
                return;
            }

            if(Nut打乱打开编码comboBox.SelectedIndex==-1)
            {
                MessageBox.Show("抱歉您没有提供打开文件所需的文件编码；请重新填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(Nut打乱变量名文本maskedTextBox.Text=="")
            {
                MessageBox.Show("抱歉您没有提供打乱的变量名文本；请重新填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Nut打乱变量名文本maskedTextBox.Focus();
                return;
            }
            else
            {
                bool Yes = false;
                int Len = Nut打乱变量名文本maskedTextBox.Text.Length;
                Regex_new regex = new Regex_new();
                regex.创建(Nut打乱变量名文本maskedTextBox.Text,"[0-9a-z_]+",1);
                if (regex.取匹配数量() > 0)
                    if (Len == regex.取匹配文本(1).Length)
                        Yes = true;

                if(Yes==false)
                {
                    MessageBox.Show("抱歉您提供打乱的变量名文本带有非法字符；请重新填写后再进行操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Nut打乱变量名文本maskedTextBox.Focus();
                    return;
                }
            }

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            string Path = Nut打乱所在目录maskedTextBox.Text;//文件目录
            string FileExName = Nut打乱扩展名comboBox.Text;//扩展名
            string coding = Nut打乱打开编码comboBox.SelectedItem.ToString();//文件编码
            string AddText = Nut打乱加入文本textBox.Text;//加入的文本
            string AddGG = Nut打乱变量名文本maskedTextBox.Text;//打乱的变量名
            Dnf dnf = new Dnf();

            dnf.SqrNut打乱(Path, FileExName, coding, AddText, AddGG);

            MessageBox.Show($"已完成所有的nut文件打乱工作；\r\n总用时：{(double)time.ElapsedMilliseconds / 1000}秒", "完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            time.Stop();
        }
        //——————————————Nut打乱————————————————


        //测试按钮
        private void CeShi_button_Click(object sender, EventArgs e)
        {

            //CheckForIllegalCrossThreadCalls = false;
            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();

            //string oo = "10000\t3171\t1\t0\t700000001\t100\t1\t0\t700000002\t500\t1\t0\t700000003\t1000\t1\t0\t700000004\t2000\t1\t0\t700000005\t10000\t1\t0\t700000006\t200000\t1\t0\t700000007\t700000\t1\t0\t700000008\t20000\t1\t0\t700000009\t50000\t1\t0\t700000010\t16400\t1\t0\t";
            string oo = "10000\t3171\t1\t0\t700000001\t188887\t1\t0\t700000002\t123123\t1\t0\t700000003\t23\t1\t0\t700000004\t4\t1\t0\t700000005\t8778\t1\t0\t700000006\t87878\t1\t0\t700000007\t458\t1\t0\t700000008\t123\t1\t0\t700000009\t89899\t1\t0\t700000010\t1124\t1\t0\t";




            string ii = "3171\t炉岩碳\r\n700000001\t测试1\r\n700000002\t测试2\r\n700000003\t测试3\r\n700000004\t测试4\r\n700000005\t测试5\r\n700000006\t测试6\r\n700000007\t测试7\r\n700000008\t测试8\r\n700000009\t测试9\r\n700000010\t测试10\r\n";


            抽奖.LotteryMoNi moNi = new 抽奖.LotteryMoNi();
            var info = moNi.StartMoHe(ii, oo, 1);

            StringBuilder builder = new StringBuilder();
            foreach (var item in info.Values)
            {
                builder.Append(item.GoodsName + "\t" + item.GoodsId + "\t" + item.GoodsProbability + "\t" + item.GoodsNumber + "\t" + item.GoodsNotice + "\t" + item.GoodsCount + "次\r\n");
            }


            MessageBox.Show(builder.ToString(), "询问", MessageBoxButtons.OK, MessageBoxIcon.None);
            time.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            time.Start();


            var ppp = "123456".Split(new string[] { "" }, StringSplitOptions.None);

            foreach (var item in ppp)
            {
                richTextBox1.AppendText(item + "\r\n");
            }

            MessageBox.Show((double)time.ElapsedMilliseconds / 1000 + "秒", "询问", MessageBoxButtons.OK, MessageBoxIcon.None);
            time.Stop();

        }

        private void CeShi_Click(object sender, EventArgs e)
        {


        }


    }



}
