﻿using Dapper;
using GrandCheese.Game.User;
using GrandCheese.Game.Data;
using GrandCheese.Util;
using GrandCheese.Util.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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

            var serverApp = new ServerApp();

            serverApp.CreateUserClient = (Client c) =>
            {
                c.User = (object)(new UserClient(c));
            };

            serverApp.CustomInvoke = (ServerApp app, Client c, Packet p, short opcode) =>
            {
                if (c.User == null)
                {
                    c.User = (object)(new UserClient(c));
                }

                var user = (UserClient)c.User;
                var method = app.serverPackets[opcode];

                Console.WriteLine(method.DeclaringType.Name);
                var invocationArgs = new List<object>();

                foreach (var param in method.GetParameters())
                {
                    if (param.ParameterType == typeof(Client))
                    {
                        invocationArgs.Add(c);
                    }
                    else if (param.ParameterType == typeof(Packet))
                    {
                        invocationArgs.Add(p);
                    }
                    else if (param.ParameterType == typeof(KUser))
                    {
                        invocationArgs.Add(user.KUser);
                    }
                }

                switch (method.DeclaringType.Name)
                {
                    case "KUser":
                        method.Invoke(user.KUser, invocationArgs.ToArray());
                        break;
                    default:
                        try
                        {
                            method.Invoke(null, invocationArgs.ToArray());
                        }
                        catch (TargetInvocationException ex)
                        {
                            Log.Get().Error(ex, "Unhandled packet. Failed to call as static.");
                        }
                        break;
                }
            };

            serverApp.StartServer(Data.Data.Server.Port, "game");

            // This won't work on Mono probably
            while (true) Console.ReadLine();
        }
    }
}
