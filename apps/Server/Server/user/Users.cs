using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Server
{
    /// <summary>
    /// class Users; helps to manipulate with users list
    /// </summary>
    class Users
    {
        /// <summary>
        /// <param name="users">List of users</param>
        /// <param name="path">file path from config where will be save list of users</param>
        /// </summary>
        private List<User> users = new List<User>();
        private string path = Stat.GetConf("usersFilePath");

        /// <summary>
        /// empty class constructor initialize for json serialize
        /// </summary>
        public Users() { }

        /// <summary>
        /// Getting data from a file and create the file if it doen't exits
        /// </summary>
        public void Initialize()
        {
            if (!File.Exists(path))
            {
                using StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create));
            }

            GetFromFile(path);
        }

        /// <summary>
        /// Adding user to the list or update user if it already exists
        /// </summary>
        /// <param name="u">user which will be saved</param>
        public void Add(User u) 
        {
            int index = users.FindIndex(x => x.Compare(u));

            if (index >= 0) users[index] = u;

            else users.Add(u);
        }

        /// <summary>
        /// Update received message time for user
        /// </summary>
        /// <param name="u">user which get message</param>
        public void UpdateTime(User u)
        {
            u.LastSeen = DateTime.Now;
            users.Add(u);
        }

        /// <summary>
        /// Method return all users in list
        /// </summary>
        /// <returns>return all users</returns>
        public List<User> AllUsers()
        {
            return users;
        }

        /// <summary>
        /// Method return user from list ny index
        /// </summary>
        /// <param name="index">users index</param>
        /// <returns>user</returns>
        public User GetUser(int index)
        {
            return users[index];
        }

        /// <summary>
        /// Method return user from list by login and password
        /// </summary>
        /// <param name="login">users login</param>
        /// <param name="password">users password</param>
        /// <returns>user</returns>
        public User GetUser(string login, string password)
        {
            User current_user = new User(login, password);
            User result = new User();
            foreach (User u in users)
            {
                if (u.Compare(current_user))
                {
                    result = u;
                }
            }

            return result;
        }

        /// <summary>
        /// Check if user in list
        /// </summary>
        /// <param name="u">user</param>
        /// <returns>bool true - if user in list, false - if list doen't contains this user</returns>
        public bool Exist(User u)
        {
            return users.Contains(u);
        }

        /// <summary>
        /// Method save list of user into the file (file path possible to change in config)
        /// </summary>
        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(users);

            if (!File.Exists(path))
            {
                using StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create));
            }

            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// Getting json object from file and deserialize to the list
        /// </summary>
        /// <param name="filename">file name</param>

        public void GetFromFile(string filename)
        {
            string jsonstring = File.ReadAllText(filename);

            if (!string.IsNullOrEmpty(jsonstring))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonstring);
            }
            else
            {
                users = new List<User>();
            }
        }

        /// <summary>
        /// ToString() method
        /// </summary>
        /// <returns>all users login in list</returns>

        public override string ToString()
        {
            string result = string.Empty;

            foreach (User x in users) {
                result += x.ToString() + "\n";
            }

            return result;
        }
    }
}
