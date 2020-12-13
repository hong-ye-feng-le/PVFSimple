using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;
using System.Drawing;


/* 设置列头文本
 * reoGridControl.CurrentWorksheet.ColumnHeaders[0].Text = "dsdsds";
 * 设置行头文本
 * reoGridControl.CurrentWorksheet.RowHeaders[0].Text = "dsdsds";
 * 当前单元格设置值
 * reoGridControl1.CurrentWorksheet[1, 1] = "dasdasd";
 * 获取指定单元格的值 参数1 为行从0开始  参数2为列 从0开始
 * reoGridControl.Worksheets[0].GetCellText(i,16);
 * 获得当前表格的行总数
 * reoGridControl1.CurrentWorksheet.RowCount
 * 设置当前表格行数
 * reoGridControl1.CurrentWorksheet.RowCount = 500;
 * 增加当前表格行数
 * reoGridControl1.CurrentWorksheet.AppendRows(50);
 * 
 * 设置当前表格列数
 * reoGridControl1.CurrentWorksheet.ColumnCount = 18;
 * 增加当前表格列数
 * reoGridControl1.CurrentWorksheet.AppendCols(10);
 * 获取当前选择的行数
 * reoGridControl1.CurrentWorksheet.SelectionRange.Rows
 * 获取当前选择的列数
 * reoGridControl1.CurrentWorksheet.SelectionRange.Cols
 * 获取当前选择的初始行0开始的位置
 * reoGridControl1.CurrentWorksheet.SelectionRange.Row
 * 获取当前选择的初始列0开始的位置
 * reoGridControl1.CurrentWorksheet.SelectionRange.Col
 * 获取当前选择的初始列行pos坐标例如A7
 * reoGridControl1.CurrentWorksheet.SelectionRange.StartPos
 * 设置或获得当前表格选择夹的名称
 * reoGridControl1.CurrentWorksheet .Name
 * 
 * 设置第列头的列宽
 * reoGridControl1.Worksheets[0].RowHeaderWidth = 200;
 * 设置指定范围的列宽；0从A开始 从0开始17个 设置宽度100像素
 * reoGridControl1.Worksheets[0].SetColumnsWidth(0,17,100);
 * 设置指定范围的行高；0从A开始 从0开始17个 设置高度100像素
 * reoGridControl1.Worksheets[0].SetRowsHeight(0,17,100);
 * 
 * 
 * 
 * 获取当前选择夹表格数量
 * reoGridControl1.Worksheets.Count
 * 增加一个新的表格选择夹
 * reoGridControl1.Worksheets.Add(reoGridControl1.Worksheets.Create("name"));
 * 在第0的位置插入一个表格选择夹
 * reoGridControl1.Worksheets.Insert(0, reoGridControl1.Worksheets.Create("name"));
 * 删除指定的表格选择夹0开始的位置
 * reoGridControl1.Worksheets.RemoveAt(0);
 * 
 * 
 * 设置单元格背景颜色
 * var range = reoGridControl1.Worksheets[0].Ranges["A1:C4"];
 * range.Style.BackColor = Color.FromArgb(0,0,0);
 * 
 * 
 * 设置文字颜色
 * reoGridControl1.Worksheets[0].SetRangeStyles("A1:B4", new WorksheetRangeStyle()
 * {
 *     Flag = PlainStyleFlag.TextColor,
 *     TextColor = Color.FromArgb(124,57,35),
 * });
 * 
 * 设置文字对齐方式 Center居中 Left居左 Right居右
 * reoGridControl1.Worksheets[0].SetRangeStyles("A1:B4", new WorksheetRangeStyle()
 * {
 *     Flag = PlainStyleFlag.HorizontalAlign,
 *     HAlign = ReoGridHorAlign.Center,
 * });
 * 
 * 
 * 设置字体大小
 * reoGridControl1.Worksheets[0].SetRangeStyles("A1:B4", new WorksheetRangeStyle()
 * {
 *     Flag = PlainStyleFlag.FontSize,
 *     FontSize=30,
 * });
 * 
 * 设置单元格焦点
 * reoGridControl1.Worksheets[0].FocusPos=new CellPosition($"Q{CuoWuPos+1}");
 * 
 * 设置当前所在选择夹表格
 * reoGridControl1.CurrentWorksheet = reoGridControl1.Worksheets[0];
 * 
 * 
 * **/


/* 选中配色
 * 70,130,180
 * 
 * 
 * **/


namespace PVFSimple.表格控件
{

    public class TongYongFangFa
    {
        public object[] GetObjShuZu(object[] ShuZu,int Pos,int Count)
        {
            object[] FanHui = new object[Count];

            for (int i = 0; i < Count; i++)
            {
                FanHui[i] = ShuZu[Pos+i];
            }

            return FanHui;
        }

    }

    public class Grid_SheJiTu
    {


        //第一页必填文字参数
        static readonly string[] LieName = { "设计图名称", "蓝字描述", "灰字描述", "掉落等级", "物品品级", "使用职业", "交易类型", "使用等级", "冷却时间", "购买价格", "卖出价格", "图标路径", "图标帧数", "掉落图标路径", "掉落图标帧数", "移动声音", "设计图样式", "写出文件名" };
        //第一页必填行距参数
        static readonly ushort[] LieKuan = { 90, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 110, 110, 80, 110 , 110 };

        static readonly string[] LieName2 = { "材料代码", "材料数量"};//第二页需求材料表头名称
        static readonly string[] LieName3 = { "合成代码", "合成数量"};//第三页合成物品表头名称

        static readonly string[] LieName4 = {"图标边框路径","图标边框帧数","商店购买代码","商店购买数量","物品堆叠上限","设计图成功率","设计图公告","设计图保留能力","设计图失败代码","设计图失败数量",  "etc随机强化", "副职业类型" ,"副职业等级","副职业额外需求材料代码", "副职业额外需求材料数量"};//第四页额外选择
        static readonly ushort[] LieKuan4 = { 110, 110, 110, 110, 110, 110, 100, 120, 120 ,  110, 110, 100, 100, 190, 190 };


