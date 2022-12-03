using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Server
{
    class Users
    {
        private List<User> users = new List<User>();

        public Users() { }

        public Users(List<User> us)
        {
            users = us;
        }

        public Users(string filename) 
        {
            GetFromFile(filename);
        }

        public void Add(User u) 
        {
            int index = users.FindIndex(x => x.Compare(u));

            if (index >= 0) users[index] = u;

            else users.Add(u);
        }

        public void UpdateTime(User u)
        {
            u.Disconnect = DateTime.Now;
            users.Add(u);
        }

        public List<User> AllUsers()
        {
            return users;
        }

        public User GetUser(int index)
        {
            return users[index];
        }

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

        public bool Exist(User u)
        {
            return users.Contains(u);
        }

        public void WriteToFile(string filename)
        {
            string jsonString = JsonSerializer.Serialize(users);

            if (!File.Exists(filename))
            {
                using StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Create));
            }

            File.WriteAllText(filename, jsonString);
        }

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
