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

        public Messages() { }

        public Messages(List<Message> ms) {
            this.messages = ms;
        }

        public Messages(string filename) {
            GetFromFile(filename);
        }

        public void WriteToFile(string filename)
        {
            string jsonString = JsonSerializer.Serialize(messages);

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
                messages = JsonSerializer.Deserialize<List<Message>>(jsonstring);
            }
            else
            {
                messages = new List<Message>();
            }
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

        public List<Message> GetMessagesForUser(User u )
        {
            List<Message> result = messages.Where((x) => { 
                return DateTime.Compare(x.Time, u.Disconnect) > 0 && x.user.Login != u.Login;
            }).ToList();

            return result;
        }
    }
}