        /// <summary>
        /// 提供需要设置的控件以及行数
        /// </summary>
        /// <param name="reoGridControl">需要被更改的第三方表格控件</param>
        /// <param name="Count">设置的行数</param>
        public void 设置行数(ReoGridControl reoGridControl,int Count)
        {
            for (int i = 0; i < reoGridControl.Worksheets.Count; i++)
            {
                reoGridControl.Worksheets[i].RowCount = Count;

                //获取范围信息
                RangePosition range1 = new RangePosition(new CellPosition(0, 0), new CellPosition(
                reoGridControl.Worksheets[i].RowCount,
                reoGridControl.Worksheets[i].ColumnCount
                ));

                //设置获取的范围内所有的单元格对齐方式为左对齐
                reoGridControl.Worksheets[i].SetRangeStyles(range1, new WorksheetRangeStyle()
                {
                    Flag = PlainStyleFlag.HorizontalAlign,
                    HAlign = ReoGridHorAlign.Left,
                });
            }
        }

        /// <summary>
        /// 用于设置设计图初始化表格参数
        /// </summary>
        /// <param name="reoGridControl">需要被初始化的第三方表格控件</param>
        public void 初始化设计图表格框架(ReoGridControl reoGridControl)
        {

            while (reoGridControl.Worksheets.Count!=0)
            {
                reoGridControl.Worksheets.RemoveAt(0);
            }

            reoGridControl.Worksheets.Add(reoGridControl.Worksheets.Create("设计图基本框架"));
            reoGridControl.Worksheets.Add(reoGridControl.Worksheets.Create("必填合成材料"));
            reoGridControl.Worksheets.Add(reoGridControl.Worksheets.Create("必填得到物品"));
            reoGridControl.Worksheets.Add(reoGridControl.Worksheets.Create("可额外选择项"));

            reoGridControl.Worksheets[0].ColumnCount = LieName.Length;//设置第一页列数

            //置表头跟列宽
            for (int i = 0; i < LieName.Length; i++)
            {
                reoGridControl.Worksheets[0].ColumnHeaders[i].Text = LieName[i];
                reoGridControl.Worksheets[0].ColumnHeaders[i].Width = LieKuan[i];
            }

            //设置第二、三页列数
            reoGridControl.Worksheets[1].ColumnCount = 100;
            reoGridControl.Worksheets[2].ColumnCount = 100;
            int AddTPos1 = 0;
            int AddTPos2 = 1;
            //置表头跟行距
            for (int i = 0; i < 50; i++)
            {
                reoGridControl.Worksheets[1].ColumnHeaders[AddTPos1].Text = LieName2[0];
                reoGridControl.Worksheets[1].ColumnHeaders[AddTPos2].Text = LieName2[1];
                reoGridControl.Worksheets[2].ColumnHeaders[AddTPos1].Text = LieName3[0];
                reoGridControl.Worksheets[2].ColumnHeaders[AddTPos2].Text = LieName3[1];

                AddTPos1 += 2;
                AddTPos2 += 2;
            }

            reoGridControl.Worksheets[1].SetColumnsWidth(0, 100, 80);//设置第二页表头列宽
            reoGridControl.Worksheets[2].SetColumnsWidth(0, 100, 80);//设置第三页表头列宽

            //设置第四页信息
            reoGridControl.Worksheets[3].ColumnCount = LieName4.Length;//设置第四页列数

            //置表头跟行距
            for (int i = 0; i < LieName4.Length; i++)
            {
                reoGridControl.Worksheets[3].ColumnHeaders[i].Text = LieName4[i];
                reoGridControl.Worksheets[3].ColumnHeaders[i].Width = LieKuan4[i];
            }

            //设置所有表文本不能溢出单元格
            for (int i = 0; i < reoGridControl.Worksheets.Count; i++)
            {
                reoGridControl.Worksheets[i].DisableSettings(WorksheetSettings.View_AllowCellTextOverflow);
            }

            设置行数(reoGridControl, 25);
        }

        /// <summary>
        /// 删除表格所有信息并且恢复到开始原有状态
        /// </summary>
        /// <param name="reoGridControl">需要被更改的第三方控件</param>
        public void 重置设计图表格(ReoGridControl reoGridControl)
        {
            初始化设计图表格框架(reoGridControl);
        }

        /// <summary>
        /// 用于验证设计图中的文件名设置是否正确
        /// </summary>
        /// <param name="reoGridControl">被检查的表格控件</param>
        /// <returns></returns>
        public int 验证FileName(ReoGridControl reoGridControl)
        {
            List<string> YZ = new List<string>();

            for (int i = 0; i < reoGridControl.Worksheets[0].RowCount; i++)
            {

                string Text = reoGridControl.Worksheets[0].GetCellText(i,17);
                if (Text != "")
                {
                    if (YZ.Contains(Text) ==false)
                    {
                        YZ.Add(Text);
                    }
                    else
                    {
                        return i++;
                    }
                }
                else
                {
                    return i++;
                }
                
            }
            return 0;
        }


        /// <summary>
        /// 用于验证设计图中的需要物品或得到物品设置是否正确
        /// </summary>
        /// <param name="reoGridControl">被检查的表格控件</param>
        /// <returns></returns>
        public string 验证CaiLiaoCount(ReoGridControl reoGridControl)
        {
            for (int a = 1; a <= 2; a++)//循环第一页第二页
            {
                for (int b = 0; b < reoGridControl.Worksheets[a].RowCount; b++)//循环所有行
                {
                    int Count = 0;//用于记录每一行对比的总数
                    for (int c = 0; c < reoGridControl.Worksheets[a].ColumnCount; c++)//循环所有列
                    {
                        if (reoGridControl.Worksheets[a].GetCellText(b, c) == "")//判断列行指定单元格是否为空
                            break;
                        Count++;
                    }

                    if(Count<=0)
                    {
                        return "0" + a.ToString() + b.ToString();
                    }
                    else if (Count % 2 > 0)
                    {
                        return "1" + a.ToString() + b.ToString();
                    }
                }   
            }

            return "0";
        }


