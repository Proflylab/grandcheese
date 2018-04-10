using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Util.Models
{
    //[Table("users")]
    public class User
    {
        //[Key]
        public int Id { get; set; }
        
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }
        
        public byte Gender { get; set; }
        
        //[Column("gp")]
        public int GP { get; set; }
    }
}
