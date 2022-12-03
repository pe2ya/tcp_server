using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Message
    {
        public string Text { get; set; }
        public User user { get; set; }
        public DateTime Time { get; set; }

        public Message() { }

        public Message(string text, User u) {
            Text = text;
            user = u;
            Time = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{user.Login} >> {Text}";
        }
    }
}
