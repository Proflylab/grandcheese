using Dapper;
using GrandCheese.Game.Inventory;
using GrandCheese.Util;
using GrandCheese.Util.Extensions;
using GrandCheese.Util.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.User
{
    public class Character : ISerializable
    {
        public int Id { get; set; }
        public KUser KUser { get; set; } = null;
        public int UserId { get; set; } = -1;
        public int CharacterType { get; set; } = 0;
        public int Promotion { get; set; } = 0;
        public int CurrentPromotion { get; set; } = 0;
        public long Exp { get; set; } = 0;
        public int Win { get; set; } = 0;
        public int Lose { get; set; } = 0;
        public int Level { get; set; } = 0;
        public int WeaponId { get; set; } = 0;
        public bool UseWeapon { get; set; } = false;
        public int SlotNumber { get; set; } = 0;
        public int GamePoints { get; set; } = 3000;
        public int BonusPoints { get; set; } = 3;
        public int InventoryCapacity { get; set; } = 120;
        public int LookInventoryCapacity { get; set; } = 120;

        public List<int> Promotions { get; set; } = new List<int>();
        public List<KItem> Items { get; set; } = new List<KItem>();
        public List<KEquipItemInfo> EquipItems { get; set; } = new List<KEquipItemInfo>();

        public void Serialize(Packet packet, int i, params object[] optional)
        {
            Log.Get().Debug("[Serializing] Character Type: {0} with index {1}", CharacterType, i);

            packet.Put(
                (byte)i, // Index
                (byte)CharacterType, // ?
                (byte)Promotion,
                (byte)CurrentPromotion,
                0, // 1 //KUser.nick?.ToWideString(),
                Exp,
                Win,
                Lose,
                Win,
                Lose,
                Exp,
                Level,
                EquipItems,

                // TODO
                160, // 10 // SkillPoint
                160, // 0 // 100, // MaxSkillTreePoint

                1, // SkillTreePoint
                0, // MaxSkillTreePoint

                (byte)0, // ?

                (long)100, // m_biInitTotalExp
                (long)100, // m_biTotalExp
                
                new KEquipItemInfo(), // for changeWeapon, TODO
                UseWeapon,

                // TODO
                // m_setPromotion
                Promotions,

                // TODO
                // kELOUserData
                new KELOUserData()
                {
                    ELOWin = Win,
                    ELOLose = Lose
                },

                i // Character Slot Position //6 as int
            );

            packet.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 07 D0 00 00 07 D0 00 00 00 0A 00 00 00 00 00 00 00 5A 00 00 00 64 00 00 00 00 00 00 00 00");

            // User ID?
            packet.WriteHexString("00 71 30 29");
        }

        public void Insert(IDbConnection db)
        {
            var query = "INSERT INTO \"characters\" (user_id, character_type) " +
                                    "VALUES (@UserId, @CharacterType) " +
                                    "RETURNING id;";

            var id = db.ExecuteScalar(query, new
            {
                UserId,
                CharacterType
            });

            Id = Convert.ToInt32(id);
        }

        // Static functions

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
        public static void CreateNewCharacter(KUser kUser, Packet packet)
        {
            var characterType = (int)packet.ReadByte();
            Console.WriteLine("Character ID: {0}", characterType);

            var character = new Character()
            {
                UserId = kUser.userId,
                CharacterType = characterType
            };

            using (var db = Database.Get())
            {
                character.Insert(db);
            }

            kUser.characters.Add(character.Id, character);
            kUser.currentCharacterId = character.Id;

            var p = new Packet(GameOpcodes.EVENT_NEW_CHAR_CHOICE_ACK, kUser);

            p.Write(0x00); // m_ucOK
            p.Put(characterType);
            p.WriteHexString("64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 01");
            
            Inventory.Inventory.WriteDefaultEquipItemInfo(p, characterType, kUser);

            p.WriteHexString("00 00 00 02 00 00 00 A0 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 64 00 00 00 00 00 00 00 64 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 07 D0 00 00 07 D0 00 00 00 0A 00 00 00 00 00 00 00 5A 00 00 00 64 00 00 00 00 00 00 00 00 FF EA 7D A8 00 00 00 55");
            
            DungeonUserInfo.WriteMapDifficulty(p); // lol
            
            Inventory.Inventory.GiveDefaultItems(p, characterType, kUser);

            p.WriteHexString("00 00 00 00 00 00 00 02 00 00 00 14 00 00 00");

            WriteEnabledCharacters(p);
            
            p.Write(255);

            p.WriteHexString("00 00 00 01 00 00 E5 6A 00 00 00 00 02 06 CC BA 00 00 00 14 00 00 00 14 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF 01 A1 AB 5D 08 D7 50 D0 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 E5 6A 00 00 00 00 02 06 CC BA 06 00 00 00 00 00 00 00");

            //Log.Get().Trace(Util.Util.ConvertBytesToHexString(p.packet.ToArray()));

            kUser.userClient.Client.SendPacket(p);
        }
    }
}
