using GrandCheese.Data;
using GrandCheese.Util;
using GrandCheese.Util.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.Inventory
{
    class KAttributeInfo : ISerializable
    {
        // todo: thishas to be added as a type
        // just cast it to a byte in the serializer
        public char SlotId { get; set; } = (char)0xFF;
        public char Type { get; set; } = (char)0xFF;
        public char State { get; set; } = (char)0x00;

        // todo: add as type in serializer also
        public float Value { get; set; } = 0.0f;

        public void Serialize(Packet packet, int i, params object[] optional)
        {
            packet.Put(
                SlotId,
                Type,
                State,
                Value
            );
        }
    }
}
