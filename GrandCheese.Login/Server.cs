using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandCheese.Util;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using GrandCheese.Util.Models;
using Dapper;
using GrandCheese.Data;

namespace GrandCheese
{
    class ServerMain
    {
        public static List<KServerInfo> Servers = new List<KServerInfo>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = "Grand Cheese Login";
            
            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine("       Grand Cheese Season V / Center     ");
            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine();

            Log.Get().Info("Getting server list...");
            using (var db = Database.Get())
            {
                var dbServers = db.Query<KServerInfo>("SELECT * FROM servers").ToArray();

                foreach (var server in dbServers)
                {
                    Servers.Add(server);
                }
            }
            Log.Get().Info("Loaded {0} server{1}.", Servers.Count, Servers.Count == 1 ? "" : "s");

            var serverApp = new ServerApp();
            serverApp.StartServer(9501);

            // This won't work on Mono probably
            while (true) Console.ReadLine();
        }
    }
}
