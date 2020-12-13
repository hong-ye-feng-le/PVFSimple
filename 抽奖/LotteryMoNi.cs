using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;//正则表达式

namespace PVFSimple.抽奖
{

    public class MoHe
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 物品代码
        /// </summary>
        public int GoodsId { get; set; }
        /// <summary>
        /// 概率
        /// </summary>
        public int GoodsProbability { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int GoodsNumber { get; set; }
        /// <summary>
        /// 公告
        /// </summary>
        public int GoodsNotice { get; set; }
        /// <summary>
        /// 计次
        /// </summary>
        public int GoodsCount { get; set; }

        public MoHe(string AddName,int AddId, int AddProbability, int AddNumber, int AddNotice, int AddCount)
        {
            GoodsName = AddName;
            GoodsId = AddId;
            GoodsProbability = AddProbability;
            GoodsNumber = AddNumber;
            GoodsNotice = AddNotice;
            GoodsCount = AddCount;
        }

    }

    public class XiuZhenGuan
    {
        


    }

    public class LiBao
    {


    }

    public class ChouJiangJi
    {


    }




    public class LotteryMoNi
    {
        /// <summary>
        /// 把代码指向名称，正则一次加入到集合中方便查找
        /// </summary>
        /// <param name="Text">待查询的所有文本</param>
        /// <returns></returns>
        public List<List<string>> LstNameInfo(string Text)
        {
            List<List<string>> Info = null;

            char[] Fg = { '\t', '\r', '\n' };//设定依靠拆分的字符
            string[] FgEnd = Text.Split(Fg, StringSplitOptions.RemoveEmptyEntries);//开始拆分字符串
            if (FgEnd.Length <= 1)
                return Info;

            Info = new List<List<string>> { new List<string>(), new List<string>() };
            int Pos = 0;
            foreach (string item in FgEnd)//得到所有被拆分的字符串，按照单数为编号  双数为路径加入到集合中
            {
                switch (Pos)
                {
                    case 0:
                        switch (item.Remove(1))
                        {
                            case "0":
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                                break;
                            default:
                                continue;
                        }
                        Info[0].Add(item);
                        Pos = 1;
                        break;
                    case 1:
                        Info[1].Add(item);
                        Pos = 0;
                        break;
                }
            }

            return Info;
        }


        /// <summary>
        /// 判断一个抽奖列表中分组信息是否正确、返回0 为正确反之不正确
        /// </summary>
        /// <param name="ListText">列表信息文本</param>
        /// <param name="MoShi">0：魔盒/1：袖珍罐/2：礼包/3：抽奖机</param>
        /// <param name="FenZu">魔盒：4/袖珍罐：3/礼包：3|5/抽奖机：4</param>
        /// <returns></returns>
        public int YesNoGeShi(string ListText,int MoShi,int FenZu)
        {
            int Info = 0;
            Regex_new regex = new Regex_new();

            switch (MoShi)
            {
                case 0:
                    regex.创建(ListText, "\t", 0);
                    Info = regex.取匹配数量() % FenZu;
                    break;
                case 1:
                    regex.创建(ListText.Substring(ListText.IndexOf("\t", ListText.IndexOf("\t") + 1) + 1), "\t", 0);
                    Info = regex.取匹配数量() % FenZu;
                    break;
                case 2:
                    regex.创建(ListText.Substring(ListText.IndexOf("\t") + 1), "\t", 0);
                    Info = regex.取匹配数量() % FenZu;
                    break;
                case 3:
                    regex.创建(ListText, "\t", 0);
                    Info = regex.取匹配数量() % FenZu;
                    break;
            }

            return Info;
        }

        /// <summary>
        /// 验证抽奖的概率总数是否在规定数值内
        /// </summary>
        /// <param name="Count">待验证的总概率</param>
        /// <param name="MoShi">0：魔盒/1：袖珍罐/2：礼包/3：抽奖机</param>
        /// <returns>true=正确 false=不正确</returns>
        public bool YesNoGaiLv(int Count, int MoShi)
        {
            bool YesNo = false;

            switch (MoShi)
            {
                case 0:
                    if(Count<= 1000000)
                        YesNo = true;

                    break;
                case 1:

                    if (Count <= 100000)
                        YesNo = true;
                    break;
                case 2:

                    if (Count <= 1000000)
                        YesNo = true;
                    break;
                case 3:

                    if (Count <= 1000000)
                        YesNo = true;
                    break;
            }

            return YesNo;
        }

