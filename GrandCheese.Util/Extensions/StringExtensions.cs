using GrandCheese.Util.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util.Extensions
{
    public static class StringExtensions
    {
        public static GCWideString ToWideString(this string str)
        {
            return new GCWideString(str);
        }
    }
}
