using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandCheese.Game.User;
using GrandCheese.Util;
using MoonSharp.Interpreter;

namespace GrandCheese.Game.Inventory
{
    public class Inventory
    {
        public static void GiveDefaultItems(Packet p, int charType, KUser kUser)
        {
            //p.Put(6); // items
            var items = new List<KItem>();

            foreach (var charItem in Lua.GetLuaGlobal("CharDefaultEquipItemInfo").Table.Values)
            {
                var obj = charItem.Table;

                if (Convert.ToInt32(obj["Char"]) == charType)
                {
                    var defaultItems = (Table)obj["DefaultItem"];

                    // Lua starts with a 1 index...
                    // retards
                    var j = 0;
                    for (var i = 1; i < defaultItems.Length + 1; i += 3)
                    {
                        var itemId = Convert.ToUInt32(defaultItems[i]);
                        var duration = Convert.ToInt32(defaultItems[i + 1]);
                        var period = Convert.ToInt32(defaultItems[i + 2]);

                        Console.WriteLine($"{itemId} / {duration} / {period}");

                        items.Add(new KItem()
                        {
                            ItemId = itemId,
                            Id = 10000000 + j,
                            GradeId = (char)0x02,

                            UserId = kUser.userId,
                            CharacterId = kUser.GetCurrentCharacter().Id,
                            
                            Sockets = new List<KSocketInfo>()
                            {
                                new KSocketInfo()
                                {
                                    SlotId = 0
                                },
                                new KSocketInfo()
                                {
                                    SlotId = 1
                                }
                            },

                            Attributes = new List<KAttributeInfo>()
                            {
                                new KAttributeInfo()
                                {
                                    SlotId = 0x00,
                                    Type = 0xFF,
                                    State = 0x01,
                                    Value = 0.0f
                                }
                            }
                        });

                        j++;
                    }

                    break;
                }
            }

            p.Put(items);
            Log.Get().Debug("Giving {0} KItem objects to client", items.Count);
        }

        public static void WriteSiegTestItems_(Packet p)
        {
            
        }

        public static void WriteDefaultEquipItemInfo(Packet p, int charType)
        {
            var equipItems = new List<KEquipItemInfo>();

            foreach (var charItem in Lua.GetLuaGlobal("CharDefaultEquipItemInfo").Table.Values)
            {
                var obj = charItem.Table;

                if (Convert.ToInt32(obj["Char"]) == charType)
                {
                    var defaultItems = (Table)obj["DefaultItem"];
                    
                    // Lua starts with a 1 index...
                    // retards

                    var j = 0;
                    for (var i = 1; i < defaultItems.Length + 1; i += 3)
                    {
                        var itemId = Convert.ToUInt32(defaultItems[i]);

                        Console.WriteLine($"[EI] {itemId}");

                        equipItems.Add(new KEquipItemInfo()
                        {
                            ItemId = itemId,
                            ItemUniqueId = 1000000 + j
                        });

                        j++;
                    }

                    break;
                }
            }

            p.Put(equipItems);
            Log.Get().Debug("Gave {0} KEquipItemInfo objects to the client.", equipItems.Count);
        }
    }
}
