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

namespace PVFSimple
{
    public class dnf
    {

        /// <summary>
        /// 查找指定文件中的img路径
        /// </summary>
        /// <param name="path">查找的目录</param>
        /// <param name="扩展名">*.*代表所有 *.exe代表应用程序</param>
        /// <param name="去重复">false代表去重复 true代表不去重</param>
        /// <param name="img_path">查找的img路径</param>
        /// <param name="npkimg_path">转成npk内的img全路径</param>
        /// <param name="npk_txt">转成npk的文件名</param>
        public void 寻找img路径(string path,string 扩展名, bool 去重复,ref string img_path,ref string npkimg_path,ref string npk_txt)
        {
            List<string> imgalltext = new List<string>();
            List<string> npktxt = new List<string>();
            
            regex_new regex = new regex_new();
            List<string> dirs = new List<string>(Directory.EnumerateFiles(path, 扩展名, SearchOption.AllDirectories));
            foreach (var item in dirs)
            {
                string alltext = File.ReadAllText(item, Encoding.UTF8);
                regex.创建(alltext, @"`(.+\.img)`", 1);
                if (regex.取匹配数量()>0)
                {
                    regex.子匹配加入集合(imgalltext, 1, 去重复, 2);
                }
            }

            foreach (var item in imgalltext)
            {
                string imgtxt = item.ToLower().Replace(@"\","/");
                imgtxt = regex.替换(imgtxt,"[/]{2,}|^[/]+","");
                img_path = img_path + imgtxt + "\r\n";

                npkimg_path = npkimg_path  + "sprite/" + imgtxt + "\r\n";

                regex.创建(imgtxt,@"(.+)/.+\.img",1);

                string lsnpk = regex.取子匹配文本(1, 1).Replace("/", "_") + ".NPK";
                if (npktxt.Contains(lsnpk) == false)
                {
                    npktxt.Add(lsnpk);
                }
            }

            foreach (var item in npktxt)
            {
                npk_txt = npk_txt + "sprite_" + item + "\r\n";
            }

            img_path = "寻找img路径遍历的文件总数为：" + imgalltext.Count.ToString() + "个\r\n\r\n" + img_path;
            npkimg_path = "寻找img路径遍历的文件总数为：" + imgalltext.Count.ToString() + "个\r\n\r\n" + npkimg_path;
            npk_txt = "转换为npk全名的文件总数为：" + npktxt.Count.ToString() + "个\r\n\r\n" + npk_txt;
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
                if (path.Contains(@"..\") == true)
                {
                    regex_new regex = new regex_new();

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

                    if (是否为小写 == true)
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
                regex_new regex = new regex_new();
                List<string> temporary_anipath = new List<string>(ani_path);//接收传过来的ani全路径到临时ani
                List<string> temporary_alspath = new List<string>();//创建临时als集合

                do
                {
                    temporary_alspath.Clear();//首先清空临时als集合，确保循环正常运行

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

                    als_path.AddRange(temporary_alspath);//查找出来的als加入到参数als

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
                    {
                        if (ani_path.Contains(anipath) == false)
                        {
                            ani_path.Add(anipath);
                        }
                    }

                } while (temporary_alspath.Count != 0);//循环遍历 临时ani数量不等于0


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
                    string aipath = Directory.GetParent(Directory.GetParent(item).ToString()).ToString().ToLower();
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
        /// <param name="lsttext">dungeon.lst文件内容</param>
        /// <param name="id">副本编号集合</param>
        /// <param name="lst">副本lst集合</param>
        /// <param name="path">副本全路径集合</param>
        /// <param name="id_pd"></param>
        /// <param name="lst_pd"></param>
        /// <param name="path_pd"></param>
        public void id_lst_副本(string pathex, string lsttext, List<string> id, List<string> lst, List<string> path)
        {

            if (id.Count>0)
            {
                regex_new 正则 = new regex_new();
                pathex = pathex.ToLower();
                foreach (var item in id)
                {
                       
                    正则.创建(lsttext, "\r\n(" + item + @"\t`)(.+\.dgn)`", 1);
                    if (正则.取匹配数量()>0)
                    {
                        string 子文本1 = 正则.取子匹配文本(1, 1);
                        string 子文本2 = 正则.取子匹配文本(1, 2);

                        //为了避免不存在也提取lst，加一个判断文件是否存在
                        if (File.Exists(pathex + @"\dungeon\" + 子文本2) == true)
                        {
                            if (lst != null)
                            {
                                if (lst.Contains(子文本1 + 子文本2 + "`") == false)
                                {
                                    lst.Add(子文本1 + 子文本2 + "`");
                                }
                                
                            }

                            if (path != null)
                            {
                                子文本2 = 子文本2.ToLower();
                                if (path.Contains(pathex + @"\dungeon\" + 子文本2) == false)
                                {
                                    path.Add(pathex + @"\dungeon\" + 子文本2);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据全路径查找编号以及lst
        /// </summary>
        /// <param name="lsttext">map.lst文件内容</param>
        /// <param name="id">地图编号集合</param>
        /// <param name="lst">地图lst集合</param>
        /// <param name="path">地图全路径集合</param>
        public void id_lst_地图(string lsttext, List<string> id, List<string> lst, List<string> path)
        {
            if (path.Count > 0)
            {
                regex_new 正则 = new regex_new();
                List<string> new_path = new List<string>();

                foreach (var item in path)
                {

                    string cs_path = item;
                    
                    正则.创建(lsttext, "([0-9]+)(\t`" + 正则.取消特殊字符(cs_path.Substring(cs_path.IndexOf(@"\map\")+5)) + "`)", 1);
                    if (正则.取匹配数量() > 0)
                    {
                        string ztxt1 = 正则.取子匹配文本(1,1);
                        string ztxt2 = 正则.取子匹配文本(1,2);

                        if (id != null)
                        {
                            id.Add(ztxt1);
                        }

                        if (lst != null)
                        {
                            lst.Add(ztxt1 + ztxt2.Replace("\\", "/"));
                        }

                        if (path != null)
                        {
                            new_path.Add(cs_path);
                        }
                    }
                }

                if (new_path.Count>0)
                {
                    
                    path.Clear();
                    path.AddRange(new_path);
                }

            }

        }

        /// <summary>
        /// 根据编号查找lst以及全路径
        /// </summary>
        /// <param name="pathex">所在目录(目录下有monster目录)</param>
        /// <param name="lsttext">monster.lst文件内容</param>
        /// <param name="id">怪物编号集合</param>
        /// <param name="lst">怪物lst集合</param>
        /// <param name="path">怪物全路径集合</param>
        public void id_lst_怪物(string pathex, string lsttext, List<string> id, List<string> lst, List<string> path)
        {

            if (id.Count > 0)
            {
                regex_new 正则 = new regex_new();
                pathex = pathex.ToLower();
                foreach (var item in id)
                {

                    正则.创建(lsttext, "\r\n(" + item + @"\t`)(.+\.mob)`", 1);
                    if (正则.取匹配数量() > 0)
                    {
                        string 子文本1 = 正则.取子匹配文本(1, 1);
                        string 子文本2 = 正则.取子匹配文本(1, 2);

                        //为了避免不存在也提取lst，加一个判断文件是否存在
                        if (File.Exists(pathex + @"\monster\" + 子文本2) == true)
                        {
                            if (lst != null)
                            {
                                if (lst.Contains(子文本1 + 子文本2 + "`") == false)
                                {
                                    lst.Add(子文本1 + 子文本2 + "`");
                                }

                            }

                            if (path != null)
                            {
                                子文本2 = 子文本2.ToLower();
                                if (path.Contains(pathex + @"\monster\" + 子文本2) == false)
                                {
                                    path.Add(pathex + @"\monster\" + 子文本2);
                                }

                            }
                        }
                        
                    }
                }
            }
        }

        /// <summary>
        /// 根据编号查找lst以及全路径
        /// </summary>
        /// <param name="pathex">所在目录(目录下有passiveobject目录)</param>
        /// <param name="lsttext">passiveobject.lst文件内容</param>
        /// <param name="id">特效编号集合</param>
        /// <param name="lst">特效lst集合</param>
        /// <param name="path">特效全路径集合</param>
        public void id_lst_特效(string pathex, string lsttext, List<string> id, List<string> lst, List<string> path)
        {
            if (id.Count > 0)
            {
                regex_new 正则 = new regex_new();
                pathex = pathex.ToLower();
                foreach (var item in id)
                {

                    正则.创建(lsttext, "\r\n(" + item + @"\t`)(.+\.obj)`", 1);
                    if (正则.取匹配数量() > 0)
                    {
                        string 子文本1 = 正则.取子匹配文本(1, 1);
                        string 子文本2 = 正则.取子匹配文本(1, 2);

                        //为了避免不存在也提取lst，加一个判断文件是否存在
                        if (File.Exists(pathex + @"\passiveobject\" + 子文本2) == true)
                        {
                            if (lst != null)
                            {
                                if (lst.Contains(子文本1 + 子文本2 + "`") == false)
                                {
                                    lst.Add(子文本1 + 子文本2 + "`");
                                }

                            }

                            if (path != null)
                            {
                                子文本2 = 子文本2.ToLower();
                                if (path.Contains(pathex + @"\passiveobject\" + 子文本2) == false)
                                {
                                    path.Add(pathex + @"\passiveobject\" + 子文本2);
                                }

                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据编号查找lst以及全路径
        /// </summary>
        /// <param name="pathex">所在目录(目录下有aicharacter目录)</param>
        /// <param name="lsttext">aicharacter.lst文件内容</param>
        /// <param name="id">apc编号集合</param>
        /// <param name="lst">apclst集合</param>
        /// <param name="path">apc全路径集合</param>
        public void id_lst_人偶(string pathex, string lsttext, List<string> id, List<string> lst, List<string> path)
        {
            if (id.Count > 0)
            {
                regex_new 正则 = new regex_new();
                pathex = pathex.ToLower();
                foreach (var item in id)
                {

                    正则.创建(lsttext, "\r\n(" + item + @"\t`)(.+\.aic)`", 1);
                    if (正则.取匹配数量() > 0)
                    {
                        string 子文本1 = 正则.取子匹配文本(1, 1);
                        string 子文本2 = 正则.取子匹配文本(1, 2);

                        //为了避免不存在也提取lst，加一个判断文件是否存在
                        if (File.Exists(pathex + @"\aicharacter\" + 子文本2) == true)
                        {
                            if (lst != null)
                            {
                                if (lst.Contains(子文本1 + 子文本2 + "`") == false)
                                {
                                    lst.Add(子文本1 + 子文本2 + "`");
                                }

                            }

                            if (path != null)
                            {
                                子文本2 = 子文本2.ToLower();
                                if (path.Contains(pathex + @"\aicharacter\" + 子文本2) == false)
                                {
                                    path.Add(pathex + @"\aicharacter\" + 子文本2);
                                }

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据编号查找lst以及全路径
        /// </summary>
        /// <param name="pathex">所在目录(目录下有town目录)</param>
        /// <param name="lsttext">town.lst文件内容</param>
        /// <param name="id">town编号集合</param>
        /// <param name="lst">townlst集合</param>
        /// <param name="path">town全路径集合</param>
        public void id_lst_城镇(string pathex, string lsttext, List<string> id, List<string> lst, List<string> path)
        {
            if (id.Count > 0)
            {
                regex_new 正则 = new regex_new();
                pathex = pathex.ToLower();
                foreach (var item in id)
                {

                    正则.创建(lsttext, "\r\n(" + item + @"\t`)(.+\.twn)`", 1);
                    if (正则.取匹配数量() > 0)
                    {
                        string 子文本1 = 正则.取子匹配文本(1, 1);
                        string 子文本2 = 正则.取子匹配文本(1, 2);

                        //为了避免不存在也提取lst，加一个判断文件是否存在
                        if (File.Exists(pathex + @"\town\" + 子文本2) == true)
                        {
                            if (lst != null)
                            {
                                if (lst.Contains(子文本1 + 子文本2 + "`") == false)
                                {
                                    lst.Add(子文本1 + 子文本2 + "`");
                                }

                            }

                            if (path != null)
                            {
                                子文本2 = 子文本2.ToLower();
                                if (path.Contains(pathex + @"\town\" + 子文本2) == false)
                                {
                                    path.Add(pathex + @"\town\" + 子文本2);
                                }

                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据编号查找lst以及全路径
        /// </summary>
        /// <param name="pathex">所在目录(目录下有appendage目录)</param>
        /// <param name="lsttext">appendage.lst文件内容</param>
        /// <param name="id">apd编号集合</param>
        /// <param name="lst">apdlst集合</param>
        /// <param name="path">apd全路径集合</param>
        public void id_lst_apd(string pathex, string lsttext, List<string> id, List<string> lst, List<string> path)
        {
            if (id.Count > 0)
            {
                regex_new 正则 = new regex_new();
                pathex = pathex.ToLower();
                foreach (var item in id)
                {

                    正则.创建(lsttext, "\r\n(" + item + @"\t`)(.+\.apd)`", 1);
                    if (正则.取匹配数量() > 0)
                    {
                        string 子文本1 = 正则.取子匹配文本(1, 1);
                        string 子文本2 = 正则.取子匹配文本(1, 2);

                        //为了避免不存在也提取lst，加一个判断文件是否存在
                        if (File.Exists(pathex + @"\appendage\" + 子文本2) == true)
                        {
                            if (lst != null)
                            {
                                if (lst.Contains(子文本1 + 子文本2 + "`") == false)
                                {
                                    lst.Add(子文本1 + 子文本2 + "`");
                                }

                            }

                            if (path != null)
                            {
                                子文本2 = 子文本2.ToLower();
                                if (path.Contains(pathex + @"\appendage\" + 子文本2) == false)
                                {
                                    path.Add(pathex + @"\appendage\" + 子文本2);
                                }

                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据编号查找lst以及全路径
        /// </summary>
        /// <param name="pathex">所在目录(目录下有npc目录)</param>
        /// <param name="lsttext">npc.lst文件内容</param>
        /// <param name="id">npc编号集合</param>
        /// <param name="lst">npclst集合</param>
        /// <param name="path">npc全路径集合</param>
        public void id_lst_npc(string pathex, string lsttext, List<string> id, List<string> lst, List<string> path)
        {
            if (id.Count > 0)
            {
                regex_new 正则 = new regex_new();
                pathex = pathex.ToLower();
                foreach (var item in id)
                {

                    正则.创建(lsttext, "\r\n(" + item + @"\t`)(.+\.npc)`", 1);
                    if (正则.取匹配数量() > 0)
                    {
                        string 子文本1 = 正则.取子匹配文本(1, 1);
                        string 子文本2 = 正则.取子匹配文本(1, 2);

                        //为了避免不存在也提取lst，加一个判断文件是否存在
                        if (File.Exists(pathex + @"\npc\" + 子文本2) == true)
                        {
                            if (lst != null)
                            {
                                if (lst.Contains(子文本1 + 子文本2 + "`") == false)
                                {
                                    lst.Add(子文本1 + 子文本2 + "`");
                                }

                            }

                            if (path != null)
                            {
                                子文本2 = 子文本2.ToLower();
                                if (path.Contains(pathex + @"\npc\" + 子文本2) == false)
                                {
                                    path.Add(pathex + @"\npc\" + 子文本2);
                                }

                            }
                        }
                    }
                }
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
        /// <param name="tbl_pd">true:加入 false:不加入</param>
        /// <param name="act_pd">true:加入 false:不加入</param>
        /// <param name="atk_pd">true:加入 false:不加入</param>
        /// <param name="ani_pd">true:加入 false:不加入</param>
        /// <param name="ptl_pd">true:加入 false:不加入</param>
        /// <param name="til_pd">true:加入 false:不加入</param>
        /// <param name="key_pd">true:加入 false:不加入</param>
        /// <param name="ai_pd">true:加入 false:不加入</param>
        /// <param name="equ_pd">true:加入 false:不加入</param>
        public void 寻找全路径(string pvf文件目录,List<string> path,List<string> tbl, List<string> act, List<string> atk, List<string> ani, List<string> ptl, List<string> til, List<string> key, List<string> ai, List<string> equ)
        {
            if (path.Count>0)
            {
                List<string> 临时act = new List<string>();

                if (pvf文件目录 != null)
                {
                    pvf文件目录 = pvf文件目录.ToLower();
                }
                
                string exname = Path.GetExtension(path[0]).ToLower();

                foreach (var item in path)
                {
                    if (File.Exists(item) ==true)
                    {
                        string alltext = File.ReadAllText(item, Encoding.UTF8);
                        regex_new regex = new regex_new();
                        regex.创建(alltext, @"(`.+\.tbl`)|(`.+\.act`)|(`.+\.atk`)|(`.+\.ani`)|(`.+\.ptl`)|(`.+\.til`)|(`.+\.key`)|(`.+\.ai`)|(`.+\.equ`)", 1);
                        if (regex.取匹配数量() > 0)
                        {
                            string f_path = Directory.GetParent(item.ToString()).ToString().ToLower();

                            List<string> regex_pp = new List<string>();
                            regex.匹配加入集合(regex_pp, false, 2);

                            foreach (var item2 in regex_pp)
                            {
                                string pp_path = item2.Replace("/","\\");
                                if (pp_path.Contains("...\\") == false)
                                {
                                    pp_path = pp_path.Replace("`", "").ToLower();
                                    string kzm = Path.GetExtension(pp_path);
                                    if (kzm == ".key" && exname == ".aic" && key != null)
                                    {
                                        if (key.Contains(f_path + @"\key\" + pp_path) == false)
                                        {
                                            key.Add(f_path + @"\key\" + pp_path);
                                        }
                                    }
                                    else if (kzm == ".ai" && ai != null)
                                    {
                                        if (exname == ".aic")
                                        {
                                            if (ai.Contains(f_path + @"\ai\" + pp_path) == false)
                                            {
                                                ai.Add(f_path + @"\ai\" + pp_path);
                                            }
                                        }
                                        else if (exname == ".mob" || exname == ".ai")
                                        {
                                            string lins = this.识别点点杠(f_path + @"\" + pp_path, false);
                                            if (ai.Contains(lins) == false)
                                            {
                                                ai.Add(lins);
                                            }
                                        }
                                    }
                                    else if (kzm == ".tbl" && tbl != null)
                                    {
                                        if (exname == ".dgn")
                                        {
                                            if (tbl.Contains(pvf文件目录 + @"\" + pp_path) == false)
                                            {
                                                tbl.Add(pvf文件目录 + @"\" + pp_path);
                                            }

                                        }
                                        else if (exname == ".mob")
                                        {
                                            if (tbl.Contains(pvf文件目录 + @"\monster\" + pp_path) == false)
                                            {
                                                tbl.Add(pvf文件目录 + @"\monster\" + pp_path);
                                            }
                                        }
                                    }
                                    else if (kzm == ".act" && act != null)
                                    {
                                        if (exname != ".act")
                                        {
                                            string lins = this.识别点点杠(f_path + @"\" + pp_path, false);
                                            if (act.Contains(lins) == false)
                                            {
                                                act.Add(lins);
                                            }
                                        }
                                        else if (exname == ".act")
                                        {
                                            string lins = this.识别点点杠(f_path + @"\" + pp_path, false);
                                            if (act.Contains(lins) == false)
                                            {
                                                临时act.Add(lins);
                                            }
                                        }

                                    }
                                    else if (kzm == ".atk" && atk != null)
                                    {
                                        string lins = this.识别点点杠(f_path + @"\" + pp_path, false);
                                        if (atk.Contains(lins) == false)
                                        {
                                            atk.Add(lins);
                                        }
                                    }
                                    else if (kzm == ".ani" && ani != null)
                                    {
                                        string lins = this.识别点点杠(f_path + @"\" + pp_path, false);
                                        if (ani.Contains(lins) == false)
                                        {
                                            ani.Add(lins);
                                        }
                                    }
                                    else if (kzm == ".ptl" && ptl != null)
                                    {
                                        if (exname == ".key")
                                        {
                                            string lins = this.识别点点杠(Directory.GetParent(f_path) + @"\" + pp_path, false);
                                            if (ptl.Contains(lins) == false)
                                            {
                                                ptl.Add(lins);
                                            }
                                        }
                                        else
                                        {
                                            string lins = this.识别点点杠(f_path + @"\" + pp_path, false);
                                            if (ptl.Contains(lins) == false)
                                            {
                                                ptl.Add(lins);
                                            }
                                        }

                                    }
                                    else if (kzm == ".til" && til != null)
                                    {
                                        string lins = this.识别点点杠(f_path + @"\" + pp_path, false);
                                        if (til.Contains(lins) == false)
                                        {
                                            til.Add(lins);
                                        }
                                    }
                                    else if (kzm == ".equ" && equ != null)
                                    {
                                        if (exname == ".mob" && equ.Contains(pvf文件目录 + @"\equipment\" + pp_path) == false)
                                        {
                                            equ.Add(pvf文件目录 + @"\equipment\" + pp_path);
                                        }
                                    }
                                }
                                
                            }

                            if (exname == ".act" && ani != null)
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
        /// <param name="map_pd">判断map编号是否加入集合</param>
        /// <param name="obj_pd">判断obj编号是否加入集合</param>
        /// <param name="mob_pd">判断mob编号是否加入集合</param>
        /// <param name="apc_pd">判断apc编号是否加入集合</param>
        public void 寻找编号(List<string> path, List<string> map, List<string> obj, List<string> mob, List<string> apc)
        {
            if (path.Count>0)//判断所提供的集合是不是空的
            {
                regex_new regex = new regex_new();
                string exname = Path.GetExtension(path[0]);
                exname = exname.ToLower();//取出扩展名并且转为小写；方便判断查找编号

                foreach (var item in path)//循环只读所有全路径
                {
                    if (File.Exists(item) ==true)//判断所提供的全路径文件是否存在
                    {
                        string alltext = File.ReadAllText(item, Encoding.UTF8);//以utf8编码读入文件所有内容
                        if (exname == ".dgn")//当提供的集合扩展名为dgn(副本)时
                        {
                            if (obj != null)//提供参数 确认加入obj编号那么继续运行
                            {
                                regex.创建(alltext, @"\[pathgate object\]\r\n(.+\t)\r\n",1);//正则捕获所有obj编号
                                if (regex.取匹配数量()>0)//确认找到了 那么继续运行
                                {
                                    string objtext = regex.取子匹配文本(1,1);//接收匹配
                                    regex.创建(objtext, @"[\x20-\x7f]+",1);//只取出所有obj编号
                                    foreach (var dgn_obj in regex.正则返回集合())//循环获取obj编号
                                    {
                                        string obj_id = dgn_obj.ToString();//接收obj编号
                                        if (obj_id != "-1" && obj.Contains(obj_id) == false)//判断不等于-1 且 不在参数obj集合中 就加入集合
                                        {
                                            obj.Add(obj_id);
                                        }
                                    }
                                }
                                
                            }

                            if (map != null)//判断 副本是否搜索map编号 如果是那么继续
                            {
                                regex.创建(alltext, @"\[seal door map index\]\r\n([\x20-\x7f]+)\t",1);//捕获深渊map房间
                                string hell_map_id =regex.取子匹配文本(1,1);//取出深渊map房间
                                if (hell_map_id != "-1" && map.Contains(hell_map_id) == false)//判断不等于-1 且 不在参数map集合中 就加入集合
                                {
                                    map.Add(hell_map_id);
                                }
                            }
                        }
                        else if (exname == ".map")//当提供的集合扩展名为map(地图)时
                        {
                            if (obj != null)//判断参数是否允许提取特效编号
                            {
                                regex.创建(alltext, @"\[passive object\]\r\n(.+\t)\r\n\[/passive object\]", 1);//捕获特效所有文本
                                if (regex.取匹配数量()>0)//检查是否匹配到
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

                                regex.创建(alltext, @"\[special passive object\](\r\n[\s\S]*?)\r\n\[/special passive object\]",1);//捕获<特殊>特效
                                if (regex.取匹配数量() > 0)//检查是否匹配到特殊特效
                                {
                                    string ts_obj = regex.取子匹配文本(1, 1);
                                    regex.创建(ts_obj, @"[\t\r\n]([0-9]+)\t([\x20-\x7f]+\t){3}0", 1);//捕获特殊特效的单纯特效编号
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(obj,1,false,0);
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
                                regex.创建(alltext, @"\[monster\](\r\n[\s\S]*?)\r\n\[/monster\]",1);//捕获怪物编号中间的内容
                                if (regex.取匹配数量()>0)//检查是否匹配到
                                {
                                    string mob_txt = regex.取子匹配文本(1,1);//取出匹配怪物中间的文本
                                    regex.创建(mob_txt, @"[\r\n\t]([0-9]+)\t([\x20-\x7f]+\t){7}`\[[a-z]+\]`\r\n`\[[a-z]+\]`",1);//捕获怪物编号
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(mob,1,false,0);
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
                                if (regex.取匹配数量()>0)//检查是否匹配到
                                {
                                    string apc_txt = regex.取子匹配文本(1,1);//取出中间的内容
                                    regex.创建(apc_txt, @"[\r\n\t]([0-9]+)\t([\x20-\x7f]+\t){3}`\[[a-z]+\]`\r\n`\[[a-z]+\]`", 1);//捕获apc编号
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.子匹配加入集合(apc,1,false,0);
                                    }
                                }
                            }
                        }
                        else if (exname == ".act")//当提供的集合扩展名为act(脚本)时
                        {
                            if (obj != null)//判断参数是否允许提取特效编号
                            {
                                regex.创建(alltext, @"\[CREATE PASSIVEOBJECT\][\r\n]+\[INDEX\][\r\n]+([0-9]+)\t", 0);//这里默认匹配大小写
                                if (regex.取匹配数量()>0)//检查是否匹配到
                                {
                                    regex.子匹配加入集合(obj,1,false,0);
                                }

                                regex.创建(alltext, @"\[CREATE PASSIVEOBJECT\][\r\n]+\[INDEX\][\r\n]+\[RANDOM SELECT\][\r\n]+(.+)\r\n\[/RANDOM SELECT\]", 0);//匹配随机的特效；默认大写匹配模式
                                if (regex.取匹配数量() > 0)//检查是否匹配到
                                {
                                    string zpptext = regex.返回所有子匹配文本(1);//返回所有匹配到的随机编号
                                    regex.创建(zpptext,@"[0-9]+",0);//捕获单个编号
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
                                    string zpptext = regex.返回所有子匹配文本(1);//返回所有匹配到的随机编号
                                    regex.创建(zpptext, @"[0-9]+", 0);//捕获单个编号
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
                                    string zpptext = regex.返回所有子匹配文本(1);//返回所有匹配到的随机编号
                                    regex.创建(zpptext, @"[0-9]+", 0);//捕获单个编号
                                    if (regex.取匹配数量() > 0)//检查是否匹配到
                                    {
                                        regex.匹配加入集合(apc, false, 0);
                                    }
                                }
                            }
                        }
                        else if (exname == ".obj")//当提供的集合扩展名为obj(特效)时
                        {
                            if (obj != null)//判断参数是否允许提取特效编号
                            {
                                regex.创建(alltext, @"`\[add object index\]`[\r\n]+[0-9]+\t(.+)\t", 0);//捕获销毁创建的特效
                                if (regex.取匹配数量() > 0)
                                {
                                    regex.创建(regex.返回所有子匹配文本(1),@"[0-9]+",0);//这里省去多个判断直接获取所有编号
                                    if (regex.取匹配数量() > 0)
                                    {
                                        regex.匹配加入集合(obj,false,0);
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
                        }
                        else if (exname == ".mob")//当提供的集合扩展名为mob(怪物)时
                        {
                            if (obj != null)//判断参数是否允许提取特效编号
                            {
                                regex.创建(alltext, @"`\[passive object index\]`\r\n([0-9]+)\t",0);//捕获怪物文件当中的obj编号
                                if (regex.取匹配数量()>0)//检查是否匹配到
                                {
                                    regex.子匹配加入集合(obj,1,false,0);
                                }
                            }
                        }
                        else if (exname == ".ptl")//当提供的集合扩展名为ptl(粒子文件)时
                        {
                            if (obj != null)//判断参数是否允许提取特效编号
                            {
                                //如果ptl文本中对象类型是特效
                                if (alltext.Contains("[object type]\r\n`[passive object]`") == true)
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
        public void 怪物寻找编号(string path,string lsttext,List<string> mob_id,List<string>mob_lst, List<string> mob_path, List<string> obj_id, List<string> apc_id, List<string> act_path, List<string> ptl_path)
        {

            id_lst_怪物(path, lsttext,mob_id,mob_lst,mob_path);

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
        public void 特效寻找编号(string path, string lsttext, List<string> obj_id, List<string> obj_lst,List<string> obj_path, List<string> mob_id, List<string> apc_id, List<string> act_path, List<string> ptl_path)
        {

            id_lst_特效(path, lsttext, obj_id, obj_lst, obj_path);

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
        public void 人偶寻找编号(string path, string lsttext, List<string> apc_id, List<string> apc_lst, List<string> apc_path, List<string> mob_id, List<string> obj_id, List<string> act_path, List<string> ptl_path,  List<string> key_path)
        {

            id_lst_人偶(path, lsttext, apc_id, apc_lst, apc_path);

            寻找全路径(path, apc_path, null, act_path, null, null, null, null, key_path, null, null);

            if (act_path.Count > 0)
            {
                int act_count = 0;
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
        public void twn寻找map_ani(string pvf所在目录, List<string> twnpath, List<string> mappath,List<string> anipath,bool CNmap)
        {
            //定义国服map集合
            List<string> cnmap = new List<string>();

            //定义一个正则类
            regex_new regex = new regex_new();

            //如果提供的城镇文件路径集合存在
            if (twnpath.Count>0)
            {
                //枚举出所有路径
                foreach (var item in twnpath)
                {
                    //读入城镇文件内容
                    string alltext = File.ReadAllText(item.ToString());
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
                            if (File.Exists(lsmap) == true)
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
                            else//反之
                            {
                                //map文件名前面插入“(r)”
                                lsmap = lsmap.Insert(lsmap.LastIndexOf("\\") + 1, "(r)");
                                //如果文件存在
                                if (File.Exists(lsmap) == true)
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
                    foreach (var item in cnmap)
                    {
                        //获取map文件内容
                        string alltext = File.ReadAllText(item.ToString());
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
                            if (File.Exists(lscnmap) == true)
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
                                if (File.Exists(lscnmap) == true)
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


        public void 城镇map寻找id和路径(List<string> mappath, List<string> obj_id, List<string> npc_id, List<string> ani_path, List<string> til_path)
        {
            //判断提供的全路径是否存在
            if (mappath.Count>0)
            {
                //创建正则变量
                regex_new regex = new regex_new();

                //声明变量
                string lsmap;
                string alltetx;
                string f_path;
                string anipath;
                string tilpath;

                //枚举出所有全路径
                foreach (var item in mappath)
                {
                    //取出全路径
                    lsmap = item.ToString();
                    //读入文本
                    alltetx = File.ReadAllText(lsmap);
                    //取出父路径
                    f_path = Directory.GetParent(lsmap).ToString() ;
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

        public void equ寻找apd_obj编号(string pvfpath,List<string> equpath, List<string> apd_id, List<string> obj_id, List<string> obj_ptlpath)
        {
            regex_new regex = new regex_new();

            if (equpath.Count>0)
            {
                foreach (var item in equpath)
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

                    if (obj_id != null && obj_ptlpath != null)
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
                                    zobjtxt = zobjtxt + objitemtxt;
                                }
                            }

                            regex.创建(zobjtxt, @"\r\n([0-9]+)\t([\x20-\x7f]+\t){5}`(.*\.ptl)?`", 0);
                            regex.子匹配加入集合(obj_id, 1, false, 1);

                            regex.创建(zobjtxt, @"`(.+\.ptl)`", 1);
                            foreach (Match objitem in regex.正则返回集合())
                            {
                                objitemtxt = pvfpath + @"\equipment\character\particle\" + objitem.Groups[1].Value;
                                objitemtxt = objitemtxt.ToLower();
                                if (File.Exists(objitemtxt) == true)
                                {
                                    if (obj_ptlpath.Contains(objitemtxt) == false)
                                    {
                                        obj_ptlpath.Add(objitemtxt);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }



}
