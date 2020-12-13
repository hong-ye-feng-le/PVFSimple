using System;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using PVFSimple.群验证;

namespace PVFSimple.数据库验证
{
    public class MysqlYz
    {


        public string qq;
        public string md5;

        public static byte[] i = Encoding.Default.GetBytes("1晏$");
        public static byte[] n = new byte[] { 115, 70, 106, 94, 49, 36, 68, 107, 106, 53, 62, 102, 103, 105, 56, 49, 64, 106, 75, 33, 49, 49, 82, 73, 111, 100, 60, 35};


        public static string Conn = "server="+ i[0]+"."+i[1]+"."+i[2]+"."+i[3]+";User Id=pvfgame;password="+ Encoding .Default.GetString(n)+ ";database=pvf;";



        public void Yse()
        {
            new 表格控件.Grid_SheJiTu().初始化设计图表格框架(Form1.reoGridContro_Newl);
            new 表格控件.Grid_RenWu().初始化任务表格框架(Form1.reoGridContro_New2, Form1.reoGridContro_New3);
            Form1.form1.任务类型comboBox.SelectedIndex=0;
            Form1.form1.完成条件类型comboBox.SelectedIndex=0;
            Form1.form1.完成条件子类型comboBox.SelectedIndex=0;
            Form1.form1.完成条件子类型comboBox.Enabled=false;
            Form1.form1.完成奖励类型comboBox.SelectedIndex=0;

            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + "SQLite.Interop.dll") == false)
                File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\" + "SQLite.Interop.dll", Properties.Resources.SQLite_Interop);

            Rectangle rec = Screen.GetWorkingArea(Form1.form1);

            int SW = (rec.Width - 985) / 2;
            int SH = (rec.Height - 725) / 2;