        /// <summary>
        /// 获得抽奖列表内的总概率
        /// </summary>
        /// <param name="ListText">待获取概率的文本</param>
        /// <param name="MoShi">0：魔盒/1：袖珍罐/2：礼包/3：抽奖机</param>
        /// <param name="FenZu">魔盒：4/袖珍罐：3/礼包：3|5/抽奖机：4</param>
        /// <returns>总概率</returns>
        public int ZongGaiLv(string ListText, int MoShi, int FenZu)
        {
            int Count = 0;
            Regex_new regex = new Regex_new();

            switch (MoShi)
            {
                case 0:
                    regex.创建(ListText.Substring(
                        ListText.IndexOf("\t",
                        ListText.IndexOf("\t",
                        ListText.IndexOf("\t",
                        ListText.IndexOf("\t") + 1) + 1) + 1) + 1), "[\x20-\x7f]+\t([\x20-\x7f]+)\t[\x20-\x7f]+\t[\x20-\x7f]+\t", 0);
                    foreach (Match item in regex.正则返回集合())
                    {
                        Count += Convert.ToInt32(item.Groups[1].Value);
                    }

                    break;
                case 1:
                    regex.创建(ListText.Substring(
                        ListText.IndexOf("\t",
                        ListText.IndexOf("\t") + 1) + 1), "[\x20-\x7f]+\t([\x20-\x7f]+)\t[\x20-\x7f]+\t", 0);
                    foreach (Match item in regex.正则返回集合())
                    {
                        Count += Convert.ToInt32(item.Groups[1].Value);
                    }

                    break;
                case 2:
                    switch (FenZu)
                    {
                        case 3:
                            regex.创建(ListText.Substring(ListText.IndexOf("\t") + 1), "[\x20-\x7f]+\t([\x20-\x7f]+)\t[\x20-\x7f]+\t", 0);
                            break;
                        case 5:
                            regex.创建(ListText.Substring(ListText.IndexOf("\t") + 1), "[\x20-\x7f]+\t([\x20-\x7f]+)\t[\x20-\x7f]+\t[\x20-\x7f]+\t[\x20-\x7f]+\t", 0);
                            break;
                    }
                    foreach (Match item in regex.正则返回集合())
                    {
                        Count += Convert.ToInt32(item.Groups[1].Value);
                    }

                    break;
                case 3:
                    regex.创建(ListText, "[\x20-\x7f]+\t([\x20-\x7f]+)\t[\x20-\x7f]+\t[\x20-\x7f]+\t", 0);
                    foreach (Match item in regex.正则返回集合())
                    {
                        Count += Convert.ToInt32(item.Groups[1].Value);
                    }

                    break;
            }

            return Count;
        }

        /// <summary>
        /// 获得抽奖信息原本的抽取次数
        /// </summary>
        /// <param name="RandomListText">待获取信息的信息</param>
        /// <returns></returns>
        public int ChouQuCiShu(string RandomListText)
        {
            return Convert.ToInt32(RandomListText.Remove(RandomListText.IndexOf("\t")));
        }

