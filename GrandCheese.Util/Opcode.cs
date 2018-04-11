using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OpcodeAttribute : Attribute
    {
        public short id;

        public OpcodeAttribute(short id) {
            this.id = id;
        }

        public OpcodeAttribute(LoginOpcodes id)
        {
            this.id = (short)id;
        }

        public OpcodeAttribute(GameOpcodes id)
        {
            this.id = (short)id;
        }
    }
}
