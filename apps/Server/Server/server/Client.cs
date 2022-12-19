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
    /// <summary>
    /// class Client; contains methods for operations with tcp client
    /// </summary>
    class Client
    {
        /// <summary>
        /// <param name="client">store connection</param>
        /// <param name="user">store user which loggined in</param>
        /// <param name="sw">object class StreamWriter; contains StreamWriter from connection</param>
        /// <param name="sr">object class StreamReader; contains StreamReader from connection</param>
        /// <param name="crypt">variable helps to store encrypted messages</param>
        /// <param name="decrypt">valiable helps to store decryped messages</param>
        /// </summary>
        public TcpClient client { get; set; }
        public User user { get; set; }
        private StreamWriter sw;
        private StreamReader sr;
        private string crypt, decrypt;

        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="cl">TcpClient connection</param>
        public Client(TcpClient cl)
        {
            client = cl;
            sw = new StreamWriter(cl.GetStream(), Encoding.UTF8);
            sr = new StreamReader(cl.GetStream(), Encoding.UTF8);
        }

        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="cl">TcpClient connection</param>
        /// <param name="u">user which loggined in</param>
        public Client(TcpClient cl, User u)
        {
            client = cl;
            user = u;
            sw = new StreamWriter(cl.GetStream(), Encoding.UTF8);
            sr = new StreamReader(cl.GetStream(), Encoding.UTF8);
        }


        /// <summary>
        /// Encrypt messages, send it to the server and flush
        /// </summary>
        /// <param name="text">data which will be send to the server</param>
        public void Print(string text)
        {
            crypt = Cipher.Cipher.Encrypt(text);
            sw.WriteLine(crypt);
            sw.Flush();
        }

        /// <summary>
        /// Encrypt messages, send it in line to the server and flush
        /// </summary>
        /// <param name="text">data which will be send to the server</param>
        public void PrintInLine(string text)
        {
            string crypt = Cipher.Cipher.Encrypt(text);
            sw.WriteLine(crypt);
            sw.Flush();
        }

        /// <summary>
        /// Encrypt message object, send it to the server and flush
        /// </summary>
        /// <param name="m">message which will be send to the server</param>
        public void Print(Message m)
        {
            crypt = Cipher.Cipher.Encrypt(m.ToString());
            sw.WriteLine(crypt);
            sw.Flush();
        }


        /// <summary>
        /// Encrypt messages list, send it to the server and flush
        /// </summary>
        /// <param name="text">messages list which will be send to the server</param>
        public void Print(List<Message> messages)
        {
            foreach (Message m in messages)
            {
                crypt = Cipher.Cipher.Encrypt(m.ToString());
                sw.WriteLine(crypt);
                sw.Flush();
            }
        }


        /// <summary>
        /// Read data from server and decrypt it
        /// </summary>
        /// <returns>decrypted text or if connection lost return null</returns>
        public string Read()
        {
            string result = sr.ReadLine();

            if (!string.IsNullOrEmpty(result))
            {
                result = Cipher.Cipher.Decrypt(result);
            }

            return result;
        }


        /// <summary>
        /// close connection, StreamWriter and StreamReader
        /// </summary>
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
