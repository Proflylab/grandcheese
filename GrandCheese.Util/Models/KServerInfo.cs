using GrandCheese.Util;
using GrandCheese.Util.Interfaces;
using GrandCheese.Util.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Data
{
    public class KServerInfo : ISerializable
    {
        public int Id { get; set; }

        public GCWideString Name { get; set; }

        public string IP { get; set; }

        public short Port { get; set; }

        //[Column("online_users")]
        public int OnlineUsers { get; set; }
        
        public int MaxUsers { get; set; }

        public int ProtocolVersion { get; set; }

        public GCWideString Description { get; set; }

        public bool Active { get; set; }

        public int MaxLevel { get; set; } = 0;

        public void Serialize(Packet packet, int i, object kUser = null)
        {
            // KServerInfo uses 1 as an index
            // kinda gay

            packet.Put(
                i + 1,
                i + 1,
                Name,
                IP,
                Port,
                OnlineUsers,
                MaxUsers,
                ProtocolVersion,
                new GCPair(-1, -1), // RangeMinMaxLevel
                IP,
                Description,
                MaxLevel
            );
        }
    }
}
