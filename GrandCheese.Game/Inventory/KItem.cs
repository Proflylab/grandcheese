using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandCheese.Util;
using GrandCheese.Util.Interfaces;

namespace GrandCheese.Game.Inventory
{
    class KItem : ISerializable
    {
        // ItemUID / DB item ID
        public int ItemUniqueId { get; set; }

        public int Count { get; set; } = 0;

        // Item ID (for the game)
        public int ItemId { get; set; }

        // todo: type?
        public int Duration { get; set; } = -1;

        // todo: type?
        public int InitDuration { get; set; } = -1;

        public char EnchantLevel { get; set; } = (char)0x00;

        public char GradeId { get; set; } = (char)0x00;

        public int EquipLevel { get; set; } = 0;

        // todo: type?
        public int Period { get; set; } = -1;

        // todo: type?
        public int StartDate { get; set; } = 0;

        // todo: type?
        public int RegDate { get; set; } = 0;

        // todo: type?
        public int EndDate { get; set; } = 0;

        public List<KSocketInfo> Sockets { get; set; } = new List<KSocketInfo>();

        public List<KAttributeInfo> Attributes { get; set; } = new List<KAttributeInfo>();

        public void Serialize(Packet packet, int i)
        {
            packet.Put(
                ItemId,
                Count,
                ItemUniqueId,
                Duration,
                InitDuration,
                EnchantLevel,
                GradeId,
                EquipLevel,
                Period,
                StartDate,
                RegDate,
                EndDate,
                
                // let the serializer deal with the lists
                Sockets,
                Attributes
            );

            packet.WriteHexString("00 00 00 00 00 00 00 00 00 00");
            packet.WriteHexString("FF FF FF FF");
            packet.WriteHexString("01 A1");
            packet.WriteHexString("AB 5D 08 D7"); // Possibly user ID? [Character ID?]

            // idfk what this is but i'm not going to question it.
            packet.WriteHexString("50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
        }
    }
}
