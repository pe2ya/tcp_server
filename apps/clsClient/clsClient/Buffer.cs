using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cipher;

namespace clsClient
{
    public class Buffer
    {
        private List<string> buffer = new List<string>();

        public Buffer() { }

        public void Add(string msg)
        {
            buffer.Add(msg);
        }

        public void Clear()
        {
            buffer.Clear();
        }

        public int Length()
        { 
            return buffer.Count;
        }

        public void Send(Client cl)
        {
            foreach (string msg in buffer)
            {
                cl.Print(msg);
            }

            Clear();
        }

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
