using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace PVFSimple.群验证
{
    public class Verification
    {
        public static string GetCpuID()
        {
            try
            {
                //获取CPU序列号代码
                string cpuInfo = "";//cpu序列号
                using (ManagementClass mc = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                    }
                    moc.Dispose();
                    mc.Dispose();
                }
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>
        /// 获取硬盘ID  
        /// </summary>
        /// <returns></returns>
        public static string GetHDid()
        {
            try
            {
                //查询得到系统盘所在硬盘的ID = 0。然后我们通过该ID，查询该硬盘信息。
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT DiskIndex FROM Win32_DiskPartition WHERE Bootable = TRUE"))
                {
                    foreach (ManagementObject mooo in searcher.Get())
                    {
                        int index = Convert.ToInt32(mooo.Properties["DiskIndex"].Value);
                        using ManagementObjectSearcher searcher_model = new ManagementObjectSearcher("SELECT Model FROM Win32_DiskDrive WHERE Index = " + index);
                        ManagementObjectCollection moc1 = searcher_model.Get();
                        foreach (ManagementObject mo in moc1)
                        {
                            string modelname = (string)mo.Properties["Model"].Value;
                            return modelname;
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///   <summary> 
        ///   获取网卡硬件地址 
        ///   </summary> 
        ///   <returns> string </returns> 
        public static string GetMoAddress()
        {
            try
            {
                string MoAddress = null;
                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    ManagementObjectCollection moc2 = mc.GetInstances();
                    foreach (ManagementObject mo in moc2)
                    {
                        if ((bool)mo["IPEnabled"] == true)
                            MoAddress = mo["MacAddress"].ToString();
                        mo.Dispose();
                    }
                }
                return MoAddress;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 通过字符串获取MD5值，返回32位字符串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetMD5String()
        {
            string s = GetCpuID() + GetHDid() + GetMoAddress();

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(s));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            md5Hasher.Dispose();
            return sBuilder.ToString().ToLower();
        }

        /// <summary>
        /// 删除程序自身
        /// </summary>
        public void End()
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/C ping 1.1.1.1 -n 1 -w 1000 > Nul & Del " + Application.ExecutablePath)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Process.Start(psi);
            Application.Exit();
        }

        /// <summary>
        /// 获取外网IP地址
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://ip.tool.chinaz.com/");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                Regex_new regex = new Regex_new();
                regex.创建(sr.ReadToEnd(), ">您的IP<.+\r\n.+\">(.+)</dd>\r\n.+\r\n.+<dd>([^<]*)<", 1);
                tempip = regex.取子匹配文本(1, 1)+" "+ regex.取子匹配文本(1, 2);
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return tempip;
        }

    }

}
