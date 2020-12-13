using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace PVFSimple
{


    public class DingWei
    {
        
        [DllImport("shell32.dll", ExactSpelling = true)]
        private static extern void ILFree(IntPtr pidlList);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern IntPtr ILCreateFromPathW(string pszPath);

        [DllImport("shell32.dll", ExactSpelling = true)]
        private static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cild, IntPtr children, uint dwFlags);

        /// <summary>
        /// 打开路径并定位文件...对于@"h:\Bleacher Report - Hardaway with the safe call ??.mp4"这样的，explorer.exe /select,d:xxx不认，用API整它
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        public void ExplorerFile(string filePath)
        {
            if (!File.Exists(filePath) && !Directory.Exists(filePath))
                return;

            if (Directory.Exists(filePath))
                System.Diagnostics.Process.Start(@"explorer.exe", "/select,\"" + filePath + "\"");
            else
            {
                IntPtr pidlList = ILCreateFromPathW(filePath);
                if (pidlList != IntPtr.Zero)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0));
                    }
                    finally
                    {
                        ILFree(pidlList);
                    }
                }
            }
        }


        /// <summary>
        /// 打开文件或打开文件夹
        /// </summary>
        /// <param name="Path">文件绝对路径</param>
        public void Explorer(string Path)
        {
            if (!File.Exists(Path) && !Directory.Exists(Path))
                return;

            if (Directory.Exists(Path))
                System.Diagnostics.Process.Start(@"explorer.exe",  Path );
            else
            {
                IntPtr pidlList = ILCreateFromPathW(Path);
                if (pidlList != IntPtr.Zero)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0));
                    }
                    finally
                    {
                        ILFree(pidlList);
                    }
                }
            }
        }
    }


    public class 文本文件读写
    {


        //public string 读入文件(string path)
        //{

        //    FileStream 读入二进制 = new FileStream(path, FileMode.Open, FileAccess.Read);//读入

        //    StreamReader 读入字符 = new StreamReader(读入二进制);

        //    string 返回的文本 = 读入字符.ReadToEnd();

        //    读入二进制.Close();//释放
        //    读入字符.Close();//释放

        //    return 返回的文本;



        //}

        //public void 写到文件(string path,string text)
        //{

        //    FileStream 写出二进制 = new FileStream(path, FileMode.Open, FileAccess.Write);//写出

        //    StreamWriter 写出字符 = new StreamWriter(写出二进制);

        //    写出字符.WriteLine(text);

        //    写出二进制.Close();//释放
        //    写出字符.Close();//释放

        //}




    }


    public class Regex_new
    {

        public MatchCollection 输出;

        
        /// <summary>
        /// 参数1：被搜索的文本
        /// 参数2：正则表达式文本
        /// 参数3：0=无；1=不区分大小写、默认大小写； 2=多行模式；
        /// </summary>
        public void 创建(string txt,string regex,int ms)
        {
            switch (ms)
            {
                case 0:
                    输出 = Regex.Matches(txt, regex, RegexOptions.None);
                    break;
                case 1:
                    输出 = Regex.Matches(txt, regex, RegexOptions.IgnoreCase);
                    break;
                case 2:
                    输出 = Regex.Matches(txt, regex, RegexOptions.Multiline);
                    break;
            }
        }

        /// <summary>
        /// 方便匹配未知文本
        /// </summary>
        /// <param name="text">提供需要转换的文本</param>
        /// <returns></returns>
        public string 取消特殊字符(string text)
        {
            return Regex.Escape(text);
        }

        /// <summary>
        /// 查看匹配的数量
        /// </summary>
        /// <returns></returns>
        public int 取匹配数量()
        {
            return 输出.Count;
        }

        /// <summary>
        /// 替换所有指定内容
        /// </summary>
        /// <param name="alltxt">搜索的文本</param>
        /// <param name="regextxt">正则文本</param>
        /// <param name="thtxt">替换内容</param>
        /// <returns>返回替换后的文本</returns>
        public string 替换(string alltxt,string regextxt,string thtxt)
        {
           return Regex.Replace(alltxt, regextxt, thtxt);
        }

        /// <summary>
        /// 返回某个匹配文本;从1开始
        /// </summary>
        /// <param name="匹配索引">匹配的第几个内容</param>
        /// <returns></returns>
        public string 取匹配文本(int 匹配索引)
        {
            return 输出[匹配索引-1].Value;
        }


        /// <summary>
        /// 返回某个匹配的子文本；从1开始
        /// </summary>
        /// <param name="匹配索引">匹配的第几个内容</param>
        /// <param name="子表达式索引">匹配内容的第几个子文本</param>
        /// <returns></returns>
        public string 取子匹配文本(int 匹配索引,int 子表达式索引)
        {
            return 输出[匹配索引-1].Groups[子表达式索引].Value;
        }


        /// <summary>
        /// 匹配的所有文本加入集合;
        /// 参数1 集合；
        /// 参数2 真：加入重复 假：不加入重复；
        /// 参数3 0=不转换 1=大写 2=小写；
        /// </summary>
        public void 匹配加入集合(List<string> 集合,bool 是否加入重复,int 转换大小写)
        {
            switch (是否加入重复)
            {
                case true:
                    foreach (Match match in 输出)
                    {
                        switch (转换大小写)
                        {
                            case 0:
                                集合.Add(match.Value);
                                break;
                            case 1:
                                集合.Add(match.Value.ToUpper());
                                break;
                            case 2:
                                集合.Add(match.Value.ToLower());
                                break;
                        }
                    }
                    break;
                case false:
                    foreach (Match match in 输出)
                    {
                        string matchtext = match.Value;
                        switch (转换大小写)
                        {
                            case 0:
                                if (集合.Contains(matchtext) == false)
                                {
                                    集合.Add(matchtext);
                                }
                                break;
                            case 1:
                                matchtext = matchtext.ToUpper();
                                if (集合.Contains(matchtext) == false)
                                {
                                    集合.Add(matchtext);
                                }
                                break;
                            case 2:
                                matchtext = matchtext.ToLower();
                                if (集合.Contains(matchtext) == false)
                                {
                                    集合.Add(matchtext);
                                }
                                break;
                        }
                    }
                    break;
            }
        }


/// <summary>
        /// 匹配的所有文本加入集合;
        /// 参数1 集合；
        /// 参数2 子匹配索引
        /// 参数3 真：加入重复 假：不加入重复；
        /// 参数4 0=不转换 1=大写 2=小写；
        /// </summary>
        public void 子匹配加入集合(List<string> 集合,int 子匹配索引, bool 是否加入重复, int 转换大小写)
        {
            switch (是否加入重复)
            {
                case true:
                    foreach (Match match in 输出)
                    {
                        switch (转换大小写)
                        {
                            case 0:
                                集合.Add(match.Groups[子匹配索引].Value);
                                break;
                            case 1:
                                集合.Add(match.Groups[子匹配索引].Value.ToUpper());
                                break;
                            case 2:
                                集合.Add(match.Groups[子匹配索引].Value.ToLower());
                                break;
                        }

                    }
                    break;
                case false:
                    foreach (Match match in 输出)
                    {
                        string zpp_txt = match.Groups[子匹配索引].Value;
                        switch (转换大小写)
                        {
                            case 0:
                                if (集合.Contains(zpp_txt) == false)
                                {
                                    集合.Add(zpp_txt);
                                }
                                break;
                            case 1:
                                zpp_txt = zpp_txt.ToUpper();
                                if (集合.Contains(zpp_txt) == false)
                                {
                                    集合.Add(zpp_txt);
                                }
                                break;
                            case 2:
                                zpp_txt = zpp_txt.ToLower();
                                if (集合.Contains(zpp_txt) == false)
                                {
                                    集合.Add(zpp_txt);
                                }
                                break;
                        }
                    }
                    break;
            }
        }


        /// <summary>
        /// 返回字符串型、捕获的所有匹配文本
        /// </summary>
        /// <returns></returns>
        public string 返回所有匹配文本()
        {
            string pptxt = "";
            foreach (Match item in 输出)
            {
                pptxt = pptxt + item.Value + "\r\n";
            }
            return pptxt;
        }

        /// <summary>
        /// 返回字符串型、捕获的所有的子匹配文本
        /// </summary>
        /// <param name="索引">子匹配 索引</param>
        /// <returns></returns>
        public string 返回所有子匹配文本(int 索引)
        {
            string zpptxt = "";
            foreach (Match item in 输出)
            {
                zpptxt = zpptxt + item.Groups[索引].Value + "\r\n";
            }
            return zpptxt;
        }


        public MatchCollection 正则返回集合()
        {
            return 输出;
        }


    }



}








