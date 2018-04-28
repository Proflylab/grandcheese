using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util.Interfaces
{
    public interface ISerializable
    {
        void Serialize(Packet packet);
    }
}
