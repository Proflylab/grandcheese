using GrandCheese.Game.Inventory;
using GrandCheese.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.User
{
    public class Character
    {
        public int Id { get; set; }

        public static void WriteEnabledCharacters(Packet p)
        {
            p.WriteInt(20);
            for (int i = 0; i < 20; i++)
            {
                p.WriteInt(i);
                p.WriteInt(i);
                p.WriteInt(0);
                p.WriteInt(0);
                p.WriteShort(0);
            }
        }

        [Opcode(GameOpcodes.EVENT_NEW_CHAR_CHOICE_REQ)]
        public static void CreateNewCharacter(KUser user, Packet packet)
        {
            var characterType = (int)packet.ReadByte();
            Console.WriteLine("Character ID: {0}", characterType);

            var p = new Packet(GameOpcodes.EVENT_NEW_CHAR_CHOICE_ACK, user);

            p.Write(0x00); // m_ucOK
            p.Put(characterType);
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 01");

            Inventory.Inventory.WriteSiegTestEquipItems(p, characterType);

            p.WriteHexString("00 00 00 02 00 00 00 A0 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 00 00 00 00 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            p.WriteInt(7); // ??????
            
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 07 D0 00 00 07 D0 00 00 00 0A 00 00 00 00 00 00 00 5A 00 00 00 64 00 00 00 00 00 00 00 00 FF EA 7D A8 00 00 00 55");
            
            DungeonUserInfo.WriteMapDifficulty(p); // lol

            Inventory.Inventory.WriteCreateSecondItems(p, characterType);

            p.WriteHexString("00 00 00 00 00 00 00 02 00 00 00 14 00 00 00");

            WriteEnabledCharacters(p);

            //p.WriteHexString("00 00 00 03 00 00 00 01 00 00 E5 6A 00 00 00 00 02 06 CC BA 00 00 00 14 00 00 00 14 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 77 50 D0 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 E5 6A 00 00 00 00 02 06 CC BA 06 00 00 00 00 00 00 00");
            p.WriteHexString("00 00 00 03 00 00 00 01 00 00 E5 6A 00 00 00 00 02 06 CC BA 00 00 00 14 00 00 00 14 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 E5 6A 00 00 00 00 02 06 CC BA 06 00 00 00 00 00 00 00");

            Log.Get().Trace(Util.Util.ConvertBytesToHexString(p.packet.ToArray()));

            user.userClient.Client.SendPacket(p);
        }
    }
}
