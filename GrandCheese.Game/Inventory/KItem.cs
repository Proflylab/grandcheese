using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandCheese.Data;
using GrandCheese.Game.User;
using GrandCheese.Util;
using GrandCheese.Util.Interfaces;

namespace GrandCheese.Game.Inventory
{
    class KItem : ISerializable
    {
        // ItemUID / DB item ID
        public long ItemUniqueId { get; set; }

        // Item ID (for the game)
        public uint ItemId { get; set; }

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

        public void Serialize(Packet packet, int i, params object[] optional)
        {
            Trace.Assert(optional.Length > 0, "Missing parameter", "The person who wrote the function that called KItem.Serialize is an idiot and didn't pass in KUser.");
            Trace.Assert(optional[0].GetType() == typeof(KUser), "Wrong parameter", "THE FIRST PARAMETER IS SUPPOSED TO BE A KUser, NOOB!!!!!!");

            var kUser = (KUser)optional[0];

            packet.Put(
                ItemId,
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

            // ??????
            packet.WriteHexString("00 00 00 00 00 00 00 00 00 00");
            packet.WriteHexString("FF FF FF FF");
            packet.WriteHexString("01 A1");

            packet.WriteHexString("AB 5D 08 D7"); // Possibly user ID? [Character ID?]
            //packet.Put(kUser.currentCharacter.Id); // ?

            // idfk what this is but i'm not going to question it.
            packet.WriteHexString("50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
        }
    }
}
