using System;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;//正则表达式


/* 异常处理
 * 
 * **/

namespace PVFSimple.项目Dnf
{

    public class DnfHttp
    {

        //---------------------------pvfhttp服务-----------------------------------
        //public string FileTou = "http://127.0.0.1:27000/file?name=";
        //public string ListPath = "http://127.0.0.1:27000/list?path=";

        //public string FileTou = Form1.MysqlYz.ChaXun(4);
        //public string ListPath = Form1.MysqlYz.ChaXun(5);
        public string FileTou = "http://127.0.0.1:27000/file?name=";
        public string ListPath = "http://127.0.0.1:27000/list?path=";

        public Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// 读取指定路径的所有文本；失败时返回空文本
        /// </summary>
        /// <param name="Filepath">文件路径例如：worldmap/worldmap.lst</param>
        /// <returns></returns>
        public string FileReadAllText(string Filepath)
        {
            string alltext = "";
            try
            {
                using WebResponse web = WebRequest.Create(FileTou + Filepath).GetResponse();
                using Stream stream = web.GetResponseStream();
                using StreamReader reader = new StreamReader(stream, encoding);
                alltext = reader.ReadToEnd();

            }
            catch (Exception)
            {
                return alltext;
            }
            return alltext;
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="Filepath">文件路径</param>
        /// <returns>返回true为存在；返回false为不存在</returns>
        public bool FileExists(string Filepath)
        {
            try
            {
                using WebResponse web = WebRequest.Create(FileTou + Filepath).GetResponse();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 得到指定目录下的所有文件路径，可以筛选；失败时返回空集合
        /// </summary>
        /// <param name="XunZhaopath">寻找路径 例如：worldmap/ (目录结尾必须加斜杠)</param>
        /// <param name="HouZhui">筛选的后缀名 “*”所有  “equ”只取equ后缀的文件路径</param>
        /// <param name="PaiXu">是否排序</param>
        /// <returns>找到的所有路径集合</returns>
        public List<string> DirectoryEnumerateFiles(string XunZhaopath, string HouZhui, bool PaiXu)
        {
            List<string> allpath = new List<string>();
            try
            {
                using WebResponse web = WebRequest.Create(ListPath + XunZhaopath).GetResponse();
                using Stream stream = web.GetResponseStream();
                switch (HouZhui)
                {
                    case "*":
                        using (StreamReader reader = new StreamReader(stream, encoding))
                        {
                            do
                            {
                                allpath.Add(reader.ReadLine());
                            } while (reader.EndOfStream == false);

                        }
                        break;
                    default:

                        using (StreamReader reader = new StreamReader(stream, encoding))
                        {
                            string YiHangText;
                            do
                            {
                                YiHangText = reader.ReadLine();
                                if (YiHangText.Substring(YiHangText.LastIndexOf(".") + 1) == HouZhui)
                                {
                                    allpath.Add(YiHangText);
                                }
                            } while (reader.EndOfStream == false);
                        }
                        break;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("在线提取文件列表时发生错误；目录名为：" + XunZhaopath, "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            if (PaiXu)
            {
                allpath.Sort();
            }
            return allpath;
        }


        public bool AddFile(string FilePath, string FileText)
        {
            //try
            //{
            //    IAsyncResult result = 

            //    using (WebResponse web = WebRequest.Create(FileTou + FilePath).EndGetResponse())
            //    {
            //        using (Stream stream = web.GetResponseStream())
            //        {
            //            using (StreamWriter reader = new StreamWriter(stream, encoding))
            //            {
            //                web.
            //                reader.Write(FileText);
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    return false;
            //}

            return true;
        }



        //---------------------------pvfhttp服务-----------------------------------


        /// <summary>
        /// 验证文本是否为整数
        /// </summary>
        /// <param name="Text">被验证的文本</param>
        /// <returns></returns>
        public bool YesNoInt(string Text)
        {
            if (Regex.IsMatch(Text, @"^[\-0-9\.]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 返回父路径 以“/”为标准
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string DirectoryGetParent(string path)
        {
            if (path.Contains("\\"))
            {
                path = path.Replace("\\", "/");
            }

            int Pos = path.LastIndexOf("/");
            if (Pos != -1)
            {
                path = path.Remove(Pos);
            }
            else
            {
                path = "";
            }


            return path;
        }


        /// <summary>
        /// 返回扩展名以“.”为标准;例如返回“.exe”
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string PathGetExtension(string path)
        {
            if (path.Contains("."))
            {
                path = path.Substring(path.LastIndexOf("."));
            }
            return path;
        }

        /// <summary>
        /// 由字符串检索进行分割 检索分成3个list列表，第一个为编号、第二个为路径、第三个为转换成小写的路径
        /// </summary>
        /// <param name="LstAllText">lst的文本</param>
        /// <returns></returns>
        public List<List<string>> LstChongPaiLie(string LstAllText)
        {
            List<List<string>> List = new List<List<string>>
            {
                new List<string>(),
                new List<string>(),
                new List<string>()
            };

            char[] Fg = { '\r', '\n' };//设定依靠拆分的字符
            List<string> FgEnd = new List<string>(LstAllText.Split(Fg, StringSplitOptions.RemoveEmptyEntries));//开始拆分字符串

            while (FgEnd[0].Contains("\t") == false)
                FgEnd.RemoveAt(0);

            foreach (string item in FgEnd)//得到所有被拆分的字符串，按照单数为编号  双数为路径加入到集合中
            {
                string Text = item;
                int Pos = Text.IndexOf("\t");
                if (Pos == -1)
                    continue;

                string Id = Text.Remove(Pos);
                string Path = "";
                if (Pos + 1 < Text.Length)
                    Path = Text.Substring(Pos + 1);

                Path = Path.Replace("`", "");

                List[0].Add(Id);
                List[1].Add(Path);
                List[2].Add(Path.ToLower());
            }

            return List;
        }


        /// <summary>
        /// 由字符串检索进行分割 检索编号指向路径或名称只返回编号
        /// </summary>
        /// <param name="LstAllText">lst的文本</param>
        /// <returns></returns>
        public List<string> LstChongPaiLieGetId(string LstAllText)
        {
            List<string> List = new List<string>();

            List<string> FgEnd = new List<string>(LstAllText.Split(new char[] { ' ', '-', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));//开始拆分字符串

            while (YesNoInt(FgEnd[0]) == false)
                FgEnd.RemoveAt(0);

            foreach (string item in FgEnd)//得到所有被拆分的字符串，按照单数为编号  双数为路径加入到集合中
            {
                if (YesNoInt(item) == false)
                    continue;
                List.Add(item);
            }
            return List;
        }


        /// <summary>
        /// 由字符串检索进行分割 检索全路径只返回全路径的子路径
        /// </summary>
        /// <param name="LstAllText">全路径文本</param>
        /// <returns></returns>
        public List<string> LstChongPaiLieGetPath(string LstAllText)
        {
            List<string> List = new List<string>();

            char[] Fg = { '\n', '\r' };//设定依靠拆分的字符
            string[] FgEnd = LstAllText.Split(Fg, StringSplitOptions.RemoveEmptyEntries);//开始拆分字符串

            foreach (string item in FgEnd)//得到所有被拆分的字符串，按照单数为编号  双数为路径加入到集合中
            {
                List.Add(item.Substring(item.IndexOf("/") + 1));
            }
            return List;
        }



        /// <summary>
        /// 查找指定文件中的img路径
        /// </summary>
        /// <param name="path">查找的目录</param>
        /// <param name="扩展名">*.*代表所有 *.exe代表应用程序</param>
        /// <param name="去重复">false代表去重复 true代表不去重</param>
        /// <param name="npkimg_path">转成npk内的img全路径</param>
        public void 寻找img路径(string path, string 扩展名, StringBuilder npkimg_path)
        {
            Regex_new regex = new Regex_new();
            List<string> Imgalltext = new List<string>();

            List<string> Dirs = new List<string>(Directory.EnumerateFiles(path, 扩展名, SearchOption.AllDirectories));

            foreach (string item in Dirs)
            {
                switch (PathGetExtension(item))
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
                    string ImgPath = Img.ToLower();
                    if (ImgPath.Contains(".img") == false)
                        continue;

                    int Pos = ImgPath.IndexOf("%d%d.img");
                    if (Pos != -1)
                    {
                        string F_Path = regex.替换("sprite/" + ImgPath.Remove(Pos).Replace("\\", "/"), "[/]{2,}|^[/]+", "");
                        for (int i = 1; i < 10; i++)
                        {
                            if (Imgalltext.Contains($"{F_Path}0{i}.img") == false)
                                Imgalltext.Add($"{F_Path}0{i}.img");
                        }
                        continue;
                    }

                    ImgPath = regex.替换("sprite/" + ImgPath.Replace("\\", "/"), "[/]{2,}|^[/]+", "");
                    if (Imgalltext.Contains(ImgPath) == false)
                        Imgalltext.Add(ImgPath);
                }
            }

            foreach (string item in Imgalltext)
            {
                npkimg_path.Append(item + "\r\n");
            }

            npkimg_path.Insert(0, "寻找img路径遍历的文件总数为：" + Imgalltext.Count.ToString() + "个\r\n\r\n", 1);
        }



        /// <summary>
        /// 在查找中自动转为正常全路径
        /// </summary>
        /// <param name="path">提供的全路径如：map/../a.ani</param>
        /// <param name="是否为小写">true转为小写 false不转小写</param>
        /// <returns></returns>
        public string 识别点点杠(string path, bool 是否为小写)
        {
            if (path.Contains(@"...") == false)
            {
                path = path.Replace("\\", "/");
                if (path.Contains(@"../"))
                {
                    Regex_new regex = new Regex_new();

                    regex.创建(path, @"(.+?)/\.\./(\.\./)*(.+\.[a-z]+)", 1);
                    string f_path = regex.取子匹配文本(1, 1);
                    string z_path = regex.取子匹配文本(1, 3);
                    regex.创建(path, @"\.\./", 0);
                    for (int i = 0; i < regex.取匹配数量(); i++)
                    {
                        f_path = DirectoryGetParent(f_path);
                    }

                    if (f_path == "")
                    {
                        if (是否为小写)
                        {
                            string fz_path = z_path;
                            return fz_path.ToLower();
                        }
                        else
                        {
                            return z_path;
                        }
                    }
                    else
                    {
                        if (是否为小写)
                        {
                            string fz_path = f_path + "/" + z_path;
                            return fz_path.ToLower();
                        }
                        else
                        {
                            return f_path + "/" + z_path;
                        }
                    }

                }
                else
                {

                    if (是否为小写)
                    {
                        return path.ToLower();
                    }
                    else
                    {
                        return path;
                    }
                }
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// 提供ani全路径，返回找到的ani、als全路径
        /// </summary>
        /// <param name="ani_path">提供ani全路径，找到返回ani</param>
        /// <param name="als_path">找到返回als</param>
        /// <param name="biglittle">转为大小写；0：不转 1：大写 2：小写</param>
        /// <param name="repetition">true：加入重复  false：不加入重复</param>
        public void 寻找als(List<string> ani_path, List<string> als_path, int biglittle, bool repetition)
        {
            if (ani_path.Count > 0)//判断所提供的ani全路径集合是不是空的
            {
                Regex_new regex = new Regex_new();
                List<string> temporary_anipath = new List<string>(ani_path);//接收传过来的ani全路径到临时ani
                List<string> temporary_alspath = new List<string>();//创建临时als集合

                int AniCount;
                int AlsCount;

                do
                {
                    AniCount = ani_path.Count;
                    AlsCount = als_path.Count;

                    foreach (var item in temporary_anipath)//枚举所有临时ani全路径，查找出来的als加入到临时als集合
                    {
                        string alspath = item + ".als";//ani全路径加上.als 赋值给字符串变量
                        if (FileExists(alspath))//如果找到了als
                        {
                            switch (biglittle)
                            {
                                case 0:
                                    switch (repetition)
                                    {
                                        case true:
                                            temporary_alspath.Add(alspath);
                                            break;
                                        case false:
                                            if (temporary_alspath.Contains(alspath) == false)//判断不在集合中就加入
                                            {
                                                temporary_alspath.Add(alspath);
                                            }
                                            break;
                                    }
                                    break;
                                case 1:
                                    switch (repetition)
                                    {
                                        case true:
                                            temporary_alspath.Add(alspath.ToUpper());
                                            break;
                                        case false:
                                            string bigtxt = alspath.ToUpper();//转为大写 赋值给字符串变量
                                            if (temporary_alspath.Contains(bigtxt) == false)//判断不在集合中就加入
                                            {
                                                temporary_alspath.Add(bigtxt);
                                            }
                                            break;
                                    }
                                    break;
                                case 2:
                                    switch (repetition)
                                    {
                                        case true:
                                            temporary_alspath.Add(alspath.ToLower());
                                            break;
                                        case false:
                                            string littletxt = alspath.ToLower();//转为小写 赋值给字符串变量
                                            if (temporary_alspath.Contains(littletxt) == false)//判断不在集合中就加入
                                            {
                                                temporary_alspath.Add(littletxt);
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                    }

                    temporary_anipath.Clear();//临时ani清空掉

                    //把ani+als扩展名的文件路径加入到参数als中
                    temporary_alspath.ForEach((AlsPath) =>
                    {
                        if (!als_path.Contains(AlsPath))
                            als_path.Add(AlsPath);
                    });


                    //寻找als当中的ani全路径 加入到临时ani集合
                    foreach (var item in temporary_alspath)
                    {
                        string alspath = item;//als全路径
                        string alsalltxt = FileReadAllText(alspath);//读入als所有文本
                        regex.创建(alsalltxt, @"\[use animation\][\r\n]+\t?`(.+\.ani)`", 1);//正则捕获ani路径
                        foreach (Match itemani in regex.正则返回集合())//枚举出所有捕获
                        {
                            string anipath = 识别点点杠(DirectoryGetParent(alspath) + "/" + itemani.Groups[1].Value, true);//取出子匹配去除../
                            if (temporary_anipath.Contains(anipath) == false)//判断不在临时ani集合中就加入
                            {
                                temporary_anipath.Add(anipath);
                            }
                        }
                    }

                    //寻找出来的临时ani加入到参数ani当中
                    foreach (var anipath in temporary_anipath)
                        if (!ani_path.Contains(anipath))
                            ani_path.Add(anipath);

                    temporary_alspath.Clear();//清楚掉临时als集合

                } while (AniCount != ani_path.Count || AlsCount != als_path.Count);//如果最开始取的数量不等于寻找后的数量那么继续循环


            }
        }

        /// <summary>
        /// 为了防止遗漏ai文件，通过返回父路径来枚举出所在目录下的所有ai文件的全路径
        /// </summary>
        /// <param name="ai_path"></param>
        public void 寻找ai(List<string> ai_path)
        {
            //如果提供的ai全路径集合不为空
            if (ai_path.Count > 0)
            {
                //新增一个临时ai目录集合
                List<string> aimulu = new List<string>();

                //用户提供的ai全路径返回两次父路径，加入临时ai目录
                foreach (var item in ai_path)
                {
                    string aipath = DirectoryGetParent(DirectoryGetParent(item)).ToLower();
                    if (aimulu.Contains(aipath) == false)
                    {
                        aimulu.Add(aipath);
                    }
                }

                //通过不重复的ai目录，枚举出目录下面所有的ai全路径
                foreach (var item in aimulu)
                {
                    List<string> dirs = new List<string>(DirectoryEnumerateFiles(item, "ai", false));
                    ai_path.AddRange(dirs);
                }
            }
        }

        /// <summary>
        /// 根据编号查找lst以及全路径
        /// </summary>
        /// <param name="pathex">例如“dungeon/”</param>
        /// <param name="ListLst">lstList集合</param>
        /// <param name="id">被寻找的编号集合</param>
        /// <param name="lst">返回加入的lst集合</param>
        /// <param name="path">返回加入的全路径集合</param>
        public void Id_lst_编号查找(string pathex, List<List<string>> ListLst, List<string> id, List<string> lst, List<string> path)
        {

            if (id.Count <= 0 || ListLst.Count <= 0) return;

            pathex = pathex.ToLower();
            foreach (string item in id)
            {
                int pos = ListLst[0].IndexOf(item);
                if (pos != -1)
                {
                    string LinShiPath = ListLst[2][pos];
                    string LstText = ListLst[0][pos] + "\t`" + ListLst[1][pos] + "`";
                    if (lst != null)
                    {
                        if (lst.Contains(LstText) == false)
                        {
                            lst.Add(LstText);
                        }
                    }

                    if (path != null)
                    {
                        if (path.Contains(pathex + LinShiPath) == false)
                        {
                            path.Add(pathex + LinShiPath);
                        }
                    }
                }
            }

        }


        /// <summary>
        /// 根据路径查找编号以及lst
        /// </summary>
        /// <param name="pathex">例如“dungeon/”</param>
        /// <param name="ListLst">lstList集合</param>
        /// <param name="id">返回加入的编号集合</param>
        /// <param name="lst">返回加入的lst集合</param>
        /// <param name="path">被寻找的路径集合</param>
        public void Id_lst_路径查找(string pathex, List<List<string>> ListLst, List<string> id, List<string> lst, List<string> path)
        {
            if (path.Count <= 0 || ListLst.Count <= 0) return;

            List<string> new_path = new List<string>();

            foreach (string item in path)
            {
                string XunZhao_Path = item.Replace(pathex, "").ToLower();
                int pos = ListLst[2].IndexOf(XunZhao_Path);
                if (pos != -1)
                {
                    string Id_List1 = ListLst[0][pos];
                    string YPath_List2 = ListLst[1][pos];
                    string TolPath_List3 = ListLst[2][pos];
                    string Id_Lst = Id_List1 + "\t`" + YPath_List2 + "`";

                    if (id != null)
                    {
                        if (id.Contains(Id_List1) == false)
                        {
                            id.Add(Id_List1);
                        }
                    }

                    if (lst != null)
                    {
                        if (lst.Contains(Id_Lst) == false)
                        {
                            lst.Add(Id_Lst);
                        }
                    }

                    if (path != null)
                    {
                        if (new_path.Contains(pathex + TolPath_List3) == false)
                        {
                            new_path.Add(pathex + TolPath_List3);
                        }
                    }
                }
            }

            if (new_path.Count > 0)
            {

                path.Clear();
                path.AddRange(new_path);
            }

        }



        /// <summary>
        /// 寻找文件中`内容`的文本，以扩展名方式加入对应集合
        /// </summary>
        /// <param name="path">全路径文本文件</param>
        /// <param name="tbl">tbl扩展名集合</param>
        /// <param name="act">act扩展名集合</param>
        /// <param name="atk">atk扩展名集合</param>
        /// <param name="ani">ani扩展名集合</param>
        /// <param name="ptl">ptl扩展名集合</param>
        /// <param name="til">til扩展名集合</param>
        /// <param name="key">key扩展名集合</param>
        /// <param name="ai">ai扩展名集合</param>
        /// <param name="equ">怪物equ扩展名集合</param>
        public void 寻找全路径(List<string> path, List<string> tbl, List<string> act, List<string> atk, List<string> ani, List<string> ptl, List<string> til, List<string> key, List<string> ai, List<string> equ)
        {
            if (path.Count > 0)
            {
                List<string> 临时act = new List<string>();

                string exname = PathGetExtension(path[0]).ToLower();

                foreach (var item in path)
                {
                    if (FileExists(item))
                    {
                        string alltext = FileReadAllText(item);
                        Regex_new regex = new Regex_new();
                        regex.创建(alltext, @"(`.+\.tbl`)|(`.+\.act`)|(`.+\.h`)|(`.+\.atk`)|(`.+\.ani`)|(`.+\.ptl`)|(`.+\.til`)|(`.+\.key`)|(`.+\.ai`)|(`.+\.equ`)", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            string f_path = DirectoryGetParent(item).ToLower();

                            List<string> regex_pp = new List<string>();
                            regex.匹配加入集合(regex_pp, false, 2);

                            foreach (var item2 in regex_pp)
                            {
                                string pp_path = item2.Replace("\\", "/");
                                if (pp_path.Contains(".../") == false)
                                {
                                    pp_path = pp_path.Replace("`", "").ToLower();
                                    string kzm = PathGetExtension(pp_path);
                                    string lins = "";

                                    switch (kzm)
                                    {
                                        case ".key":
                                            if (exname == ".aic" && key != null)
                                            {
                                                if (key.Contains(f_path + "/key/" + pp_path) == false)
                                                {
                                                    key.Add(f_path + "/key/" + pp_path);
                                                }
                                            }
                                            break;
                                        case ".ai":
                                            if (ai != null)
                                            {
                                                switch (exname)
                                                {
                                                    case ".aic":
                                                        if (pp_path.Replace("\\", "/").ToLower().Contains("aicharacter/"))
                                                        {
                                                            if (ai.Contains(pp_path) == false)
                                                            {
                                                                ai.Add(pp_path);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ai.Contains(f_path + "/ai/" + pp_path) == false)
                                                            {
                                                                ai.Add(f_path + "/ai/" + pp_path);
                                                            }
                                                        }
                                                        break;
                                                    case ".mob":
                                                    case ".ai":
                                                        lins = 识别点点杠(f_path + "/" + pp_path, false);
                                                        if (ai.Contains(lins) == false)
                                                        {
                                                            ai.Add(lins);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case ".tbl":
                                            if (tbl != null)
                                            {
                                                switch (exname)
                                                {
                                                    case ".dgn":
                                                        if (tbl.Contains(pp_path) == false)
                                                        {
                                                            tbl.Add(pp_path);
                                                        }
                                                        break;
                                                    case ".mob":
                                                        if (tbl.Contains("monster/" + pp_path) == false)
                                                        {
                                                            tbl.Add("monster/" + pp_path);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case ".act":
                                        case ".h":
                                            if (act != null)
                                            {
                                                switch (exname)
                                                {
                                                    case ".act":
                                                    case ".h":
                                                        lins = 识别点点杠(f_path + "/" + pp_path, false);
                                                        if (act.Contains(lins) == false)
                                                        {
                                                            临时act.Add(lins);
                                                        }
                                                        break;
                                                    default:
                                                        lins = 识别点点杠(f_path + "/" + pp_path, false);
                                                        if (act.Contains(lins) == false)
                                                        {
                                                            act.Add(lins);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case ".atk":
                                            if (atk != null)
                                            {
                                                lins = 识别点点杠(f_path + "/" + pp_path, false);
                                                if (atk.Contains(lins) == false)
                                                {
                                                    atk.Add(lins);
                                                }
                                            }
                                            break;
                                        case ".ani":
                                            if (ani != null)
                                            {
                                                lins = 识别点点杠(f_path + "/" + pp_path, false);
                                                if (ani.Contains(lins) == false)
                                                {
                                                    ani.Add(lins);
                                                }
                                            }
                                            break;
                                        case ".ptl":
                                            if (ptl != null)
                                            {
                                                switch (exname)
                                                {
                                                    case ".key":
                                                        lins = 识别点点杠(DirectoryGetParent(f_path) + "/" + pp_path, false);
                                                        if (ptl.Contains(lins) == false)
                                                        {
                                                            ptl.Add(lins);
                                                        }
                                                        break;
                                                    default:
                                                        lins = 识别点点杠(f_path + "/" + pp_path, false);
                                                        if (ptl.Contains(lins) == false)
                                                        {
                                                            ptl.Add(lins);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case ".til":
                                            if (til != null)
                                            {
                                                lins = 识别点点杠(f_path + "/" + pp_path, false);
                                                if (til.Contains(lins) == false)
                                                {
                                                    til.Add(lins);
                                                }
                                            }
                                            break;
                                        case ".equ":
                                            if (equ != null)
                                            {
                                                if (exname == ".mob" && equ.Contains("equipment/" + pp_path) == false)
                                                {
                                                    equ.Add("equipment/" + pp_path);
                                                }
                                            }
                                            break;
                                    }
                                }
                            }

                            if (ani != null)
                            {
                                if (exname == ".act" || exname == ".h")
                                {
                                    regex.创建(alltext, @"\[ANI FILE\]\r\n`(.+\.ani)`", 1);
                                    if (regex.取匹配数量() > 0)
                                    {
                                        List<string> ui_path = new List<string>();
                                        regex.子匹配加入集合(ui_path, 1, false, 2);
                                        foreach (var item_ui in ui_path)
                                        {
                                            string uitxt = item_ui.Replace("\\", "/");

                                            if (ani.Contains(uitxt) == false)
                                            {
                                                ani.Add(uitxt);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (临时act.Count > 0)
                {
                    foreach (var item in 临时act)
                    {

                        if (act.Contains(item) == false)
                        {
                            act.Add(item);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 寻找文件中 map、obj、mob、apc的编号 加入对应集合
        /// </summary>
        /// <param name="path">所寻找的文件全路径</param>
        /// <param name="map">返回加入集合map编号</param>
        /// <param name="obj">返回加入集合obj编号</param>
        /// <param name="mob">返回加入集合mob编号</param>
        /// <param name="apc">返回加入集合apc编号</param>
        public void 寻找编号(List<string> path, List<string> map, List<string> obj, List<string> mob, List<string> apc)
        {
            if (path.Count > 0)//判断所提供的集合是不是空的
            {
                Regex_new regex = new Regex_new();
                string exname = PathGetExtension(path[0]).ToLower();//取出扩展名并且转为小写；方便判断查找编号

                foreach (var item in path)//循环只读所有全路径
                {
                    if (FileExists(item))//判断所提供的全路径文件是否存在
                    {
                        string alltext = FileReadAllText(item);//读入所有内容

                        switch (exname)
                        {
                            case ".dgn":
                                if (obj != null)//提供参数 确认加入obj编号那么继续运行
                                {
                                    regex.创建(alltext, @"\[pathgate object\]\r\n(.+\t)\r\n", 1);//正则捕获所有obj编号
                                    if (regex.取匹配数量() > 0)//确认找到了 那么继续运行
                                    {
                                        regex.创建(regex.取子匹配文本(1, 1), @"[\x20-\x7f]+", 1);//只取出所有obj编号
                                        foreach (Match dgn_obj in regex.正则返回集合())//循环获取obj编号
                                        {
                                            string obj_id = dgn_obj.Value;//接收obj编号
                                            if (obj_id != "-1" && obj.Contains(obj_id) == false)//判断不等于-1 且 不在参数obj集合中 就加入集合
                                            {
                                                obj.Add(obj_id);
                                            }
                                        }
                                    }

                                }

                                if (map != null)//判断 副本是否搜索map编号 如果是那么继续
                                {
                                    regex.创建(alltext, @"\[seal door map index\]\r\n([\x20-\x7f]+)\t", 1);//捕获深渊map房间
                                    if (regex.取匹配数量() > 0)
                                    {
                                        string hell_map_id = regex.取子匹配文本(1, 1);//取出深渊map房间
                                        if (hell_map_id != "-1" && map.Contains(hell_map_id) == false)//判断不等于-1 且 不在参数map集合中 就加入集合
                                        {
                                            map.Add(hell_map_id);
                                        }
                                    }
                                }
                                break;
                            case ".map":
                                if (obj != null)//判断参数是否允许提取特效编号
                                {
                                    regex.创建(alltext, @"\[passive object\]\r\n(.+\t)\r\n\[/passive object\]", 1);//捕获特效所有文本
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.创建(regex.取子匹配文本(1, 1), @"([0-9]+)\t([\x20-\x7f]+\t){3}", 1);//捕获单个特效编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            foreach (Match obj_id_item in regex.正则返回集合())//循环
                                            {
                                                string obj_id = obj_id_item.Groups[1].Value;//取出子匹配文本(特效编号)
                                                if (obj_id != "-1" && obj.Contains(obj_id) == false)//判断不等于-1 且 不在参数obj集合 就加入
                                                {
                                                    obj.Add(obj_id);
                                                }
                                            }
                                        }
                                    }

                                    regex.创建(alltext, @"\[special passive object\](\r\n[\s\S]*?)\r\n\[/special passive object\]", 1);//捕获<特殊>特效
                                    if (regex.取匹配数量() > 0)//检查是否匹配到特殊特效
                                    {
                                        string ts_obj = regex.取子匹配文本(1, 1);
                                        regex.创建(ts_obj, @"[\t\r\n]([0-9]+)\t([\x20-\x7f]+\t){3}0", 1);//捕获特殊特效的单纯特效编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.子匹配加入集合(obj, 1, false, 0);
                                        }

                                        regex.创建(ts_obj, @"([0-9]+)\t([\x20-\x7f]+\t){3}([1-9][0-9]*)\t`\[trap\]`", 1);//捕获特殊特效的破坏召唤特效编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.子匹配加入集合(obj, 1, false, 0);
                                        }

                                        regex.创建(ts_obj, @"`\[trap\]`\r\n([0-9]+)\t([\x20-\x7f]+\t){3}-1\t", 1);//捕获特殊特效破坏召唤后的特效编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.子匹配加入集合(obj, 1, false, 0);
                                        }

                                        regex.创建(ts_obj, @"([0-9]+)\t([\x20-\x7f]+\t){3}([1-9][0-9]*)\t`\[item\]`", 1);//捕获特殊特效的掉落特效
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.子匹配加入集合(obj, 1, false, 0);
                                        }

                                        regex.创建(ts_obj, @"([0-9]+)\t([\x20-\x7f]+\t){3}([1-9][0-9]*)\t`\[monster\]`", 1);//捕获特殊特效的召唤怪物特效
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.子匹配加入集合(obj, 1, false, 0);
                                        }

                                        regex.创建(ts_obj, @"([0-9]+)\t([\x20-\x7f]+\t){3}([1-9][0-9]*)\t`\[quest\]`", 1);//捕获特殊特效的任务特效
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.子匹配加入集合(obj, 1, false, 0);
                                        }
                                    }

                                }

                                if (mob != null)//判断参数是否允许提取怪物编号
                                {
                                    regex.创建(alltext, @"\[monster\](\r\n[\s\S]*?)\r\n\[/monster\]", 1);//捕获怪物编号中间的内容
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.创建(regex.取子匹配文本(1, 1), @"[\r\n\t]([0-9]+)\t([\x20-\x7f]+\t){7}`\[[a-z]+\]`\r\n`\[[a-z]+\]`", 1);//捕获怪物编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.子匹配加入集合(mob, 1, false, 0);
                                        }
                                    }

                                    regex.创建(alltext, @"`\[monster\]`\r\n([0-9]+)\t([\x20-\x7f]+\t){3}-1\t", 1);//最后额外匹配特殊特效召唤的怪物编号
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(mob, 1, false, 0);
                                    }
                                }

                                if (apc != null) //判断参数是否允许apc怪物编号
                                {
                                    regex.创建(alltext, @"\[ai character\](\r\n[\s\S]*?)\r\n\[/ai character\]", 1);//捕获怪物apc中间的内容
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.创建(regex.取子匹配文本(1, 1), @"[\r\n\t]([0-9]+)\t([\x20-\x7f]+\t){3}`\[[a-z]+\]`\r\n`\[[a-z]+\]`", 1);//捕获apc编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.子匹配加入集合(apc, 1, false, 0);
                                        }
                                    }
                                }
                                break;
                            case ".act":
                            case ".h":
                                if (obj != null)//判断参数是否允许提取特效编号
                                {
                                    regex.创建(alltext, @"\[CREATE PASSIVEOBJECT\][\r\n]+\[INDEX\][\r\n]+([0-9]+)\t", 0);//这里默认匹配大小写
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(obj, 1, false, 0);
                                    }

                                    regex.创建(alltext, @"\[CREATE PASSIVEOBJECT\][\r\n]+\[INDEX\][\r\n]+\[RANDOM SELECT\][\r\n]+(.+)\r\n\[/RANDOM SELECT\]", 0);//匹配随机的特效；默认大写匹配模式
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.创建(regex.返回所有子匹配文本(1), @"[0-9]+", 0);//捕获单个编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.匹配加入集合(obj, false, 0);
                                        }
                                    }
                                }

                                if (mob != null)//判断参数是否允许提取怪物编号
                                {
                                    regex.创建(alltext, @"\[SUMMON MONSTER\][\r\n]+\[INDEX\][\r\n]+([0-9]+)\t", 0);//这里默认匹配大小写
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(mob, 1, false, 0);
                                    }

                                    regex.创建(alltext, @"\[SUMMON MONSTER\][\r\n]+\[INDEX\][\r\n]+\[RANDOM SELECT\][\r\n]+(.+)\r\n\[/RANDOM SELECT\]", 0);//匹配随机的怪物；默认大写匹配模式
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.创建(regex.返回所有子匹配文本(1), @"[0-9]+", 0);//捕获单个编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.匹配加入集合(mob, false, 0);
                                        }
                                    }
                                }

                                if (apc != null)//判断参数是否允许提取apc编号
                                {
                                    regex.创建(alltext, @"\[SUMMON APC\][\r\n]+\[INDEX\][\r\n]+([0-9]+)\t", 0);//这里默认匹配大小写
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(apc, 1, false, 0);
                                    }

                                    regex.创建(alltext, @"\[SUMMON APC\][\r\n]+\[INDEX\][\r\n]+\[RANDOM SELECT\][\r\n]+(.+)\r\n\[/RANDOM SELECT\]", 0);//匹配随机的人偶；默认大写匹配模式
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.创建(regex.返回所有子匹配文本(1), @"[0-9]+", 0);//捕获单个编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            regex.匹配加入集合(apc, false, 0);
                                        }
                                    }
                                }
                                break;
                            case ".obj":
                                if (obj != null)//判断参数是否允许提取特效编号
                                {
                                    regex.创建(alltext, @"`\[add object index\]`[\r\n]+[0-9]+\t(.+)\t", 0);//捕获销毁创建的特效
                                    if (regex.取匹配数量() > 0)
                                    {
                                        regex.创建(regex.返回所有子匹配文本(1), @"[0-9]+", 0);//这里省去多个判断直接获取所有编号
                                        if (regex.取匹配数量() > 0)
                                        {
                                            regex.匹配加入集合(obj, false, 0);
                                        }
                                    }

                                    regex.创建(alltext, @"`\[add object index on attack\]`[\r\n]+[0-9]+\t(.+)\t", 0);//捕获攻击时创建的特效
                                    if (regex.取匹配数量() > 0)
                                    {
                                        regex.创建(regex.返回所有子匹配文本(1), @"[0-9]+", 0);//这里省去多个判断直接获取所有编号
                                        if (regex.取匹配数量() > 0)
                                        {
                                            regex.匹配加入集合(obj, false, 0);
                                        }
                                    }
                                }
                                break;
                            case ".mob":
                                if (obj != null)//判断参数是否允许提取特效编号
                                {
                                    regex.创建(alltext, @"`\[passive object index\]`\r\n([0-9]+)\t", 0);//捕获怪物文件当中的obj编号
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(obj, 1, false, 0);
                                    }
                                }
                                break;
                            case "ptl":
                                if (obj != null)//判断参数是否允许提取特效编号
                                {
                                    //如果ptl文本中对象类型是特效
                                    if (alltext.Contains("[object type]\r\n`[passive object]`"))
                                    {
                                        regex.创建(alltext, @"\[object\](\r\n[\s\S]*?)\r\n\[/object\]", 0);//捕获ptl的特效编号
                                        if (regex.取匹配数量() > 0)//检查是否匹配到
                                        {
                                            string ptlobjtxt = regex.取子匹配文本(1, 1).ToLower();
                                            //如果ptl对象里面没有ani路径
                                            if (ptlobjtxt.Contains(".ani") == false)
                                            {
                                                regex.创建(ptlobjtxt, @"([\x20-\x7f]+)\t", 0);//捕获单个特效编号
                                                if (regex.取匹配数量() > 0)//检查是否匹配到
                                                {
                                                    regex.子匹配加入集合(obj, 1, false, 0);
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据提供的怪物编号 查找出所有关联编号(mob,obj,apc)
        /// </summary>
        /// <param name="lsttext">怪物lst文件内容</param>
        /// <param name="mob_id">怪物编号</param>
        /// <param name="mob_lst">怪物lst</param>
        /// <param name="mob_path">怪物全路径</param>
        /// <param name="obj_id">特效编号</param>
        /// <param name="apc_id">apc编号</param>
        /// <param name="act_path">act全路径</param>
        /// <param name="ptl_path">ptl全路径</param>
        public void 怪物寻找编号(List<List<string>> lsttext, List<string> mob_id, List<string> mob_lst, List<string> mob_path, List<string> obj_id, List<string> apc_id, List<string> act_path, List<string> ptl_path)
        {

            Id_lst_编号查找("monster/", lsttext, mob_id, mob_lst, mob_path);

            寻找全路径(mob_path, null, act_path, null, null, null, null, null, null, null);

            if (act_path.Count > 0)
            {
                int act_count;
                do
                {
                    act_count = act_path.Count;
                    寻找全路径(act_path, null, act_path, null, null, null, null, null, null, null);
                } while (act_count != act_path.Count);
            }

            寻找全路径(act_path, null, null, null, null, ptl_path, null, null, null, null);
            寻找全路径(mob_path, null, null, null, null, ptl_path, null, null, null, null);

            寻找编号(mob_path, null, obj_id, null, null);
            寻找编号(ptl_path, null, obj_id, null, null);
            寻找编号(act_path, null, obj_id, mob_id, apc_id);
        }


        /// <summary>
        /// 根据提供的怪物编号 查找出所有关联编号(mob,obj,apc)
        /// </summary>
        /// <param name="lsttext">特效lst文件内容</param>
        /// <param name="obj_id">特效编号</param>
        /// <param name="obj_lst">特效lst</param>
        /// <param name="obj_path">特效全路径</param>
        /// <param name="mob_id">怪物编号</param>
        /// <param name="apc_id">apc编号</param>
        /// <param name="act_path">act全路径</param>
        /// <param name="ptl_path">ptl全路径</param>
        public void 特效寻找编号(List<List<string>> lsttext, List<string> obj_id, List<string> obj_lst, List<string> obj_path, List<string> mob_id, List<string> apc_id, List<string> act_path, List<string> ptl_path)
        {

            Id_lst_编号查找("passiveobject/", lsttext, obj_id, obj_lst, obj_path);

            寻找全路径(obj_path, null, act_path, null, null, null, null, null, null, null);

            if (act_path.Count > 0)
            {
                int act_count;
                do
                {
                    act_count = act_path.Count;
                    寻找全路径(act_path, null, act_path, null, null, null, null, null, null, null);
                } while (act_count != act_path.Count);
            }


            寻找全路径(act_path, null, null, null, null, ptl_path, null, null, null, null);
            寻找全路径(obj_path, null, null, null, null, ptl_path, null, null, null, null);

            寻找编号(obj_path, null, obj_id, null, null);
            寻找编号(ptl_path, null, obj_id, null, null);
            寻找编号(act_path, null, obj_id, mob_id, apc_id);

        }


        /// <summary>
        /// 根据提供的怪物编号 查找出所有关联编号(mob,obj,apc)
        /// </summary>
        /// <param name="lsttext">人偶lst文件内容</param>
        /// <param name="apc_id">人偶编号</param>
        /// <param name="apc_lst">人偶lst</param>
        /// <param name="mob_id">怪物编号</param>
        /// <param name="obj_id">特效编号</param>
        /// <param name="act_path">act全路径</param>
        /// <param name="ptl_path">ptl全路径</param>
        /// <param name="key_path">key全路径</param>
        public void 人偶寻找编号(List<List<string>> lsttext, List<string> apc_id, List<string> apc_lst, List<string> apc_path, List<string> mob_id, List<string> obj_id, List<string> act_path, List<string> ptl_path, List<string> key_path)
        {

            Id_lst_编号查找("aicharacter/", lsttext, apc_id, apc_lst, apc_path);

            寻找全路径(apc_path, null, act_path, null, null, null, null, key_path, null, null);

            if (act_path.Count > 0)
            {
                int act_count;
                do
                {
                    act_count = act_path.Count;
                    寻找全路径(act_path, null, act_path, null, null, null, null, null, null, null);
                } while (act_count != act_path.Count);
            }


            寻找全路径(act_path, null, null, null, null, ptl_path, null, null, null, null);
            寻找全路径(key_path, null, null, null, null, ptl_path, null, null, null, null);

            寻找编号(ptl_path, null, obj_id, null, null);
            寻找编号(key_path, null, obj_id, mob_id, apc_id);
            寻找编号(act_path, null, obj_id, mob_id, apc_id);
        }


        /// <summary>
        /// 获取提供twn城镇文件中的ani map 全路径
        /// </summary>
        /// <param name="twnpath">城镇文件全路径集合</param>
        /// <param name="mappath">map文件全路径集合</param>
        /// <param name="anipath">ani文件全路径集合</param>
        public void Twn寻找map_ani(List<string> twnpath, List<string> mappath, List<string> anipath, bool CNmap)
        {
            //定义国服map集合
            List<string> cnmap = new List<string>();

            //定义一个正则类
            Regex_new regex = new Regex_new();

            //如果提供的城镇文件路径集合存在
            if (twnpath.Count > 0)
            {
                //枚举出所有路径
                foreach (string item in twnpath)
                {
                    //读入城镇文件内容
                    string alltext = FileReadAllText(item);
                    //正则获取城镇文件内的所有map路径
                    regex.创建(alltext, @"`(.+\.map)`", 1);
                    //如果匹配不为空
                    if (regex.取匹配数量() > 0)
                    {
                        //枚举出所有匹配的map路径
                        foreach (Match mapitem in regex.正则返回集合())
                        {
                            //获取真实map全路径
                            string lsmap = "map/" + mapitem.Groups[1].Value;
                            //map全路径替换斜杠，转为小写
                            lsmap = lsmap.Replace("\\", "/").ToLower();
                            //如果文件存在
                            if (FileExists(lsmap))
                            {
                                //如果不在map集合就加入map路径集合
                                if (mappath.Contains(lsmap) == false)
                                {
                                    mappath.Add(lsmap);

                                    //如果确认提取的是国服map
                                    if (CNmap)
                                    {
                                        if (cnmap.Contains(lsmap) == false)
                                        {
                                            cnmap.Add(lsmap);
                                        }
                                    }
                                }
                            }
                            else//反之
                            {
                                //map文件名前面插入“(r)”
                                lsmap = lsmap.Insert(lsmap.LastIndexOf("/") + 1, "(r)");
                                //如果文件存在
                                if (FileExists(lsmap))
                                {
                                    //如果不在map集合就加入map路径集合
                                    if (mappath.Contains(lsmap) == false)
                                    {
                                        mappath.Add(lsmap);

                                        //如果确认提取的是国服map
                                        if (CNmap)
                                        {
                                            if (cnmap.Contains(lsmap) == false)
                                            {
                                                cnmap.Add(lsmap);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //再次正则捕获twn文件中的ani文件
                    regex.创建(alltext, @"`(.+\.ani)`", 1);
                    //如果找到了
                    if (regex.取匹配数量() > 0)
                    {
                        //枚举出所有找到的ani
                        foreach (Match aniitem in regex.正则返回集合())
                        {
                            //取出当前父路径，取子匹配文本，识别点点杠，转为小写
                            string lsani = DirectoryGetParent(item) + "/" + aniitem.Groups[1].Value;
                            lsani = lsani.Replace("\\", "/");
                            lsani = 识别点点杠(lsani, true);
                            //如果不存在参数ani全路径中就加入
                            if (anipath.Contains(lsani) == false)
                            {
                                anipath.Add(lsani);
                            }
                        }
                    }
                }

                //如果提供的国服map不为空
                if (cnmap.Count > 0)
                {
                    //枚举出所有查找出来的map全路径
                    foreach (string item in cnmap)
                    {
                        //获取map文件内容
                        string alltext = FileReadAllText(item);
                        //正则创建捕获map中的map
                        regex.创建(alltext, @"\[import script\]\r\n`(.+\.map)`", 1);
                        //如果捕获到
                        if (regex.取匹配数量() > 0)
                        {
                            //获取查找到的map全路径
                            string lscnmap = "map/" + regex.取子匹配文本(1, 1);
                            //转为小写
                            lscnmap = lscnmap.Replace("\\", "/").ToLower();
                            //如果文件存在
                            if (FileExists(lscnmap))
                            {
                                //如果不存在参数map全路径中就加入
                                if (mappath.Contains(lscnmap) == false)
                                {
                                    mappath.Add(lscnmap);
                                }
                            }
                            else//反之
                            {
                                //map文件名前面插入“(r)”
                                lscnmap = lscnmap.Insert(lscnmap.LastIndexOf("/") + 1, "(r)");
                                //如果文件存在
                                if (FileExists(lscnmap))
                                {
                                    //如果不存在参数map全路径中就加入
                                    if (mappath.Contains(lscnmap) == false)
                                    {
                                        mappath.Add(lscnmap);
                                    }
                                }
                            }
                            //再次正则捕获map文件中的ani文件
                            regex.创建(alltext, @"`(.+\.ani)`", 1);
                            //如果找到了
                            if (regex.取匹配数量() > 0)
                            {
                                //枚举出所有找到的ani
                                foreach (Match aniitem in regex.正则返回集合())
                                {
                                    //取出当前父路径，取子匹配文本，识别点点杠，转为小写
                                    string lsani = DirectoryGetParent(item) + "/" + aniitem.Groups[1].Value;
                                    lsani = lsani.Replace("\\", "/");
                                    lsani = 识别点点杠(lsani, true);
                                    //如果不存在参数ani全路径中就加入
                                    if (anipath.Contains(lsani) == false)
                                    {
                                        anipath.Add(lsani);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 通过提供的map全路径，寻找出特效、npc编号 ani、til全路径
        /// </summary>
        /// <param name="mappath">map全路径集合</param>
        /// <param name="obj_id">特效编号集合</param>
        /// <param name="npc_id">npc编号集合</param>
        /// <param name="ani_path">ani全路径集合</param>
        /// <param name="til_path">til全路径集合</param>
        public void 城镇map寻找id和路径(List<string> mappath, List<string> obj_id, List<string> npc_id, List<string> ani_path, List<string> til_path)
        {
            //判断提供的全路径是否存在
            if (mappath.Count > 0)
            {
                //创建正则变量
                Regex_new regex = new Regex_new();

                //枚举出所有全路径
                foreach (string item in mappath)
                {
                    //声明变量
                    string alltetx;
                    string f_path;
                    string anipath;
                    string tilpath;

                    //读入文本
                    alltetx = FileReadAllText(item);
                    //取出父路径
                    f_path = DirectoryGetParent(item);
                    //如果参数obj_id不为空
                    if (obj_id != null)
                    {
                        //捕获特效编号
                        regex.创建(alltetx, @"\[passive object\]\r\n(.+)\r\n\[/passive object\]", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.创建(regex.取子匹配文本(1, 1), @"([0-9]+)\t([\x20-\x7f]+\t){3}", 0);
                            if (regex.取匹配数量() > 0)
                            {
                                regex.子匹配加入集合(obj_id, 1, false, 0);
                            }
                        }
                    }

                    //如果参数npc_id不为空
                    if (npc_id != null)
                    {
                        //捕获npc_id
                        regex.创建(alltetx, @"([0-9]+)\t`\[[a-z]+\]`\r\n([\x20-\x7f]+\t){3}", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(npc_id, 1, false, 0);
                        }

                    }

                    //如果参数ani路径不为空
                    if (ani_path != null)
                    {
                        //捕获ani
                        regex.创建(alltetx, @"`(.+\.ani)`", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            foreach (Match aniitem in regex.正则返回集合())
                            {
                                anipath = aniitem.Groups[1].Value.Replace("\\", "/");
                                if (anipath.Contains(".../") == false)
                                {
                                    //得到ani全路径
                                    anipath = f_path + "/" + anipath;
                                    //处理../
                                    anipath = 识别点点杠(anipath, true);
                                    //不存在集合就加入
                                    if (ani_path.Contains(anipath) == false)
                                    {
                                        ani_path.Add(anipath);
                                    }
                                }

                            }
                        }
                    }

                    //如果参数til路径不为空
                    if (til_path != null)
                    {
                        //捕获til
                        regex.创建(alltetx, @"`(.+\.til)`", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            foreach (Match tilitem in regex.正则返回集合())
                            {
                                tilpath = tilitem.Groups[1].Value.Replace("\\", "/");
                                if (tilpath.Contains(".../") == false)
                                {
                                    //得到ani全路径
                                    tilpath = f_path + "/" + tilpath;
                                    //处理../
                                    tilpath = 识别点点杠(tilpath, true);
                                    //如果不存在集合中就加入
                                    if (til_path.Contains(tilpath) == false)
                                    {
                                        til_path.Add(tilpath);
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 通过装备全路径，寻找出召唤apc、mob、obj、apd编号 ptl全路径
        /// </summary>
        /// <param name="equpath">装备全路径</param>
        /// <param name="apd_id">apd编号</param>
        /// <param name="mob_id">怪物编号</param>
        /// <param name="apc_id">apc编号</param>
        /// <param name="obj_id">特效编号</param>
        /// <param name="ptlpath">ptl全路径</param>
        public void Equ寻找编号和路径(List<string> equpath, List<string> apd_id, List<string> mob_id, List<string> apc_id, List<string> obj_id, List<string> ptlpath, List<string> part_set_id, List<string> ani_path)
        {

            if (equpath.Count > 0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in equpath)
                {
                    string alltext = FileReadAllText(item);

                    if (apd_id != null)
                    {
                        regex.创建(alltext, @"\[appendage\]\r\n([0-9]+)\t", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(apd_id, 1, false, 0);
                        }
                    }

                    if (obj_id != null)
                    {
                        regex.创建(alltext, @"\[passive object\](\r\n[\s\S]*?\r\n)\[/passive object\]", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            string zobjtxt = "";
                            string objitemtxt = "";
                            foreach (Match objitem in regex.正则返回集合())
                            {
                                objitemtxt = objitem.Groups[1].Value;
                                if (objitemtxt.Contains("passive object") == false)
                                {
                                    zobjtxt += objitemtxt;
                                }
                            }

                            regex.创建(zobjtxt, @"\r\n([0-9]+)\t([\x20-\x7f]+\t){5}`(.*\.ptl)?`", 0);
                            regex.子匹配加入集合(obj_id, 1, false, 1);

                            if (ptlpath != null)
                            {
                                regex.创建(zobjtxt, @"`(.+\.ptl)`", 1);
                                foreach (Match objitem in regex.正则返回集合())
                                {
                                    objitemtxt = "equipment/character/particle/" + objitem.Groups[1].Value;
                                    objitemtxt = objitemtxt.ToLower();
                                    if (FileExists(objitemtxt))
                                    {
                                        if (ptlpath.Contains(objitemtxt) == false)
                                        {
                                            ptlpath.Add(objitemtxt);
                                        }
                                    }
                                }
                            }

                        }
                    }

                    if (mob_id != null)
                    {
                        regex.创建(alltext, @"\[summon monster\]\r\n([0-9]+)\t[\x20-\x7f]+\t[\x20-\x7f]+\t", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(mob_id, 1, false, 0);
                        }
                    }

                    if (apc_id != null)
                    {
                        regex.创建(alltext, @"\[summon apc\]\r\n([0-9]+)\t[\x20-\x7f]+\t[\x20-\x7f]+\t", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(apc_id, 1, false, 0);
                        }
                    }

                    if (ptlpath != null)
                    {
                        regex.创建(alltext, @"`(.+\.ptl)`\r\n([\x20-\x7f]+\t){3}\r\n\[/particle\]", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            foreach (Match items in regex.正则返回集合())
                            {
                                string LinShiPtlPath = items.Groups[1].Value.ToLower().Replace("\\", "/"); ;
                                if (ptlpath.Contains(LinShiPtlPath) == false)
                                {
                                    ptlpath.Add(LinShiPtlPath);
                                }
                            }
                        }
                    }

                    if (part_set_id != null)
                    {
                        regex.创建(alltext, @"\[part set index\]\r\n([0-9]+)\t", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(part_set_id, 1, false, 0);
                        }
                    }

                    if (ani_path != null)
                    {
                        regex.创建(alltext, @"\[aurora graphic effects\]\r\n(.+`.+\.ani`\r\n){1,}", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            foreach (Match items in regex.正则返回集合())
                            {
                                Regex_new GHregex = new Regex_new();
                                GHregex.创建(items.Value, @"`(.+\.ani)`", 1);
                                foreach (Match itemss in GHregex.正则返回集合())
                                {
                                    string AniZ1 = itemss.Groups[1].Value.ToLower().Replace("\\", "/");
                                    if (ani_path.Contains(AniZ1) == false)
                                    {
                                        ani_path.Add(AniZ1);
                                    }
                                }
                            }
                        }

                        regex.创建(alltext, @"\[custom animation\]\r\n`(.+\.ani)`", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            string F_path = DirectoryGetParent(item);
                            foreach (Match items in regex.正则返回集合())
                            {
                                string TitleAni = 识别点点杠(F_path + "/" + items.Groups[1].Value, true);
                                if (ani_path.Contains(TitleAni) == false)
                                {
                                    ani_path.Add(TitleAni);
                                }

                            }
                        }

                        regex.创建(alltext, @"\[file name\]\r\n`(.+\.ani)`", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            foreach (Match items in regex.正则返回集合())
                            {
                                string EffectAni = items.Groups[1].Value.ToLower().Replace("\\", "/");
                                if (ani_path.Contains(EffectAni) == false)
                                {
                                    ani_path.Add(EffectAni);
                                }

                            }
                        }

                        regex.创建(alltext, @"\[attack success effect animation\]\r\n(.+`.+\.ani`\r\n){1,}", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            foreach (Match items in regex.正则返回集合())
                            {
                                Regex_new GHregex = new Regex_new();
                                GHregex.创建(items.Value, @"`(.+\.ani)`", 1);
                                foreach (Match itemss in GHregex.正则返回集合())
                                {
                                    string AniZ1 = itemss.Groups[1].Value.ToLower().Replace("\\", "/");
                                    if (ani_path.Contains(AniZ1) == false)
                                    {
                                        ani_path.Add(AniZ1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        /// <summary>
        /// 根据套装ETC套装ID寻找出对应信息，以及equ套装路径
        /// </summary>
        /// <param name="part_set_id">ETC套装ID</param>
        /// <param name="part_set_path">返回的套装文件路径</param>
        /// <param name="part_set_xinxi">返回的套装信息</param>
        public void 寻找Etc套装编号路径(List<string> part_set_id, List<string> part_set_path, StringBuilder part_set_xinxi)
        {
            if (part_set_id.Count > 0)
            {
                string Part_Set_Text;
                if (FileExists("etc/equipmentpartset.etc"))
                {
                    Regex_new regex = new Regex_new();
                    Part_Set_Text = FileReadAllText("etc/equipmentpartset.etc");
                    foreach (string item in part_set_id)
                    {
                        regex.创建(Part_Set_Text, @"\[equipment part set\][\r\n]+" + item + @"\t`(.+\.equ)`\r\n[\s\S]*?\[/equipment part set\]", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            string LinShiText = regex.取匹配文本(1);

                            Regex_new linShiPanDuan = new Regex_new();
                            linShiPanDuan.创建(LinShiText, @"\[equipment part set\]", 0);
                            if (linShiPanDuan.取匹配数量() <= 1)
                            {
                                string LinShiPuHuoText = regex.取子匹配文本(1, 1);
                                if (part_set_path.Contains("equipment/" + LinShiPuHuoText) == false)
                                {
                                    part_set_path.Add("equipment/" + LinShiPuHuoText);
                                    part_set_xinxi.Append(LinShiText + "\r\n\r\n");
                                }
                            }
                        }
                    }
                }
            }

        }



        /// <summary>
        /// 根据提供的装备全路径返回对应的img全路径
        /// </summary>
        /// <param name="EquPath">equ装备全路径集合</param>
        /// <param name="ImgPath">要加入的img全路径集合</param>
        /// <param name="FanKuiCuoWu">返回的错误equ列表</param>
        public void EquXunZhaoImgPath(string DaoChuPath, List<string> EquPath, List<string> ImgPath, StringBuilder FanKuiCuoWu)
        {
            if (EquPath.Count > 0)
            {
                //正则
                Regex_new regex = new Regex_new();
                //枚举出所有equ路径
                foreach (string item in EquPath)
                {
                    //判断文件是否存在
                    if (File.Exists(DaoChuPath + "/" + item))
                    {
                        string EquAllText = File.ReadAllText(DaoChuPath + "/" + item);//得到所有文件内容
                        //判断文件内是否存在img图层
                        if (EquAllText.Contains("[variation]"))
                        {
                            //正则捕获equ内的图层
                            regex.创建(EquAllText, @"(\[animation job\][\r\n]+(`\[.+\]`\r\n)+\r\n)?\[variation\][\r\n]+[0-9]+\t[0-9]+\t[\r\n]+(\[layer variation\][\r\n]+[0-9]+\t`.+`[\r\n]+\[equipment ani script\][\r\n]+`equipment[/\\].+\.lay`[\r\n]+){0,}", 1);
                            if (regex.取匹配数量() > 0)
                            {
                                int CuoWu = -1;//创建临时变量；用于接收转换的img是否正常；不正常则判断这个equ是错误的
                                foreach (Match items in regex.正则返回集合())
                                {
                                    CuoWu = EquAniFanHuiImgPath(items.Value, ImgPath);
                                }
                                if (CuoWu == 0)
                                {
                                    FanKuiCuoWu.Append(item + "\r\n");
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据传过来装备内的aniimg文本，返回对应的img全路径
        /// </summary>
        /// <param name="EquAniText">装备内的aniimg文本</param>
        /// <param name="ImgPath">返回到的img全路径集合</param>
        public int EquAniFanHuiImgPath(string EquAniText, List<string> ImgPath)
        {
            //正则
            Regex_new regex = new Regex_new();
            EquAniText = EquAniText.ToLower().Replace("\\", "/"); ;//转成小写；替换斜杠
            int FanKuiCuoWu = -1;//反馈错误信息；初始值-1
            string type = "";//装备职业类型
            string IdImg = "";//img编号
            string PathImg1;//img路径前段部分
            string PathImg2;//img路径后端部分

            //正则捕获职业类型
            regex.创建(EquAniText, @"\[equipment ani script\][\r\n]+`equipment/.+/(.+).lay`", 1);
            if (regex.取匹配数量() > 0)
            {
                type = regex.取子匹配文本(1, 1);
                if (type.Remove(2) == "at")//如果有at就加个空格
                {
                    type = type.Insert(2, " ");
                }
            }
            else//如果没找到；捕获第二种职业类型
            {
                regex.创建(EquAniText, @"\[animation job\][\r\n]+`\[(.+)\]`", 1);
                if (regex.取匹配数量() > 0)
                {
                    type = regex.取子匹配文本(1, 1);
                }
            }


            //捕获职业类型完成后；把调用相同职业时装的类型改成其他职业；
            switch (type)
            {
                case "dsswordman":
                    type = "swordman";
                    break;
                case "demonic swordman":
                    type = "swordman";
                    break;
                case "creator mage":
                    type = "mage";
                    break;
            }

            //正则捕获img编号
            regex.创建(EquAniText, @"\[variation\][\r\n]+([0-9]+)\t([0-9]+)\t", 1);
            if (regex.取匹配数量() > 0)
            {
                //这里创建两个编号，进行判断(原因：长度小于4、img编号最前面就要补上0)
                string Id1 = regex.取子匹配文本(1, 1);
                string Id2 = regex.取子匹配文本(1, 2);
                if (Id1 == "0")
                {
                    while (Id2.Length < 4)
                    {
                        Id2 = "0" + Id2;
                    }
                    IdImg = Id2;
                }
                else
                {
                    IdImg = Id1 + "0" + Id2;
                    while (IdImg.Length < 4)
                    {
                        IdImg = "0" + IdImg;
                    }
                }

            }


            //正则捕获img图层，看有多少个img
            regex.创建(EquAniText, @"\[layer variation\][\r\n]+[0-9]+\t`(.+)`", 1);
            if (regex.取匹配数量() > 0)
            {
                //枚举出所有捕获图层内容
                foreach (Match item in regex.正则返回集合())
                {
                    string MuLuMing = item.Groups[1].Value;
                    #region //过长的代码，用于判断到底是哪个img全路径
                    switch (type)
                    {
                        case "at fighter":
                            switch (MuLuMing)
                            {
                                case "at_knuckled":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "d.img";
                                    break;
                                case "shoes_c":
                                    PathImg1 = "character/fighter/atequipment/avatar/shoes/fm_shoes";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_df":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "df.img";
                                    break;
                                case "neck_xf":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "xf.img";
                                    break;
                                case "neck_ef":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "ef.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "neck_f":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_h":
                                    PathImg1 = "character/fighter/atequipment/avatar/coat/fm_coat";
                                    PathImg2 = "h.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/fighter/atequipment/avatar/coat/fm_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_x":
                                    PathImg1 = "character/fighter/atequipment/avatar/coat/fm_coat";
                                    PathImg2 = "x.img";
                                    break;
                                case "belt_e":
                                    PathImg1 = "character/fighter/atequipment/avatar/belt/fm_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_a":
                                    PathImg1 = "character/fighter/atequipment/avatar/belt/fm_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/fighter/atequipment/avatar/belt/fm_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/fighter/atequipment/avatar/belt/fm_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/fighter/atequipment/avatar/belt/fm_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/fighter/atequipment/avatar/belt/fm_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/fighter/atequipment/avatar/cap/fm_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/fighter/atequipment/avatar/cap/fm_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/fighter/atequipment/avatar/cap/fm_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/fighter/atequipment/avatar/cap/fm_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/fighter/atequipment/avatar/cap/fm_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/fighter/atequipment/avatar/coat/fm_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/fighter/atequipment/avatar/coat/fm_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/fighter/atequipment/avatar/coat/fm_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/fighter/atequipment/avatar/coat/fm_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/fighter/atequipment/avatar/coat/fm_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/fighter/atequipment/avatar/face/fm_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/fighter/atequipment/avatar/face/fm_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/fighter/atequipment/avatar/face/fm_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "face_g":
                                    PathImg1 = "character/fighter/atequipment/avatar/face/fm_face";
                                    PathImg2 = "g.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/fighter/atequipment/avatar/hair/fm_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/fighter/atequipment/avatar/hair/fm_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/fighter/atequipment/avatar/hair/fm_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "hair_f1":
                                    PathImg1 = "character/fighter/atequipment/avatar/hair/fm_hair";
                                    PathImg2 = "f1.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/fighter/atequipment/avatar/neck/fm_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/fighter/atequipment/avatar/pants/fm_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/fighter/atequipment/avatar/pants/fm_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/fighter/atequipment/avatar/pants/fm_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/fighter/atequipment/avatar/shoes/fm_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/fighter/atequipment/avatar/shoes/fm_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/fighter/atequipment/avatar/shoes/fm_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "at_bglovea":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_bglovea1":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_bglovea2":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_bgloveb":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_bgloveb1":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "b1.img";
                                    break;
                                case "at_bglovec":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_bglovec1":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_bglovec2":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_bgloved2":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "d2.img";
                                    break;
                                case "at_bglovex":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "x.img";
                                    break;
                                case "at_bglovex1":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "x1.img";
                                    break;
                                case "at_bglovex2":
                                    PathImg1 = "character/fighter/atequipment/weapon/boxglove/fm_bglove";
                                    PathImg2 = "x2.img";
                                    break;
                                case "at_boneclawa":
                                    PathImg1 = "character/fighter/atequipment/weapon/boneclaw/fm_bclaw";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_boneclawc":
                                    PathImg1 = "character/fighter/atequipment/weapon/boneclaw/fm_bclaw";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_boneclawx":
                                    PathImg1 = "character/fighter/atequipment/weapon/boneclaw/fm_bclaw";
                                    PathImg2 = "x.img";
                                    break;
                                case "at_clawa":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_clawa1":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_clawa2":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_clawb1":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "b1.img";
                                    break;
                                case "at_clawb2":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "b2.img";
                                    break;
                                case "at_clawc":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_clawc1":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_clawc2":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_clawx":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "x.img";
                                    break;
                                case "at_clawx1":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "x1.img";
                                    break;
                                case "at_clawx2":
                                    PathImg1 = "character/fighter/atequipment/weapon/claw/fm_claw";
                                    PathImg2 = "x2.img";
                                    break;
                                case "at_arma":
                                    PathImg1 = "character/fighter/atequipment/weapon/arm/fm_arms";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_armb":
                                    PathImg1 = "character/fighter/atequipment/weapon/arm/fm_arms";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_armc":
                                    PathImg1 = "character/fighter/atequipment/weapon/arm/fm_arms";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_armx":
                                    PathImg1 = "character/fighter/atequipment/weapon/arm/fm_arms";
                                    PathImg2 = "x.img";
                                    break;
                                case "at_gauntleta":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_gauntleta1":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_gauntleta2":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_gauntletc":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_gauntletc1":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_gauntletc2":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_gauntletx":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "x.img";
                                    break;
                                case "at_gauntletx1":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "x1.img";
                                    break;
                                case "at_gauntletx2":
                                    PathImg1 = "character/fighter/atequipment/weapon/gauntlet/fm_gauntlet";
                                    PathImg2 = "x2.img";
                                    break;
                                case "at_glovea":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_gloveb":
                                    PathImg1 = "character/fighter/atequipment/weapon/glove/fm_glove";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_glovec":
                                    PathImg1 = "character/fighter/atequipment/weapon/glove/fm_glove";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_glovex":
                                    PathImg1 = "character/fighter/atequipment/weapon/glove/fm_glove";
                                    PathImg2 = "x.img";
                                    break;
                                case "at_knucklea":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_knucklea1":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_knucklea2":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_knucklec":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_knucklec1":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_knucklec2":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_knucklex":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "x.img";
                                    break;
                                case "at_knucklex1":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "x1.img";
                                    break;
                                case "at_knucklex2":
                                    PathImg1 = "character/fighter/atequipment/weapon/knuckle/fm_knuckle";
                                    PathImg2 = "x2.img";
                                    break;
                                case "at_tonfaa":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_tonfaa1":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_tonfaa2":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_tonfab":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_tonfab1":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "b1.img";
                                    break;
                                case "at_tonfab2":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "b2.img";
                                    break;
                                case "at_tonfac":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_tonfac1":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_tonfac2":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_tonfax":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "x.img";
                                    break;
                                case "at_tonfax1":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "x1.img";
                                    break;
                                case "at_tonfax2":
                                    PathImg1 = "character/fighter/atequipment/weapon/tonfa/fm_tonfa";
                                    PathImg2 = "x2.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        case "fighter":
                            switch (MuLuMing)
                            {
                                case "coat_df":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "df.img";
                                    break;
                                case "coat_xf":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "xf.img";
                                    break;
                                case "coat_ef":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "ef.img";
                                    break;
                                case "coat_k":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "k.img";
                                    break;
                                case "coat_x":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "x.img";
                                    break;
                                case "belt_h":
                                    PathImg1 = "character/fighter/equipment/avatar/belt/ft_belt";
                                    PathImg2 = "h.img";
                                    break;
                                case "belt_a":
                                    PathImg1 = "character/fighter/equipment/avatar/belt/ft_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/fighter/equipment/avatar/belt/ft_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/fighter/equipment/avatar/belt/ft_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/fighter/equipment/avatar/belt/ft_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_e":
                                    PathImg1 = "character/fighter/equipment/avatar/belt/ft_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/fighter/equipment/avatar/belt/ft_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "belt_g":
                                    PathImg1 = "character/fighter/equipment/avatar/belt/ft_belt";
                                    PathImg2 = "g.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/fighter/equipment/avatar/cap/ft_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/fighter/equipment/avatar/cap/ft_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/fighter/equipment/avatar/cap/ft_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/fighter/equipment/avatar/cap/ft_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_e":
                                    PathImg1 = "character/fighter/equipment/avatar/cap/ft_cap";
                                    PathImg2 = "e.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/fighter/equipment/avatar/cap/ft_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "coat_e":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "e.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_h":
                                    PathImg1 = "character/fighter/equipment/avatar/coat/ft_coat";
                                    PathImg2 = "h.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/fighter/equipment/avatar/face/ft_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/fighter/equipment/avatar/face/ft_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/fighter/equipment/avatar/face/ft_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "face_d":
                                    PathImg1 = "character/fighter/equipment/avatar/face/ft_face";
                                    PathImg2 = "d.img";
                                    break;
                                case "face_e":
                                    PathImg1 = "character/fighter/equipment/avatar/face/ft_face";
                                    PathImg2 = "e.img";
                                    break;
                                case "face_f":
                                    PathImg1 = "character/fighter/equipment/avatar/face/ft_face";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_g":
                                    PathImg1 = "character/fighter/equipment/avatar/face/ft_face";
                                    PathImg2 = "g.img";
                                    break;
                                case "face_h":
                                    PathImg1 = "character/fighter/equipment/avatar/face/ft_face";
                                    PathImg2 = "h.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/fighter/equipment/avatar/hair/ft_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/fighter/equipment/avatar/hair/ft_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_c":
                                    PathImg1 = "character/fighter/equipment/avatar/hair/ft_hair";
                                    PathImg2 = "c.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/fighter/equipment/avatar/hair/ft_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "hair_e":
                                    PathImg1 = "character/fighter/equipment/avatar/hair/ft_hair";
                                    PathImg2 = "e.img";
                                    break;
                                case "hair_f":
                                    PathImg1 = "character/fighter/equipment/avatar/hair/ft_hair";
                                    PathImg2 = "f.img";
                                    break;
                                case "hair_f1":
                                    PathImg1 = "character/fighter/equipment/avatar/hair/ft_hair";
                                    PathImg2 = "f1.img";
                                    break;
                                case "neck_df":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "df.img";
                                    break;
                                case "neck_xf":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "xf.img";
                                    break;
                                case "neck_ef":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "ef.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_f":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "f.img";
                                    break;
                                case "neck_g":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "g.img";
                                    break;
                                case "neck_h":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "h.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "neck_z":
                                    PathImg1 = "character/fighter/equipment/avatar/neck/ft_neck";
                                    PathImg2 = "z.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/fighter/equipment/avatar/pants/ft_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/fighter/equipment/avatar/pants/ft_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_c":
                                    PathImg1 = "character/fighter/equipment/avatar/pants/ft_pants";
                                    PathImg2 = "c.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/fighter/equipment/avatar/pants/ft_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "pants_e":
                                    PathImg1 = "character/fighter/equipment/avatar/pants/ft_pants";
                                    PathImg2 = "e.img";
                                    break;
                                case "pants_f":
                                    PathImg1 = "character/fighter/equipment/avatar/pants/ft_pants";
                                    PathImg2 = "f.img";
                                    break;
                                case "pants_g":
                                    PathImg1 = "character/fighter/equipment/avatar/pants/ft_pants";
                                    PathImg2 = "g.img";
                                    break;
                                case "pants_h":
                                    PathImg1 = "character/fighter/equipment/avatar/pants/ft_pants";
                                    PathImg2 = "h.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/fighter/equipment/avatar/shoes/ft_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/fighter/equipment/avatar/shoes/ft_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "shoes_c":
                                    PathImg1 = "character/fighter/equipment/avatar/shoes/ft_shoes";
                                    PathImg2 = "c.img";
                                    break;
                                case "shoes_d":
                                    PathImg1 = "character/fighter/equipment/avatar/shoes/ft_shoes";
                                    PathImg2 = "d.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/fighter/equipment/avatar/shoes/ft_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "bgloveb":
                                    PathImg1 = "character/fighter/equipment/weapon/boxglove/bglove";
                                    PathImg2 = "b.img";
                                    break;
                                case "bgloveb1":
                                    PathImg1 = "character/fighter/equipment/weapon/boxglove/bglove";
                                    PathImg2 = "b1.img";
                                    break;
                                case "bgloveb2":
                                    PathImg1 = "character/fighter/equipment/weapon/boxglove/bglove";
                                    PathImg2 = "b2.img";
                                    break;
                                case "bglovec":
                                    PathImg1 = "character/fighter/equipment/weapon/boxglove/bglove";
                                    PathImg2 = "c.img";
                                    break;
                                case "bglovec1":
                                    PathImg1 = "character/fighter/equipment/weapon/boxglove/bglove";
                                    PathImg2 = "c1.img";
                                    break;
                                case "bglovec2":
                                    PathImg1 = "character/fighter/equipment/weapon/boxglove/bglove";
                                    PathImg2 = "c2.img";
                                    break;
                                case "catgloveb":
                                    PathImg1 = "character/fighter/equipment/weapon/catglove/cglove";
                                    PathImg2 = "b.img";
                                    break;
                                case "catglovec":
                                    PathImg1 = "character/fighter/equipment/weapon/catglove/cglove";
                                    PathImg2 = "c.img";
                                    break;
                                case "boneclawb":
                                    PathImg1 = "character/fighter/equipment/weapon/boneclaw/bclaw";
                                    PathImg2 = "b.img";
                                    break;
                                case "boneclawc":
                                    PathImg1 = "character/fighter/equipment/weapon/boneclaw/bclaw";
                                    PathImg2 = "c.img";
                                    break;
                                case "clawa1":
                                    PathImg1 = "character/fighter/equipment/weapon/claw/claw";
                                    PathImg2 = "a1.img";
                                    break;
                                case "clawa2":
                                    PathImg1 = "character/fighter/equipment/weapon/claw/claw";
                                    PathImg2 = "a2.img";
                                    break;
                                case "clawb":
                                    PathImg1 = "character/fighter/equipment/weapon/claw/claw";
                                    PathImg2 = "b.img";
                                    break;
                                case "clawb1":
                                    PathImg1 = "character/fighter/equipment/weapon/claw/claw";
                                    PathImg2 = "b1.img";
                                    break;
                                case "clawb2":
                                    PathImg1 = "character/fighter/equipment/weapon/claw/claw";
                                    PathImg2 = "b2.img";
                                    break;
                                case "clawc":
                                    PathImg1 = "character/fighter/equipment/weapon/claw/claw";
                                    PathImg2 = "c.img";
                                    break;
                                case "clawc1":
                                    PathImg1 = "character/fighter/equipment/weapon/claw/claw";
                                    PathImg2 = "c1.img";
                                    break;
                                case "clawc2":
                                    PathImg1 = "character/fighter/equipment/weapon/claw/claw";
                                    PathImg2 = "c2.img";
                                    break;
                                case "armb":
                                    PathImg1 = "character/fighter/equipment/weapon/arm/arm";
                                    PathImg2 = "b.img";
                                    break;
                                case "armc":
                                    PathImg1 = "character/fighter/equipment/weapon/arm/arm";
                                    PathImg2 = "c.img";
                                    break;
                                case "gauntletb":
                                    PathImg1 = "character/fighter/equipment/weapon/gauntlet/gauntlet";
                                    PathImg2 = "b.img";
                                    break;
                                case "gauntletb1":
                                    PathImg1 = "character/fighter/equipment/weapon/gauntlet/gauntlet";
                                    PathImg2 = "b1.img";
                                    break;
                                case "gauntletb2":
                                    PathImg1 = "character/fighter/equipment/weapon/gauntlet/gauntlet";
                                    PathImg2 = "b2.img";
                                    break;
                                case "gauntletc":
                                    PathImg1 = "character/fighter/equipment/weapon/gauntlet/gauntlet";
                                    PathImg2 = "c.img";
                                    break;
                                case "gauntletc1":
                                    PathImg1 = "character/fighter/equipment/weapon/gauntlet/gauntlet";
                                    PathImg2 = "c1.img";
                                    break;
                                case "gauntletc2":
                                    PathImg1 = "character/fighter/equipment/weapon/gauntlet/gauntlet";
                                    PathImg2 = "c2.img";
                                    break;
                                case "yangyangib":
                                    PathImg1 = "character/fighter/equipment/weapon/glove/glove";
                                    PathImg2 = "b.img";
                                    break;
                                case "yangyangic":
                                    PathImg1 = "character/fighter/equipment/weapon/glove/glove";
                                    PathImg2 = "c.img";
                                    break;
                                case "gloveb":
                                    PathImg1 = "character/fighter/equipment/weapon/glove/glove";
                                    PathImg2 = "b.img";
                                    break;
                                case "glovec":
                                    PathImg1 = "character/fighter/equipment/weapon/glove/glove";
                                    PathImg2 = "c.img";
                                    break;
                                case "knuckleb":
                                    PathImg1 = "character/fighter/equipment/weapon/knuckle/knuckle";
                                    PathImg2 = "b.img";
                                    break;
                                case "knuckleb1":
                                    PathImg1 = "character/fighter/equipment/weapon/knuckle/knuckle";
                                    PathImg2 = "b1.img";
                                    break;
                                case "knuckleb2":
                                    PathImg1 = "character/fighter/equipment/weapon/knuckle/knuckle";
                                    PathImg2 = "b2.img";
                                    break;
                                case "knucklec":
                                    PathImg1 = "character/fighter/equipment/weapon/knuckle/knuckle";
                                    PathImg2 = "c.img";
                                    break;
                                case "knucklec1":
                                    PathImg1 = "character/fighter/equipment/weapon/knuckle/knuckle";
                                    PathImg2 = "c1.img";
                                    break;
                                case "knucklec2":
                                    PathImg1 = "character/fighter/equipment/weapon/knuckle/knuckle";
                                    PathImg2 = "c2.img";
                                    break;
                                case "tonfaa":
                                    PathImg1 = "character/fighter/equipment/weapon/tonfa/tonfa";
                                    PathImg2 = "a.img";
                                    break;
                                case "tonfab":
                                    PathImg1 = "character/fighter/equipment/weapon/tonfa/tonfa";
                                    PathImg2 = "b.img";
                                    break;
                                case "tonfab1":
                                    PathImg1 = "character/fighter/equipment/weapon/tonfa/tonfa";
                                    PathImg2 = "b1.img";
                                    break;
                                case "tonfab2":
                                    PathImg1 = "character/fighter/equipment/weapon/tonfa/tonfa";
                                    PathImg2 = "b2.img";
                                    break;
                                case "tonfac":
                                    PathImg1 = "character/fighter/equipment/weapon/tonfa/tonfa";
                                    PathImg2 = "c.img";
                                    break;
                                case "tonfac1":
                                    PathImg1 = "character/fighter/equipment/weapon/tonfa/tonfa";
                                    PathImg2 = "c1.img";
                                    break;
                                case "tonfac2":
                                    PathImg1 = "character/fighter/equipment/weapon/tonfa/tonfa";
                                    PathImg2 = "c2.img";
                                    break;
                                case "tonfax":
                                    PathImg1 = "character/fighter/equipment/weapon/tonfa/tonfa";
                                    PathImg2 = "x.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        case "at gunner":
                            switch (MuLuMing)
                            {
                                case "neck_kf":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "kf.img";
                                    break;
                                case "neck_ef":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "ef.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "face_h":
                                    PathImg1 = "character/gunner/atequipment/avatar/face/gg_face";
                                    PathImg2 = "h.img";
                                    break;
                                case "coat_x":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "x.img";
                                    break;
                                case "belt_g":
                                    PathImg1 = "character/gunner/atequipment/avatar/belt/gg_belt";
                                    PathImg2 = "g.img";
                                    break;
                                case "belt_a":
                                    PathImg1 = "character/gunner/atequipment/avatar/belt/gg_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/gunner/atequipment/avatar/belt/gg_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/gunner/atequipment/avatar/belt/gg_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/gunner/atequipment/avatar/belt/gg_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_e":
                                    PathImg1 = "character/gunner/atequipment/avatar/belt/gg_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/gunner/atequipment/avatar/belt/gg_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/gunner/atequipment/avatar/cap/gg_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/gunner/atequipment/avatar/cap/gg_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/gunner/atequipment/avatar/cap/gg_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/gunner/atequipment/avatar/cap/gg_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_e":
                                    PathImg1 = "character/gunner/atequipment/avatar/cap/gg_cap";
                                    PathImg2 = "e.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/gunner/atequipment/avatar/cap/gg_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "cap_g":
                                    PathImg1 = "character/gunner/atequipment/avatar/cap/gg_cap";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "coat_e":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "e.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_h":
                                    PathImg1 = "character/gunner/atequipment/avatar/coat/gg_coat";
                                    PathImg2 = "h.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/gunner/atequipment/avatar/face/gg_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/gunner/atequipment/avatar/face/gg_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/gunner/atequipment/avatar/face/gg_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "face_d":
                                    PathImg1 = "character/gunner/atequipment/avatar/face/gg_face";
                                    PathImg2 = "d.img";
                                    break;
                                case "face_e":
                                    PathImg1 = "character/gunner/atequipment/avatar/face/gg_face";
                                    PathImg2 = "e.img";
                                    break;
                                case "face_f":
                                    PathImg1 = "character/gunner/atequipment/avatar/face/gg_face";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_g":
                                    PathImg1 = "character/gunner/atequipment/avatar/face/gg_face";
                                    PathImg2 = "g.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/gunner/atequipment/avatar/hair/gg_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/gunner/atequipment/avatar/hair/gg_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_c":
                                    PathImg1 = "character/gunner/atequipment/avatar/hair/gg_hair";
                                    PathImg2 = "c.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/gunner/atequipment/avatar/hair/gg_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "hair_e":
                                    PathImg1 = "character/gunner/atequipment/avatar/hair/gg_hair";
                                    PathImg2 = "e.img";
                                    break;
                                case "hair_f":
                                    PathImg1 = "character/gunner/atequipment/avatar/hair/gg_hair";
                                    PathImg2 = "f.img";
                                    break;
                                case "hair_f1":
                                    PathImg1 = "character/gunner/atequipment/avatar/hair/gg_hair";
                                    PathImg2 = "f1.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_f":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "f.img";
                                    break;
                                case "neck_g":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "g.img";
                                    break;
                                case "neck_h":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "h.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "neck_z":
                                    PathImg1 = "character/gunner/atequipment/avatar/neck/gg_neck";
                                    PathImg2 = "z.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/gunner/atequipment/avatar/pants/gg_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/gunner/atequipment/avatar/pants/gg_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_c":
                                    PathImg1 = "character/gunner/atequipment/avatar/pants/gg_pants";
                                    PathImg2 = "c.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/gunner/atequipment/avatar/pants/gg_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "pants_e":
                                    PathImg1 = "character/gunner/atequipment/avatar/pants/gg_pants";
                                    PathImg2 = "e.img";
                                    break;
                                case "pants_f":
                                    PathImg1 = "character/gunner/atequipment/avatar/pants/gg_pants";
                                    PathImg2 = "f.img";
                                    break;
                                case "pants_g":
                                    PathImg1 = "character/gunner/atequipment/avatar/pants/gg_pants";
                                    PathImg2 = "g.img";
                                    break;
                                case "pants_h":
                                    PathImg1 = "character/gunner/atequipment/avatar/pants/gg_pants";
                                    PathImg2 = "h.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/gunner/atequipment/avatar/shoes/gg_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/gunner/atequipment/avatar/shoes/gg_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "shoes_c":
                                    PathImg1 = "character/gunner/atequipment/avatar/shoes/gg_shoes";
                                    PathImg2 = "c.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/gunner/atequipment/avatar/shoes/gg_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "at_autob":
                                    PathImg1 = "character/gunner/atequipment/weapon/auto/gg_auto";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_autob1":
                                    PathImg1 = "character/gunner/atequipment/weapon/auto/gg_auto";
                                    PathImg2 = "b1.img";
                                    break;
                                case "at_autob2":
                                    PathImg1 = "character/gunner/atequipment/weapon/auto/gg_auto";
                                    PathImg2 = "b2.img";
                                    break;
                                case "at_autoc":
                                    PathImg1 = "character/gunner/atequipment/weapon/auto/gg_auto";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_autoc1":
                                    PathImg1 = "character/gunner/atequipment/weapon/auto/gg_auto";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_autoc2":
                                    PathImg1 = "character/gunner/atequipment/weapon/auto/gg_auto";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_musketb":
                                    PathImg1 = "character/gunner/atequipment/weapon/musket/gg_musket";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_musketc":
                                    PathImg1 = "character/gunner/atequipment/weapon/musket/gg_musket";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_bowgunb":
                                    PathImg1 = "character/gunner/atequipment/weapon/bowgun/gg_bowgun";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_bowgunb1":
                                    PathImg1 = "character/gunner/atequipment/weapon/bowgun/gg_bowgun";
                                    PathImg2 = "b1.img";
                                    break;
                                case "at_bowgunb2":
                                    PathImg1 = "character/gunner/atequipment/weapon/bowgun/gg_bowgun";
                                    PathImg2 = "b2.img";
                                    break;
                                case "at_bowgunc":
                                    PathImg1 = "character/gunner/atequipment/weapon/bowgun/gg_bowgun";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_bowgunc1":
                                    PathImg1 = "character/gunner/atequipment/weapon/bowgun/gg_bowgun";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_bowgunc2":
                                    PathImg1 = "character/gunner/atequipment/weapon/bowgun/gg_bowgun";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_hcana":
                                    PathImg1 = "character/gunner/atequipment/weapon/hcan/gg_hcan";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_hcanb":
                                    PathImg1 = "character/gunner/atequipment/weapon/hcan/gg_hcan";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_hcanb1":
                                    PathImg1 = "character/gunner/atequipment/weapon/hcan/gg_hcan";
                                    PathImg2 = "b1.img";
                                    break;
                                case "at_hcanb2":
                                    PathImg1 = "character/gunner/atequipment/weapon/hcan/gg_hcan";
                                    PathImg2 = "b2.img";
                                    break;
                                case "at_hcanc":
                                    PathImg1 = "character/gunner/atequipment/weapon/hcan/gg_hcan";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_hcanc1":
                                    PathImg1 = "character/gunner/atequipment/weapon/hcan/gg_hcan";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_hcanc2":
                                    PathImg1 = "character/gunner/atequipment/weapon/hcan/gg_hcan";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_musketb1":
                                    PathImg1 = "character/gunner/atequipment/weapon/musket/gg_musket";
                                    PathImg2 = "b1.img";
                                    break;
                                case "at_musketb2":
                                    PathImg1 = "character/gunner/atequipment/weapon/musket/gg_musket";
                                    PathImg2 = "b2.img";
                                    break;
                                case "at_musketc1":
                                    PathImg1 = "character/gunner/atequipment/weapon/musket/gg_musket";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_musketc2":
                                    PathImg1 = "character/gunner/atequipment/weapon/musket/gg_musket";
                                    PathImg2 = "c2.img";
                                    break;
                                case "at_mcgeeb":
                                    PathImg1 = "character/gunner/atequipment/weapon/auto/gg_auto";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_mcgeec":
                                    PathImg1 = "character/gunner/atequipment/weapon/auto/gg_auto";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_revb":
                                    PathImg1 = "character/gunner/atequipment/weapon/rev/gg_rev";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_revb1":
                                    PathImg1 = "character/gunner/atequipment/weapon/rev/gg_rev";
                                    PathImg2 = "b1.img";
                                    break;
                                case "at_revb2":
                                    PathImg1 = "character/gunner/atequipment/weapon/rev/gg_rev";
                                    PathImg2 = "b2.img";
                                    break;
                                case "at_revc":
                                    PathImg1 = "character/gunner/atequipment/weapon/rev/gg_rev";
                                    PathImg2 = "c.img";
                                    break;
                                case "at_revc1":
                                    PathImg1 = "character/gunner/atequipment/weapon/rev/gg_rev";
                                    PathImg2 = "c1.img";
                                    break;
                                case "at_revc2":
                                    PathImg1 = "character/gunner/atequipment/weapon/rev/gg_rev";
                                    PathImg2 = "c2.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        case "gunner":
                            switch (MuLuMing)
                            {
                                case "neck_xf":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "xf.img";
                                    break;
                                case "neck_kf":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "kf.img";
                                    break;
                                case "neck_ef":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "ef.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "coat_h":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "h.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_x":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "x.img";
                                    break;
                                case "belt_a":
                                    PathImg1 = "character/gunner/equipment/avatar/belt/gn_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/gunner/equipment/avatar/belt/gn_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/gunner/equipment/avatar/belt/gn_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/gunner/equipment/avatar/belt/gn_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_e":
                                    PathImg1 = "character/gunner/equipment/avatar/belt/gn_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/gunner/equipment/avatar/belt/gn_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "belt_g":
                                    PathImg1 = "character/gunner/equipment/avatar/belt/gn_belt";
                                    PathImg2 = "g.img";
                                    break;
                                case "belt_h":
                                    PathImg1 = "character/gunner/equipment/avatar/belt/gn_belt";
                                    PathImg2 = "h.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/gunner/equipment/avatar/cap/gn_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/gunner/equipment/avatar/cap/gn_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/gunner/equipment/avatar/cap/gn_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/gunner/equipment/avatar/cap/gn_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_e":
                                    PathImg1 = "character/gunner/equipment/avatar/cap/gn_cap";
                                    PathImg2 = "e.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/gunner/equipment/avatar/cap/gn_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "cap_g":
                                    PathImg1 = "character/gunner/equipment/avatar/cap/gn_cap";
                                    PathImg2 = "g.img";
                                    break;
                                case "cap_h":
                                    PathImg1 = "character/gunner/equipment/avatar/cap/gn_cap";
                                    PathImg2 = "h.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "coat_e":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "e.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/gunner/equipment/avatar/coat/gn_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/gunner/equipment/avatar/face/gn_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/gunner/equipment/avatar/face/gn_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/gunner/equipment/avatar/face/gn_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "face_d":
                                    PathImg1 = "character/gunner/equipment/avatar/face/gn_face";
                                    PathImg2 = "d.img";
                                    break;
                                case "face_e":
                                    PathImg1 = "character/gunner/equipment/avatar/face/gn_face";
                                    PathImg2 = "e.img";
                                    break;
                                case "face_f":
                                    PathImg1 = "character/gunner/equipment/avatar/face/gn_face";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_g":
                                    PathImg1 = "character/gunner/equipment/avatar/face/gn_face";
                                    PathImg2 = "g.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/gunner/equipment/avatar/hair/gn_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/gunner/equipment/avatar/hair/gn_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_c":
                                    PathImg1 = "character/gunner/equipment/avatar/hair/gn_hair";
                                    PathImg2 = "c.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/gunner/equipment/avatar/hair/gn_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "hair_e":
                                    PathImg1 = "character/gunner/equipment/avatar/hair/gn_hair";
                                    PathImg2 = "e.img";
                                    break;
                                case "hair_f":
                                    PathImg1 = "character/gunner/equipment/avatar/hair/gn_hair";
                                    PathImg2 = "f.img";
                                    break;
                                case "hair_f1":
                                    PathImg1 = "character/gunner/equipment/avatar/hair/gn_hair";
                                    PathImg2 = "f1.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_f":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "f.img";
                                    break;
                                case "neck_h":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "h.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "neck_z":
                                    PathImg1 = "character/gunner/equipment/avatar/neck/gn_neck";
                                    PathImg2 = "z.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/gunner/equipment/avatar/pants/gn_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/gunner/equipment/avatar/pants/gn_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_c":
                                    PathImg1 = "character/gunner/equipment/avatar/pants/gn_pants";
                                    PathImg2 = "c.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/gunner/equipment/avatar/pants/gn_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "pants_e":
                                    PathImg1 = "character/gunner/equipment/avatar/pants/gn_pants";
                                    PathImg2 = "e.img";
                                    break;
                                case "pants_f":
                                    PathImg1 = "character/gunner/equipment/avatar/pants/gn_pants";
                                    PathImg2 = "f.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/gunner/equipment/avatar/shoes/gn_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/gunner/equipment/avatar/shoes/gn_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "shoes_c":
                                    PathImg1 = "character/gunner/equipment/avatar/shoes/gn_shoes";
                                    PathImg2 = "c.img";
                                    break;
                                case "shoes_d":
                                    PathImg1 = "character/gunner/equipment/avatar/shoes/gn_shoes";
                                    PathImg2 = "d.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/gunner/equipment/avatar/shoes/gn_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "autob":
                                    PathImg1 = "character/gunner/equipment/weapon/auto/auto";
                                    PathImg2 = "b.img";
                                    break;
                                case "autob1":
                                    PathImg1 = "character/gunner/equipment/weapon/auto/auto";
                                    PathImg2 = "b1.img";
                                    break;
                                case "autob2":
                                    PathImg1 = "character/gunner/equipment/weapon/auto/auto";
                                    PathImg2 = "b2.img";
                                    break;
                                case "autoc":
                                    PathImg1 = "character/gunner/equipment/weapon/auto/auto";
                                    PathImg2 = "c.img";
                                    break;
                                case "autoc1":
                                    PathImg1 = "character/gunner/equipment/weapon/auto/auto";
                                    PathImg2 = "c1.img";
                                    break;
                                case "autoc2":
                                    PathImg1 = "character/gunner/equipment/weapon/auto/auto";
                                    PathImg2 = "c2.img";
                                    break;
                                case "musketb":
                                    PathImg1 = "character/gunner/equipment/weapon/musket/musket";
                                    PathImg2 = "b.img";
                                    break;
                                case "musketc":
                                    PathImg1 = "character/gunner/equipment/weapon/musket/musket";
                                    PathImg2 = "c.img";
                                    break;
                                case "bowgunb":
                                    PathImg1 = "character/gunner/equipment/weapon/bowgun/bowgun";
                                    PathImg2 = "b.img";
                                    break;
                                case "bowgunb1":
                                    PathImg1 = "character/gunner/equipment/weapon/bowgun/bowgun";
                                    PathImg2 = "b1.img";
                                    break;
                                case "bowgunb2":
                                    PathImg1 = "character/gunner/equipment/weapon/bowgun/bowgun";
                                    PathImg2 = "b2.img";
                                    break;
                                case "bowgunc":
                                    PathImg1 = "character/gunner/equipment/weapon/bowgun/bowgun";
                                    PathImg2 = "c.img";
                                    break;
                                case "bowgunc1":
                                    PathImg1 = "character/gunner/equipment/weapon/bowgun/bowgun";
                                    PathImg2 = "c1.img";
                                    break;
                                case "bowgunc2":
                                    PathImg1 = "character/gunner/equipment/weapon/bowgun/bowgun";
                                    PathImg2 = "c2.img";
                                    break;
                                case "hcana":
                                    PathImg1 = "character/gunner/equipment/weapon/hcan/hcan";
                                    PathImg2 = "a.img";
                                    break;
                                case "hcanb":
                                    PathImg1 = "character/gunner/equipment/weapon/hcan/hcan";
                                    PathImg2 = "b.img";
                                    break;
                                case "hcanb1":
                                    PathImg1 = "character/gunner/equipment/weapon/hcan/hcan";
                                    PathImg2 = "b1.img";
                                    break;
                                case "hcanb2":
                                    PathImg1 = "character/gunner/equipment/weapon/hcan/hcan";
                                    PathImg2 = "b2.img";
                                    break;
                                case "hcanc":
                                    PathImg1 = "character/gunner/equipment/weapon/hcan/hcan";
                                    PathImg2 = "c.img";
                                    break;
                                case "hcanc1":
                                    PathImg1 = "character/gunner/equipment/weapon/hcan/hcan";
                                    PathImg2 = "c1.img";
                                    break;
                                case "hcanc2":
                                    PathImg1 = "character/gunner/equipment/weapon/hcan/hcan";
                                    PathImg2 = "c2.img";
                                    break;
                                case "musketb1":
                                    PathImg1 = "character/gunner/equipment/weapon/musket/musket";
                                    PathImg2 = "b1.img";
                                    break;
                                case "musketb2":
                                    PathImg1 = "character/gunner/equipment/weapon/musket/musket";
                                    PathImg2 = "b2.img";
                                    break;
                                case "musketc1":
                                    PathImg1 = "character/gunner/equipment/weapon/musket/musket";
                                    PathImg2 = "c1.img";
                                    break;
                                case "musketc2":
                                    PathImg1 = "character/gunner/equipment/weapon/musket/musket";
                                    PathImg2 = "c2.img";
                                    break;
                                case "mcgeeb":
                                    PathImg1 = "character/gunner/equipment/weapon/auto/auto";
                                    PathImg2 = "b.img";
                                    break;
                                case "mcgeec":
                                    PathImg1 = "character/gunner/equipment/weapon/auto/auto";
                                    PathImg2 = "c.img";
                                    break;
                                case "revb":
                                    PathImg1 = "character/gunner/equipment/weapon/rev/rev";
                                    PathImg2 = "b.img";
                                    break;
                                case "revb1":
                                    PathImg1 = "character/gunner/equipment/weapon/rev/rev";
                                    PathImg2 = "b1.img";
                                    break;
                                case "revb2":
                                    PathImg1 = "character/gunner/equipment/weapon/rev/rev";
                                    PathImg2 = "b2.img";
                                    break;
                                case "revc":
                                    PathImg1 = "character/gunner/equipment/weapon/rev/rev";
                                    PathImg2 = "c.img";
                                    break;
                                case "revc1":
                                    PathImg1 = "character/gunner/equipment/weapon/rev/rev";
                                    PathImg2 = "c1.img";
                                    break;
                                case "revc2":
                                    PathImg1 = "character/gunner/equipment/weapon/rev/rev";
                                    PathImg2 = "c2.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        case "at mage":
                            switch (MuLuMing)
                            {
                                case "shoes_c":
                                    PathImg1 = "character/mage/atequipment/avatar/shoes/mm_shoes";
                                    PathImg2 = "c.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/mage/atequipment/avatar/shoes/mm_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "neck_kf":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "kf.img";
                                    break;
                                case "neck_df":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "df.img";
                                    break;
                                case "neck_bf":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "bf.img";
                                    break;
                                case "neck_xf":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "xf.img";
                                    break;
                                case "neck_ef":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "ef.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "coat_h":
                                    PathImg1 = "character/mage/atequipment/avatar/coat/mm_coat";
                                    PathImg2 = "h.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/mage/atequipment/avatar/coat/mm_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/mage/atequipment/avatar/coat/mm_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_x":
                                    PathImg1 = "character/mage/atequipment/avatar/coat/mm_coat";
                                    PathImg2 = "x.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/mage/atequipment/avatar/cap/mm_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/mage/atequipment/avatar/cap/mm_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/mage/atequipment/avatar/belt/mm_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "belt_e":
                                    PathImg1 = "character/mage/atequipment/avatar/belt/mm_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_a":
                                    PathImg1 = "character/mage/atequipment/avatar/belt/mm_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/mage/atequipment/avatar/belt/mm_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/mage/atequipment/avatar/belt/mm_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/mage/atequipment/avatar/belt/mm_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/mage/atequipment/avatar/cap/mm_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/mage/atequipment/avatar/cap/mm_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/mage/atequipment/avatar/cap/mm_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/mage/atequipment/avatar/coat/mm_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/mage/atequipment/avatar/coat/mm_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/mage/atequipment/avatar/coat/mm_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/mage/atequipment/avatar/coat/mm_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/mage/atequipment/avatar/face/mm_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/mage/atequipment/avatar/face/mm_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/mage/atequipment/avatar/face/mm_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/mage/atequipment/avatar/hair/mm_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/mage/atequipment/avatar/hair/mm_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/mage/atequipment/avatar/hair/mm_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/mage/atequipment/avatar/neck/mm_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/mage/atequipment/avatar/pants/mm_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/mage/atequipment/avatar/pants/mm_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/mage/atequipment/avatar/pants/mm_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/mage/atequipment/avatar/shoes/mm_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/mage/atequipment/avatar/shoes/mm_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "at_brooma":
                                    PathImg1 = "character/mage/atequipment/weapon/broom/mm_broom";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_brooma1":
                                    PathImg1 = "character/mage/atequipment/weapon/broom/mm_broom";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_brooma2":
                                    PathImg1 = "character/mage/atequipment/weapon/broom/mm_broom";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_broomd":
                                    PathImg1 = "character/mage/atequipment/weapon/broom/mm_broom";
                                    PathImg2 = "d.img";
                                    break;
                                case "at_broomd1":
                                    PathImg1 = "character/mage/atequipment/weapon/broom/mm_broom";
                                    PathImg2 = "d1.img";
                                    break;
                                case "at_broomd2":
                                    PathImg1 = "character/mage/atequipment/weapon/broom/mm_broom";
                                    PathImg2 = "d2.img";
                                    break;
                                case "at_cintsticka":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_cintstickd":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "d.img";
                                    break;
                                case "at_dakshaa":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_dakshad":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "d.img";
                                    break;
                                case "at_polea":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_polea1":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_polea2":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_poled":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "d.img";
                                    break;
                                case "at_poled1":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "d1.img";
                                    break;
                                case "at_poled2":
                                    PathImg1 = "character/mage/atequipment/weapon/pole/mm_pole";
                                    PathImg2 = "d2.img";
                                    break;
                                case "at_roda":
                                    PathImg1 = "character/mage/atequipment/weapon/rod/mm_rod";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_roda1":
                                    PathImg1 = "character/mage/atequipment/weapon/rod/mm_rod";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_roda2":
                                    PathImg1 = "character/mage/atequipment/weapon/rod/mm_rod";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_rodd":
                                    PathImg1 = "character/mage/atequipment/weapon/rod/mm_rod";
                                    PathImg2 = "d.img";
                                    break;
                                case "at_rodd1":
                                    PathImg1 = "character/mage/atequipment/weapon/rod/mm_rod";
                                    PathImg2 = "d1.img";
                                    break;
                                case "at_rodd2":
                                    PathImg1 = "character/mage/atequipment/weapon/rod/mm_rod";
                                    PathImg2 = "d2.img";
                                    break;
                                case "at_beamspeara1":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_beamspeara2":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_beamspeard1":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "d1.img";
                                    break;
                                case "at_beamspeard2":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "d2.img";
                                    break;
                                case "at_lancea":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_lanced":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "d.img";
                                    break;
                                case "at_speara":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_speara1":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_speara2":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_speard":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "d.img";
                                    break;
                                case "at_speard1":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "d1.img";
                                    break;
                                case "at_speard2":
                                    PathImg1 = "character/mage/atequipment/weapon/spear/mm_spear";
                                    PathImg2 = "d2.img";
                                    break;
                                case "at_staffa":
                                    PathImg1 = "character/mage/atequipment/weapon/staff/mm_staff";
                                    PathImg2 = "a.img";
                                    break;
                                case "at_staffa1":
                                    PathImg1 = "character/mage/atequipment/weapon/staff/mm_staff";
                                    PathImg2 = "a1.img";
                                    break;
                                case "at_staffa2":
                                    PathImg1 = "character/mage/atequipment/weapon/staff/mm_staff";
                                    PathImg2 = "a2.img";
                                    break;
                                case "at_staffd":
                                    PathImg1 = "character/mage/atequipment/weapon/staff/mm_staff";
                                    PathImg2 = "d.img";
                                    break;
                                case "at_staffd1":
                                    PathImg1 = "character/mage/atequipment/weapon/staff/mm_staff";
                                    PathImg2 = "d1.img";
                                    break;
                                case "at_staffd2":
                                    PathImg1 = "character/mage/atequipment/weapon/staff/mm_staff";
                                    PathImg2 = "d2.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        case "mage":
                            switch (MuLuMing)
                            {
                                case "neck_xf":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "xf.img";
                                    break;
                                case "neck_df":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "df.img";
                                    break;
                                case "neck_cf":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "cf.img";
                                    break;
                                case "neck_bf":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "bf.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "belt_a":
                                    PathImg1 = "character/mage/equipment/avatar/belt/mg_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/mage/equipment/avatar/belt/mg_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/mage/equipment/avatar/belt/mg_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/mage/equipment/avatar/belt/mg_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_e":
                                    PathImg1 = "character/mage/equipment/avatar/belt/mg_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/mage/equipment/avatar/belt/mg_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "belt_g":
                                    PathImg1 = "character/mage/equipment/avatar/belt/mg_belt";
                                    PathImg2 = "g.img";
                                    break;
                                case "belt_h":
                                    PathImg1 = "character/mage/equipment/avatar/belt/mg_belt";
                                    PathImg2 = "h.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/mage/equipment/avatar/cap/mg_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/mage/equipment/avatar/cap/mg_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/mage/equipment/avatar/cap/mg_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/mage/equipment/avatar/cap/mg_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_e":
                                    PathImg1 = "character/mage/equipment/avatar/cap/mg_cap";
                                    PathImg2 = "e.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/mage/equipment/avatar/cap/mg_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "cap_g":
                                    PathImg1 = "character/mage/equipment/avatar/cap/mg_cap";
                                    PathImg2 = "g.img";
                                    break;
                                case "cap_h":
                                    PathImg1 = "character/mage/equipment/avatar/cap/mg_cap";
                                    PathImg2 = "h.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/mage/equipment/avatar/coat/mg_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/mage/equipment/avatar/coat/mg_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/mage/equipment/avatar/coat/mg_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/mage/equipment/avatar/coat/mg_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "coat_e":
                                    PathImg1 = "character/mage/equipment/avatar/coat/mg_coat";
                                    PathImg2 = "e.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/mage/equipment/avatar/coat/mg_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/mage/equipment/avatar/coat/mg_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_h":
                                    PathImg1 = "character/mage/equipment/avatar/coat/mg_coat";
                                    PathImg2 = "h.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/mage/equipment/avatar/face/mg_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/mage/equipment/avatar/face/mg_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/mage/equipment/avatar/face/mg_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "face_d":
                                    PathImg1 = "character/mage/equipment/avatar/face/mg_face";
                                    PathImg2 = "d.img";
                                    break;
                                case "face_e":
                                    PathImg1 = "character/mage/equipment/avatar/face/mg_face";
                                    PathImg2 = "e.img";
                                    break;
                                case "face_f":
                                    PathImg1 = "character/mage/equipment/avatar/face/mg_face";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_g":
                                    PathImg1 = "character/mage/equipment/avatar/face/mg_face";
                                    PathImg2 = "g.img";
                                    break;
                                case "face_h":
                                    PathImg1 = "character/mage/equipment/avatar/face/mg_face";
                                    PathImg2 = "h.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/mage/equipment/avatar/hair/mg_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/mage/equipment/avatar/hair/mg_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_c":
                                    PathImg1 = "character/mage/equipment/avatar/hair/mg_hair";
                                    PathImg2 = "c.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/mage/equipment/avatar/hair/mg_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "hair_e":
                                    PathImg1 = "character/mage/equipment/avatar/hair/mg_hair";
                                    PathImg2 = "e.img";
                                    break;
                                case "hair_f":
                                    PathImg1 = "character/mage/equipment/avatar/hair/mg_hair";
                                    PathImg2 = "f.img";
                                    break;
                                case "hair_f1":
                                    PathImg1 = "character/mage/equipment/avatar/hair/mg_hair";
                                    PathImg2 = "f1.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_f":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "f.img";
                                    break;
                                case "neck_g":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "g.img";
                                    break;
                                case "neck_h":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "h.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "neck_z":
                                    PathImg1 = "character/mage/equipment/avatar/neck/mg_neck";
                                    PathImg2 = "z.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/mage/equipment/avatar/pants/mg_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/mage/equipment/avatar/pants/mg_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_c":
                                    PathImg1 = "character/mage/equipment/avatar/pants/mg_pants";
                                    PathImg2 = "c.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/mage/equipment/avatar/pants/mg_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "pants_e":
                                    PathImg1 = "character/mage/equipment/avatar/pants/mg_pants";
                                    PathImg2 = "e.img";
                                    break;
                                case "pants_f":
                                    PathImg1 = "character/mage/equipment/avatar/pants/mg_pants";
                                    PathImg2 = "f.img";
                                    break;
                                case "pants_g":
                                    PathImg1 = "character/mage/equipment/avatar/pants/mg_pants";
                                    PathImg2 = "g.img";
                                    break;
                                case "pants_h":
                                    PathImg1 = "character/mage/equipment/avatar/pants/mg_pants";
                                    PathImg2 = "h.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/mage/equipment/avatar/shoes/mg_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/mage/equipment/avatar/shoes/mg_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "shoes_c":
                                    PathImg1 = "character/mage/equipment/avatar/shoes/mg_shoes";
                                    PathImg2 = "c.img";
                                    break;
                                case "shoes_d":
                                    PathImg1 = "character/mage/equipment/avatar/shoes/mg_shoes";
                                    PathImg2 = "d.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/mage/equipment/avatar/shoes/mg_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "shoes_g":
                                    PathImg1 = "character/mage/equipment/avatar/shoes/mg_shoes";
                                    PathImg2 = "g.img";
                                    break;
                                case "shoes_h":
                                    PathImg1 = "character/mage/equipment/avatar/shoes/mg_shoes";
                                    PathImg2 = "h.img";
                                    break;
                                case "broomc":
                                    PathImg1 = "character/mage/equipment/weapon/broom/mg_broom";
                                    PathImg2 = "c.img";
                                    break;
                                case "broomc1":
                                    PathImg1 = "character/mage/equipment/weapon/broom/mg_broom";
                                    PathImg2 = "c1.img";
                                    break;
                                case "broomc2":
                                    PathImg1 = "character/mage/equipment/weapon/broom/mg_broom";
                                    PathImg2 = "c2.img";
                                    break;
                                case "broomd":
                                    PathImg1 = "character/mage/equipment/weapon/broom/mg_broom";
                                    PathImg2 = "d.img";
                                    break;
                                case "broomd1":
                                    PathImg1 = "character/mage/equipment/weapon/broom/mg_broom";
                                    PathImg2 = "d1.img";
                                    break;
                                case "broomd2":
                                    PathImg1 = "character/mage/equipment/weapon/broom/mg_broom";
                                    PathImg2 = "d2.img";
                                    break;
                                case "cintstickc":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "c.img";
                                    break;
                                case "cintstickd":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "d.img";
                                    break;
                                case "dakshac":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "c.img";
                                    break;
                                case "dakshad":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "d.img";
                                    break;
                                case "polec":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "c.img";
                                    break;
                                case "polec1":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "c1.img";
                                    break;
                                case "polec2":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "c2.img";
                                    break;
                                case "poled":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "d.img";
                                    break;
                                case "poled1":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "d1.img";
                                    break;
                                case "poled2":
                                    PathImg1 = "character/mage/equipment/weapon/pole/mg_pole";
                                    PathImg2 = "d2.img";
                                    break;
                                case "rodc":
                                    PathImg1 = "character/mage/equipment/weapon/rod/mg_rod";
                                    PathImg2 = "c.img";
                                    break;
                                case "rodc1":
                                    PathImg1 = "character/mage/equipment/weapon/rod/mg_rod";
                                    PathImg2 = "c1.img";
                                    break;
                                case "rodc2":
                                    PathImg1 = "character/mage/equipment/weapon/rod/mg_rod";
                                    PathImg2 = "c2.img";
                                    break;
                                case "rodd":
                                    PathImg1 = "character/mage/equipment/weapon/rod/mg_rod";
                                    PathImg2 = "d.img";
                                    break;
                                case "rodd1":
                                    PathImg1 = "character/mage/equipment/weapon/rod/mg_rod";
                                    PathImg2 = "d1.img";
                                    break;
                                case "rodd2":
                                    PathImg1 = "character/mage/equipment/weapon/rod/mg_rod";
                                    PathImg2 = "d2.img";
                                    break;
                                case "beamspearc1":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "c1.img";
                                    break;
                                case "beamspearc2":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "c2.img";
                                    break;
                                case "beamspeard1":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "d1.img";
                                    break;
                                case "beamspeard2":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "d2.img";
                                    break;
                                case "lancec":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "c.img";
                                    break;
                                case "lanced":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "d.img";
                                    break;
                                case "spearc":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "c.img";
                                    break;
                                case "spearc1":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "c1.img";
                                    break;
                                case "spearc2":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "c2.img";
                                    break;
                                case "speard":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "d.img";
                                    break;
                                case "speard1":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "d1.img";
                                    break;
                                case "speard2":
                                    PathImg1 = "character/mage/equipment/weapon/spear/mg_spear";
                                    PathImg2 = "d2.img";
                                    break;
                                case "staffc":
                                    PathImg1 = "character/mage/equipment/weapon/staff/mg_staff";
                                    PathImg2 = "c.img";
                                    break;
                                case "staffc1":
                                    PathImg1 = "character/mage/equipment/weapon/staff/mg_staff";
                                    PathImg2 = "c1.img";
                                    break;
                                case "staffc2":
                                    PathImg1 = "character/mage/equipment/weapon/staff/mg_staff";
                                    PathImg2 = "c2.img";
                                    break;
                                case "staffd":
                                    PathImg1 = "character/mage/equipment/weapon/staff/mg_staff";
                                    PathImg2 = "d.img";
                                    break;
                                case "staffd1":
                                    PathImg1 = "character/mage/equipment/weapon/staff/mg_staff";
                                    PathImg2 = "d1.img";
                                    break;
                                case "staffd2":
                                    PathImg1 = "character/mage/equipment/weapon/staff/mg_staff";
                                    PathImg2 = "d2.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        case "priest":
                            switch (MuLuMing)
                            {
                                case "neck_kf":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "kf.img";
                                    break;
                                case "neck_cf":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "cf.img";
                                    break;
                                case "neck_ef":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "ef.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "coat_x":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "x.img";
                                    break;
                                case "coat_h":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "h.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "belt_a":
                                    PathImg1 = "character/priest/equipment/avatar/belt/pr_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/priest/equipment/avatar/belt/pr_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/priest/equipment/avatar/belt/pr_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/priest/equipment/avatar/belt/pr_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_e":
                                    PathImg1 = "character/priest/equipment/avatar/belt/pr_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/priest/equipment/avatar/belt/pr_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/priest/equipment/avatar/cap/pr_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/priest/equipment/avatar/cap/pr_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/priest/equipment/avatar/cap/pr_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/priest/equipment/avatar/cap/pr_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_e":
                                    PathImg1 = "character/priest/equipment/avatar/cap/pr_cap";
                                    PathImg2 = "e.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/priest/equipment/avatar/cap/pr_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "cap_g":
                                    PathImg1 = "character/priest/equipment/avatar/cap/pr_cap";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "coat_e":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "e.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/priest/equipment/avatar/coat/pr_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/priest/equipment/avatar/face/pr_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/priest/equipment/avatar/face/pr_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/priest/equipment/avatar/face/pr_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "face_d":
                                    PathImg1 = "character/priest/equipment/avatar/face/pr_face";
                                    PathImg2 = "d.img";
                                    break;
                                case "face_e":
                                    PathImg1 = "character/priest/equipment/avatar/face/pr_face";
                                    PathImg2 = "e.img";
                                    break;
                                case "face_f":
                                    PathImg1 = "character/priest/equipment/avatar/face/pr_face";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_g":
                                    PathImg1 = "character/priest/equipment/avatar/face/pr_face";
                                    PathImg2 = "g.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/priest/equipment/avatar/hair/pr_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/priest/equipment/avatar/hair/pr_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_c":
                                    PathImg1 = "character/priest/equipment/avatar/hair/pr_hair";
                                    PathImg2 = "c.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/priest/equipment/avatar/hair/pr_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "hair_e":
                                    PathImg1 = "character/priest/equipment/avatar/hair/pr_hair";
                                    PathImg2 = "e.img";
                                    break;
                                case "hair_f":
                                    PathImg1 = "character/priest/equipment/avatar/hair/pr_hair";
                                    PathImg2 = "f.img";
                                    break;
                                case "hair_f1":
                                    PathImg1 = "character/priest/equipment/avatar/hair/pr_hair";
                                    PathImg2 = "f1.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_f":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "f.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "neck_z":
                                    PathImg1 = "character/priest/equipment/avatar/neck/pr_neck";
                                    PathImg2 = "z.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/priest/equipment/avatar/pants/pr_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/priest/equipment/avatar/pants/pr_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_c":
                                    PathImg1 = "character/priest/equipment/avatar/pants/pr_pants";
                                    PathImg2 = "c.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/priest/equipment/avatar/pants/pr_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "pants_e":
                                    PathImg1 = "character/priest/equipment/avatar/pants/pr_pants";
                                    PathImg2 = "e.img";
                                    break;
                                case "pants_f":
                                    PathImg1 = "character/priest/equipment/avatar/pants/pr_pants";
                                    PathImg2 = "f.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/priest/equipment/avatar/shoes/pr_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/priest/equipment/avatar/shoes/pr_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "shoes_c":
                                    PathImg1 = "character/priest/equipment/avatar/shoes/pr_shoes";
                                    PathImg2 = "c.img";
                                    break;
                                case "shoes_d":
                                    PathImg1 = "character/priest/equipment/avatar/shoes/pr_shoes";
                                    PathImg2 = "d.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/priest/equipment/avatar/shoes/pr_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "axec":
                                    PathImg1 = "character/priest/equipment/weapon/axe/pr_axe";
                                    PathImg2 = "c.img";
                                    break;
                                case "axec1":
                                    PathImg1 = "character/priest/equipment/weapon/axe/pr_axe";
                                    PathImg2 = "c1.img";
                                    break;
                                case "axec2":
                                    PathImg1 = "character/priest/equipment/weapon/axe/pr_axe";
                                    PathImg2 = "c2.img";
                                    break;
                                case "axed":
                                    PathImg1 = "character/priest/equipment/weapon/axe/pr_axe";
                                    PathImg2 = "d.img";
                                    break;
                                case "axed1":
                                    PathImg1 = "character/priest/equipment/weapon/axe/pr_axe";
                                    PathImg2 = "d1.img";
                                    break;
                                case "axed2":
                                    PathImg1 = "character/priest/equipment/weapon/axe/pr_axe";
                                    PathImg2 = "d2.img";
                                    break;
                                case "crossc":
                                    PathImg1 = "character/priest/equipment/weapon/cross/pr_cross";
                                    PathImg2 = "c.img";
                                    break;
                                case "crossc1":
                                    PathImg1 = "character/priest/equipment/weapon/cross/pr_cross";
                                    PathImg2 = "c1.img";
                                    break;
                                case "crossc2":
                                    PathImg1 = "character/priest/equipment/weapon/cross/pr_cross";
                                    PathImg2 = "c2.img";
                                    break;
                                case "crossd":
                                    PathImg1 = "character/priest/equipment/weapon/cross/pr_cross";
                                    PathImg2 = "d.img";
                                    break;
                                case "crossd1":
                                    PathImg1 = "character/priest/equipment/weapon/cross/pr_cross";
                                    PathImg2 = "d1.img";
                                    break;
                                case "crossd2":
                                    PathImg1 = "character/priest/equipment/weapon/cross/pr_cross";
                                    PathImg2 = "d2.img";
                                    break;
                                case "rosaryc":
                                    PathImg1 = "character/priest/equipment/weapon/rosary/pr_rosary";
                                    PathImg2 = "c.img";
                                    break;
                                case "rosaryc1":
                                    PathImg1 = "character/priest/equipment/weapon/rosary/pr_rosary";
                                    PathImg2 = "c1.img";
                                    break;
                                case "rosaryc2":
                                    PathImg1 = "character/priest/equipment/weapon/rosary/pr_rosary";
                                    PathImg2 = "c2.img";
                                    break;
                                case "rosaryd":
                                    PathImg1 = "character/priest/equipment/weapon/rosary/pr_rosary";
                                    PathImg2 = "d.img";
                                    break;
                                case "rosaryd1":
                                    PathImg1 = "character/priest/equipment/weapon/rosary/pr_rosary";
                                    PathImg2 = "d1.img";
                                    break;
                                case "rosaryd2":
                                    PathImg1 = "character/priest/equipment/weapon/rosary/pr_rosary";
                                    PathImg2 = "d2.img";
                                    break;
                                case "holyscythec1":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "c1.img";
                                    break;
                                case "holyscythec2":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "c2.img";
                                    break;
                                case "holyscythed1":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "d1.img";
                                    break;
                                case "holyscythed2":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "d2.img";
                                    break;
                                case "scythec":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "c.img";
                                    break;
                                case "scythec1":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "c1.img";
                                    break;
                                case "scythec2":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "c2.img";
                                    break;
                                case "scythed":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "d.img";
                                    break;
                                case "scythed1":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "d1.img";
                                    break;
                                case "scythed2":
                                    PathImg1 = "character/priest/equipment/weapon/scythe/pr_scythe";
                                    PathImg2 = "d2.img";
                                    break;
                                case "totemc":
                                    PathImg1 = "character/priest/equipment/weapon/totem/pr_totem";
                                    PathImg2 = "c.img";
                                    break;
                                case "totemc1":
                                    PathImg1 = "character/priest/equipment/weapon/totem/pr_totem";
                                    PathImg2 = "c1.img";
                                    break;
                                case "totemc2":
                                    PathImg1 = "character/priest/equipment/weapon/totem/pr_totem";
                                    PathImg2 = "c2.img";
                                    break;
                                case "totemd":
                                    PathImg1 = "character/priest/equipment/weapon/totem/pr_totem";
                                    PathImg2 = "d.img";
                                    break;
                                case "totemd1":
                                    PathImg1 = "character/priest/equipment/weapon/totem/pr_totem";
                                    PathImg2 = "d1.img";
                                    break;
                                case "totemd2":
                                    PathImg1 = "character/priest/equipment/weapon/totem/pr_totem";
                                    PathImg2 = "d2.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        case "swordman":
                            switch (MuLuMing)
                            {
                                case "belt_a":
                                    PathImg1 = "character/swordman/equipment/avatar/belt/sm_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/swordman/equipment/avatar/belt/sm_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/swordman/equipment/avatar/belt/sm_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/swordman/equipment/avatar/belt/sm_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_e":
                                    PathImg1 = "character/swordman/equipment/avatar/belt/sm_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/swordman/equipment/avatar/belt/sm_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "belt_h":
                                    PathImg1 = "character/swordman/equipment/avatar/belt/sm_belt";
                                    PathImg2 = "h.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/swordman/equipment/avatar/cap/sm_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/swordman/equipment/avatar/cap/sm_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/swordman/equipment/avatar/cap/sm_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/swordman/equipment/avatar/cap/sm_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_e":
                                    PathImg1 = "character/swordman/equipment/avatar/cap/sm_cap";
                                    PathImg2 = "e.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/swordman/equipment/avatar/cap/sm_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_x":
                                    PathImg1 = "character/swordman/equipment/avatar/coat/sm_coat";
                                    PathImg2 = "x.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/swordman/equipment/avatar/coat/sm_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/swordman/equipment/avatar/coat/sm_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/swordman/equipment/avatar/coat/sm_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/swordman/equipment/avatar/coat/sm_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "coat_e":
                                    PathImg1 = "character/swordman/equipment/avatar/coat/sm_coat";
                                    PathImg2 = "e.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/swordman/equipment/avatar/coat/sm_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/swordman/equipment/avatar/coat/sm_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/swordman/equipment/avatar/face/sm_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/swordman/equipment/avatar/face/sm_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/swordman/equipment/avatar/face/sm_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "face_d":
                                    PathImg1 = "character/swordman/equipment/avatar/face/sm_face";
                                    PathImg2 = "d.img";
                                    break;
                                case "face_e":
                                    PathImg1 = "character/swordman/equipment/avatar/face/sm_face";
                                    PathImg2 = "e.img";
                                    break;
                                case "face_f":
                                    PathImg1 = "character/swordman/equipment/avatar/face/sm_face";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_g":
                                    PathImg1 = "character/swordman/equipment/avatar/face/sm_face";
                                    PathImg2 = "g.img";
                                    break;
                                case "face_h":
                                    PathImg1 = "character/swordman/equipment/avatar/face/sm_face";
                                    PathImg2 = "h.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/swordman/equipment/avatar/hair/sm_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/swordman/equipment/avatar/hair/sm_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_c":
                                    PathImg1 = "character/swordman/equipment/avatar/hair/sm_hair";
                                    PathImg2 = "c.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/swordman/equipment/avatar/hair/sm_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "hair_e":
                                    PathImg1 = "character/swordman/equipment/avatar/hair/sm_hair";
                                    PathImg2 = "e.img";
                                    break;
                                case "hair_f":
                                    PathImg1 = "character/swordman/equipment/avatar/hair/sm_hair";
                                    PathImg2 = "f.img";
                                    break;
                                case "hair_f1":
                                    PathImg1 = "character/swordman/equipment/avatar/hair/sm_hair";
                                    PathImg2 = "f1.img";
                                    break;
                                case "neck_kf":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "kf.img";
                                    break;
                                case "neck_df":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "df.img";
                                    break;
                                case "neck_bf":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "bf.img";
                                    break;
                                case "neck_xf":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "xf.img";
                                    break;
                                case "neck_cf":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "cf.img";
                                    break;
                                case "neck_ef":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "ef.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_f":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "f.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "neck_z":
                                    PathImg1 = "character/swordman/equipment/avatar/neck/sm_neck";
                                    PathImg2 = "z.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/swordman/equipment/avatar/pants/sm_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/swordman/equipment/avatar/pants/sm_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_c":
                                    PathImg1 = "character/swordman/equipment/avatar/pants/sm_pants";
                                    PathImg2 = "c.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/swordman/equipment/avatar/pants/sm_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "pants_e":
                                    PathImg1 = "character/swordman/equipment/avatar/pants/sm_pants";
                                    PathImg2 = "e.img";
                                    break;
                                case "pants_f":
                                    PathImg1 = "character/swordman/equipment/avatar/pants/sm_pants";
                                    PathImg2 = "f.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/swordman/equipment/avatar/shoes/sm_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/swordman/equipment/avatar/shoes/sm_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "shoes_c":
                                    PathImg1 = "character/swordman/equipment/avatar/shoes/sm_shoes";
                                    PathImg2 = "c.img";
                                    break;
                                case "shoes_d":
                                    PathImg1 = "character/swordman/equipment/avatar/shoes/sm_shoes";
                                    PathImg2 = "d.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/swordman/equipment/avatar/shoes/sm_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "beamswdb":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "b.img";
                                    break;
                                case "beamswdb1":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "b1.img";
                                    break;
                                case "beamswdb2":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "b2.img";
                                    break;
                                case "beamswdb3":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "b1.img";
                                    break;
                                case "beamswdc":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "c.img";
                                    break;
                                case "beamswdc1":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "c1.img";
                                    break;
                                case "beamswdc2":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "c2.img";
                                    break;
                                case "cainusswdb1":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "b1.img";
                                    break;
                                case "cainusswdc1":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "c1.img";
                                    break;
                                case "hellkariumswdb1":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "b1.img";
                                    break;
                                case "hellkariumswdc1":
                                    PathImg1 = "character/swordman/equipment/weapon/beamswd/beamswd";
                                    PathImg2 = "c1.img";
                                    break;
                                case "clubb":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "b.img";
                                    break;
                                case "clubb1":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "b1.img";
                                    break;
                                case "clubb2":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "b2.img";
                                    break;
                                case "clubc":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "c.img";
                                    break;
                                case "clubc1":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "c1.img";
                                    break;
                                case "clubc2":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "c2.img";
                                    break;
                                case "clubd":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "d.img";
                                    break;
                                case "lclubb":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "b.img";
                                    break;
                                case "lclubc":
                                    PathImg1 = "character/swordman/equipment/weapon/club/club";
                                    PathImg2 = "c.img";
                                    break;
                                case "lswdb":
                                    PathImg1 = "character/swordman/equipment/weapon/lswd/lswd";
                                    PathImg2 = "b.img";
                                    break;
                                case "lswdb1":
                                    PathImg1 = "character/swordman/equipment/weapon/lswd/lswd";
                                    PathImg2 = "b1.img";
                                    break;
                                case "lswdb2":
                                    PathImg1 = "character/swordman/equipment/weapon/lswd/lswd";
                                    PathImg2 = "b2.img";
                                    break;
                                case "lswdc":
                                    PathImg1 = "character/swordman/equipment/weapon/lswd/lswd";
                                    PathImg2 = "c.img";
                                    break;
                                case "lswdc1":
                                    PathImg1 = "character/swordman/equipment/weapon/lswd/lswd";
                                    PathImg2 = "c1.img";
                                    break;
                                case "lswdc2":
                                    PathImg1 = "character/swordman/equipment/weapon/lswd/lswd";
                                    PathImg2 = "c2.img";
                                    break;
                                case "mswdb":
                                    PathImg1 = "character/swordman/equipment/weapon/mswd/mswd";
                                    PathImg2 = "b.img";
                                    break;
                                case "mswdc":
                                    PathImg1 = "character/swordman/equipment/weapon/mswd/mswd";
                                    PathImg2 = "c.img";
                                    break;
                                case "katanab":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "b.img";
                                    break;
                                case "katanab1":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "b1.img";
                                    break;
                                case "katanab2":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "b2.img";
                                    break;
                                case "katanac":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "c.img";
                                    break;
                                case "katanac1":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "c1.img";
                                    break;
                                case "katanac2":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "c2.img";
                                    break;
                                case "lkatanab":
                                    PathImg1 = "character/swordman/equipment/weapon/lkatana/lkatana";
                                    PathImg2 = "b.img";
                                    break;
                                case "lkatanac":
                                    PathImg1 = "character/swordman/equipment/weapon/lkatana/lkatana";
                                    PathImg2 = "c.img";
                                    break;
                                case "sasakib":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "b.img";
                                    break;
                                case "sasakic":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "c.img";
                                    break;
                                case "sayoungb":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "b.img";
                                    break;
                                case "sayoungc":
                                    PathImg1 = "character/swordman/equipment/weapon/katana/katana";
                                    PathImg2 = "c.img";
                                    break;
                                case "bantub":
                                    PathImg1 = "character/swordman/equipment/weapon/bantu/bantu";
                                    PathImg2 = "b.img";
                                    break;
                                case "bantuc":
                                    PathImg1 = "character/swordman/equipment/weapon/bantu/bantu";
                                    PathImg2 = "c.img";
                                    break;
                                case "bswdb":
                                    PathImg1 = "character/swordman/equipment/weapon/boneswd/bswd";
                                    PathImg2 = "b.img";
                                    break;
                                case "bswdc":
                                    PathImg1 = "character/swordman/equipment/weapon/boneswd/bswd";
                                    PathImg2 = "c.img";
                                    break;
                                case "gemswdb":
                                    PathImg1 = "character/swordman/equipment/weapon/gemswd/gemswd";
                                    PathImg2 = "b.img";
                                    break;
                                case "gemswdc":
                                    PathImg1 = "character/swordman/equipment/weapon/gemswd/gemswd";
                                    PathImg2 = "c.img";
                                    break;
                                case "lgswdb":
                                    PathImg1 = "character/swordman/equipment/weapon/lgswd/lgswd";
                                    PathImg2 = "b.img";
                                    break;
                                case "lgswdc":
                                    PathImg1 = "character/swordman/equipment/weapon/lgswd/lgswd";
                                    PathImg2 = "c.img";
                                    break;
                                case "sswdb":
                                    PathImg1 = "character/swordman/equipment/weapon/sswd/sswd";
                                    PathImg2 = "b.img";
                                    break;
                                case "sswdb1":
                                    PathImg1 = "character/swordman/equipment/weapon/sswd/sswd";
                                    PathImg2 = "b1.img";
                                    break;
                                case "sswdb2":
                                    PathImg1 = "character/swordman/equipment/weapon/sswd/sswd";
                                    PathImg2 = "b2.img";
                                    break;
                                case "sswdc":
                                    PathImg1 = "character/swordman/equipment/weapon/sswd/sswd";
                                    PathImg2 = "c.img";
                                    break;
                                case "sswdc1":
                                    PathImg1 = "character/swordman/equipment/weapon/sswd/sswd";
                                    PathImg2 = "c1.img";
                                    break;
                                case "sswdc2":
                                    PathImg1 = "character/swordman/equipment/weapon/sswd/sswd";
                                    PathImg2 = "c2.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        case "thief":
                            switch (MuLuMing)
                            {
                                case "belt_e":
                                    PathImg1 = "character/thief/equipment/avatar/belt/th_belt";
                                    PathImg2 = "e.img";
                                    break;
                                case "belt_a":
                                    PathImg1 = "character/thief/equipment/avatar/belt/th_belt";
                                    PathImg2 = "a.img";
                                    break;
                                case "belt_b":
                                    PathImg1 = "character/thief/equipment/avatar/belt/th_belt";
                                    PathImg2 = "b.img";
                                    break;
                                case "belt_c":
                                    PathImg1 = "character/thief/equipment/avatar/belt/th_belt";
                                    PathImg2 = "c.img";
                                    break;
                                case "belt_d":
                                    PathImg1 = "character/thief/equipment/avatar/belt/th_belt";
                                    PathImg2 = "d.img";
                                    break;
                                case "belt_f":
                                    PathImg1 = "character/thief/equipment/avatar/belt/th_belt";
                                    PathImg2 = "f.img";
                                    break;
                                case "belt_g":
                                    PathImg1 = "character/thief/equipment/avatar/belt/th_belt";
                                    PathImg2 = "g.img";
                                    break;
                                case "belt_h":
                                    PathImg1 = "character/thief/equipment/avatar/belt/th_belt";
                                    PathImg2 = "h.img";
                                    break;
                                case "cap_a":
                                    PathImg1 = "character/thief/equipment/avatar/cap/th_cap";
                                    PathImg2 = "a.img";
                                    break;
                                case "cap_b":
                                    PathImg1 = "character/thief/equipment/avatar/cap/th_cap";
                                    PathImg2 = "b.img";
                                    break;
                                case "cap_c":
                                    PathImg1 = "character/thief/equipment/avatar/cap/th_cap";
                                    PathImg2 = "c.img";
                                    break;
                                case "cap_d":
                                    PathImg1 = "character/thief/equipment/avatar/cap/th_cap";
                                    PathImg2 = "d.img";
                                    break;
                                case "cap_e":
                                    PathImg1 = "character/thief/equipment/avatar/cap/th_cap";
                                    PathImg2 = "e.img";
                                    break;
                                case "cap_f":
                                    PathImg1 = "character/thief/equipment/avatar/cap/th_cap";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_a":
                                    PathImg1 = "character/thief/equipment/avatar/coat/th_coat";
                                    PathImg2 = "a.img";
                                    break;
                                case "coat_b":
                                    PathImg1 = "character/thief/equipment/avatar/coat/th_coat";
                                    PathImg2 = "b.img";
                                    break;
                                case "coat_c":
                                    PathImg1 = "character/thief/equipment/avatar/coat/th_coat";
                                    PathImg2 = "c.img";
                                    break;
                                case "coat_d":
                                    PathImg1 = "character/thief/equipment/avatar/coat/th_coat";
                                    PathImg2 = "d.img";
                                    break;
                                case "coat_e":
                                    PathImg1 = "character/thief/equipment/avatar/coat/th_coat";
                                    PathImg2 = "e.img";
                                    break;
                                case "coat_f":
                                    PathImg1 = "character/thief/equipment/avatar/coat/th_coat";
                                    PathImg2 = "f.img";
                                    break;
                                case "coat_g":
                                    PathImg1 = "character/thief/equipment/avatar/coat/th_coat";
                                    PathImg2 = "g.img";
                                    break;
                                case "coat_h":
                                    PathImg1 = "character/thief/equipment/avatar/coat/th_coat";
                                    PathImg2 = "h.img";
                                    break;
                                case "face_a":
                                    PathImg1 = "character/thief/equipment/avatar/face/th_face";
                                    PathImg2 = "a.img";
                                    break;
                                case "face_b":
                                    PathImg1 = "character/thief/equipment/avatar/face/th_face";
                                    PathImg2 = "b.img";
                                    break;
                                case "face_c":
                                    PathImg1 = "character/thief/equipment/avatar/face/th_face";
                                    PathImg2 = "c.img";
                                    break;
                                case "face_f":
                                    PathImg1 = "character/thief/equipment/avatar/face/th_face";
                                    PathImg2 = "f.img";
                                    break;
                                case "face_g":
                                    PathImg1 = "character/thief/equipment/avatar/face/th_face";
                                    PathImg2 = "g.img";
                                    break;
                                case "face_h":
                                    PathImg1 = "character/thief/equipment/avatar/face/th_face";
                                    PathImg2 = "h.img";
                                    break;
                                case "hair_a":
                                    PathImg1 = "character/thief/equipment/avatar/hair/th_hair";
                                    PathImg2 = "a.img";
                                    break;
                                case "hair_b":
                                    PathImg1 = "character/thief/equipment/avatar/hair/th_hair";
                                    PathImg2 = "b.img";
                                    break;
                                case "hair_c":
                                    PathImg1 = "character/thief/equipment/avatar/hair/th_hair";
                                    PathImg2 = "c.img";
                                    break;
                                case "hair_d":
                                    PathImg1 = "character/thief/equipment/avatar/hair/th_hair";
                                    PathImg2 = "d.img";
                                    break;
                                case "hair_f1":
                                    PathImg1 = "character/thief/equipment/avatar/hair/th_hair";
                                    PathImg2 = "f1.img";
                                    break;
                                case "neck_df":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "df.img";
                                    break;
                                case "neck_xf":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "xf.img";
                                    break;
                                case "neck_ef":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "ef.img";
                                    break;
                                case "neck_k":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "k.img";
                                    break;
                                case "neck_a":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "a.img";
                                    break;
                                case "neck_b":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "b.img";
                                    break;
                                case "neck_c":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "c.img";
                                    break;
                                case "neck_d":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "d.img";
                                    break;
                                case "neck_d1":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "d1.img";
                                    break;
                                case "neck_e":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "e.img";
                                    break;
                                case "neck_h":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "h.img";
                                    break;
                                case "neck_f":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "f.img";
                                    break;
                                case "neck_x":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "x.img";
                                    break;
                                case "neck_x1":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "x1.img";
                                    break;
                                case "neck_z":
                                    PathImg1 = "character/thief/equipment/avatar/neck/th_neck";
                                    PathImg2 = "z.img";
                                    break;
                                case "pants_h":
                                    PathImg1 = "character/thief/equipment/avatar/pants/th_pants";
                                    PathImg2 = "h.img";
                                    break;
                                case "pants_a":
                                    PathImg1 = "character/thief/equipment/avatar/pants/th_pants";
                                    PathImg2 = "a.img";
                                    break;
                                case "pants_b":
                                    PathImg1 = "character/thief/equipment/avatar/pants/th_pants";
                                    PathImg2 = "b.img";
                                    break;
                                case "pants_d":
                                    PathImg1 = "character/thief/equipment/avatar/pants/th_pants";
                                    PathImg2 = "d.img";
                                    break;
                                case "pants_g":
                                    PathImg1 = "character/thief/equipment/avatar/pants/th_pants";
                                    PathImg2 = "g.img";
                                    break;
                                case "shoes_a":
                                    PathImg1 = "character/thief/equipment/avatar/shoes/th_shoes";
                                    PathImg2 = "a.img";
                                    break;
                                case "shoes_b":
                                    PathImg1 = "character/thief/equipment/avatar/shoes/th_shoes";
                                    PathImg2 = "b.img";
                                    break;
                                case "shoes_f":
                                    PathImg1 = "character/thief/equipment/avatar/shoes/th_shoes";
                                    PathImg2 = "f.img";
                                    break;
                                case "chakramb":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "b.img";
                                    break;
                                case "chakramc":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "c.img";
                                    break;
                                case "daggerc":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "c.img";
                                    break;
                                case "daggerc1":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "c1.img";
                                    break;
                                case "daggerc2":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "c2.img";
                                    break;
                                case "daggerd":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "d.img";
                                    break;
                                case "daggerd1":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "d1.img";
                                    break;
                                case "daggerd2":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "d2.img";
                                    break;
                                case "daggerx":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "x.img";
                                    break;
                                case "daggerx1":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "x1.img";
                                    break;
                                case "daggerx2":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "x2.img";
                                    break;
                                case "twinswordc":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "c.img";
                                    break;
                                case "twinswordc1":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "c1.img";
                                    break;
                                case "twinswordc2":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "c2.img";
                                    break;
                                case "twinswordd":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "d.img";
                                    break;
                                case "twinswordd1":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "d1.img";
                                    break;
                                case "twinswordd2":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "d2.img";
                                    break;
                                case "twinswordx":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "x.img";
                                    break;
                                case "twinswordx1":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "x1.img";
                                    break;
                                case "twinswordx2":
                                    PathImg1 = "character/thief/equipment/weapon/twinswd/twinswd";
                                    PathImg2 = "x2.img";
                                    break;
                                case "vajrab":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "b.img";
                                    break;
                                case "vajrac":
                                    PathImg1 = "character/thief/equipment/weapon/dagger/dagger";
                                    PathImg2 = "c.img";
                                    break;
                                case "wandc":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "c.img";
                                    break;
                                case "wandc1":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "c1.img";
                                    break;
                                case "wandc2":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "c2.img";
                                    break;
                                case "wandd":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "d.img";
                                    break;
                                case "wandd1":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "d1.img";
                                    break;
                                case "wandd2":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "d2.img";
                                    break;
                                case "wandx":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "x.img";
                                    break;
                                case "wandx1":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "x1.img";
                                    break;
                                case "wandx2":
                                    PathImg1 = "character/thief/equipment/weapon/wand/wand";
                                    PathImg2 = "x2.img";
                                    break;
                                default:
                                    PathImg1 = "";
                                    PathImg2 = "";
                                    break;
                            }
                            break;
                        default:
                            PathImg1 = "";
                            PathImg2 = "";
                            break;
                    }
                    #endregion

                    if (PathImg1 != "" && IdImg != "" && PathImg2 != "")
                    {
                        if (ImgPath.Contains("sprite/" + PathImg1 + IdImg + PathImg2) == false)
                        {
                            ImgPath.Add("sprite/" + PathImg1 + IdImg + PathImg2);
                        }
                    }
                    else
                    {
                        FanKuiCuoWu = 0;
                    }
                }

            }
            else//如果没有找到img图层；哪么默认是皮肤直接进行职业判断
            {
                switch (type)
                {
                    case "at fighter":
                        PathImg1 = "character/fighter/atequipment/avatar/skin/fm_body";
                        PathImg2 = ".img";
                        break;
                    case "fighter":
                        PathImg1 = "character/fighter/equipment/avatar/skin/ft_body";
                        PathImg2 = ".img";
                        break;
                    case "at gunner":
                        PathImg1 = "character/gunner/atequipment/avatar/skin/gg_body";
                        PathImg2 = ".img";
                        break;
                    case "gunner":
                        PathImg1 = "character/gunner/equipment/avatar/skin/gn_body";
                        PathImg2 = ".img";
                        break;
                    case "at mage":
                        PathImg1 = "character/mage/atequipment/avatar/skin/mm_body";
                        PathImg2 = ".img";
                        break;
                    case "mage":
                        PathImg1 = "character/mage/equipment/avatar/skin/mg_body";
                        PathImg2 = ".img";
                        break;
                    case "priest":
                        PathImg1 = "character/priest/equipment/avatar/skin/pr_body";
                        PathImg2 = ".img";
                        break;
                    case "swordman":
                        PathImg1 = "character/swordman/equipment/avatar/skin/sm_body";
                        PathImg2 = ".img";
                        break;
                    case "thief":
                        PathImg1 = "character/thief/equipment/avatar/skin/th_body";
                        PathImg2 = ".img";
                        break;
                    default:
                        PathImg1 = "";
                        PathImg2 = "";
                        break;
                }

                if (PathImg1 != "" && IdImg != "" && PathImg2 != "")
                {
                    if (ImgPath.Contains("sprite/" + PathImg1 + IdImg + PathImg2) == false)
                    {
                        ImgPath.Add("sprite/" + PathImg1 + IdImg + PathImg2);
                    }
                }
                else
                {
                    FanKuiCuoWu = 0;
                }

            }

            //判断完成后反馈错误信息；没有错误返回-1反之返回0
            return FanKuiCuoWu;

        }


        /// <summary>
        /// 寻找宠物蛋中的宠物equ编号
        /// </summary>
        /// <param name="equpath">被寻找的equ全路径集合</param>
        /// <param name="equ_id">返回加入的equ编号集合</param>
        public void Equ宠物蛋寻找宠物Equ(List<string> equpath, List<string> equ_id)
        {
            if (equpath.Count > 0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in equpath)
                {
                    if (FileExists(item))
                    {
                        string EquAllText = FileReadAllText(item);
                        regex.创建(EquAllText, @"\[output index\][\r\n]+([0-9]+)\t", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(equ_id, 1, false, 0);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// equ文件内寻找宠物cre编号
        /// </summary>
        /// <param name="equpath">要寻找的equ全路径</param>
        /// <param name="cre_id">返回加入的cre编号集合</param>
        public void Equ寻找Cre编号(List<string> equpath, List<string> cre_id)
        {
            if (equpath.Count > 0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in equpath)
                {
                    if (FileExists(item))
                    {
                        string EquAllText = FileReadAllText(item);
                        regex.创建(EquAllText, @"\[creature species\][\r\n]+([0-9]+)\t", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(cre_id, 1, false, 0);
                        }
                    }

                }
            }
        }


        /// <summary>
        /// 寻找宠物cre文件中的ani、atk、skl、ptl全路径
        /// </summary>
        /// <param name="crepath">被寻找的cre全路径</param>
        /// <param name="ani_path">返回加入的ani全路径集合</param>
        /// <param name="atk_path">返回加入的atk全路径集合</param>
        /// <param name="skl_path">返回加入的skl全路径集合</param>
        /// <param name="ptl_path">返回加入的ptl全路径集合</param>
        public void Cre寻找路径(List<string> crepath, List<string> ani_path, List<string> atk_path, List<string> skl_path, List<string> ptl_path)
        {
            if (crepath.Count > 0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in crepath)
                {
                    if (FileExists(item))
                    {
                        string CreAllText = FileReadAllText(item);
                        regex.创建(CreAllText, @"\[.+\][\r\n]+`(.+\.[a-z]+)`", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            string F_path = item.Replace("\\", "/");
                            F_path = F_path.Remove(F_path.LastIndexOf("/") + 1);
                            foreach (Match itmes in regex.正则返回集合())
                            {
                                string paths = F_path + itmes.Groups[1].Value.ToLower();
                                string HouZhui = paths.Substring(paths.Length - 3);
                                paths = 识别点点杠(paths, true);
                                switch (HouZhui)
                                {
                                    case "ani":
                                        if (ani_path != null)
                                        {
                                            if (ani_path.Contains(paths) == false)
                                            {
                                                ani_path.Add(paths);
                                            }
                                        }
                                        break;
                                    case "atk":
                                        if (atk_path != null)
                                        {
                                            if (atk_path.Contains(paths) == false)
                                            {
                                                atk_path.Add(paths);
                                            }
                                        }
                                        break;
                                    case "skl":
                                        if (skl_path != null)
                                        {
                                            if (skl_path.Contains(paths) == false)
                                            {
                                                skl_path.Add(paths);
                                            }
                                        }
                                        break;
                                    case "ptl":
                                        if (ptl_path != null)
                                        {
                                            if (ptl_path.Contains(paths) == false)
                                            {
                                                ptl_path.Add(paths);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 宠物cre的skl技能文件中查找ani、ptl路径以及obj编号
        /// </summary>
        /// <param name="skl_path">宠物cre的skl技能全路径</param>
        /// <param name="ani_path">返回加入的ani全路径集合</param>
        /// <param name="ptl_path">返回加入的ptl全路径集合</param>
        /// <param name="obj_id">返回加入的obj编号集合</param>
        public void CreSkl寻找路径或编号(List<string> skl_path, List<string> ani_path, List<string> ptl_path, List<string> obj_id)
        {
            if (skl_path.Count > 0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in skl_path)
                {
                    if (FileExists(item))
                    {
                        string CreSklAllText = FileReadAllText(item);
                        string F_Path = item.Replace("\\", "/");
                        F_Path = DirectoryGetParent(DirectoryGetParent(F_Path)) + "/";
                        if (ani_path != null)
                        {
                            regex.创建(CreSklAllText, @"`(.+\.[a-z]+)`", 1);
                            if (regex.取匹配数量() > 0)
                            {
                                foreach (Match items in regex.正则返回集合())
                                {
                                    string EndPath = F_Path + items.Groups[1].Value;
                                    string HouZhui = EndPath.Substring(EndPath.Length - 3).ToLower();
                                    EndPath = 识别点点杠(EndPath, true);
                                    switch (HouZhui)
                                    {
                                        case "ani":
                                            if (ani_path.Contains(EndPath) == false)
                                            {
                                                ani_path.Add(EndPath);
                                            }
                                            break;
                                        case "plt":
                                            if (ptl_path.Contains(EndPath) == false)
                                            {
                                                ptl_path.Add(EndPath);
                                            }
                                            break;
                                    }

                                }
                            }
                        }

                        if (obj_id != null)
                        {
                            regex.创建(CreSklAllText, @"\[passive\][\r\n]+(([\x20-\x7f]+\t){5}`.*`\r\n)+", 1);
                            if (regex.取匹配数量() > 0)
                            {
                                Regex_new regex1 = new Regex_new();
                                regex1.创建(regex.返回所有匹配文本(), @"([\x20-\x7f]+)\t([\x20-\x7f]+\t){4}`.*`", 1);
                                if (regex1.取匹配数量() > 0)
                                {
                                    regex1.子匹配加入集合(obj_id, 1, false, 0);
                                }

                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据提供的消耗品ID找出其中是礼包的ID跟路径
        /// </summary>
        /// <param name="pathexm">父路径例如：stackable/</param>
        /// <param name="XunZhaoStkId">被寻找的消耗品ID集合</param>
        /// <param name="StkLstAllText">整理过的lst列表集合</param>
        /// <param name="JiaRuStkId">返回加入找出的礼包ID</param>
        /// <param name="JiaRuStkPath">返回加入找出的礼包全路径</param>
        public void Id寻找Stk礼包(string pathexm, List<string> XunZhaoStkId, List<List<string>> StkLstAllText, List<string> JiaRuStkId, List<string> JiaRuStkPath)
        {
            if (XunZhaoStkId.Count > 0)
            {
                Regex_new regex = new Regex_new();

                foreach (string item in XunZhaoStkId)
                {
                    int pos = StkLstAllText[0].IndexOf(item);
                    if (pos != -1)
                    {
                        string FilePath = pathexm + StkLstAllText[2][pos];
                        if (FileExists(FilePath))
                        {
                            regex.创建(FileReadAllText(FilePath), @"\[stackable type\]\r\n`\[(.*booster.*|cera package|usable cera package|multi upgradable legacy|upgradable legacy|upgrade limit cube)\]`", 1);
                            if (regex.取匹配数量() > 0)
                            {
                                string XunZhaoDId = StkLstAllText[0][pos];
                                if (JiaRuStkId.Contains(XunZhaoDId) == false)
                                {
                                    JiaRuStkId.Add(XunZhaoDId);
                                }

                                if (JiaRuStkPath.Contains(FilePath) == false)
                                {
                                    JiaRuStkPath.Add(FilePath);
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据礼包全路径，找出所有给予的ID
        /// </summary>
        /// <param name="LinShiStkPath">被寻找的全路径集合</param>
        /// <param name="LinShiId">返回加入找到的ID</param>
        public void Stk礼包获取AllId(List<string> LinShiStkPath, List<string> LinShiId)
        {
            Regex_new regex = new Regex_new();

            foreach (string item in LinShiStkPath)
            {
                if (FileExists(item))
                {
                    string AllText = FileReadAllText(item);

                    regex.创建(AllText, @"\[stackable type\]\r\n`\[(.*booster.*|cera package|usable cera package|multi upgradable legacy|upgradable legacy|upgrade limit cube)\]`", 1);
                    if (regex.取匹配数量() > 0)
                    {
                        string Type = regex.取子匹配文本(1, 1);
                        switch (Type)
                        {
                            case "booster":
                            case "cera booster":
                            case "booster random":
                                regex.创建(AllText, @"\[(etc|cera|equipment|stackable|creature)\]\r\n[0-9]+\t(.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.创建(regex.返回所有子匹配文本(2), "([0-9]+)\t([0-9]+\t){2}", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }

                                regex.创建(AllText, @"\[(avatar|special avatar)\]\r\n[0-9]+\t(.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.创建(regex.返回所有子匹配文本(2), "([0-9]+)\t([0-9]+\t){4}", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }
                                break;
                            case "booster selection":
                                regex.创建(AllText, @"\[(etc|equipment|stackable)\]\r\n(.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.创建(regex.返回所有子匹配文本(2), "([0-9]+)\t[0-9]+\t", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }

                                regex.创建(AllText, @"\[avatar\]\r\n(.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.创建(regex.返回所有子匹配文本(1), "([0-9]+)\t([0-9]+\t){3}", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }
                                break;
                            case "cera package":
                            case "usable cera package":
                                regex.创建(AllText, @"\[(package data|package data selection|secret add item)\]\r\n([0-9]+\t.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.创建(regex.返回所有子匹配文本(2), "([0-9]+)\t[0-9]+\t", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }
                                break;
                            case "multi upgradable legacy":
                                regex.创建(AllText, @"\[random list\]\r\n[0-9]+\t([0-9]+)\t[0-9]+\t[0-9]+\t(.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                    regex.创建(regex.返回所有子匹配文本(2), "([0-9]+)\t([0-9]+\t){3}", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }
                                break;
                            case "upgradable legacy":
                                regex.创建(AllText, @"\[int data\]\r\n([0-9]+)\t[0-9]+\t([0-9]+\t.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                    regex.创建(regex.返回所有子匹配文本(2), "([0-9]+)\t([0-9]+\t){2}", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }
                                break;
                            case "upgrade limit cube":
                                regex.创建(AllText, @"\[A condition item\]\r\n([0-9]+\t.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.创建(regex.返回所有子匹配文本(1), "([0-9]+)\t[0-9]+\t", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }
                                regex.创建(AllText, @"\[result item\]\r\n([0-9]+\t.+\t)", 1);
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.创建(regex.返回所有子匹配文本(1), "([0-9]+)\t([0-9]+\t){2}", 1);
                                    regex.子匹配加入集合(LinShiId, 1, false, 0);
                                }
                                break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据提供的ID，返回加入字符串ID和名称；消耗品或装备
        /// </summary>
        /// <param name="pathex">父路径例如：stackable/</param>
        /// <param name="LinShiId">被寻找的编号集合</param>
        /// <param name="LstAllText">整理过的lst列表集合</param>
        /// <param name="EquStkId">返回并加入ID指向名称</param>
        public void IdBianBieType(string pathex, List<string> LinShiId, List<List<string>> LstAllText, StringBuilder EquStkId)
        {
            if (LinShiId.Count > 0)
            {
                Regex_new regex = new Regex_new();

                foreach (string item in LinShiId)
                {
                    int pos = LstAllText[0].IndexOf(item);
                    if (pos != -1)
                    {
                        string FilePath = pathex + LstAllText[2][pos];
                        if (FileExists(FilePath))
                        {
                            regex.创建(FileReadAllText(FilePath), @"\[name\]\r\n`([^`]+)`", 1);
                            if (regex.取匹配数量() > 0)
                            {
                                string Name = regex.取子匹配文本(1, 1).Replace("\r\n", " ");
                                EquStkId.Append(LstAllText[0][pos] + "\t" + Name + "\t\r\n");
                            }
                            else
                            {
                                EquStkId.Append(LstAllText[0][pos] + "\t\r\n");
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据提供的ID，返回加入字符串ID和名称；消耗品或装备
        /// </summary>
        /// <param name="pathex">父路径例如：stackable/</param>
        /// <param name="LinShiId">被寻找的编号集合</param>
        /// <param name="LstAllText">整理过的lst列表集合</param>
        /// <param name="EquStkId">返回并加入ID指向名称</param>
        /// <param name="New_StkLst">返回并加入额外消耗品lst列表</param>
        /// <param name="New_StkPath">返回并加入额外消耗品全路径</param>
        public void IdBianBieType(string pathex, List<string> LinShiId, List<List<string>> LstAllText, StringBuilder EquStkId, List<string> New_StkLst, List<string> New_StkPath)
        {
            if (LinShiId.Count > 0)
            {
                Regex_new regex = new Regex_new();

                foreach (string item in LinShiId)
                {
                    int pos = LstAllText[0].IndexOf(item);
                    if (pos != -1)
                    {
                        string FileLst = $"{LstAllText[0][pos]}\t`{LstAllText[1][pos]}`";
                        string FilePath = pathex + LstAllText[2][pos];
                        if (FileExists(FilePath))
                        {
                            regex.创建(FileReadAllText(FilePath), @"\[name\]\r\n`([^`]+)`", 1);
                            if (regex.取匹配数量() > 0)
                            {
                                string Name = regex.取子匹配文本(1, 1).Replace("\r\n", " ");
                                EquStkId.Append(LstAllText[0][pos] + "\t" + Name + "\t\r\n");
                            }
                            else
                            {
                                EquStkId.Append(LstAllText[0][pos] + "\t\r\n");
                            }
                        }

                        if (New_StkPath.Contains(FilePath) == false)
                            New_StkPath.Add(FilePath);

                        if (New_StkLst.Contains(FileLst) == false)
                            New_StkLst.Add(FileLst);
                    }
                }
            }
        }


        /// <summary>
        /// 根据提供的礼包ID，循环遍历出所有的礼包ID
        /// </summary>
        /// <param name="pathex">父路径例如：stackable/</param>
        /// <param name="StkId">礼包ID集合</param>
        /// <param name="StkLstAllText">整理过的lst列表集合</param>
        public void Stk礼包循环遍历StkId(string pathex, List<string> StkId, List<List<string>> StkLstAllText)
        {
            if (StkId.Count > 0)
            {
                List<string> LinShiStkId = new List<string>();
                List<string> LinShiStkPath = new List<string>();

                Id_lst_编号查找(pathex, StkLstAllText, StkId, null, LinShiStkPath);

                do
                {
                    Stk礼包获取AllId(LinShiStkPath, LinShiStkId);
                    LinShiStkPath.Clear();
                    Id寻找Stk礼包(pathex, LinShiStkId, StkLstAllText, StkId, LinShiStkPath);
                    LinShiStkId.Clear();

                } while (LinShiStkPath.Count > 0);

            }
        }


        /// <summary>
        /// 根据宝珠全路径，查找出卡片代码
        /// </summary>
        /// <param name="StkBaoZhuPath">被寻找的宝珠全路径集合</param>
        /// <param name="StkKaPianId">返回加入的卡片代码集合</param>
        public void Stk宝珠寻找卡片Id(List<string> StkBaoZhuPath, List<string> StkKaPianId)
        {
            if (StkBaoZhuPath.Count > 0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in StkBaoZhuPath)
                {
                    if (FileExists(item))
                    {
                        string AllText = FileReadAllText(item);
                        regex.创建(AllText, @"\[monster card id\]\r\n([0-9]+)\t", 1);
                        regex.子匹配加入集合(StkKaPianId, 1, false, 0);
                    }
                }
            }
        }


    }
}

