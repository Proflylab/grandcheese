using GrandCheese.Util;
using GrandCheese.Util.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.Inventory
{
    public class KEquipItemInfo : ISerializable
    {
        // for now, let's just hardcode things
        // public KItem item { get; set; }

        public long ItemUniqueId { get; set; }

        public int ItemId { get; set; }

        public byte EnchantLevel { get; set; } = 0x00;

        public byte EnchantEquipGradeId { get; set; } = 0x00;

        public byte GradeId { get; set; } = 0x00;

        public int EquipLevel { get; set; } = 0x00;

        public int DesignCoordiId { get; set; } = 0; // Does this go after the vectors?

        public List<KSocketInfo> Sockets { get; set; } = new List<KSocketInfo>();

        public List<KAttributeInfo> Attributes { get; set; } = new List<KAttributeInfo>();

        public void Serialize(Packet packet, int i, params object[] optional)
        {
            packet.Put(
                ItemId,
                ItemUniqueId,
                EnchantLevel, //byte
                GradeId, // byte
                EquipLevel, // int
                EnchantEquipGradeId, // byte
                DesignCoordiId, // int
                Sockets,
                Attributes
            );
        }
    }
}
