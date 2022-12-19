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
    /// <summary>
    ///  class clsClient; all program commmunicates with different parts of code
    /// </summary>
    public class ClsClient
    {
        /// <summary>
        /// <param name="attempts">amount of attempts to restore connection with tcp server</param>
        /// <param name="cl">object class Client; used for operations with tcp client</param>
        /// <param name="buffer">object class buffer; store messages which cannot send to the server</param>
        /// <param name="isRunning">used for while loop</param>
        /// <param name="msg">used for store data from console</param>
        /// <param name="receiveData">Parallel thread used for receive data from server</param>
        /// </summary>
        private int attempts = 0;
        private Client cl;
        private Buffer buffer = new Buffer();
        private bool isRunning = true;
        private string msg = string.Empty;

        Thread receiveData;


        /// <summary>
        /// class constactor
        /// </summary>
        public ClsClient() 
        {
            cl = new Client();

            Start();
        }

        /// <summary>
        /// Method which start the program
        /// start parallel thread for receive data
        /// waiting for console input and call Write method
        /// </summary>

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

        /// <summary>
        /// Method check connection with server
        /// if there is a connection with server send msg to the server 
        /// and if attempts more then zero method will start again parallel thread which will receive data from server and show msg in buffer
        /// if there is no connection with server method will save msg to buffer and try to restore connection
        /// 
        /// </summary>
        /// <param name="msg">console input(string)</param>

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

        /// <summary>
        ///  Method save msg to buffer
        /// </summary>
        /// <param name="msg">input from console(string)</param>
        /// 
        public void SaveToBuffer(string msg)
        {
            buffer.Add(msg);
        }

        /// <summary>
        /// Method send messages to server from buffer if there is some data
        /// </summary>
        public void SendToServer()
        {
            if (buffer.Length() > 0)
            {
                buffer.Send(cl);
            }
        }
    }
}