        /// <summary>
        /// 用于验证设计图中的第一页是否有空单元格
        /// </summary>
        /// <param name="reoGridControl">被检查的表格控件</param>
        /// <returns></returns>
        public string 验证AllText(ReoGridControl reoGridControl)
        {

            for (int i = 0; i < reoGridControl.Worksheets[0].RowCount; i++)//循环第一页的所有单元格
            {
                for (int a = 0; a < reoGridControl.Worksheets[0].ColumnCount; a++)//循环所有列
                {
                    if (reoGridControl.Worksheets[0].GetCellText(i, a) == "")
                        return i.ToString() + a.ToString();//如果单元格空，则返回列和行序号
                }

            }
            return "0";
        }



        //第一页必填文字参数
        static readonly string[] ChaKanInfoName = { 
            "设计图名称",
            "蓝字描述",
            "灰字描述",
            "掉落等级",
            "物品品级",
            "使用职业",
            "交易类型",
            "使用等级",
            "冷却时间",
            "购买价格",
            "卖出价格",
            "图标路径",
            "图标帧数",
            "掉落图标路径",
            "掉落图标帧数",
            "移动声音",
            "设计图样式",
            "写出文件名" ,
            "材料代码",
            "材料数量" ,
            "合成代码",
            "合成数量" ,
            "图标边框路径",
            "图标边框帧数",
            "商店购买代码",
            "商店购买数量",
            "物品堆叠上限",
            "设计图成功率",
            "设计图公告",
            "设计图保留能力",
            "设计图失败代码",
            "设计图失败数量",
            "etc随机强化",
            "副职业类型",
            "副职业等级",
            "副职业额外需求材料代码",
            "副职业额外需求材料数量" };

        static readonly string[] ChaKanInfoMiaoShu = { 
            "用于设置生成设计图时的物品名称",
            "用于设置物品在游戏中显示的蓝色字描述信息；例如：“有一定機率製作成功。 成功時維持強化/增幅/提鍊/魔法屬性，裝備變成封裝狀態。 失敗時除了武器之外的材料道具會消失。”", 
            "用于设置物品在游戏中最底下显示的灰色字描述信息",
            "如果设计图可掉落哪么请设置这里的数值；例如“75”则75等级左右的怪物掉落",
            "用于在游戏显示的色彩以及珍惜程度；0：白/1：蓝/2：紫/3：粉/4：SS/5：异界/",
            "物品可使用的职业；例如：“all”则所有职业可用",
            "可交易的类型；例如：“trade”不可交易“free”可交易",
            "使用此物品则需要达到对应等级；例如：“1”则人物一级才可使用",
            "使用物品后下一次使用的间隔；例如“1000”则为1秒", 
            "物品放入NPC商店中的购买价格；例如“10000”则需要1W金币才能获得",
            "卖给NPC商店的价格；例如“50000”则卖出1W金币",
            "对应指向图标NPKimg的路径；例如：“Item/stackable/recipe.img”", 
            "图标img的对应帧数；例如：“28”",
            "对应指向掉落图标NPKimg的路径；例如：“Item/FieldImage.img”",
            "掉落图标img的对应帧数；例如：“28”", 
            "在背包中移动物品时触发的声音；例如：“PAPER_TOUCH”",
            "打开设计图的样式；“machinary”最简洁古老样式；“enchant”或“chemistry”水晶样式；“weaving”一般用于装备型；“craftmanship”一般用于材料型",
            "用于写出本地或在线时的物品文件名；例如“500000001”" , 
            "合成时需要的物品代码", 
            "合成时需要的物品数量",
            "成功合成后给予的物品代码",
            "成功合成后给予的物品数量",
            "对应指向图标边框的NPKimg的路径；例如“Item/IconMark.img”",
            "图标边框img的对应帧数；例如：“11”",
            "物品放入NPC商店时购买需要的物品代码",
            "物品放入NPC商店时购买需要的物品数量",
            "总共可获得的上限数量；例如“1000”则最多只能获得1000个",
            "设计图合成时的成功概率；例如“100”则100%成功", 
            "设计图成功合成后是否有系统公告信息；例如“1”则公告/“0”则不公告", 
            "设计图成功合成时，会保留材料内从左到右最后一个装备的强化增幅等信息", 
            "设计图合成失败时给予的安慰物品代码", "设计图合成失败时给予的安慰物品数量", 
            "PVF文件位置：“etc/serverparameter.etc”对应类似标签：“[recipe upgrade table]”写入设计图内的标签：“use upgrade table”或“use 2nd upgrade table”", 
            "限制副职业才能使用设计图；例如：“191”为附魔师“192”为控偶师“193”为炼金术士", 
            "限制副职业达到对应等级才可使用；例如：“10”则需要达到十级才行", 
            "副职业在对应副职业窗口合成设计图时需求的额外材料代码", 
            "副职业在对应副职业窗口合成设计图时需求的额外材料数量" };


        public void 设计图查看信息初始化(ReoGridControl reoGridControl)
        {
            //设置行数
            reoGridControl.CurrentWorksheet.RowCount = ChaKanInfoName.Length;
            //设置列数
            reoGridControl.CurrentWorksheet.ColumnCount = 1;
            //设置行高
            reoGridControl.CurrentWorksheet.SetRowsHeight(0, ChaKanInfoName.Length, 25);
            //设置列宽
            reoGridControl.CurrentWorksheet.RowHeaderWidth = 200;
            reoGridControl.CurrentWorksheet.SetColumnsWidth(0, 1, 1200);

            for (int i = 0; i < ChaKanInfoName.Length; i++)
            {
                reoGridControl.CurrentWorksheet.RowHeaders[i].Text = ChaKanInfoName[i];
                reoGridControl.CurrentWorksheet[i, 0] = ChaKanInfoMiaoShu[i];
            }
        }


