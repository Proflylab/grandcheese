using GrandCheese.Util;
using GrandCheese.Util.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using GrandCheese.Game.Guild;

namespace GrandCheese.Game.User
{
    public class KUser
    {
        public int userId = -1;
        public string username = null;
        public string password = null;
        public string userIP = null;
        public int authLevel = 0;
        public string nick = null;
        public int gp = 0;
        public int vp = 0;
        public int inventoryCapacity = 0;
        public int bonusPoints = 0;
        public int specialBonusPoints = 0;
        public int attendTime = 0;
        public int attendPoint = 0;

        public UserClient userClient = null;

        public KUser(UserClient userClient)
        {
            this.userClient = userClient;
        }

        [Opcode((short)GameOpcodes.EVENT_VERIFY_ACCOUNT_REQ)]
        public void VerifyAccount(Client client, Packet packet)
        {
            username = packet.ReadString();
            password = packet.ReadString();
            userIP = packet.ReadUnicodeString();

            byte sex = packet.ReadByte();

            int ProtocolVersion = packet.ReadInt();
            int P2PVersion = packet.ReadInt();
            int MainChecksum = packet.ReadInt();
            int ConnectType = packet.ReadInt(); // First connection / Move server
            int Age = packet.ReadInt();
            int AuthType = packet.ReadInt();
            int AuthTick = packet.ReadInt();
            byte ExpAccount = packet.ReadByte(); // 체험계정 (?)
            string CountryCode = packet.ReadUnicodeString();
            int FunBoxBonus = packet.ReadInt();
            int m_nLinBonus = packet.ReadInt();
            int m_dwChannellingType = packet.ReadInt();
            int m_nUniqueKey = packet.ReadInt();
            long m_biUniqueKey = packet.ReadLong();
            int m_nLanguageCode = packet.ReadInt();

            using (var db = Database.Get())
            {
                var user = db.Query<GrandCheese.Util.Models.User>("SELECT * FROM users WHERE username = @name",
                    new { name = username }).FirstOrDefault();

                if (user != null)
                {
                    if (user.Password != password)
                    {
                        client.SendPacket(GetLoginFailPacket(username), true);
                        return;
                    }

                    // OK, checks passed
                    userId = user.Id;
                    nick = user.Nickname;
                    authLevel = user.AuthLevel;
                    gp = user.GP;
                    vp = user.VP;
                    inventoryCapacity = user.InventoryCapacity;
                    bonusPoints = user.BonusPoints;
                    specialBonusPoints = user.SpecialBonusPoints;
                }
                else
                {
                    client.SendPacket(GetLoginFailPacket(username), true);
                    return;
                }
            }

            Packet pLogin = new Packet((short)GameOpcodes.EVENT_VERIFY_ACCOUNT_ACK);
            pLogin.WriteUnicodeString(username, true);
            if (nick == null)
            {
                pLogin.WriteInt(0); // no nickname
            } else
            {
                pLogin.WriteUnicodeString(nick, true);
            }
            pLogin.WriteInt(0); // ucOK
            pLogin.WriteString(password, true);
            pLogin.WriteHexString("00 2E 00 31 00 36 00 31 00 00");
            pLogin.WriteIntLittle(BitConverter.ToInt32((client.Sock.RemoteEndPoint as IPEndPoint).Address.GetAddressBytes(), 0));
            GuildUserInfo.write_NoGuildUserInfoPacket(pLogin);
            pLogin.Write((byte)authLevel); // AuthLevel
            pLogin.WriteInt(20); // age
            pLogin.Write(0); // 개인정보 동의 체크
            pLogin.Write(0); // PC방

            // TODO: Characters
            //https://github.com/lovemomory/GrandChaseSeasonV/blob/master/GrandChaseSeasonV/src/game/user/GameUser.java#L189
            pLogin.WriteInt(0);

            pLogin.WriteShort(9401); // 포트긴 한데 udp겠지..? 9401 in Madness
            pLogin.WriteInt(userId);
            pLogin.WriteUnicodeString(Data.Data.Server.Name, true);
            pLogin.WriteInt(3); // New user, initial connection, reconnection? 03 in Madness

            // TODO: Server message...?
            // https://github.com/lovemomory/GrandChaseSeasonV/blob/master/GrandChaseSeasonV/src/game/user/GameUser.java#L201
            pLogin.WriteInt(0);

            DungeonUserInfo.write_mapDifficulty(pLogin);

            pLogin.WriteHexString("00 18 00 00 00 00 00 01 00 12 9D FA 00 00 00 01 00 98 98 0F 00 00 00 00 59 6E 2F DB 59 6C DE 5B 00 00 00 00 00 00 00 00 29 E1 56 52 22 00 74 17");

            // 메시지서버
            pLogin.WriteInt(0); // Num msg servers
            /*
            pLogin.WriteInt(0); // server uid
            pLogin.WriteInt(0); // server part
            pLogin.WriteUnicodeString("MSG1", true);
            pLogin.WriteString("127.0.0.1", true);
            pLogin.WriteShort(4444);
            pLogin.WriteInt(0); // user num
            pLogin.WriteInt(0); // max user num
            pLogin.WriteInt(0); // 프로토콜
            pLogin.WriteInt(-1); // pair-left 레벨범위
            pLogin.WriteInt(-1); // pair-right 레벨범위
            pLogin.WriteString(Data.Data.Server.IP, true); // 전달용
            pLogin.WriteUnicodeString(Data.Data.Server.Description, true); // 서버 설명
            pLogin.WriteInt(0); // max level
            */

            pLogin.Write(3); // m_cRecommendUser
            pLogin.WriteInt(0x57F173AC); // m_tFirstLoginTime
            pLogin.WriteInt(0x57F173AC); // m_tLastLoginTime
            pLogin.WriteInt(0); // m_nPubEvent

            // Pets (Unimplemented)
            pLogin.WriteInt(0); // Size
            /*
            for( int i=0; i < Pets.size(); i++ ) {
                pLogin.WriteLong( Pets.get(i).m_dwUID ); // 원래 맵이라 인덱스를 보낸다
                Pets.get(i).write_PetInfoPacket(pLogin);
            }
            */

            pLogin.WriteInt(0); // vector<integer> m_vecExpiredMission
            pLogin.Write(1); // m_bEnableNewTermEvent
            pLogin.WriteInt(0); // m_kPremiumInfo
            pLogin.Write(0); // m_bIsRecommendEvent
            pLogin.Write(1); // m_bCheckChanneling
            pLogin.WriteInt(1); // m_dwChannelType
            pLogin.WriteInt(0x61D0B2C0); // m_tVirtualEnableDate
            pLogin.Write(0); // m_cUserBenfitType
            pLogin.WriteHexString("30 FF E9 7D 53 0B A0 0A 00 5A 0D 2A 00 00 00 00 00 74 39 5F 5A 0D 2A 3A");
            pLogin.WriteInt(0); // ?
            pLogin.WriteInt(20); // Character count
            for (int i = 0; i < 20; i++)
            {
                pLogin.WriteInt(i);
                pLogin.WriteInt(i); // Send twice, one index of map; other character ID
                pLogin.WriteInt(0);
                pLogin.WriteInt(0);
                pLogin.WriteShort((short)0);
            }
            pLogin.WriteHexString("00 00 00 07 00 00 00 68 00 16 5D 64 00 00 00 00 00 00 00 69 00 03 A1 EC 00 00 00 00 00 00 01 36 00 16 05 4E 00 00 00 01 00 00 01 37 00 12 C8 FC 00 00 00 01 00 00 01 38 00 13 AF A6 00 00 00 01 00 00 01 39 00 02 DF 6E 00 00 00 01 00 00 01 3A 00 08 5A D4 00 00 00 01 00 00 00 00 11 00 00");
            
            client.SendPacket(pLogin, true);
            
            Game.Send_ServerTime(this);
            Game.Send_PetVestedItem(this);
            Game.Send_GraduateCharInfo(this);
            Game.Send_JumpingCharInfo(this);
            Game.Send_SlotInfo(this);
            Game.Send_GuideCompleteInfo(this);
            Game.Send_FullLookInfo(this);
        }

        public Packet GetLoginFailPacket(string id)
        {
            Packet pLoginfail = new Packet((short)LoginOpcodes.ENU_VERIFY_ACCOUNT_ACK);
            pLoginfail.WriteInt(20); // login failure
            pLoginfail.WriteString(id, true);
            pLoginfail.WriteInt(0);
            return pLoginfail;
        }
    }
}
