using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util
{
    public class Client
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public Socket Sock { get; set; }
        public Crypto Crypto { get; set; }
        
        public object User { get; set; }

        /*
         * 0: Center
         * 1: Game
         * 2: Message
         */
        public int Type { get; set; } = 0;

        public DateTime LastHeartbeat { get; set; }
        public int Number { get; set; } = 1;
        public byte[] Prefix { get; set; } = new byte[2];

        public void SendPacket(Packet p, bool compress = false)
        {
            Crypto.SendPacket(p, compress);
        }
    }
}
