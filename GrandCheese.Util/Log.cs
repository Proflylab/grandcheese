using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese
{
    public class Log
    {
        public static Logger Get()
        {
            return LogManager.GetLogger("GrandCheese");
        }
    }
}
