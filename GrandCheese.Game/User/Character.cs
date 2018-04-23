using GrandCheese.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.User
{
    class Character
    {
        [Opcode(GameOpcodes.EVENT_NEW_CHAR_CHOICE_REQ)]
        public static void CreateNewCharacter(KUser user)
        {
            var p = new Packet(GameOpcodes.EVENT_NEW_CHAR_CHOICE_ACK);

            throw new NotImplementedException("TODO :///");

            user.userClient.Client.SendPacket(p);
        }
    }
}
