using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace PVFSimple.项目Dnf
{
    public class Dnf
    {

        //static readonly string YZ = Form1.MysqlYz.ChaXun(3);
        public string YZ = "\\";


        /// <summary>
        /// 验证文本是否为整数
        /// </summary>
        /// <param name="Text">被验证的文本</param>
        /// <returns></returns>
        public bool YesNoInt(string Text)
        {
            if(Regex.IsMatch(Text,@"^[\-0-9\.]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 返回父路径 以“\”为标准,例如：“obj\obj\obj.obj”返回“obj\obj”
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string DirectoryGetParent(string path)
        {
            path = path.Replace("/", YZ);
            path = path.Remove(path.LastIndexOf(YZ));

            return path;
        }


        /// <summary>
        /// 返回扩展名以“.”为标准,例如返回“.exe”
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
                if (Pos==-1)
                    continue;

                string Id = Text.Remove(Pos);
                string Path = "";
                if(Pos+1< Text.Length)
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

            List<string> FgEnd = new List<string>(LstAllText.Split(new char[] {' ','-','\t','\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));//开始拆分字符串

            while (YesNoInt(FgEnd[0]) ==false)
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
                List.Add(item.Substring(item.IndexOf("/") + 1));

            return List;
        }


        /// <summary>
        /// 由字符串检索进行分割 检索全路径按照编号进行排序或按照整行打乱顺序
        /// </summary>
        /// <param name="LstAllText">全路径文本</param>
        /// <param name="MoShi">0:正序排序/1：倒序排序/2：整行打乱排序</param>
        /// <returns></returns>
        public StringBuilder LstChongPaiLiePaiXu(string LstAllText,int MoShi)
        {
            SortedList<int,string> List = new SortedList<int, string>();
            StringBuilder FanHui = new StringBuilder();


            List<string> FgEnd = new List<string>(LstAllText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));//开始拆分字符串

            bool YesNoPvfFileTou = false;
            if (FgEnd[0] == "#PVF_File")
            {
                FgEnd.RemoveAt(0);
                YesNoPvfFileTou = true;
            }

            while (FgEnd[0].Contains("\t") == false)
                FgEnd.RemoveAt(0);


            switch (MoShi)
            {
                case 0:
                case 1:
                    foreach (string item in FgEnd)//得到所有被拆分的字符串，按照单数为编号  双数为路径加入到集合中
                    {
                        int Pos = item.IndexOf("\t");
                        if (Pos == -1)
                            continue;

                        int Id = Convert.ToInt32(item.Remove(Pos));
                        string Path = "";
                        if (List.ContainsKey(Id))
                        {
                            FanHui.Clear();
                            FanHui.Append($"请仔细检查提供重复的编号；重复编号为：{Id}");
                            return FanHui;
                        }
                        if (Pos + 1 < item.Length)
                            Path = item.Substring(Pos + 1);
                        List.Add(Id, Path);

                    }
                    if (MoShi==0)
                    {
                        foreach (int item in List.Keys)
                        {
                            FanHui.Append($"{item}\t{List[item]}\r\n");
                        }
                    }
                    else if(MoShi==1)
                    {
                        IList<int> LinShiDaoXu = List.Keys;
                        for (int i = LinShiDaoXu.Count-1; i >= 0 ; i--)
                        {
                            FanHui.Append($"{LinShiDaoXu[i]}\t{List[LinShiDaoXu[i]]}\r\n");
                        }
                    }
                    break;
                case 2:
                    Random random=new Random();

                    do
                    {
                        int LuanPos = random.Next(FgEnd.Count - 1);
                        FanHui.Append(FgEnd[LuanPos]+"\r\n");
                        FgEnd.RemoveAt(LuanPos);

                    } while (FgEnd.Count > 0);
                    break;
            }

            if (YesNoPvfFileTou)
                FanHui.Insert(0,"#PVF_File\r\n",1);

            return FanHui;
        }


        /// <summary>
        /// 由字符串检索进行分割 检索全路径分割编号跟路径键值
        /// </summary>
        /// <param name="LstAllText">全路径文本</param>
        /// <returns></returns>
        public Dictionary<string, string> LstSplit(string LstAllText,StringBuilder ChongFu)
        {
            Dictionary<string, string> List = new Dictionary<string, string>();

            List<string> FgEnd = new List<string>(LstAllText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));//开始拆分字符串

            while (FgEnd[0].Contains("\t") == false)
                FgEnd.RemoveAt(0);


            foreach (string item in FgEnd)
            {
                int Pos = item.IndexOf("\t");
                if (Pos == -1)
                    continue;

                string Id = item.Remove(Pos);
                string Path = "";

                if (Pos + 1 < item.Length)
                    Path = item.Substring(Pos+1);

                if(List.ContainsKey(Id))
                {
                    ChongFu.Append($"{Id}\t{Path}\r\n");
                }
                else
                {
                    List.Add(Id, Path);
                }

            }

            return List;
        }


        /// <summary>
        /// 寻找一个集合中重复的内容，并且返回给可变型字符串
        /// </summary>
        /// <param name="XunZhao">待寻找的集合</param>
        /// <param name="FanKui">反馈的内容</param>
        public void 寻找ChongFu(List<string>XunZhao,StringBuilder FanKui)
        {
            foreach (var item in XunZhao.GroupBy(s => s))
            {
                if (item.Count() > 1)
                    FanKui.Append(item.Key+"\r\n");
            }
        }


        /// <summary>
        /// 查找指定文件中的img路径
        /// </summary>
        /// <param name="path">查找的目录</param>
        /// <param name="扩展名">*.*代表所有 *.exe代表应用程序</param>
        /// <param name="去重复">false代表去重复 true代表不去重</param>
        /// <param name="npkimg_path">转成npk内的img全路径</param>
        public void 寻找img路径(string path,string 扩展名,StringBuilder npkimg_path)
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
                    if (Pos!=-1)
                    {
                        string F_Path = regex.替换("sprite/" + ImgPath.Remove(Pos).Replace("\\", "/"), "[/]{2,}|^[/]+", "");
                        for (int i = 1; i < 10; i++)
                        {
                            if (Imgalltext.Contains($"{F_Path}0{i}.img") == false)
                                Imgalltext.Add($"{F_Path}0{i}.img");
                        }
                        continue;
                    }

                    ImgPath = regex.替换("sprite/" + ImgPath.Replace("\\","/"), "[/]{2,}|^[/]+", "");
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
        /// <param name="path">提供的全路径如：C:\Users\MSI-NB\Desktop\map\../a.ani</param>
        /// <param name="是否为小写">true转为小写 false不转小写</param>
        /// <returns></returns>
        public string 识别点点杠(string path, bool 是否为小写)
        {
            if (path.Contains(@"...") == false)
            {
                path = path.Replace("/", @"\");
                if (path.Contains(@"..\"))
                {
                    Regex_new regex = new Regex_new();

                    regex.创建(path, @"(.+?)\\\.\.\\(\.\.\\)*(.+\.[a-z]+)", 0);
                    string f_path = regex.取子匹配文本(1, 1);
                    string z_path = regex.取子匹配文本(1, 3);
                    regex.创建(path, @"\.\.\\", 0);
                    for (int i = 0; i < regex.取匹配数量(); i++)
                    {
                        f_path = Convert.ToString(Directory.GetParent(f_path));
                    }

                    if (是否为小写 == true)
                    {
                        string fz_path = f_path + @"\" + z_path;
                        return fz_path.ToLower();
                    }
                    else
                    {
                        return f_path + @"\" + z_path;
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
        public void 寻找als(List<string>ani_path, List<string> als_path,int biglittle,bool repetition)
        {
            if (ani_path.Count>0)//判断所提供的ani全路径集合是不是空的
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
                        if (File.Exists(alspath) == true)//如果找到了als
                        {
                            //加入als
                            if (biglittle == 0)//如果参数是不转换大小写
                            {
                                if (repetition == true)//如果加入重复
                                {
                                    temporary_alspath.Add(alspath);
                                }
                                else//反之不加入重复
                                {
                                    if (temporary_alspath.Contains(alspath) == false)//判断不在集合中就加入
                                    {
                                        temporary_alspath.Add(alspath);
                                    }
                                }

                            }
                            else if (biglittle == 1)//如果参数是转换大写
                            {
                                if (repetition == true)//如果加入重复
                                {
                                    temporary_alspath.Add(alspath.ToUpper());
                                }
                                else//反之不加入重复
                                {
                                    string bigtxt = alspath.ToUpper();//转为大写 赋值给字符串变量
                                    if (temporary_alspath.Contains(bigtxt) == false)//判断不在集合中就加入
                                    {
                                        temporary_alspath.Add(bigtxt);
                                    }
                                }
                            }
                            else if (biglittle == 2)//如果参数是转换小写
                            {
                                if (repetition == true)//如果加入重复
                                {
                                    temporary_alspath.Add(alspath.ToLower());
                                }
                                else//反之不加入重复
                                {
                                    string littletxt = alspath.ToLower();//转为小写 赋值给字符串变量
                                    if (temporary_alspath.Contains(littletxt) == false)//判断不在集合中就加入
                                    {
                                        temporary_alspath.Add(littletxt);
                                    }
                                }
                            }
                        }
                    }

                    temporary_anipath.Clear();//临时ani清空掉

                    //把ani+als扩展名的文件路径加入到参数als中
                    temporary_alspath.ForEach((AlsPath) => {
                        if (!als_path.Contains(AlsPath))
                            als_path.Add(AlsPath);
                    });

                    //寻找als当中的ani全路径 加入到临时ani集合
                    foreach (var item in temporary_alspath)
                    {
                        string alspath = item;//als全路径
                        string alsalltxt = File.ReadAllText(alspath, Encoding.UTF8);//读入als所有文本
                        regex.创建(alsalltxt, @"\[use animation\]\r\n`(.+\.ani)`",1);//正则捕获ani路径
                        foreach (Match itemani in regex.正则返回集合())//枚举出所有捕获
                        {
                            string anipath = 识别点点杠(Directory.GetParent(alspath) + @"\" + itemani.Groups[1].Value,true);//取出子匹配去除../
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
        public void 寻找ai(List<string>ai_path)
        {
            //如果提供的ai全路径集合不为空
            if (ai_path.Count>0)
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
                    List<string> dirs = new List<string>(Directory.EnumerateFiles(item, "*.ai", SearchOption.AllDirectories));
                    ai_path.AddRange(dirs);
                }
            }
        }


        /// <summary>
        /// 根据编号查找lst以及全路径
        /// </summary>
        /// <param name="pathex">所在目录(目录下有dungeon目录)</param>
        /// <param name="ListLst">lstList集合</param>
        /// <param name="id">被寻找的编号集合</param>
        /// <param name="lst">返回加入的lst集合</param>
        /// <param name="path">返回加入的全路径集合</param>
        public void Id_lst_编号查找(string pathex, List<List<string>> ListLst, List<string> id, List<string> lst, List<string> path)
        {

            if (id.Count <= 0 || ListLst.Count <= 0) return;
                
            if (pathex!=null)
            {
                pathex = pathex.ToLower();
            }
            
            foreach (string item in id)
            {
                int pos = ListLst[0].IndexOf(item);
                if (pos!=-1)
                {
                    string LinShiPath = ListLst[2][pos];
                    string LstText = ListLst[0][pos] + "\t`" + ListLst[1][pos] + "`";
                    if (lst!=null)
                    {
                        if (lst.Contains(LstText) == false)
                        {
                            lst.Add(LstText);
                        }
                    }

                    if (path!=null)
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
        /// <param name="pathex">用于取到有用的路径</param>
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
                if (pos!=-1)
                {
                    string Id_List1 = ListLst[0][pos];
                    string YPath_List2 = ListLst[1][pos];
                    string TolPath_List3 = ListLst[2][pos];
                    string Id_Lst = Id_List1 + "\t`" + YPath_List2 + "`";

                    if (id != null)
                    {
                        if (id.Contains(Id_List1) ==false)
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
                        if (new_path.Contains(pathex+ TolPath_List3) == false)
                        {
                            new_path.Add(pathex + TolPath_List3);
                        }
                    }
                }
            }

            if (new_path.Count>0)
            {
                
                path.Clear();
                path.AddRange(new_path);
            }

        }


        /// <summary>
        /// 寻找文件中`内容`的文本，以扩展名方式加入对应集合
        /// </summary>
        /// <param name="path">全路径文本文件；utf8编码</param>
        /// <param name="tbl">tbl扩展名集合</param>
        /// <param name="act">act扩展名集合</param>
        /// <param name="atk">atk扩展名集合</param>
        /// <param name="ani">ani扩展名集合</param>
        /// <param name="ptl">ptl扩展名集合</param>
        /// <param name="til">til扩展名集合</param>
        /// <param name="key">key扩展名集合</param>
        /// <param name="ai">ai扩展名集合</param>
        /// <param name="equ">怪物equ扩展名集合</param>
        public void 寻找全路径(string pvf文件目录,List<string> path,List<string> tbl, List<string> act, List<string> atk, List<string> ani, List<string> ptl, List<string> til, List<string> key, List<string> ai, List<string> equ)
        {
            if (path.Count>0)
            {
                List<string> 临时act = new List<string>();

                if (pvf文件目录 != null)
                {
                    pvf文件目录 = pvf文件目录.ToLower();
                }
                
                //得到全路径文件的扩展名
                string exname = Path.GetExtension(path[0]).ToLower();

                foreach (string item in path)
                {
                    if (File.Exists(item))
                    {
                        string alltext = File.ReadAllText(item);
                        Regex_new regex = new Regex_new();
                        //测试中发现act换成h后缀也被识别哪么这里加一个后缀名
                        regex.创建(alltext, @"(`.+\.tbl`)|(`.+\.act`)|(`.+\.h`)|(`.+\.atk`)|(`.+\.ani`)|(`.+\.ptl`)|(`.+\.til`)|(`.+\.key`)|(`.+\.ai`)|(`.+\.equ`)", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            string f_path = DirectoryGetParent(item).ToLower();

                            List<string> regex_pp = new List<string>();
                            regex.匹配加入集合(regex_pp, false, 2);

                            foreach (var item2 in regex_pp)
                            {
                                string pp_path = item2.Replace("/","\\");
                                if (pp_path.Contains("...\\") == false)
                                {
                                    pp_path = pp_path.Replace("`", "").ToLower();
                                    //得到匹配的扩展名
                                    string kzm = Path.GetExtension(pp_path);
                                    string lins = "";

                                    switch (kzm)
                                    {
                                        case ".key":
                                            if (exname == ".aic" && key != null)
                                            {
                                                if (key.Contains(f_path + @"\key\" + pp_path) == false)
                                                {
                                                    key.Add(f_path + @"\key\" + pp_path);
                                                }
                                            }
                                            break;
                                        case ".ai":
                                            if (ai != null)
                                            {
                                                switch (exname)
                                                {
                                                    case ".aic":
                                                        if (pp_path.Replace("/", "\\").ToLower().Contains("aicharacter\\"))
                                                        {
                                                            if (ai.Contains(pvf文件目录+"\\" + pp_path) == false)
                                                            {
                                                                ai.Add(pvf文件目录 + "\\" + pp_path);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ai.Contains(f_path + @"\ai\" + pp_path) == false)
                                                            {
                                                                ai.Add(f_path + @"\ai\" + pp_path);
                                                            }
                                                        }
                                                        break;
                                                    case ".mob":
                                                    case ".ai":
                                                        lins = 识别点点杠(f_path + @"\" + pp_path, false);
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
                                                        if (tbl.Contains(pvf文件目录 + @"\" + pp_path) == false)
                                                        {
                                                            tbl.Add(pvf文件目录 + @"\" + pp_path);
                                                        }
                                                        break;
                                                    case ".mob":
                                                        if (tbl.Contains(pvf文件目录 + @"\monster\" + pp_path) == false)
                                                        {
                                                            tbl.Add(pvf文件目录 + @"\monster\" + pp_path);
                                                        }
                                                        break;
                                                }
                                            }
                                            break;
                                        case ".act":
                                        case ".h":
                                            if (act != null)
                                            {
                                                switch (exname)//正则匹配的扩展名
                                                {
                                                    case ".act":
                                                    case ".h":
                                                        lins = 识别点点杠(f_path + @"\" + pp_path, false);
                                                        if (act.Contains(lins) == false)
                                                        {
                                                            临时act.Add(lins);
                                                        }
                                                        break;
                                                    default:
                                                        lins = 识别点点杠(f_path + @"\" + pp_path, false);
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
                                                lins = 识别点点杠(f_path + @"\" + pp_path, false);
                                                if (atk.Contains(lins) == false)
                                                {
                                                    atk.Add(lins);
                                                }
                                            }
                                            break;
                                        case ".ani":
                                            if (ani != null)
                                            {
                                                lins = 识别点点杠(f_path + @"\" + pp_path, false);
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
                                                        lins = 识别点点杠(Directory.GetParent(f_path) + @"\" + pp_path, false);
                                                        if (ptl.Contains(lins) == false)
                                                        {
                                                            ptl.Add(lins);
                                                        }
                                                        break;
                                                    default:
                                                        lins = 识别点点杠(f_path + @"\" + pp_path, false);
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
                                                lins = 识别点点杠(f_path + @"\" + pp_path, false);
                                                if (til.Contains(lins) == false)
                                                {
                                                    til.Add(lins);
                                                }
                                            }
                                            break;
                                        case ".equ":
                                            if (equ != null)
                                            {
                                                if (exname == ".mob" && equ.Contains(pvf文件目录 + @"\equipment\" + pp_path) == false)
                                                {
                                                    equ.Add(pvf文件目录 + @"\equipment\" + pp_path);
                                                }
                                            }
                                            break;
                                    }
                                }
                                
                            }

                            if ( ani != null)
                            {
                                if(exname == ".act" || exname == ".h")
                                {
                                    regex.创建(alltext, @"\[ANI FILE\]\r\n`(.+\.ani)`", 1);
                                    if (regex.取匹配数量() > 0)
                                    {
                                        List<string> ui_path = new List<string>();
                                        regex.子匹配加入集合(ui_path, 1, false, 2);
                                        foreach (var item_ui in ui_path)
                                        {
                                            string uitxt = item_ui.Replace("/", @"\");

                                            if (ani.Contains(pvf文件目录 + @"\" + uitxt) == false)
                                            {
                                                ani.Add(pvf文件目录 + @"\" + uitxt);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (临时act.Count>0)
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
        /// <param name="path">所寻找的文件全路径 utf8编码</param>
        /// <param name="map">返回加入集合map编号</param>
        /// <param name="obj">返回加入集合obj编号</param>
        /// <param name="mob">返回加入集合mob编号</param>
        /// <param name="apc">返回加入集合apc编号</param>
        public void 寻找编号(List<string> path, List<string> map, List<string> obj, List<string> mob, List<string> apc)
        {
            if (path.Count>0)//判断所提供的集合是不是空的
            {
                Regex_new regex = new Regex_new();
                string exname = Path.GetExtension(path[0]).ToLower();//取出扩展名并且转为小写；方便判断查找编号

                foreach (var item in path)//循环只读所有全路径
                {
                    if (File.Exists(item))//判断所提供的全路径文件是否存在
                    {
                        string alltext = File.ReadAllText(item, Encoding.UTF8);//以utf8编码读入文件所有内容

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

                                    //国服新标签
                                    regex.创建(alltext, @"\[CREATE PASSIVEOBJECT CIRCLE\][\r\n]+\[INDEX\][\r\n]+([0-9]+)\t\r\n", 0);//这里默认匹配大小写
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(obj, 1, false, 0);
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
        /// <param name="path">PVF文件所在目录</param>
        /// <param name="lsttext">怪物lst文件内容</param>
        /// <param name="mob_id">怪物编号</param>
        /// <param name="mob_lst">怪物lst</param>
        /// <param name="mob_path">怪物全路径</param>
        /// <param name="obj_id">特效编号</param>
        /// <param name="apc_id">apc编号</param>
        /// <param name="act_path">act全路径</param>
        /// <param name="ptl_path">ptl全路径</param>
        public void 怪物寻找编号(string path, List<List<string>> lsttext,List<string> mob_id,List<string>mob_lst, List<string> mob_path, List<string> obj_id, List<string> apc_id, List<string> act_path, List<string> ptl_path)
        {

            Id_lst_编号查找(path, lsttext,mob_id,mob_lst,mob_path);

            寻找全路径(path,mob_path,null,act_path,null,null,null,null,null,null,null);

            if (act_path.Count >0)
            {
                int act_count;
                do
                {
                    act_count = act_path.Count;
                    寻找全路径(path, act_path, null, act_path, null, null, null, null, null, null, null);
                } while (act_count != act_path.Count);
            }

            寻找全路径(path, act_path, null, null, null, null, ptl_path, null, null, null, null);
            寻找全路径(path, mob_path, null, null, null, null, ptl_path, null, null, null, null);

            寻找编号(mob_path,null,obj_id,null,null);
            寻找编号(ptl_path, null,obj_id,null,null);
            寻找编号(act_path, null,obj_id, mob_id, apc_id);
        }


        /// <summary>
        /// 根据提供的怪物编号 查找出所有关联编号(mob,obj,apc)
        /// </summary>
        /// <param name="path">PVF文件所在目录</param>
        /// <param name="lsttext">特效lst文件内容</param>
        /// <param name="obj_id">特效编号</param>
        /// <param name="obj_lst">特效lst</param>
        /// <param name="obj_path">特效全路径</param>
        /// <param name="mob_id">怪物编号</param>
        /// <param name="apc_id">apc编号</param>
        /// <param name="act_path">act全路径</param>
        /// <param name="ptl_path">ptl全路径</param>
        public void 特效寻找编号(string path, List<List<string>> lsttext, List<string> obj_id, List<string> obj_lst,List<string> obj_path, List<string> mob_id, List<string> apc_id, List<string> act_path, List<string> ptl_path)
        {

            Id_lst_编号查找(path, lsttext, obj_id, obj_lst, obj_path);

            寻找全路径(path, obj_path, null, act_path, null, null, null, null, null, null, null);

            if (act_path.Count>0)
            {
                int act_count;
                do
                {
                    act_count = act_path.Count;
                    寻找全路径(path, act_path, null, act_path, null, null, null, null, null, null, null);
                } while (act_count != act_path.Count);
            }


            寻找全路径(path, act_path, null, null, null, null, ptl_path, null, null, null, null);
            寻找全路径(path, obj_path, null, null, null, null, ptl_path, null, null, null, null);

            寻找编号(obj_path, null, obj_id, null, null);
            寻找编号(ptl_path, null, obj_id, null, null);
            寻找编号(act_path, null, obj_id, mob_id, apc_id);

        }

        
        /// <summary>
        /// 根据提供的怪物编号 查找出所有关联编号(mob,obj,apc)
        /// </summary>
        /// <param name="path">PVF文件所在目录</param>
        /// <param name="lsttext">人偶lst文件内容</param>
        /// <param name="apc_id">人偶编号</param>
        /// <param name="apc_lst">人偶lst</param>
        /// <param name="mob_id">怪物编号</param>
        /// <param name="obj_id">特效编号</param>
        /// <param name="act_path">act全路径</param>
        /// <param name="ptl_path">ptl全路径</param>
        /// <param name="key_path">key全路径</param>
        public void 人偶寻找编号(string path, List<List<string>> lsttext, List<string> apc_id, List<string> apc_lst, List<string> apc_path, List<string> mob_id, List<string> obj_id, List<string> act_path, List<string> ptl_path,  List<string> key_path)
        {

            Id_lst_编号查找(path, lsttext, apc_id, apc_lst, apc_path);

            寻找全路径(path, apc_path, null, act_path, null, null, null, null, key_path, null, null);

            if (act_path.Count > 0)
            {
                int act_count;
                do
                {
                    act_count = act_path.Count;
                    寻找全路径(path, act_path, null, act_path, null, null, null, null, null, null, null);
                } while (act_count != act_path.Count);
            }


            寻找全路径(path, act_path, null, null, null, null, ptl_path, null, null, null, null);
            寻找全路径(path, key_path, null, null, null, null, ptl_path, null, null, null, null);

            寻找编号(ptl_path, null, obj_id, null, null);
            寻找编号(key_path, null, obj_id, mob_id, apc_id);
            寻找编号(act_path, null, obj_id, mob_id, apc_id);
        }


        /// <summary>
        /// 获取提供twn城镇文件中的ani map 全路径
        /// </summary>
        /// <param name="pvf所在目录">指定提取文件的PVF根目录</param>
        /// <param name="twnpath">城镇文件全路径集合</param>
        /// <param name="mappath">map文件全路径集合</param>
        /// <param name="anipath">ani文件全路径集合</param>
        public void Twn寻找map_ani(string pvf所在目录, List<string> twnpath, List<string> mappath,List<string> anipath,bool CNmap)
        {
            //定义国服map集合
            List<string> cnmap = new List<string>();

            //定义一个正则类
            Regex_new regex = new Regex_new();

            //如果提供的城镇文件路径集合存在
            if (twnpath.Count>0)
            {
                //枚举出所有路径
                foreach (string item in twnpath)
                {
                    //读入城镇文件内容
                    string alltext = File.ReadAllText(item);
                    //正则获取城镇文件内的所有map路径
                    regex.创建(alltext,@"`(.+\.map)`",1);
                    //如果匹配不为空
                    if (regex.取匹配数量()>0)
                    {
                        //枚举出所有匹配的map路径
                        foreach (Match mapitem in regex.正则返回集合())
                        {
                            //获取真实map全路径
                            string lsmap = pvf所在目录 + @"\map\" + mapitem.Groups[1].Value;
                            //map全路径替换斜杠，转为小写
                            lsmap = lsmap.Replace("/","\\").ToLower();
                            //如果文件存在
                            if (File.Exists(lsmap))
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
                                lsmap = lsmap.Insert(lsmap.LastIndexOf("\\") + 1, "(r)");
                                //如果文件存在
                                if (File.Exists(lsmap))
                                {
                                    //如果不在map集合就加入map路径集合
                                    if (mappath.Contains(lsmap) == false)
                                    {
                                        mappath.Add(lsmap);

                                        //如果确认提取的是国服map
                                        if (CNmap == true)
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
                    regex.创建(alltext,@"`(.+\.ani)`",1);
                    //如果找到了
                    if (regex.取匹配数量()>0)
                    {
                        //枚举出所有找到的ani
                        foreach (Match aniitem in regex.正则返回集合())
                        {
                            //取出当前父路径，取子匹配文本，识别点点杠，转为小写
                            string lsani = Directory.GetParent(item.ToString()).ToString() + "\\" + aniitem.Groups[1].Value;
                            lsani = lsani.Replace("/","\\");
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
                if (cnmap.Count>0)
                {
                    //枚举出所有查找出来的map全路径
                    foreach (string item in cnmap)
                    {
                        //获取map文件内容
                        string alltext = File.ReadAllText(item);
                        //正则创建捕获map中的map
                        regex.创建(alltext, @"\[import script\]\r\n`(.+\.map)`",1);
                        //如果捕获到
                        if (regex.取匹配数量()>0)
                        {
                            //获取查找到的map全路径
                            string lscnmap = pvf所在目录 + @"\map\" + regex.取子匹配文本(1, 1);
                            //转为小写
                            lscnmap = lscnmap.Replace("/","\\").ToLower();
                            //如果文件存在
                            if (File.Exists(lscnmap))
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
                                lscnmap = lscnmap.Insert(lscnmap.LastIndexOf("\\") + 1, "(r)");
                                //如果文件存在
                                if (File.Exists(lscnmap))
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
                                    string lsani = Directory.GetParent(item.ToString()).ToString() + "\\" + aniitem.Groups[1].Value;
                                    lsani = lsani.Replace("/", "\\");
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
            if (mappath.Count>0)
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
                    alltetx = File.ReadAllText(item);
                    //取出父路径
                    f_path = Directory.GetParent(item).ToString() ;
                    //如果参数obj_id不为空
                    if (obj_id != null)
                    {
                        //捕获特效编号
                        regex.创建(alltetx, @"\[passive object\]\r\n(.+)\r\n\[/passive object\]", 0);
                        if (regex.取匹配数量()>0)
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
                        regex.创建(alltetx, @"([0-9]+)\t`\[[a-z]+\]`\r\n([\x20-\x7f]+\t){3}",0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(npc_id, 1, false, 0);
                        }

                    }

                    //如果参数ani路径不为空
                    if (ani_path != null)
                    {
                        //捕获ani
                        regex.创建(alltetx,@"`(.+\.ani)`",1);
                        if (regex.取匹配数量() > 0)
                        {
                            foreach (Match aniitem in regex.正则返回集合())
                            {
                                anipath = aniitem.Groups[1].Value.Replace("/", "\\");
                                if (anipath.Contains("...\\") == false)
                                {
                                    //得到ani全路径
                                    anipath = f_path + "\\" + anipath;
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
                                tilpath = tilitem.Groups[1].Value.Replace("/", "\\");
                                if (tilpath.Contains("...\\") == false)
                                {
                                    //得到ani全路径
                                    tilpath = f_path + "\\" + tilpath;
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
        /// <param name="pvfpath">pvf文件所在目录</param>
        /// <param name="equpath">装备全路径</param>
        /// <param name="apd_id">apd编号</param>
        /// <param name="mob_id">怪物编号</param>
        /// <param name="apc_id">apc编号</param>
        /// <param name="obj_id">特效编号</param>
        /// <param name="ptlpath">ptl全路径</param>
        public void Equ寻找编号和路径(string pvfpath,List<string> equpath, List<string> apd_id,List<string> mob_id,List<string> apc_id, List<string> obj_id, List<string> ptlpath, List<string> part_set_id, List<string> ani_path)
        {

            if (equpath.Count>0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in equpath)
                {
                    string alltext = File.ReadAllText(item);

                    if (apd_id != null)
                    {
                        regex.创建(alltext, @"\[appendage\]\r\n([0-9]+)\t", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(apd_id, 1, false, 0);
                        }
                    }

                    if (obj_id != null )
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
                                    objitemtxt = pvfpath + @"\equipment\character\particle\" + objitem.Groups[1].Value;
                                    objitemtxt = objitemtxt.ToLower();
                                    if (File.Exists(objitemtxt))
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

                    if (mob_id!=null)
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
                                string LinShiPtlPath = items.Groups[1].Value.ToLower().Replace("/","\\");
                                if (ptlpath.Contains(pvfpath+"\\"+LinShiPtlPath) ==false)
                                {
                                    ptlpath.Add(pvfpath + "\\" + LinShiPtlPath);
                                }
                            }
                        }
                    }

                    if (part_set_id!=null)
                    {
                        regex.创建(alltext, @"\[part set index\]\r\n([0-9]+)\t", 0);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(part_set_id, 1, false, 0);
                        }
                    }

                    if (ani_path!=null)
                    {
                        regex.创建(alltext, @"\[aurora graphic effects\]\r\n(.+`.+\.ani`\r\n){1,}", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            foreach (Match items in regex.正则返回集合())
                            {
                                Regex_new GHregex = new Regex_new();
                                GHregex.创建(items.Value,@"`(.+\.ani)`",1);
                                foreach (Match itemss in GHregex.正则返回集合())
                                {
                                    string AniZ1 = pvfpath + "\\" + itemss.Groups[1].Value.ToLower().Replace("/","\\");
                                    if (ani_path.Contains(AniZ1) ==false)
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
                                string TitleAni = 识别点点杠(F_path + "\\" + items.Groups[1].Value, true);
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
                                string EffectAni = pvfpath + "\\" + items.Groups[1].Value.ToLower().Replace("/","\\");
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
                                    string AniZ1 = pvfpath + "\\" + itemss.Groups[1].Value.ToLower().Replace("/", "\\");
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
        /// <param name="pvfpath">PVF文件目录路径</param>
        /// <param name="part_set_id">ETC套装ID</param>
        /// <param name="part_set_path">返回的套装文件路径</param>
        /// <param name="part_set_xinxi">返回的套装信息</param>
        public void 寻找Etc套装编号路径(string pvfpath, List<string> part_set_id,List<string> part_set_path,StringBuilder part_set_xinxi)
        {
            if (part_set_id.Count>0)
            {
                string Part_Set_Text;
                if (File.Exists(pvfpath + "\\etc\\equipmentpartset.etc"))
                {
                    Regex_new regex = new Regex_new();
                    Part_Set_Text = File.ReadAllText(pvfpath + "\\etc\\equipmentpartset.etc");
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
                                if (part_set_path.Contains(pvfpath + "\\equipment\\" + LinShiPuHuoText) == false)
                                {
                                    part_set_path.Add(pvfpath + "\\equipment\\" + LinShiPuHuoText);
                                    part_set_xinxi.Append(LinShiText+"\r\n\r\n");
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
        public void EquXunZhaoImgPath(List<string>EquPath,List<string>ImgPath,StringBuilder FanKuiCuoWu)
        {
            if (EquPath.Count>0)
            {
                //正则
                Regex_new regex = new Regex_new();
                //枚举出所有equ路径
                foreach (string item in EquPath)
                {
                    //判断文件是否存在
                    if (File.Exists(item))
                    {
                        string EquAllText = File.ReadAllText(item);//得到所有文件内容
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
                                    FanKuiCuoWu.Append(item+"\r\n");
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
            EquAniText = EquAniText.ToLower().Replace("\\","/"); ;//转成小写；替换斜杠
            int FanKuiCuoWu = -1;//反馈错误信息；初始值-1
            string type="";//装备职业类型
            string IdImg="";//img编号
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
                string Id1= regex.取子匹配文本(1, 1);
                string Id2= regex.取子匹配文本(1, 2);
                if (Id1=="0")
                {
                    while (Id2.Length<4)
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
            if (regex.取匹配数量()>0)
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

                if (PathImg1!=""&& IdImg!=""&& PathImg2!="")
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
        /// 读取PVF全职业装备一个ani中取出对应img全路径，返回c#语言switch判断语句
        /// </summary>
        /// <param name="EquPath">需要查找的装备路径</param>
        /// <returns></returns>
        public string EquImgShengChengSwitch(string EquPath)
        {
            Regex_new regex_New = new Regex_new();
            StringBuilder switchtxt = new StringBuilder();

            List<string> dirs = new List<string>(Directory.EnumerateFiles(EquPath, "jumpattack.ani", SearchOption.AllDirectories));
            dirs.Sort();

            string JueShe = "";
            for (int i = 0; i < 9; i++)
            {
                switch (i)
                {
                    case 0:
                        JueShe = "at fighter";
                        switchtxt.Append("switch (type)\r\n{\r\n   case \"at fighter\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                    case 1:
                        JueShe = "fighter";
                        switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   case \"fighter\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                    case 2:
                        JueShe = "at gunner";
                        switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   case \"at gunner\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                    case 3:
                        JueShe = "gunner";
                        switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   case \"gunner\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                    case 4:
                        JueShe = "at mage";
                        switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   case \"at mage\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                    case 5:
                        JueShe = "mage";
                        switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   case \"mage\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                    case 6:
                        JueShe = "priest";
                        switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   case \"priest\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                    case 7:
                        JueShe = "swordman";
                        switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   case \"swordman\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                    case 8:
                        JueShe = "thief";
                        switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   case \"thief\":\r\n       switch (MuLuMing)\r\n       {\r\n");
                        break;
                }

                foreach (string item in dirs)
                {
                    string txt = File.ReadAllText(item).ToLower() ;
                    switch (JueShe)
                    {
                        case "at fighter":

                            if (txt.Contains("/fighter/atequipment/") ==true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                 ml = ml.Substring(ml.LastIndexOf("\\")+1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \""+ ml+ "\":\r\n               PathImg1 =\""+ path1+ "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                        case "fighter":
                            if (txt.Contains("/fighter/equipment/") == true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                ml = ml.Substring(ml.LastIndexOf("\\") + 1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \"" + ml + "\":\r\n               PathImg1 =\"" + path1 + "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                        case "at gunner":
                            if (txt.Contains("/gunner/atequipment/") == true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                ml = ml.Substring(ml.LastIndexOf("\\") + 1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \"" + ml + "\":\r\n               PathImg1 =\"" + path1 + "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                        case "gunner":
                            if (txt.Contains("/gunner/equipment/") == true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                ml = ml.Substring(ml.LastIndexOf("\\") + 1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \"" + ml + "\":\r\n               PathImg1 =\"" + path1 + "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                        case "at mage":
                            if (txt.Contains("/mage/atequipment/") == true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                ml = ml.Substring(ml.LastIndexOf("\\") + 1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \"" + ml + "\":\r\n               PathImg1 =\"" + path1 + "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                        case "mage":
                            if (txt.Contains("/mage/equipment/") == true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                ml = ml.Substring(ml.LastIndexOf("\\") + 1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \"" + ml + "\":\r\n               PathImg1 =\"" + path1 + "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                        case "priest":
                            if (txt.Contains("/priest/equipment/") == true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                ml = ml.Substring(ml.LastIndexOf("\\") + 1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \"" + ml + "\":\r\n               PathImg1 =\"" + path1 + "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                        case "swordman":
                            if (txt.Contains("/swordman/equipment/") == true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                ml = ml.Substring(ml.LastIndexOf("\\") + 1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \"" + ml + "\":\r\n               PathImg1 =\"" + path1 + "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                        case "thief":
                            if (txt.Contains("/thief/equipment/") == true)
                            {
                                string ml = item.Remove(item.LastIndexOf("\\"));
                                ml = ml.Substring(ml.LastIndexOf("\\") + 1);
                                string regextxt = @"`(.+)%02d%02d(.*\.img)`";
                                regex_New.创建(txt, regextxt, 1);
                                string path1 = regex_New.取子匹配文本(1, 1);
                                string path2 = regex_New.取子匹配文本(1, 2);
                                switchtxt.Append("           case \"" + ml + "\":\r\n               PathImg1 =\"" + path1 + "\";\r\n" + "               PathImg2 =\"" + path2 + "\";\r\n               break;\r\n");
                            }
                            break;
                    }


                }
            }
            switchtxt.Append("           default:\r\n               PathImg1 = \"\";\r\n               PathImg1 = \"\";\r\n               break;\r\n       }\r\n       break;\r\n   default:\r\n       PathImg1 = \"\";\r\n       PathImg1 = \"\";\r\n       break;\r\n}");

            return switchtxt.ToString();
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
                    if (File.Exists(item))
                    {
                        string EquAllText = File.ReadAllText(item);
                        regex.创建(EquAllText, @"\[output index\][\r\n]+([0-9]+)\t", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            regex.子匹配加入集合(equ_id,1,false,0);
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
            if (equpath.Count>0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in equpath)
                {
                    if (File.Exists(item))
                    {
                        string EquAllText = File.ReadAllText(item);
                        regex.创建(EquAllText, @"\[creature species\][\r\n]+([0-9]+)\t",1);
                        if (regex.取匹配数量()>0)
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
        public void Cre寻找路径(List<string> crepath, List<string> ani_path,List<string> atk_path, List<string> skl_path,List<string> ptl_path)
        {
            if (crepath.Count>0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in crepath)
                {
                    if (File.Exists(item))
                    {
                        string CreAllText = File.ReadAllText(item);
                        regex.创建(CreAllText, @"\[.+\][\r\n]+`(.+\.[a-z]+)`", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            string F_path = item.Replace("/", "\\");
                            F_path = F_path.Remove(F_path.LastIndexOf("\\")+1);
                            foreach (Match itmes in regex.正则返回集合())
                            {
                                string paths = F_path+itmes.Groups[1].Value.ToLower();
                                string HouZhui = paths.Substring(paths.Length - 3);
                                paths = 识别点点杠(paths, true);
                                switch (HouZhui)
                                {
                                    case "ani":
                                        if (ani_path != null)
                                        {
                                            if (ani_path.Contains(paths) ==false)
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
        public void CreSkl寻找路径或编号(List<string> skl_path, List<string> ani_path,List<string> ptl_path, List<string> obj_id)
        {
            if (skl_path.Count>0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in skl_path)
                {
                    if (File.Exists(item))
                    {
                        string CreSklAllText = File.ReadAllText(item);
                        string F_Path = item.Replace("/","\\");
                        F_Path = DirectoryGetParent(DirectoryGetParent(F_Path))+"\\";
                        if (ani_path!=null)
                        {
                            regex.创建(CreSklAllText,@"`(.+\.[a-z]+)`",1);
                            if (regex.取匹配数量()>0)
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

                        if (obj_id!=null)
                        {
                            regex.创建(CreSklAllText, @"\[passive\][\r\n]+(([\x20-\x7f]+\t){5}`.*`\r\n)+", 1);
                            if (regex.取匹配数量()>0)
                            {
                                Regex_new regex1 = new Regex_new();
                                regex1.创建(regex.返回所有匹配文本(), @"([\x20-\x7f]+)\t([\x20-\x7f]+\t){4}`.*`", 1);
                                if (regex1.取匹配数量()>0)
                                {
                                    regex1.子匹配加入集合(obj_id,1,false,0);
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
        /// <param name="pathexm">父路径例如：E:\DNF\stackable\</param>
        /// <param name="XunZhaoStkId">被寻找的消耗品ID集合</param>
        /// <param name="StkLstAllText">整理过的lst列表集合</param>
        /// <param name="JiaRuStkId">返回加入找出的礼包ID</param>
        /// <param name="JiaRuStkPath">返回加入找出的礼包全路径</param>
        public void Id寻找Stk礼包(string pathexm,List<string> XunZhaoStkId, List<List<string>> StkLstAllText, List<string> JiaRuStkId, List<string> JiaRuStkPath)
        {
            if (XunZhaoStkId.Count>0)
            {
                Regex_new regex = new Regex_new();

                foreach (string item in XunZhaoStkId)
                {
                    int pos = StkLstAllText[0].IndexOf(item);
                    if (pos!=-1)
                    {
                        string FilePath = pathexm + StkLstAllText[2][pos];
                        if (File.Exists(FilePath))
                        {
                            regex.创建(File.ReadAllText(FilePath), @"\[stackable type\]\r\n`\[(.*booster.*|cera package|usable cera package|multi upgradable legacy|upgradable legacy|upgrade limit cube)\]`", 1);
                            if (regex.取匹配数量()>0)
                            {
                                string XunZhaoDId = StkLstAllText[0][pos];
                                if (JiaRuStkId.Contains(XunZhaoDId) ==false)
                                {
                                    JiaRuStkId.Add(XunZhaoDId);
                                }

                                if (JiaRuStkPath.Contains(FilePath) ==false)
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
        public void Stk礼包获取AllId(List<string> LinShiStkPath, List<string>LinShiId)
        {
            Regex_new regex = new Regex_new();

            foreach (string item in LinShiStkPath)
            {
                if (File.Exists(item))
                {
                    string AllText = File.ReadAllText(item);

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
        /// <param name="pathex">父路径例如：E:\DNF\stackable\</param>
        /// <param name="LinShiId">被寻找的编号集合</param>
        /// <param name="LstAllText">整理过的lst列表集合</param>
        /// <param name="EquStkId">返回并加入ID指向名称</param>
        public void IdBianBieType(string pathex,List<string> LinShiId, List<List<string>> LstAllText, StringBuilder EquStkId)
        {
            if (LinShiId.Count>0)
            {
                Regex_new regex = new Regex_new();

                foreach (string item in LinShiId)
                {
                    int pos = LstAllText[0].IndexOf(item);
                    if (pos!=-1)
                    {
                        string FilePath = pathex + LstAllText[2][pos];
                        if (File.Exists(FilePath))
                        {
                            regex.创建(File.ReadAllText(FilePath),@"\[name\]\r\n`([^`]+)`",1);
                            if (regex.取匹配数量()>0)
                            {
                                string Name = regex.取子匹配文本(1,1).Replace("\r\n"," ");
                                EquStkId.Append(LstAllText[0][pos]+"\t"+ Name+"\t\r\n");
                            }
                            else
                            {
                                EquStkId.Append(LstAllText[0][pos] + "\t无名称\t\r\n");
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
                        if (File.Exists(FilePath))
                        {
                            regex.创建(File.ReadAllText(FilePath), @"\[name\]\r\n`([^`]+)`", 1);
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
        /// <param name="pathex">父路径例如：E:\DNF\stackable\</param>
        /// <param name="StkId">礼包ID集合</param>
        /// <param name="StkLstAllText">整理过的lst列表集合</param>
        public void Stk礼包循环遍历StkId(string pathex,List<string> StkId,List<List<string>> StkLstAllText)
        {
            if (StkId.Count>0)
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

                } while (LinShiStkPath.Count>0);
               
            }
        }


        /// <summary>
        /// 根据宝珠全路径，查找出卡片代码
        /// </summary>
        /// <param name="StkBaoZhuPath">被寻找的宝珠全路径集合</param>
        /// <param name="StkKaPianId">返回加入的卡片代码集合</param>
        public void Stk宝珠寻找卡片Id(List<string>StkBaoZhuPath,List<string>StkKaPianId)
        {
            if (StkBaoZhuPath.Count>0)
            {
                Regex_new regex = new Regex_new();
                foreach (string item in StkBaoZhuPath)
                {
                    if (File.Exists(item))
                    {
                        string AllText = File.ReadAllText(item);
                        regex.创建(AllText, @"\[monster card id\]\r\n([0-9]+)\t",1);
                        regex.子匹配加入集合(StkKaPianId,1,false,0);
                    }
                }
            }
        }

        /// <summary>
        /// 整理Etc套装，返回单个套装ID以及套装信息的集合
        /// </summary>
        /// <param name="EtcInfo">被整理的Etc套装信息</param>
        /// <returns></returns>
        public Dictionary<string, string> Etc套装整理(string EtcInfo)
        {
            Dictionary<string, string> Etc = new Dictionary<string, string>();//声明并实例一个字典集合

            int YesNo1 = -1;//声明一个整数型变量；用于判断是否获取文本
            StringBuilder AddText = new StringBuilder();//可变字符串类型
            string AddId = "";//声明一个字符串类型；用于获取套装的ID

            //按照换行符分割Etc套装属性内容，删除所有空字符串数组；并且一一枚举出所有数组内容
            foreach (string item in EtcInfo.Split(new char[] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries))
            {
                
                switch (item)
                {
                    case "[equipment part set]"://当“item”为[equipment part set]时 YesNo1变量为0
                        YesNo1 = 0;
                        break;
                    case "[/equipment part set]"://当“item”为[/equipment part set]时 YesNo1变量为1
                        YesNo1 = 1;
                        break;
                }

                if (item.ToLower().Contains(".equ`"))//如果“item”转为小写，并且字符串中有“.equ`”字符串时条件为真
                    AddId = item.Remove(item.IndexOf("\t"));//为真时；item从左到右找到第一个\t后删除往后所有字符串只保留ID并赋值给 Addid字符串变量

                switch (YesNo1)
                {
                    case 0:
                        AddText.Append(item + "\r\n");//如果yesno1变量为0时增加到可变字符串类型中
                        continue;//直接进行下一个枚举
                    case 1://如果yesno1变量为1时
                        AddText.Append(item + "\r\n\r\n");//加入到可变型字符串变量中；相当于[/equipment part set]结尾了

                        if (Etc.ContainsKey(AddId) == false)//如果字典集合中的键不存在addid变量的套装ID；条件为真
                            Etc.Add(AddId, AddText.ToString());//以上条件为真时；套装ID以及套装的信息加入到字典集合中

                        AddText.Clear();//清空可变型字符串

                        YesNo1 = -1;//yesno1赋值为-1；代表已经找完一个套装属性了，可以继续下一个寻找
                        AddId = "";//addid套装编号为空
                        continue;//直接进行下一个枚举
                    default://如果以上俩个条件都不是
                        continue;//直接进行下一个枚举
                }
            }
            return Etc;//寻找完加入字典集合中就返回
        }

        /// <summary>
        /// 根据宝珠全路径，查找出卡片代码
        /// </summary>
        /// <param name="Path">需要整理的ani动作文件目录</param>
        public void 修复裸体ActFileZl(string Path, Dictionary<string, 修复裸体> ActInfo)
        {

            //枚举一个目录下的所有ani文件
            List<string> dirs = new List<string>(Directory.EnumerateFiles(Path, "*.ani", SearchOption.AllDirectories));

            dirs.ForEach((FliePath) =>
            {
                string AllText = File.ReadAllText(FliePath);//读入所有文本内容

                //正则判断一次 帧的数量是不是大于0
                Regex_new regex = new Regex_new();
                regex.创建(AllText+"\r\n\r\n", @"(\[FRAME\d+\][\S\s]*?)\r\n[\r\n]+", 1);
                if (regex.取匹配数量() > 0)
                {

                    KeyValuePair<List<string>, List<string>> Info = new KeyValuePair<List<string>, List<string>>(new List<string>(), new List<string>());

                    List<string> Pos = new List<string>();//记录每一帧的坐标
                    List<string> Rotate = new List<string>();//记录每一帧的旋转角度
                    List<string> Rate = new List<string>();//记录每一帧的放大率

                    foreach (Match item in regex.正则返回集合())
                    {
                        string zText = item.Groups[1].Value.ToLower().Replace("\\", "/");//得到子匹配1

                        //找帧路径 找到了获取帧ID  没找到加入-1值
                        regex.创建(zText, @"\[IMAGE\]\r\n(\t+)?`character/(fighter|gunner|mage|priest|swordman|thief).+/skin/.+\.img`\r\n(\t+)?(\d+)", 1);
                        if (regex.取匹配数量() > 0)
                            Info.Key.Add(regex.取子匹配文本(1, 4));
                        else
                            Info.Key.Add("-1");

                        //找帧的延时 找到了获取延时数 没找到加入-1值
                        regex.创建(zText, @"\[DELAY\]\r\n(\t+)?(\d+)", 1);
                        if (regex.取匹配数量() > 0)
                            Info.Value.Add(regex.取子匹配文本(1, 2));
                        else
                            Info.Value.Add("-1");


                        //找到帧的坐标 并记录到集合中
                        regex.创建(zText, @"\[IMAGE POS\]\r\n(\t+)?(.+)\r\n", 1);
                        if (regex.取匹配数量() > 0)
                            Pos.Add(regex.取子匹配文本(1, 2));
                        else
                            Pos.Add("0\t0");

                        //找到帧的旋转标签 并记录到集合中
                        regex.创建(zText, @"\[IMAGE ROTATE\]\r\n(\t+)?(.+)\r\n", 1);
                        if (regex.取匹配数量() > 0)
                            Rotate.Add(regex.取子匹配文本(1, 2));
                        else
                            Rotate.Add("-1");

                        //找到帧的旋转标签 并记录到集合中
                        regex.创建(zText, @"\[IMAGE RATE\]\r\n(\t+)?(.+)\r\n", 1);
                        if (regex.取匹配数量() > 0)
                            Rate.Add(regex.取子匹配文本(1, 2));
                        else
                            Rate.Add("-1");

                    }

                    string FileName = FliePath.Substring(FliePath.LastIndexOf("\\") + 1);//得到文件名

                    string TouText = "";//头文本
                    regex.创建(AllText, @"([\S\s]*?)\[FRAME MAX\]", 1);
                    if (regex.取匹配数量() > 0)//匹配到头文本就更改
                        TouText = regex.取子匹配文本(1, 1);

                    ActInfo.Add(FileName, new 修复裸体(TouText, Info, Pos, Rotate, Rate));//加入字典
                }


            });
        }

        /// <summary>
        /// 根据提供的角色编号 来返回对于的key键值对的信息
        /// </summary>
        /// <param name="ChrId">角色ID：0男格斗 1女格斗 2女抢手 3男抢手 4男魔法 5女魔法 6圣职者 7鬼剑士 8暗夜使者</param>
        /// <returns></returns>
        public KeyValuePair<List<string>, List<string>> 修复裸体GetChrKey(int ChrId)
        {
            KeyValuePair<List<string>, List<string>> AddInfo = new KeyValuePair<List<string>, List<string>>(new List<string>(), new List<string>());

            switch (ChrId)
            {
                case 0:
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/belt/fm_belt%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/belt/fm_belt%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/belt/fm_belt%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/belt/fm_belt%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\belt\\belt_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/belt/fm_belt%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/cap/fm_cap%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/cap/fm_cap%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/cap/fm_cap%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\cap\\cap_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/cap/fm_cap%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\cap\\cap_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/cap/fm_cap%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/coat/fm_coat%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/coat/fm_coat%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/coat/fm_coat%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/coat/fm_coat%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\coat\\coat_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/coat/fm_coat%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/face/fm_face%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/face/fm_face%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/face/fm_face%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\face\\face_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/face/fm_face%02d%02dg.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/hair/fm_hair%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/hair/fm_hair%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/hair/fm_hair%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\hair\\hair_f1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/hair/fm_hair%02d%02df1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/neck/fm_neck%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/neck/fm_neck%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/neck/fm_neck%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/neck/fm_neck%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/neck/fm_neck%02d%02de.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/neck/fm_neck%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/pants/fm_pants%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/pants/fm_pants%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/pants/fm_pants%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/shoes/fm_shoes%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/shoes/fm_shoes%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_avatar\\shoes\\shoes_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Avatar/shoes/fm_shoes%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_custom\\atskill_definitegrab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/ATgang.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[RGBA]\r\n\t\t255\t150\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_custom\\atskill_flamelegseyes\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/ATFlameLegs/eye_dodge.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_custom\\atskill_flamelegsfoots\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/ATFlameLegs/firebody_dodge.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_custom\\atskill_slowdownpowerup\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/ATgang.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[RGBA]\r\n\t\t255\t0\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\custom\\skill_definitegrab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/gang.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t150\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\custom\\skill_slowdownpowerup\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/gang.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t0\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\at_grabbler_hairband_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/GrowType/grabbler_hairband_a.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\at_grabbler_hairband_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/GrowType/grabbler_hairband_b.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\at_streetfighter\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/GrowType/streetfighter.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\at_striker\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/GrowType/striker.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\at_striker_band\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/GrowType/striker_dodge.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovea\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovea1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02da1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovea2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02da2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bgloveb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bgloveb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02db1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovec1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02dc1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovec2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02dc2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bgloved2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02dd2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovex\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovex1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\at_bglovex2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boxglove/fm_bglove%02d%02dx2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_boneclawa\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boneclaw/fm_bclaw%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_boneclawc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boneclaw/fm_bclaw%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_boneclawx\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/boneclaw/fm_bclaw%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawa\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawa1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02da1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawa2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02da2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02db1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02db2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02dc1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02dc2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawx\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawx1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\at_clawx2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02dx2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_arma\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/arm/fm_arms%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_armb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/arm/fm_arms%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_armc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/arm/fm_arms%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_armx\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/arm/fm_arms%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntleta\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntleta1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02da1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntleta2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02da2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntletc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntletc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02dc1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntletc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02dc2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntletx\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntletx1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gauntletx2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/gauntlet/fm_gauntlet%02d%02dx2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_glovea\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_gloveb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/glove/fm_glove%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_glovec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/glove/fm_glove%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\at_glovex\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/glove/fm_glove%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_clawa\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_clawc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_clawx\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/claw/fm_claw%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_glovea\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/glove/fm_glove%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_glovec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/glove/fm_glove%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_glovex\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/glove/fm_glove%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklea\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklea1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02da1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklea2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02da2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklec1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02dc1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklec2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02dc2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklex\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklex1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\at_knucklex2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/knuckle/fm_knuckle%02d%02dx2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfaa\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfaa1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02da1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfaa2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02da2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfab1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02db1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfab2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02db2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfac\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfac1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02dc1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfac2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02dc2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfax\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfax1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\at_tonfax2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/ATEquipment/Weapon/tonfa/fm_tonfa%02d%02dx2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[DELAY]\r\n\t\t200\r\n");
                    break;
                case 1:
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_custom\\atskill_definitegrab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/ATgang.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[RGBA]\r\n\t\t255\t150\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_custom\\atskill_flamelegseyes\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/ATFlameLegs/eye_dodge.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_custom\\atskill_flamelegsfoots\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/ATFlameLegs/firebody_dodge.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\at_custom\\atskill_slowdownpowerup\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/ATgang.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-246\t-329\r\n\t[RGBA]\r\n\t\t255\t0\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t200\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/belt/ft_belt%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/belt/ft_belt%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/belt/ft_belt%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/belt/ft_belt%02d%02dd.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\belt\\belt_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/belt/ft_belt%02d%02de.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\belt\\belt_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/belt/ft_belt%02d%02df.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\belt\\belt_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/belt/ft_belt%02d%02dg.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/cap/ft_cap%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/cap/ft_cap%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/cap/ft_cap%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\cap\\cap_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/cap/ft_cap%02d%02dd.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\cap\\cap_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/cap/ft_cap%02d%02de.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\cap\\cap_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/cap/ft_cap%02d%02df.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/coat/ft_coat%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/coat/ft_coat%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/coat/ft_coat%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/coat/ft_coat%02d%02dd.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\coat\\coat_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/coat/ft_coat%02d%02de.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\coat\\coat_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/coat/ft_coat%02d%02df.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\coat\\coat_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/coat/ft_coat%02d%02dg.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\coat\\coat_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/coat/ft_coat%02d%02dh.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/face/ft_face%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/face/ft_face%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/face/ft_face%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\face\\face_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/face/ft_face%02d%02dd.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\face\\face_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/face/ft_face%02d%02de.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\face\\face_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/face/ft_face%02d%02df.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\face\\face_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/face/ft_face%02d%02dg.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\face\\face_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/face/ft_face%02d%02dh.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/hair/ft_hair%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/hair/ft_hair%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\hair\\hair_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/hair/ft_hair%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/hair/ft_hair%02d%02dd.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\hair\\hair_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/hair/ft_hair%02d%02de.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\hair\\hair_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/hair/ft_hair%02d%02df.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\hair\\hair_f1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/hair/ft_hair%02d%02df1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02dd.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02de.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02df.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02dg.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02dh.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02dx.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\neck\\neck_z\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/neck/ft_neck%02d%02dz.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t160\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/pants/ft_pants%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/pants/ft_pants%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\pants\\pants_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/pants/ft_pants%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/pants/ft_pants%02d%02dd.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\pants\\pants_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/pants/ft_pants%02d%02de.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\pants\\pants_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/pants/ft_pants%02d%02df.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\pants\\pants_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/pants/ft_pants%02d%02dg.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\pants\\pants_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/pants/ft_pants%02d%02dh.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/shoes/ft_shoes%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/shoes/ft_shoes%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\shoes\\shoes_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/shoes/ft_shoes%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\shoes\\shoes_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/shoes/ft_shoes%02d%02dd.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\avatar\\shoes\\shoes_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Avatar/shoes/ft_shoes%02d%02df.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\custom\\skill_definitegrab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/gang.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t150\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\custom\\skill_slowdownpowerup\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Effect/gang.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[RGBA]\r\n\t\t255\t0\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\grabbler_hairband_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/GrowType/grabbler_hairband_a.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\grabbler_hairband_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/GrowType/grabbler_hairband_b.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\streetfighter\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/GrowType/streetfighter.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\striker\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/GrowType/striker.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\growtype\\striker_band\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/GrowType/striker_band.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\bgloveb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/boxglove/bglove%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\bgloveb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/boxglove/bglove%02d%02db1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\bgloveb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/boxglove/bglove%02d%02db2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\bglovec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/boxglove/bglove%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\bglovec1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/boxglove/bglove%02d%02dc1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\bglovec2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/boxglove/bglove%02d%02dc2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\catgloveb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/catglove/cglove%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\boxglove\\catglovec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/catglove/cglove%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\boneclawb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/boneclaw/bclaw%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\boneclawc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/boneclaw/bclaw%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\clawa1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02da1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\clawa2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02da2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\clawb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\clawb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02db1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\clawb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02db2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\clawc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\clawc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02dc1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\claw\\clawc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02dc2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\armb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/arm/arm%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\armc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/arm/arm%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\gauntletb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/gauntlet/gauntlet%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\gauntletb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/gauntlet/gauntlet%02d%02db1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\gauntletb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/gauntlet/gauntlet%02d%02db2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\gauntletc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/gauntlet/gauntlet%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\gauntletc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/gauntlet/gauntlet%02d%02dc1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\gauntletc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/gauntlet/gauntlet%02d%02dc2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\yangyangib\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/glove/glove%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\gauntlet\\yangyangic\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/glove/glove%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\clawb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\clawc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/claw/claw%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\gloveb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/glove/glove%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\glovec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/glove/glove%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\knuckleb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/knuckle/knuckle%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\knuckleb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/knuckle/knuckle%02d%02db1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\knuckleb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/knuckle/knuckle%02d%02db2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\knucklec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/knuckle/knuckle%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\knucklec1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/knuckle/knuckle%02d%02dc1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\knuckle\\knucklec2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/knuckle/knuckle%02d%02dc2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\tonfaa\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/tonfa/tonfa%02d%02da.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\tonfab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/tonfa/tonfa%02d%02db.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\tonfab1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/tonfa/tonfa%02d%02db1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\tonfab2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/tonfa/tonfa%02d%02db2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\tonfac\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/tonfa/tonfa%02d%02dc.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\tonfac1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/tonfa/tonfa%02d%02dc1.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\tonfac2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/tonfa/tonfa%02d%02dc2.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\fighter\\weapon\\tonfa\\tonfax\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Fighter/Equipment/Weapon/tonfa/tonfa%02d%02dx.img`\r\n\t\t132\r\n\t[IMAGE POS]\r\n\t\t-248\t-380\r\n\t[DELAY]\r\n\t\t120\r\n");
                    break;
                case 2:
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/belt/gg_belt%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/belt/gg_belt%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/belt/gg_belt%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/belt/gg_belt%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\belt\\belt_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/belt/gg_belt%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\belt\\belt_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/belt/gg_belt%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/cap/gg_cap%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/cap/gg_cap%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/cap/gg_cap%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\cap\\cap_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/cap/gg_cap%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\cap\\cap_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/cap/gg_cap%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\cap\\cap_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/cap/gg_cap%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\cap\\cap_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/cap/gg_cap%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/coat/gg_coat%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/coat/gg_coat%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/coat/gg_coat%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/coat/gg_coat%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\coat\\coat_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/coat/gg_coat%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\coat\\coat_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/coat/gg_coat%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\coat\\coat_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/coat/gg_coat%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\coat\\coat_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/coat/gg_coat%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/face/gg_face%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/face/gg_face%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/face/gg_face%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\face\\face_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/face/gg_face%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\face\\face_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/face/gg_face%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\face\\face_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/face/gg_face%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\face\\face_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/face/gg_face%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/hair/gg_hair%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/hair/gg_hair%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\hair\\hair_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/hair/gg_hair%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/hair/gg_hair%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\hair\\hair_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/hair/gg_hair%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\hair\\hair_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/hair/gg_hair%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\hair\\hair_f1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/hair/gg_hair%02d%02df1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02dx.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\neck\\neck_z\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/neck/gg_neck%02d%02dz.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t153\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/pants/gg_pants%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/pants/gg_pants%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\pants\\pants_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/pants/gg_pants%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/pants/gg_pants%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\pants\\pants_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/pants/gg_pants%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\pants\\pants_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/pants/gg_pants%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\pants\\pants_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/pants/gg_pants%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\pants\\pants_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/pants/gg_pants%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/shoes/gg_shoes%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/shoes/gg_shoes%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\shoes\\shoes_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/shoes/gg_shoes%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_avatar\\shoes\\shoes_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Avatar/shoes/gg_shoes%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_custom\\skill_camouflage\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Effect/SubWeaponStuckDown/eye_light_green_at.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n\t[DAMAGE BOX]\r\n\t-13\t-5\t0\t35\t10\t99\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_custom\\skill_subweaponstuckdown\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Effect/SubWeaponStuckDown/eye_light_white_at.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t150\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n\t[DAMAGE BOX]\r\n\t-13\t-5\t0\t35\t10\t99\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\custom\\skill_camouflage\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Effect/SubWeaponStuckDown/eye_light_green.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\custom\\skill_subweaponstuckdown\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Effect/SubWeaponStuckDown/eye_light_white.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[RGBA]\r\n\t\t255\t150\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\at_launchera\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/GrowType/launchera.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\at_launcherb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/GrowType/launcherb.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\at_launcherc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/GrowType/launcherc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\at_rangera\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/GrowType/rangera.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\at_rangerd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/GrowType/rangerd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\at_spitfire_magazine_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/GrowType/spitfire_magazine_a.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\at_spitfire_magazine_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/GrowType/spitfire_magazine_b.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\at_spitfire_magazine_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/GrowType/spitfire_magazine_c.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\at_autob\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/auto/gg_auto%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\at_autob1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/auto/gg_auto%02d%02db1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\at_autob2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/auto/gg_auto%02d%02db2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\at_autoc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/auto/gg_auto%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\at_autoc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/auto/gg_auto%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\at_autoc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/auto/gg_auto%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\at_musketb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/musket/gg_musket%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\at_musketc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/musket/gg_musket%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\at_bowgunb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/bowgun/gg_bowgun%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\at_bowgunb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/bowgun/gg_bowgun%02d%02db1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\at_bowgunb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/bowgun/gg_bowgun%02d%02db2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\at_bowgunc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/bowgun/gg_bowgun%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\at_bowgunc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/bowgun/gg_bowgun%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\at_bowgunc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/bowgun/gg_bowgun%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\at_hcana\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/hcan/gg_hcan%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\at_hcanb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/hcan/gg_hcan%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\at_hcanb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/hcan/gg_hcan%02d%02db1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\at_hcanb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/hcan/gg_hcan%02d%02db2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\at_hcanc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/hcan/gg_hcan%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\at_hcanc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/hcan/gg_hcan%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\at_hcanc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/hcan/gg_hcan%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\at_musketb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/musket/gg_musket%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\at_musketb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/musket/gg_musket%02d%02db1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\at_musketb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/musket/gg_musket%02d%02db2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\at_musketc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/musket/gg_musket%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\at_musketc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/musket/gg_musket%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\at_musketc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/musket/gg_musket%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\at_mcgeeb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/auto/gg_auto%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\at_mcgeec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/auto/gg_auto%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\at_revb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/rev/gg_rev%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\at_revb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/rev/gg_rev%02d%02db1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\at_revb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/rev/gg_rev%02d%02db2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\at_revc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/rev/gg_rev%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\at_revc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/rev/gg_rev%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\at_revc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/ATEquipment/Weapon/rev/gg_rev%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t600\r\n");
                    break;
                case 3:
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_custom\\skill_camouflage\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Effect/SubWeaponStuckDown/eye_light_green_at.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n\t[DAMAGE BOX]\r\n\t-13\t-5\t0\t35\t10\t99\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\at_custom\\skill_subweaponstuckdown\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Effect/SubWeaponStuckDown/eye_light_white_at.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t150\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n\t[DAMAGE BOX]\r\n\t-13\t-5\t0\t35\t10\t99\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/belt/gn_belt%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/belt/gn_belt%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/belt/gn_belt%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/belt/gn_belt%02d%02dd.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\belt\\belt_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/belt/gn_belt%02d%02de.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\belt\\belt_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/belt/gn_belt%02d%02df.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/cap/gn_cap%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/cap/gn_cap%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/cap/gn_cap%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\cap\\cap_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/cap/gn_cap%02d%02dd.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\cap\\cap_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/cap/gn_cap%02d%02de.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\cap\\cap_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/cap/gn_cap%02d%02df.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\cap\\cap_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/cap/gn_cap%02d%02dg.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\cap\\cap_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/cap/gn_cap%02d%02dh.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/coat/gn_coat%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/coat/gn_coat%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/coat/gn_coat%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/coat/gn_coat%02d%02dd.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\coat\\coat_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/coat/gn_coat%02d%02de.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\coat\\coat_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/coat/gn_coat%02d%02df.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/face/gn_face%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/face/gn_face%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/face/gn_face%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\face\\face_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/face/gn_face%02d%02dd.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\face\\face_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/face/gn_face%02d%02de.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\face\\face_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/face/gn_face%02d%02df.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\face\\face_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/face/gn_face%02d%02dg.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/hair/gn_hair%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/hair/gn_hair%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\hair\\hair_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/hair/gn_hair%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/hair/gn_hair%02d%02dd.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\hair\\hair_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/hair/gn_hair%02d%02de.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\hair\\hair_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/hair/gn_hair%02d%02df.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\hair\\hair_f1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/hair/gn_hair%02d%02df1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02dd.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02de.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02df.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02dh.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02dx.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\neck\\neck_z\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/neck/gn_neck%02d%02dz.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/pants/gn_pants%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/pants/gn_pants%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\pants\\pants_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/pants/gn_pants%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/pants/gn_pants%02d%02dd.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\pants\\pants_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/pants/gn_pants%02d%02de.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\pants\\pants_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/pants/gn_pants%02d%02df.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/shoes/gn_shoes%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/shoes/gn_shoes%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\shoes\\shoes_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/shoes/gn_shoes%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\shoes\\shoes_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/shoes/gn_shoes%02d%02dd.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\avatar\\shoes\\shoes_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Avatar/shoes/gn_shoes%02d%02df.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\custom\\skill_camouflage\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Effect/SubWeaponStuckDown/eye_light_green.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\custom\\skill_subweaponstuckdown\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Effect/SubWeaponStuckDown/eye_light_white.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[RGBA]\r\n\t\t255\t150\t0\t255\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\launchera\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/GrowType/launchera.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\launcherb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/GrowType/launcherb.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\ranger\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/GrowType/ranger.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\spitfire_magazine_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/GrowType/spitfire_magazine_a.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\growtype\\spitfire_magazine_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/GrowType/spitfire_magazine_b.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\autob\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/auto/auto%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\autob1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/auto/auto%02d%02db1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\autob2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/auto/auto%02d%02db2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\autoc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/auto/auto%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\autoc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/auto/auto%02d%02dc1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\autoc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/auto/auto%02d%02dc2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\musketb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/musket/musket%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\automatic\\musketc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/musket/musket%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\bowgunb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/bowgun/bowgun%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\bowgunb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/bowgun/bowgun%02d%02db1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\bowgunb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/bowgun/bowgun%02d%02db2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\bowgunc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/bowgun/bowgun%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\bowgunc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/bowgun/bowgun%02d%02dc1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\bowgun\\bowgunc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/bowgun/bowgun%02d%02dc2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\hcana\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/hcan/hcan%02d%02da.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\hcanb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/hcan/hcan%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\hcanb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/hcan/hcan%02d%02db1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\hcanb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/hcan/hcan%02d%02db2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\hcanc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/hcan/hcan%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\hcanc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/hcan/hcan%02d%02dc1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\hcannon\\hcanc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/hcan/hcan%02d%02dc2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\musketb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/musket/musket%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\musketb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/musket/musket%02d%02db1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\musketb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/musket/musket%02d%02db2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\musketc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/musket/musket%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\musketc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/musket/musket%02d%02dc1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\musket\\musketc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/musket/musket%02d%02dc2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\mcgeeb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/auto/auto%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\mcgeec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/auto/auto%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\revb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/rev/rev%02d%02db.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\revb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/rev/rev%02d%02db1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\revb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/rev/rev%02d%02db2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\revc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/rev/rev%02d%02dc.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\revc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/rev/rev%02d%02dc1.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t600\r\n");
                    AddInfo.Key.Add("equipment\\character\\gunner\\weapon\\revolver\\revc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Gunner/Equipment/Weapon/rev/rev%02d%02dc2.img`\r\n\t\t13\r\n\t[IMAGE POS]\r\n\t\t-248\t-375\r\n\t[DELAY]\r\n\t\t600\r\n");
                    break;
                case 4:
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/belt/mm_belt%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/belt/mm_belt%02d%02db.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/belt/mm_belt%02d%02dc.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/belt/mm_belt%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/cap/mm_cap%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/cap/mm_cap%02d%02db.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/cap/mm_cap%02d%02dc.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/coat/mm_coat%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/coat/mm_coat%02d%02db.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/coat/mm_coat%02d%02dc.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/coat/mm_coat%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/face/mm_face%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/face/mm_face%02d%02db.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/face/mm_face%02d%02dc.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/hair/mm_hair%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/hair/mm_hair%02d%02db.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/hair/mm_hair%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/neck/mm_neck%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/neck/mm_neck%02d%02db.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/neck/mm_neck%02d%02dc.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/neck/mm_neck%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/neck/mm_neck%02d%02de.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/neck/mm_neck%02d%02dx.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/pants/mm_pants%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/pants/mm_pants%02d%02db.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/pants/mm_pants%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/shoes/mm_shoes%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\at_avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Avatar/shoes/mm_shoes%02d%02db.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\at_brooma\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/broom/mm_broom%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\at_brooma1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/broom/mm_broom%02d%02da1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\at_brooma2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/broom/mm_broom%02d%02da2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\at_broomd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/broom/mm_broom%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\at_broomd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/broom/mm_broom%02d%02dd1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\at_broomd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/broom/mm_broom%02d%02dd2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_cintsticka\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_cintstickd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_dakshaa\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_dakshad\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_polea\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_polea1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02da1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_polea2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02da2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_poled\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_poled1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02dd1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\at_poled2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/pole/mm_pole%02d%02dd2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\at_roda\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/rod/mm_rod%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\at_roda1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/rod/mm_rod%02d%02da1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\at_roda2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/rod/mm_rod%02d%02da2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\at_rodd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/rod/mm_rod%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\at_rodd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/rod/mm_rod%02d%02dd1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\at_rodd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/rod/mm_rod%02d%02dd2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_beamspeara1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02da1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_beamspeara2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02da2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_beamspeard1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02dd1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_beamspeard2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02dd2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_lancea\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_lanced\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_speara\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_speara1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02da1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_speara2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02da2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_speard\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_speard1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02dd1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\at_speard2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/spear/mm_spear%02d%02dd2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\at_staffa\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/staff/mm_staff%02d%02da.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\at_staffa1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/staff/mm_staff%02d%02da1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\at_staffa2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/staff/mm_staff%02d%02da2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\at_staffd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/staff/mm_staff%02d%02dd.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\at_staffd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/staff/mm_staff%02d%02dd1.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\at_staffd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/ATEquipment/Weapon/staff/mm_staff%02d%02dd2.img`\r\n\t\t8\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t120\r\n");
                    break;
                case 5:
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/belt/mg_belt%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/belt/mg_belt%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/belt/mg_belt%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/belt/mg_belt%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\belt\\belt_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/belt/mg_belt%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\belt\\belt_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/belt/mg_belt%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\belt\\belt_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/belt/mg_belt%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\belt\\belt_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/belt/mg_belt%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/cap/mg_cap%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/cap/mg_cap%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/cap/mg_cap%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\cap\\cap_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/cap/mg_cap%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\cap\\cap_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/cap/mg_cap%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\cap\\cap_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/cap/mg_cap%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\cap\\cap_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/cap/mg_cap%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\cap\\cap_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/cap/mg_cap%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/coat/mg_coat%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/coat/mg_coat%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/coat/mg_coat%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/coat/mg_coat%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\coat\\coat_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/coat/mg_coat%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\coat\\coat_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/coat/mg_coat%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\coat\\coat_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/coat/mg_coat%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\coat\\coat_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/coat/mg_coat%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/face/mg_face%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/face/mg_face%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/face/mg_face%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\face\\face_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/face/mg_face%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\face\\face_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/face/mg_face%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\face\\face_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/face/mg_face%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\face\\face_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/face/mg_face%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\face\\face_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/face/mg_face%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/hair/mg_hair%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/hair/mg_hair%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\hair\\hair_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/hair/mg_hair%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/hair/mg_hair%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\hair\\hair_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/hair/mg_hair%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\hair\\hair_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/hair/mg_hair%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\hair\\hair_f1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/hair/mg_hair%02d%02df1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02dx.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\neck\\neck_z\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/neck/mg_neck%02d%02dz.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t178\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/pants/mg_pants%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/pants/mg_pants%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\pants\\pants_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/pants/mg_pants%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/pants/mg_pants%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\pants\\pants_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/pants/mg_pants%02d%02de.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\pants\\pants_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/pants/mg_pants%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\pants\\pants_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/pants/mg_pants%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\pants\\pants_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/pants/mg_pants%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/shoes/mg_shoes%02d%02da.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/shoes/mg_shoes%02d%02db.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\shoes\\shoes_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/shoes/mg_shoes%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\shoes\\shoes_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/shoes/mg_shoes%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\shoes\\shoes_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/shoes/mg_shoes%02d%02df.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\shoes\\shoes_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/shoes/mg_shoes%02d%02dg.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\avatar\\shoes\\shoes_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Avatar/shoes/mg_shoes%02d%02dh.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\growtype\\battlemagea\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/GrowType/battlemagea.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\growtype\\battlemageb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/GrowType/battlemageb.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\growtype\\summoner\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/GrowType/summoner.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\growtype\\witch\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/GrowType/witch.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\broomc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/broom/mg_broom%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\broomc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/broom/mg_broom%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\broomc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/broom/mg_broom%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\broomd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/broom/mg_broom%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\broomd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/broom/mg_broom%02d%02dd1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\broom\\broomd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/broom/mg_broom%02d%02dd2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\cintstickc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\cintstickd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\dakshac\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\dakshad\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\polec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\polec1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\polec2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\poled\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\poled1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dd1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\pole\\poled2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/pole/mg_pole%02d%02dd2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\rodc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/rod/mg_rod%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\rodc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/rod/mg_rod%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\rodc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/rod/mg_rod%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\rodd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/rod/mg_rod%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\rodd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/rod/mg_rod%02d%02dd1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\rod\\rodd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/rod/mg_rod%02d%02dd2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\beamspearc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\beamspearc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\beamspeard1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dd1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\beamspeard2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dd2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\lancec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\lanced\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\spearc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\spearc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\spearc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\speard\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\speard1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dd1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\spear\\speard2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/spear/mg_spear%02d%02dd2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\staffc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/staff/mg_staff%02d%02dc.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\staffc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/staff/mg_staff%02d%02dc1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\staffc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/staff/mg_staff%02d%02dc2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\staffd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/staff/mg_staff%02d%02dd.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\staffd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/staff/mg_staff%02d%02dd1.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\mage\\weapon\\staff\\staffd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Mage/Equipment/Weapon/staff/mg_staff%02d%02dd2.img`\r\n\t\t14\r\n\t[IMAGE POS]\r\n\t\t-248\t-378\r\n\t[DELAY]\r\n\t\t180\r\n");
                    break;
                case 6:
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/belt/pr_belt%02d%02da.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/belt/pr_belt%02d%02db.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/belt/pr_belt%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/belt/pr_belt%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\belt\\belt_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/belt/pr_belt%02d%02de.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\belt\\belt_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/belt/pr_belt%02d%02df.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/cap/pr_cap%02d%02da.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/cap/pr_cap%02d%02db.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/cap/pr_cap%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\cap\\cap_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/cap/pr_cap%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\cap\\cap_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/cap/pr_cap%02d%02de.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\cap\\cap_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/cap/pr_cap%02d%02df.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\cap\\cap_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/cap/pr_cap%02d%02dg.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/coat/pr_coat%02d%02da.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/coat/pr_coat%02d%02db.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/coat/pr_coat%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/coat/pr_coat%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\coat\\coat_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/coat/pr_coat%02d%02de.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\coat\\coat_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/coat/pr_coat%02d%02df.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/face/pr_face%02d%02da.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/face/pr_face%02d%02db.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/face/pr_face%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\face\\face_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/face/pr_face%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\face\\face_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/face/pr_face%02d%02de.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\face\\face_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/face/pr_face%02d%02df.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\face\\face_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/face/pr_face%02d%02dg.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/hair/pr_hair%02d%02da.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/hair/pr_hair%02d%02db.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\hair\\hair_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/hair/pr_hair%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/hair/pr_hair%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\hair\\hair_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/hair/pr_hair%02d%02de.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\hair\\hair_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/hair/pr_hair%02d%02df.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\hair\\hair_f1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/hair/pr_hair%02d%02df1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/neck/pr_neck%02d%02da.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/neck/pr_neck%02d%02db.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/neck/pr_neck%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/neck/pr_neck%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/neck/pr_neck%02d%02de.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\neck\\neck_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/neck/pr_neck%02d%02df.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/neck/pr_neck%02d%02dx.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\neck\\neck_z\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/neck/pr_neck%02d%02dz.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/pants/pr_pants%02d%02da.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/pants/pr_pants%02d%02db.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\pants\\pants_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/pants/pr_pants%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/pants/pr_pants%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\pants\\pants_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/pants/pr_pants%02d%02de.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\pants\\pants_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/pants/pr_pants%02d%02df.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/shoes/pr_shoes%02d%02da.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/shoes/pr_shoes%02d%02db.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\shoes\\shoes_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/shoes/pr_shoes%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\shoes\\shoes_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/shoes/pr_shoes%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\avatar\\shoes\\shoes_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Avatar/shoes/pr_shoes%02d%02df.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\growtype\\infighter\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/growtype/Infighter.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\axe\\axec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/axe/pr_axe%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\axe\\axec1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/axe/pr_axe%02d%02dc1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\axe\\axec2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/axe/pr_axe%02d%02dc2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\axe\\axed\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/axe/pr_axe%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\axe\\axed1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/axe/pr_axe%02d%02dd1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\axe\\axed2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/axe/pr_axe%02d%02dd2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\cross\\crossc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/cross/pr_cross%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\cross\\crossc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/cross/pr_cross%02d%02dc1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\cross\\crossc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/cross/pr_cross%02d%02dc2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\cross\\crossd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/cross/pr_cross%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\cross\\crossd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/cross/pr_cross%02d%02dd1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\cross\\crossd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/cross/pr_cross%02d%02dd2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\rosary\\rosaryc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/rosary/pr_rosary%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\rosary\\rosaryc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/rosary/pr_rosary%02d%02dc1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\rosary\\rosaryc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/rosary/pr_rosary%02d%02dc2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\rosary\\rosaryd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/rosary/pr_rosary%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\rosary\\rosaryd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/rosary/pr_rosary%02d%02dd1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\rosary\\rosaryd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/rosary/pr_rosary%02d%02dd2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\holyscythec1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dc1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\holyscythec2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dc2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\holyscythed1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dd1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\holyscythed2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dd2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\scythec\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\scythec1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dc1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\scythec2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dc2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\scythed\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\scythed1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dd1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\scythe\\scythed2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/scythe/pr_scythe%02d%02dd2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\totem\\totemc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/totem/pr_totem%02d%02dc.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\totem\\totemc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/totem/pr_totem%02d%02dc1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\totem\\totemc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/totem/pr_totem%02d%02dc2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\totem\\totemd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/totem/pr_totem%02d%02dd.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\totem\\totemd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/totem/pr_totem%02d%02dd1.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n");
                    AddInfo.Key.Add("equipment\\character\\priest\\weapon\\totem\\totemd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Priest/Equipment/Weapon/totem/pr_totem%02d%02dd2.img`\r\n\t\t4\r\n\t[IMAGE POS]\r\n\t\t-222\t-314\r\n\t[DELAY]\r\n\t\t180\r\n");
                    break;
                case 7:
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/belt/sm_belt%02d%02da.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/belt/sm_belt%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/belt/sm_belt%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/belt/sm_belt%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\belt\\belt_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/belt/sm_belt%02d%02de.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\belt\\belt_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/belt/sm_belt%02d%02df.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t150\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\belt\\belt_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/belt/sm_belt%02d%02dh.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/cap/sm_cap%02d%02da.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/cap/sm_cap%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/cap/sm_cap%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\cap\\cap_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/cap/sm_cap%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\cap\\cap_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/cap/sm_cap%02d%02de.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\cap\\cap_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/cap/sm_cap%02d%02df.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t150\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/coat/sm_coat%02d%02da.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/coat/sm_coat%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/coat/sm_coat%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/coat/sm_coat%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\coat\\coat_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/coat/sm_coat%02d%02de.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\coat\\coat_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/coat/sm_coat%02d%02df.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t150\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\coat\\coat_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/coat/sm_coat%02d%02dg.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/face/sm_face%02d%02da.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/face/sm_face%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/face/sm_face%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\face\\face_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/face/sm_face%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\face\\face_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/face/sm_face%02d%02de.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\face\\face_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/face/sm_face%02d%02df.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\face\\face_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/face/sm_face%02d%02dg.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\face\\face_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/face/sm_face%02d%02dh.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/hair/sm_hair%02d%02da.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/hair/sm_hair%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\hair\\hair_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/hair/sm_hair%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/hair/sm_hair%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\hair\\hair_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/hair/sm_hair%02d%02de.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\hair\\hair_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/hair/sm_hair%02d%02df.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\hair\\hair_f1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/hair/sm_hair%02d%02df1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/neck/sm_neck%02d%02da.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/neck/sm_neck%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/neck/sm_neck%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/neck/sm_neck%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/neck/sm_neck%02d%02de.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\neck\\neck_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/neck/sm_neck%02d%02df.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t150\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/neck/sm_neck%02d%02dx.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\neck\\neck_z\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/neck/sm_neck%02d%02dz.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/pants/sm_pants%02d%02da.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/pants/sm_pants%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\pants\\pants_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/pants/sm_pants%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/pants/sm_pants%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\pants\\pants_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/pants/sm_pants%02d%02de.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\pants\\pants_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/pants/sm_pants%02d%02df.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t150\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/shoes/sm_shoes%02d%02da.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/shoes/sm_shoes%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\shoes\\shoes_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/shoes/sm_shoes%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\shoes\\shoes_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/shoes/sm_shoes%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\avatar\\shoes\\shoes_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Avatar/shoes/sm_shoes%02d%02df.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[RGBA]\r\n\t\t255\t255\t255\t150\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\growtype\\asura\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/GrowType/asura.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\growtype\\berserker\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/GrowType/berserker.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\growtype\\berserker_eye\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/GrowType/berserker_eye.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\growtype\\weaponmaster\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/GrowType/weaponmaster.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\beamswdb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\beamswdb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02db1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\beamswdb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02db2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\beamswdb3\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02db1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\beamswdc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\beamswdc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02dc1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\beamswdc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02dc2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\cainusswdb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02db1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\cainusswdc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02dc1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\hellkariumswdb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02db1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\beamsword\\hellkariumswdc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/beamswd/beamswd%02d%02dc1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\clubb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\clubb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02db1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\clubb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02db2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\clubc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\clubc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02dc1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\clubc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02dc2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\clubd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02dd.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\lclubb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\club\\lclubc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/club/club%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\hsword\\lswdb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lswd/lswd%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\hsword\\lswdb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lswd/lswd%02d%02db1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\hsword\\lswdb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lswd/lswd%02d%02db2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\hsword\\lswdc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lswd/lswd%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\hsword\\lswdc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lswd/lswd%02d%02dc1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\hsword\\lswdc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lswd/lswd%02d%02dc2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\hsword\\mswdb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/mswd/mswd%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\hsword\\mswdc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/mswd/mswd%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\katanab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\katanab1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02db1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\katanab2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02db2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\katanac\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\katanac1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02dc1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\katanac2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02dc2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\lkatanab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lkatana/lkatana%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\lkatanac\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lkatana/lkatana%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\sasakib\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\sasakic\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\sayoungb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\katana\\sayoungc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/katana/katana%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\bantub\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/bantu/bantu%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\bantuc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/bantu/bantu%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\bswdb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/boneswd/bswd%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\bswdc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/boneswd/bswd%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\gemswdb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/gemswd/gemswd%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\gemswdc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/gemswd/gemswd%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\lgswdb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lgswd/lgswd%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\lgswdc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/lgswd/lgswd%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\sswdb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/sswd/sswd%02d%02db.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\sswdb1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/sswd/sswd%02d%02db1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\sswdb2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/sswd/sswd%02d%02db2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\sswdc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/sswd/sswd%02d%02dc.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\sswdc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/sswd/sswd%02d%02dc1.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t120\r\n");
                    AddInfo.Key.Add("equipment\\character\\swordman\\weapon\\ssword\\sswdc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Swordman/Equipment/Weapon/sswd/sswd%02d%02dc2.img`\r\n\t\t90\r\n\t[IMAGE POS]\r\n\t\t-232\t-333\r\n\t[DELAY]\r\n\t\t120\r\n");
                    break;
                case 8:
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\belt\\belt_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/belt/th_belt%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\belt\\belt_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/belt/th_belt%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\belt\\belt_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/belt/th_belt%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\belt\\belt_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/belt/th_belt%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\belt\\belt_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/belt/th_belt%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\belt\\belt_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/belt/th_belt%02d%02dg.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\belt\\belt_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/belt/th_belt%02d%02dh.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\cap\\cap_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/cap/th_cap%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\cap\\cap_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/cap/th_cap%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\cap\\cap_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/cap/th_cap%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\cap\\cap_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/cap/th_cap%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\cap\\cap_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/cap/th_cap%02d%02de.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\cap\\cap_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/cap/th_cap%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\coat\\coat_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/coat/th_coat%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\coat\\coat_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/coat/th_coat%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\coat\\coat_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/coat/th_coat%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\coat\\coat_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/coat/th_coat%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\coat\\coat_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/coat/th_coat%02d%02de.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\coat\\coat_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/coat/th_coat%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\coat\\coat_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/coat/th_coat%02d%02dg.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\coat\\coat_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/coat/th_coat%02d%02dh.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\face\\face_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/face/th_face%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\face\\face_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/face/th_face%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\face\\face_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/face/th_face%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\face\\face_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/face/th_face%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\face\\face_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/face/th_face%02d%02dg.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\face\\face_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/face/th_face%02d%02dh.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\hair\\hair_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/hair/th_hair%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\hair\\hair_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/hair/th_hair%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\hair\\hair_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/hair/th_hair%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\hair\\hair_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/hair/th_hair%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\hair\\hair_f1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/hair/th_hair%02d%02df1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_c\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_d1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02dd1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_e\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02de.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_h\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02dh.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_x\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_x1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\neck\\neck_z\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/neck/th_neck%02d%02dz.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t160\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\pants\\pants_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/pants/th_pants%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\pants\\pants_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/pants/th_pants%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\pants\\pants_d\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/pants/th_pants%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\pants\\pants_g\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/pants/th_pants%02d%02dg.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[RGBA]\r\n\t\t255\t255\t255\t170\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\shoes\\shoes_a\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/shoes/th_shoes%02d%02da.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\shoes\\shoes_b\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/shoes/th_shoes%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\avatar\\shoes\\shoes_f\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Avatar/shoes/th_shoes%02d%02df.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n\t[DAMAGE BOX]\r\n\t-15\t-5\t0\t19\t10\t86\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\growtype\\rogue\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Effect/rogue.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\chakram\\chakramb\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\chakram\\chakramc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dc1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dc2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dd1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dd2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerx\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerx1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\dagger\\daggerx2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dx2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dc1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dc2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dd1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dd2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordx\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordx1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\twinsword\\twinswordx2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/twinswd/twinswd%02d%02dx2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\vajra\\vajrab\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02db.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\vajra\\vajrac\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/dagger/dagger%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandc\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dc.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandc1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dc1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandc2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dc2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandd\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dd.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandd1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dd1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandd2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dd2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandx\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dx.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandx1\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dx1.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[GRAPHIC EFFECT]\r\n\t\t`LINEARDODGE`\r\n\t[DELAY]\r\n\t\t150\r\n");
                    AddInfo.Key.Add("equipment\\character\\thief\\weapon\\wand\\wandx2\\stay.ani");
                    AddInfo.Value.Add("[FRAME000]\r\n\t[IMAGE]\r\n\t\t`Character/Thief/Equipment/Weapon/wand/wand%02d%02dx2.img`\r\n\t\t12\r\n\t[IMAGE POS]\r\n\t\t-238\t-327\r\n\t[DELAY]\r\n\t\t150\r\n");
                    break;
            }

            return AddInfo;
        }

        /// <summary>
        /// 提供导出目录，以及查找好的动作文件字典，以及对应的生成文件，自动生成到指定目录
        /// </summary>
        /// <param name="Path">要导出的目录</param>
        /// <param name="ActInfo">查找好的动作文件字典</param>
        /// <param name="AddInfo">需要生成的文件信息</param>
        public void 修复裸体生成修复File(string Path, Dictionary<string, 修复裸体> ActInfo, KeyValuePair<List<string>, List<string>> AddInfo)
        {

            Regex_new regex = new Regex_new();

            foreach (var item in ActInfo.Keys)
            {
                string TouText = "";

                var DaoChuInfo = ActInfo[item];//得到修复裸体类

                TouText += DaoChuInfo.HeadText;//首先加入头信息
                TouText += $"[FRAME MAX]\r\n\t{DaoChuInfo.Info.Key.Count}\r\n\r\n";//再加入帧的总数

                List<string> FilePath = AddInfo.Key;//得到导出的文件路径
                List<string> FileText = AddInfo.Value;//得到导出的文件内容

                for (int i = 0; i < FilePath.Count; i++)
                {
                    StringBuilder AddFileText = new StringBuilder();//需要写出的文本

                    List<string> Id = DaoChuInfo.Info.Key;//得到帧ID
                    List<string> Delay = DaoChuInfo.Info.Value;//得到延时
                    List<string> Pos = DaoChuInfo.Pos;//得到坐标
                    List<string> Rotate = DaoChuInfo.Rotate;//得到旋转角度
                    List<string> Rate = DaoChuInfo.Rate;//得到放大率

                    string CurrentFilePath = FilePath[i];//得到当前的文件路径
                    //得到导出文件路径
                    CurrentFilePath = (Path + "\\" + CurrentFilePath.Remove(CurrentFilePath.Length - 8) + item).Replace("/", "\\");

                    
                    for (int a = 0; a < Id.Count; a++)
                    {
                        string CurrentText = FileText[i];//得到当前的文件内容

                        if (Id[a] == "-1")
                        {
                            //如果为-1则没有路径
                            CurrentText = regex.替换(CurrentText, @"`.+\.img`\r\n[\t]+\d+", "``\r\n\t\t0");
                        }
                        else
                        {
                            //如果有路径则改变帧数
                            regex.创建(CurrentText, @"(`.+\.img`\r\n[\t]+)\d+", 1);

                            CurrentText = CurrentText.Replace(regex.取匹配文本(1), regex.取子匹配文本(1, 1) + Id[a]);
                        }

                        if (Delay[a] == "-1")
                        {
                            //如果为-1则没有延时
                            CurrentText = regex.替换(CurrentText, @"\[DELAY\]\r\n[\t]+\d+", "");
                        }
                        else
                        {
                            //如果有路径则改变延时数
                            regex.创建(CurrentText, @"(\[DELAY\]\r\n[\t]+)\d+", 1);

                            CurrentText = CurrentText.Replace(regex.取匹配文本(1), regex.取子匹配文本(1, 1) + Delay[a]);
                        }

                        //如果旋转角度不为-1 代表有旋转角度
                        if (Rotate[a] != "-1")
                        {
                            //加入旋转角度
                            CurrentText = CurrentText.Replace("[DELAY]", "\t[IMAGE ROTATE]\r\n\t\t" + Rotate[a] + "\r\n[DELAY]");
                        }

                        //如果放大率不为-1 代表有放大
                        if (Rate[a] != "-1")
                        {
                            //加入放大率
                            CurrentText = CurrentText.Replace("[DELAY]", "\t[IMAGE RATE]\r\n\t\t" + Rate[a] + "\r\n[DELAY]");
                        }
                        

                        //改变每一帧的坐标值
                        regex.创建(CurrentText, @"(\[IMAGE POS\]\r\n[\t]+).+\r\n", 1);
                        CurrentText = CurrentText.Replace(regex.取匹配文本(1), regex.取子匹配文本(1, 1) + Pos[a]+"\r\n");

                        AddFileText.Append(CurrentText);
                    }

                    Directory.CreateDirectory(DirectoryGetParent(CurrentFilePath));//创建目录
                    File.WriteAllText(CurrentFilePath, TouText + AddFileText.ToString());//写出修复好的文件
                }
            }

        }


        /// <summary>
        /// 根据PVF导出的目录 自动生成switch语句
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="DaiMa"></param>
        public void 修复裸体整理代码(string Path, StringBuilder DaiMa)
        {
            List<string> dirs = new List<string>(Directory.EnumerateFiles(Path, "*.ani", SearchOption.AllDirectories));

            DaiMa.Append("KeyValuePair<List<string>, List<string>> AddInfo = new KeyValuePair<List<string>, List<string>>(new List<string>(),new List<string>());\r\n\r\n" +
                "switch (ChrId)\r\n" +
                "{\r\n");


            for (int i = 0; i < 9; i++)
            {
                DaiMa.Append($"   case {i}:\r\n");

                foreach (string item in dirs)
                {
                    string AllText = File.ReadAllText(item);

                    Regex_new regex = new Regex_new();

                    string Zhen = "";

                    switch (i)
                    {
                        case 0:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Fighter/ATEquipment|`Character/Fighter/Effect", 1);
                            break;
                        case 1:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Fighter/Equipment|`Character/Fighter/Effect", 1);
                            break;
                        case 2:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Gunner/ATEquipment|`Character/Gunner/Effect", 1);
                            break;
                        case 3:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Gunner/Equipment|`Character/Gunner/Effect", 1);
                            break;
                        case 4:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Mage/ATEquipment|`Character/Mage/Effect", 1);
                            break;
                        case 5:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Mage/Equipment|`Character/Mage/Effect", 1);
                            break;
                        case 6:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Priest/Equipment|`Character/Priest/Effect", 1);
                            break;
                        case 7:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Swordman/Equipment|`Character/Swordman/Effect", 1);
                            break;
                        case 8:
                            regex.创建(AllText, @"(\[FRAME\d+\][\S\s]*?)\r\n\[FRAME\d+\]", 1);
                            Zhen = regex.取子匹配文本(1, 1);
                            regex.创建(Zhen.Replace("\\", "/"), @"`Character/Thief/Equipment|`Character/Thief/Effect", 1);
                            break;
                    }

                    if (regex.取匹配数量() > 0)
                    {
                        Zhen = Zhen.Replace("\t", "\\t").Replace("\r\n", "\\r\\n");
                        string FilePath = item.Substring(item.IndexOf("equipment"));
                        DaiMa.Append($"       AddInfo.Key.Add(\"{FilePath.Replace("\\", "\\\\")}\");\r\n       AddInfo.Value.Add(\"{Zhen}\");\r\n");
                    }

                }

                DaiMa.Append("       break;\r\n");
            }

            DaiMa.Append("}\r\n");

        }

        /// <summary>
        /// 打乱nut脚本文件内容
        /// </summary>
        /// <param name="Path">需要打乱的目录</param>
        /// <param name="FileExName">文件扩展名</param>
        /// <param name="coding">文件编码</param>
        /// <param name="AddText">打乱后增加的文本</param>
        public void SqrNut打乱(string Path,string FileExName,string coding, string AddText,string AddGG)
        {
            //枚举一个目录下的所有文件
            List<string> dirs = new List<string>(Directory.EnumerateFiles(Path, $"*.{FileExName}", SearchOption.AllDirectories));

            coding = new Regex_new().替换(coding,@"\(.+\)","");//替换掉括号跟括号内的文本

            //枚举出所有文件路径
            foreach (string item in dirs)
            {
                string AllText = File.ReadAllText(item,Encoding.GetEncoding(coding));//按指定编码打开

                Regex_new regex = new Regex_new();

                AllText = regex.替换(AllText,"//.+\r\n","\r\n");//替换掉注释

                AllText = regex.替换(AllText, "local[ \t]+", "local ");//规整变量声明格式

                AllText = regex.替换(AllText, "[ \t]{2,}", "");//删除所有多余空格跟制表符
                AllText = AllText.Replace("{", "\r\n{\r\n");//替换大括号换行
                AllText = AllText.Replace("}", "\r\n}\r\n");//替换大括号换行
                AllText = AllText.Replace("for (", "for(");//替换循环的中间空格
                AllText = AllText.Replace(",", " , ");//替换逗号多加一个空格

                regex.创建(AllText, @"=([^=]+$)", 2);
                if (regex.取匹配数量() > 0)
                {
                    foreach (Match itemex in regex.正则返回集合())
                    {
                        AllText = AllText.Replace(itemex.Value, "= " + itemex.Groups[1].Value);
                    }
                }

                AllText = regex.替换(AllText, "(\r\n){2,}", "\r\n");//删除所有多余换行

                regex.创建(AllText, @"(.+)/(.+)",0);
                if(regex.取匹配数量()>0)
                {
                    foreach (Match itemex in regex.正则返回集合())
                    {
                        if(!itemex.Value.Contains(@""""))
                            AllText = AllText.Replace(itemex.Value, itemex.Groups[1].Value+" / "+ itemex.Groups[2].Value);
                    }
                }



                List<string> EndText = new List<string>();//记录找出的函数
                List<string> ExText = new List<string>();//记录打乱后的函数

                GetAllFunction_Nut(AllText, EndText);//找出所有带变量的函数
                DlFunctionToLocal_Nut(EndText, ExText, AddGG);//打乱函数 并且加入到 ExText集合中
                AllText = ThEndText_Nut(AllText, EndText, ExText);//再替换原来的文本并返回

                //增加广告文本
                AllText = AllText.Replace("\r\n",$"\r\n{AddText}\r\n");

                //替换完成后写出文件
                File.WriteAllText(item, AllText, Encoding.GetEncoding(coding));
            }
        }

        /// <summary>
        /// 得到一个nut文本中带有变量的函数反馈给参数中的集合内
        /// </summary>
        /// <param name="text">被寻找的nut文本</param>
        /// <param name="EndText">寻找后加入的list集合</param>
        public void GetAllFunction_Nut(string text , List<string> EndText)
        {

            //开始寻找所有带变量的函数
            if (text.Contains("local"))//如果整个文本中存在变量名local
            {
                //换行分割所有文本
                string[] AllStTetx = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder AddText = new StringBuilder();//声明一个可变长度字符串

                bool Yes = false;//用于记录是否存储获取文本
                int new_kh1 = 0;//用于记录 { 出现次数
                int new_kh2 = 0;//用于记录 } 出现次数

                foreach (string itemex in AllStTetx)//枚举出字符串数组中的所有字符串
                {
                    string GbText = itemex;//赋值给新变量

                    if (GbText.Contains("function "))//如果找到了函数名
                        Yes = true;//改变为true 开始存储文本

                    if(Yes)//如果为true 
                    {
                        if (GbText.Contains("{"))//如果找到 { 变量记录次数加一
                            new_kh1++;
                        else if (GbText.Contains("}"))
                            new_kh2++;

                        AddText.Append(GbText+"\r\n");//增加文本

                        //当左右括号寻找数量对等且 左括号计次数量大于0
                        if (new_kh1 == new_kh2 && new_kh1>0)
                        {
                            EndText.Add(AddText.ToString());//加入到list
                            AddText.Clear();//清除字符串
                            Yes = false;//改变为false
                            new_kh1 = 0;//重新赋值数值
                            new_kh2 = 0;//重庆赋值数值
                        }
                    }
                }

                new List<string>(EndText).ForEach((text) => {
                    if (!text.Contains("local "))//如果整个文本中不存在变量
                        EndText.Remove(text);//清除没有变量的文本
                });

            }
        }

        /// <summary>
        /// 在多个函数中打乱变量名
        /// </summary>
        /// <param name="EndText">函数集合</param>
        /// <param name="AddGG">打乱的文本</param>
        public void DlFunctionToLocal_Nut(List<string> EndText,List<string>ExText,string AddGG)
        {
            Regex_new regex = new Regex_new();

            for (int a=0;a< EndText.Count;a++)
            {
                int AddInt = 1;
                string thText = EndText[a];

                List<string> local = new List<string>();
                List<string> localEx = new List<string>();

                regex.创建(thText, @".*( |)local ([\s\S]*?)( |=)", 1);
                if (regex.取匹配数量() > 0)
                    foreach (Match item in regex.正则返回集合())
                    {
                        string blName = item.Value;
                        if (blName.Contains("for(")) continue;

                        if(!local.Contains(item.Groups[2].Value))
                        {
                            local.Add(item.Groups[2].Value);
                            localEx.Add(AddGG + AddInt);
                        }
                        AddInt++;
                    }


                    if (local.Count>0)
                    {
                        for (int i=0;i< local.Count;i++)
                        {
                            regex.创建(thText, @$"([^a-zA-Z\./_]|\r\n)({Regex.Escape(local[i])})([^\(""a-zA-Z])", 0);
                            if (regex.取匹配数量() > 0)
                                foreach (Match itemex in regex.正则返回集合())
                                {
                                Regex regex1 = new Regex(Regex.Escape(itemex.Value));
                                thText = regex1.Replace(thText, itemex.Groups[1].Value + localEx[i] + itemex.Groups[3].Value, 1) ;
                                }
                        }
                    }
                ExText.Add(thText);
            }
        }

        /// <summary>
        /// 把打乱后的函数替换到原本的函数中
        /// </summary>
        /// <param name="text">被替换</param>
        /// <param name="EndText"></param>
        /// <param name="ExText"></param>
        public string ThEndText_Nut(string text, List<string> EndText, List<string> ExText)
        {
            for (int i=0;i< EndText.Count;i++ )
            {
                Regex regex = new Regex(Regex.Escape(EndText[i]));//正则文本
                text = regex.Replace(text, ExText[i],1);//替换1次
            }
            return text;
        }

    }

    /// <summary>
    /// 用于修复动作文件的裸体使用
    /// </summary>
    public class 修复裸体
    {
        /// <summary>
        /// 头文本 例如：循环或阴影
        /// </summary>
        public string HeadText { set; get; }

        /// <summary>
        /// 储存每一帧的ID跟延时 ID为-1时没有路径 帧为-1时没有帧延时
        /// </summary>
        public KeyValuePair<List<string>, List<string>> Info { set; get; }

        /// <summary>
        /// 每一帧的POS坐标
        /// </summary>
        public List<string> Pos { set; get; }

        /// <summary>
        /// 每一帧的旋转角度
        /// </summary>
        public List<string> Rotate { set; get; }

        /// <summary>
        /// 每一帧的放大率
        /// </summary>
        public List<string> Rate { set; get; }


        public 修复裸体(string AddText, KeyValuePair<List<string>, List<string>> AddInfo, List<string> AddPos, List<string> AddRotate, List<string> AddRate)
        {
            HeadText = AddText;
            Info = AddInfo;
            Pos = AddPos;
            Rotate = AddRotate;
            Rate = AddRate;
        }

    }



}
