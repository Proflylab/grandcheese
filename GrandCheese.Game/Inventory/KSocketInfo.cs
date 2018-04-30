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
        public uint CardItemId { get; set; } = 0;
        public byte SlotId { get; set; } = 0x00; // 0xFF
        public byte State { get; set; } = 0x02;

        public void Serialize(Packet packet, int i, params object[] optional)
        {
            packet.Put(
                CardItemId,
                SlotId,
                State
            );
        }
    }
}
