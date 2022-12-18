using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Cipher;
using System.Security.Cryptography;

namespace Server
{
    class Client
    {
        public TcpClient client { get; set; }
        public User user { get; set; }
        private StreamWriter sw;
        private StreamReader sr;
        private string crypt, decrypt;

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
            crypt = Cipher.Cipher.Encrypt(text);
            sw.WriteLine(crypt);
            sw.Flush();
        }

        public void PrintInLine(string text)
        {
            string crypt = Cipher.Cipher.Encrypt(text);
            sw.WriteLine(crypt);
            sw.Flush();
        }

        public void Print(Message m)
        {
            crypt = Cipher.Cipher.Encrypt(m.ToString());
            sw.WriteLine(crypt);
            sw.Flush();
        }

        public void Print(List<Message> messages)
        {
            foreach (Message m in messages)
            {
                crypt = Cipher.Cipher.Encrypt(m.ToString());
                sw.WriteLine(crypt);
                sw.Flush();
            }
        }

        public string Read()
        {
            decrypt = Cipher.Cipher.Decrypt(sr.ReadLine());
            return decrypt; 
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
