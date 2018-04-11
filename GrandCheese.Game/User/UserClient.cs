using GrandCheese.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game.User
{
    public class UserClient
    {
        public KUser KUser = null;
        public Client Client = null;

        public UserClient(Client c)
        {
            Client = c;
            KUser = new KUser(this);
        }
    }
}
