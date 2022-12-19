using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cipher;

namespace clsClient
{

    /// <summary>
    /// class Buffer; used for operation with unsend messages
    /// </summary>
    public class Buffer
    {
        /// <summary>
        /// <param name="buffer">list of string</param>
        /// </summary>
        private List<string> buffer = new List<string>();


        /// <summary>
        /// empty class constructor initialize
        /// </summary>
        public Buffer() { }


        /// <summary>
        /// Adding msg to list
        /// </summary>
        /// <param name="msg">input string saved to list</param>
        public void Add(string msg)
        {
            buffer.Add(msg);
        }


        /// <summary>
        /// Clear all msg in list
        /// </summary>
        public void Clear()
        {
            buffer.Clear();
        }

        /// <summary>
        /// Method for find out length of list
        /// </summary>
        /// <returns>list length</returns>
        public int Length()
        { 
            return buffer.Count;
        }

        /// <summary>
        /// Send all messages from list to the server and after that clear the list
        /// </summary>
        /// <param name="cl">tcp client</param>
        public void Send(Client cl)
        {
            foreach (string msg in buffer)
            {
                cl.Print(msg);
            }

            Clear();
        }

        /// <summary>
        /// get all messages from list and split it every 100 char
        /// </summary>
        /// <returns>string of messages</returns>

        public string ShowMessages()
        {
            string text = string.Empty,
                   result = string.Empty;

            foreach (string msg in buffer)
            {
                result += $"{msg}; ";
            }

            for (int i = 0; i < result.Length; i += 100) 
            {
                result += text.Substring(i, Math.Min(100, text.Length - i)) + "\n";
            }

            return result;
        }
    }
}
