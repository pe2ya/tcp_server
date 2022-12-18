using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace Server
{
    class Messages
    {   

        private List<Message> messages = new List<Message>();
        private string path = Stat.GetConf("messagesFilePath");

        public Messages() { }

        public void Initialize()
        {
            if (!File.Exists(path))
            {
                using StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create));
            }

            GetFromFile(path);
        }

        public void Add(Message m)
        {
            messages.Add(m);
        }

        public void Add(string text, User u)
        {
            messages.Add(new Message(text, u));
        }

        public List<Message> AllMessages()
        {
            return messages;
        }

        public List<Message> GetMessagesForUser(User u)
        {
            List<Message> result = messages.Where((x) => {
                return DateTime.Compare(x.Time, u.LastSeen) > 0 && x.Sender.Login != u.Login;
            }).ToList();

            return result;
        }

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(messages);

            if (!File.Exists(path))
            {
                using StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create));
            }

            File.WriteAllText(path, jsonString);
        }

        public void GetFromFile(string filename)
        {
            string jsonstring = File.ReadAllText(filename);

            if (!string.IsNullOrEmpty(jsonstring))
            {
                messages = JsonSerializer.Deserialize<List<Message>>(jsonstring);
            }
            else
            {
                messages = new List<Message>();
            }
        }
    }
}
