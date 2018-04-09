using GrandCheese.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Packets
{
    /*
    class Reader
    {
        public static void Handle(Client c, Packet p)
        {
            c.Crypto.DecryptPacket(p);

            short opcode = p.ReadShort();
            int length = p.ReadInt();
            int isCompressed = p.ReadByte();
            if (isCompressed == 1) p.ReadInt();

            var handled = true;

            switch((LoginOpcodes)opcode)
            {
                case LoginOpcodes.HEART_BIT_NOT:
                    Login.HandleHeartbeat(c, p);
                    break;

                case LoginOpcodes.ENU_VERIFY_ACCOUNT_REQ:
                    Login.VerifyAccount(c, p);
                    break;

                case LoginOpcodes.ENU_CLIENT_CONTENTS_FIRST_INIT_INFO_REQ:
                    Login.SendClientContentsFirstInitInfo(c, p);
                    break;

                case LoginOpcodes.ENU_SHAFILENAME_LIST_REQ:
                    Login.SendShaFileList(c, p);
                    break;

                case LoginOpcodes.ENU_GUIDE_BOOK_LIST_REQ:
                    Login.SendGuideBookList(c, p);
                    break;

                case LoginOpcodes.ENU_CLIENT_PING_CONFIG_REQ:
                    Login.SendClientPingConfig(c, p);
                    break;

                default:
                    handled = false;
                    Log.Get().Warn("Unknown packet received. Opcode: {0} Length: {1}", opcode, length);
                    break;
            }

            if(handled)
            {
                Log.Get().Info("[Receive] {0} : {1} ({2})", c.Id, (LoginOpcodes)opcode, opcode);
            }
        }
    }
    */
}
