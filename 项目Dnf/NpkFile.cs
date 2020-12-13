using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data.OleDb;//更改access数据库
using System.Text.RegularExpressions;//正则表达式
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace PVFSimple.项目Dnf
{
    public class ImgFile
    {
        public string Name;

        public byte[] Data;

        public ImgFile(string name)
        {
            Name = name;
        }

        public ImgFile(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }


        public override string ToString()
        {
            return Name;
        }

    }


    public class NpkImgFile
    {
        public List<string> Name = new List<string>();

        public List<byte[]> Data = new List<byte[]>();

        public NpkImgFile()
        {

        }

        public NpkImgFile(string ImgName)
        {
            Name.Add(ImgName);
        }

        public NpkImgFile(byte[] ImgData)
        {
            Data.Add(ImgData);
        }

        public NpkImgFile(string ImgName,byte[] ImgData)
        {
            Name.Add(ImgName);
            Data.Add(ImgData);
        }

    }


    public class NpkFile
    {


        public static string npk_key = "puchikon@neople dungeon and fighter DNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNFDNF\0";

        public byte[] _key = Encoding.Default.GetBytes(npk_key);

        public byte[] _head = Encoding.Default.GetBytes("NeoplePack_Bill\0");


        /// <summary>
        /// 判断是否是NPK文件
        /// </summary>
        /// <param name="NpkFilePath">NPK全路径</param>
        /// <returns></returns>
        public bool YesNpNpk(string NpkFilePath)
        {

            try
            {
                var NpkFs = File.OpenRead(NpkFilePath);
                BinaryReader NpkBr = new BinaryReader(NpkFs);
                string NpkT = Encoding.Default.GetString(NpkBr.ReadBytes(16)).TrimEnd('\0');
                NpkBr.Dispose();
                if (NpkT == "NeoplePack_Bill")
                {
                    return true;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("发生错误；方法名：YesNpNpk；错误路径：" + NpkFilePath, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return false;
        }




        /// <summary>
        /// 打开一个NPK获取里面的所有img路径
        /// </summary>
        /// <param name="NpkFilePath">NPK路径</param>
        /// <returns>img路径集合</returns>
        public List<string> NpkAllImgPath(string NpkFilePath)
        {
            List<string> ImgPath = null;
            if (YesNpNpk(NpkFilePath))
            {
                try
                {
                    ImgPath = new List<string>();
                    var NpkFs = File.OpenRead(NpkFilePath);
                    BinaryReader NpkBr = new BinaryReader(NpkFs);

                    NpkFs.Position = 16;//跳过npk头

                    var ImgCount = NpkBr.ReadUInt32();

                    for (int i = 0; i < ImgCount; i++)
                    {
                        NpkFs.Position += 4;//跳过偏移
                        NpkFs.Position += 4;//跳过大小
                        ImgPath.Add(ImgName解密(NpkBr.ReadBytes(256)));
                    }
                    NpkBr.Dispose();
                }
                catch (Exception)
                {
                    MessageBox.Show("发生错误；方法名：NpkAllImgPath；错误路径：" + NpkFilePath, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                
            }

            return ImgPath;
        }



        /// <summary>
        /// 查找一个NPK内是否有重复img
        /// </summary>
        /// <param name="NpkFilePath">NPK路径</param>
        /// <returns>false代表无 true代表有重复</returns>
        public bool NpkImg是否重复(string NpkFilePath)
        {
            List<string> LinShiPath = NpkAllImgPath(NpkFilePath);

            if (LinShiPath!=null)
            {
                foreach (var item in LinShiPath.GroupBy(s => s))
                {
                    if (item.Count() > 1)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }



        /// <summary>
        /// 打开单个npk返回img名称键、img数据值
        /// </summary>
        /// <param name="NpkPath">npk全路径</param>
        /// <returns>img名称和img数据</returns>
        public KeyValuePair<List<string>, List<byte[]>> 打开(string NpkPath)
        {
            KeyValuePair<List<string>, List<byte[]>> ImgPathData = new KeyValuePair<List<string>, List<byte[]>>(new List<string>(), new List<byte[]>());

            
            if (YesNpNpk(NpkPath))
            {
                var NpkFs = File.OpenRead(NpkPath);
                BinaryReader NpkBr = new BinaryReader(NpkFs);

                NpkFs.Position = 16;//跳过npk头
                var ImgCount = NpkBr.ReadInt32();//获取img数量

                for (int i = 0; i < ImgCount; i++)
                {
                    var Offset = NpkBr.ReadInt32();//获取偏移
                    var Size = NpkBr.ReadInt32();//获取大小
                    string ImgName = ImgName解密(NpkBr.ReadBytes(256));
                    var Pos = NpkFs.Position;

                    NpkFs.Position = Offset;
                    ImgPathData.Key.Add(ImgName);
                    ImgPathData.Value.Add(NpkBr.ReadBytes(Size));
                    NpkFs.Position = Pos;

                }
                NpkBr.Dispose();

                return ImgPathData;
            }
            else
            {
                return ImgPathData;
            }
        }


        /// <summary>
        /// 打开单个npk返回img名称键、img数据值（重复的不会被载入）
        /// </summary>
        /// <param name="NpkPath">npk全路径</param>
        /// <returns>img名称和img数据</returns>
        public KeyValuePair<List<string>, List<byte[]>> 打开EX(string NpkPath)
        {
            KeyValuePair<List<string>, List<byte[]>> ImgPathData = new KeyValuePair<List<string>, List<byte[]>>(new List<string>(), new List<byte[]>());


            if (YesNpNpk(NpkPath))
            {
                var NpkFs = File.OpenRead(NpkPath);
                BinaryReader NpkBr = new BinaryReader(NpkFs);

                NpkFs.Position = 16;//跳过npk头
                var ImgCount = NpkBr.ReadInt32();//获取img数量

                for (int i = 0; i < ImgCount; i++)
                {
                    var Offset = NpkBr.ReadInt32();//获取偏移
                    var Size = NpkBr.ReadInt32();//获取大小
                    string ImgName = ImgName解密(NpkBr.ReadBytes(256));

                    if (ImgName.Contains("/character/") && ImgName.Contains("equipment/"))
                        if (ImgName.Contains("/weapon/") || ImgName.Contains("/avatar/"))
                        {
                            string LinImgName = ImgName.Substring(ImgName.LastIndexOf("/") + 1);
                            if (LinImgName.Remove(4) == "(tn)" || LinImgName.Contains("_mask1.img"))
                                continue;
                        }

                    var Pos = NpkFs.Position;
                    NpkFs.Position = Offset;
                    if (ImgPathData.Key.Contains(ImgName) ==false)
                    {
                        ImgPathData.Key.Add(ImgName);
                        ImgPathData.Value.Add(NpkBr.ReadBytes(Size));
                    }
                    NpkFs.Position = Pos;

                }
                NpkBr.Dispose();

                return ImgPathData;
            }
            else
            {
                return ImgPathData;
            }
        }


        /// <summary>
        /// 根据提供的img名称键、img数据值写出到指定路径并且覆盖
        /// </summary>
        /// <param name="NpkPath">npk全路径</param>
        /// <param name="ImgPathData">img名称键img数据值</param>
        public void 写出(string NpkPath, KeyValuePair<List<string>, List<byte[]>> ImgPathData)
        {
            int ImgCount = ImgPathData.Key.Count;

            FileStream stream = File.Create(NpkPath); //创建文件并覆盖
            MemoryStream checksumTemp = new MemoryStream();

            checksumTemp.Write(_head, 0, 16);//写入文件头
            checksumTemp.Write(BitConverter.GetBytes(ImgCount), 0, 4);//写入img数量

            int offset = 20 + (8 + 256) * ImgCount + 32;//得到初始偏移
            for (int i = 0; i < ImgCount; i++)
            {
                int ImgDataDaXiao = ImgPathData.Value[i].Length;
                checksumTemp.Write(BitConverter.GetBytes(offset), 0, 4); //写偏移
                checksumTemp.Write(BitConverter.GetBytes(ImgDataDaXiao), 0, 4); //写img大小
                checksumTemp.Write(ImgName加密(ImgPathData.Key[i]), 0, 256);//写入加密后的img名称

                offset += ImgDataDaXiao;//再次计算下一个偏移
            }

            byte[] Checksum = Npk计算校验(checksumTemp.ToArray());
            checksumTemp.WriteTo(stream);
            checksumTemp.Dispose();

            stream.Write(Checksum, 0, Checksum.Length);
            foreach (var item in ImgPathData.Value)
            {
                stream.Write(item, 0, item.Length); //把img数据逐一写入npk
            }

            stream.Dispose();
        }


        /// <summary>
        /// 打开单个NPK返回NPK名，img数据字典
        /// </summary>
        /// <param name="NpkPath">NPK全路径</param>
        /// <returns></returns>
        public Dictionary<string, NpkImgFile> 打开返回字典(string NpkPath)
        {
            Dictionary<string, NpkImgFile> ImgPathData = new Dictionary<string, NpkImgFile>();
            string NpkName = NpkPath.Substring(NpkPath.LastIndexOf("\\") + 1);
            ImgPathData.Add(NpkName, new NpkImgFile());

            if (YesNpNpk(NpkPath))
            {
                var NpkFs = File.OpenRead(NpkPath);
                BinaryReader NpkBr = new BinaryReader(NpkFs);

                NpkFs.Position = 16;//跳过npk头
                var ImgCount = NpkBr.ReadUInt32();//获取img数量
                for (int i = 0; i < ImgCount; i++)
                {
                    var Offset = NpkBr.ReadUInt32();//获取偏移
                    var Size = NpkBr.ReadUInt32();//获取大小
                    string ImgName = ImgName解密(NpkBr.ReadBytes(256));

                    var Pos = NpkFs.Position;
                    NpkFs.Position = Offset;
                    ImgPathData[NpkName].Name.Add(ImgName);
                    ImgPathData[NpkName].Data.Add(NpkBr.ReadBytes((int)Size));
                    NpkFs.Position = Pos;
                    
                }
                NpkBr.Dispose();

                return ImgPathData;
            }
            else
            {
                return ImgPathData;
            }
        }



        /// <summary>
        /// 新建img数据库后，直接可边查询边插入数据库信息
        /// </summary>
        /// <param name="DnfNpkPath">img目录</param>
        /// <param name="MdbPath">mdb全路径</param>
        public void AddAccess(string DnfNpkPath,string MdbPath)
        {
            //枚举npk文件
            List<string> dirs = new List<string>(Directory.EnumerateFiles(DnfNpkPath, "*.npk", SearchOption.TopDirectoryOnly));
            dirs.Sort();

            //mdb数据库连接
            //OleDbConnection olecon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + MdbPath);
            OleDbConnection olecon = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + MdbPath);
            olecon.Open();

            OleDbCommand cmd = new OleDbCommand
            {
                Connection = olecon,
                Transaction = olecon.BeginTransaction()
            };
            //mdb数据库连接

            uint MdbId = 0;

            foreach (string item in dirs)
            {
                try
                {
                    if (YesNpNpk(item))
                    {
                        //开始读取
                        FileStream fileStream = new FileStream(item, FileMode.Open, FileAccess.Read);
                        using (BinaryReader binaryReader = new BinaryReader(fileStream))
                        {
                            fileStream.Position = 16;//跳过npk头
                            int npksm = BitConverter.ToInt32(binaryReader.ReadBytes(4), 0);//获取img数量

                            //获取npk文件名
                            string NpkName = item.Substring(item.LastIndexOf("\\") + 1);

                            if (NpkName.Contains("'"))
                            {
                                NpkName = NpkName.Replace("'", "''");
                            }

                            for (int i = 0; i < npksm; i++)
                            {
                                fileStream.Position += 4;//跳过偏移
                                fileStream.Position += 4;//跳过大小

                                string imgname = ImgName解密(binaryReader.ReadBytes(256));//获取解密后的img名称
                                if (imgname.Contains("'"))
                                {
                                    imgname = imgname.Replace("'", "''");
                                }

                                //获取img文件名
                                string img = imgname.Substring(imgname.LastIndexOf("/") + 1);

                                //提交给事务
                                cmd.CommandText = "insert into ImagePacks2(id,img全路径,img名称,npk文件名) values('" + MdbId + "','" + imgname + "','" + img + "','" + NpkName + "')";
                                cmd.ExecuteNonQuery();

                                MdbId++;

                            }
                        }
                        fileStream.Dispose();
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show($"在对已创建的access数据增加img全路径等数据时发生错误,错误文件路径：{item}，错误img坐标为：{MdbId}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            cmd.Transaction.Commit();  //提交事务
            cmd.Dispose();
            olecon.Close();
        }

             
   
        /// <summary>
        /// 提供需要查找的目录路径，按不同的查找方式返回img全路径
        /// </summary>
        /// <param name="XunZhaoPath">寻找的目录路径</param>
        /// <param name="ImgNameJiHe">img全路径集合</param>
        /// <param name="MoShi">0:装备模型img 1：明文img 2：npk中的img</param>
        public void Img所有全路径(string XunZhaoPath, List<string> ImgNameJiHe, uint MoShi,StringBuilder FanKuiCuoWu)
        {
            Dnf dnf = new Dnf();
            Regex_new regex = new Regex_new();
            switch (MoShi)
            {
                case 0:
                    List<string> EquPathItem = new List<string>(Directory.EnumerateFiles(XunZhaoPath, "*.equ", SearchOption.AllDirectories));
                    foreach (string item in EquPathItem)
                    {
                        string EquAllText = File.ReadAllText(item);//得到所有文件内容
                        //判断文件内是否存在img图层
                        if (EquAllText.Contains("[variation]"))
                        {
                            //正则捕获equ内的图层
                             regex.创建(EquAllText, @"(\[animation job\][\r\n]+`\[.+\]`[\r\n]+)?\[variation\][\r\n]+[0-9]+\t[0-9]+\t[\r\n]+(\[layer variation\][\r\n]+[0-9]+\t`.+`[\r\n]+\[equipment ani script\][\r\n]+`equipment[/\\].+\.lay`[\r\n]+){0,}", 1);
                            if (regex.取匹配数量() > 0)
                            {
                                int CuoWu = -1;//创建临时变量；用于接收转换的img是否正常；不正常则判断这个equ是错误的
                                foreach (Match items in regex.正则返回集合())
                                {
                                    CuoWu = dnf.EquAniFanHuiImgPath(items.Value, ImgNameJiHe);
                                }
                                if (CuoWu == 0)
                                {
                                    FanKuiCuoWu.Append(item+"\r\n");
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    List<string> PathItem = new List<string>(Directory.EnumerateFiles(XunZhaoPath, "*.*", SearchOption.AllDirectories));

                    foreach (string item in PathItem)
                    {
                        switch (new Dnf().PathGetExtension(item))
                        {
                            case ".txt":
                            case ".exe":
                            case ".pvf":
                            case ".dll":
                            case ".7z":
                            case ".zip":
                            case ".rar":
                            case ".png":
                            case ".jpg":
                            case ".gif":
                            case ".avi":
                            case ".mp3":
                            case ".mp4":
                                continue;
                        }
                        
                        List<string> FileAllText = new List<string>(File.ReadAllText(item).Split(new char[] { '`' }, StringSplitOptions.RemoveEmptyEntries));
                        foreach (string Img in FileAllText)
                        {
                            string ImgPath = Img.ToLower().Replace("`", "");
                            if (ImgPath.Contains(".img") == false)
                                continue;

                            int Pos = ImgPath.IndexOf("%d%d.img");
                            if (Pos != -1)
                            {
                                string F_Path = regex.替换("sprite/" + ImgPath.Remove(Pos).Replace("\\", "/"), "[/]{2,}|^[/]+", "");
                                for (int i = 1; i < 10; i++)
                                {
                                    if (ImgNameJiHe.Contains($"{F_Path}0{i}.img") == false)
                                        ImgNameJiHe.Add($"{F_Path}0{i}.img");
                                }
                                continue;
                            }

                            ImgPath = regex.替换("sprite/" + ImgPath.Replace("\\", "/"), "[/]{2,}|^[/]+", "");
                            if (ImgNameJiHe.Contains(ImgPath) == false)
                                ImgNameJiHe.Add(ImgPath);
                        }
                    }

                        break;
                case 2:
                    //枚举提供目录的所有npk文件，并且按顺序排序
                    List<string> dirs = new List<string>(Directory.EnumerateFiles(XunZhaoPath, "*.npk", SearchOption.TopDirectoryOnly));
                    dirs.Sort();

                    //获取每个npk文件中的img全路径
                    foreach (string item in dirs)
                    {
                        
                        if (YesNpNpk(item))
                        {
                            //开始读取
                            FileStream fileStream = new FileStream(item, FileMode.Open, FileAccess.Read);
                            BinaryReader binaryReader = new BinaryReader(fileStream);

                            fileStream.Position = 16;//跳过npk头
                            int npksm = BitConverter.ToInt32(binaryReader.ReadBytes(4), 0);//获取img数量
                            for (int i = 0; i < npksm; i++)
                            {
                                fileStream.Position += 4;//跳过偏移
                                fileStream.Position += 4;//跳过大小

                                string imgname = ImgName解密(binaryReader.ReadBytes(256));//获取解密后的img名称
                                if (ImgNameJiHe.Contains(imgname) == false)
                                {
                                    ImgNameJiHe.Add(imgname);
                                }
                            }

                            binaryReader.Dispose();//释放读取的文件
                            fileStream.Dispose();//释放读取的文件

                        }
                    }
                    break;
            }

        }


        /// <summary>
        /// 提供枚举好的NPK全路径，返回读取的所有img全路径以及NPK文件名集合
        /// </summary>
        /// <param name="dirs">枚举好的NPK全路径</param>
        /// <param name="NpkNameJiHe">NPK文件名集合</param>
        /// <param name="ImgNameJiHe">img全路径集合</param>
        public void Img所有全路径( List<string> dirs, List<string> NpkNameJiHe, List<string> ImgNameJiHe)
        {

            //获取每个npk文件中的img全路径
            foreach (string item in dirs)
            {
                try
                {
                    if (YesNpNpk(item))
                    {
                        //开始读取
                        using FileStream fileStream = new FileStream(item, FileMode.Open, FileAccess.Read);
                        using BinaryReader binaryReader = new BinaryReader(fileStream);
                        fileStream.Position = 16;//跳过NPK头
                        int npksm = BitConverter.ToInt32(binaryReader.ReadBytes(4), 0);//获取img数量
                        string NpkName = item.Substring(item.LastIndexOf("\\") + 1);
                        for (int i = 0; i < npksm; i++)
                        {
                            fileStream.Position += 4;//跳过偏移
                            fileStream.Position += 4;//跳过大小

                            string imgname = ImgName解密(binaryReader.ReadBytes(256));//获取解密后的img名称

                            NpkNameJiHe.Add(NpkName);
                            ImgNameJiHe.Add(imgname);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"在获取所有npk内img全路径时发生错误，错误路径为：{item}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        /// <summary>
        /// 根据用户提供的img全路径文本以及NPK目录，返回对应的img全路径以及img数据
        /// </summary>
        /// <param name="AllImgText">img全路径文本</param>
        /// <param name="ImagePacks2Path">NPK所在目录</param>
        /// <param name="NpkNameJiHe">NPK文件名集合</param>
        /// <param name="ImgNameJiHe">img全路径集合</param>
        /// <param name="NpkNo">返回的NPK错误</param>
        /// <param name="ImgNo">返回的img错误</param>
        /// <returns></returns>
        public Dictionary<string, NpkImgFile> NpkImgZhengliData(string AllImgText, string ImagePacks2Path,  List<string> NpkNameJiHe, List<string> ImgNameJiHe, StringBuilder NpkNo, StringBuilder ImgNo)
        {
            Regex_new regex_New = new Regex_new();
            Dictionary<string, NpkImgFile> NpkImgList = new Dictionary<string, NpkImgFile>();

            regex_New.创建(AllImgText, @"sprite/.+\.img", 1);
            if (regex_New.取匹配数量()>0)
            {
                string BuHuoImgPath;
                int ID;
                string NpkName;
                string ImgPath;
                foreach (Match item in regex_New.正则返回集合())
                {
                    BuHuoImgPath = item.Value;
                    ID = ImgNameJiHe.IndexOf(BuHuoImgPath);
                    if (ID != -1)
                    {
                        NpkName = NpkNameJiHe[ID];
                        ImgPath = ImgNameJiHe[ID];
                        if (NpkImgList.ContainsKey(NpkName) ==true)
                        {
                            if (NpkImgList[NpkName].Name.Contains(ImgPath) == false)
                            {
                                NpkImgList[NpkName].Name.Add(ImgPath);
                                NpkImgList[NpkName].Data.Add(new byte[1]);
                            }
                        }
                        else
                        {
                            NpkImgList.Add(NpkName, new NpkImgFile(ImgPath, new byte[1]));
                        }
                    }
                    else
                    {
                        ImgNo.Append(item + "\t(未找到)\r\n");
                    }

                }
            }

            if (NpkImgList.Count>0)
            {
                foreach (string item in NpkImgList.Keys)
                {
                    if (File.Exists(ImagePacks2Path + "\\" + item) == true)
                    {

                        if (YesNpNpk(ImagePacks2Path + "\\" + item))
                        {
                            var NpkFs = File.OpenRead(ImagePacks2Path + "\\" + item);
                            BinaryReader NpkBr = new BinaryReader(NpkFs);

                            NpkFs.Position = 16;//跳过npk头
                            var ImgCount = NpkBr.ReadUInt32();//获取img数量
                            for (int i = 0; i < ImgCount; i++)
                            {
                                var Offset = NpkBr.ReadUInt32();//获取偏移
                                var Size = NpkBr.ReadUInt32();//获取大小
                                string ImgName = ImgName解密(NpkBr.ReadBytes(256));

                                for (int j = 0; j < NpkImgList[item].Name.Count; j++)
                                {
                                    if (NpkImgList[item].Name[j] == ImgName)
                                    {
                                        var Pos = NpkFs.Position;
                                        NpkFs.Position = Offset;
                                        NpkImgList[item].Data[j] = NpkBr.ReadBytes((int)Size);
                                        NpkFs.Position = Pos;
                                    }
                                }
                                
                            }
                            NpkBr.Dispose();

                        }
                        else
                        {
                            NpkNo.Append(item + "\t(不是NPK文件)\r\n");
                        }


                    }
                    else
                    {
                        NpkNo.Append(item + "\t(未找到)\r\n");
                    }

                }

                foreach (NpkImgFile item in NpkImgList.Values)
                {
                    List<int> DeleteId = new List<int>();

                    for (int i = 0; i < item.Data.Count; i++)
                    {
                        if (item.Data[i].Length == 1)
                        {
                            DeleteId.Add(i);
                        }
                    }

                    DeleteId.Sort();
                    int LinShi = 0;
                    for (int i = 0; i < DeleteId.Count; i++)
                    {
                        item.Data.RemoveAt(DeleteId[i] - LinShi);
                        ImgNo.Append(item.Name[DeleteId[i] - LinShi] + "\t(未找到的img)\r\n");
                        item.Name.RemoveAt(DeleteId[i] - LinShi);

                        LinShi++;
                    }

                    DeleteId.Clear();
                }
            }

            return NpkImgList;
        }



        /// <summary>
        /// 根据用户提供的img全路径集合以及NPK目录，返回对应的img全路径以及img数据
        /// </summary>
        /// <param name="AllImgText">img全路径文本</param>
        /// <param name="ImagePacks2Path">NPK所在目录</param>
        /// <param name="NpkNameJiHe">NPK文件名集合</param>
        /// <param name="ImgNameJiHe">img全路径集合</param>
        /// <param name="NpkNo">返回的NPK错误</param>
        /// <param name="ImgNo">返回的img错误</param>
        /// <returns></returns>
        public Dictionary<string, NpkImgFile> NpkImgZhengliData(List<string> AllImgPath, string ImagePacks2Path, List<string> NpkNameJiHe, List<string> ImgNameJiHe, StringBuilder NpkNo, StringBuilder ImgNo)
        {
            Dictionary<string, NpkImgFile> NpkImgList = new Dictionary<string, NpkImgFile>();
            if (AllImgPath.Count>0)
            {
                foreach (string imgpath in AllImgPath)
                {
                    string BuHuoImgPath;
                    int ID;
                    string NpkName;
                    string ImgPath;

                    BuHuoImgPath = imgpath;
                    ID = ImgNameJiHe.IndexOf(BuHuoImgPath);
                    if (ID != -1)
                    {
                        NpkName = NpkNameJiHe[ID];
                        ImgPath = ImgNameJiHe[ID];
                        if (NpkImgList.ContainsKey(NpkName) == true)
                        {
                            if (NpkImgList[NpkName].Name.Contains(ImgPath) == false)
                            {
                                NpkImgList[NpkName].Name.Add(ImgPath);
                                NpkImgList[NpkName].Data.Add(new byte[1]);
                            }
                        }
                        else
                        {
                            NpkImgList.Add(NpkName, new NpkImgFile(ImgPath, new byte[1]));
                        }
                    }
                    else
                    {
                        ImgNo.Append(imgpath + "\t(未找到)\r\n");
                    }
                  
                }
            }
            
            if (NpkImgList.Count > 0)
            {
                foreach (string item in NpkImgList.Keys)
                {
                    if (File.Exists(ImagePacks2Path + "\\" + item) == true)
                    {
                        if (YesNpNpk(ImagePacks2Path + "\\" + item))
                        {
                            var NpkFs = File.OpenRead(ImagePacks2Path + "\\" + item);
                            BinaryReader NpkBr = new BinaryReader(NpkFs);

                            NpkFs.Position = 16;//跳过npk头
                            var ImgCount = NpkBr.ReadUInt32();//获取img数量
                            for (int i = 0; i < ImgCount; i++)
                            {
                                var Offset = NpkBr.ReadUInt32();//获取偏移
                                var Size = NpkBr.ReadUInt32();//获取大小
                                string ImgName = ImgName解密(NpkBr.ReadBytes(256));

                                for (int j = 0; j < NpkImgList[item].Name.Count; j++)
                                {
                                    if (NpkImgList[item].Name[j] == ImgName)
                                    {
                                        var Pos = NpkFs.Position;
                                        NpkFs.Position = Offset;
                                        NpkImgList[item].Data[j] = NpkBr.ReadBytes((int)Size);
                                        NpkFs.Position = Pos;
                                    }
                                }


                            }
                            NpkBr.Dispose();
                        }
                        else
                        {
                            NpkNo.Append(item + "\t(不是NPK文件)\r\n");
                        }


                    }
                    else
                    {
                        NpkNo.Append(item + "\t(未找到)\r\n");
                    }

                }

                foreach (NpkImgFile item in NpkImgList.Values)
                {
                    List<int> DeleteId = new List<int>();

                    for (int i = 0; i < item.Data.Count; i++)
                    {
                        if (item.Data[i].Length == 1)
                        {
                            DeleteId.Add(i);
                        }
                    }

                    DeleteId.Sort();
                    int LinShi = 0;
                    for (int i = 0; i < DeleteId.Count; i++)
                    {
                        item.Data.RemoveAt(DeleteId[i] - LinShi);
                        ImgNo.Append(item.Name[DeleteId[i] - LinShi] + "\t(未找到的img)\r\n");
                        item.Name.RemoveAt(DeleteId[i] - LinShi);

                        LinShi++;
                    }

                    DeleteId.Clear();
                }
            }

            return NpkImgList;
        }




        /// <summary>
        /// 提供NPKIMG以及IMG数据字典，按照不同的方式写出
        /// </summary>
        /// <param name="NpkImgJiHe">NPKimg字典集合</param>
        /// <param name="DaoChuPath">导出的目录</param>
        /// <param name="YesNoHeBing">0：合并后写出 1：按原NPK名写出 2：按img全路径名写出</param>
        public void DaoChuNpkImg(Dictionary<string, NpkImgFile> NpkImgJiHe,string DaoChuPath,uint YesNoHeBing)
        {
            FileStream stream;
            MemoryStream checksumTemp;
            int offset;
            byte[] Checksum;
            switch (YesNoHeBing)
            {
                case 0:
                    int DaoChuImgCount = 0;
                    foreach (NpkImgFile item in NpkImgJiHe.Values)
                    {
                        DaoChuImgCount += item.Name.Count;
                    }

                    string XieChuTime = "\\" + DateTime.Now.ToString("hh_mm_ss_");

                    stream = File.Create(DaoChuPath + XieChuTime + "提取合并.NPK"); //创建文件
                    checksumTemp = new MemoryStream();

                    checksumTemp.Write(_head, 0, 16);
                    checksumTemp.Write(BitConverter.GetBytes(DaoChuImgCount), 0, 4);

                    offset = 20 + (8 + 256) * DaoChuImgCount + 32;
                    foreach (NpkImgFile item in NpkImgJiHe.Values)
                    {
                        for (int i = 0; i < item.Data.Count; i++)
                        {
                            checksumTemp.Write(BitConverter.GetBytes(offset), 0, 4); //写偏移
                            checksumTemp.Write(BitConverter.GetBytes(item.Data[i].Length), 0, 4); //写img大小
                            checksumTemp.Write(ImgName加密(item.Name[i]), 0, 256);

                            offset += item.Data[i].Length;
                        }

                    }
                    Checksum = Npk计算校验(checksumTemp.ToArray());
                    checksumTemp.WriteTo(stream);
                    checksumTemp.Close();

                    stream.Write(Checksum, 0, Checksum.Length);
                    foreach (NpkImgFile item in NpkImgJiHe.Values)
                    {
                        foreach (byte[] items in item.Data)
                        {
                            stream.Write(items, 0, items.Length); //把img数据逐一写入npk
                        }
                    }

                    stream.Close();
                    break;
                case 1:
                    foreach (string npkname in NpkImgJiHe.Keys)
                    {
                        stream = File.Create(DaoChuPath + "\\" + npkname); //创建文件
                        checksumTemp = new MemoryStream();

                        checksumTemp.Write(_head, 0, 16);
                        checksumTemp.Write(BitConverter.GetBytes(NpkImgJiHe[npkname].Name.Count), 0, 4);

                        offset = 20 + (8 + 256) * NpkImgJiHe[npkname].Name.Count + 32;

                        for (int i = 0; i < NpkImgJiHe[npkname].Name.Count; i++)
                        {
                            checksumTemp.Write(BitConverter.GetBytes(offset), 0, 4); //写偏移
                            checksumTemp.Write(BitConverter.GetBytes(NpkImgJiHe[npkname].Data[i].Length), 0, 4); //写img大小
                            checksumTemp.Write(ImgName加密(NpkImgJiHe[npkname].Name[i]), 0, 256);

                            offset += NpkImgJiHe[npkname].Data[i].Length;
                        }

                        Checksum = Npk计算校验(checksumTemp.ToArray());
                        checksumTemp.WriteTo(stream);
                        checksumTemp.Close();

                        stream.Write(Checksum, 0, Checksum.Length);
                        foreach (byte[] item in NpkImgJiHe[npkname].Data)
                        {

                            stream.Write(item, 0, item.Length); //把img数据逐一写入npk

                        }
                        stream.Close();
                    }
                    break;
                case 2:
                    SortedDictionary<string, NpkImgFile> ChongXinImg = new SortedDictionary<string, NpkImgFile>();

                    Dictionary<string, byte[]> LinShiImg = new Dictionary<string, byte[]>();

                    foreach (NpkImgFile item in NpkImgJiHe.Values)
                    {
                        for (int i = 0; i < item.Name.Count; i++)
                        {
                            LinShiImg.Add(item.Name[i], item.Data[i]);
                        }
                    }

                    foreach (var item in LinShiImg)
                    {
                        string ImgName = item.Key;
                        string NpkName = ImgName.Remove(ImgName.LastIndexOf("/")).Replace("/", "_") + ".NPK";
                        if (ChongXinImg.ContainsKey(NpkName) == false)
                        {
                            ChongXinImg.Add(NpkName, new NpkImgFile(ImgName, item.Value));
                        }
                        else
                        {
                            ChongXinImg[NpkName].Name.Add(ImgName);
                            ChongXinImg[NpkName].Data.Add(item.Value);
                        }

                    }

                    foreach (string npkname in ChongXinImg.Keys)
                    {
                        stream = File.Create(DaoChuPath + "\\" + npkname); //创建文件
                        checksumTemp = new MemoryStream();

                        checksumTemp.Write(_head, 0, 16);
                        checksumTemp.Write(BitConverter.GetBytes(ChongXinImg[npkname].Name.Count), 0, 4);

                        offset = 20 + (8 + 256) * ChongXinImg[npkname].Name.Count + 32;

                        for (int i = 0; i < ChongXinImg[npkname].Name.Count; i++)
                        {
                            checksumTemp.Write(BitConverter.GetBytes(offset), 0, 4); //写偏移
                            checksumTemp.Write(BitConverter.GetBytes(ChongXinImg[npkname].Data[i].Length), 0, 4); //写img大小
                            checksumTemp.Write(ImgName加密(ChongXinImg[npkname].Name[i]), 0, 256);

                            offset += ChongXinImg[npkname].Data[i].Length;
                        }

                        Checksum = Npk计算校验(checksumTemp.ToArray());
                        checksumTemp.WriteTo(stream);
                        checksumTemp.Close();

                        stream.Write(Checksum, 0, Checksum.Length);
                        foreach (byte[] item in ChongXinImg[npkname].Data)
                        {

                            stream.Write(item, 0, item.Length); //把img数据逐一写入npk

                        }
                        stream.Close();
                    }
                    break;
            }
            
        }



        /// <summary>
        /// 提供NPKIMG以及IMG数据字典，按照不同的方式加入到NPK内
        /// </summary>
        /// <param name="NpkImgJiHe">NPKimg字典集合</param>
        /// <param name="DaoChuPath">导出的目录</param>
        /// <param name="YesNoHeBing">0：按原NPK名写出 1：按img全路径名写出</param>
        /// <param name="YesNoDelete">0：有重复删掉对方 1：有重复删掉我自己</param>
        public void DaoChuAddNpkImg(Dictionary<string, NpkImgFile> NpkImgJiHe, string DaoChuPath, uint YesNoHeBing,uint YesNoDelete)
        {
            switch (YesNoHeBing)
            {
                case 1:
                    ChongXinPaiLie(NpkImgJiHe);
                    break;
            }

            foreach (string NpkName in NpkImgJiHe.Keys)
            {
                if (File.Exists(DaoChuPath + "\\" + NpkName) == true)
                {
                    KeyValuePair<List<string>, List<byte[]>> YuanData = 打开(DaoChuPath + "\\" + NpkName);
                    DeleteAndPaiLie(YuanData, NpkImgJiHe[NpkName], YesNoDelete);
                    写出(DaoChuPath + "\\" + NpkName, YuanData);
                }
                else
                {
                    写出(DaoChuPath + "\\" + NpkName, new KeyValuePair<List<string>, List<byte[]>>(NpkImgJiHe[NpkName].Name, NpkImgJiHe[NpkName].Data));
                }
            }
        }



        /// <summary>
        /// 提供NPKIMG以及IMG数据字典，按照不同的方式加入到NPK内，如果文件不存在则不会写出
        /// </summary>
        /// <param name="NpkImgJiHe">NPKimg字典集合</param>
        /// <param name="DaoChuPath">导出的目录</param>
        /// <param name="YesNoHeBing">0：按原NPK名写出 1：按img全路径名写出</param>
        /// <param name="YesNoDelete">0：有重复删掉对方 1：有重复删掉我自己</param>
        /// <param name="NoNpkName">返回不存在的NPK文件名</param>
        public void DaoChuAddNpkImg(Dictionary<string, NpkImgFile> NpkImgJiHe, string DaoChuPath, uint YesNoHeBing, uint YesNoDelete,StringBuilder NoNpkName)
        {
            switch (YesNoHeBing)
            {
                case 1:
                    ChongXinPaiLie(NpkImgJiHe);
                    break;
            }

            foreach (string NpkName in NpkImgJiHe.Keys)
            {
                if (File.Exists(DaoChuPath + "\\" + NpkName) == true)
                {
                    KeyValuePair<List<string>, List<byte[]>> YuanData = 打开(DaoChuPath + "\\" + NpkName);
                    DeleteAndPaiLie(YuanData, NpkImgJiHe[NpkName], YesNoDelete);
                    写出(DaoChuPath + "\\" + NpkName, YuanData);
                }
                else
                {
                    NoNpkName.Append(NpkName+"\r\n");
                }
            }
        }


        /// <summary>
        /// 根据IMG全路径文本、npk名、img全路径集合。按照不同模式返回npk名+img集合字典
        /// </summary>
        /// <param name="AllImgText">所有img全路径文本</param>
        /// <param name="NpkNameJiHe">npk名集合</param>
        /// <param name="ImgNameJiHe">img全路径集合</param>
        /// <param name="ImgNo">img错误反馈</param>
        /// <param name="MoShi">0：只找第一个 1：只找除了第一个 2：所有</param>
        /// <returns>npk名+img集合字典</returns>
        public Dictionary<string, List<string>> NpkImgZhengLiDeleteList(string AllImgText, List<string> NpkNameJiHe, List<string> ImgNameJiHe, StringBuilder ImgNo,uint MoShi)
        {
            Regex_new regex_New = new Regex_new();
            Dictionary<string, List<string>> NpkImgList = new Dictionary<string, List<string>>();

            regex_New.创建(AllImgText, @"sprite/.+\.img", 1);
            if (regex_New.取匹配数量() > 0)
            {
                string BuHuoImgPath;
                int ID;
                string NpkName;
                string ImgPath;
                foreach (Match item in regex_New.正则返回集合())
                {
                    BuHuoImgPath = item.Value;
                    ID = ImgNameJiHe.IndexOf(BuHuoImgPath);
                    if (ID != -1)
                    {
                        switch (MoShi)
                        {
                            case 0:
                                NpkName = NpkNameJiHe[ID];
                                ImgPath = ImgNameJiHe[ID];
                                if (NpkImgList.ContainsKey(NpkName) == true)
                                {
                                    if (NpkImgList[NpkName].Contains(ImgPath) == false)
                                    {
                                        NpkImgList[NpkName].Add(ImgPath);
                                    }
                                }
                                else
                                {
                                    NpkImgList.Add(NpkName, new List<string> { ImgPath });
                                }
                                break;
                            case 1:
                                int IDHou;
                                IDHou = ImgNameJiHe.IndexOf(BuHuoImgPath, ID + 1);
                                while (IDHou != -1)
                                {
                                    NpkName = NpkNameJiHe[IDHou];
                                    ImgPath = ImgNameJiHe[IDHou];
                                    if (NpkImgList.ContainsKey(NpkName) == true)
                                    {
                                        if (NpkImgList[NpkName].Contains(ImgPath) == false)
                                        {
                                            NpkImgList[NpkName].Add(ImgPath);
                                        }
                                    }
                                    else
                                    {
                                        NpkImgList.Add(NpkName, new List<string> { ImgPath });
                                    }
                                    IDHou = ImgNameJiHe.IndexOf(BuHuoImgPath, IDHou + 1);
                                }
                                break;
                            case 2:
                                do
                                {
                                    NpkName = NpkNameJiHe[ID];
                                    ImgPath = ImgNameJiHe[ID];
                                    if (NpkImgList.ContainsKey(NpkName) == true)
                                    {
                                        if (NpkImgList[NpkName].Contains(ImgPath) == false)
                                        {
                                            NpkImgList[NpkName].Add(ImgPath);
                                        }
                                    }
                                    else
                                    {
                                        NpkImgList.Add(NpkName, new List<string>{ImgPath});
                                    }
                                    ID = ImgNameJiHe.IndexOf(BuHuoImgPath, ID + 1);
                                } while (ID != -1);
                                break;
                        }
                    }
                    else
                    {
                        ImgNo.Append(item + "\t(未找到)\r\n");
                    }
                }
            }
            return NpkImgList;
        }



        /// <summary>
        /// 根据IMG全路径、npk名、img全路径集合。按照不同模式返回npk名+img集合字典
        /// </summary>
        /// <param name="AllImgText">img全路径集合</param>
        /// <param name="NpkNameJiHe">npk名集合</param>
        /// <param name="ImgNameJiHe">img全路径集合</param>
        /// <param name="ImgNo">img错误反馈</param>
        /// <param name="MoShi">0：只找第一个 1：只找除了第一个 2：所有</param>
        /// <returns>npk名+img集合字典</returns>
        public Dictionary<string, List<string>> NpkImgZhengLiDeleteList(List<string> AllImgText, List<string> NpkNameJiHe, List<string> ImgNameJiHe, StringBuilder ImgNo, uint MoShi)
        {
            Dictionary<string, List<string>> NpkImgList = new Dictionary<string, List<string>>();

            int ID;
            string NpkName;
            string ImgPath;
            foreach (string BuHuoImgPath in AllImgText)
            {
                ID = ImgNameJiHe.IndexOf(BuHuoImgPath);
                if (ID != -1)
                {
                    switch (MoShi)
                    {
                        case 0:
                            NpkName = NpkNameJiHe[ID];
                            ImgPath = ImgNameJiHe[ID];
                            if (NpkImgList.ContainsKey(NpkName) == true)
                            {
                                if (NpkImgList[NpkName].Contains(ImgPath) == false)
                                {
                                    NpkImgList[NpkName].Add(ImgPath);
                                }
                            }
                            else
                            {
                                NpkImgList.Add(NpkName, new List<string> { ImgPath });
                            }
                            break;
                        case 1:
                            int IDHou;
                            IDHou = ImgNameJiHe.IndexOf(BuHuoImgPath, ID + 1);
                            while (IDHou != -1)
                            {
                                NpkName = NpkNameJiHe[IDHou];
                                ImgPath = ImgNameJiHe[IDHou];
                                if (NpkImgList.ContainsKey(NpkName) == true)
                                {
                                    if (NpkImgList[NpkName].Contains(ImgPath) == false)
                                    {
                                        NpkImgList[NpkName].Add(ImgPath);
                                    }
                                }
                                else
                                {
                                    NpkImgList.Add(NpkName, new List<string> { ImgPath });
                                }
                                IDHou = ImgNameJiHe.IndexOf(BuHuoImgPath, IDHou + 1);
                            }
                            break;
                        case 2:
                            do
                            {
                                NpkName = NpkNameJiHe[ID];
                                ImgPath = ImgNameJiHe[ID];
                                if (NpkImgList.ContainsKey(NpkName) == true)
                                {
                                    if (NpkImgList[NpkName].Contains(ImgPath) == false)
                                    {
                                        NpkImgList[NpkName].Add(ImgPath);
                                    }
                                }
                                else
                                {
                                    NpkImgList.Add(NpkName, new List<string> { ImgPath });
                                }
                                ID = ImgNameJiHe.IndexOf(BuHuoImgPath, ID + 1);
                            } while (ID != -1);
                            break;
                    }
                }
                else
                {
                    ImgNo.Append(BuHuoImgPath + "\t(未找到)\r\n");
                }
            }
            return NpkImgList;
        }



        /// <summary>
        /// 一个个npk处理把不删除的取出，再写入覆盖；变相删除某个img
        /// </summary>
        /// <param name="ImagePacks2Path">NPK所在目录</param>
        /// <param name="AllNpkImg">NPK img集合</param>
        public void DeleteImg(string ImagePacks2Path, Dictionary<string, List<string>> AllNpkImg)
        {

            foreach (string npkname in AllNpkImg.Keys)
            {
                try
                {
                    Dictionary<string, NpkImgFile> BaoLiuDeNpkImg = new Dictionary<string, NpkImgFile>();

                    var NpkFs = File.OpenRead(ImagePacks2Path + "\\" + npkname);
                    BinaryReader NpkBr = new BinaryReader(NpkFs);

                    NpkFs.Position = 16;//跳过npk头
                    var ImgCount = NpkBr.ReadUInt32();//获取img数量
                    for (int i = 0; i < ImgCount; i++)
                    {
                        var Offset = NpkBr.ReadUInt32();//获取偏移
                        var Size = NpkBr.ReadUInt32();//获取大小
                        string ImgName = ImgName解密(NpkBr.ReadBytes(256));//获取名称


                        if (AllNpkImg[npkname].Contains(ImgName) == false)
                        {
                            var Pos = NpkFs.Position;
                            NpkFs.Position = Offset;
                            if (BaoLiuDeNpkImg.ContainsKey(npkname) == true)
                            {
                                if (BaoLiuDeNpkImg[npkname].Name.Contains(ImgName) == false)
                                {
                                    BaoLiuDeNpkImg[npkname].Name.Add(ImgName);
                                    BaoLiuDeNpkImg[npkname].Data.Add(NpkBr.ReadBytes((int)Size));
                                }
                            }
                            else
                            {
                                BaoLiuDeNpkImg.Add(npkname, new NpkImgFile(ImgName, NpkBr.ReadBytes((int)Size)));
                            }

                            NpkFs.Position = Pos;
                        }

                    }
                    NpkBr.Dispose();

                    if (BaoLiuDeNpkImg.Count > 0)
                    {
                        DaoChuNpkImg(BaoLiuDeNpkImg, ImagePacks2Path, 1);
                    }
                    else
                    {
                        File.Delete(ImagePacks2Path + "\\" + npkname);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"在最后删除重复img时发生错误，错误路径为：{npkname}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                
            }
        }


        /// <summary>
        /// 按照img全路径名，重新排列NPK名称
        /// </summary>
        /// <param name="NpkImgJiHe">NPK、img全路径、img数据字典</param>
        public void ChongXinPaiLie(Dictionary<string, NpkImgFile> NpkImgJiHe)
        {
            Dictionary<string, NpkImgFile> LinShiNpkImgJiHe = new Dictionary<string, NpkImgFile>(NpkImgJiHe);
            NpkImgJiHe.Clear();

            foreach (NpkImgFile item in LinShiNpkImgJiHe.Values)
            {
                for (int i = 0; i < item.Name.Count; i++)
                {
                    string ImgName = item.Name[i];
                    int Pos = ImgName.LastIndexOf("/");
                    if (Pos!=-1)
                    {
                        string NpkName = ImgName.Remove(Pos).Replace("/", "_") + ".NPK";
                        if (NpkImgJiHe.ContainsKey(NpkName) == false)
                        {
                            NpkImgJiHe.Add(NpkName, new NpkImgFile(ImgName, item.Data[i]));
                        }
                        else if (NpkImgJiHe[NpkName].Name.Contains(ImgName) == false)
                        {
                            NpkImgJiHe[NpkName].Name.Add(ImgName);
                            NpkImgJiHe[NpkName].Data.Add(item.Data[i]);
                        }
                    }
                    
                }
            }
        }



        /// <summary>
        /// 提供读入的键值img数据，要加入的img数据，根据不同的删除重复方式来加入原键值中
        /// </summary>
        /// <param name="YuanData">读入的键值img数据</param>
        /// <param name="AddData">要加入的img数据</param>
        /// <param name="YesNoDelete">重复时--0：删除读入的 1：删除加入的</param>
        public void DeleteAndPaiLie(KeyValuePair<List<string>, List<byte[]>> YuanData, NpkImgFile AddData,uint YesNoDelete)
        {
            switch (YesNoDelete)
            {
                case 0:
                    foreach (string item in AddData.Name)
                    {
                        int DeletePos = YuanData.Key.IndexOf(item);
                        if (DeletePos!=-1)
                        {
                            YuanData.Key.RemoveAt(DeletePos);
                            YuanData.Value.RemoveAt(DeletePos);
                        }
                    }
                    break;
                case 1:
                    foreach (string item in YuanData.Key)
                    {
                        int DeletePos = AddData.Name.IndexOf(item);
                        if (DeletePos != -1)
                        {
                            AddData.Name.RemoveAt(DeletePos);
                            AddData.Data.RemoveAt(DeletePos);
                        }
                    }
                    break;
            }
            YuanData.Key.AddRange(AddData.Name);
            YuanData.Value.AddRange(AddData.Data);

        }


        /// <summary>
        /// 根据提供的NPK路径集合，按指定大小合并为一个NPK或多个大小的NPK
        /// </summary>
        /// <param name="DaiHeBingFilePath">NPK全路径集合</param>
        /// <param name="XieChuFilePath">写出文件的路径</param>
        /// <param name="FanKuiXinXi">返回错误NPK信息</param>
        /// <param name="Jdt">进度条</param>
        /// <param name="MaxSize">指定大小</param>
        public void HeBingNpk(List<string> DaiHeBingFilePath,string XieChuFilePath,StringBuilder FanKuiXinXi,ProgressBar Jdt,long MaxSize)
        {

            if (MaxSize>0)
            {
                if (DaiHeBingFilePath.Count <= 0)
                    return;

                List<string> ImgPath = new List<string>();
                List<byte[]> ImgData = new List<byte[]>();


                Jdt.Maximum = DaiHeBingFilePath.Count;
                Jdt.Value = 0;
                Jdt.Visible = true;
                long NpkSize = 0;
                int CSCount = 1;
                foreach (string NpkPath in DaiHeBingFilePath)
                {
                    try
                    {
                        if (YesNpNpk(NpkPath) == false)
                        {
                            FanKuiXinXi.Append(NpkPath + "\t(此文件不是DnfNpk)\r\n");
                            continue;
                        }

                        var NpkFs = File.OpenRead(NpkPath);
                        BinaryReader NpkBr = new BinaryReader(NpkFs);

                        NpkFs.Position = 16;//跳过npk头
                        var ImgCount = NpkBr.ReadUInt32();//获取img数量

                        for (int i = 0; i < ImgCount; i++)
                        {
                            var Offset = NpkBr.ReadUInt32();//获取偏移
                            var Size = NpkBr.ReadUInt32();//获取大小
                            string ImgName = ImgName解密(NpkBr.ReadBytes(256));
                            if (ImgName.Remove(7) != "sprite/")
                                continue;

                            var Pos = NpkFs.Position;
                            NpkFs.Position = Offset;
                            ImgPath.Add(ImgName);
                            ImgData.Add(NpkBr.ReadBytes((int)Size));
                            NpkFs.Position = Pos;


                            NpkSize += Size;
                            if (NpkSize >= MaxSize)
                            {
                                写出(XieChuFilePath.Insert(XieChuFilePath.LastIndexOf("\\") + 1, "(" + CSCount + ")"),new KeyValuePair<List<string>, List<byte[]>> ( ImgPath , ImgData ));

                                CSCount++;
                                NpkSize = 0;
                                ImgPath.Clear();
                                ImgData.Clear();
                            }
                        }
                        NpkBr.Dispose();
                        Jdt.Value++;

                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"出现错误，错误文件路径为：{NpkPath}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }

                if (ImgPath.Count>0)
                {
                    写出(XieChuFilePath.Insert(XieChuFilePath.LastIndexOf("\\") + 1, "(" + CSCount + ")"), new KeyValuePair<List<string>, List<byte[]>>(ImgPath, ImgData));
                }
                
                Jdt.Visible = false;
            }
            else
            {
                if (DaiHeBingFilePath.Count <= 0)
                    return;

                List<string> ImgPath = new List<string>();
                List<byte[]> ImgData = new List<byte[]>();

                Jdt.Maximum = DaiHeBingFilePath.Count;
                Jdt.Value = 0;
                Jdt.Visible = true;
                foreach (string NpkPath in DaiHeBingFilePath)
                {
                    try
                    {
                        if (YesNpNpk(NpkPath) == false)
                        {
                            FanKuiXinXi.Append(NpkPath + "\t(此文件不是DnfNpk)\r\n");
                            continue;
                        }

                        var NpkFs = File.OpenRead(NpkPath);
                        BinaryReader NpkBr = new BinaryReader(NpkFs);

                        NpkFs.Position = 16;//跳过npk头
                        var ImgCount = NpkBr.ReadUInt32();//获取img数量

                        for (int i = 0; i < ImgCount; i++)
                        {
                            var Offset = NpkBr.ReadUInt32();//获取偏移
                            var Size = NpkBr.ReadUInt32();//获取大小
                            string ImgName = ImgName解密(NpkBr.ReadBytes(256));
                            var Pos = NpkFs.Position;

                            NpkFs.Position = Offset;
                            ImgPath.Add(ImgName);
                            ImgData.Add(NpkBr.ReadBytes((int)Size));
                            NpkFs.Position = Pos;
                        }
                        NpkBr.Dispose();
                        Jdt.Value++;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"在读入NPK时出现错误，错误文件路径为：{NpkPath}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                if (ImgPath.Count <= 0)
                    return;


                int XCImgCount = ImgPath.Count;
                Jdt.Maximum = XCImgCount;
                Jdt.Value = 0;

                FileStream stream = File.Create(XieChuFilePath); //创建文件并覆盖
                MemoryStream checksumTemp = new MemoryStream();

                checksumTemp.Write(_head, 0, 16);//写入文件头
                checksumTemp.Write(BitConverter.GetBytes(XCImgCount), 0, 4);//写入img数量

                int offset = 20 + (8 + 256) * XCImgCount + 32;//得到初始偏移
                for (int i = 0; i < XCImgCount; i++)
                {
                    int ImgDataDaXiao = ImgData[i].Length;
                    checksumTemp.Write(BitConverter.GetBytes(offset), 0, 4); //写偏移
                    checksumTemp.Write(BitConverter.GetBytes(ImgDataDaXiao), 0, 4); //写img大小
                    checksumTemp.Write(ImgName加密(ImgPath[i]), 0, 256);//写入加密后的img名称

                    offset += ImgDataDaXiao;//再次计算下一个偏移
                }

                byte[] Checksum = Npk计算校验(checksumTemp.ToArray());
                checksumTemp.WriteTo(stream);
                checksumTemp.Dispose();

                stream.Write(Checksum, 0, Checksum.Length);
                foreach (var item in ImgData)
                {
                    stream.Write(item, 0, item.Length); //把img数据逐一写入npk
                    Jdt.Value++;
                }
                stream.Dispose();
                Jdt.Visible = false;
            }
        }



        public string ImgName解密(byte[] imgname)
        {

            for (int i = 0; i < imgname.Length; i++)
            {
                imgname[i] ^= _key[i]; 
            }

            string LinShiImgPath = Encoding.Default.GetString(imgname);
            int Pos = LinShiImgPath.IndexOf("\0");
            if (Pos!=-1)
                LinShiImgPath = LinShiImgPath.Remove(Pos);

            return LinShiImgPath;
        }

        public byte[] ImgName加密(string imgname)
        {
            byte[] nameBytes = new byte[256];
            byte[] bytes = Encoding.Default.GetBytes(imgname);
            Buffer.BlockCopy(bytes, 0, nameBytes, 0, bytes.Length);

            for (var i = 0; i < nameBytes.Length; i++)
            {
                nameBytes[i] ^= _key[i]; 
            }

            return nameBytes;
        }

        public byte[] Npk计算校验(byte[] bytes)
        {
            int len = bytes.Length / 17 * 17;
            byte[] temp = new byte[len];
            Buffer.BlockCopy(bytes, 0, temp, 0, len);
            SHA256 sHA256= SHA256.Create();
            try
            {
                return  sHA256.ComputeHash(temp); //计算sha256
                 
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sHA256.Dispose();
            }

        }


        

    }
}
