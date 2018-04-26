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
        public static void writeCreateSecondItems(Packet p)
        {
            /*p.Put(
                6, // #items

                380300, // // Item ID 00 05 CD 8C
                0, // Count
                10000001, // Item UID
                -1, // m_nDuration
                -1, // m_nInitDuration
                (byte)0, // m_cEnchantLevel,
                (byte)2, // m_cGradeID
                0, // m_nEquipLevel
                -1, // m_nPeriod
                0, // m_tStartDate
                0, // m_tRegDate
                0 // m_tEndDate
            );*/

            p.Put(6); // items

            p.WriteHexString("00 05 CD 8C 00 00 00 00 01 FD 42 71 FF FF FF FF FF FF FF FF 00 02 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 02 00 00 00 00 01 02 00 00 00 03 00 0A 01 3E 0F 5C 29 01 03 01 41 10 00 00 02 00 01 40 A0 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");

            p.WriteHexString("00 05 CD 96 00 00 00 00 01 FD 42 72 FF FF FF FF FF FF FF FF 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 02 00 00 00 02 00 03 01 41 10 00 00 01 07 01 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("00 05 CD A0 00 00 00 00 01 FD 42 74 FF FF FF FF FF FF FF FF 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 02 00 00 00 02 00 04 01 41 D8 00 00 01 03 01 41 10 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("00 05 CD AA 00 00 00 00 01 FD 42 75 FF FF FF FF FF FF FF FF 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 02 00 00 00 02 00 02 01 41 00 00 00 01 03 01 41 10 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");

            p.WriteHexString("00 05 CD B4 00 00 00 00 01 FD 42 76 FF FF FF FF FF FF FF FF 00 02 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 02 00 00 00 00 01 02 00 00 00 03 00 07 01 3F 00 00 00 01 01 01 41 00 00 00 02 09 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00");
            p.WriteHexString("00 06 53 1A 00 00 00 00 01 FD 42 77 FF FF FF FF FF FF FF FF 00 02 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 02 00 00 00 00 01 02 00 00 00 03 00 09 01 00 00 00 00 01 01 01 41 00 00 00 02 02 01 41 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02");
        }

        public static void writeSiegTestItems(Packet p)
        {
            
        }

        public static void writeSiegTestEquipItems(Packet p)
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
