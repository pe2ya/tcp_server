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

    /// <summary>
    ///  class Client; contains methods for operations with tcp client
    /// </summary>
    public class Client
    {

        /// <summary>
        /// <param name="tcpClient">object class TcpClient; contains connection</param>
        /// <param name="sw">object class StreamWriter; contains StreamWriter from connection</param>
        /// <param name="sr">object class StreamReader; contains StreamReader from connection</param>
        /// <param name="ip">tcp server ip</param>
        /// <param name="port">tcp server port</param>
        /// <param name="crypt">variable helps to store encypted messages</param>
        /// </summary>
        public TcpClient tcpClient { get; set; }
        private StreamWriter sw;
        private StreamReader sr;

        private string ip = Stat.GetConf("serverIP");
        private int port = Convert.ToInt32(Stat.GetConf("port"));
        private string crypt = string.Empty;


        /// <summary>
        /// class constactor
        /// </summary>
        public Client()
        { 
            Connect();
        }

        /// <summary>
        /// Method which try to connect to the server
        /// set StreamWriter and StreamReader after successful connection
        /// handle exception if cannot connect to the server
        /// </summary>

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

        /// <summary>
        /// Method for check connection
        /// </summary>
        /// <returns>bool connection</returns>
        public bool isConnected()
        {
            return tcpClient.Connected;
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
        /// loop method which receive data from server as long as there is a connection to the server
        /// </summary>
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


        /// <summary>
        /// close connection, StreamWriter and StreamReader
        /// </summary>
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
