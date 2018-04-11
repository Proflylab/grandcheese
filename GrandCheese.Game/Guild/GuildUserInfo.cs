using GrandCheese.Game.Data;
using GrandCheese.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.Guild
{
    public class GuildUserInfo
    {
        public static void write_NoGuildUserInfoPacket(Packet p)
        {
            p.WriteInt(0); // UserUID
            p.WriteInt(0); // Login Length
                           //p.WriteUnicodeString(""); // Login
            p.WriteInt(0); // Nick Length
                           //p.WriteUnicodeString(""); // Nick
            p.WriteInt(0); // GuildUID
            p.Write(0xFF); // 길드원 등급, -1 originally
            p.Write(0); // 캐릭터 등급
            p.WriteInt(0); // 기여도
            p.WriteInt(0); // 승리회수
            p.WriteInt(0); // 패배회수
            p.WriteInt(0); // 자기소개 길이
                           //p.WriteUnicodeString(""); // 자기소개
            KSimpleDate.write_KSimpleDate(p, 0, 0, 0, 0); // 가입날짜
            p.Write(0); // 현재 접속
            p.Write(0); // 서버UID
            p.WriteInt(0); // 현재위치 길이
                           //p.WriteUnicodeString(""); // 현재위치
            KSimpleDate.write_KSimpleDate(p, 0, 0, 0, 0); // 마지막접속일
            p.Write(0); // 길드채널 등급
        }
    }
}
