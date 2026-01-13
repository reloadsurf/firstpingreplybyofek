using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace pr
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i < 255; i++)
            {
                String ip = "10.67.200." + i;
                SendPing(ip);
            }

        }

        public static void SendPing(String ip)
        {
            Ping ping = new Ping();
            PingReply pingreply = ping.Send(ip);
            Console.WriteLine($"{ip} status is: {pingreply.Status.ToString()}");

        }
    }
}
