using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util.Models
{
    //[Table("servers")]
    public class Server
    {
        //[Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        //[Column("online_users")]
        public int OnlineUsers { get; set; }

        //[Column("max_users")]
        public int MaxUsers { get; set; }

        public int ProtocolVersion { get; set; }

        public bool Active { get; set; }
    }
}