        public void AddSheJiTuInfo(ReoGridControl reoGridControl, Dictionary<string, string> SheJiTu)
        {
            for (int a = 0; a < reoGridControl.Worksheets[0].RowCount; a++)
            {
                string Name = "";
                string Explain = "";
                string FlavorText = "";
                string Grade = "";
                string Rarity = "";
                string UsableJob = "";
                string AttachType = "";
                string MinimumLevel = "";
                string Cooltime = "";
                string Price = "";
                string Value = "";
                string Icon = "";
                string IconId = "";
                string FieldImage = "";
                string FieldImageId = "";
                string MoveWav = "";
                string FileName = "";
                StringBuilder IntDataMaterial = new StringBuilder();
                StringBuilder IntDataEnd = new StringBuilder();
                string IconMark = "";
                string IconMarkId = "";
                string NeedMaterialId = "";
                string NeedMaterialSl = "";
                string StackLimit = "";
                string SuccessRate = "";
                string BroadcastType = "";
                string MaintainAbility = "";
                string FailOutputId = "";
                string FailOutputSl = "";
                string StringData = "";
                string UpgradeTable = "";
                string FuZhiYeType = "";
                string FuZhiYeLevel = "";
                string BeadItemId = "";
                string BeadItemSl = "";
                string StkType = "[stackable type]\r\n`[recipe]`\r\n1\t\r\n";
                string FileTou = "#PVF_File\r\n\r\n";

                int Count = 0;
                for (int b = 0; b < reoGridControl.Worksheets.Count; b++)
                {
                    for (int c = 0; c < reoGridControl.Worksheets[b].ColumnCount; c++)
                    {
                        string LinShiText = reoGridControl.Worksheets[b].GetCellText(a, c);
                        if (LinShiText == "")
                            continue;

                        switch (b)
                        {
                            case 0:
                                switch (c)
                                {
                                    case 0:
                                        Name = $"[name]\r\n`{LinShiText}`\r\n\r\n";
                                        break;
                                    case 1:
                                        Explain = $"[explain]\r\n`{LinShiText}`\r\n\r\n";
                                        break;
                                    case 2:
                                        FlavorText = $"[flavor text]\r\n`{LinShiText}`\r\n\r\n";
                                        break;
                                    case 3:
                                        Grade = $"[grade]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 4:
                                        Rarity = $"[rarity]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 5:
                                        UsableJob = $"[usable job]\r\n`[{LinShiText}]`\r\n\r\n[/usable job]\r\n\r\n";
                                        break;
                                    case 6:
                                        AttachType = $"[attach type]\r\n`[{LinShiText}]`\r\n\r\n";
                                        break;
                                    case 7:
                                        MinimumLevel = $"[minimum level]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 8:
                                        Cooltime = $"[cool time]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 9:
                                        Price = $"[price]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 10:
                                        Value = $"[value]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 11:
                                        Icon = $"[icon]\r\n`{LinShiText}`\r\n";
                                        break;
                                    case 12:
                                        IconId = $"{LinShiText}\t\r\n";
                                        break;
                                    case 13:
                                        FieldImage = $"[field image]\r\n`{LinShiText}`\r\n";
                                        break;
                                    case 14:
                                        FieldImageId = $"{LinShiText}\t\r\n";
                                        break;
                                    case 15:
                                        MoveWav = $"[move wav]\r\n`{LinShiText}`\r\n\r\n";
                                        break;
                                    case 16:
                                        StringData = $"[string data]\r\n`[{LinShiText}]`\r\n\r\n[/string data]\r\n\r\n";
                                        break;
                                    case 17:
                                        FileName = $"{LinShiText}.stk";
                                        break;
                                }
                                break;
                            case 1:
                                IntDataMaterial.Append(LinShiText+"\t");
                                Count++;
                                break;
                            case 2:
                                IntDataEnd.Append(LinShiText + "\t");
                                Count++;
                                break;
                            case 3:
                                switch (c)
                                {
                                    case 0:
                                        IconMark = $"[icon mark]\r\n`{LinShiText}`\r\n";
                                        break;
                                    case 1:
                                        IconMarkId = $"{LinShiText}\t\r\n";
                                        break;
                                    case 2:
                                        NeedMaterialId = $"[need material]\r\n{LinShiText}\t";
                                        break;
                                    case 3:
                                        NeedMaterialSl = $"{LinShiText}\t\r\n";
                                        break;
                                    case 4:
                                        StackLimit = $"[stack limit]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 5:
                                        SuccessRate = $"[success rate]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 6:
                                        BroadcastType = $"[broadcast type]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 7:
                                        MaintainAbility = $"[maintain ability]\r\n{LinShiText}\t\r\n";
                                        break;
                                    case 8:
                                        FailOutputId = $"[fail output]\r\n{LinShiText}\t";
                                        break;
                                    case 9:
                                        FailOutputSl = $"{LinShiText}\t\r\n[/fail output]\r\n\r\n";
                                        break;
                                    case 10:
                                        UpgradeTable = $"[{LinShiText}]\r\n\r\n";
                                        break;
                                    case 11:
                                        FuZhiYeType = $"{LinShiText}\t";
                                        break;
                                    case 12:
                                        FuZhiYeLevel = $"{LinShiText}\t2\t";
                                        break;
                                    case 13:
                                        BeadItemId = $"[bead item]\r\n{LinShiText}\t";
                                        break;
                                    case 14:
                                        BeadItemSl = $"{LinShiText}\t\r\n[/bead item]\r\n\r\n";
                                        break;
                                }
                                break;
                        }
                    }

                    switch (b)
                    {
                        case 1:
                            IntDataMaterial.Insert(0, Count / 2 + "\t");
                            Count = 0;
                            break;
                        case 2:
                            IntDataEnd.Insert(0, Count / 2 + "\t");
                            Count = 0;
                            break;
                    }
                }

                if (FuZhiYeType=="")
                {
                    SheJiTu.Add(FileName, FileTou + Name+ Explain+ FlavorText+ NeedMaterialId+ NeedMaterialSl + Grade+ Rarity+ UsableJob+ AttachType+ MinimumLevel+ Cooltime+ Price+ Value+ StackLimit + Icon+ IconId+ FieldImage+ FieldImageId+ IconMark+ IconMarkId+ StkType+ MoveWav+$"[int data]\r\n{IntDataMaterial.ToString()}{IntDataEnd.ToString()}0\t\r\n[/int data]\r\n\r\n"+ SuccessRate+ BroadcastType+ MaintainAbility+ FailOutputId+ FailOutputSl+ StringData+ UpgradeTable);
                }
                else
                {
                    SheJiTu.Add(FileName, FileTou + Name + Explain + FlavorText + NeedMaterialId + NeedMaterialSl + Grade + Rarity + UsableJob + AttachType + MinimumLevel + Cooltime + Price + Value + StackLimit + Icon + IconId + FieldImage + FieldImageId + IconMark + IconMarkId + StkType + MoveWav + $"[int data]\r\n{IntDataMaterial.ToString()}{IntDataEnd.ToString()}1\t{FuZhiYeType}{FuZhiYeLevel}\r\n[/int data]\r\n\r\n" + BeadItemId + BeadItemSl + SuccessRate + BroadcastType + MaintainAbility + FailOutputId + FailOutputSl + StringData + UpgradeTable);
                }

            }
        }



    }


