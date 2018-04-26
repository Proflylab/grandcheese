using GrandCheese.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.User
{
    class Shop
    {
        [Opcode(GameOpcodes.EVENT_ITEM_BUY_CHECK_REQ)]
        public static void BuyVpItem(KUser user, Packet packet)
        {
            // 02 A4 00 00 00 04 00 00 01 8E 8E 00 00 00 00 01 02 03 04 05 06 07 08 08
            var itemId = packet.ReadInt();

            Packet p = new Packet(GameOpcodes.EVENT_ITEM_BUY_CHECK_ACK);
            
            p.Put(
                1, // probably count
                itemId //36353 // Item ID? 00 01 8E 8E
            );
            user.userClient.Client.SendPacket(p);
        }
    }
}
