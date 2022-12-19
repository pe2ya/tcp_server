using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;


namespace Server
{
    /// <summary>
    ///  class Server; all program commmunicates with different parts of code
    /// </summary>
    class Server
    {

        /// <summary>
        /// <param name="myServer">tcl listener which accept new clients</param>
        /// <param name="isRunning">used for while loop</param>
        /// <param name="clients">list of clients; store clients which connected to the server</param>
        /// <param name="users">object class Users; store users which connected to the server</param>
        /// <param name="messages">object class Messages;used for store messages which was sent to the server</param>
        /// <param name="ip">tcp server ip</param>
        /// <param name="port">tcp server port</param>
        /// </summary>
        private TcpListener myServer;
        private bool isRurung;
        private List<Client> clients = new List<Client>();
        private Users users = new Users();
        private Messages messages = new Messages();
        private string ip = Stat.GetConf("serverIP");
        private int port = Convert.ToInt32(Stat.GetConf("port"));


        /// <summary>
        /// class constructor
        /// create server and start it
        /// also initialize Messages and Users objects
        /// </summary>
        public Server()
        {
            myServer = new TcpListener(localaddr: System.Net.IPAddress.Parse(ip), port: port);
            isRurung = true;
            myServer.Start();

            messages.Initialize();
            users.Initialize();

            LoopServer();
        }

        /// <summary>
        /// Method which start the program
        /// waiting for connect new users to the server
        /// start new thread for new client
        /// </summary>
        public void LoopServer()
        {
            try
            { 
                Console.WriteLine("Server is running");

                while (isRurung)
                {
                    TcpClient client = myServer.AcceptTcpClient();
                    Thread thread = new Thread(new ParameterizedThreadStart(ClientLoop));
                    thread.Start(client);


                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        isRurung = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                users.Save();
                messages.Save();
                myServer.Stop();
            }
        }

        /// <summary>
        /// Start loop for new client 
        /// receive data from client and and data to him from another clients
        /// also recognise new client 
        /// </summary>
        /// <param name="obj">tcpclient which connected to the server</param>
        public void ClientLoop(object obj)
        {
            Client cl = new Client((TcpClient)obj);
            try
            {
                Console.WriteLine("Client connected to the server");
                Console.WriteLine(((IPEndPoint)cl.client.Client.RemoteEndPoint).Address);



                bool clientRunning = true;
                bool loggined = false;
                int attempts = 3;
                string resived = string.Empty,
                       password = string.Empty,
                       login = string.Empty,
                       name = string.Empty,
                       msg = string.Empty; ;

                while (!loggined)
                {

                    if (attempts == 0)
                    {
                        cl.Print("You don't have any attempts");
                    }

                    cl.Print("Type: log - for login; sup - for sing up");

                    msg = cl.Read();
                    Console.WriteLine(msg);

                    switch (msg)
                    {
                        case "log":
                            while (attempts > 0)
                            {
                                cl.Print("Enter login: ");

                                login = cl.Read();

                                cl.Print("Enter password: ");

                                password = cl.Read();

                                cl.user = users.GetUser(login, password);

                                attempts--;

                                if (users.Exist(cl.user))
                                {
                                    loggined = true;
                                    clients.Add(cl);
                                    break;
                                }
                            }
                            break;
                        case "sup":
                            cl.Print("Enter name: ");

                            name = cl.Read();

                            cl.Print("Enter login: ");

                            login = cl.Read();

                            cl.Print("Enter password: ");

                            password = cl.Read();

                            cl.user = new User(name, login, password);
                            users.Add(cl.user);

                            loggined = true;
                            clients.Add(cl);
                            users.Save();

                            break;
                        default:
                            cl.Print("Incorrect input");
                            break;
                    }

                }

                Broadcast($"--- {cl.user.Login} arrived ---");

                cl.Print("Type /quit for disconnect");

                cl.PrintInLine(">>");

                while (clientRunning)
                {

                    Broadcast();
                    msg = cl.Read();
                    cl.user.LastSeen = DateTime.Now;

                    if (msg == "/quit")
                    {
                        Broadcast($"+++ {cl.user} go for a walk +++");
                        clients.Remove(cl);
                        clientRunning = false;
                        break;
                    }

                    if (!cl.client.Connected)
                    {
                        break;
                    }

                    Message m = new Message(msg, cl.user);
                    messages.Add(m);
                    Console.WriteLine(m);

                    cl.PrintInLine(">>");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                users.UpdateTime(cl.user);
                users.Save();
                cl.Close();
            }
        }

        /// <summary>
        /// Broadcast send text to all clients connected to the server
        /// </summary>
        /// <param name="text"></param>
        public void Broadcast(string text)
        {
            Console.WriteLine(text);
            foreach (Client cl in clients)
            {
                cl.Print(text);
            } 
        }

        /// <summary>
        /// Broadcast send all messages for user except users messages
        /// and update time received messages for users which received messages
        /// </summary>
        public void Broadcast()
        {
            foreach (Client cl in clients)
            {
                cl.Print(messages.GetMessagesForUser(cl.user));
                cl.user.LastSeen = DateTime.Now;
                
            }
        }
    }
}
