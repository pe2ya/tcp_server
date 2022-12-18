using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace clsClient
{
    public class ClsClient
    {
        private int attempts = 0;
        private Client cl;
        private Buffer buffer = new Buffer();
        private bool isRunning = true;
        private string msg = string.Empty;

        Thread receiveData, restoreConnection;
        

        public object locker = new object();


        public ClsClient() 
        {
            cl = new Client();

            Start();
        }

        public void Start()
        {
            receiveData = new Thread(new ThreadStart(cl.ReceiveData));

            try
            {
                receiveData.Start();

                string s = string.Empty;

                while (isRunning)
                {
                    msg = Console.ReadLine().ToString();
                    Write(msg);

                    switch (msg)
                    {
                        case "/quit":
                            isRunning = false;
                            break;

                        case "/send":
                            SendToServer();
                            break;

                        case "/clear":
                            buffer.Clear();
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                receiveData.Join();
                cl.Close();
            }
        }

        public void Write(string msg)
        {

            if (cl.isConnected())
            {
                if (attempts > 0)
                {
                    receiveData = new Thread(new ThreadStart(cl.ReceiveData));
                    receiveData.Start();
                    if (buffer.Length() > 0)
                    {
                        string text = $"You have unsend messages\n\n{buffer.ShowMessages()}\n\n" +
                            $"type /send if you want to send it again or /clear to clear the buffer";

                        Console.WriteLine(text);
                    }
                }
                cl.Print(msg);
                attempts = 0;
            }
            else
            { 
                SaveToBuffer(msg);
                receiveData.Join();
                cl.Connect();
                attempts++;
            }
        }

        public void SaveToBuffer(string msg)
        {
            buffer.Add(msg);
        }

        public void SendToServer()
        {
            if (buffer.Length() > 0)
            {
                buffer.Send(cl);
            }
        }

        //public void ReceiveData(object cl)
        //{
        //    TcpClient tcpClient = (TcpClient)cl;

        //    StreamReader sr = new StreamReader(tcpClient.GetStream(), Encoding.UTF8);
        //    string line;

        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        Console.WriteLine(line);
        //    }
        //    sr.Close();

        //}
    }
}
