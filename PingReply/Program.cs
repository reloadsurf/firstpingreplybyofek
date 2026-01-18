// for sunday

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.Net.Sockets;
using System.Net.Http;
using System.Windows.Forms;

namespace visualpingr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            ipad.Text = GetLocalIPAddress();
            statusping.Text = GetSubnetMask();
            Task.Run(() =>
            {
                Parallel.For(1, 255, i =>
                {
                    string ip = "10.67.200." + i;
                    string status = SendPing(ip);
                    listView1.Invoke(new Action(() =>
                    {

                        var item1 = new ListViewItem(new[] { ip, status });
                        listView1.Items.Add(item1);
                    }));
                });
            });
        }
        public static String SendPing(String ip)
        {
            Ping ping = new Ping();
            PingReply pingreply = ping.Send(ip);
            return pingreply.Status.ToString();
        }


        static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);

            if (ip == null)
                throw new Exception("No network adapters with an IPv4 address in the system!");

            return ip.ToString();
        }

        public string GetSubnetMask()
        {
            // Look through all network interfaces (Ethernet, Wi-Fi, etc.)
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only look at interfaces that are actually UP
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation unicastIP in adapter.GetIPProperties().UnicastAddresses)
                    {
                        // Filter for IPv4 and ignore the loopback (127.0.0.1)
                        if (unicastIP.Address.AddressFamily == AddressFamily.InterNetwork && !unicastIP.Address.ToString().StartsWith("127"))
                        {
                            return unicastIP.IPv4Mask.ToString();
                        }
                    }
                }
            }
            return "Not Found";
        }

        public static double GetTotalIps(string subnetMask)
        {
            System.Net.IPAddress mask = System.Net.IPAddress.Parse(subnetMask);

            byte[] bytes = mask.GetAddressBytes();
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            uint maskInt = BitConverter.ToUInt32(bytes, 0);
            uint hostBits = ~maskInt;
            return Math.Pow(2, CountSetBits(hostBits));
        }

        private static int CountSetBits(uint n)
        {
            int count = 0;
            while (n > 0)
            {
                n &= (n - 1);
                count++;
            }
            return count;
        }
    }
}
