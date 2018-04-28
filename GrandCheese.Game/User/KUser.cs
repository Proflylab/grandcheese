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
using GrandCheese.Util.Extensions;
using GrandCheese.Game.Inventory;
using System.Diagnostics;

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
        public List<Inventory.Inventory> items = new List<Inventory.Inventory>();

        public UserClient userClient = null;

        public KUser(UserClient userClient)
        {
            this.userClient = userClient;
        }

        [Opcode(GameOpcodes.HEART_BIT_NOT)]
        public void OnHeartbeat(Client client, Packet p)
        {
            client.LastHeartbeat = DateTime.Now;
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

            Game.Send_ExpTable(this); // 04E1 (1249d)

            Packet pLogin = new Packet((short)GameOpcodes.EVENT_VERIFY_ACCOUNT_ACK);

            pLogin.Put(
                username.ToWideString(),
                nick?.ToWideString(),
                0, // ucOK,
                password
            );
            
            // test
            //pLogin.WriteHexString("00 2E 00 31 00 36 00 31 00 00");
            pLogin.WriteHexString("00 30 00 2E 00 2E 00 34 00 00");

            pLogin.WriteIntLittle(BitConverter.ToInt32((client.Sock.RemoteEndPoint as IPEndPoint).Address.GetAddressBytes(), 0));
            
            GuildUserInfo.write_NoGuildUserInfoPacket(pLogin);
            //pLogin.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            // test
            // OK! matches madness kinda
            pLogin.Write((byte)authLevel); // AuthLevel - 0xFA in test packet (modified)
            pLogin.WriteInt(20); // age, 0x14
            pLogin.Write(0); // 개인정보 동의 체크
            pLogin.Write(0); // PC방

            // TODO: Characters
            //https://github.com/lovemomory/GrandChaseSeasonV/blob/master/GrandChaseSeasonV/src/game/user/GameUser.java#L189
            pLogin.WriteInt(0);

            /*
            pLogin.WriteInt(1); // OK

            pLogin.Write(19); // Index of character
            pLogin.Write(19); // m_cCharType
            pLogin.WriteUnicodeString("", true); // m_strCharName
            pLogin.Write(0); // m_cPromotion
            pLogin.Write(0); // m_cCurrentPromotion
            pLogin.WriteLong(0); // m_biInitExp
            pLogin.WriteInt(0); // m_iInitWin
            pLogin.WriteInt(0); // m_iInitLose
            pLogin.WriteInt(0); // m_iWin
            pLogin.WriteInt(0); // m_iLose
            pLogin.WriteLong(0); // m_biExp
            pLogin.WriteInt(1); // m_dwLevel

            pLogin.WriteInt(0); // m_vecEquipItems.size()

            pLogin.WriteInt(1); // SkillPoint
            pLogin.WriteInt(0); // MaxSkillPoint

            pLogin.WriteInt(1); // SkillTreePoint
            pLogin.WriteInt(0); // MaxSkillTreePoint

            pLogin.Write(0); // 오류나면 바이트 말고 인트로..

            pLogin.WriteLong(100); // m_biInitTotalExp
            pLogin.WriteLong(100); // m_biTotalExp

            // EquipItemInfo
            pLogin.WriteInt(0);

            // TODO: What are these?
            /*
            pLogin.WriteInt(item.Id);
            pLogin.WriteInt(1); // Count?
            pLogin.WriteInt(item.ItemId);
            //pLogin.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            pLogin.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            pLogin.WriteInt(0);
            pLogin.WriteInt(0);
            pLogin.WriteInt(0);
            pLogin.WriteInt(0);

            pLogin.WriteShort(0);

            pLogin.Write(0);
            */

            /*
            pLogin.WriteBool(false); // m_bChangeWeaponLock

            // Promotion (Vector)
            pLogin.WriteInt(0); // Size
            //pLogin.Write(1); // Promotion

            // ELOUserData
            pLogin.WriteInt(0); // m_nInitELOWin
            pLogin.WriteInt(0); // m_nELOWin
            pLogin.WriteInt(0); // m_nInitELOLose
            pLogin.WriteInt(0); // m_nELOLose
            pLogin.WriteInt(1660); // m_nRatingPoint
            pLogin.WriteInt(1660); // m_nInitRatingPoint
            pLogin.WriteInt(0); // m_nELOType
            pLogin.WriteInt(0); // m_nInitMatchTotalCount
            pLogin.WriteInt(0); // m_nMatchTotalCount
            pLogin.WriteInt(0); // m_nLastWinLose
            pLogin.WriteInt(0); // m_nConstantK
            pLogin.Write(0); // m_ucGrade
            pLogin.WriteInt(0); // m_nPlacementTestPlayCount
            
            // New in Season 5:
            pLogin.WriteInt(0); // Character slot position
            pLogin.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 07 D0 00 00 07 D0 00 00 00 0A 00 00 00 00 00 00 00 5A 00 00 00 64 00 00 00 00 00 00 00 00 00 71 30 29");
            */
            /////////////////////

            pLogin.WriteShort(9401); // 포트긴 한데 udp겠지..? 9401 in Madness
            pLogin.WriteInt(userId);
            pLogin.WriteUnicodeString(ServerMain.Info.Name, true);

            // int 3 在lovemomory的代碼
            //pLogin.WriteInt(3); // New user, initial connection, reconnection? 03 in Madness
            // int 0 在madness中
            pLogin.WriteInt(0);


            // TODO: Server message...?
            // https://github.com/lovemomory/GrandChaseSeasonV/blob/master/GrandChaseSeasonV/src/game/user/GameUser.java#L201
            pLogin.WriteInt(0); // If not 0, the client crashes

            DungeonUserInfo.WriteMapDifficulty(pLogin);
            
            // ?
            //pLogin.WriteHexString("00 18 00 00 00 00 00 01 00 12 9D FA 00 00 00 01 00 98 98 0F 00 00 00 00 59 6E 2F DB 59 6C DE 5B 00 00 00 00 00 00 00 00 29 E1 56 52 22 00 74 17");
            pLogin.WriteHexString("40 18 00 00 00 00 00 00 00 00 00 00 29 E1 E0 76 74 08 74 80");

            // MsgServer
            pLogin.WriteInt(1); // Num msg servers
            
            pLogin.WriteInt(0); // server uid
            pLogin.WriteInt(0); // server part
            pLogin.WriteUnicodeString("MsgServer_GS1", true);
            pLogin.WriteString("127.0.0.1", true);
            pLogin.WriteShort(4444);
            pLogin.WriteInt(0); // user num
            pLogin.WriteInt(0); // max user num
            pLogin.WriteInt(0); // 프로토콜
            pLogin.WriteInt(-1); // pair-left 레벨범위
            pLogin.WriteInt(-1); // pair-right 레벨범위
            pLogin.WriteString(ServerMain.Info.IP, true); // 전달용
            
            // modified for test:
            //pLogin.WriteUnicodeString("", true); // 서버 설명
            pLogin.WriteInt(0); // No description

            pLogin.WriteInt(0); // max level
            
            pLogin.Write(3); // m_cRecommendUser, 0x3
            
            //pLogin.WriteInt(0x57F173AC); // m_tFirstLoginTime
            //pLogin.WriteInt(0x57F173AC); // m_tLastLoginTime
            pLogin.WriteHexString("57 F1 73 AC"); // m_tFirstLoginTime
            pLogin.WriteHexString("57 F1 73 AC");

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

            // modified for test:
            // pLogin.WriteInt(1); // m_dwChannelType
            // TODO: why 1 in lovemomory's source, but 0 in madness
            pLogin.WriteInt(0);

            // test: maybe WriteInt would reverse 0xC0B2D061......
            //pLogin.WriteInt(0x61D0B2C0);
            pLogin.WriteHexString("61 D0 42 40"); // m_tVirtualEnableDate

            pLogin.Write(0); // m_cUserBenfitType
            
            //pLogin.WriteHexString("30 FF E9 7D 53 0B A0 0A 00 5A 0D 2A 00 00 00 00 00 74 39 5F 5A 0D 2A 3A");
            pLogin.WriteHexString("88 FF E9 7D 9C 02 5A 13 00 5A D0 1F 00 00 00 00 00 74 A2 5F 5A CE A1 80"); // from madness

            pLogin.WriteInt(0); // ?

            Character.WriteEnabledCharacters(pLogin);

            // ?
            pLogin.WriteHexString("00 00 00 07 00 00 00 68 00 16 5D 64 00 00 00 00 00 00 00 69 00 03 A1 EC 00 00 00 00 00 00 01 36 00 16 05 4E 00 00 00 01 00 00 01 37 00 12 C8 FC 00 00 00 01 00 00 01 38 00 13 AF A6 00 00 00 01 00 00 01 39 00 02 DF 6E 00 00 00 01 00 00 01 3A 00 08 5A D4 00 00 00 01");

            pLogin.WriteInt(10); // Number of character slots

            pLogin.WriteHexString("11 00 00");
            //pLogin.WriteHexString("00 00 00 07 00 00 00 72 00 17 F5 AC 00 00 00 00 00 00 00 73 00 10 8A 74 00 00 00 00 00 00 01 4F 00 05 B4 C8 00 00 00 01 00 00 01 50 00 18 2D B0 00 00 00 01 00 00 01 51 00 05 2E 2C 00 00 00 01 00 00 01 52 00 07 0C 88 00 00 00 01 00 00 01 53 00 04 F1 3C 00 00 00 01 00 00 00 04 FF 01 00");
            
            client.SendPacket(pLogin, true);
            
            Game.Send_ServerTime(this); // 01A0 (416d)
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

        [Opcode(GameOpcodes.EVENT_REGISTER_NICKNAME_REQ)]
        public void RegisterNickname(Client client, Packet packet)
        {
            var nickname = packet.ReadUnicodeString();

            var response = new Packet(GameOpcodes.EVENT_REGISTER_NICKNAME_ACK);

            using (var db = Database.Get())
            {
                var user = db.Query<GrandCheese.Util.Models.User>("SELECT * FROM users WHERE nickname ILIKE @nickname",
                    new { nickname }).FirstOrDefault();

                if (user != null)
                {
                    // Already used
                    response.Put(
                        -9, // Failure: FF FF FF F7
                        nickname.ToWideString()
                    );
                }
                else
                {
                    db.Query("UPDATE users SET nickname = @nickname WHERE username = @username",
                        new { username, nickname });

                    response.Put(
                        0, // Success
                        nickname.ToWideString()
                    );

                    nick = nickname;
                }
            }

            client.SendPacket(response);
        }

        [Opcode(GameOpcodes.EVENT_CHAR_SELECT_JOIN_REQ)]
        public void OnCharacterJoinGame(Packet packet)
        {
            Log.Get().Info($"<{ServerMain.Info.Name}> {nick} is joining the world.");

            // in case
            Trace.Assert(nick != null, "nick was null", $"The nickname of user {userId} was null. Unable to send the character selection reply packet.");

            Game.Send_EVENT_NONE_INVEN_ITEM_LIST_NOT(this);
            Game.Send_EVENT_STRENGTH_MATERIAL_INFO(this);
            Game.Send_EVENT_MATCH_RANK_REWARD_NOT(this);
            Game.Send_EVENT_HERO_DUNGEON_INFO_NOT(this);
            Game.Send_EVENT_USER_CHANGE_WEAPON_NOT(this);
            Game.Send_EVENT_NEW_CHAR_CARD_INFO_NOT(this);
            Game.Send_EVENT_VIRTUAL_CASH_LIMIT_RATIO_NOT(this);
            Game.Send_EVENT_BAD_USER_INFO_NOT(this);
            Game.Send_EVENT_COLLECTION_MISSION_NOT(this);
            Game.Send_EVENT_HELL_TICKET_FREE_MODE_NOT(this);
            Game.Send_EVENT_CAPSULE_LIST_NOT(this);
            Game.Send_EVENT_MISSION_PACK_LIST_NOT(this);
            Game.Send_EVENT_RAINBOW_EVENT_NOT(this);
            Game.Send_EVENT_RKTORNADO_ITEM_LIST_NOT(this);
            Game.Send_EVENT_ITEM_COMPOSE_INFO_NOT(this);
            Game.Send_EVENT_ITEM_TRADE_LIST_NOT(this);
            Game.Send_EVENT_NPC_GIFTS_NOT(this);
            Game.Send_EVENT_ITEM_CHARPROMOTION_LEVEL_NOT(this);
            Game.Send_EVENT_GP_ATTRIBUTE_INIT_ITEM_LIST(this);
            Game.Send_EVENT_GP_ATTRIBUTE_RANDOM_ITEM_LIST(this);
            Game.Send_EVENT_ITEM_ATTRIBUTE_RANDOM_SELECT_LIST(this);
            Game.Send_EVENT_SINGLE_RANDOM_ATTRIBUTE_ITEM_LIST_NOT(this);
            Game.Send_EVENT_MAX_CHAR_SP_LEVEL_NOT(this);
            Game.Send_EVENT_GUILD_LEVEL_TABLE_NOT(this);
            Game.Send_EVENT_MISSION_GETTABLE_CONDITION_INFO_NOT(this);
            Game.Send_EVENT_EXP_POTION_LIST_ACK(this);
            Game.Send_EVENT_AGIT_STORE_CATALOG_ACK(this);
        }

        [Opcode(GameOpcodes.EVENT_CHANGE_CHARACTER_INFO_REQ)]
        public void ChangeCharacter(Packet packet)
        {
            int oldChar = packet.ReadByte();
            int newChar = packet.ReadByte();

            Log.Get().Info($"<{ServerMain.Info.Name}> {nick} has changed characters ({oldChar} -> {newChar}).");

            Packet pr;
            pr = new Packet(GameOpcodes.EVENT_FINISHED_MISSION_LIST_NOT);
            pr.WriteInt(0);
            userClient.Client.SendPacket(pr, false);

            pr = new Packet(GameOpcodes.EVENT_WORLDBOSS_CONTRIBUTION_POINT_UPDATE_ACK);
            pr.WriteInt(-100);
            pr.WriteInt(0);
            userClient.Client.SendPacket(pr, false);

            pr = new Packet(GameOpcodes.EVENT_GRADUATE_CHARACTER_USER_INFO_NOT);
            pr.WriteInt(0);
            pr.WriteInt(0);
            pr.WriteInt(0);
            pr.WriteInt(0);
            pr.WriteInt(0);
            pr.WriteInt(0);
            userClient.Client.SendPacket(pr, false);

            pr = new Packet(GameOpcodes.EVENT_WEEKLY_REWARD_LIST_NOT);
            pr.WriteHexString("00 00 00 03 00 00 ab d6 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ab e0 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ab ea 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            userClient.Client.SendPacket(pr, true);

            pr = new Packet(GameOpcodes.EVENT_MONTHLY_REWARD_LIST_NOT);
            pr.WriteHexString("00 00 00 09 00 02 03 82 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 03 aa 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 03 d2 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 03 fa 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 04 22 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 04 4a 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 04 72 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 04 90 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 e5 04 00 00 00 00 00 00 00 00 ff ff ff ff ff ff ff ff 00 00 00 00 00 00 00 00 00 0f 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 4c 20 e4 00 00 00 00 01 d4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ");
            userClient.Client.SendPacket(pr, true);

            pr = new Packet(GameOpcodes.EVENT_DUNGEON_PERSONAL_RECORD_INFO_NOT);
            pr.WriteHexString("00 00 00 00 02 00 00 00 00");
            userClient.Client.SendPacket(pr, false);

            pr = new Packet(GameOpcodes.EVENT_CHANGE_CHARACTER_INFO_ACK);
            pr.WriteInt(0);
            pr.Write((byte)newChar);

            /*pr.WriteInt(items.size());
            foreach(var item in items)
            {

            }*/
        }
    }
}
