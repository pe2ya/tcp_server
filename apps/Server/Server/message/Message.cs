using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// class Message; helps to store individual msg data
    /// </summary>
    class Message
    {
        /// <summary>
        /// <param name="Text">store message text</param>
        /// <param name="Sender">store user which send a message</param>
        /// <param name="Time">store time when message was received</param>
        /// </summary>
        public string Text { get; set; }
        public User Sender { get; set; }
        public DateTime Time { get; set; }

        /// <summary>
        /// empty class constructor initialize for json serialize
        /// </summary>
        public Message() { }

        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="text">users message</param>
        /// <param name="u"><user which send a message</param>
        public Message(string text, User u) {
            Text = text;
            Sender = u;
            Time = DateTime.Now;
        }

        /// <summary>
        /// ToString() method
        /// </summary>
        /// <returns>user login and his message</returns>
        public override string ToString()
        {
            return $"{Sender.Login} >> {Text}";
        }
    }
}