    public class Grid_RenWu
    {

        static readonly List<object[]> QuestTou1 = new List<object[]>(){
            new object[]{12,"任务类型",100,"接取npc",80,"完成npc",80,"接取职业",80,"接取的职业类型",110,"最低接取等级",110,"最高接取等级",110,"任务名",70,"接取的对话",85,"接取后的描述",90,"完成后的对话",90,"写出文件名",85 },
            new object[]{2,"完成条件类型",135,"完成子类型",100 },
            new object[]{1,"完成奖励类型",110 },
            new object[]{6,"活动任务",110,"无法放弃",110,"隐藏NPC",110,"奖励金币增加",130,"奖励QP点",110,"PK等级接取",120 },
            new object[]{12,"自选物品代码",110,"自选物品数量",110},
        };

        static readonly List<object[]> QuestTou2 = new List<object[]>() { 
            new object[] { 20,"任务编号", 90 } ,
            new object[] { 40,"任务物品代码", 110,"任务物品数量", 110 } ,
            new object[] {4, "副本编号", 90,"副本难度", 90,"Map地图编号", 110,"出现概率", 90 } ,
            new object[] {20, "副本编号", 90 } ,
            new object[] {20 ,"任务编号", 90 } ,
            new object[] {140, "怪物代码", 90,"副本编号", 90,"副本难度", 90,"任务物品代码", 100,"掉落数量", 90,"掉落概率", 90,"上限掉落总数", 100 } ,
            new object[] {160, "敌人代码", 90,"敌人类型", 90,"副本编号", 90,"副本难度", 90,"任务物品代码", 100,"掉落数量", 90,"掉落概率", 90,"上限掉落总数", 100},
            new object[] { 1,"任务编号", 90 }
        };

        readonly object[] QuestTou3 = {
            140,800,
            "任务类型","选择框可以选择不同的任务类型，并且可自动填充单元格",
            "接取npc","任务接取的npc编号；例如：“2”则任务在赛丽亚处接取",
            "完成npc","完成任务条件后需要去对应的npc交付任务；例如：“-1”则直接完成 “2”则需要去赛丽亚处交付",
            "接取职业","只有对应的角色才可看到任务；例如：“all”则所有角色都可看到并接取 “swordman”则只有鬼剑士",
            "接取的职业类型","只有对应角色的职业才可看到任务；例如“-1”则所有职业 “0”则没转职才可看见",
            "最低接取等级","只有达到对应等级才可接取任务；例如“1”则角色等级达到1级时才看到并可接取",
            "最高接取等级","只有在对应等级内才可接取任务；例如“99”则角色等级不超过99级时才看到并可接取",
            "任务名","任务的名称",
            "接取的对话","当玩家接取任务时，弹出的npc对话的内容",
            "接取后的描述","当玩家接取任务后，任务窗口中的任务描述信息",
            "完成后的对话","当玩家完成并交付任务时的npc对话内容",
            "完成条件类型","任务完成的条件，在“任务必填项”中可自行选择",
            "完成子类型","任务完成的条件的子类型，在“任务必填项”中可自行选择",
            "完成奖励类型","任务完成后给予的奖励类型，在“任务必填项”中可自行选择",
            "活动任务","在任务窗口中会显示绿色样式并且有置顶效果；例如：“1”则开启效果 “0”或不填写则不开启效果",
            "无法放弃","接取任务后无法放弃任务；例如：“1”则开启效果 “0”或不填写则不开启效果",
            "隐藏NPC","接取任务后会在城镇中隐藏填写的npc；例如“2”则接取任务后赛丽亚将会被隐藏",
            "奖励金币增加","如果给予奖励中有代码0，会按照百分比增加奖励的金币数量；例如：“100”则奖励的金币X2",
            "奖励QP点","在奖励的物品中增加QP点；例如：“100”则完成任务后会额外给予100点QP的奖励",
            "PK等级接取","需要达到对应的PK等级才可接取任务；例如：“0”则需要段位10级 “1”则需要段位9级；上限为34",
            "自选物品代码","在设置的6个自选物品中只能选择一样作为奖励；这里需要在对应单元格填写代码",
            "自选物品数量","在设置的6个自选物品中只能选择一样作为奖励；这里需要在对应单元格填写数量",
            "前置任务","例如：“1231”则需要完成任务编号1231才可显示此任务",
            "给予道具","接取任务时会给予角色一个或多个任务物品",
            "额外地图","接取任务后会在设定的副本中额外增加并且出现一个map房间",
            "限制副本","限制此任务完成必须在指定的副本编号中才可完成；例如：“-1”则通关任何副本都可完成",
            "冲突任务","如果接取在此设置的任务编号，哪么该任务将不会显示",
            "怪物掉落","可以指定某个副本某个怪物可以掉落任务类型的物品",
            "敌人掉落","可以指定某个副本某个敌人可以掉落任务类型的物品；类型：“1”为怪物 “2”为apc",
            "弹出任务","例如：“1231”则完成任务后会自动弹出1231这个任务；常用为主线任务"
        };


