using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace clsClient
{
    public class Client
    {
        public TcpClient tcpClient { get; set; }
        private StreamWriter sw;
        private StreamReader sr;

        private string ip = Stat.GetConf("serverIP");
        private int port = Convert.ToInt32(Stat.GetConf("port"));
        private string crypt = string.Empty, 
                       decrypt = string.Empty;



        public Client()
        { 
            Connect();
        }

        public void Connect()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(ip, port);

                if (tcpClient.Connected)
                {
                    sw = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8);
                    sr = new StreamReader(tcpClient.GetStream(), Encoding.UTF8);
                    Console.WriteLine($"Connected to server {ip}:{port}");
                }
            }
            catch (Exception) { }     
        }

        public bool isConnected()
        {
            return tcpClient.Connected;
        }

        public void Print(string text)
        {
            crypt = Cipher.Cipher.Encrypt(text);
            sw.WriteLine(crypt);
            sw.Flush();

        }

        public string Read()
        {
            string result = sr.ReadLine();

            if (!string.IsNullOrEmpty(result))
            {
                result = Cipher.Cipher.Decrypt(result);
            }

            return result;

        }

        public void ReceiveData()
        {
            string line;

            while (tcpClient.Connected)
            {
                try
                {

                    line = Read();

                    if (string.IsNullOrEmpty(line)) { break; }

                    Console.WriteLine(line);
                }
                catch (Exception) { break; }
            }
            sr.Close();

        }

        public void Close()
        {
            sr.Dispose();
            sr.Close();
            sw.Close();
            sw.Dispose();

            tcpClient.Close();
        }



    }
}
