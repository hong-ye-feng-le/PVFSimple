using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;


/* 基本知识
 * 注释快捷键：
 * ctrl+k+c 加入选中注释
 * ctrl+k+u 取消选中注释
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

namespace PVFSimple
{
    
    public partial class Form1 : Form
    {
        //正则
        regex_new regex = new regex_new();

        //创建选择文件或文件夹对话框
        CommonOpenFileDialog file_folder = new CommonOpenFileDialog();

        //窗口批量载入编号以及全路径
        批量载入 IDpath = new 批量载入();

        public Form1()
        {
            InitializeComponent();
        }
        //程序启动时的操作
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //程序即将关闭前的操作
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        //——————————————副本提取功能区————————————————

        //批量载入编号或全路径
        private void 副本编号textbutton_Click(object sender, EventArgs e)
        {
            IDpath.button1.Text = "导出副本编号";
            IDpath.ShowDialog();
            if (IDpath.richTextBox1.Text!="")
            {
                regex.创建(IDpath.richTextBox1.Text, "(.+)(\n|\r\n|)", 0);
                if (regex.取匹配数量()>0)
                {
                    string text = "";
                    foreach (Match item in regex.正则返回集合())
                    {
                        text = text + "-" + item.Groups[1].Value;
                    }

                    副本编号.Text = text.Substring(1);
                }
            }
        }

        //方便用户操作获取目录路径;打开显示选择文件夹对话框
        private void PVF文件所在目录_MouseDown(object sender, MouseEventArgs e)
        {

            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                PVF文件所在目录.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        //方便用户操作获取目录路径;打开显示选择文件夹对话框
        private void 提取并导出的目录_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择提取完成后需要导出的目录；例如：E:\DNF\提取输出目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                提取并导出的目录.Text = file_folder.FileName;//获取的路径加入文本框
            }

        }

        private void 开始提取副本_Click(object sender, EventArgs e)
        {

            bool 是否可执行提取副本 = false;


            //开始初始判断是否提取特效
            if (副本编号.Text != "")
            {
                if (PVF文件所在目录.Text != "")
                {

                    if (Directory.Exists(PVF文件所在目录.Text) == true)
                    {

                        if (Directory.Exists(PVF文件所在目录.Text + @"\dungeon") == true)
                        {

                            if (提取并导出的目录.Text != "")
                            {

                                if (Directory.Exists(提取并导出的目录.Text) == true)
                                {
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
            if (是否可执行提取副本 == true)
            {
                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch xhtime = new System.Diagnostics.Stopwatch();
                time.Start();

                double dgntqtime = 0;
                double mobobjapcxhtime = 0;
                double imgtime = 0;

                regex_new regex = new regex_new();
                dnf dnf = new dnf();

                string pvfpath = PVF文件所在目录.Text.ToLower();
                string movepath = 提取并导出的目录.Text;

                string dgn_lst_alltext = "";
                string map_lst_alltext = "";
                string mob_lst_alltext = "";
                string obj_lst_alltext = "";
                string apc_lst_alltext = "";

                //读入副本lst列表内容
                if (File.Exists(pvfpath + @"\dungeon\dungeon.lst") == true)
                {
                    dgn_lst_alltext = File.ReadAllText(pvfpath + @"\dungeon\dungeon.lst").Replace("/",@"\");
                }
                //读入地图lst列表内容
                if (File.Exists(pvfpath + @"\map\map.lst") == true)
                {
                    map_lst_alltext = File.ReadAllText(pvfpath + @"\map\map.lst").Replace("/", @"\");
                }
                //读入怪物lst列表内容
                if (File.Exists(pvfpath + @"\monster\monster.lst") == true)
                {
                    mob_lst_alltext = File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", @"\");
                }
                //读入特效lst列表内容
                if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst") == true)
                {
                    obj_lst_alltext = File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", @"\");
                }
                //读入人偶lst列表内容
                if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst") == true)
                {
                    apc_lst_alltext = File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", @"\");
                }
                

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
                regex.创建(副本编号.Text,"[0-9]+",0);
                regex.匹配加入集合(dgn_id,false,0);

                //通过副本编号获取lst 跟全路径
                dnf.id_lst_副本(pvfpath, dgn_lst_alltext, dgn_id, dgn_lst, dgn_path);

                //获取副本文件内的深渊map编号以及门的obj编号
                dnf.寻找编号(dgn_path, map_id,obj_id,null,null);

                //获取副本文件内的tbl、ani全路径
                dnf.寻找全路径(pvfpath, dgn_path,tbl_path,null,null,ani_path,null,null,null,null,null);

                //如果从副本文件内找到深渊map编号
                if (map_id.Count>0)
                {
                    foreach (var item in map_id)//从lst文件中查找出编号对应的lst，存在就加入
                    {
                        regex.创建(map_lst_alltext,"\r\n" + item.ToString() + "`(.+\\.map)`",1);
                        if (regex.取匹配数量()>0)
                        {
                            map_path.Add(pvfpath + "\\map\\" + regex.取子匹配文本(1,1));
                        }
                    }
                }


                //开始枚举map文件，查找副本对应的map文件路径
                List<string> map临时 = new List<string>(Directory.EnumerateFiles(pvfpath+"\\map", "*.map", SearchOption.AllDirectories));
                foreach (var item in map临时)
                {
                    string mappath = item.ToString().ToLower();//取出全路径并且转为小写
                    string mapalltext = File.ReadAllText(mappath);//读入所有内容

                    if (mapalltext.Contains("[dungeon]") == true)//如果找到了副本标签
                    {
                        foreach (var dgnid in dgn_id)//获取用户提供的副本ID
                        {
                            //正则匹配副本编号
                            regex.创建(mapalltext, @"\[dungeon\](\r\n|\r\n.+\t)" + dgnid + "\t",0);
                            if (regex.取匹配数量()>0)//如果匹配数量大于0
                            {
                                map_path.Add(mappath);//加入map全路径
                               
                            }
                        }
                    }
                }

                //获取map文件中的 ani、til
                dnf.寻找全路径(pvfpath,map_path,null,null,null,ani_path,null,til_path,null,null,null);

                //通过枚举寻找出来的map全路径，获取map的编号以及lst
                dnf.id_lst_地图(map_lst_alltext,map_id,map_lst,map_path);

                //通过map全路径查找出 人偶、怪物、特效编号
                dnf.寻找编号(map_path,null,obj_id,mob_id,apc_id);

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

                        dnf.怪物寻找编号(pvfpath, mob_lst_alltext, mob_id,mob_lst,mob_path, obj_id, apc_id,act_path,ptl_path);
                        dnf.特效寻找编号(pvfpath, obj_lst_alltext, obj_id,obj_lst,obj_path, mob_id, apc_id,act_path, ptl_path);
                        dnf.人偶寻找编号(pvfpath, apc_lst_alltext, apc_id,apc_lst,apc_path, mob_id, obj_id,act_path, ptl_path,key_path);

                    } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                }
                mobobjapcxhtime = xhtime.ElapsedMilliseconds;
                xhtime.Stop();

                //获取mob文件中的ani、tbl、ai、equani、atk
                dnf.寻找全路径(pvfpath,mob_path,tbl_path,null,atk_path,ani_path,null,null,null,ai_path,mob_equ_path);

                //获取obj文件中的ani、til、atk
                dnf.寻找全路径(pvfpath, obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);

                //如果副本内有人偶就获取ai、key
                if (apc_id.Count>0)
                {
                    dnf.寻找全路径(pvfpath, apc_path, null, null, null, null, null, null, key_path, ai_path, null);
                }

                //获取act中的atk、ani
                dnf.寻找全路径(pvfpath, act_path, null, null, atk_path, ani_path, null, null, null, null, null);

                //获取ptl中的ani
                dnf.寻找全路径(pvfpath, ptl_path, null, null, null, ani_path, null, null, null, null, null);

                //寻找ani的als
                dnf.寻找als(ani_path,als_path,2,false);

                //寻找ai
                dnf.寻找ai(ai_path);

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
                if (dgn_lst.Count>0)
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "----------------------------------以下内容加入PVF【dungeon/dungeon.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【"+ dgn_lst.Count.ToString()+ "】-------------------------------------------------------\r\n";
                    foreach (var item in dgn_lst)
                    {
                        查找出来的lst列表内容 = 查找出来的lst列表内容 + item.ToString() + "\r\n";
                    }
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "\r\n\r\n";
                }
                else
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "未找到副本lst\r\n\r\n\r\n";
                }

                if (map_lst.Count > 0)
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "----------------------------------以下内容加入PVF【map/map.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + map_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (var item in map_lst)
                    {
                        查找出来的lst列表内容 = 查找出来的lst列表内容 + item.ToString() + "\r\n";
                    }
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "\r\n\r\n";
                }
                else
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "未找到地图lst\r\n\r\n\r\n";
                }

                if (mob_lst.Count > 0)
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (var item in mob_lst)
                    {
                        查找出来的lst列表内容 = 查找出来的lst列表内容 + item.ToString() + "\r\n";
                    }
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "\r\n\r\n";
                }
                else
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "未找到怪物lst\r\n\r\n\r\n";
                }

                if (obj_lst.Count > 0)
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (var item in obj_lst)
                    {
                        查找出来的lst列表内容 = 查找出来的lst列表内容 + item.ToString() + "\r\n";
                    }
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "\r\n\r\n";
                }
                else
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "未找到特效lst\r\n\r\n\r\n";
                }

                if (apc_lst.Count > 0)
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (var item in apc_lst)
                    {
                        查找出来的lst列表内容 = 查找出来的lst列表内容 + item.ToString() + "\r\n";
                    }
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "\r\n\r\n";
                }
                else
                {
                    查找出来的lst列表内容 = 查找出来的lst列表内容 + "未找到人偶lst\r\n\r\n\r\n";
                }


                progressBar1.Maximum = end_all_path.Count;
                progressBar1.Value = 0;

                string endpath;
                string endmovepath;
                string moveDirectory;

                //开始输出所有文件
                foreach (var item in end_all_path)
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

                    ++progressBar1.Value;
                }

                progressBar1.Value = progressBar1.Maximum;

                //输出lst列表内容,并且\替换为/
                File.WriteAllText(movepath + "\\(4)查找出来的lst列表.txt", 查找出来的lst列表内容.Replace("\\","/"));

                //结束副本提取测量时间
                dgntqtime = time.ElapsedMilliseconds;

                //根据用户选择查找img路径，并获取img提取用时
                if (副本提取完后查找img路径.Checked == true)
                {
                    time.Restart();
                    string imgpath = "";
                    string npkimgpath = "";
                    string npkpath = "";
                    dnf.寻找img路径(movepath,"*.*",false, ref imgpath,ref npkimgpath,ref npkpath);

                    if (imgpath!="")
                    {
                        File.WriteAllText(movepath + "\\(1)未更改的img全路径.txt", imgpath);
                    }
                    if (npkimgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(2)npk当中的img全路径.txt", npkimgpath);
                    }
                    if (npkpath != "")
                    {
                        File.WriteAllText(movepath + "\\(3)转换为npk全名.txt", npkpath);
                    }

                    //获取查找img路径的时间
                    imgtime = time.ElapsedMilliseconds;
                    time.Stop();
                }
                else
                {
                    time.Stop();
                }

                //弹出信息框，结束提取副本，判断img提取时间是否存在
                if (imgtime==0)
                {
                    
                    MessageBox.Show("已完成指定的副本提取工作；\r\n循环遍历mob、obj、apc编号用时：" + mobobjapcxhtime.ToString() + "毫秒；\r\n提取副本总用时：" + dgntqtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    
                    MessageBox.Show("已完成指定的副本提取工作；\r\n循环遍历mob、obj、apc编号用时：" + mobobjapcxhtime.ToString() + "毫秒；\r\n提取副本总用时：" + dgntqtime.ToString() + "毫秒；\r\nimg路径查找用时：" + imgtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                

                //提取完后根据用户选择是否定位导出后的目录
                if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe",movepath );
                }
                
            }
        }


        //——————————————副本提取功能区————————————————
        
        //——————————————城镇提取功能区————————————————
        private void 开始提取城镇_Click(object sender, EventArgs e)
        {
            bool 是否可执行提取城镇 = false;

            //开始初始判断是否提取城镇
            if (城镇编号txt.Text != "")
            {
                if (城镇pvf文件txt.Text != "")
                {

                    if (Directory.Exists(城镇pvf文件txt.Text) == true)
                    {

                        if (Directory.Exists(城镇pvf文件txt.Text + @"\town") == true)
                        {

                            if (城镇导出目录txt.Text != "")
                            {

                                if (Directory.Exists(城镇导出目录txt.Text) == true)
                                {
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
                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                time.Start();

                long twntime = 0;
                long imgtime = 0;

                regex_new regex = new regex_new();
                dnf dnf = new dnf();

                string pvfpath = 城镇pvf文件txt.Text.ToLower();
                string movepath = 城镇导出目录txt.Text.ToLower();

                string twn_lst_alltext = "";
                string obj_lst_alltext = "";
                string npc_lst_alltext = "";

                if (File.Exists(pvfpath + @"\town\town.lst") == true)
                {
                    twn_lst_alltext = File.ReadAllText(pvfpath + @"\town\town.lst").Replace("/", "\\");
                }

                if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst") == true)
                {
                    obj_lst_alltext = File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", "\\");
                }

                if (File.Exists(pvfpath + @"\npc\npc.lst") == true)
                {
                    npc_lst_alltext = File.ReadAllText(pvfpath + @"\npc\npc.lst").Replace("/", "\\");
                }

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
                regex.匹配加入集合(twn_id,false,0);

                dnf.id_lst_城镇(pvfpath, twn_lst_alltext, twn_id, twn_lst, twn_path);

                if (twn_path.Count>0)
                {
                    dnf.twn寻找map_ani(pvfpath, twn_path, map_path, ani_path,true);
                    dnf.城镇map寻找id和路径(map_path, obj_id,npc_id,ani_path,til_path);


                    if (npc_id.Count>0)
                    {
                        dnf.id_lst_npc(pvfpath, npc_lst_alltext, npc_id, npc_lst, npc_path);
                        dnf.寻找全路径(pvfpath, npc_path, null, null, null, ani_path, null, null, null, null, null);
                    }

                    if (obj_id.Count > 0)
                    {
                        dnf.id_lst_特效(pvfpath, obj_lst_alltext, obj_id, obj_lst, obj_path);
                        dnf.特效寻找编号(pvfpath, obj_lst_alltext, obj_id, obj_lst, obj_path, null, null, act_path, ptl_path);
                        dnf.寻找全路径(pvfpath, obj_path, null, null, atk_path, ani_path, null, til_path, null, null, null);
                        dnf.寻找全路径(pvfpath, act_path, null, null, null, ani_path, null, null, null, null, null);
                        dnf.寻找全路径(pvfpath, ptl_path, null, null, null, ani_path, null, null, null, null, null);
                    }

                    if (ani_path.Count>0)
                    {
                        dnf.寻找als(ani_path,als_path,2,false);
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

                    //如果获取的输出全路径集合存在就输出
                    if (end_path.Count > 0)
                    {
                        string endpath;
                        string endmovepath;
                        string moveDirectory;

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

                    //收集lst信息
                    //城镇lst
                    if (twn_lst.Count>0)
                    {
                        endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【town/town.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + twn_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in twn_lst)
                        {
                            endtetxlst = endtetxlst + item.ToString() + "\r\n";
                        }
                        endtetxlst = endtetxlst + "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst = endtetxlst + "未找到城镇lst\r\n\r\n";
                    }

                    //npclst
                    if (npc_lst.Count > 0)
                    {
                        endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【npc/npc.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + npc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in npc_lst)
                        {
                            endtetxlst = endtetxlst + item.ToString() + "\r\n";
                        }
                        endtetxlst = endtetxlst + "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst = endtetxlst + "未找到npc lst\r\n\r\n";
                    }

                    //特效lst
                    if (obj_lst.Count > 0)
                    {
                        endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                        foreach (var item in obj_lst)
                        {
                            endtetxlst = endtetxlst + item.ToString() + "\r\n";
                        }
                        endtetxlst = endtetxlst + "\r\n\r\n";
                    }
                    else
                    {
                        endtetxlst = endtetxlst + "未找到特效lst\r\n\r\n";
                    }

                    //输出lst列表内容,并且\替换为/
                    if (endtetxlst != "")
                    {
                        File.WriteAllText(movepath + "\\(4)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));
                    }

                    twntime = time.ElapsedMilliseconds;

                    //根据用户选择查找img路径，并获取img提取用时
                    if (城镇提取完后查找img路径.Checked == true)
                    {
                        time.Restart();
                        string imgpath = "";
                        string npkimgpath = "";
                        string npkpath = "";
                        dnf.寻找img路径(movepath, "*.*", false, ref imgpath, ref npkimgpath, ref npkpath);

                        if (imgpath != "")
                        {
                            File.WriteAllText(movepath + "\\(1)未更改的img全路径.txt", imgpath);
                        }
                        if (npkimgpath != "")
                        {
                            File.WriteAllText(movepath + "\\(2)npk当中的img全路径.txt", npkimgpath);
                        }
                        if (npkpath != "")
                        {
                            File.WriteAllText(movepath + "\\(3)转换为npk全名.txt", npkpath);
                        }

                        //获取查找img路径的时间
                        imgtime = time.ElapsedMilliseconds;
                        time.Stop();
                    }
                    else
                    {
                        time.Stop();
                    }


                    //弹出信息框，结束提取城镇，判断img提取时间是否存在
                    if (imgtime == 0)
                    {

                        MessageBox.Show("已完成指定的城镇提取工作；\r\n提取城镇总用时：" + twntime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                    else
                    {

                        MessageBox.Show("已完成指定的城镇提取工作；\r\n提取城镇总用时：" + twntime.ToString() + "毫秒；\r\nimg路径查找用时：" + imgtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }


                    //提取完后根据用户选择是否定位导出后的目录
                    if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", movepath);
                    }

                }
            }
        }

        private void 城镇pvf文件txt_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                城镇pvf文件txt.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void 城镇导出目录txt_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                城镇导出目录txt.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }
        //——————————————城镇提取功能区————————————————

        //——————————————怪物提取功能区————————————————
        private void 怪物pvf文件目录text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                怪物pvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void 怪物导出目录text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                怪物导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void 怪物开始提取button_Click(object sender, EventArgs e)
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

            if (YesNoMob_tq == true)
            {
                regex_new regex = new regex_new();
                dnf dnf = new dnf();

                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                time.Start();

                long mobtime = 0;
                long imgtime = 0;

                string pvfpath = 怪物pvf文件目录text.Text.ToLower();
                string movepath = 怪物导出目录text.Text.ToLower();

                string mob_lst_alltext = "";
                string obj_lst_alltext = "";
                string apc_lst_alltext = "";

                if (File.Exists(pvfpath + @"\monster\monster.lst") == true)
                {
                    mob_lst_alltext = File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", "\\");
                }
                if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst") == true)
                {
                    obj_lst_alltext = File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", "\\");
                }
                if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst") == true)
                {
                    apc_lst_alltext = File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", "\\");
                }

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
                regex.创建(怪物编号text.Text,"[0-9]+",0);
                regex.匹配加入集合(mob_id,false,0);

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

                        dnf.怪物寻找编号(pvfpath, mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                        dnf.特效寻找编号(pvfpath, obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                        dnf.人偶寻找编号(pvfpath, apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                    } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                }

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

                if (mob_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (string item in mob_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到怪物lst\r\n\r\n\r\n";
                }

                if (obj_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (string item in obj_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到特效lst\r\n\r\n\r\n";
                }

                if (apc_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (var item in apc_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到人偶lst\r\n\r\n\r\n";
                }


                string endpath;
                string endmovepath;
                string moveDirectory;

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

                //输出lst列表内容,并且\替换为/
                File.WriteAllText(movepath + "\\(4)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                //结束怪物提取测量时间
                mobtime = time.ElapsedMilliseconds;

                //根据用户选择查找img路径，并获取img提取用时
                if (怪物查找img路径checkBox.Checked == true)
                {
                    time.Restart();
                    string imgpath = "";
                    string npkimgpath = "";
                    string npkpath = "";
                    dnf.寻找img路径(movepath, "*.*", false, ref imgpath, ref npkimgpath, ref npkpath);

                    if (imgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(1)未更改的img全路径.txt", imgpath);
                    }
                    if (npkimgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(2)npk当中的img全路径.txt", npkimgpath);
                    }
                    if (npkpath != "")
                    {
                        File.WriteAllText(movepath + "\\(3)转换为npk全名.txt", npkpath);
                    }

                    //获取查找img路径的时间
                    imgtime = time.ElapsedMilliseconds;
                    time.Stop();
                }
                else
                {
                    time.Stop();
                }

                //弹出信息框，结束提取副本，判断img提取时间是否存在
                if (imgtime == 0)
                {

                    MessageBox.Show("已完成指定的怪物提取工作；\r\n提取怪物总用时：" + mobtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {

                    MessageBox.Show("已完成指定的怪物提取工作；\r\n提取怪物总用时：" + mobtime.ToString() + "毫秒；\r\nimg路径查找用时：" + imgtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }


                //提取完后根据用户选择是否定位导出后的目录
                if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", movepath);
                }

            }
        }
        //——————————————怪物提取功能区————————————————


        //——————————————特效提取功能区————————————————
        private void 特效pvf文件目录text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                特效pvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void 特效导出目录text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                特效导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void 特效开始提取button_Click(object sender, EventArgs e)
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

            if (YesNoObj_tq == true)
            {
                regex_new regex = new regex_new();
                dnf dnf = new dnf();

                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                time.Start();

                long objtime = 0;
                long imgtime = 0;

                string pvfpath = 特效pvf文件目录text.Text.ToLower();
                string movepath = 特效导出目录text.Text.ToLower();

                string obj_lst_alltext = "";
                string mob_lst_alltext = "";
                string apc_lst_alltext = "";

                if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst") == true)
                {
                    obj_lst_alltext = File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", "\\");
                }
                if (File.Exists(pvfpath + @"\monster\monster.lst") == true)
                {
                    mob_lst_alltext = File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", "\\");
                }
                if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst") == true)
                {
                    apc_lst_alltext = File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", "\\");
                }

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

                //获取用户提供的特效编号
                regex.创建(特效编号text.Text, "[0-9]+", 0);
                regex.匹配加入集合(obj_id, false, 0);

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

                        dnf.怪物寻找编号(pvfpath, mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                        dnf.特效寻找编号(pvfpath, obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                        dnf.人偶寻找编号(pvfpath, apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                    } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                }

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

                if (obj_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (string item in obj_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到特效lst\r\n\r\n\r\n";
                }

                if (mob_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (string item in mob_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到怪物lst\r\n\r\n\r\n";
                }

                if (apc_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (var item in apc_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到人偶lst\r\n\r\n\r\n";
                }


                string endpath;
                string endmovepath;
                string moveDirectory;

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

                //输出lst列表内容,并且\替换为/
                File.WriteAllText(movepath + "\\(4)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                //结束特效提取测量时间
                objtime = time.ElapsedMilliseconds;

                //根据用户选择查找img路径，并获取img提取用时
                if (特效查找img路径checkBox.Checked == true)
                {
                    time.Restart();
                    string imgpath = "";
                    string npkimgpath = "";
                    string npkpath = "";
                    dnf.寻找img路径(movepath, "*.*", false, ref imgpath, ref npkimgpath, ref npkpath);

                    if (imgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(1)未更改的img全路径.txt", imgpath);
                    }
                    if (npkimgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(2)npk当中的img全路径.txt", npkimgpath);
                    }
                    if (npkpath != "")
                    {
                        File.WriteAllText(movepath + "\\(3)转换为npk全名.txt", npkpath);
                    }

                    //获取查找img路径的时间
                    imgtime = time.ElapsedMilliseconds;
                    time.Stop();
                }
                else
                {
                    time.Stop();
                }
                
                //弹出信息框，结束提取特效，判断img提取时间是否存在
                if (imgtime == 0)
                {

                    MessageBox.Show("已完成指定的特效提取工作；\r\n提取特效总用时：" + objtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {

                    MessageBox.Show("已完成指定的特效提取工作；\r\n提取特效总用时：" + objtime.ToString() + "毫秒；\r\nimg路径查找用时：" + imgtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }


                //提取完后根据用户选择是否定位导出后的目录
                if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", movepath);
                }
            }
        }
        //——————————————特效提取功能区————————————————


        //——————————————APC提取功能区————————————————
        private void APCpvf文件目录text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                APCpvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void APC导出目录text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                APC导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void APC开始提取button_Click(object sender, EventArgs e)
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

            if (YesNoApc_tq == true)
            {
                regex_new regex = new regex_new();
                dnf dnf = new dnf();

                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                time.Start();

                long apctime = 0;
                long imgtime = 0;

                string pvfpath = APCpvf文件目录text.Text.ToLower();
                string movepath = APC导出目录text.Text.ToLower();

                string apc_lst_alltext = "";
                string obj_lst_alltext = "";
                string mob_lst_alltext = "";


                if (File.Exists(pvfpath + @"\aicharacter\aicharacter.lst") == true)
                {
                    apc_lst_alltext = File.ReadAllText(pvfpath + @"\aicharacter\aicharacter.lst").Replace("/", "\\");
                }
                if (File.Exists(pvfpath + @"\passiveobject\passiveobject.lst") == true)
                {
                    obj_lst_alltext = File.ReadAllText(pvfpath + @"\passiveobject\passiveobject.lst").Replace("/", "\\");
                }
                if (File.Exists(pvfpath + @"\monster\monster.lst") == true)
                {
                    mob_lst_alltext = File.ReadAllText(pvfpath + @"\monster\monster.lst").Replace("/", "\\");
                }


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
                regex.创建(APC编号text.Text, "[0-9]+", 0);
                regex.匹配加入集合(apc_id, false, 0);

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

                        dnf.怪物寻找编号(pvfpath, mob_lst_alltext, mob_id, mob_lst, mob_path, obj_id, apc_id, act_path, ptl_path);
                        dnf.特效寻找编号(pvfpath, obj_lst_alltext, obj_id, obj_lst, obj_path, mob_id, apc_id, act_path, ptl_path);
                        dnf.人偶寻找编号(pvfpath, apc_lst_alltext, apc_id, apc_lst, apc_path, mob_id, obj_id, act_path, ptl_path, key_path);

                    } while (obj_id_count != obj_id.Count || mob_id_count != mob_id.Count || apc_id_count != apc_id.Count);
                }

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

                if (apc_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【aicharacter/aicharacter.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + apc_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (var item in apc_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到人偶lst\r\n\r\n\r\n";
                }

                if (obj_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【passiveobject/passiveobject.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + obj_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (string item in obj_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到特效lst\r\n\r\n\r\n";
                }

                if (mob_lst.Count > 0)
                {
                    endtetxlst = endtetxlst + "----------------------------------以下内容加入PVF【monster/monster.lst】文件内：----------------------------------\r\n---------------------------------------------------以下总数为【" + mob_lst.Count.ToString() + "】-------------------------------------------------------\r\n";
                    foreach (string item in mob_lst)
                    {
                        endtetxlst = endtetxlst + item + "\r\n";
                    }
                    endtetxlst = endtetxlst + "\r\n\r\n";
                }
                else
                {
                    endtetxlst = endtetxlst + "未找到怪物lst\r\n\r\n\r\n";
                }


                string endpath;
                string endmovepath;
                string moveDirectory;

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

                //输出lst列表内容,并且\替换为/
                File.WriteAllText(movepath + "\\(4)查找出来的lst列表.txt", endtetxlst.Replace("\\", "/"));

                //结束APC提取测量时间
                apctime = time.ElapsedMilliseconds;

                //根据用户选择查找img路径，并获取img提取用时
                if (APC查找img路径checkBox.Checked == true)
                {
                    time.Restart();
                    string imgpath = "";
                    string npkimgpath = "";
                    string npkpath = "";
                    dnf.寻找img路径(movepath, "*.*", false, ref imgpath, ref npkimgpath, ref npkpath);

                    if (imgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(1)未更改的img全路径.txt", imgpath);
                    }
                    if (npkimgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(2)npk当中的img全路径.txt", npkimgpath);
                    }
                    if (npkpath != "")
                    {
                        File.WriteAllText(movepath + "\\(3)转换为npk全名.txt", npkpath);
                    }

                    //获取查找img路径的时间
                    imgtime = time.ElapsedMilliseconds;
                    time.Stop();
                }
                else
                {
                    time.Stop();
                }

                //弹出信息框，结束提取APC，判断img提取时间是否存在
                if (imgtime == 0)
                {

                    MessageBox.Show("已完成指定的APC提取工作；\r\n提取APC总用时：" + apctime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {

                    MessageBox.Show("已完成指定的APC提取工作；\r\n提取APC总用时：" + apctime.ToString() + "毫秒；\r\nimg路径查找用时：" + imgtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }


                //提取完后根据用户选择是否定位导出后的目录
                if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", movepath);
                }
            }
        }
        //——————————————APC提取功能区————————————————


        //——————————————ANI提取功能区————————————————
        private void ANI全路径文件text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = false;//设置不能选择文件，可以选择文件
            file_folder.Title = @"请选择ANI全路径文件；例如：E:\DNF\ANI全路径.txt";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                ANI全路径文件text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }
        private void ANIpvf文件目录text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择PVF导出的文件所在目录；例如：E:\DNF\导出的PVF文件目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                ANIpvf文件目录text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void ANI导出目录text_MouseDown(object sender, MouseEventArgs e)
        {
            file_folder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//设置初始目录为桌面
            file_folder.IsFolderPicker = true;//设置不能选择文件，可以选择文件夹
            file_folder.Title = @"请选择最后寻找文件需要导出的目录；例如：E:\DNF\提取出的目录";//设置标题
            if (file_folder.ShowDialog() == CommonFileDialogResult.Ok)//如果点击对话框确定
            {
                ANI导出目录text.Text = file_folder.FileName;//获取的路径加入文本框
            }
        }

        private void ANI开始提取button_Click(object sender, EventArgs e)
        {
            bool YesNoAni_tq = false;

            //开始初始判断是否提取特效
            if (ANI全路径文件text.Text != "")
            {

                if (File.Exists(ANI全路径文件text.Text) ==true)
                {

                    if (ANIpvf文件目录text.Text != "")
                    {

                        if (Directory.Exists(ANIpvf文件目录text.Text) == true)
                        {

                            if (ANI导出目录text.Text != "")
                            {

                                if (Directory.Exists(ANI导出目录text.Text) == true)
                                {
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

            if (YesNoAni_tq == true)
            {
                dnf dnf = new dnf();

                System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
                time.Start();

                long anitime = 0;
                long imgtime = 0;

                string pvfpath = ANIpvf文件目录text.Text.ToLower();
                string movepath = ANI导出目录text.Text.ToLower();

                List<string> ani_path = new List<string>();
                List<string> als_path = new List<string>();

                List<string> end_path = new List<string>();

                
                if (pvf中的全路径radioButton.Checked == true)
                {
                    foreach (string item in File.ReadAllLines(ANI全路径文件text.Text))
                    {
                        if (ani_path.Contains(pvfpath + "\\" + item) == false)
                        {
                            ani_path.Add(pvfpath + "\\" + item);
                        }
                    }
                }
                else if (Windows中的全路径radioButton.Checked == true)
                {
                    ani_path = new List<string>(File.ReadLines(ANI全路径文件text.Text));
                }

                dnf.寻找als(ani_path, als_path,2,false);


                end_path.AddRange(ani_path);
                end_path.AddRange(als_path);

                string endpath;
                string endmovepath;
                string moveDirectory;

                //开始输出所有文件
                foreach (string item in end_path)
                {
                    //取出原文件所在全路径，并且转为小写
                    endpath = item.ToLower();

                    //要复制的文件是否存在
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

                //结束APC提取测量时间
                anitime = time.ElapsedMilliseconds;

                //根据用户选择查找img路径，并获取img提取用时
                if (ANI查找img路径checkBox.Checked == true)
                {
                    time.Restart();
                    string imgpath = "";
                    string npkimgpath = "";
                    string npkpath = "";
                    dnf.寻找img路径(movepath, "*.*", false, ref imgpath, ref npkimgpath, ref npkpath);

                    if (imgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(1)未更改的img全路径.txt", imgpath);
                    }
                    if (npkimgpath != "")
                    {
                        File.WriteAllText(movepath + "\\(2)npk当中的img全路径.txt", npkimgpath);
                    }
                    if (npkpath != "")
                    {
                        File.WriteAllText(movepath + "\\(3)转换为npk全名.txt", npkpath);
                    }

                    //获取查找img路径的时间
                    imgtime = time.ElapsedMilliseconds;
                    time.Stop();
                }
                else
                {
                    time.Stop();
                }

                //弹出信息框，结束提取ani;als，判断img提取时间是否存在
                if (imgtime == 0)
                {

                    MessageBox.Show("已完成指定的ani;als提取工作；\r\n提取ani;als总用时：" + anitime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {

                    MessageBox.Show("已完成指定的ani;als提取工作；\r\n提取ani;als总用时：" + anitime.ToString() + "毫秒；\r\nimg路径查找用时：" + imgtime.ToString() + "毫秒；", "完成", MessageBoxButtons.OK, MessageBoxIcon.None);
                }


                //提取完后根据用户选择是否定位导出后的目录
                if (MessageBox.Show("是否打开提取并导出的目录", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", movepath);
                }

            }
        }
        //——————————————ANI提取功能区————————————————


        //——————————————装备提取功能区————————————————
        //——————————————装备提取功能区————————————————


        //——————————————礼包提取功能区————————————————
        //——————————————礼包提取功能区————————————————



        //测试按钮
        private void button2_Click(object sender, EventArgs e)
        {
            dnf dnf = new dnf();
            string path = @"C:\Users\MSI-NB\Desktop\2020\新建文件夹 (3)";
            string img = "";
            string npkimg = "";
            string npk = "";
            dnf.寻找img路径(path,"*.*",false,ref img,ref npkimg,ref npk);
            File.WriteAllText(@"C:\Users\MSI-NB\Desktop\2020\1.txt", img);
            File.WriteAllText(@"C:\Users\MSI-NB\Desktop\2020\2.txt", npkimg);
            File.WriteAllText(@"C:\Users\MSI-NB\Desktop\2020\3.txt", npk);


        }

        
    }
}
