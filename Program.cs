using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server sw = new Server(65525);
        }
    }
}
