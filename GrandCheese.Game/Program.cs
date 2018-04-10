using GrandCheese.Util;
using GrandCheese.Util.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game
{
    class Program : ServerApp
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.Title = "Grand Cheese Game";

            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine("         Grand Cheese Season V / Game     ");
            Console.WriteLine("  ////////////////////////////////////////");
            Console.WriteLine();

            StartServer(9401);

            // This won't work on Mono probably
            while (true) Console.ReadLine();
        }
    }
}
