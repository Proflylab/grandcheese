using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandCheese.Util;
using GrandCheese.Packets;
using System.Net.Sockets;
using System.Net;
using System.Reflection;

namespace GrandCheese
{
    class Program : Server
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = "Grand Cheese Login";
            
            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine("       Grand Cheese Season V / Center     ");
            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine();
            
            StartServer();

            // This won't work on Mono probably
            while (true) Console.ReadLine();
        }
    }
}