            Form1.form1.Width = 985;
            Form1.form1.Height = 725;
            Form1.form1.Opacity = (double)100;
            Form1.form1.Location = new Point(SW, SH);

        }


        /// <summary>
        /// 查询返回第一行第一列
        /// </summary>
        /// <returns></returns>
        public int Data()
        {
            int refanhui=0;
            MySqlCommand Cmd;
            //MySqlDataAdapter MsData;

            
            try
            {
                using MySqlConnection mySql = new MySqlConnection(Conn);
                mySql.Open();
                Cmd = new MySqlCommand("select * from pvf where md5='" + md5 + "'", mySql);
                object fanhui = Cmd.ExecuteScalar();
                if (fanhui != null)
                {
                    refanhui = (int)fanhui;
                }
                Cmd.Dispose();
                mySql.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return refanhui;
        }


        public bool ChaR()
        {
            bool y = true;
            //bool y = false;
            //try
            //{
            //    string time = DateTime.Now.ToString("yyyy-MM-dd-") + DateTime.Now.ToString("hh-mm-ss");
            //    string ip = new Verification().GetIP();
            //    using MySqlConnection mySql = new MySqlConnection(Conn);
            //    mySql.Open();
            //    using MySqlCommand And = new MySqlCommand("insert into info(qq,md5,time,ip) values('" + qq + "','" /+/ md5 + "','" + time + "','" + ip + "')", mySql);
            //    if (And.ExecuteNonQuery() > 0)
            //    {
            //        mySql.Close();
            //        y = true;
            //    }
            //    else
            //    {
            //        MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return y;
            //    }
            //
            //
            //}
            //catch (Exception)
            //{
            //    
            //    MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return y;
            //
            //}

            return y;
        }


        public void ChaRError()
        {
            //try
            //{
            //    string time = DateTime.Now.ToString("yyyy-MM-dd-") + DateTime.Now.ToString("hh-mm-ss");
            //    string ip = new Verification().GetIP();
            //    using MySqlConnection mySql = new MySqlConnection(Conn);
            //    mySql.Open();
            //    MySqlCommand And = new MySqlCommand("insert into error(qq,md5,time,ip) values('" + qq + "','" + /md5 /+ "','" + time + "','" + ip + "')", mySql);
            //    if (And.ExecuteNonQuery() > 0)
            //    {
            //        mySql.Close();
            //    }
            //    else
            //    {
            //        MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //    And.Dispose();
            //
            //
            //}
            //catch (Exception)
            //{
            //
            //    MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //
            //}
        }


        public bool ChaXun()
        {
            //try
            //{
            //    string SqlCx = "select * from pvf where qq='" + qq + "' and md5='" + md5 + "'";
            //
            //    using (MySqlConnection mySql = new MySqlConnection(Conn))
            //    {
            //        mySql.Open();//连接服务器
            //        using (MySqlCommand mySqlCommand = new MySqlCommand(SqlCx, mySql))
            //        {
            //            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            //            if (mySqlDataReader.Read() == false)
            //            {
            //                ChaRError();
            //                MessageBox.Show("抱歉您未授权此工具", "提示", MessageBoxButtons.OK, //MessageBoxIcon.Error);
            //                Form1.form1.Close();
            //                mySql.Close();
            //                return false;
            //            }
            //        }
            //        mySql.Close();
            //    } 
            //    
            //    return true;
            //}
            //catch (Exception)
            //{
            //
            //    MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
            return true;
        }


        public string ChaXun(uint MoShi)
        {
            try
            {
                string SqlCx = "select * from pvf where qq='" + qq + "' and md5='" + md5 + "'";
                string GatText = "";

                using (MySqlConnection mySql = new MySqlConnection(Conn))
                {
                    mySql.Open();//连接服务器


                    using MySqlCommand mySqlCommand = new MySqlCommand(SqlCx, mySql);
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    if (mySqlDataReader.Read())
                    {

                        switch (MoShi)
                        {
                            case 0:
                                GatText = mySqlDataReader["qq"].ToString();
                                break;
                            case 1:
                                GatText = mySqlDataReader["md5"].ToString();
                                break;
                            case 2:
                                GatText = mySqlDataReader["time"].ToString();
                                break;
                            case 3:
                                GatText = mySqlDataReader["dnf_bd"].ToString();
                                break;
                            case 4:
                                GatText = mySqlDataReader["httpfile"].ToString();
                                break;
                            case 5:
                                GatText = mySqlDataReader["httplist"].ToString();
                                break;
                            case 6:
                                GatText = mySqlDataReader["dgn_tq"].ToString();
                                break;
                            case 7:
                                GatText = mySqlDataReader["twn_tq"].ToString();
                                break;
                            case 8:
                                GatText = mySqlDataReader["mob_tq"].ToString();
                                break;
                            case 9:
                                GatText = mySqlDataReader["obj_tq"].ToString();
                                break;
                            case 10:
                                GatText = mySqlDataReader["apc_tq"].ToString();
                                break;
                            case 11:
                                GatText = mySqlDataReader["anials_tq"].ToString();
                                break;
                            case 12:
                                GatText = mySqlDataReader["equ_tq"].ToString();
                                break;
                            case 13:
                                GatText = mySqlDataReader["stk_lb_id_tq"].ToString();
                                break;
                            case 14:
                                GatText = mySqlDataReader["stk_bz_tq"].ToString();
                                break;
                            case 15:
                                GatText = mySqlDataReader["lst_lb_tq"].ToString();
                                break;
                            case 16:
                                GatText = mySqlDataReader["npk_cx"].ToString();
                                break;
                            case 17:
                                GatText = mySqlDataReader["npk_img_tq"].ToString();
                                break;
                            case 18:
                                GatText = mySqlDataReader["npk_img_sc"].ToString();
                                break;
                            case 19:
                                GatText = mySqlDataReader["npk_img_qcf"].ToString();
                                break;
                            case 20:
                                GatText = mySqlDataReader["npk_img_add"].ToString();
                                break;
                            case 21:
                                GatText = mySqlDataReader["npk_img_lst_tq"].ToString();
                                break;
                            case 22:
                                GatText = mySqlDataReader["npk_img_hb"].ToString();
                                break;
                            case 23:
                                GatText = mySqlDataReader["sjt_sc"].ToString();
                                break;
                            case 24:
                                GatText = mySqlDataReader["stk_lb_sc"].ToString();
                                break;
                            case 25:
                                GatText = mySqlDataReader["lst_add"].ToString();
                                break;
                            case 26:
                                GatText = mySqlDataReader["rw_sc"].ToString();
                                break;
                            case 27:
                                GatText = mySqlDataReader["add_etc_info"].ToString();
                                break;
                            case 28:
                                GatText = mySqlDataReader["xf_luoti"].ToString();
                                break;
                        }
                        mySql.Close();
                    }
                    else
                    {
                        MessageBox.Show("抱歉您未授权此工具", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Form1.form1.Close();
                        mySql.Close();
                        return "";
                    }
                } 
                
                
                return GatText;
            }
            catch (Exception)
            {

                MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }

        }

        public void XY(uint MoShi)
        {
            //try
            //{
            //    string SqlCx = "select * from pvf where qq='" + qq + "' and md5='" + md5 + "'";
            //
            //    using MySqlConnection mySql = new MySqlConnection(Conn);
            //    mySql.Open();//连接服务器
            //    string GatText = "";
            //
            //    using (MySqlCommand mySqlCommand = new MySqlCommand(SqlCx, mySql))
            //    {
            //        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            //        if (mySqlDataReader.Read())
            //        {
            //
            //            switch (MoShi)
            //            {
            //                case 6:
            //                    GatText = mySqlDataReader["dgn_tq"].ToString();
            //                    break;
            //                case 7:
            //                    GatText = mySqlDataReader["twn_tq"].ToString();
            //                    break;
            //                case 8:
            //                    GatText = mySqlDataReader["mob_tq"].ToString();
            //                    break;
            //                case 9:
            //                    GatText = mySqlDataReader["obj_tq"].ToString();
            //                    break;
            //                case 10:
            //                    GatText = mySqlDataReader["apc_tq"].ToString();
            //                    break;
            //                case 11:
            //                    GatText = mySqlDataReader["anials_tq"].ToString();
            //                    break;
            //                case 12:
            //                    GatText = mySqlDataReader["equ_tq"].ToString();
            //                    break;
            //                case 13:
            //                    GatText = mySqlDataReader["stk_lb_id_tq"].ToString();
            //                    break;
            //                case 14:
            //                    GatText = mySqlDataReader["stk_bz_tq"].ToString();
            //                    break;
            //                case 15:
            //                    GatText = mySqlDataReader["lst_lb_tq"].ToString();
            //                    break;
            //                case 16:
            //                    GatText = mySqlDataReader["npk_cx"].ToString();
            //                    break;
            //                case 17:
            //                    GatText = mySqlDataReader["npk_img_tq"].ToString();
            //                    break;
            //                case 18:
            //                    GatText = mySqlDataReader["npk_img_sc"].ToString();
            //                    break;
            //                case 19:
            //                    GatText = mySqlDataReader["npk_img_qcf"].ToString();
            //                    break;
            //                case 20:
            //                    GatText = mySqlDataReader["npk_img_add"].ToString();
            //                    break;
            //                case 21:
            //                    GatText = mySqlDataReader["npk_img_lst_tq"].ToString();
            //                    break;
            //                case 22:
            //                    GatText = mySqlDataReader["npk_img_hb"].ToString();
            //                    break;
            //                case 23:
            //                    GatText = mySqlDataReader["sjt_sc"].ToString();
            //                    break;
            //                case 24:
            //                    GatText = mySqlDataReader["stk_lb_sc"].ToString();
            //                    break;
            //                case 25:
            //                    GatText = mySqlDataReader["lst_add"].ToString();
            //                    break;
            //                case 26:
            //                    GatText = mySqlDataReader["rw_sc"].ToString();
            //                    break;
            //                case 27:
            //                    GatText = mySqlDataReader["add_etc_info"].ToString();
            //                    break;
            //                case 28:
            //                    GatText = mySqlDataReader["xf_luoti"].ToString();
            //                    break;
            //            }
            //            if (GatText != "1")
            //            {
            //                MessageBox.Show("抱歉您未购买此功能，请联系作者QQ506807329后再使用", "提示", //MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                ChaRError();
            //                mySql.Close();
            //                Form1.form1.Close();
            //
            //            }
            //
            //        }
            //        else
            //        {
            //            MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            mySql.Close();
            //            Form1.form1.Close();
            //        }
            //    }
            //    mySql.Close();
            //
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Form1.form1.Close();
            //}
        }



        //使用模式
        public int ChaXunMoShi()
        {
            try
            {
                string SqlCx = "select * from moshi where id='2'";

                using (MySqlConnection mySql = new MySqlConnection(Conn))
                {
                    mySql.Open();//连接服务器
                    using (MySqlCommand mySqlCommand = new MySqlCommand(SqlCx, mySql))
                    {
                        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                        if (mySqlDataReader.Read() == false)
                        {
                            return 1;
                        }
                    }
                    mySql.Close();
                }

                return 2;
            }
            catch (Exception)
            {

                MessageBox.Show("网络连接中断", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

        }



    }
}
