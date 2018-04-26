using GrandCheese.Game.Inventory;
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
        public static void CreateNewCharacter(KUser user, Packet packet)
        {
            var characterType = packet.ReadByte();
            Console.WriteLine("Character ID: {0}", characterType);

            var p = new Packet(GameOpcodes.EVENT_NEW_CHAR_CHOICE_ACK);

            p.Write(0x00); // m_ucOK
            p.Put((int)characterType);
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 01");

            KItem.writeSiegTestEquipItems(p);

            // End items

            p.WriteHexString("00 00 00 04 00 00 00 A0 00 00 00 02 60 00 00 00 00 00 00 00 00 00 00 00 06 40 00 00 00 00 00 00 06 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 12 C0 00 00 12 C0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            p.WriteInt(1); // Character slot position

            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 07 D0 00 00 07 D0 00 00 00 0A 00 00 00 00 00 00 00 5A 00 00 00 64 00 00 00 00 00 00 00 00");

            p.WriteHexString("FF EA 7D A8"); // User ID? not sure

            p.WriteHexString("00 00 00 55");

            DungeonUserInfo.write_mapDifficulty(p); // lol

            KItem.writeCreateSecondItems(p);

            p.WriteInt(20);
            for (int i = 0; i < 20; i++)
            {
                p.WriteInt(i);
                p.WriteInt(i);
                p.WriteInt(0);
                p.WriteInt(0);
                p.WriteShort(0);
            }

            p.WriteHexString("00 00 00 01 00 00 00 01 00 00 E5 6A 00 00 00 00 01 FD 42 78 00 00 00 14 00 00 00 14 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 E5 6A 00 00 00 00 01 FD 42 78 08 00 00 00 00 00 00");

            user.userClient.Client.SendPacket(p);
        }
    }
}
