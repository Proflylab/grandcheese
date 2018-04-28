using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandCheese.Util;

namespace GrandCheese.Game.Inventory
{
    public class KItem
    {
        public static void WriteCreateSecondItems(Packet p)
        {
            p.Put(6); // items

            p.Put(
                380300, // ID
                0, // Count
                10000001 // Item UID,
                -1, // m_nDuration
                -1, // m_nInitDuration
                (byte)0x00, // m_cEnchantLevel
                (byte)0x02, // m_cGradeID
                0, // m_nEquipLevel
                -1, // m_nPeriod
                0, // m_tStartDate
                0, // m_tRegDate
                0 // m_tEndDate
            );
            /*
            p.WriteHexString("00 00 00 00"); // Count
            p.WriteHexString("01 FD 42 71"); // Item UID
            p.WriteHexString("FF FF FF FF");
            p.WriteHexString("FF FF FF FF");
            p.WriteHexString("00");
            p.WriteHexString("02");
            p.WriteHexString("00 00 00 00");
            p.WriteHexString("FF FF FF FF");
            p.WriteHexString("00 00 00 00");
            p.WriteHexString("00 00 00 00");
            p.WriteHexString("00 00 00 00");
            */

            // vector<KSocketInfo>
            p.Put(2); // Count

            p.WriteHexString("00 00 00 00"); // Card ID
            p.WriteHexString("00"); // Slot index
            p.WriteHexString("02"); // Type: Not assigned

            p.WriteHexString("00 00 00 00"); // Card ID
            p.WriteHexString("01"); // Slot index
            p.WriteHexString("02"); // Type: Not assigned

            // vector<KItemAttribute>...?
            p.Put(3);
            p.WriteHexString("00");
            p.WriteHexString("0A");
            p.WriteHexString("01");
            p.WriteHexString("3E 0F 5C 29"); // 0.14f

            p.WriteHexString("01");
            p.WriteHexString("03");
            p.WriteHexString("01");
            p.WriteHexString("41 10 00 00"); // 9.0f

            p.WriteHexString("02");
            p.WriteHexString("00");
            p.WriteHexString("01");
            p.WriteHexString("40 A0 00 00"); // 5.0f

            p.WriteHexString("00 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("FF FF FF FF");
            p.WriteHexString("01 A1");
            p.WriteHexString("AB 5D 08 D7"); // Possibly user ID? [Character ID?]
            p.WriteHexString("50 D0 00 00 08 00 00 00 00 00 00 00 00 00");

            p.WriteHexString("00 05 CD 96 00 00 00 00 01 FD 42 72 FF FF FF FF FF FF FF FF 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 02 00 00 00 02 00 03 01 41 10 00 00 01 07 01 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("00 05 CD A0 00 00 00 00 01 FD 42 74 FF FF FF FF FF FF FF FF 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 02 00 00 00 02 00 04 01 41 D8 00 00 01 03 01 41 10 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("00 05 CD AA 00 00 00 00 01 FD 42 75 FF FF FF FF FF FF FF FF 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 02 00 00 00 02 00 02 01 41 00 00 00 01 03 01 41 10 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");

            p.WriteHexString("00 05 CD B4 00 00 00 00 01 FD 42 76 FF FF FF FF FF FF FF FF 00 02 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 02 00 00 00 00 01 02 00 00 00 03 00 07 01 3F 00 00 00 01 01 01 41 00 00 00 02 09 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("00 06 53 1A 00 00 00 00 01 FD 42 77 FF FF FF FF FF FF FF FF 00 02 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 02 00 00 00 00 01 02 00 00 00 03 00 07 01 00 00 00 00 01 01 01 41 00 00 00 02 09 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
        }

        public static void WriteSiegTestItems_(Packet p)
        {
            
        }

        public static void WriteSiegTestEquipItems(Packet p)
        {
            p.Put(
                6,

                380300, // Item ID, OK as-is (00 05 CD 8C)
                0, // Count
                10000001 // Item UID
            );
            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            p.Put(
                380310,
                0,
                10000002
            );

            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            p.Put(
                380320,
                0,
                10000003
            );

            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            p.Put(
                380330,
                0,
                10000004
            );

            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            p.Put(
                380340,
                0,
                10000005
            );

            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            p.Put(
                414490,
                0,
                10000006
            );


            p.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
        }
    }
}
