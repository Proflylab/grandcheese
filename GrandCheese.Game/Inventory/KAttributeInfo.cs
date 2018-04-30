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
        // todo: this has to be added as a type
        // just cast it to a byte in the serializer
        public byte SlotId { get; set; } = 0xFF;
        public byte Type { get; set; } = 0xFF;
        public byte State { get; set; } = 0x00;

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
