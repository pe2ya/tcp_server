using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class User
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime LastSeen { get; set; }

        public User() { }

        public User(string login, string password) {
            Login = login;
            Password = password;
            LastSeen = DateTime.Now;
        }

        public User(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
            LastSeen = DateTime.Now;
        }

        public bool Compare(User other)
        {
            if (Login == other.Login && Password == other.Password)
                return true;

            return false;
        }

        public override string ToString()
        {
            return Login;
        }
    }
}
