using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;

namespace Server
{
    static class Stat
    {
        public static void WriteToFile(this User u, string filename)
        {
            List <User> users = GetFromFileUsers(filename);

            string jsonString = JsonSerializer.Serialize(u);

            if (!File.Exists(filename)) {
                using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Create))) { }
            }

            using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Append)))
            {
                sw.Write(jsonString + "\n" );
            }
        }

        public static List<User> GetFromFileUsers(string filename)
        {
            List<User> result = new List<User>();

            using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        User u = JsonSerializer.Deserialize<User>(line);
                        result.Add(u);
                    }
                }
            }

            return result;
        }

        public static void WriteToFile(this Message m, string filename)
        {
            string jsonString = JsonSerializer.Serialize(m);

            if (!File.Exists(filename))
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Create))) { }
            }

            using (StreamWriter sw = new StreamWriter(new FileStream(filename, FileMode.Append)))
            {
                sw.Write(jsonString + "\n");
            }
        }

        public static List<Message> GetFromFileMessage(string filename)
        {
            List<Message> result = new List<Message>();

            using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        Message m = JsonSerializer.Deserialize<Message>(line);
                        result.Add(m);
                    }
                }
            }

            return result;
        }

        public static User GetByLogin(string s)
        {
            return new User(s, s, s);
        }

    }
}
