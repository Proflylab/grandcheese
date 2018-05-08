using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util.Interfaces
{
    public class IListCount<T> : List<T>
    {
        public int index = 0;
    }
}
