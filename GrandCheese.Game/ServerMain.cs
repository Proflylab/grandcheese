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
using System.Reflection;
using GrandCheese.Data;
using MoonSharp.Interpreter;

namespace GrandCheese.Game
{
    class ServerMain
    {
        public static KServerInfo Info = null;

        static string GetLuaInfo()
        {
            string versionString = Lua.script.DoString("return _VERSION").String;
            string luaCompat = Lua.script.DoString("return _MOONSHARP.luacompat").String;

            return $"{versionString} (Lua {luaCompat}-compatible)";
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = "Grand Cheese Game";

            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine("         Grand Cheese Season V / Game     ");
            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine();
            
            Lua.RunLuaScript("Config");

            using (var db = Database.Get())
            {
                var server = db.Query<KServerInfo>("SELECT * FROM servers WHERE port = @port",
                    new { port = Lua.GetLuaGlobal("port").CastToNumber() }).FirstOrDefault();

                if (server != null)
                {
                    Info = server;
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
                }
            };

            serverApp.StartServer(Info.Port, "game");

            Lua.RunLuaScript("CharDefaultInfo");

            if (serverApp.initialized)
            {
                // Delay the loading of the shell just in case...
                System.Threading.Thread.Sleep(2500);

                // Start the Lua shell
                Console.WriteLine();
                Console.WriteLine(GetLuaInfo());
                
                //var script = new Script();

                while (true)
                {
                    Console.Write("> ");
                    var input = Console.ReadLine();

                    if (input == "exit")
                    {
                        Environment.Exit(0);
                    }

                    try
                    {
                        Console.WriteLine(Lua.script.DoString(input).CastToString());
                    }
                    catch (Exception ex)
                    {
                        Log.Get().Fatal(ex.Message);
                    }
                }
            }
        }
    }
}
