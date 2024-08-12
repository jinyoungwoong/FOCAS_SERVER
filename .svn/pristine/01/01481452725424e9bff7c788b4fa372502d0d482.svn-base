using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NCManagementSystem.Components.License
{
    class DeviceInfo
    {
        public static List<string> GetAllSerialNoList(bool includeLogialHdd = false)
        {
            List<string> serialNumbers = new List<string>();
            serialNumbers.Add(GetOsHddSerialNo());
            serialNumbers.AddRange(GetPhysicalHddSerialNoList());

            if (includeLogialHdd)
                serialNumbers.AddRange(GetLogicalHddSerialNoList());

            return serialNumbers;
        }

        private static string GetOsHddSerialNo()
        {
            ManagementObjectSearcher finder = new ManagementObjectSearcher("Select * from Win32_OperatingSystem");
            string name = "";
            string serialNumber = "";
            foreach (ManagementObject OS in finder.Get())
                name = OS["Name"].ToString();

            int ind = name.IndexOf("Harddisk") + 8;
            int hardIndex = Convert.ToInt16(name.Substring(ind, 1));

            finder = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE Index=" + hardIndex);
            foreach (ManagementObject HardDisks in finder.Get())
                foreach (ManagementObject HardDisk in HardDisks.GetRelated("Win32_PhysicalMedia"))
                    serialNumber = HardDisk["SerialNumber"].ToString();

            return serialNumber;
        }

        private static List<string> GetPhysicalHddSerialNoList()
        {
            ManagementObjectSearcher moSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            List<string> serialNumbers = new List<string>();

            foreach (ManagementObject wmi_HD in moSearcher.Get())
            {
                if (wmi_HD["SerialNumber"] != null)
                    serialNumbers.Add(wmi_HD["SerialNumber"].ToString());
            }

            return serialNumbers;
        }

        private static List<string> GetLogicalHddSerialNoList()
        {
            List<string> serialNumbers = new List<string>();

            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                var drive = driveInfo.Name.Replace(":\\", "");
                ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"" + drive + ":\"");
                disk.Get();

                if (disk["VolumeSerialNumber"] != null)
                    serialNumbers.Add(disk["VolumeSerialNumber"].ToString());
            }

            return serialNumbers;
        }

        public static string GetInternalIPAddress(string pSpliiter = null)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            if (string.IsNullOrEmpty(pSpliiter))
            {
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        sb.Append(ip.ToString());
                        sb.Append(pSpliiter);
                    }
                }
                return sb.ToString();
            }

            return "";
            //throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string GetExternalIPAddress()
        {
            string externalip = new WebClient().DownloadString("http://ipinfo.io/ip").Trim(); //http://icanhazip.com

            if (String.IsNullOrWhiteSpace(externalip))
            {
                externalip = GetInternalIPAddress();//null경우 Get Internal IP를 가져오게 한다.
            }

            return externalip;
        }

        public static string GetMacAddress(string pSpliiter = null)
        {
            var netInf = NetworkInterface.GetAllNetworkInterfaces();

            if (netInf.Length > 0)
            {
                if (string.IsNullOrEmpty(pSpliiter))
                {
                    return netInf[0].GetPhysicalAddress().ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var inf in netInf)
                    {
                        sb.Append(inf.GetPhysicalAddress().ToString());
                        sb.Append(pSpliiter);
                    }
                    return sb.ToString();
                }
            }

            return "";
        }
    }
}
