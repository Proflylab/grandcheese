using GrandCheese.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.User
{
    class DungeonUserInfo
    {
        public static void write_DungeonUserInfoPacket_sub(Packet p,
                                                         int gamemode,
                                                         bool isHaveDiff,
                                                         int Diff,
                                                         bool isClear,
                                                         int lastDiff,
                                                         int canDiff,
                                                         int LeftRewardCnt)
        {
            // 00 00 00 07 / 00 00 00 01 / 01 / 01 / 00 00 / 00 00 / 00 00
            p.WriteInt(gamemode);
            if (true == isHaveDiff)
            {
                p.WriteInt(1);
                p.Write((byte)Diff);
            }
            else
            {
                p.WriteInt(0);
            }
            p.WriteBool(isClear);
            p.WriteShort((short)lastDiff);
            p.WriteShort((short)canDiff);
            p.WriteShort((short)LeftRewardCnt);
            p.WriteInt(0); // std::map< int, KUsersDungeonRecordData > m_mapBestRecord
        }

        // 하드코딩
        public static void read_DungeonUserInfoPacketForDummy(Packet p)
        {
            int size = p.ReadInt();

            for (int i = 0; i < size; i++)
            {
                int gamemode = p.ReadInt();
                int isHaveDiff = p.ReadInt();

                if (isHaveDiff == 1)
                {
                    p.ReadByte(); // diff
                }

                bool isClear = p.ReadBool();
                short lastDiff = p.ReadShort();
                short canDiff = p.ReadShort();
                short LeftRewardCnt = p.ReadShort();
            }
        }

        // 하드코딩..
        public static void WriteMapDifficulty(Packet p)
        {
            p.WriteInt(85); // vector size
            write_DungeonUserInfoPacket_sub(p, 0x07, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x08, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x09, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x0A, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x0B, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x0C, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x0D, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x0E, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x0F, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x10, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x11, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x12, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x13, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x14, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x15, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x16, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x17, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x18, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x19, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x1A, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x1B, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x1D, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x1E, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x24, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x27, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x28, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x29, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x2A, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x2B, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x2C, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x2D, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x2E, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x2F, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x30, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x31, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x32, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x33, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x34, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x35, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x36, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x37, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x38, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x39, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x3A, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x3B, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x3C, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x3D, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x3E, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x3F, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x40, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x43, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x44, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x45, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x46, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x47, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x48, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x49, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x4A, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x4B, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x4C, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x4E, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x4F, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x50, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x51, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x52, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x53, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x54, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x55, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x56, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x57, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x58, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x59, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x5A, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x5B, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x5C, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x5D, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x5E, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x5F, true, 1, true, 0, 0, 0);

            write_DungeonUserInfoPacket_sub(p, 0x62, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x63, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x64, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x65, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x66, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x67, true, 1, true, 0, 0, 0);
            write_DungeonUserInfoPacket_sub(p, 0x6A, true, 1, true, 0, 0, 0);
        }
    }
}
