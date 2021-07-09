using System;
using System.Collections.Generic;
using System.Text;

namespace Project_Dvd.Models
{
    class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}
