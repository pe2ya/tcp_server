using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;


namespace Server
{
    class Server
    {
        private TcpListener myServer;
        private bool isRurung;
        private List<Client> clients = new List<Client>();
        private Users users = new Users();
        private Messages messages = new Messages();
        private string ip = Stat.GetConf("ServerIP");


        public Server(int port)
        {
            try
            {
                //myServer = new TcpListener(localaddr: System.Net.IPAddress.Any, port: port);
                myServer = new TcpListener(localaddr: System.Net.IPAddress.Parse(ip), port: port);
                isRurung = true;
                myServer.Start();

                messages.Initialize();
                users.Initialize();

                LoopServer();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                users.Save();
                messages.Save();
            }
        }

        public void LoopServer()
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

        public void ClientLoop(object obj)
        {
            try
            {
                Client cl = new Client((TcpClient)obj);
                Console.WriteLine("Client connected to the server");
                Console.WriteLine(((IPEndPoint)cl.client.Client.RemoteEndPoint).Address);



                bool clientRunning = true;
                bool loggined = false;
                int attempts = 3;
                string resived = string.Empty,
                       password = string.Empty,
                       login = string.Empty,
                       name = string.Empty,
                       msg = string.Empty;;

                while (!loggined)
                {

                    if (attempts == 0)
                    {
                        cl.Print("You don't have any attempts");
                    }

                    cl.Print("Type: 1 - for login; 2 - for sing up");

                    msg = cl.Read();

                    switch (msg)
                    {
                        case "1":
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
                        case "2":
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
            
                Broadcast($"+++ {cl.user.Login} arrived +++");

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
                        users.Save();
                        clientRunning = false;
                        break;
                    }

                    if (!cl.client.Connected)
                    {
                        break;
                    }

                    Message m = new Message(msg, cl.user);
                    messages.Add(m);
                    messages.Save();
                    Console.WriteLine(m);

                    cl.PrintInLine(">>");
                }


                users.UpdateTime(cl.user);
                users.Save();
                cl.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Broadcast(string text)
        {
            Console.WriteLine(text);
            foreach (Client cl in clients)
            {
                cl.Print(text);
            } 
        }

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