        /// <summary>
        /// 把抽奖信息汇总到字典中
        /// </summary>
        /// <param name="ListText">待汇总的抽奖信息</param>
        /// <param name="RandomInfo">加入的字典</param>
        public void MoHeAddInfo(string ListText, Dictionary<int, MoHe> RandomInfo, List<List<string>> LstNameInfo)
        {
            Regex_new regex = new Regex_new();

            string AwName="";
            int AwId;
            int AwSl;
            int AwGg;

            
            ListText =ListText.Substring(ListText.IndexOf("\t") + 1);
            AwId = Convert.ToInt32(ListText.Remove(ListText.IndexOf("\t")));
            ListText = ListText.Substring(ListText.IndexOf("\t") + 1);
            AwSl = Convert.ToInt32(ListText.Remove(ListText.IndexOf("\t")));
            ListText = ListText.Substring(ListText.IndexOf("\t") + 1);
            AwGg = Convert.ToInt32(ListText.Remove(ListText.IndexOf("\t")));
            ListText = ListText.Substring(ListText.IndexOf("\t") + 1);

            int Pos = LstNameInfo[0].IndexOf(AwId.ToString());
            if (Pos != -1)
                AwName = LstNameInfo[1][Pos];

            int AddCount = 0;
            RandomInfo.Add(AddCount, new MoHe(
                    AwName,
                    AwId,
                    0,
                    AwSl,
                    AwGg,
                    0
                    ));
            AddCount++;


            regex.创建(ListText, "([\x20-\x7f]+)\t([\x20-\x7f]+)\t([\x20-\x7f]+)\t([\x20-\x7f]+)\t", 0);
            foreach (Match item in regex.正则返回集合())
            {
                string Id = item.Groups[1].Value;
                string Name = "";

                int Poss = LstNameInfo[0].IndexOf(Id);
                if (Poss != -1)
                    Name = LstNameInfo[1][Poss];

                RandomInfo.Add(AddCount,new MoHe(
                    Name,
                    Convert.ToInt32(Id),
                    Convert.ToInt32(item.Groups[2].Value),
                    Convert.ToInt32(item.Groups[3].Value),
                    Convert.ToInt32(item.Groups[4].Value),
                    0
                    ));
                AddCount++;
            }

        }




        public Dictionary<int, MoHe> StartMoHe(string LstNameText,string RandomListText,int ChouQuTimeS)
        {


            //if(YesNoGeShi(RandomListText,4) != 0)
            //{
            //    MessageBox.Show("抱歉您所需要模拟抽奖的数据错误；错误内容为：\r\n" +
            //        "抽奖的信息分组不正确，请仔细检查对应的分组信息。\r\n" +
            //        "正确信息应该为：\r\n" +
            //        "第一个数为抽取次数\r\n" +
            //        "第二、三、四个数为物品代码、给予数量、是否公告\r\n" +
            //        "往后都是四个数为一组", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return RandomInfo;
            //}

            //if(YesNoGaiLv(RandomListText,0,1000000)==false)
            //{
            //    MessageBox.Show("抱歉您所需要模拟抽奖的总概率错误；错误内容为：\r\n" +
            //           "抽取的总概率不能超过100W，请更正后再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return RandomInfo;
            //}

            Regex_new regex = new Regex_new();
            Dictionary<int, MoHe> RandomInfo = new Dictionary<int, MoHe>();
            Random random = new Random();

            int ZhongGaiLv = ZongGaiLv(RandomListText,0,4);
            int YuanChiShu = ChouQuCiShu(RandomListText);
            MoHeAddInfo(RandomListText, RandomInfo, LstNameInfo(LstNameText));

            for (int q = 0; q < ChouQuTimeS; q++)
            {
                for (int w = 0; w < YuanChiShu; w++)
                {
                    int SuiJiShu = random.Next(1000000);
                    if (SuiJiShu > ZhongGaiLv)
                    {
                        RandomInfo[0].GoodsCount++;
                    }
                    else
                    {
                        bool YesNoChouZhong = false;

                        for (int i = 0; i < RandomInfo.Count; i++)
                        {
                            int LinShiZongShu = 0;
                            for (int a = i; a < RandomInfo.Count; a++)
                            {
                                LinShiZongShu += RandomInfo[a].GoodsProbability;
                            }

                            int LinShiSuiJiShu = random.Next(LinShiZongShu);

                            //if (LinShiSuiJiShu + RandomInfo[i].GoodsProbability > LinShiZongShu)
                            //{
                            //    RandomInfo[i].GoodsCount++;
                            //    YesNoChouZhong = true;
                            //    break;
                            //}

                            if (LinShiSuiJiShu <= RandomInfo[i].GoodsProbability)
                            {
                                RandomInfo[i].GoodsCount++;
                                YesNoChouZhong = true;
                                break;
                            }
                        }


                        if (YesNoChouZhong == false)
                            RandomInfo[0].GoodsCount++;
                    }
                }
            }

            return RandomInfo;
        }

    }


}
