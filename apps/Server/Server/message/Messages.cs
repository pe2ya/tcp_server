using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace Server
{
    /// <summary>
    /// class Messages; helps to manipulate with messages list
    /// </summary>
    class Messages
    {
        /// <summary>
        /// <param name="messages">List of messages</param>
        /// <param name="path">file path from config where will be save list of messages</param>
        /// </summary>
        private List<Message> messages = new List<Message>();
        private string path = Stat.GetConf("messagesFilePath");

        /// <summary>
        /// empty class constructor initialize for json serialize
        /// </summary>
        public Messages() { }


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
        /// Adding messages to the list
        /// </summary>
        /// <param name="m">message obj</param>
        public void Add(Message m)
        {
            messages.Add(m);
        }


        /// <summary>
        /// Adding messages to the list
        /// </summary>
        /// <param name="text">string message</param>
        /// <param name="u">user which send the messages</param>
        public void Add(string text, User u)
        {
            messages.Add(new Message(text, u));
        }

        /// <summary>
        /// Method which return list of messages
        /// </summary>
        /// <returns>list of messages</returns>
        public List<Message> AllMessages()
        {
            return messages;
        }

        /// <summary>
        /// Method return all messages which user doen't receive yet
        /// </summary>
        /// <param name="u">user</param>
        /// <returns>list of messages</returns>
        public List<Message> GetMessagesForUser(User u)
        {
            List<Message> result = messages.Where((x) => {
                return DateTime.Compare(x.Time, u.LastSeen) > 0 && x.Sender.Login != u.Login;
            }).ToList();

            return result;
        }


        /// <summary>
        /// Method save list of messages into the file(file path possible to change in config)
        /// </summary>
        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(messages);

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
                messages = JsonSerializer.Deserialize<List<Message>>(jsonstring);
            }
            else
            {
                messages = new List<Message>();
            }
        }
    }
}
