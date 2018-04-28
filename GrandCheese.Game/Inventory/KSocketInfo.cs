using GrandCheese.Game.User;
using GrandCheese.Util;
using GrandCheese.Util.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.Inventory
{
    class KSocketInfo : ISerializable
    {
        // todo: uint needs to be added as a type in the serializer
        public uint CardItemId { get; set; } = (char)0x00;
        public char SlotId { get; set; } = (char)0xFF;
        public char State { get; set; } = (char)0x02;

        public void Serialize(Packet packet, int i, object kUser = null)
        {
            packet.Put(
                CardItemId,
                SlotId,
                State
            );
        }
    }
}
