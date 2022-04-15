using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer001.Model
{
    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User(int Id,string Username,string Password)
        {
            this.Id = Id;
            this.Username = Username;
            this.Password = Password;
        }
    }
}