        /// <summary>
        /// 用于验证任务中的第一页是否有空单元格
        /// </summary>
        /// <param name="reoGridControl">被检查的表格控件</param>
        /// <returns></returns>
        public string 验证AllText(ReoGridControl reoGridControl)
        {

            for (int i = 0; i < reoGridControl.Worksheets[0].RowCount; i++)//循环第一页的所有单元格
            {
                for (int a = 0; a < reoGridControl.Worksheets[0].ColumnCount; a++)//循环所有列
                {
                    if (reoGridControl.Worksheets[0].GetCellText(i, a) == "")
                        return i.ToString() + a.ToString();//如果单元格空，则返回列和行序号
                }

            }
            return "0";
        }

        /// <summary>
        /// 用于验证任务中的文件名设置是否正确
        /// </summary>
        /// <param name="reoGridControl">被检查的表格控件</param>
        /// <returns></returns>
        public int 验证FileName(ReoGridControl reoGridControl)
        {
            List<string> YZ = new List<string>();

            for (int i = 0; i < reoGridControl.Worksheets[0].RowCount; i++)
            {

                string Text = reoGridControl.Worksheets[0].GetCellText(i, 11);
                if (Text != "")
                {
                    if (YZ.Contains(Text) == false)
                    {
                        YZ.Add(Text);
                    }
                    else
                    {
                        return i++;
                    }
                }
                else
                {
                    return i++;
                }

            }
            return 0;
        }

        /// <summary>
        /// 当选择任务条件为宠物进化时需要限制的功能以及提示
        /// </summary>
        /// <param name="reoGridControl_New1">被更改的任务表格</param>
        /// <param name="comboBox_New1">被更改的选择文本框</param>
        public void 宠物进化Yes(ReoGridControl reoGridControl_New1, ComboBox comboBox_New1)
        {
            comboBox_New1.Enabled = false;
            comboBox_New1.Items[0] = "必须选择自选奖励";
            reoGridControl_New1.Worksheets[2].ColumnCount = 1;
            reoGridControl_New1.Worksheets[2].Name = "不可用";
            reoGridControl_New1.Worksheets[2].ColumnHeaders[0].Text = "不可用";
            for (int i = 0; i < reoGridControl_New1.Worksheets[4].ColumnCount; i += 2)
            {
                reoGridControl_New1.Worksheets[4].ColumnHeaders[i].Text = "宠物Equ代码";
                reoGridControl_New1.Worksheets[4].ColumnHeaders[i + 1].Text = "数量；必须为1";
            }
        }

        /// <summary>
        /// 当选择任务条件不为宠物进化时需要解除的功能以及提示
        /// </summary>
        /// <param name="reoGridControl_New1">被更改的任务表格</param>
        /// <param name="comboBox_New1">被更改的选择文本框</param>
        public void 宠物进化No(ReoGridControl reoGridControl_New1, ComboBox comboBox_New1)
        {
            comboBox_New1.Enabled = true;
            comboBox_New1.Items[0] = "选择完成奖励类型";
            reoGridControl_New1.Worksheets[2].ColumnCount = 1;
            reoGridControl_New1.Worksheets[2].Name = "必填完成奖励";
            reoGridControl_New1.Worksheets[2].ColumnHeaders[0].Text = "完成奖励类型";
            for (int i = 0; i < reoGridControl_New1.Worksheets[4].ColumnCount; i += 2)
            {
                reoGridControl_New1.Worksheets[4].ColumnHeaders[i].Text = "自选物品代码";
                reoGridControl_New1.Worksheets[4].ColumnHeaders[i + 1].Text = "自选物品数量";
            }
        }

        /// <summary>
        /// 用于设置任务初始化表格参数
        /// </summary>
        /// <param name="reoGridControl">需要被初始化的第三方表格控件</param>
        public void 初始化任务表格框架(ReoGridControl reoGridControl_New1, ReoGridControl reoGridControl_New2)
        {

            while (reoGridControl_New1.Worksheets.Count != 0 || reoGridControl_New2.Worksheets.Count != 0)
            {
                if (reoGridControl_New1.Worksheets.Count != 0)
                    reoGridControl_New1.Worksheets.RemoveAt(0);

                if (reoGridControl_New2.Worksheets.Count != 0)
                    reoGridControl_New2.Worksheets.RemoveAt(0);
            }

            reoGridControl_New1.Worksheets.Add(reoGridControl_New1.Worksheets.Create("任务基础框架"));
            reoGridControl_New1.Worksheets.Add(reoGridControl_New1.Worksheets.Create("必填完成条件"));
            reoGridControl_New1.Worksheets.Add(reoGridControl_New1.Worksheets.Create("必填完成奖励"));
            reoGridControl_New1.Worksheets.Add(reoGridControl_New1.Worksheets.Create("额外选择项"));
            reoGridControl_New1.Worksheets.Add(reoGridControl_New1.Worksheets.Create("自选奖励"));

            reoGridControl_New2.Worksheets.Add(reoGridControl_New2.Worksheets.Create("前置任务"));
            reoGridControl_New2.Worksheets.Add(reoGridControl_New2.Worksheets.Create("给予道具"));
            reoGridControl_New2.Worksheets.Add(reoGridControl_New2.Worksheets.Create("额外地图"));
            reoGridControl_New2.Worksheets.Add(reoGridControl_New2.Worksheets.Create("限制副本"));
            reoGridControl_New2.Worksheets.Add(reoGridControl_New2.Worksheets.Create("冲突任务"));
            reoGridControl_New2.Worksheets.Add(reoGridControl_New2.Worksheets.Create("怪物掉落"));
            reoGridControl_New2.Worksheets.Add(reoGridControl_New2.Worksheets.Create("敌人掉落"));
            reoGridControl_New2.Worksheets.Add(reoGridControl_New2.Worksheets.Create("弹出任务"));

            //设置第一个表格的数据
            for (int i = 0; i < reoGridControl_New1.Worksheets.Count; i++)
            {
                int ColCount = (int)QuestTou1[i][0];
                reoGridControl_New1.Worksheets[i].ColumnCount = ColCount;

                object[] LinShi = new TongYongFangFa().GetObjShuZu(QuestTou1[i], 1, QuestTou1[i].Length - 1);

                if (ColCount == LinShi.Length / 2)
                {
                    for (int a = 0; a < ColCount; a++)
                    {
                        reoGridControl_New1.Worksheets[i].ColumnHeaders[a].Text = (string)LinShi[a * 2];
                        reoGridControl_New1.Worksheets[i].ColumnHeaders[a].Width = Convert.ToUInt16(LinShi[a * 2 + 1]);
                    }
                }
                else
                {
                    int Pos = 0;
                    for (int a = 0; a < ColCount; a++)
                    {
                        if (Pos > LinShi.Length - 1)
                            Pos = 0;

                        reoGridControl_New1.Worksheets[i].ColumnHeaders[a].Text = (string)LinShi[Pos];
                        reoGridControl_New1.Worksheets[i].ColumnHeaders[a].Width = Convert.ToUInt16(LinShi[Pos + 1]);

                        Pos += 2;
                    }
                }

            }

            //设置第二个表格的数据
            for (int i = 0; i < reoGridControl_New2.Worksheets.Count; i++)
            {
                int ColCount = (int)QuestTou2[i][0];
                reoGridControl_New2.Worksheets[i].ColumnCount = ColCount;

                object[] LinShi = new TongYongFangFa().GetObjShuZu(QuestTou2[i], 1, QuestTou2[i].Length - 1);

                if (ColCount == LinShi.Length / 2)
                {
                    for (int a = 0; a < ColCount; a++)
                    {
                        reoGridControl_New2.Worksheets[i].ColumnHeaders[a].Text = (string)LinShi[a * 2];
                        reoGridControl_New2.Worksheets[i].ColumnHeaders[a].Width = Convert.ToUInt16(LinShi[a * 2 + 1]);
                    }
                }
                else
                {
                    int Pos = 0;
                    for (int a = 0; a < ColCount; a++)
                    {
                        if (Pos > LinShi.Length - 1)
                            Pos = 0;

                        reoGridControl_New2.Worksheets[i].ColumnHeaders[a].Text = (string)LinShi[Pos];
                        reoGridControl_New2.Worksheets[i].ColumnHeaders[a].Width = Convert.ToUInt16(LinShi[Pos + 1]);

                        Pos += 2;
                    }
                }

            }

            new Grid_SheJiTu().设置行数(reoGridControl_New1, 25);
            new Grid_SheJiTu().设置行数(reoGridControl_New2, 25);
        }

