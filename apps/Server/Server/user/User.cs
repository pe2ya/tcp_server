using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// class User; helps to store data individual user
    /// </summary>
    class User
    {

        /// <summary>
        /// <param name="Name">store users name</param>
        /// <param name="Login">store users login</param>
        /// <param name="Password">store users password</param>
        /// <param name="LastSeen">store time when user receive last messages</param>
        /// </summary>
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// empty class constructor initialize for json serialize
        /// </summary>
        public User() { }


        /// <summary>
        /// class constuctor
        /// </summary>
        /// <param name="login">users login</param>
        /// <param name="password">users password</param>
        public User(string login, string password) {
            Login = login;
            Password = password;
            LastSeen = DateTime.Now;
        }

        /// <summary>
        /// class constuctor
        /// </summary>
        /// <param name="name">users name</param>
        /// <param name="login">users login</param>
        /// <param name="password">users password</param>
        public User(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
            LastSeen = DateTime.Now;
        }


        /// <summary>
        /// compare users based on users login and users password
        /// </summary>
        /// <param name="other">users to compare</param>
        /// <returns>bool true - if users same and false - if users different</returns>
        public bool Compare(User other)
        {
            if (Login == other.Login && Password == other.Password)
                return true;

            return false;
        }

        /// <summary>
        /// ToString() method
        /// </summary>
        /// <returns>return users login</returns>
        public override string ToString()
        {
            return Login;
        }
    }
}
