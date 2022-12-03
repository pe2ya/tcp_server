using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Client
    {
        public TcpClient client { get; set; }
        public User user { get; set; }
        private StreamWriter sw;
        private StreamReader sr;

        public Client(TcpClient cl)
        {
            client = cl;
            sw = new StreamWriter(cl.GetStream(), Encoding.UTF8);
            sr = new StreamReader(cl.GetStream(), Encoding.UTF8);
        }

        public Client(TcpClient cl, User u)
        {
            client = cl;
            user = u;
            sw = new StreamWriter(cl.GetStream(), Encoding.UTF8);
            sr = new StreamReader(cl.GetStream(), Encoding.UTF8);
        }

        public void Print(string text)
        {
            sw.WriteLine(text);
            sw.Flush();
        }

        public void PrintInLine(string text)
        {
            sw.Write(text);
            sw.Flush();
        }

        public void Print(Message m)
        {
            sw.WriteLine(m);
            sw.Flush();
        }

        public void Print(List<Message> messages)
        {
            foreach (Message m in messages)
            {
                sw.WriteLine(m);
                sw.Flush();
            }
        }

        public string Read()
        {
            return sr.ReadLine();
        }

        public void Close()
        {
            sr.Dispose();
            sr.Close();
            sw.Close();
            sw.Dispose();

            client.Close();
        }

    }
}
