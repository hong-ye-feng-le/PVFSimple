using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ADOX;//access数据库创建
using System.Data.OleDb;//更改access数据库
using System.Data;


namespace PVFSimple.数据库access
{
    //ADOX.Column[] userSetColumns =
    //        {
    //            //这块大家注意数据类型的问题
    //            new ADOX.Column(){Name="img全路径",Type=DataTypeEnum.adVarWChar,DefinedSize=255},
    //            new ADOX.Column(){Name="img名称",Type=DataTypeEnum.adVarWChar ,DefinedSize=255},
    //            new ADOX.Column(){Name="npk文件名",Type=DataTypeEnum.adVarWChar ,DefinedSize=255},
    //        };


    public class Access
    {


        /// <summary>
        /// 在access数据库中创建表
        /// </summary>
        /// <param name="filePath">数据库表文件全路径</param>
        /// <param name="tableName">表名</param>
        /// <param name="colums">ADOX.Column对象数组</param>
        public void CreateAccessTable(string filePath, string tableName, params ADOX.Column[] colums)
        {
            ADOX.Catalog catalog = new Catalog();
            //数据库文件不存在则创建
            if (!File.Exists(filePath))
            {
                //创建数据库
                //catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Jet OLEDB:Engine Type=5");
                catalog.Create("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Jet OLEDB:Engine Type=5");
            }
            //创建连接
            ADODB.Connection cn = new ADODB.Connection();
            //打开连接
            //cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath, null, null, -1);
            cn.Open("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath, null, null, -1);
            catalog.ActiveConnection = cn;
            //创建一个表格
            ADOX.Table table =new ADOX.Table 
            {
                //获取表名
                Name = tableName
            };
            //遍历一个字段的集合，从而添加字段
            foreach (var column in colums)
            {
                //如果不是bool类型的，可以为空
                if (column.Type != DataTypeEnum.adBoolean)
                {
                    //允许空值
                    column.Attributes = ColumnAttributesEnum.adColNullable;
                }
                //保存字段
                table.Columns.Append(column);
            }
            //定义主键 
            //主要解释一下第三个参数：你设置为主键的名称
            //这里默认为：id
            table.Keys.Append("FirstTablePrimaryKey", KeyTypeEnum.adKeyPrimary, colums[0], tableName, null);
            //向数据中添加表
            catalog.Tables.Append(table);
            //关闭连接
            cn.Close();

            catalog = null; GC.Collect();
            //设置自动增长
            //column.Properties["AutoIncrement"].Value = true;           
        }


        /// <summary>
        /// 通过access事务一次性提交多条img达到速度提升
        /// </summary>
        /// <param name="Mdbpath">mdb文件全路径</param>
        /// <param name="AllItem">命令集合</param>
        public void AddImgTable(string Mdbpath,List<string> AllItem)
        {
            //连接数据库
            //OleDbConnection olecon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Mdbpath );
            OleDbConnection olecon = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Mdbpath );
            olecon.Open();

            //事务
            OleDbCommand cmd = new OleDbCommand
            {
                Connection = olecon,
                Transaction = olecon.BeginTransaction()
            };

            try
            {
                
                //所有命令提交给事务
                foreach (string item in AllItem)
                {
                    cmd.CommandText = item;
                    cmd.ExecuteNonQuery();
                }

                cmd.Transaction.Commit();  //提交事务

            }
            catch (Exception)
            {
                cmd.Transaction.Rollback();
            }
            finally
            {
                //释放
                cmd.Dispose();
                olecon.Close();
            }
        }

        
        /// <summary>
        /// 多行或单行查询
        /// </summary>
        /// <param name="MdbPath">mdb文件全路径</param>
        /// <param name="ChaZhaoZiDuan">查找字段</param>
        /// <param name="ChaZzhao">查找内容</param>
        /// <param name="YesNoMoHu">是否模糊查询</param>
        /// <returns></returns>
        public DataRowCollection ChaXunText(string MdbPath, string ChaZhaoZiDuan, string ChaZzhao, bool YesNoMoHu)
        {
            //创建数据表
            DataSet dataSet = new DataSet();
            //连接mdb数据库信息
            //OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + MdbPath);
            OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + MdbPath);

            //设置可变字符串
            string EndChaZhao;

            //执行命令查询
            OleDbDataAdapter oleDapAdapter = null ;

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
                        oleDapAdapter = new OleDbDataAdapter(EndChaZhao, oleDbConnection);
                        //添加数据集
                        oleDapAdapter.Fill(dataSet);
                    }
                    else
                    {
                        foreach (var item in ChaZzhao.Split('|'))
                        {
                            EndChaZhao = "select * from ImagePacks2 where " + ChaZhaoZiDuan + " Like '%" + item + "%'";
                            //执行命令查询
                            oleDapAdapter = new OleDbDataAdapter(EndChaZhao, oleDbConnection);
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
                        oleDapAdapter = new OleDbDataAdapter(EndChaZhao, oleDbConnection);
                        //添加数据集
                        oleDapAdapter.Fill(dataSet);
                    }
                    else
                    {
                        foreach (var item in ChaZzhao.Split('|'))
                        {
                            EndChaZhao = "select * from ImagePacks2 where " + ChaZhaoZiDuan + " ='" + item + "'";
                            //执行命令查询
                            oleDapAdapter = new OleDbDataAdapter(EndChaZhao, oleDbConnection);
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



        public void SetData()
        {
            string MdbFilePath = @"C:\Users\MSI-NB\Desktop\新建文件夹 (2)\剑圣60-img数据库2020年02月11日11点51分50秒.mdb";

            OleDbConnection conn = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={MdbFilePath}");
            
            OleDbCommand oleDbCommand = new OleDbCommand("select * from ImagePacks2", conn);


            OleDbDataAdapter oleDbData = new OleDbDataAdapter(oleDbCommand);

            OleDbCommandBuilder oleDbCommandBuilder = new OleDbCommandBuilder(oleDbData);
            DataTable dataTable = new DataTable();
            oleDbData.Fill(dataTable);


            //DataTable dataTable2 = new DataTable("ImagePacks2");
            //dataTable2.TableName = "ImagePacks2";

            object[] vs = new object [4];
            vs[0] = 68001;
            vs[1] = "999";
            vs[2] = "999";
            vs[3] = "999";

            dataTable.Rows.Add(vs);

            oleDbData.Update(dataTable);
            oleDbCommandBuilder.Dispose();
        }
        //server=.;uid=sa;pwd=sa;database=TAOBAODB


    }
}
