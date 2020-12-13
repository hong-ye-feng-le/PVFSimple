using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Data;
using PVFSimple.项目Dnf;

namespace PVFSimple.Sqlite数据库
{
    public class Sqlite
    {

         static readonly string TableName = "ImagePacks2";
         static readonly string TableZiDuan = "id Text,img全路径 Text,img名称 Text,npk文件名 Text";

        /// <summary>
        /// 创建DNFNPK数据库表
        /// </summary>
        /// <param name="SqlFilePath"></param>
        public void CreateSqliteTable(string SqlFilePath)
        {
            SQLiteConnection sqlcn = new SQLiteConnection("data source=" + SqlFilePath);
            try
            {
                sqlcn.Open();
                using SQLiteCommand sqlcmd = new SQLiteCommand
                {
                    Connection = sqlcn,
                    CommandText = $"CREATE TABLE {TableName}({TableZiDuan})"
                };
                //sqlcmd.CommandText = "CREATE TABLE IF NOT EXISTS t1(id varchar(4),score int)";
                sqlcmd.ExecuteNonQuery();
                sqlcn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "询问", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        /// <summary>
        /// 新建img数据库后，直接可边查询边插入数据库信息
        /// </summary>
        /// <param name="DnfNpkPath">img目录</param>
        /// <param name="MdbPath">mdb全路径</param>
        public void AddSqlite(List<string> NpkFilePaths, string SqlFilePath)
        {
            NpkFile npk = new NpkFile();
            List<string> SqlText = new List<string>();

            uint MdbId = 0;
            foreach (string item in NpkFilePaths)
            {
                try
                {
                    if (npk.YesNpNpk(item) == false)
                        continue;

                    //开始读取
                    using FileStream fileStream = new FileStream(item, FileMode.Open, FileAccess.Read);
                    using BinaryReader binaryReader = new BinaryReader(fileStream);

                    fileStream.Position = 16;//跳过npk头
                    int npksm = binaryReader.ReadInt32();//获取img数量

                    //获取npk文件名
                    string NpkName = item.Substring(item.LastIndexOf("\\") + 1);

                    if (NpkName.Contains("'"))//如果找到数据库修饰符，就替换
                        NpkName = NpkName.Replace("'", "''");

                    for (int i = 0; i < npksm; i++)
                    {
                        fileStream.Position += 4;//跳过偏移
                        fileStream.Position += 4;//跳过大小

                        string ImgPath = npk.ImgName解密(binaryReader.ReadBytes(256)).Replace("\\", "/");//获取解密后的img名称;并且替换右斜杠
                        if (ImgPath.Contains("'"))//如果找到数据库修饰符，就替换
                            ImgPath = ImgPath.Replace("'", "''");

                        //获取img文件名
                        string ImgName = ImgPath.Substring(ImgPath.LastIndexOf("/") + 1);

                        //增加sql语句
                        SqlText.Add($"insert into {TableName}(id,img全路径,img名称,npk文件名) values('{MdbId}','{ImgPath}','{ImgName}','{NpkName}')");

                        MdbId++;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"在对已创建的sqlite数据库增加img全路径等数据时发生错误,错误文件路径：{item}，错误img坐标为：{MdbId}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }

            if (TransacStart(SqlText, SqlFilePath) == false)
                MessageBox.Show("在最后执行数据库事务插入时发生错误", "失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }



        /// <summary>
        /// 按照提供的sqlite数据库文件路径，如果文件不存在则创建一个
        /// </summary>
        /// <param name="SqlPath">要创建的数据库文件路径</param>
        public void CreateSqlFile(string SqlFilePath)
        {
            SQLiteConnection cn = new SQLiteConnection("data source=" + SqlFilePath);
            cn.Open();
            cn.Close();
        }

        /// <summary>
        /// 删除指定的sqlite文件
        /// </summary>
        /// <param name="SqlPath">要删除的数据库文件路径</param>
        public void DeleteSqlFile(string SqlFilePath)
        {
            if (File.Exists(SqlFilePath))
                File.Delete(SqlFilePath);
        }


        /// <summary>
        /// 多行或单行查询
        /// </summary>
        /// <param name="SqlFilePath">sqlite文件全路径</param>
        /// <param name="ChaZhaoZiDuan">查找字段</param>
        /// <param name="ChaZzhao">查找内容</param>
        /// <param name="YesNoMoHu">是否模糊查询</param>
        /// <returns></returns>
        public DataRowCollection ChaXunText(string SqlFilePath, string ChaZhaoZiDuan, string ChaZzhao, bool YesNoMoHu)
        {
            //创建数据表
            DataSet dataSet = new DataSet();

            //连接mdb数据库信息
            using SQLiteConnection oleDbConnection = new SQLiteConnection("data source=" + SqlFilePath);

            //设置可变字符串
            string EndChaZhao;

            //执行命令查询
            SQLiteDataAdapter oleDapAdapter = null;

            try
            {
                //打开连接
                oleDbConnection.Open();
                //所有内容加入可变字符串中
                if (YesNoMoHu)
                {
                    if (ChaZzhao.Contains("|") == false)
                    {
                        EndChaZhao = "select * from ImagePacks2 where " + ChaZhaoZiDuan + " Like '%" + ChaZzhao + "%'";
                        //执行命令查询
                        oleDapAdapter = new SQLiteDataAdapter(EndChaZhao, oleDbConnection);
                        //添加数据集
                        oleDapAdapter.Fill(dataSet);
                    }
                    else
                    {
                        foreach (var item in ChaZzhao.Split('|'))
                        {
                            EndChaZhao = "select * from ImagePacks2 where " + ChaZhaoZiDuan + " Like '%" + item + "%'";
                            //执行命令查询
                            oleDapAdapter = new SQLiteDataAdapter(EndChaZhao, oleDbConnection);
                            //添加数据集
                            oleDapAdapter.Fill(dataSet);
                        }

                    }

                }
                else
                {
                    if (ChaZzhao.Contains("|") == false)
                    {
                        EndChaZhao = "select * from ImagePacks2 where " + ChaZhaoZiDuan + " ='" + ChaZzhao + "'";
                        //执行命令查询
                        oleDapAdapter = new SQLiteDataAdapter(EndChaZhao, oleDbConnection);
                        //添加数据集
                        oleDapAdapter.Fill(dataSet);
                    }
                    else
                    {
                        foreach (var item in ChaZzhao.Split('|'))
                        {
                            EndChaZhao = "select * from ImagePacks2 where " + ChaZhaoZiDuan + " ='" + item + "'";
                            //执行命令查询
                            oleDapAdapter = new SQLiteDataAdapter(EndChaZhao, oleDbConnection);
                            //添加数据集
                            oleDapAdapter.Fill(dataSet);
                        }

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }


            //释放
            oleDapAdapter.Dispose();
            oleDbConnection.Close();

            using (dataSet)
            {
                //返回查找出来的内容
                return dataSet.Tables[0].Rows;
            }


        }


        /// <summary>
        /// 批量执行事务sql语句
        /// </summary>
        /// <param name="SqlText">被执行的sql语句集合</param>
        /// <param name="SqlFilePath">sql文件路径</param>
        /// <returns></returns>
        public bool TransacStart(List<string> SqlText, string SqlFilePath)
        {
            bool YesNo = false;
            int Count = 0;

            //sql数据库连接
            using SQLiteConnection sqlconn = new SQLiteConnection("data source=" + SqlFilePath);
            sqlconn.Open();//连接数据库

            using SQLiteCommand sqlcmd = new SQLiteCommand//命令对象
            {
                Connection = sqlconn,//连接对象
                Transaction = sqlconn.BeginTransaction()//命令开始事务
            };

            SqlText.ForEach((Sql) => {
                sqlcmd.CommandText = Sql;//设置命令文本
                if (sqlcmd.ExecuteNonQuery() > 0)//欲执行
                    Count++;
            });

            if (Count == SqlText.Count)
            {
                sqlcmd.Transaction.Commit(); //提交事务
                YesNo = true;
            }
            else
            {
                sqlcmd.Transaction.Rollback();  //事务回滚
            }

            return YesNo;
        }

    }
}
