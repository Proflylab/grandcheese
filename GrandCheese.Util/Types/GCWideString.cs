using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util.Types
{
    public class GCWideString
    {
        readonly string _value;

        public GCWideString(string str)
        {
            _value = str;
        }
        
        public override string ToString()
        {
            return _value;
        }

        public static implicit operator string(GCWideString str)
        {
            return str.ToString();
        }

        public static implicit operator GCWideString(string str)
        {
            return new GCWideString(str);
        }
    }
}
