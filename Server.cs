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
        private Users users = new Users("u.txt");
        private Messages messages = new Messages("m.txt");


        public Server(int port)
        {
            //myServer = new TcpListener(localaddr: System.Net.IPAddress.Parse("127.0.0.1"), port: port);
            myServer = new TcpListener(localaddr: System.Net.IPAddress.Any, port: port);
            isRurung = true;
            myServer.Start();

            try
            {
                LoopServer();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                users.WriteToFile("u.txt");
                messages.WriteToFile("m.txt");
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

                            loggined = true;
                            clients.Add(cl);

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
                    cl.user.Disconnect = DateTime.Now;

                    if (msg == "/quit")
                    {
                        Broadcast($"+++ {cl.user.Login} go for a walk +++");
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


                users.UpdateTime(cl.user);
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
            }
        }
    }
}
