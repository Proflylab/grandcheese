using Dapper;
using GrandCheese.Game.User;
using GrandCheese.Game.Data;
using GrandCheese.Util;
using GrandCheese.Util.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = "Grand Cheese Game";

            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine("         Grand Cheese Season V / Game     ");
            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine();

            using (var db = Database.Get())
            {
                var server = db.Query<Server>("SELECT * FROM servers WHERE port = @port",
                    new { port = 9401 }).FirstOrDefault();

                if (server != null)
                {
                    Data.Data.Server = server;
                }
            }

            var serverApp = new ServerApp
            {
                CreateUserClient = (Client c) =>
                {
                    c.User = new UserClient(c);
                },

                CustomInvoke = (ServerApp app, Client c, Packet p, short opcode) =>
                {
                    if (c.User == null)
                    {
                        c.User = new UserClient(c);
                    }

                    var user = (UserClient)c.User;
                    var method = app.serverPackets[opcode];

                    Console.WriteLine(method.DeclaringType.Name);

                    switch (method.DeclaringType.Name)
                    {
                        case "KUser":
                            method.Invoke(user.KUser, new object[] { c, p });
                            break;
                        default:
                            if (method.GetParameters().Length == 1)
                            {
                                method.Invoke(null, new object[] { user.KUser });
                            }
                            else
                            {
                                Log.Get().Warn("Unhandled. Attempting to call as static.");
                                method.Invoke(null, new object[] { c, p });
                            }
                            break;
                    }
                }
            };

            serverApp.StartServer(Data.Data.Server.Port, "game");

            // This won't work on Mono probably
            while (true) Console.ReadLine();
        }
    }
}