        public void 任务查看信息初始化(ReoGridControl reoGridControl_New1)
        {
            //设置列数
            reoGridControl_New1.CurrentWorksheet.ColumnCount = 1;
            //设置行数
            reoGridControl_New1.CurrentWorksheet.RowCount = QuestTou3.Length/2-1;
            //设置行高
            reoGridControl_New1.CurrentWorksheet.SetRowsHeight(0, QuestTou3.Length / 2 - 1, 25);
            //设置列宽
            reoGridControl_New1.CurrentWorksheet.RowHeaderWidth = Convert.ToInt32(QuestTou3[0]);
            reoGridControl_New1.CurrentWorksheet.SetColumnsWidth(0, 1, Convert.ToUInt16(QuestTou3[1]));

            int Pos = 0;
            for (int i = 2; i < QuestTou3.Length; i+=2)
            {
                reoGridControl_New1.CurrentWorksheet.RowHeaders[Pos].Text = QuestTou3[i].ToString();
                reoGridControl_New1.CurrentWorksheet[Pos, 0] = QuestTou3[i+1].ToString();
                Pos++;
            }
        }

        public void AddSheJiTuInfo(ReoGridControl reoGridControl_New1,ReoGridControl reoGridControl_New2, Dictionary<string, string> RenWu)
        {
            for (int a = 0; a < reoGridControl_New1.Worksheets[0].RowCount; a++)
            {
                string Grade = "";
                string NpcId = "";
                string CompleteNpcId = "";
                string Job = "";
                string GrowType = "";
                string Level = "";
                string Name = "";
                string DependMessage = "";
                string ConditionMessage = "";
                string SolveMessage = "";
                string RenWuFileName = "";

                string Type = "";
                string SubType = "";
                StringBuilder IntData = new StringBuilder();

                string RewardType = "";
                StringBuilder RewardIntData = new StringBuilder();
                
                StringBuilder RewardSelectionIntData = new StringBuilder();

                string Event = "";
                string CantGiveup = "";
                string DeleteNpcId = "";
                string GoldMultiple = "";
                string QuestPoint = "";
                string PVPRANK = "";

                StringBuilder PreRequiredQuest = new StringBuilder();
                StringBuilder DependGiveItem = new StringBuilder();
                StringBuilder AppearMap = new StringBuilder();
                StringBuilder LimitDungeonId = new StringBuilder();
                StringBuilder CollisionQuest = new StringBuilder();
                StringBuilder MonsterRewardItem = new StringBuilder();
                StringBuilder EnemyRewardItem = new StringBuilder();
                string RelationQuest = "";
                
                string FileTou = "#PVF_File\r\n\r\n";

                for (int b = 0; b < reoGridControl_New1.Worksheets.Count; b++)
                {
                    for (int c = 0; c < reoGridControl_New1.Worksheets[b].ColumnCount; c++)
                    {
                        string AddText = reoGridControl_New1.Worksheets[b].GetCellText(a,c);
                        if (AddText == "")
                            continue;

                        switch (b)
                        {
                            case 0:
                                switch (c)
                                {
                                    case 0:
                                        Grade = $"[grade]\r\n`[{AddText}]`\r\n\r\n";
                                        break;
                                    case 1:
                                        NpcId = $"[npc index]\r\n{AddText}\t\r\n";
                                        break;
                                    case 2:
                                        CompleteNpcId = $"[complete npc index]\r\n{AddText}\t\r\n";
                                        break;
                                    case 3:
                                        Job = $"[job]\r\n`[{AddText}]`\r\n\r\n";
                                        break;
                                    case 4:
                                        GrowType = $"[grow type]\r\n{AddText}\t\r\n";
                                        break;
                                    case 5:
                                        Level = $"[level]\r\n{AddText}\t";
                                        break;
                                    case 6:
                                        Level += $"{AddText}\t\r\n";
                                        break;
                                    case 7:
                                        Name = $"[name]\r\n`{AddText}`\r\n\r\n";
                                        break;
                                    case 8:
                                        DependMessage = $"[depend message]\r\n`{AddText}`\r\n\r\n";
                                        break;
                                    case 9:
                                        ConditionMessage = $"[condition message]\r\n`{AddText}`\r\n\r\n";
                                        break;
                                    case 10:
                                        SolveMessage = $"[solve message]\r\n`{AddText}`\r\n\r\n";
                                        break;
                                    case 11:
                                        RenWuFileName = $"{AddText}.qst";
                                        break;
                                }
                                break;
                            case 1:
                                switch (c)
                                {
                                    case 0:
                                        Type = $"[type]\r\n`[{AddText}]`\r\n\r\n";
                                        break;
                                    case 1:
                                        SubType = $"[sub type]\r\n{AddText}\t\r\n";
                                        break;
                                    default:
                                        IntData.Append(AddText + "\t");
                                        break;
                                }
                                break;
                            case 2:
                                switch (c)
                                {
                                    case 0:
                                        RewardType = $"[reward type]\r\n`[{AddText}]`\r\n\r\n";
                                        break;
                                    default:
                                        RewardIntData.Append(AddText + "\t");
                                        break;
                                }
                                break;
                            case 3:
                                switch (c)
                                {
                                    case 0:
                                        Event = $"[event]\r\n{AddText}\t\r\n";
                                        break;
                                    case 1:
                                        CantGiveup = $"[cant giveup]\r\n{AddText}\t\r\n";
                                        break;
                                    case 2:
                                        DeleteNpcId = $"[delete npc index]\r\n{AddText}\t\r\n";
                                        break;
                                    case 3:
                                        GoldMultiple = $"[gold multiple]\r\n{AddText}\t\r\n";
                                        break;
                                    case 4:
                                        QuestPoint = $"[quest point]\r\n{AddText}\t\r\n";
                                        break;
                                    case 5:
                                        PVPRANK = $"[PVP RANK]\r\n{AddText}\t\r\n";
                                        break;
                                }
                                break;
                            case 4:
                                RewardSelectionIntData.Append(AddText + "\t");
                                break;
                        }
                    }
                }

                for (int b = 0; b < reoGridControl_New2.Worksheets.Count; b++)
                {
                    for (int c = 0; c < reoGridControl_New2.Worksheets[b].ColumnCount; c++)
                    {
                        string Text = reoGridControl_New2.Worksheets[b].GetCellText(a,c);
                        if (Text == "")
                            break;

                        switch (b)
                        {
                            case 0:
                                PreRequiredQuest.Append(Text+"\t");
                                break;
                            case 1:
                                DependGiveItem.Append(Text + "\t");
                                break;
                            case 2:
                                AppearMap.Append(Text + "\t");
                                break;
                            case 3:
                                LimitDungeonId.Append(Text + "\t");
                                break;
                            case 4:
                                CollisionQuest.Append(Text + "\t");
                                break;
                            case 5:
                                MonsterRewardItem.Append(Text + "\t");
                                break;
                            case 6:
                                EnemyRewardItem.Append(Text + "\t");
                                break;
                            case 7:
                                RelationQuest = $"[relation quest]\r\n{Text}\t\r\n";
                                break;
                        }
                    }
                }

                if(IntData.Length>0)
                    IntData.Insert(0,"[int data]\r\n",1).Append("\r\n[/int data]\r\n\r\n");

                if(RewardIntData.Length>0)
                    RewardIntData.Insert(0,"[reward int data]\r\n",1).Append("\r\n[/reward int data]\r\n\r\n");

                if(RewardSelectionIntData.Length > 0)
                    RewardSelectionIntData.Insert(0,"[reward selection int data]\r\n",1).Append("\r\n[/reward selection int data]\r\n\r\n");

                if (PreRequiredQuest.Length > 0)
                    PreRequiredQuest.Insert(0, "[pre required quest]\r\n", 1).Append("\r\n[/pre required quest]\r\n\r\n");
                if (DependGiveItem.Length > 0)
                    DependGiveItem.Insert(0, "[depend give item]\r\n", 1).Append("\r\n[/depend give item]\r\n\r\n");
                if (AppearMap.Length > 0)
                    AppearMap.Insert(0, "[appear map]\r\n", 1).Append("\r\n");
                if (LimitDungeonId.Length > 0)
                    LimitDungeonId.Insert(0, "[limit dungeon index]\r\n", 1).Append("\r\n[/limit dungeon index]\r\n\r\n");
                if (CollisionQuest.Length > 0)
                    CollisionQuest.Insert(0, "[collision quest]\r\n", 1).Append("\r\n[/collision quest]\r\n\r\n");
                if (MonsterRewardItem.Length > 0)
                    MonsterRewardItem.Insert(0, "[monster reward item]\r\n", 1).Append("\r\n[/monster reward item]\r\n\r\n");
                if (EnemyRewardItem.Length > 0)
                    EnemyRewardItem.Insert(0, "[enemy reward item]\r\n", 1).Append("\r\n[/enemy reward item]\r\n\r\n");


                RenWu.Add(RenWuFileName, FileTou + Grade + NpcId + CompleteNpcId + Job + GrowType + Level + Event + CantGiveup + DeleteNpcId + GoldMultiple + QuestPoint + PVPRANK + PreRequiredQuest.ToString() + DependGiveItem.ToString() + AppearMap.ToString() + LimitDungeonId.ToString() + CollisionQuest.ToString()+ MonsterRewardItem.ToString()+ EnemyRewardItem.ToString()+Type + SubType + IntData.ToString() + RewardType + RewardIntData.ToString() + RewardSelectionIntData.ToString() + RelationQuest+Name + DependMessage + ConditionMessage + SolveMessage);

            }
        }

    }

}
