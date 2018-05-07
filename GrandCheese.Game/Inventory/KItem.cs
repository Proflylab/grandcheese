using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GrandCheese.Data;
using GrandCheese.Game.User;
using GrandCheese.Util;
using GrandCheese.Util.Interfaces;

namespace GrandCheese.Game.Inventory
{
    public class KItem : ISerializable
    {
        // ItemUID / DB item ID
        public long Id { get; set; }

        // Item ID (for the game)
        public int ItemId { get; set; }

        // User ID
        public int UserId { get; set; } = -1;

        // Character ID
        public int CharacterId { get; set; } = -1;

        // todo: type?
        public int Count { get; set; } = -1;

        // todo: type?
        public int InitCount { get; set; } = -1;

        public byte EnchantLevel { get; set; } = 0x00;

        public byte GradeId { get; set; } = 0x00;

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
                Id,
                Count,
                InitCount,
                (int)EnchantLevel,
                (int)GradeId,
                EquipLevel,
                // EquipLevelDown,
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

        public void Insert(IDbConnection db, bool Equipped)
        {
            var query = "INSERT INTO items (item_id, user_id, character_id, count, init_count, enchant_level, grade_id, equip_level, period, start_date, reg_date, end_date, equip_state) " +
                                    "VALUES(@ItemId, @UserId, @CharacterId, @Count, @InitCount, @EnchantLevel, @GradeId, @EquipLevel, @Period, @StartDate, @RegDate, @EndDate, @Equipped) " +
                                    "RETURNING id;";

            var id = db.ExecuteScalar(query, new
            {
                ItemId,
                UserId,
                CharacterId,
                Count,
                InitCount,
                EnchantLevel,
                GradeId,
                EquipLevel,
                Period,
                StartDate,
                RegDate,
                EndDate,
                Equipped
            });

            Id = Convert.ToInt64(id);
        }
    }
}
